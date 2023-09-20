using FileStreamLibrary;
using System;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommonLibrary
{
    public struct ScaraPosDef
    {
        public double[] Value;
        public ushort SpeedPercentage;

        public ScaraPosDef(bool flg)
        {

            Value = new double[4];
            Value[0] = 0;
            Value[1] = 0;
            Value[2] = 0;
            Value[3] = 0;

            SpeedPercentage = 5;
        }

        public int GetAxisNum()
        {
            return 4;
        }
    }

    public class ScaraGJSDef : IDisposable
    {
        public enum Axis
        {
            J1,
            J2,
            J3,
            J4,
            Count,
        }
        public enum CartesianCoordinate
        {
            X,
            Y,
            Z,
            Angle,
            Count
        }

        private const int _NumberOfAxis = (int)Axis.Count;
        private readonly AxisBaseDef[] _Axes;

        public double PowerPercent;
        private double _LinearMoveAccTime;
        private double _LinearMoveDecTime;
        private double[] _AccTime;
        private double[] _DecTime;
        private double[] _MaxSpeed;
        public readonly bool[] _IsAxisReverse;
        private readonly double[] _Ratio_PulsePerMm;

        public ScaraPosDef[] MotorPositionArray;
        public readonly bool _Valid;
        private double _ZeroAlpha;
        private double _ZeroR;
        private double _ZeroTheta;

        private double[] _TargetPosition;
        private bool[] _InPosition;
        private bool[] _IsStopped;
        private string _filePath;
        private EAXIS_TYPE _AxisType;
        private PanasonicA6Def[] _ServroDriver;
        private double[] _PulsePerCircle;
        private double[] _ZeroLap;
        private List<double[]> _LastScaraData;
        private const int _SpiliteNum = 300;
        private List<SerialPort> _AxisPort;
        public bool IsValid { get; private set; }

        public ScaraGJSDef(List<SerialPort> Ports, string fileName)
        {
            try
            {

                IsValid = true;
                _IsStopped = new bool[_NumberOfAxis];
                _LastScaraData = new List<double[]>();

                _filePath = fileName + "\\Scara.ini";
                IniFile ini = new IniFile(_filePath, true);
                _AxisPort = Ports;
                _AxisType = (EAXIS_TYPE)Enum.Parse(typeof(EAXIS_TYPE), ini.ReadStr("System", "AxisType", EAXIS_TYPE.AxisA6_ABS485.ToString()), true);
                PowerPercent = ini.ReadDouble("System", "PowerPercent", 100);
                _LinearMoveAccTime = ini.ReadDouble("System", "LinearMoveAccTime", 0.4);
                _LinearMoveDecTime = ini.ReadDouble("System", "LinearMoveDecTime", 0.6);
                MotorPositionArray = new ScaraPosDef[(int)EScaraPosition.Count];
                for (int i = 0; i < (int)EScaraPosition.Count; i++)
                {
                    MotorPositionArray[i] = new ScaraPosDef(true);
                }

                if (_AxisType == EAXIS_TYPE.AxisA6_ABS || _AxisType == EAXIS_TYPE.AxisA6_ABS485)
                {
                    string comport = ini.ReadStr("System", "PanasonicA6COMPort", "COM9");
                    _ServroDriver = new PanasonicA6Def[_NumberOfAxis];
                    _ServroDriver[0] = new PanasonicA6Def(comport, 1);
                    _ServroDriver[1] = new PanasonicA6Def(_ServroDriver[0].Port, 2);
                    _ServroDriver[2] = new PanasonicA6Def(_ServroDriver[0].Port, 3);
                    _ServroDriver[3] = new PanasonicA6Def(_ServroDriver[0].Port, 4);

                    _ZeroLap = new double[_NumberOfAxis];
                }

                ini.FileClose();
                ini.Dispose();

                if (_AxisType == EAXIS_TYPE.AxisMrJeA)
                    _Axes = new Axis_mr_je_a[_NumberOfAxis];
                else if (_AxisType == EAXIS_TYPE.AxisGJS || _AxisType == EAXIS_TYPE.AxisA6_ABS)
                    _Axes = new AxisGJS[_NumberOfAxis];
                else if (_AxisType == EAXIS_TYPE.AxisGJS485 || _AxisType == EAXIS_TYPE.AxisA6_ABS485)
                    _Axes = new AxisGJS485[_NumberOfAxis];
                _MaxSpeed = new double[_NumberOfAxis];
                _AccTime = new double[_NumberOfAxis];
                _DecTime = new double[_NumberOfAxis];
                _IsAxisReverse = new bool[_NumberOfAxis];
                _PulsePerCircle = new double[_NumberOfAxis];
                _Ratio_PulsePerMm = new double[_NumberOfAxis];
                _InPosition = new bool[_NumberOfAxis];
               

                ReadJ1ToJ4Parameter();
                ReadScaraParameter();
                Load();

                _Valid = true;

                _TargetPosition = new double[4];
                for (int i = 0; i < _NumberOfAxis; i++)
                {
                    if (_Axes == null || _Axes[i] == null)
                    {
                        AlarmTextDisplay.Add(
                            (int)AlarmCode.Machine_RobotConnectionError,
                            AlarmType.Alarm,
                            "Scara " + "J" + i.ToString() + " initial failed");
                        _Valid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var className = frame.GetMethod().DeclaringType.FullName;
                var methodName = frame.GetMethod().Name;
                MessageBox.Show(className + ":" + methodName + "()" + Environment.NewLine + ex.ToString());
                _Valid = false;
            }
        }

        public void Dispose()
        {
            if (_Valid)
                _Axes[0].Dispose();

            if (_Valid && _ServroDriver != null && _ServroDriver.Length > 0)
            {
                _ServroDriver[0].Port.Close();
            }
        }

        public SerialPort GetSerialPort()
        {
            if (_Axes != null && _Axes[0] != null && _Axes[0].GetType() == typeof(AxisGJS))
                return ((AxisGJS)_Axes[0]).GetSerialPort();
            if (_Axes != null && _Axes[0] != null && _Axes[0].GetType() == typeof(AxisGJS485))
                return ((AxisGJS485)_Axes[0]).GetSerialPort();

            return null;
        }

        public void Save()
        {
            IniFile wriFileInfo = new IniFile(_filePath, false);
            String sSection = "EScaraPosition";
            string str, str1;
            for (int i = 0; i < MotorPositionArray.Count(); i++)
            {
                str = string.Join(",", MotorPositionArray[i].Value);
                str1 = string.Join(",", MotorPositionArray[i].SpeedPercentage);
                String sKeyFront = ((EScaraPosition)i).ToString();
                wriFileInfo.WriteStr(sSection, sKeyFront, str + "," + str1);
            }
            wriFileInfo.FileClose();
            wriFileInfo.Dispose();

        }
        public void UpdateScaraPositionFromServoDriver()
        {
            for (int i = 0; i < _NumberOfAxis; i++)
            {
                double pos = GetScaraAbsPositionFromServoDriver((Axis)i);
                _Axes[i].SetPos(pos);
            }
        }

        public double GetScaraAbsPositionFromServoDriver(Axis axis)
        {
            if (_ServroDriver == null || _ServroDriver.Length < _NumberOfAxis)
                return 0;

            string errname = "";
            double absLap = _ServroDriver[(int)axis].GetAbslap();
            if (_ServroDriver[(int)axis].GetBatteryStatus(ref errname))
            {
                LogDef.Add(ELogFileName.Alarm, "LowPower", ((ScaraGJSDef.Axis)(int)axis).ToString(), errname);
                AlarmTextDisplay.Add((int)AlarmCode.Warning, AlarmType.Warning, ((ScaraGJSDef.Axis)(int)axis).ToString() + " " + errname);
            }
            //if (!_IsAxisReverse[index])
            //    return (absLap - _ZeroLap[index]) * _PulsePerCircle[index] * _Ratio_PulsePerMm[index]*-1;
            //else
            return (absLap - _ZeroLap[(int)axis]) * _PulsePerCircle[(int)axis] * _Ratio_PulsePerMm[(int)axis] * -1;
        }

        private void Load()
        {
            if (!System.IO.File.Exists(_filePath))
                return;

            String[] _tMotorPos;
            _tMotorPos = new string[MotorPositionArray.Count()];

            IniFile cReaFileInfo = new IniFile(_filePath, true);
            String sSection = "EScaraPosition";
            string str, str1;
            for (int i = 0; i < MotorPositionArray.Count(); i++)
            {
                str = string.Join(",", MotorPositionArray[i].Value);
                str1 = string.Join(",", MotorPositionArray[i].SpeedPercentage);
                String sKeyFront = ((EScaraPosition)i).ToString();
                _tMotorPos[i] = cReaFileInfo.ReadStr(sSection, sKeyFront, str + "," + str1);

                String[] split = _tMotorPos[i].Split(',');
                for (int j = 0; j < MotorPositionArray[i].Value.Count(); j++)
                {
                    MotorPositionArray[i].Value[j] = double.Parse(split[j]);
                }
                // for (int j = 0; j < (int)ESPEED_TYPE.SPEED_COUNT; j++)
                // {
                MotorPositionArray[i].SpeedPercentage = ushort.Parse(split[split.Count() - 1]);
                //}
            }

            cReaFileInfo.FileClose();
            cReaFileInfo.Dispose();
        }

        #region Get/Set

        public void SetSEL(Axis Jn, double Plus, double Minus)
        {
            IniFile ini = new IniFile(_filePath, false);
            string section = "J" + ((int)Jn + 1).ToString();

            if (Plus == Minus)
            {
                _Axes[(int)Jn].SetSoftELEnable(false);
                ini.WriteBool(section, "SELEnable", false);
                ini.FileClose();
                ini.Dispose();
                return;
            }

            _Axes[(int)Jn].SetSoftELEnable(true);
            if (_IsAxisReverse[(int)Jn])
            {
                _Axes[(int)Jn].SetSPEL(Minus * -1);
                _Axes[(int)Jn].SetSMEL(Plus * -1);
            }
            else
            {
                _Axes[(int)Jn].SetSPEL(Plus);
                _Axes[(int)Jn].SetSMEL(Minus);
            }

            ini.WriteBool(section, "SELEnable", true);
            ini.WriteDouble(section, "SPEL", Plus);
            ini.WriteDouble(section, "SMEL", Minus);
            ini.FileClose();
            ini.Dispose();
        }

        public bool GetSEL(Axis Jn, ref double Plus, ref double Minus)
        {
            IniFile ini = new IniFile(_filePath, true);
            string section = "J" + ((int)Jn + 1).ToString();
            if (!ini.ReadBool(section, "SELEnable", false))
                return false;


            Plus = ini.ReadDouble(section, "SPEL", Plus);
            Minus = ini.ReadDouble(section, "SMEL", Minus);
            ini.Dispose();
            return true;
        }
        private void ReadJ1ToJ4Parameter()
        {
            IniFile ini = new IniFile(_filePath, true);
            for (int i = 0; i < _NumberOfAxis; i++)
            {
                try
                {
                    string section = "J" + (i + 1).ToString();

                    if (_AxisType == EAXIS_TYPE.AxisMrJeA)
                    {
                        _Axes[i] =
                            i < 1 ?
                            new Axis_mr_je_a(ini.ReadStr(section, "Comport", "COM10"),
                                115200, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One,
                                (byte)ini.ReadInt(section, "SlaveAddress", i + 1)) :
                            new Axis_mr_je_a(_Axes[0].GetModbus(),
                                (byte)ini.ReadInt(section, "SlaveAddress", i + 1));
                    }
                    else if (_AxisType == EAXIS_TYPE.AxisA6_ABS || _AxisType == EAXIS_TYPE.AxisGJS)
                    {
                        bool same = false;
                        SerialPort SPort = new SerialPort();
                        for (int j = 0; j < _AxisPort.Count; j++)
                        {
                            if (_AxisPort[j].PortName.ToUpper() == ini.ReadStr(section, "Comport", "COM10").ToUpper())
                            {
                                same = true;
                                SPort = _AxisPort[j];
                            }
                        }

                        _Axes[i] =
                            i < 1 && !same ?
                            new AxisGJS(ini.ReadStr(section, "Comport", "COM10"),
                                115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One,
                                (byte)ini.ReadInt(section, "SlaveAddress", i)) :
                            new AxisGJS(SPort,
                                (byte)ini.ReadInt(section, "SlaveAddress", i));

                        if (!same)
                            _AxisPort.Add(((AxisGJS)_Axes[i]).GetSerialPort());
                    }
                    else if (_AxisType == EAXIS_TYPE.AxisA6_ABS485 || _AxisType == EAXIS_TYPE.AxisGJS485)
                    {
                        bool same = false;
                        SerialPort SPort = new SerialPort();
                        for (int j = 0; j < _AxisPort.Count; j++)
                        {
                            if (_AxisPort[j].PortName.ToUpper() == ini.ReadStr(section, "Comport", "COM10").ToUpper())
                            {
                                same = true;
                                SPort = _AxisPort[j];
                            }
                        }

                        _Axes[i] =
                            i < 1 && !same ?
                            new AxisGJS485(ini.ReadStr(section, "Comport", "COM10"),
                                115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One,
                                (byte)ini.ReadInt(section, "SlaveAddress", i), (byte)ini.ReadInt(section, "StationID", 1)) :
                            new AxisGJS485(SPort,
                                (byte)ini.ReadInt(section, "SlaveAddress", i), (byte)ini.ReadInt(section, "StationID", 1));

                        if (!same)
                            _AxisPort.Add(((AxisGJS485)_Axes[i]).GetSerialPort());
                    }

                    _IsAxisReverse[i] = ini.ReadBool(section, "IsReverse", false);
                    _Ratio_PulsePerMm[i] = ini.ReadDouble(section, "Ratio_PulsePerMm", (double)1);

                    double maxRPM = ini.ReadInt(section, "MaxRPM", 3000);
                    _PulsePerCircle[i] = ini.ReadInt(section, "PulsePerCircle", 3000);
                    _AccTime[i] = ini.ReadDouble(section, "Acc", 0.3);
                    _DecTime[i] = ini.ReadDouble(section, "Dec", 0.3);

                    

                    _MaxSpeed[i] = maxRPM / 60.0 * _PulsePerCircle[i] * _Ratio_PulsePerMm[i];

                    _Axes[i].SetPlsRto(_Ratio_PulsePerMm[i]);
                    if (_Axes[i] != null && _AxisType == EAXIS_TYPE.AxisMrJeA)
                    {
                        _Axes[i].HmMv(true);
                    }
                    else
                    {
                        _ZeroLap[i] = ini.ReadDouble(section, "ZeroLap", 0);
                        double pos = GetScaraAbsPositionFromServoDriver(i);
                        _Axes[i].SetPos(pos);
                        _Axes[i].EnableEL(false);
                    }

                    if (ini.ReadBool(section, "SELEnable", false))
                    {
                        _Axes[i].SetSoftELEnable(true);

                        if (_IsAxisReverse[i])
                        {
                            _Axes[i].SetSMEL(ini.ReadDouble(section, "SPEL", 0) * -1);
                            _Axes[i].SetSPEL(ini.ReadDouble(section, "SMEL", 0) * -1);
                        }
                        else
                        {
                            _Axes[i].SetSMEL(ini.ReadDouble(section, "SMEL", 0));
                            _Axes[i].SetSPEL(ini.ReadDouble(section, "SPEL", 0));
                        }
                    }
                    else
                        _Axes[i].SetSoftELEnable(false);
                }
                catch (Exception ex)
                {
                    IsValid = false;
                    var frame = (new StackTrace(ex, true)).GetFrame(0);
                    var className = frame.GetMethod().DeclaringType.FullName;
                    var methodName = frame.GetMethod().Name;

                    string msg = "ERROR CLASS : " + className + Environment.NewLine +
                        "ERROR FUNCTION : " + methodName + "()" + Environment.NewLine +
                        "ERROR KEY : " + i.ToString() + Environment.NewLine +
                        "ERROR CONTENT : " + Environment.NewLine + ex.ToString();

                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_ScaraError, AlarmType.Alarm, msg);

                    MessageBox.Show(msg,
                        this.GetType().Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            ini.FileClose();
            ini.Dispose();
        }

        public double GetAbsLapFromServoDriver(int index)
        {
            if (_ServroDriver == null || _ServroDriver.Length < _NumberOfAxis)
                return 0;
            return _ServroDriver[index].GetAbslap();
        }

        public double GetScaraAbsPositionFromServoDriver(int index)
        {
            if (_ServroDriver == null || _ServroDriver.Length < _NumberOfAxis)
                return 0;

            string errname = "";
            double absLap = _ServroDriver[index].GetAbslap();
            if (_ServroDriver[index].GetBatteryStatus(ref errname))
            {
                LogDef.Add(ELogFileName.Alarm, "LowPower", ((ScaraGJSDef.Axis)index).ToString(), errname);
                AlarmTextDisplay.Add((int)AlarmCode.Warning, AlarmType.Warning, ((ScaraGJSDef.Axis)index).ToString() + " " + errname);
            }
            //if (!_IsAxisReverse[index])
            //    return (absLap - _ZeroLap[index]) * _PulsePerCircle[index] * _Ratio_PulsePerMm[index]*-1;
            //else
            return (absLap - _ZeroLap[index]) * _PulsePerCircle[index] * _Ratio_PulsePerMm[index] * -1;
        }

        public void SetPos(ScaraGJSDef.Axis axis, double value)
        {
            if (_IsAxisReverse[(int)axis])
                value = value * -1;
            _Axes[(int)axis].SetPos(value);
        }

        public bool BatteryLowPowerStatus()
        {
            if (_ServroDriver == null)
                return false;

            bool PowerLow = false;

            for (int i = 0; i < (int)ScaraGJSDef.Axis.Count; i++)
            {
                if (_ServroDriver[i] == null)
                    return false;


                _ServroDriver[i].GetAbslap();

                string errname = "";
                if (_ServroDriver[i].GetBatteryStatus(ref errname))
                {
                    LogDef.Add(ELogFileName.Alarm, "LowPower", ((ScaraGJSDef.Axis)i).ToString(), errname);
                    AlarmTextDisplay.Add((int)AlarmCode.Warning, AlarmType.Warning, ((ScaraGJSDef.Axis)i).ToString() + " " + errname);
                    PowerLow = true;
                }
            }

            return PowerLow;
        }

        public void SetZeroPosition(int Index)
        {
            IniFile ini = new IniFile(_filePath, false);
            string section = "J" + (Index + 1).ToString();
            _ZeroLap[Index] = GetAbsLapFromServoDriver(Index);
            ini.WriteDouble(section, "ZeroLap", _ZeroLap[Index]);
            ini.FileClose();
            ini.Dispose();

            _Axes[Index].SetPos(0);
        }
        private void ReadScaraParameter()
        {
            IniFile ini = new IniFile(_filePath, true);

            string section = "Scara";
            double initialX = ini.ReadDouble(section, "InitialX", 0);
            double initialY = ini.ReadDouble(section, "InitialY", 0);
            double initialAngle = ini.ReadDouble(section, "InitialAngle", 0);
            double initialJ1 = ini.ReadDouble(section, "InitialJ1", 0);
            double initialJ2 = ini.ReadDouble(section, "InitialJ2", 0);
            double initialJ3 = ini.ReadDouble(section, "InitialJ3", 0);

            double d = Math.Pow(Math.Pow(initialX, 2) + Math.Pow(initialY, 2), 0.5);
            _ZeroR = d - initialJ2;
            _ZeroAlpha = initialJ1 - Math.Atan2(initialY, initialX) / Math.PI * 180;
            _ZeroTheta = initialAngle + _ZeroAlpha - initialJ3;

            ini.FileClose();
            ini.Dispose();
        }
        private void ClearIsStoppeStatus()
        {
            for (int i = 0; i < _IsStopped.Length; i++)
            {
                _IsStopped[i] = false;
                _InPosition[i] = false;
            }
        }
        public double[] GetJ1ToJ4Position()
        {
            if (!_Valid)
                return new double[] { 0, 0, 0, 0 };
            double[] j1Toj4Position_mm = new double[_NumberOfAxis];
            for (int i = 0; i < _NumberOfAxis; i++)
            {
                j1Toj4Position_mm[i] = _IsAxisReverse[i] ?
                    _Axes[i].GetPos(0) * -1 :
                    _Axes[i].GetPos(0);
            }
            return j1Toj4Position_mm;
        }
        public double GetPosition(Axis axis)
        {
            double position_mm = _IsAxisReverse[(int)axis] ?
                    _Axes[(int)axis].GetPos(0) * -1 :
                    _Axes[(int)axis].GetPos(0);
            return position_mm;
        }

        /// <summary>
        /// return X,Y,Z,Angle
        /// </summary>
        /// <returns></returns>
        public double[] GetCartesianCoordinatePostion()
        {
            double[] ps = GetJ1ToJ4Position();
            double[] pc = new double[(int)CartesianCoordinate.Count];
            ConvertScaraToCartesianCoordinate(ps[0], ps[1], ps[2], out pc[0], out pc[1], out pc[3]);
            pc[2] = ps[3];   //z = j4
            return pc;

        }



        public bool InPosition(EScaraPosition EPos, double[] tolerance)
        {
            if (!_Valid)
                return true;

            for (int i = 0; i < _NumberOfAxis; i++)
            {
                if (!_InPosition[i])
                {
                    _InPosition[i] = Math.Abs(GetPosition((Axis)i) - MotorPositionArray[(int)EPos].Value[i]) < tolerance[i];
                    if (!_InPosition[i])
                        return false;
                }
            }

            return true;
        }

        public bool InPosition(double tolerance)
        {
            if (!_Valid)
                return true;

            for (int i = 0; i < _NumberOfAxis; i++)
            {
                if (!_InPosition[i])
                {
                    _InPosition[i] = Math.Abs(GetPosition((Axis)i) - _TargetPosition[i]) < tolerance;
                    if (!_InPosition[i])
                        return false;
                }
            }

            return true;
        }

        public bool IsStopped(bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;

            for (int i = 0; i < _NumberOfAxis; i++)
            {
                if (!_Axes[i].Stop(true))
                {
                    return false;
                }
            }
            _LastScaraData.Clear();
            return true;
        }

        public bool IsStopped(Axis axis, bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;
            bool s = _Axes[(int)axis].Stop(true);
            return s;
        }

        public bool GetLspSignal(Axis axis, bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;
            if (_IsAxisReverse[(int)axis])
                return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.MEL);
            return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.PEL);
        }

        public bool GetSoftLspSignal(Axis axis, bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;
            if (_AxisType != EAXIS_TYPE.AxisGJS)
                return defaultStatus;
            if (_IsAxisReverse[(int)axis])
                return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.SoftMEL);
            return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.SoftPEL);
        }

        public bool GetLsnSignal(Axis axis, bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;
            if (_AxisType != EAXIS_TYPE.AxisGJS)
                return defaultStatus;
            if (_IsAxisReverse[(int)axis])
                return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.PEL);
            return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.MEL);
        }

        public bool GetSoftLsnSignal(Axis axis, bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;
            if (_IsAxisReverse[(int)axis])
                return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.SoftPEL);
            return _Axes[(int)axis].GetMtnIOStatus(EMOTION_IO.SoftMEL);
        }
        //public void Test(double[] j1Toj4TargetPosition_mm, ushort speedPercentage)
        //{
        //    double[] currentPosition_mm = Gej1Toj4Position();

        //    int[] targetPosition_pulse;
        //    double[] motorRpm, motorAccDecTimeToRatedRpm_second, dwell_second;


        //    CalculateAllAxisArrivedSameTimeSpeed(currentPosition_mm, j1Toj4TargetPosition_mm, speedPercentage,
        //    out targetPosition_pulse, out motorRpm, out motorAccDecTimeToRatedRpm_second, out dwell_second);

        //    double[] t = new double[4];

        //    for (int i = 0; i < _NumberOfAxis; i++)
        //    {
        //        double ta = (motorRpm[i] * motorAccDecTimeToRatedRpm_second[i]) / 3000;
        //        double v = (motorRpm[i] / 60) * 3000;
        //        double tv = (targetPosition_pulse[i] / v) - ta;
        //        if (targetPosition_pulse[i] == 0)
        //        {
        //            t[i] = dwell_second[i];
        //        }
        //        else
        //        {
        //            t[i] = (ta * 2) + tv + dwell_second[i] + (2 * _SCurveTime_second[i]);
        //        }
        //    }
        //}

        public void SetCurrentPosition(EScaraPosition position)
        {
            if (_Axes[0].GetType() == typeof(AxisGJS))
            {
                lock (((AxisGJS)_Axes[0]).GetSerialPort())
                {
                    double[] currentPosition = GetJ1ToJ4Position();
                    for (int i = 0; i < currentPosition.Length; i++)
                        MotorPositionArray[(int)position].Value[i] = currentPosition[i];
                    MotorPositionArray[(int)position].SpeedPercentage = 20;
                }
            }
            if (_Axes[0].GetType() == typeof(AxisGJS485))
            {
                lock (((AxisGJS485)_Axes[0]).GetSerialPort())
                {
                    double[] currentPosition = GetJ1ToJ4Position();
                    for (int i = 0; i < currentPosition.Length; i++)
                        MotorPositionArray[(int)position].Value[i] = currentPosition[i];
                    MotorPositionArray[(int)position].SpeedPercentage = 20;
                }

            }
        }
        #endregion

        #region Calculation
        //private void CalculateAllAxisArrivedSameTimeSpeed(double[] startingPosition_mm, double[] targetPosition_mm, ushort speedPercentage,
        //    out int[] targetPosition_pulse, out double[] motorRpm, out double[] motorAccDecTime_second, out double[] dwell_second)
        //{
        //    lock (_lock)
        //    {
        //        int[] startingPosition_pulse = new int[_NumberOfAxis];
        //        double[] speed_pulsePerSecond = new double[_NumberOfAxis];
        //        double[] accDecRate_pulsePerSecondSquare = new double[_NumberOfAxis];
        //        double[] moveTime_second = new double[_NumberOfAxis];

        //        targetPosition_pulse = new int[_NumberOfAxis];
        //        motorRpm = new double[_NumberOfAxis];
        //        motorAccDecTime_second = new double[_NumberOfAxis];
        //        dwell_second = new double[_NumberOfAxis];


        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            ConvertSpeedPercentageToTruelySpeed((Axis)i, speedPercentage, out motorRpm[i], out motorAccDecTime_second[i]);
        //            targetPosition_pulse[i] = ConvertMmToPulse((Axis)i, targetPosition_mm[i]);
        //            startingPosition_pulse[i] = ConvertMmToPulse((Axis)i, startingPosition_mm[i]);
        //            accDecRate_pulsePerSecondSquare[i] = ConvertMotorAccTimeToAccRate((Axis)i, motorAccDecTime_second[i]);
        //            speed_pulsePerSecond[i] = ConvertRpmToPuslePerSecond((Axis)i, motorRpm[i]);


        //            double ta = speed_pulsePerSecond[i] / accDecRate_pulsePerSecondSquare[i];
        //            moveTime_second[i] = (Math.Abs(startingPosition_pulse[i] - targetPosition_pulse[i]) - ta * speed_pulsePerSecond[i] / 2 - ta * speed_pulsePerSecond[i] / 2) / speed_pulsePerSecond[i] + (ta * 2);

        //            if (Math.Abs(startingPosition_pulse[i] - targetPosition_pulse[i]) < (double)1 / (double)60 * _PulsePerRound[i] * _SCurveTime_second[i])
        //            {
        //                targetPosition_pulse[i] = startingPosition_pulse[i];
        //                continue;
        //            }
        //            if ((ta * 2) * speed_pulsePerSecond[i] / 2 > Math.Abs(startingPosition_pulse[i] - targetPosition_pulse[i]))
        //            {
        //                //當vt圖無法呈現梯形，先降低轉速，當轉速降到極限下時改降加速度
        //                //當無法順利移動時將不會移動
        //                //這邊計算是為了讓移動時間是有意義且馬達可以做到的
        //                while ((ta * 2) * speed_pulsePerSecond[i] / 2 > Math.Abs(startingPosition_pulse[i] - targetPosition_pulse[i]))
        //                {
        //                    motorRpm[i]--;
        //                    speed_pulsePerSecond[i] = motorRpm[i] / 60 * _PulsePerRound[i];
        //                    ta = speed_pulsePerSecond[i] / accDecRate_pulsePerSecondSquare[i];
        //                    if (motorRpm[i] < 2)
        //                    {
        //                        while ((ta * 2) * speed_pulsePerSecond[i] / 2 > Math.Abs(startingPosition_pulse[i] - targetPosition_pulse[i]))
        //                        {
        //                            motorAccDecTime_second[i] += 0.01;
        //                            accDecRate_pulsePerSecondSquare[i] = (_RatedRpm[i] / 60 * _PulsePerRound[i]) / motorAccDecTime_second[i];
        //                            ta = speed_pulsePerSecond[i] / accDecRate_pulsePerSecondSquare[i];
        //                            if (motorAccDecTime_second[i] >= 20)
        //                            {
        //                                targetPosition_pulse[i] = startingPosition_pulse[i];
        //                                break;
        //                            }
        //                        }
        //                        break;
        //                    }
        //                }
        //                moveTime_second[i] = ta * 2;
        //            }
        //        }

        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            if (startingPosition_pulse[i] == targetPosition_pulse[i]) //移動量太低 不移動
        //                dwell_second[i] = moveTime_second.Max() + _SCurveTime_second[i] * 2;
        //            else
        //            {
        //                double moveDistance = Math.Abs(startingPosition_pulse[i] - targetPosition_pulse[i]);
        //                double tv = Math.Sqrt(Math.Pow(moveTime_second.Max(), 2) - (4 * moveDistance / accDecRate_pulsePerSecondSquare[i]));
        //                if (double.IsNaN(tv))
        //                    tv = 0;
        //                double ta = (moveTime_second.Max() - tv) / 2;
        //                speed_pulsePerSecond[i] = accDecRate_pulsePerSecondSquare[i] * ta;
        //                motorRpm[i] = ConvertPulsePerSecondToRpm((Axis)i, speed_pulsePerSecond[i]);

        //                //去小數點
        //                double reducePointBaseNumber = Math.Ceiling(motorRpm[i]) / motorRpm[i];
        //                motorRpm[i] *= reducePointBaseNumber;
        //                speed_pulsePerSecond[i] *= reducePointBaseNumber;
        //                ta = ta * (1 / reducePointBaseNumber) + ta * ((reducePointBaseNumber - 1) / reducePointBaseNumber) + tv * ((reducePointBaseNumber - 1) / reducePointBaseNumber);


        //                if (ConvertAccRateToMotorAccTime((Axis)i, speed_pulsePerSecond[i] / ta) > 20)
        //                {
        //                    motorRpm[i]++;
        //                    speed_pulsePerSecond[i] = ConvertRpmToPuslePerSecond((Axis)i, motorRpm[i]);
        //                    ta = speed_pulsePerSecond[i] / accDecRate_pulsePerSecondSquare[i];
        //                    tv = (moveDistance - speed_pulsePerSecond[i] * ta) / speed_pulsePerSecond[i];
        //                    dwell_second[i] += moveTime_second.Max() - (2 * ta + tv);
        //                }
        //                motorAccDecTime_second[i] = ConvertAccRateToMotorAccTime((Axis)i, speed_pulsePerSecond[i] / ta);
        //            }
        //            if (motorAccDecTime_second[i] >= 20)
        //                motorAccDecTime_second[i] = 20;
        //        }

        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            if (_IsAxisReverse[i])
        //                targetPosition_pulse[i] *= -1;
        //            if (motorRpm[i] < 1)
        //                motorRpm[i] = 1;
        //        }
        //    }
        //}

        private void ConvertScaraToCartesianCoordinate(double j1, double j2, double j3, out double x, out double y, out double angle)
        {
            angle = j1 + j3 - _ZeroAlpha - _ZeroTheta;

            x = (j2 + _ZeroR) * Math.Cos((j1 - _ZeroAlpha) / 180.0 * Math.PI);
            y = (j2 + _ZeroR) * Math.Sin((j1 - _ZeroAlpha) / 180.0 * Math.PI);
        }
        private void ConvertCartesianToScaraCoordinate(double x, double y, double angle, out double j1, out double j2, out double j3)
        {
            double fDistance = Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5);
            j2 = fDistance - _ZeroR;
            j1 = Math.Atan2(y, x) / Math.PI * 180 + _ZeroAlpha;
            j3 = angle + _ZeroAlpha + _ZeroTheta - j1;
        }
        public void ConvertCartesianShiftToScaraCoordinate(
            double currentAlpha, double currentR, double currentTheta,
            double shiftX, double shiftY, double shiftAngle,
            out double shiftAlpha, out double shiftR, out double shiftTheta)
        {
            double currentX, currentY, currentAngle;
            ConvertScaraToCartesianCoordinate(currentAlpha, currentR, currentTheta, out currentX, out currentY, out currentAngle);

            double dstX = currentX + shiftX;
            double dstY = currentY + shiftY;
            double dstAngle = currentAngle + shiftAngle;

            double absAlpha, absR, absTheta;
            ConvertCartesianToScaraCoordinate(dstX, dstY, dstAngle, out absAlpha, out absR, out absTheta);

            shiftAlpha = absAlpha - currentAlpha;
            shiftR = absR - currentR;
            shiftTheta = absTheta - currentTheta;
            if (shiftAlpha > 180)
                shiftAlpha -= 360;
            if (shiftAlpha < -180)
                shiftAlpha += 360;
            if (shiftTheta > 180)
                shiftTheta -= 360;
            if (shiftTheta < -180)
                shiftTheta += 360;

        }
        #endregion

        #region Move  (Use ClearIsStoppedStatus() first)
        public void Move(EScaraPosition Position)
        {
            if (CheckEncoderFail())
                return;
            //    double[][] pos = new double[2][];
            //    pos[0] = (double[])Gej1Toj4Position().Clone();
            //    pos[1] =(double[]) _MotorPositionArray[(int)Position]._Value.Clone();
            //    double[] distance = GetPathDistance(pos);

            //    double maxT = 0;
            //    for(int i = 0; i < _NumberOfAxis; i++)
            //    {
            //        double t = GetMoveTime((Axis)i, _MotorPositionArray[(int)Position]._SpeedPercentage, distance[i]);
            //        if(t > maxT)
            //            maxT = t;
            //    }
            AxisMove(Axis.J1, Position);
            AxisMove(Axis.J2, Position);
            AxisMove(Axis.J3, Position);
            AxisMove(Axis.J4, Position);

            _LastScaraData.Clear();
        }

        public void Move(EScaraPosition Position, double percentage)
        {
            if (CheckEncoderFail())
                return;

            AxisMove(Axis.J1, Position, percentage);
            AxisMove(Axis.J2, Position, percentage);
            AxisMove(Axis.J3, Position, percentage);
            AxisMove(Axis.J4, Position, percentage);

            _LastScaraData.Clear();
        }

        //private void AxisRelMv(Axis Axis, double Distance, double MoveTime)
        //{
        //    if (CheckEncoderFail())
        //        return;

        //    if (!_Valid)
        //        return;
        //    ClearIsStoppeStatus();

        //    Distance = _IsAxisReverse[(int)Axis] ? Distance * -1 : Distance;

        //    SpeedDef moveSpeed = new SpeedDef();
        //    moveSpeed._EndSpd = 0;
        //    moveSpeed._StartSpd = 0;

        //    if(MoveTime < _AccTime[(int)Axis] + _DecTime[(int)Axis])
        //    {
        //        moveSpeed._AccRate = 1.0/(MoveTime * _AccTime[(int)Axis] / (_AccTime[(int)Axis] + _DecTime[(int)Axis]));
        //        moveSpeed._DecRate = 1.0/(MoveTime * _DecTime[(int)Axis] / (_AccTime[(int)Axis] + _DecTime[(int)Axis]));

        //    }
        //    moveSpeed._MaxSpd = _MaxSpeed[(int)Axis] * speedPercentage * 0.01;
        //    moveSpeed._AccRate = moveSpeed._MaxSpd / _AccTime[(int)Axis];
        //    moveSpeed._DecRate = moveSpeed._MaxSpd / _DecTime[(int)Axis];

        //    _Axes[(int)Axis].SetSpd(moveSpeed);

        //    _Axes[(int)Axis].RelMv(Distance);
        //}

        public void ShiftMove(EScaraPosition Position, double[] shift)
        {
            if (CheckEncoderFail())
                return;

            AxisShiftMove(Axis.J1, Position, shift[0]);
            AxisShiftMove(Axis.J2, Position, shift[1]);
            AxisShiftMove(Axis.J3, Position, shift[2]);
            AxisShiftMove(Axis.J4, Position, shift[3]);

            _LastScaraData.Clear();
        }

        public void AxisMove(Axis axis, EScaraPosition Position)
        {
            if (CheckEncoderFail(axis))
                return;

            if (axis == Axis.J3)
            {
                double speed = MotorPositionArray[(int)Position].SpeedPercentage;
                speed = speed * _Ratio_PulsePerMm[(int)Axis.J1] / _Ratio_PulsePerMm[(int)Axis.J3];
                AbsoluteMove(axis, MotorPositionArray[(int)Position].Value[(int)axis], (ushort)speed);
                return;
            }
            AbsoluteMove(axis, MotorPositionArray[(int)Position].Value[(int)axis], MotorPositionArray[(int)Position].SpeedPercentage);
        }

        public void AxisMove(Axis axis, EScaraPosition Position, double speedPercentage)
        {
            if (CheckEncoderFail(axis))
                return;

            if (axis == Axis.J3)
            {
                double speed = speedPercentage;
                speed = speed * _Ratio_PulsePerMm[(int)Axis.J1] / _Ratio_PulsePerMm[(int)Axis.J3];
                AbsoluteMove(axis, MotorPositionArray[(int)Position].Value[(int)axis], (ushort)speed);
                return;
            }
            AbsoluteMove(axis, MotorPositionArray[(int)Position].Value[(int)axis], (ushort)speedPercentage);
        }

        public bool AxisInPosition(Axis axis, EScaraPosition Position, double tolerance)
        {
            if (CheckEncoderFail(axis))
                return false;

            double pos = GetPosition(axis);
            if (!_Valid)
                return true;

            if (Math.Abs(pos - MotorPositionArray[(int)Position].Value[(int)axis]) < tolerance)
                return true;

            return false;

        }

        public bool AxisIsStopped(Axis axis, EScaraPosition Position, double tolerance)
        {
            if (CheckEncoderFail(axis))
                return false;

            double pos = GetPosition(axis);
            if (_Valid)
            {
                if (Math.Abs(pos - MotorPositionArray[(int)Position].Value[(int)axis]) > tolerance)
                    return false;
            }
            if (!IsStopped(axis, true))
                return false;

            return true;

        }

        public bool PositionIsStopped(EScaraPosition Position, double tolerance)
        {
            if (CheckEncoderFail())
                return false;

            for (int i = 0; i < MotorPositionArray[(int)Position].GetAxisNum(); i++)
            {
                double pos = GetPosition((Axis)i);
                if (_Valid)
                {
                    if (Math.Abs(pos - MotorPositionArray[(int)Position].Value[i]) > tolerance)
                        return false;
                }
                if (!IsStopped((Axis)i, true))
                    return false;
            }
            return true;

        }

        public void AbsoluteMove(Axis axis, double position, double speedPercentage)
        {
            speedPercentage = PowerPercent*0.01 * speedPercentage;

            if (CheckEncoderFail(axis))
                return;

            if (!_Valid)
                return;
            ClearIsStoppeStatus();

            position = _IsAxisReverse[(int)axis] ? position * -1 : position;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            moveSpeed._AccRate = moveSpeed._MaxSpd / (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
            moveSpeed._DecRate = moveSpeed._MaxSpd / (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));

            _Axes[(int)axis].SetSpd(moveSpeed);

            _Axes[(int)axis].AbsMv(position);
        }

        public bool IsGreaterThan(Axis Axis, EScaraPosition EPos)
        {
            return GetPosition(Axis) > MotorPositionArray[(int)EPos].Value[(int)Axis];
        }

        public bool IsLessThan(Axis Axis, EScaraPosition EPos)
        {
            return GetPosition(Axis) < MotorPositionArray[(int)EPos].Value[(int)Axis];
        }

        public bool IsClose(Axis Axis, EScaraPosition EPos, double Tolerance)
        {
            return Math.Abs(GetPosition(Axis) - MotorPositionArray[(int)EPos].Value[(int)Axis]) < Tolerance;
        }

        public void RelativeGoJ1J3(double[] OrgPos, double[] DstPos, ushort SpeedPercentage)
        {
            if (CheckEncoderFail())
                return;

            double[] sumDistance = new double[_NumberOfAxis];
            for (int i = 0; i < sumDistance.Length; i++)
            {
                sumDistance[i] = DstPos[i] - OrgPos[i];
            }

            double maxTime = GetMoveTime(Axis.J1, SpeedPercentage, Math.Abs(sumDistance[(int)Axis.J1]));
            double t = GetMoveTime(Axis.J3, SpeedPercentage, Math.Abs(sumDistance[(int)Axis.J3]));
            if (t > maxTime)
                maxTime = t;

            RelativeMoveByTime(Axis.J1, sumDistance[(int)Axis.J1], maxTime);
            LogDef.Add(ELogFileName.Alarm, "Relative", "GO", "StartJ1");
            RelativeMoveByTime(Axis.J3, sumDistance[(int)Axis.J3], maxTime);
            LogDef.Add(ELogFileName.Alarm, "Relative", "GO", "StartJ3");

            _LastScaraData.Clear();
        }

        public void RelativeGo(double[] OrgPos, double[] DstPos, ushort SpeedPercentage)
        {
            if (CheckEncoderFail())
                return;

            double[] sumDistance = new double[_NumberOfAxis];
            for (int i = 0; i < sumDistance.Length; i++)
            {
                sumDistance[i] = DstPos[i] - OrgPos[i];
            }

            double maxTime = 0;

            for (int i = 0; i < sumDistance.Length; i++)
            {
                double t = GetMoveTime((Axis)i, SpeedPercentage, Math.Abs(sumDistance[i]));
                if (t > maxTime)
                    maxTime = t;
            }

            for (int i = 0; i < _NumberOfAxis; i++)
            {
                RelativeMoveByTime((Axis)i, sumDistance[i], maxTime);
            }

            _LastScaraData.Clear();
        }

        public void ContinueGo(double[] OrgPos, double[] DstPos, ushort SpeedPercentage)
        {
            if (CheckEncoderFail())
                return;

            double[] sumDistance = new double[_NumberOfAxis];
            for (int i = 0; i < sumDistance.Length; i++)
            {
                sumDistance[i] = DstPos[i] - OrgPos[i];
            }

            double maxTime = 0;

            for (int i = 0; i < sumDistance.Length; i++)
            {
                double t = GetMoveTime((Axis)i, SpeedPercentage, Math.Abs(sumDistance[i]));
                if (t > maxTime)
                    maxTime = t;
            }

            for (int i = 0; i < _NumberOfAxis; i++)
            {
                ChangeDstByTime((Axis)i, DstPos[i], maxTime, 0.8);
            }

            _LastScaraData.Clear();
        }

        public void ChangeDstByTime(Axis axis, double Dst, double MoveTime, double ChangeRatio)
        {
            if (CheckEncoderFail(axis))
                return;

            if (!_Valid)
                return;
            ClearIsStoppeStatus();
            Dst = _IsAxisReverse[(int)axis] ? Dst * -1 : Dst;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = 0;
            moveSpeed._AccRate = 0;
            moveSpeed._DecRate = 0;

            _Axes[(int)axis].SetSpd(moveSpeed);

            if (_Axes[(int)axis].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[(int)axis]).ChangeDestinationByTime(Dst, MoveTime, _AccTime[(int)axis], _DecTime[(int)axis], ChangeRatio);
            else if (_Axes[(int)axis].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[(int)axis]).ChangeDestinationByTime(Dst, MoveTime, _AccTime[(int)axis], _DecTime[(int)axis], ChangeRatio);
            _LastScaraData.Clear();
        }

        public void RelativeMoveByTime(Axis axis, double distance, double MoveTime)
        {
            if (CheckEncoderFail(axis))
                return;

            if (!_Valid)
                return;
            ClearIsStoppeStatus();
            distance = _IsAxisReverse[(int)axis] ? distance * -1 : distance;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = 0;
            moveSpeed._AccRate = 0;
            moveSpeed._DecRate = 0;

            _Axes[(int)axis].SetSpd(moveSpeed);
            if (_Axes[(int)axis].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[(int)axis]).RelMvByTime(distance, MoveTime, _AccTime[(int)axis], _DecTime[(int)axis]);
            else if (_Axes[(int)axis].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[(int)axis]).RelMvByTime(distance, MoveTime, _AccTime[(int)axis], _DecTime[(int)axis]);
            _LastScaraData.Clear();
        }

        public void RelativeMove(Axis axis, double distance, double speedPercentage)
        {
            if (CheckEncoderFail(axis))
                return;

            if (!_Valid)
                return;
            ClearIsStoppeStatus();
            speedPercentage = PowerPercent * 0.01 * speedPercentage;

            distance = _IsAxisReverse[(int)axis] ? distance * -1 : distance;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            moveSpeed._AccRate = moveSpeed._MaxSpd / (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
            moveSpeed._DecRate = moveSpeed._MaxSpd / (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));

            _Axes[(int)axis].SetSpd(moveSpeed);

            _Axes[(int)axis].RelMv(distance);

            _LastScaraData.Clear();
        }

        public void RelativeMove(double shiftX, double shiftY, double shiftZ, double shiftAngle, double speedPercentage)
        {
            if (CheckEncoderFail())
                return;

            if (!_Valid)
                return;

            speedPercentage = PowerPercent * 0.01 * speedPercentage;

            ClearIsStoppeStatus();
            double[] currentPosition = GetJ1ToJ4Position();
            double[] shiftPosition = new double[_NumberOfAxis];
            for (int i = 0; i < _NumberOfAxis; i++)
                shiftPosition[i] = 0;
            shiftPosition[_NumberOfAxis - 1] = shiftZ;

            ConvertCartesianShiftToScaraCoordinate(currentPosition[0], currentPosition[1], currentPosition[2],
                shiftX, shiftY, shiftAngle,
                out shiftPosition[0], out shiftPosition[1], out shiftPosition[2]);

            double[] dstPosition = new double[_NumberOfAxis];

            for (int i = 0; i < _NumberOfAxis; i++)
                dstPosition[i] = currentPosition[i] + shiftPosition[i];


            if (shiftPosition.Max() > 0 || shiftPosition.Min() < 0)
            {
                AbsoluteMove(Axis.J1, dstPosition[0], speedPercentage);
                AbsoluteMove(Axis.J2, dstPosition[1], speedPercentage);
                AbsoluteMove(Axis.J3, dstPosition[2], speedPercentage);
                AbsoluteMove(Axis.J4, dstPosition[3], speedPercentage);

                // SingleLinearMove(dstPosition, speedPercentage);
            }

            _LastScaraData.Clear();
        }
        /// <summary>derection == true : positive , derection == false : nagetive</summary>
        /// <param name="derection"></param>

        public void JogMove(Axis axis, double speedPercentage, bool direction)
        {
            if (CheckEncoderFail(axis))
                return;

            if (!_Valid)
                return;
            ClearIsStoppeStatus();

            speedPercentage = PowerPercent * 0.01 * speedPercentage;

            //double rpm, at;
            //ConvertSpeedPercentageToTruelySpeed(axis, speedPercentage, out rpm, out at);

            if (_IsAxisReverse[(int)axis])
                direction = !direction;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            moveSpeed._AccRate = moveSpeed._MaxSpd / (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
            moveSpeed._DecRate = moveSpeed._MaxSpd / (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
            _Axes[(int)axis].SetSpd(moveSpeed);

            _Axes[(int)axis].ConMv(direction);

            _LastScaraData.Clear();
        }

        public void HomeMove(Axis axis, uint returningSpeed, uint creepSpeed)
        {
            if (CheckEncoderFail(axis))
                return;

            if (!_Valid)
                return;
            ClearIsStoppeStatus();
            _Axes[(int)axis].HmMv(true);
        }
        /// <summary>
        /// Stop specific axis
        /// </summary>
        /// <param name="axis"></param>
        public void Stop(Axis axis)
        {
            if (!_Valid)
                return;
            _Axes[(int)axis].SdStop(false);

            _LastScaraData.Clear();
        }

        public void EmgStop(Axis axis)
        {
            if (!_Valid)
                return;
            _Axes[(int)axis].EmgStop();

            _LastScaraData.Clear();
        }

        /// <summary>Stop all axis</summary>
        public void StopAll()
        {
            if (!_Valid)
                return;
            for (int i = 0; i < _NumberOfAxis; i++)
                _Axes[i].EmgStop();

            _LastScaraData.Clear();
        }

        public void AxisShiftMove(Axis axis, EScaraPosition Position, double shift)
        {
            if (CheckEncoderFail(axis))
                return;

            double pos = MotorPositionArray[(int)Position].Value[(int)axis] + shift;

            if (axis == Axis.J3)
            {
                double speed = MotorPositionArray[(int)Position].SpeedPercentage;
                speed = speed * _Ratio_PulsePerMm[(int)Axis.J1] / _Ratio_PulsePerMm[(int)Axis.J3];
                AbsoluteMove(axis, pos, (ushort)speed);
                return;
            }
            AbsoluteMove(axis, pos, MotorPositionArray[(int)Position].SpeedPercentage);

            _LastScaraData.Clear();
        }

        public void AxisShiftMove(Axis axis, EScaraPosition Position, double shift, double speedPercentage)
        {
            if (CheckEncoderFail(axis))
                return;

            double pos = MotorPositionArray[(int)Position].Value[(int)axis] + shift;

            if (axis == Axis.J3)
            {
                double speed = speedPercentage;
                speed = speed * _Ratio_PulsePerMm[(int)Axis.J1] / _Ratio_PulsePerMm[(int)Axis.J3];
                AbsoluteMove(axis, pos, (ushort)speed);
                return;
            }
            AbsoluteMove(axis, pos, (ushort)speedPercentage);

            _LastScaraData.Clear();
        }

        public bool AxisShiftIsStopped(Axis axis, EScaraPosition Position, double shift, double tolerance)
        {
            if (CheckEncoderFail(axis))
                return false;

            double pos = GetPosition(axis);
            if (Math.Abs(pos - (MotorPositionArray[(int)Position].Value[(int)axis] + shift)) > tolerance)
                return false;

            if (!IsStopped(axis, true))
                return false;

            return true;

        }
        #endregion

        private bool CheckEncoderFail(Axis eAxis = Axis.Count)
        {
            bool bBuf = false;

            if (eAxis != Axis.Count)// 單軸檢測
            {
                if (_ServroDriver[(int)eAxis].IsReadEncoderFail())
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_AxisError, AlarmType.Alarm, eAxis.ToString() + " Read encoder fail");

                    bBuf = true;
                }
            }
            else// J1~J4全檢測
            {
                for (int i = 0; i < _ServroDriver.Length; i++)
                {
                    if (_ServroDriver[i].IsReadEncoderFail())
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_AxisError, AlarmType.Alarm, ((Axis)i).ToString() + " Read encoder fail");

                        if (!bBuf)
                            bBuf = true;
                    }
                }
            }

            return bBuf;
        }

        public double[] ConvertToCartesianCoordinatePostion(double[] position)
        {
            double[] pc = new double[(int)CartesianCoordinate.Count];
            ConvertScaraToCartesianCoordinate(position[0], position[1], position[2], out pc[0], out pc[1], out pc[3]);
            pc[2] = position[3];   //z = j4
            return pc;
        }

        public double GetDistance(double[] pointA, double[] pointB)
        {
            double calculate_abs;
            double calculate = 0;
            for (int i = 0; i < 3; i++)
            {
                calculate_abs = Math.Abs(pointA[i] - pointB[i]);
                calculate += Math.Pow(calculate_abs, 2);
            }
            double distance = Math.Pow(calculate, 0.5);
            return distance;
        }

        ///   pico linear move
        public void LinearMoveStart()
        {
            if (_Axes[0].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[0]).LinearMv();
            else if (_Axes[0].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[0]).LinearMv();
            ClearIsStoppeStatus();
        }

        public void LinearMoveWithoutAxisStart(int WithoutAxis)
        {
            if (_Axes[0].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[0]).LinearMvWithoutAxis(WithoutAxis);
            else if (_Axes[0].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[0]).LinearMvWithoutAxis(WithoutAxis);

            ClearIsStoppeStatus();
        }

        /// <summary>
        /// 連續移動，最多四點(含起始點)
        /// </summary>
        //public void SendGoArray(double[] ScaraPoints, int speedPercentage)
        //{
        //    if(IsStopped(true))
        //    {
        //        double[][] goDistanceArray = new double[_NumberOfAxis][];
        //        List<byte> goList = new List<byte>();

        //        double[][] points = { Gej1Toj4Position(), ScaraPoints };
        //        double t = GetMaxTime(points, speedPercentage);
        //        double[] timeArray = GetGoTimeArray(t, _AccTime[0], _DecTime[0], _SpiliteNum / 2);

        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            double dis = points[1][i] - points[0][i];
        //            goDistanceArray[i] = GetGoDistanceArray(t, _AccTime[0], _DecTime[0], dis, _SpiliteNum / 2);
        //        }
        //        double[][] goDistanceArray2 = new double[timeArray.Length][];
        //        for(int i = 0; i < goDistanceArray2.Length; i++)
        //        {
        //            goDistanceArray2[i] = new double[_NumberOfAxis];
        //            for (int j = 0; j < _NumberOfAxis; j++)
        //            {
        //                double dis = points[1][j] - points[0][j];
        //                goDistanceArray2[i][j] = goDistanceArray[j][i];
        //                if (dis < 0)
        //                    goDistanceArray2[i][j] *= -1;
        //            }
        //        }

        //        goList.AddRange(GetGoData(goDistanceArray2, timeArray));


        //        ((AxisGJS)_Axes[0]).SendLinearArray(goList.ToArray(), 1000);
        //        //LinearMoveStart();
        //        double[] scaraP = new double[_NumberOfAxis + 1];

        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            scaraP[i] = points[0][i];
        //        }
        //        scaraP[_NumberOfAxis] = 0;

        //        for (int i = 0; i < goDistanceArray2.Length; i++)
        //        {

        //            for (int j = 0; j < _NumberOfAxis; j++)
        //            {
        //                scaraP[j] += goDistanceArray2[i][j];
        //            }
        //            scaraP[_NumberOfAxis] = timeArray[i];

        //            _LastScaraData.Add((double[])scaraP.Clone());
        //        }
        //    }
        //    else
        //    {
        //        double[] CartesianOrg = new double[4];
        //        double[] CartesianMid = new double[4];
        //        double[] CartesianMid2 = new double[4];
        //        double[] CartesianDst = new double[4];


        //        double[] scaraOrg = new double[_NumberOfAxis];
        //        double[] scaraMid = new double[_NumberOfAxis];
        //        double[] scaraMid2 = new double[_NumberOfAxis];
        //        double[] scaraDst = new double[_NumberOfAxis];
        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            scaraOrg[i] = _LastScaraData[_LastScaraData.Count - 20][i];
        //            scaraMid[i] = _LastScaraData[_LastScaraData.Count - 1][i];
        //            scaraDst[i] = ScaraPoints[i];
        //            double dis = scaraOrg[i] - scaraMid[i];
        //            double dis2 = scaraDst[i] - scaraMid[i];
        //            if (Math.Abs(dis2) < 0.0001)
        //                scaraMid2[i] = scaraMid[i];
        //            else
        //                scaraMid2[i] = dis / dis2 + scaraMid[i];
        //        }



        //        ConvertScaraToCartesianCoordinate(scaraOrg[0], scaraOrg[1], scaraOrg[2], out CartesianOrg[0], out CartesianOrg[1], out CartesianOrg[3]);
        //        CartesianOrg[2] = scaraOrg[3];

        //        ConvertScaraToCartesianCoordinate(scaraMid[0], scaraMid[1], scaraMid[2], out CartesianMid[0], out CartesianMid[1], out CartesianMid[3]);
        //        CartesianMid[2] = scaraMid[3];

        //        ConvertScaraToCartesianCoordinate(scaraMid2[0], scaraMid2[1], scaraMid2[2], out CartesianMid2[0], out CartesianMid2[1], out CartesianMid2[3]);
        //        CartesianMid2[2] = scaraMid2[3];

        //        ConvertScaraToCartesianCoordinate(scaraDst[0], scaraDst[1], scaraDst[2], out CartesianDst[0], out CartesianDst[1], out CartesianDst[3]);
        //        CartesianDst[2] = scaraDst[3];

        //        Point3D circleP1 = new Point3D(CartesianOrg[0], CartesianOrg[1], CartesianOrg[2]);
        //        Point3D circleP2 = new Point3D(CartesianMid2[0], CartesianMid2[1], CartesianMid2[2]);
        //        Point3D midP = new Point3D(CartesianMid[0], CartesianMid[1], CartesianMid[2]);
        //        Point3D center = GetCenterOfCircle(circleP1, circleP2, midP);

        //        Point3D[] Cartesians = GetCirclePoints(center, circleP1, circleP2, midP, 30);

        //        double[][] linePoints = new double[Cartesians.Length][];
        //        double angle = CartesianMid2[3] - CartesianOrg[3];
        //        angle = angle / linePoints.Length;
        //        for (int i = 0; i < linePoints.Length; i++)
        //        {
        //            linePoints[i] = new double[_NumberOfAxis];
        //            linePoints[i][0] = Cartesians[i].X;
        //            linePoints[i][1] = Cartesians[i].Y;
        //            linePoints[i][2] = Cartesians[i].Z;
        //            linePoints[i][3] = angle * i + CartesianOrg[3];
        //        }

        //        List<byte> goList = new List<byte>();
        //        double[][] scaraPoints = GetScaraPoints((double[])scaraOrg.Clone(), linePoints);


        //        goList.AddRange(GetCircleData(scaraPoints, _LastScaraData[_LastScaraData.Count - 20][4]));



        //        double[][] goDistanceArray = new double[_NumberOfAxis][];
        //        double[][] points = { scaraPoints[scaraPoints.Length - 1], ScaraPoints };
        //        double t = GetMaxTime(points, speedPercentage);
        //        double[] timeArray = GetGoTimeArray(t, _AccTime[0], _DecTime[0], _SpiliteNum / 2);

        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            double dis = points[1][i] - points[0][i];
        //            goDistanceArray[i] = GetGoDistanceArray(t, _AccTime[0], _DecTime[0], dis, _SpiliteNum / 2);
        //        }
        //        double[][] goDistanceArray2 = new double[timeArray.Length][];
        //        for (int i = 0; i < goDistanceArray2.Length; i++)
        //        {
        //            goDistanceArray2[i] = new double[_NumberOfAxis];
        //            for (int j = 0; j < _NumberOfAxis; j++)
        //            {
        //                double dis = points[1][j] - points[0][j];
        //                goDistanceArray2[i][j] = goDistanceArray[j][i];
        //                if (dis < 0)
        //                    goDistanceArray2[i][j] *= -1;
        //            }
        //        }

        //        goList.AddRange(GetGoData(goDistanceArray2, timeArray));


        //        ((AxisGJS)_Axes[0]).SendLinearArray(goList.ToArray(), 1000);

        //        double[] scaraP = new double[_NumberOfAxis + 1];

        //        for (int i = 0; i < _NumberOfAxis; i++)
        //        {
        //            scaraP[i] = points[0][i];
        //        }
        //        scaraP[_NumberOfAxis] = 0;

        //        _LastScaraData.Clear();
        //        for (int i = 0; i < goDistanceArray2.Length; i++)
        //        {

        //            for (int j = 0; j < _NumberOfAxis; j++)
        //            {
        //                scaraP[j] += goDistanceArray2[i][j];
        //            }
        //            scaraP[_NumberOfAxis] = timeArray[i];

        //            _LastScaraData.Add((double[])scaraP.Clone());
        //        }
        //    }


        //    LinearMoveStart();
        //    System.Threading.Thread.Sleep(100);
        //}

        public void SendLinearArray(double[] ScaraOrg, double[] ScaraDst, int SpliteNum, double speedPercentage)
        {
            speedPercentage = PowerPercent * 0.01 * speedPercentage;

            double[] CartesianOrg = new double[4];
            double[] CartesianDst = new double[4];

            ConvertScaraToCartesianCoordinate(ScaraOrg[0], ScaraOrg[1], ScaraOrg[2], out CartesianOrg[0], out CartesianOrg[1], out CartesianOrg[3]);
            CartesianOrg[2] = ScaraOrg[3];

            ConvertScaraToCartesianCoordinate(ScaraDst[0], ScaraDst[1], ScaraDst[2], out CartesianDst[0], out CartesianDst[1], out CartesianDst[3]);
            CartesianDst[2] = ScaraDst[3];

            double[][] linePoints = SpiltPoints(CartesianOrg, CartesianDst, SpliteNum);
            double[][] scaraPoints = GetScaraPoints((double[])ScaraOrg.Clone(), linePoints);

            scaraPoints[0] = (double[])ScaraOrg.Clone();
            scaraPoints[scaraPoints.Length - 1] = (double[])ScaraDst.Clone();

            double distance = GetDistance(CartesianOrg, CartesianDst);
            double t = GetMaxTime(scaraPoints, speedPercentage);

            int accNum = 0;
            int decNum = 0;
            int maxVNum = 0;
            double maxVTime = 0;
            if (Math.Abs(distance) < 0.1)
                distance = 0.1;

            double[] distanceArray = GetDistanceArray_TCurve(t, (_LinearMoveAccTime * Math.Sqrt(speedPercentage * 0.01)), (_LinearMoveDecTime * Math.Sqrt(speedPercentage * 0.01)), distance, SpliteNum, ref accNum, ref decNum, ref maxVTime, ref maxVNum);

            //double dis = 0;
            //using (System.IO.StreamWriter strWriter = new System.IO.StreamWriter("C:\\t.txt", true))
            //{
            //    for (int i = 0; i < distanceArray.Length; i++)
            //    {
            //        strWriter.WriteLine(i.ToString() + ":" + distanceArray[i].ToString("0.00000"));
            //        dis += distanceArray[i];
            //    }
            //}
            //MessageBox.Show(distance.ToString("0.000")  + ":" + dis.ToString("0.000"));

            double[][] cartesianPoints = GetCartesianPoint(CartesianOrg, CartesianDst, distanceArray);
            double[][] scaraPoints2 = GetScaraPoints((double[])ScaraOrg.Clone(), cartesianPoints);
            scaraPoints2[0] = (double[])ScaraOrg.Clone();
            scaraPoints2[scaraPoints2.Length - 1] = (double[])ScaraDst.Clone();
            //double[] timeArray = GetTimeArray(scaraPoints, speedPercentage);
            byte[] linearData = GetLinearData(scaraPoints2, (_LinearMoveAccTime * Math.Sqrt(speedPercentage * 0.01)), accNum, (_LinearMoveDecTime * Math.Sqrt(speedPercentage * 0.01)), decNum, maxVTime, maxVNum);
            //double dis = 0;
            //using (System.IO.StreamWriter strWriter = new System.IO.StreamWriter("D:\\t.txt", true))
            //{
            //    for (int i = 0; i < distanceArray.Length; i++)
            //    {
            //        strWriter.WriteLine(i.ToString() + ":" + distanceArray[i].ToString("0.00000"));
            //        dis += distanceArray[i];
            //    }
            //}

            if (ScaraDst[0] > ScaraOrg[0])   //在J1做防呆
            {
                for (int i = 0; i < scaraPoints2.Length; i++)
                {
                    if (scaraPoints2[i][0] < ScaraOrg[0] - 1 || scaraPoints2[i][0] > ScaraDst[0] + 1)
                    {
                        throw new Exception("LinearArray Error.");
                    }
                }
            }
            else
            {
                for (int i = 0; i < scaraPoints2.Length; i++)
                {
                    if (scaraPoints2[i][0] < ScaraDst[0] - 1 || scaraPoints2[i][0] > ScaraOrg[0] + 1)
                    {
                        throw new Exception("LinearArray Error.");
                    }
                }
            }

            //MessageBox.Show(distance.ToString("0.000") + ":" + dis.ToString("0.000"));
            if (_Axes[0].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[0]).SendLinearArray(linearData, 1000);
            else if (_Axes[0].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[0]).SendLinearArray(linearData, 1000);
        }

        private byte[] GetCircleData(double[][] ScaraPoints, double IntervalTime)
        {
            byte[] array = new byte[(ScaraPoints.Length - 1) * 4 * 4];
            double sec = 0;

            double dis = 0;
            //using (System.IO.StreamWriter strWriter = new System.IO.StreamWriter("C:\\t.txt", true))
            //{

            int[] totalPulse = new int[4];
            int[] lastPulse = new int[4];
            double[] totalDistance = new double[4];

            for (int i = 0; i < ScaraPoints.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double distance = ScaraPoints[i + 1][j] - ScaraPoints[i][j];


                    totalDistance[j] += distance;
                    totalPulse[j] = _Axes[j].MmToPls(totalDistance[j]);
                    int pulse = totalPulse[j] - lastPulse[j];
                    lastPulse[j] = totalPulse[j];

                    if (_IsAxisReverse[j])
                        pulse *= -1;

                    sec = IntervalTime;

                    array[i * 4 * 4 + j * 4] = (byte)((Math.Abs(sec) * 1000.0 * 1000.0 / Math.Abs(pulse)) % 256);
                    array[i * 4 * 4 + j * 4 + 1] = (byte)((Math.Abs(sec) * 1000.0 * 1000.0 / Math.Abs(pulse)) / 256);

                    //if (j == 0)
                    //    strWriter.WriteLine(i.ToString() + "_" + j.ToString() + ":" + array[i * 4 * 4 + j * 4].ToString("0") + ":" + array[i * 4 * 4 + j * 4 + 1].ToString("0"));

                    pulse += 256 * 256 / 2;

                    array[i * 4 * 4 + j * 4 + 2] = (byte)((Math.Abs(pulse)) % 256);
                    array[i * 4 * 4 + j * 4 + 3] = (byte)((Math.Abs(pulse)) / 256);
                }
            }

            //}



            return array;
        }
        private byte[] GetLinearData(double[][] ScaraPoints, double AccTime, int AccNum, double DecTime, int DecNum, double MaxVTime, int MaxVNum)
        {
            byte[] array = new byte[(ScaraPoints.Length - 1) * 4 * 4];
            double sec = 0;

            double dis = 0;
            //using (System.IO.StreamWriter strWriter = new System.IO.StreamWriter("C:\\t.txt", true))
            //{

            int[] totalPulse = new int[4];
            int[] lastPulse = new int[4];
            double[] totalDistance = new double[4];

            for (int i = 0; i < ScaraPoints.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double distance = ScaraPoints[i + 1][j] - ScaraPoints[i][j];

                    totalDistance[j] += distance;
                    totalPulse[j] = _Axes[j].MmToPls(totalDistance[j]);
                    int pulse = totalPulse[j] - lastPulse[j];
                    lastPulse[j] = totalPulse[j];

                    if (_IsAxisReverse[j])
                        pulse *= -1;

                    if (i < AccNum)
                        sec = AccTime / AccNum;
                    else if (i < AccNum + MaxVNum)
                        sec = MaxVTime / MaxVNum;
                    else
                        sec = DecTime / DecNum;


                    array[i * 4 * 4 + j * 4] = (byte)((Math.Abs(sec) * 1000.0 * 1000.0 / Math.Abs(pulse)) % 256);
                    array[i * 4 * 4 + j * 4 + 1] = (byte)((Math.Abs(sec) * 1000.0 * 1000.0 / Math.Abs(pulse)) / 256);

                    //if (j == 0)
                    //    strWriter.WriteLine(i.ToString() + "_" + j.ToString() + ":" + array[i * 4 * 4 + j * 4].ToString("0") + ":" + array[i * 4 * 4 + j * 4 + 1].ToString("0"));

                    pulse += 256 * 256 / 2;

                    array[i * 4 * 4 + j * 4 + 2] = (byte)((Math.Abs(pulse)) % 256);
                    array[i * 4 * 4 + j * 4 + 3] = (byte)((Math.Abs(pulse)) / 256);
                }
            }
            //}
            return array;
        }
        private double[][] GetCartesianPoint(double[] CartesianOrg, double[] CartesianDst, double[] DistanceArray)
        {
            double TotalDistance = 0;
            for (int i = 0; i < DistanceArray.Length; i++)
                TotalDistance += DistanceArray[i];

            double[] CartesianDis = new double[4];
            for (int i = 0; i < CartesianDis.Length; i++)
                CartesianDis[i] = CartesianDst[i] - CartesianOrg[i];

            double[][] CartesianPoints = new double[DistanceArray.Length][];
            double dis = 0;
            for (int i = 0; i < DistanceArray.Length; i++)
            {
                CartesianPoints[i] = new double[4];
                dis += DistanceArray[i];
                for (int j = 0; j < CartesianDis.Length; j++)
                    CartesianPoints[i][j] = CartesianOrg[j] + CartesianDis[j] * dis / TotalDistance;
            }

            return CartesianPoints;
        }

        public double GetMoveTime(Axis axis, double speedPercentage, double distance)
        {
            speedPercentage = PowerPercent * 0.01 * speedPercentage;

            SpeedDef stSpeedVal = new SpeedDef();
            stSpeedVal._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            stSpeedVal._AccRate = stSpeedVal._MaxSpd / (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
            stSpeedVal._DecRate = stSpeedVal._MaxSpd / (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
            stSpeedVal._StartSpd = 0;
            stSpeedVal._EndSpd = 0;

            double accDistance = stSpeedVal._AccRate * (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01)) * (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01)) / 2;
            double decDistance = stSpeedVal._DecRate * (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01)) * (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01)) / 2;

            if (accDistance + decDistance >= Math.Abs(distance))
                return (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01)) + (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));

            return (Math.Abs(distance) - accDistance - decDistance) / stSpeedVal._MaxSpd + (_AccTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01)) + (_DecTime[(int)axis] * Math.Sqrt(speedPercentage * 0.01));
        }

        private double[][] SpiltPoints(double[] Org, double[] Dst, int SpliteNum)
        {
            double[][] Points = new double[SpliteNum][];
            for (int i = 0; i < SpliteNum; i++)
                Points[i] = new double[4];

            double[] interval = new double[4];
            for (int i = 0; i < 4; i++)
            {
                interval[i] = (Dst[i] - Org[i]) / (SpliteNum - 1);
            }

            for (int i = 0; i < SpliteNum; i++)
            {
                for (int j = 0; j < 4; j++)
                    Points[i][j] = Org[j] + interval[j] * i;
            }

            return Points;
        }

        private double[][] GetScaraPoints(double[] firstScaraPoint, double[][] CartesianPoints)
        {
            double[][] scaraPoints = new double[CartesianPoints.Length][];
            scaraPoints[0] = firstScaraPoint;
            double lastJ1 = scaraPoints[0][0];
            double lastJ3 = scaraPoints[0][2];


            for (int i = 1; i < CartesianPoints.Length; i++)
            {
                scaraPoints[i] = new double[4];
                double j1 = 0;
                double j2 = 0;
                double j3 = 0;
                ConvertCartesianToScaraCoordinate(CartesianPoints[i][0], CartesianPoints[i][1], CartesianPoints[i][3], out j1, out j2, out j3);

                if (i > 0)
                {
                    double shiftJ1 = j1 - lastJ1;
                    double shiftJ3 = j3 - lastJ3;
                    if (shiftJ1 > 180)
                        j1 -= 360;
                    if (shiftJ1 < -180)
                        j1 += 360;
                    if (shiftJ3 > 180)
                        j3 -= 360;
                    if (shiftJ3 < -180)
                        j3 += 360;
                }
                lastJ1 = j1;
                lastJ3 = j3;

                scaraPoints[i][0] = j1;
                scaraPoints[i][1] = j2;
                scaraPoints[i][2] = j3;
                scaraPoints[i][3] = CartesianPoints[i][2];
            }

            return scaraPoints;
        }

        private double[] GetPathDistance(double[][] ScaraPoints)
        {
            double[] sumDistance = new double[4];
            for (int i = 0; i < 4; i++)
                sumDistance[i] = 0;

            for (int i = 0; i < ScaraPoints.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                    sumDistance[j] += Math.Abs(ScaraPoints[i][j] - ScaraPoints[i + 1][j]);
            }

            return sumDistance;
        }

        private double GetMaxTime(double[][] ScaraPoints, double SpeedPercentage)
        {
            double[] sumDistance = GetPathDistance(ScaraPoints);

            double maxTime = 0;
            for (int i = 0; i < sumDistance.Length; i++)
            {
                double t = GetMoveTime((Axis)i, SpeedPercentage, sumDistance[i]);
                if (t > maxTime)
                    maxTime = t;
            }

            return maxTime;
        }

        public double[] GetGoTimeArray(double MaxTime, double AccTime, double DecTime, int HalfSpliteNum)
        {
            int accNum = HalfSpliteNum;
            int decNum = HalfSpliteNum;
            int maxVNum = 0;
            double maxVTime = 0;
            if (MaxTime > AccTime + DecTime)
            {
                maxVTime = MaxTime - AccTime - DecTime;
                maxVNum = 10;
            }

            double[] timeArray = new double[accNum + decNum + maxVNum];
            for (int i = 0; i < accNum; i++)
                timeArray[i] = AccTime / accNum;
            for (int i = accNum; i < maxVNum + accNum; i++)
                timeArray[i] = maxVTime / maxVNum;
            for (int i = accNum + maxVNum; i < maxVNum + accNum + decNum; i++)
                timeArray[i] = DecTime / decNum;

            return timeArray;
        }
        private double[] GetDistanceArray_TCurve(
            double MoveTime,
            double AccTime,
            double DecTime,
            double Distance,
            int SpliteNum,
            ref int AccNum, ref int DecNum, ref double MaxVTime, ref int MaxVNum)
        {
            double Vmax = 0;
            double accDistance = 0;
            double decDistance = 0;
            double vMaxDistance = 0;
            double maxVT = 0;
            if (MoveTime > (AccTime + DecTime))
            {
                maxVT = MoveTime - AccTime - DecTime;
                double temp = (0.5 * AccTime + 0.5 * DecTime + maxVT);
                Vmax = (Math.Abs(Distance)) / temp;
                accDistance = (Math.Abs(Distance)) * AccTime * 0.5 / temp;
                decDistance = (Math.Abs(Distance)) * DecTime * 0.5 / temp;
                vMaxDistance = (Math.Abs(Distance)) * maxVT / temp;
            }
            else
            {
                double temp = (0.5 * AccTime + 0.5 * DecTime);
                Vmax = (Math.Abs(Distance)) / temp;
                accDistance = (Math.Abs(Distance)) * AccTime * 0.5 / temp;
                decDistance = (Math.Abs(Distance)) * DecTime * 0.5 / temp;
            }

            int accNum = (int)(SpliteNum * AccTime / MoveTime);
            int decNum = (int)(SpliteNum * DecTime / MoveTime);
            int maxVNum = (int)(SpliteNum * maxVT / MoveTime);

            AccNum = accNum;
            DecNum = decNum;
            MaxVNum = maxVNum;
            MaxVTime = maxVT;
            double[] DistanceArray = new double[accNum + decNum + maxVNum];
            // acc time  ----------------
            double last = 0;
            double t = 0;
            double s1 = 0;

            double interval_t_ms = AccTime / (double)(accNum);
            double a = Vmax / (AccTime);

            for (int i = 0; i < accNum; i++)
            {
                t = interval_t_ms * (i + 1);

                s1 = t * t * a * 0.5;

                DistanceArray[i] = s1 - last;
                last += DistanceArray[i];
            }

            double MaxVSpliteInterval = 0;

            if (vMaxDistance > 0 && maxVNum > 0)
                MaxVSpliteInterval = (double)vMaxDistance / (double)(maxVNum); // 單位 pulse

            for (int i = 0; i < maxVNum; i++)
                DistanceArray[i + accNum] = MaxVSpliteInterval;

            // 單位 us
            s1 = 0;
            last = 0;
            interval_t_ms = DecTime / ((double)(decNum));

            double d = Vmax / DecTime;

            for (int i = 0; i < decNum; i++)
            {
                t = interval_t_ms * (i + 1);
                s1 = Vmax * t - d * t * t * 0.5;

                DistanceArray[i + accNum + maxVNum] = s1 - last;
                last += DistanceArray[i + accNum + maxVNum];
            }

            return DistanceArray;
        }
        private double[] GetDistanceArray(
            double MoveTime,
            double AccTime,
            double DecTime,
            double Distance,
            int SpliteNum,
            ref int AccNum, ref int DecNum, ref double MaxVTime, ref int MaxVNum)
        {
            double Vmax = 0;
            double accDistance = 0;
            double decDistance = 0;
            double vMaxDistance = 0;
            double maxVT = 0;
            if (MoveTime > (AccTime + DecTime))
            {
                maxVT = MoveTime - AccTime - DecTime;
                double temp = (0.5 * AccTime + 0.5 * DecTime + maxVT);
                Vmax = (Math.Abs(Distance)) / temp;
                accDistance = (Math.Abs(Distance)) * AccTime * 0.5 / temp;
                decDistance = (Math.Abs(Distance)) * DecTime * 0.5 / temp;
                vMaxDistance = (Math.Abs(Distance)) * maxVT / temp;
            }
            else
            {
                double temp = (0.5 * AccTime + 0.5 * DecTime);
                Vmax = (Math.Abs(Distance)) / temp;
                accDistance = (Math.Abs(Distance)) * AccTime * 0.5 / temp;
                decDistance = (Math.Abs(Distance)) * DecTime * 0.5 / temp;
            }

            int accNum = (int)(SpliteNum * AccTime / MoveTime);
            int decNum = (int)(SpliteNum * DecTime / MoveTime);
            int maxVNum = (int)(SpliteNum * maxVT / MoveTime);

            AccNum = accNum;
            DecNum = decNum;
            MaxVNum = maxVNum;
            MaxVTime = maxVT;
            double[] DistanceArray = new double[accNum + decNum + maxVNum];
            // acc time  ----------------
            double last = 0;
            double t = 0;
            double s1 = 0;
            double s2 = 0;

            int pulse = 0;
            double interval_t_ms = AccTime / (double)(accNum);
            double currentV = 0;
            double a1 = (Vmax) / (AccTime * AccTime) * 2;
            double a2 = (Vmax) / Math.Sqrt(AccTime * 2.0);

            for (int i = 0; i < accNum; i++)
            {
                t = interval_t_ms * (i + 1);
                if (t <= AccTime / 2)
                {
                    currentV = t * t * a1;
                    s1 = a1 * t * t * t / 3.0;
                }
                else
                {
                    s2 = (Vmax) / 2.0 * (t - AccTime / 2.0) + 2.0 / 3.0 * Math.Sqrt((t - AccTime / 2.0)) * (t - AccTime / 2) * a2;
                    currentV = (Vmax) / 2.0 + Math.Sqrt(t - AccTime / 2.0) * a2;
                }

                DistanceArray[i] = s1 + s2 - last;
                last += DistanceArray[i];
            }

            double MaxVSpliteInterval = 0;

            if (vMaxDistance > 0 && maxVNum > 0)
                MaxVSpliteInterval = (double)vMaxDistance / (double)(maxVNum); // 單位 pulse

            for (int i = 0; i < maxVNum; i++)
                DistanceArray[i + accNum] = MaxVSpliteInterval;

            // 單位 us
            s1 = 0;
            s2 = 0;
            last = 0;
            currentV = Vmax;
            interval_t_ms = DecTime / ((double)(decNum));

            double d1 = (Vmax) / (DecTime * DecTime) * 2;
            double d2 = (Vmax) / Math.Sqrt(DecTime * 2.0);

            //rintf("DecPulseCount %d \n",DecPulseCount);
            for (int i = 0; i < decNum; i++)
            {
                t = interval_t_ms * (i + 1);

                if (t <= DecTime / 2)
                {
                    s1 = Vmax * t - d1 * t * t * t / 3.0;
                    currentV = Vmax - t * t * d1;
                }
                else
                {
                    currentV = (Vmax) / 2.0 - Math.Sqrt((t - DecTime / 2.0)) * d2;
                    s2 = (Vmax) / 2.0 * (t - DecTime / 2.0) - 2.0 / 3.0 * Math.Sqrt((t - DecTime / 2.0)) * (t - DecTime / 2.0) * d2;
                }
                DistanceArray[i + accNum + maxVNum] = s1 + s2 - last;
                last += DistanceArray[i + accNum + maxVNum];
            }

            return DistanceArray;
        }

        private double[] GetGoDistanceArray(
            double MoveTime,
            double AccTime,
            double DecTime,
            double Distance,
            int HalfSpliteNum)
        {
            double Vmax = 0;
            double accDistance = 0;
            double decDistance = 0;
            double vMaxDistance = 0;
            double maxVT = 0;

            int accNum = HalfSpliteNum;
            int decNum = HalfSpliteNum;
            int maxVNum = 0;

            if (MoveTime > (AccTime + DecTime))
            {
                maxVT = MoveTime - AccTime - DecTime;
                double temp = (0.5 * AccTime + 0.5 * DecTime + maxVT);
                Vmax = (Math.Abs(Distance)) / temp;
                accDistance = (Math.Abs(Distance)) * AccTime * 0.5 / temp;
                decDistance = (Math.Abs(Distance)) * DecTime * 0.5 / temp;
                vMaxDistance = (Math.Abs(Distance)) * maxVT / temp;
                maxVNum = 10;
            }
            else
            {
                double temp = (0.5 * AccTime + 0.5 * DecTime);
                Vmax = (Math.Abs(Distance)) / temp;
                accDistance = (Math.Abs(Distance)) * AccTime * 0.5 / temp;
                decDistance = (Math.Abs(Distance)) * DecTime * 0.5 / temp;
            }




            int AccNum = accNum;
            int DecNum = decNum;
            int MaxVNum = maxVNum;
            double MaxVTime = maxVT;
            double[] distanceArray = new double[accNum + decNum + maxVNum];

            // acc time  ----------------
            double last = 0;
            double t = 0;
            double s1 = 0;
            double s2 = 0;


            double interval_t_ms = AccTime / (double)(accNum);
            double currentV = 0;
            double a1 = (Vmax) / (AccTime * AccTime) * 2;
            double a2 = (Vmax) / Math.Sqrt(AccTime * 2.0);

            for (int i = 0; i < accNum; i++)
            {

                t = interval_t_ms * (i + 1);
                if (t <= AccTime / 2)
                {
                    currentV = t * t * a1;
                    s1 = a1 * t * t * t / 3.0;
                }
                else
                {
                    s2 = (Vmax) / 2.0 * (t - AccTime / 2.0) + 2.0 / 3.0 * Math.Sqrt((t - AccTime / 2.0)) * (t - AccTime / 2) * a2;
                    currentV = (Vmax) / 2.0 + Math.Sqrt(t - AccTime / 2.0) * a2;
                }

                distanceArray[i] = s1 + s2 - last;
                last += distanceArray[i];

            }

            accDistance -= last;


            double MaxVSpliteInterval = 0;

            if (vMaxDistance > 0 && maxVNum > 0)
            {
                MaxVSpliteInterval = (double)vMaxDistance / (double)(maxVNum); // 單位 pulse
            }

            for (int i = 0; i < maxVNum; i++)
            {
                distanceArray[i + accNum] = MaxVSpliteInterval;
            }

            // 單位 us
            s1 = 0;
            s2 = 0;
            last = 0;
            currentV = Vmax;
            interval_t_ms = DecTime / ((double)(decNum));

            double d1 = (Vmax) / (DecTime * DecTime) * 2;
            double d2 = (Vmax) / Math.Sqrt(DecTime * 2.0);

            //rintf("DecPulseCount %d \n",DecPulseCount);
            for (int i = 0; i < decNum; i++)
            {
                t = interval_t_ms * (i + 1);

                if (t <= DecTime / 2)
                {
                    s1 = Vmax * t - d1 * t * t * t / 3.0;
                    currentV = Vmax - t * t * d1;
                }
                else
                {
                    currentV = (Vmax) / 2.0 - Math.Sqrt((t - DecTime / 2.0)) * d2;
                    s2 = (Vmax) / 2.0 * (t - DecTime / 2.0) - 2.0 / 3.0 * Math.Sqrt((t - DecTime / 2.0)) * (t - DecTime / 2.0) * d2;
                }


                distanceArray[i + accNum + maxVNum] = s1 + s2 - last;
                last += distanceArray[i + accNum + maxVNum];

            }

            decDistance -= last;
            distanceArray[decNum + accNum + maxVNum - 1] += decDistance + accDistance;

            return distanceArray;

        }

        private byte[] GetGoData(double[][] GoPointArray, double[] TimeArray)
        {
            if (GoPointArray.Length != TimeArray.Length)
            {
                return null;
            }
            byte[] array = new byte[(GoPointArray.Length) * 4 * 4];

            int[] totalPulse = new int[4];
            int[] lastPulse = new int[4];
            double[] totalDistance = new double[4];

            for (int i = 0; i < GoPointArray.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double distance = GoPointArray[i][j];
                    totalDistance[j] += distance;
                    totalPulse[j] = _Axes[j].MmToPls(totalDistance[j]);
                    int pulse = totalPulse[j] - lastPulse[j];
                    lastPulse[j] = totalPulse[j];

                    if (_IsAxisReverse[j])
                        pulse *= -1;

                    double sec = TimeArray[i];

                    array[i * 4 * 4 + j * 4] = (byte)((Math.Abs(sec) * 1000.0 * 1000.0 / Math.Abs(pulse)) % 256);
                    array[i * 4 * 4 + j * 4 + 1] = (byte)((Math.Abs(sec) * 1000.0 * 1000.0 / Math.Abs(pulse)) / 256);

                    pulse += 256 * 256 / 2;

                    array[i * 4 * 4 + j * 4 + 2] = (byte)((Math.Abs(pulse)) % 256);
                    array[i * 4 * 4 + j * 4 + 3] = (byte)((Math.Abs(pulse)) / 256);
                }
            }

            return array;
        }

        //private Point3D GetCenterOfCircle(Point3D CirclePoint1, Point3D CirclePoint2, Point3D TrianglePoint)
        //{
        //    Vector3 v1 = new Vector3((float)(CirclePoint1.X - TrianglePoint.X), (float)(CirclePoint1.Y - TrianglePoint.Y), (float)(CirclePoint1.Z - TrianglePoint.Z));
        //    Vector3 v2 = new Vector3((float)(CirclePoint2.X - TrianglePoint.X), (float)(CirclePoint2.Y - TrianglePoint.Y), (float)(CirclePoint2.Z - TrianglePoint.Z));
        //    Vector3 v3 = new Vector3((float)(CirclePoint2.X - CirclePoint1.X), (float)(CirclePoint2.Y - CirclePoint1.Y), (float)(CirclePoint2.Z - CirclePoint1.Z));



        //    Vector3 nV1V2 = Vector3.Cross(v1, v2);
        //    Vector3 nV3V2 = Vector3.Cross(v3, v2);
        //    if (nV1V2.LengthSquared() == 0)
        //        return CirclePoint1;
        //    double num2 = Vector3.Dot(nV3V2, nV1V2) / nV1V2.LengthSquared();

        //    Point3D intersection = new Point3D( CirclePoint1.X + v1.X * num2, CirclePoint1.Y + v1.Y * num2, CirclePoint1.Z + v1.Z * num2);
        //    return intersection;
        //}

        //private Point3D[] GetCirclePoints(Point3D CircleCenter, Point3D CirclePoint1, Point3D CirclePoint2, Point3D TrianglePoint, int SpliteNum)
        //{
        //    Vector3D v1 = new Vector3D(CirclePoint1.X - CircleCenter.X, CirclePoint1.Y - CircleCenter.Y, CirclePoint1.Z - CircleCenter.Z);
        //    Vector3D v2 = new Vector3D(CirclePoint2.X - CircleCenter.X, CirclePoint2.Y - CircleCenter.Y, CirclePoint2.Z - CircleCenter.Z);
        //    double angle = Vector3D.AngleBetween(v1, v2)/ SpliteNum;
        //    Vector3D vR = new Vector3D(CirclePoint1.X - CircleCenter.X, CirclePoint1.Y - CircleCenter.Y, CirclePoint1.Z - CircleCenter.Z);
        //    double r = vR.Length;
        //    //if(r == 0)
        //    {
        //        Point3D[] Points = new Point3D[SpliteNum];

        //        for (int i = 0; i < SpliteNum; i++)
        //        {
        //            Points[i] = Point3D.Subtract(CirclePoint2, CirclePoint1)/ SpliteNum*i + CirclePoint1;
        //        }

        //        return Points;
        //    }
        //    Vector3D nV1V2 = Vector3D.CrossProduct(v1, v2);
        //    nV1V2.Normalize();
        //    Point3D[] circlePoints = new Point3D[SpliteNum];

        //    for (int i = 0; i < SpliteNum; i++)
        //    {
        //        RotateTransform3D rotation = new RotateTransform3D(new AxisAngleRotation3D(nV1V2, angle*i), CircleCenter);
        //        circlePoints[i] = rotation.Transform(CirclePoint1);
        //    }

        //    return circlePoints;
        //}
    }
}
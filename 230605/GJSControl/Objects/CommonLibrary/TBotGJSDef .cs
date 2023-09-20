using FileStreamLibrary;
using System;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace CommonLibrary
{


    public struct TBotPosDef
    {
        public double[] _Value;
        public ushort _SpeedPercentage;

        public TBotPosDef(bool flg)
        {

            _Value = new double[2];
            _Value[0] = 0;
            _Value[1] = 0;
           

            _SpeedPercentage = 10;
        }

        public int GetAxisNum()
        {
            return 2;
        }
    }


    public class TBotGJSDef : IDisposable
    {
        public enum Axis
        {
            M1,
            M2,
            Count,
        }
        public enum CartesianCoordinate
        {
            Y,
            Z,
            Count
        }

        public readonly int NumberOfAxis = (int)Axis.Count;
        private readonly AxisBaseDef[] _Axes;

        private double[] _AccTime;
        private double[] _DecTime;
        private double[] _MaxSpeed;
        private bool[] _IsAxisReverse;

        public TBotPosDef[] _MotorPositionArray;
        public readonly bool _Valid;

        private double[] _TargetPosition;
        private double[] _PulsePerCircle;
        private string _FilePath;
        private Region _SafeArea;
        public bool EnableSafeArea;
        private int _TickCount;
        private int _StopDelayTime;
        private List<SerialPort> GJSSerialPort;
        public TBotGJSDef(List<SerialPort> ports, string FolderPath)
        {
            try
            {
                GJSSerialPort = ports;
                _FilePath = FolderPath + "\\TBot.ini";

                IniFile ini = new IniFile(_FilePath, true);

                EAXIS_TYPE axisType = (EAXIS_TYPE)Enum.Parse(typeof(EAXIS_TYPE), ini.ReadStr("System", "AxisType", EAXIS_TYPE.AxisGJS485.ToString()), true);

                if(axisType == EAXIS_TYPE.AxisGJS)
                    _Axes = new AxisGJS[NumberOfAxis];
                else if (axisType == EAXIS_TYPE.AxisGJS485)
                    _Axes = new AxisGJS485[NumberOfAxis];

                _MaxSpeed = new double[NumberOfAxis];
                _AccTime = new double[NumberOfAxis];
                _DecTime = new double[NumberOfAxis];
                _IsAxisReverse = new bool[NumberOfAxis];
                _PulsePerCircle = new double[NumberOfAxis];
                EnableSafeArea = false;
              
                _TickCount = Environment.TickCount;
                _MotorPositionArray = new TBotPosDef[(int)ETBotPosition.Count];
                for (int i = 0; i < (int)ETBotPosition.Count; i++)
                {
                    _MotorPositionArray[i] = new TBotPosDef(true);
                }

                Load();
                _Valid = true;
                _TargetPosition = new double[4];
                for (int i = 0; i < NumberOfAxis; i++)
                {
                    if (_Axes == null || _Axes[i] == null)
                    {
                        AlarmTextDisplay.Add(
                            (int)AlarmCode.Machine_RobotConnectionError,
                            AlarmType.Alarm,
                            "TBot " + "M" + i.ToString() + " initial failed");
                        _Valid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                _Valid = false;
            }
        }

        public void Dispose()
        {
            if (_Valid)
                _Axes[0].Dispose();
        }

        public void Save()
        {
            IniFile wriFileInfo = new IniFile(_FilePath, false);
            String sSection = "ETBotPosition";
            string str, str1;
            for (int i = 0; i < _MotorPositionArray.Count(); i++)
            {
                str = string.Join(",", _MotorPositionArray[i]._Value);
                str1 = string.Join(",", _MotorPositionArray[i]._SpeedPercentage);
                String sKeyFront = ((ETBotPosition)i).ToString();
                wriFileInfo.WriteStr(sSection, sKeyFront, str + "," + str1);
            }
            wriFileInfo.FileClose();
            wriFileInfo.Dispose();

        }

        private void Load()
        {

            if (!System.IO.File.Exists(_FilePath))
                return;


            String[] motorPos;
            motorPos = new string[_MotorPositionArray.Count()];

            IniFile cReaFileInfo = new IniFile(_FilePath, true);

            _StopDelayTime = (int)(cReaFileInfo.ReadDouble("System", "StopDelayTime", 0.1)*1000);
            EAXIS_TYPE axisType = (EAXIS_TYPE)Enum.Parse(typeof(EAXIS_TYPE), cReaFileInfo.ReadStr("System", "AxisType", EAXIS_TYPE.AxisGJS485.ToString()), true);
            for (int i = 0; i < NumberOfAxis; i++)
            {
                string section = "M" + (i + 1).ToString();
                
                string portName = cReaFileInfo.ReadStr(section, "Comport", "COM10");
                if (i == 0)
                {
                    bool getport = false;
                    for (int j = 0; j < GJSSerialPort.Count; j++)
                    {
                        if (GJSSerialPort[j] != null && GJSSerialPort[j].PortName == portName)
                        {
                            if (axisType == EAXIS_TYPE.AxisGJS)
                                _Axes[i] = new AxisGJS(GJSSerialPort[j], (byte)cReaFileInfo.ReadInt(section, "SlaveAddress", i));
                            else if (axisType == EAXIS_TYPE.AxisGJS485)
                                _Axes[i] = new AxisGJS485(GJSSerialPort[j], (byte)cReaFileInfo.ReadInt(section, "SlaveAddress", i), cReaFileInfo.ReadInt(section, "StationID", 1));
                            getport = true;
                        }


                    }

                    if (!getport)
                    {
                        if (axisType == EAXIS_TYPE.AxisGJS)
                        {
                            _Axes[i] = new AxisGJS(cReaFileInfo.ReadStr(section, "Comport", "COM10"),
                            115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One,
                            (byte)cReaFileInfo.ReadInt(section, "SlaveAddress", i));

                            GJSSerialPort.Add(((AxisGJS)_Axes[i]).GetSerialPort());
                        }
                        else if (axisType == EAXIS_TYPE.AxisGJS485)
                        {
                            _Axes[i] = new AxisGJS485(cReaFileInfo.ReadStr(section, "Comport", "COM10"),
                            115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One,
                            (byte)cReaFileInfo.ReadInt(section, "SlaveAddress", i),
                            cReaFileInfo.ReadInt(section, "StationID", 1));

                            GJSSerialPort.Add(((AxisGJS485)_Axes[i]).GetSerialPort());
                        }
                    }
                }
                else
                {
                    if (axisType == EAXIS_TYPE.AxisGJS)
                        _Axes[i] = new AxisGJS(((AxisGJS)_Axes[0]).GetSerialPort(), (byte)cReaFileInfo.ReadInt(section, "SlaveAddress", i));
                    else if (axisType == EAXIS_TYPE.AxisGJS485)
                        _Axes[i] = new AxisGJS485(((AxisGJS485)_Axes[0]).GetSerialPort(), (byte)cReaFileInfo.ReadInt(section, "SlaveAddress", i), cReaFileInfo.ReadInt(section, "StationID", 1));
                }

                _IsAxisReverse[i] = cReaFileInfo.ReadBool(section, "IsReverse", false);
                double PulsePerMm = cReaFileInfo.ReadDouble(section, "PulsePerMm", (double)1);
                double maxRPM = cReaFileInfo.ReadInt(section, "MaxRPM", 1000);
                _PulsePerCircle[i] = cReaFileInfo.ReadInt(section, "PulsePerCircle", 3000);
                _AccTime[i] = cReaFileInfo.ReadDouble(section, "Acc", 0.3);
                _DecTime[i] = cReaFileInfo.ReadDouble(section, "Dec", 0.3);
                _MaxSpeed[i] = maxRPM / 60.0 * _PulsePerCircle[i] * PulsePerMm;

                _Axes[i].SetPlsRto(PulsePerMm);
                _Axes[i].EnableEL(false);
            }

            _SafeArea = null;
          
            string strArea = cReaFileInfo.ReadStr("SafeArea", "Polygon", "");
            string[] strArray = strArea.Split(',');
            if (strArray.Length >= 6 && strArray.Length % 2 == 0)
            {
                PointF[] ptArreay = new PointF[strArray.Length / 2];
                for (int i = 0; i < ptArreay.Length; i++)
                {
                    ptArreay[i].X = Single.Parse(strArray[i * 2]);
                    ptArreay[i].Y = Single.Parse(strArray[i * 2 + 1]);
                }

                var path = new GraphicsPath();
                path.AddPolygon(ptArreay);

                _SafeArea = new Region(path);
            }



            String sSection = "ETBotPosition";
            string str, str1;
            for (int i = 0; i < _MotorPositionArray.Count(); i++)
            {
                str = string.Join(",", _MotorPositionArray[i]._Value);
                str1 = string.Join(",", _MotorPositionArray[i]._SpeedPercentage);
                String sKeyFront = ((ETBotPosition)i).ToString();
                motorPos[i] = cReaFileInfo.ReadStr(sSection, sKeyFront, str + "," + str1);

                String[] split = motorPos[i].Split(',');
                for (int j = 0; j < _MotorPositionArray[i]._Value.Count(); j++)
                {
                    _MotorPositionArray[i]._Value[j] = double.Parse(split[j]);
                }
                _MotorPositionArray[i]._SpeedPercentage = ushort.Parse(split[split.Count() - 1]);
            }

            cReaFileInfo.FileClose();
            cReaFileInfo.Dispose();
        }

        #region Get/Set

        private bool IsOnSafeArea(float X, float Z)
        {
            if (_SafeArea == null)
                return true;

            PointF Destination = new PointF(X, -Z);
            return _SafeArea.IsVisible(Destination);
        }
        public void SetZero()
        {
            _Axes[(int)Axis.M1].SetPos(0);
            _Axes[(int)Axis.M2].SetPos(0);
        }

        public double[] GetM1M2Position()
        {
            if (!_Valid)
                return new double[] { 0, 0 };
            double[] m1m2Position_mm = new double[NumberOfAxis];
            for (int i = 0; i < NumberOfAxis; i++)
            {
                m1m2Position_mm[i] = _IsAxisReverse[i] ?
                    _Axes[i].GetPos(0) * -1 :
                    _Axes[i].GetPos(0);
            }
            return m1m2Position_mm;
        }
        public double GetPosition(Axis axis)
        {
            if (_Axes == null || _Axes[(int)axis] == null)
                return 0;

            double position_mm = _IsAxisReverse[(int)axis] ?
                    _Axes[(int)axis].GetPos(0) * -1 :
                    _Axes[(int)axis].GetPos(0);
            return position_mm;
        }

        /// <summary>
        /// return X,Y,Z,Angle
        /// </summary>
        /// <returns></returns>
        public void ConvertCartesianCoordinateToMCoordinate(double xPos, double zPos, ref double M1, ref double M2)
        {
            double x = xPos, y = zPos;
            //double M1, M2;
            double theta = 45 * (Math.PI / 180);
            double cos_theta = Math.Cos(theta);
            double sin_theta = Math.Sin(theta);

            double[][] rotationMatrix = new double[][]
            {
                new double[] {cos_theta, -sin_theta},
                new double[] {sin_theta, cos_theta},
            };

            M1 = Math.Round((x * cos_theta - y * sin_theta) * Math.Pow(2, 0.5), 4);
            M2 = Math.Round((x * sin_theta + y * cos_theta) * Math.Pow(2, 0.5), 4);

        }
        public void ConvertMCoordinateToCartesianCoordinate(double M1, double M2, ref double xPos, ref double zPos)
        {
            //double M1, M2;
            //double x = xPos, y = yPos;
            double theta = -45 * (Math.PI / 180);
            double cos_theta = Math.Cos(theta);
            double sin_theta = Math.Sin(theta);

            xPos = Math.Round((M1 * cos_theta - M2 * sin_theta) / Math.Pow(2, 0.5), 4);
            zPos = Math.Round((M1 * sin_theta + M2 * cos_theta) / Math.Pow(2, 0.5), 4);

        }

        public bool IsStopped(bool defaultStatus)
        {
            if (!_Valid)
                return false;

            for (int i = 0; i < NumberOfAxis; i++)
            {
                if (!_Axes[i].Stop(true))
                {
                    _TickCount = Environment.TickCount;
                    return false;
                }
            }

            if(Environment.TickCount - _TickCount < _StopDelayTime)
            {
                return false;
            }

            return true;
        }


        public bool IsStopped(Axis axis, bool defaultStatus)
        {
            if (!_Valid)
                return defaultStatus;
            bool s = _Axes[(int)axis].Stop(true);
            return s;
        }

        public bool InPosition(ETBotPosition EPos, double Tolerance)
        {
            if (!_Valid)
                return true;

            double[] pos = GetM1M2Position();
            for (int i = 0; i < NumberOfAxis; i++)
            {
                if (Math.Abs(pos[i] - _MotorPositionArray[(int)EPos]._Value[i]) > Tolerance)
                {
                    return false;
                }
            }

            return true;
        }



        public void SetCurrentPosition(ETBotPosition position)
        {
            if (_Axes[0].GetType() == typeof(AxisGJS))
            {
                lock (((AxisGJS)_Axes[0]).GetSerialPort())
                {
                    double[] currentPosition = GetM1M2Position();
                    for (int i = 0; i < currentPosition.Length; i++)
                        _MotorPositionArray[(int)position]._Value[i] = currentPosition[i];
                    _MotorPositionArray[(int)position]._SpeedPercentage = 20;
                }
            }
            else if (_Axes[0].GetType() == typeof(AxisGJS485))
            {
                lock (((AxisGJS485)_Axes[0]).GetSerialPort())
                {
                    double[] currentPosition = GetM1M2Position();
                    for (int i = 0; i < currentPosition.Length; i++)
                        _MotorPositionArray[(int)position]._Value[i] = currentPosition[i];
                    _MotorPositionArray[(int)position]._SpeedPercentage = 20;
                }
            }
                
        }
        #endregion

        #region Move  (Use ClearIsStoppedStatus() first)



        public void Move(ETBotPosition Position)
        {
            if (EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                ConvertMCoordinateToCartesianCoordinate(
                    _MotorPositionArray[(int)Position]._Value[0],
                    _MotorPositionArray[(int)Position]._Value[1],
                    ref x,
                    ref z);
                if (!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }

            AxisMove(Axis.M1, Position);
            AxisMove(Axis.M2, Position);
        }

        public void Go(ETBotPosition Position)
        {
            if (EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                ConvertMCoordinateToCartesianCoordinate(
                    _MotorPositionArray[(int)Position]._Value[0],
                    _MotorPositionArray[(int)Position]._Value[1],
                    ref x,
                    ref z);
                if (!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }

            RelativeGo(GetM1M2Position(), _MotorPositionArray[(int)Position]._Value, _MotorPositionArray[(int)Position]._SpeedPercentage);
        }


        public void ContinueGo(ETBotPosition Position)
        {
            if (EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                ConvertMCoordinateToCartesianCoordinate(
                    _MotorPositionArray[(int)Position]._Value[0],
                    _MotorPositionArray[(int)Position]._Value[1],
                    ref x,
                    ref z);
                if (!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }

            RelativeGo(GetM1M2Position(), _MotorPositionArray[(int)Position]._Value, _MotorPositionArray[(int)Position]._SpeedPercentage);
        }


        //public void AbsContinueGo(ETBotPosition Position)
        //{
        //    if (EnableSafeArea)
        //    {
        //        double x = 0;
        //        double z = 0;
        //        ConvertMCoordinateToCartesianCoordinate(
        //            _MotorPositionArray[(int)Position]._Value[0],
        //            _MotorPositionArray[(int)Position]._Value[1],
        //            ref x,
        //            ref z);
        //        if (!IsOnSafeArea((float)x, (float)z))
        //        {
        //            AlarmTextDisplay.Add(
        //                AlarmCode.Alarm_RobotOnDangerZone,
        //                AlarmType.Alarm);
        //            return;
        //        }
        //    }

        //    double[] orgPos = GetM1M2Position();
        //    double[] sumDistance = new double[NumberOfAxis];
        //    for (int i = 0; i < sumDistance.Length; i++)
        //    {
        //        sumDistance[i] = _MotorPositionArray[(int)Position]._Value[i] - orgPos[i];
        //    }

        //    double maxTime = 0;

        //    for (int i = 0; i < sumDistance.Length; i++)
        //    {
        //        double t = GetMoveTime((Axis)i, _MotorPositionArray[(int)Position]._SpeedPercentage, Math.Abs(sumDistance[i]));
        //        if (t > maxTime)
        //            maxTime = t;
        //    }

        //    for (int i = 0; i < sumDistance.Length; i++)
        //    {
        //        sumDistance[i] = _IsAxisReverse[i] ? sumDistance[i] * -1 : sumDistance[i];
        //    }


        //    double pos0 = _IsAxisReverse[0] ? -_MotorPositionArray[(int)Position]._Value[0] : _MotorPositionArray[(int)Position]._Value[0];
        //    double pos1 = _IsAxisReverse[1] ? -_MotorPositionArray[(int)Position]._Value[1] : _MotorPositionArray[(int)Position]._Value[1];

        //    ((AxisGJS)_Axes[0]).SynMove2AxisByTime(
        //        _Axes[0].GetAxisIndex(),
        //        _Axes[1].GetAxisIndex(),
        //        pos0,
        //        pos1,
        //        maxTime, 
        //        _AccTime[0], 
        //        _DecTime[0]);

        //    ((AxisGJS)_Axes[0]).Run2Axis(0, 1);
        //}

        public void ShiftMove(ETBotPosition Position, double[] shift)
        { 
            if(EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                ConvertMCoordinateToCartesianCoordinate(
                    _MotorPositionArray[(int)Position]._Value[0] + shift[0],
                    _MotorPositionArray[(int)Position]._Value[1] + shift[1],
                    ref x,
                    ref z);
                if(!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }
            AxisShiftMove(Axis.M1, Position, shift[0]);
            AxisShiftMove(Axis.M2, Position, shift[1]);
        }

        private void AxisMove(Axis axis, ETBotPosition Position)
        {
            AbsoluteMove(axis, _MotorPositionArray[(int)Position]._Value[(int)axis], _MotorPositionArray[(int)Position]._SpeedPercentage);
        }

        private void AxisMove(Axis axis, ETBotPosition Position, double speedPercentage)
        {
            if (EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                ConvertMCoordinateToCartesianCoordinate(
                    _MotorPositionArray[(int)Position]._Value[0],
                    _MotorPositionArray[(int)Position]._Value[1],
                    ref x,
                    ref z);
                if (!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }
            AbsoluteMove(axis, _MotorPositionArray[(int)Position]._Value[(int)axis], (ushort)speedPercentage);
        }

        


        private void AbsoluteMove(Axis axis, double position, ushort speedPercentage)
        {
            if (!_Valid)
                return;
            
            position = _IsAxisReverse[(int)axis] ? position * -1 : position;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            moveSpeed._AccRate = moveSpeed._MaxSpd / _AccTime[(int)axis];
            moveSpeed._DecRate = moveSpeed._MaxSpd / _DecTime[(int)axis];

            _Axes[(int)axis].SetSpd(moveSpeed);

            _Axes[(int)axis].AbsMv(position);
        }

      

       
        public void RelativeGo(double[] OrgPos, double[] DstPos, ushort SpeedPercentage)
        {
            if (EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                ConvertMCoordinateToCartesianCoordinate(
                    DstPos[0],
                    DstPos[1],
                    ref x,
                    ref z);
                if (!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }

            double[] sumDistance = new double[NumberOfAxis];
            for(int i = 0; i < sumDistance.Length; i++)
            {
                sumDistance[i] = DstPos[i] - OrgPos[i];
            }

            double maxTime = 0;
         
            for (int i = 0; i < sumDistance.Length; i++)
            {
                double t = GetMoveTime((Axis)i, SpeedPercentage,Math.Abs( sumDistance[i]));
                if (t > maxTime)
                    maxTime = t;
            }

            for(int i = 0; i < NumberOfAxis; i++)
            {
                RelativeMoveByTime((Axis)i, sumDistance[i], maxTime);
            }

            if (_Axes[0].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[0]).Run2Axis(_Axes[0].GetAxisIndex(), _Axes[1].GetAxisIndex());
            else if (_Axes[0].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[0]).Run2Axis(_Axes[0].GetAxisIndex(), _Axes[1].GetAxisIndex());
        }


        private void RelativeMoveByTime(Axis axis, double distance, double MoveTime)
        {
            if (!_Valid)
                return;
            

            distance = _IsAxisReverse[(int)axis] ? distance * -1 : distance;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = 0;
            moveSpeed._AccRate = 0;
            moveSpeed._DecRate = 0;

            _Axes[(int)axis].SetSpd(moveSpeed);

            if (_Axes[(int)axis].GetType() == typeof(AxisGJS))
                ((AxisGJS)_Axes[(int)axis]).SetRelByTime(distance, MoveTime, _AccTime[(int)axis], _DecTime[(int)axis]);
            else if (_Axes[(int)axis].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_Axes[(int)axis]).SetRelByTime(distance, MoveTime, _AccTime[(int)axis], _DecTime[(int)axis]);
        }

        public void RelativeMove(double distance1, double distance2, ushort speedPercentage)
        {
            if (!_Valid)
                return;
            if (EnableSafeArea)
            {
                double x = 0;
                double z = 0;
                double[] DstPos = GetM1M2Position();
                ConvertMCoordinateToCartesianCoordinate(
                    DstPos[0] + distance1,
                    DstPos[1] + distance2,
                    ref x,
                    ref z);
                if (!IsOnSafeArea((float)x, (float)z))
                {
                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_RobotOnDangerZone,
                        AlarmType.Alarm);
                    return;
                }
            }

            RelativeMove(Axis.M1, distance1, speedPercentage);
            RelativeMove(Axis.M2, distance2, speedPercentage);
        }

        private void RelativeMove(Axis axis, double distance, ushort speedPercentage)
        {
            if (!_Valid)
                return;
          
            distance = _IsAxisReverse[(int)axis] ? distance * -1 : distance;

            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            moveSpeed._AccRate = moveSpeed._MaxSpd / _AccTime[(int)axis];
            moveSpeed._DecRate = moveSpeed._MaxSpd / _DecTime[(int)axis];

            _Axes[(int)axis].SetSpd(moveSpeed);

            _Axes[(int)axis].RelMv(distance);
        }

        //public void RelativeMove(double shiftY, double shiftZ,  ushort speedPercentage)
        //{
        //    if (!_Valid)
        //        return;

        //    double[] currentPosition = GetM1M2Position();
        //    double[] shiftPosition = new double[_NumberOfAxis];
        //    for (int i = 0; i < _NumberOfAxis; i++)
        //        shiftPosition[i] = 0;
        //    shiftPosition[_NumberOfAxis - 1] = shiftZ;

        //    ConvertMCoordinateToCartesianCoordinate(currentPosition[0], currentPosition[1],
        //        shiftY, shiftZ,
        //        out shiftPosition[0], out shiftPosition[1], out shiftPosition[2]);

        //    double[] dstPosition = new double[_NumberOfAxis];

        //    for (int i = 0; i < _NumberOfAxis; i++)
        //        dstPosition[i] = currentPosition[i] + shiftPosition[i];


        //    if (shiftPosition.Max() > 0 || shiftPosition.Min() < 0)
        //    {
        //        AbsoluteMove(Axis.J1, dstPosition[0], speedPercentage);
        //        AbsoluteMove(Axis.J2, dstPosition[1], speedPercentage);
        //        AbsoluteMove(Axis.J3, dstPosition[2], speedPercentage);
        //        AbsoluteMove(Axis.J4, dstPosition[3], speedPercentage);

        //        // SingleLinearMove(dstPosition, speedPercentage);
        //    }
        //}
        /// <summary>derection == true : positive , derection == false : nagetive</summary>
        /// <param name="derection"></param>

        public void JogMoveZ(bool Up, ushort speedPercentage)
        {
            if (!_Valid)
                return;

            double dis = 9999999;
            if (!Up)
                dis *= -1;
            double M1distance = 0;
            double M2distance = 0;
            ConvertCartesianCoordinateToMCoordinate(0, dis, ref M1distance, ref M2distance);
            RelativeMove(TBotGJSDef.Axis.M1, M1distance, speedPercentage);
            RelativeMove(TBotGJSDef.Axis.M2, M2distance, speedPercentage);

            //JogMove(Axis.M1, speedPercentage, !Up);
            //JogMove(Axis.M2, speedPercentage, Up);
        }

        public void JogMoveY(bool Forward, ushort speedPercentage)
        {
            if (!_Valid)
                return;

            double dis = 9999999;
            if (!Forward)
                dis *= -1;
            double M1distance = 0;
            double M2distance = 0;
            ConvertCartesianCoordinateToMCoordinate(dis, 0, ref M1distance, ref M2distance);
            RelativeMove(TBotGJSDef.Axis.M1, M1distance, speedPercentage);
            RelativeMove(TBotGJSDef.Axis.M2, M2distance, speedPercentage);

            //JogMove(Axis.M1, speedPercentage, Forward);
            //JogMove(Axis.M2, speedPercentage, Forward);
        }

        public void JogMove(Axis axis, ushort speedPercentage, bool direction)
        {
            if (!_Valid)
                return;
          
            //double rpm, at;
            //ConvertSpeedPercentageToTruelySpeed(axis, speedPercentage, out rpm, out at);

            if (_IsAxisReverse[(int)axis])
                direction = !direction;


            SpeedDef moveSpeed = new SpeedDef();
            moveSpeed._EndSpd = 0;
            moveSpeed._StartSpd = 0;
            moveSpeed._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            moveSpeed._AccRate = moveSpeed._MaxSpd / _AccTime[(int)axis];
            moveSpeed._DecRate = moveSpeed._MaxSpd / _DecTime[(int)axis];
            _Axes[(int)axis].SetSpd(moveSpeed);

            _Axes[(int)axis].ConMv(direction);
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
        }

        /// <summary>Stop all axis</summary>
        public void StopAll()
        {
            if (!_Valid)
                return;
            for (int i = 0; i < NumberOfAxis; i++)
                _Axes[i].SdStop(false);
        }

        public void EmgStop()
        {
            if (!_Valid)
                return;
            for (int i = 0; i < NumberOfAxis; i++)
                _Axes[i].EmgStop();
        }

        private void AxisShiftMove(Axis axis, ETBotPosition Position, double shift)
        {
            double pos = _MotorPositionArray[(int)Position]._Value[(int)axis] + shift;

          
            AbsoluteMove(axis, pos, _MotorPositionArray[(int)Position]._SpeedPercentage);
        }

        private void AxisShiftMove(Axis axis, ETBotPosition Position, double shift, double speedPercentage)
        {
            double pos = _MotorPositionArray[(int)Position]._Value[(int)axis] + shift;

           
            AbsoluteMove(axis, pos, (ushort)speedPercentage);
        }

        private bool AxisShiftIsStopped(Axis axis, ETBotPosition Position, double shift, double tolerance)
        {
            double pos = GetPosition(axis);
            if (Math.Abs(pos - (_MotorPositionArray[(int)Position]._Value[(int)axis] + shift)) > tolerance)
                return false;

            if (!IsStopped(axis, true))
                return false;

            return true;

        }
        #endregion



        private double GetDistance(double[] pointA, double[] pointB, ref double distance)
        {
            double calculate_abs;
            double calculate = 0;
            for (int i = 0; i < 3; i++)
            {
                calculate_abs = Math.Abs(pointA[i] - pointB[i]);
                calculate += Math.Pow(calculate_abs, 2);
            }
            distance = Math.Pow(calculate, 0.5);
            return distance;
        }

        ///   pico linear move
       

        public double GetMoveTime(Axis axis, int speedPercentage, double distance)
        {
            SpeedDef stSpeedVal = new SpeedDef();
            stSpeedVal._MaxSpd = _MaxSpeed[(int)axis] * speedPercentage * 0.01;
            stSpeedVal._AccRate = stSpeedVal._MaxSpd / _AccTime[(int)axis];
            stSpeedVal._DecRate = stSpeedVal._MaxSpd / _DecTime[(int)axis];
            stSpeedVal._StartSpd = 0;
            stSpeedVal._EndSpd = 0;

            double accDistance = stSpeedVal._AccRate * _AccTime[(int)axis] * _AccTime[(int)axis] / 2;
            double decDistance = stSpeedVal._DecRate * _DecTime[(int)axis] * _DecTime[(int)axis] / 2;
            if (accDistance + decDistance >= Math.Abs(distance))
            {
                return _AccTime[(int)axis] + _DecTime[(int)axis];
            }

            return (Math.Abs(distance) - accDistance - decDistance) / stSpeedVal._MaxSpd + _AccTime[(int)axis] + _DecTime[(int)axis];
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

       

        private double[] GetPathDistance(double[][] ScaraPoints)
        {
            double[] sumDistance = new double[4];
            for (int i = 0; i < 4; i++)
                sumDistance[i] = 0;

            for (int i = 0; i < ScaraPoints.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sumDistance[j] += Math.Abs(ScaraPoints[i][j] - ScaraPoints[i + 1][j]);
                }
            }

            return sumDistance;
        }


        private double[] GetTimeArray(double[][] ScaraPoints, int SpeedPercentage)
        {
            double[] sumDistance = GetPathDistance(ScaraPoints);

            double maxTime = 0;
            int axisIndex = 0;
            for (int i = 0; i < sumDistance.Length; i++)
            {
                double t = GetMoveTime((Axis)i, SpeedPercentage, sumDistance[i]);
                if (t > maxTime)
                {
                    maxTime = t;
                    axisIndex = i;
                }
            }



            double maxVTime = maxTime - _AccTime[axisIndex] - _DecTime[axisIndex];
            double maxV = sumDistance[axisIndex] / (_AccTime[axisIndex] * 0.5 + _DecTime[axisIndex] * 0.5 + maxVTime);
            double acc = maxV / _AccTime[axisIndex];
            double dec = maxV / _DecTime[axisIndex];



            double accDistance = maxV * _AccTime[axisIndex] / 2;
            double decDistance = maxV * _DecTime[axisIndex] / 2;
            double vDistance = sumDistance[axisIndex] - accDistance - decDistance;
            if (vDistance < 0)
                vDistance = 0;


            double[] timeArray = new double[ScaraPoints.Length - 1];
            double totalDistance = 0;
            double currentV = 0;
            for (int i = 0; i < ScaraPoints.Length - 1; i++)
            {
                double distance = ScaraPoints[i + 1][axisIndex] - ScaraPoints[i][axisIndex];
                double sec = 0;

                totalDistance += Math.Abs(distance);

                if (totalDistance < accDistance)
                {
                    sec = (-2 * currentV + Math.Sqrt(4 * currentV * currentV + 8 * Math.Abs(distance) * acc)) / (2 * acc);
                    currentV += acc * sec;
                }
                else if (totalDistance < accDistance + vDistance)
                {
                    currentV = maxV;

                    sec = Math.Abs(distance) / maxV;
                }
                else
                {
                    sec = (2 * currentV - Math.Sqrt(4 * currentV * currentV - 8 * Math.Abs(distance) * dec)) / (2 * dec);
                    currentV -= dec * sec;
                }
              //  if (double.IsNaN(sec))
             //       sec = 1;
                timeArray[i] = sec;


            }

            return timeArray;
        }

        private byte[] GetLinearData(double[][] ScaraPoints, double[] TimeArray)
        {
            byte[] array = new byte[(ScaraPoints.Length - 1) * 4 * 4];

            for (int i = 0; i < ScaraPoints.Length - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double distance = ScaraPoints[i + 1][j] - ScaraPoints[i][j];

                    int pulse = _Axes[j].MmToPls(distance);

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
    }
}

using FileStreamLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace CommonLibrary
{
    public enum ESPEED_TYPE
    {
        Low,
        Mid,
        High,
        Home,
        SPEED_COUNT
    }
    public enum EAXIS_TYPE
    {
        AxisMrJeA,
        AxisGJS,
        AxisA6_ABS,
        AxisGJS485,
        AxisA6_ABS485,
        AxisSimulate,
        AxisTypeCount
    }

    public class MtnCtrlDef : IDisposable
    {
        private struct AxisA6Info
        {
            public PanasonicA6Def PanasonicA6;
            public EAXIS_NAME AxisGJS;
        }
        private String _FilePath;
        private object _objLock;
        private AxisBaseDef[] _AxisAry;
        private bool[] _AxisHomeDoneAry;          //記錄各軸是否復歸結束 
        private bool[] _AxisReadEncoderErrAry;

        private List<List<SpeedDef>> _SpdLst;
        private String[] _AxisNameAry;
        private bool[] _DirInverseAry;
        private bool[] _SoftELEnable;
        private bool[] _ELEnable;
        private double[] _SMEL;
        private double[] _SPEL;
        public bool IsValid { get; private set; }
        private List<AxisA6Info> _ServorDriverA6;

        private List<SerialPort> GJSSerialPort;

        public MtnCtrlDef(List<SerialPort> comport, String sFName)
        {
            IsValid = true;
            _FilePath = sFName + "\\MotionList.ini";
            _objLock = new object();
            GJSSerialPort = comport;
            _ServorDriverA6 = new List<AxisA6Info>();

            _SpdLst = new List<List<SpeedDef>>();
            for (int i = 0; i < (int)ESPEED_TYPE.SPEED_COUNT; i++)
            {
                List<SpeedDef> stSpdLst = new List<SpeedDef>();
                for (int j = 0; j < (int)EAXIS_NAME.Count; j++)
                    stSpdLst.Add(new SpeedDef());
                _SpdLst.Add(stSpdLst);
            }

            _AxisNameAry = new string[(int)EAXIS_NAME.Count];
            _AxisAry = new AxisBaseDef[(int)EAXIS_NAME.Count];
            _DirInverseAry = new bool[(int)EAXIS_NAME.Count];
            _SoftELEnable = new bool[(int)EAXIS_NAME.Count];
            _SMEL = new double[(int)EAXIS_NAME.Count];
            _SPEL = new double[(int)EAXIS_NAME.Count];
            _ELEnable = new bool[(int)EAXIS_NAME.Count];
            _AxisHomeDoneAry = new bool[(int)EAXIS_NAME.Count];
            _AxisReadEncoderErrAry = new bool[(int)EAXIS_NAME.Count];
            for (int i = 0; i < (int)EAXIS_NAME.Count; i++)
            {
                _SMEL[i] = 0;
                _SPEL[i] = 0;
                _ELEnable[i] = true;
                _SoftELEnable[i] = false;
                _AxisHomeDoneAry[i] = false;
                _AxisReadEncoderErrAry[i] = false;
            }
            ReadMotionFile();
            Initial();
        }

        ~MtnCtrlDef() { }

        public void Dispose()
        {
            for (int i = 0; i < _AxisAry.Count(); i++)
                if (_AxisAry[i] != null)
                    _AxisAry[i].SetServoOn(false);
            _AxisHomeDoneAry = null;
            _SpdLst.Clear();
            for (int i = 0; i < _AxisAry.Count(); i++)
                if (_AxisAry[i] != null)
                    _AxisAry[i].Dispose();
            _objLock = null;
        }
        private void Initial()
        {
            for (int i = 0; i < _AxisAry.Count(); i++)
            {
                if (_AxisAry[i] != null)
                    _AxisAry[i].SetServoOn(true);
            }
        }

        #region Axis
        public int GetAxisNum() { return (int)EAXIS_NAME.Count; }
        public String GetAxisName(EAXIS_NAME eAxisType) { return _AxisNameAry[(int)eAxisType]; }

        public bool GetAlm(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (_AxisAry[(int)eAxisName] == null || !_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.ALM);
        }

        public bool GetOrg(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.ORG);
        }

        public bool GetEmg(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.EMG);
        }

        /// <summary>正極限</summary>
        public bool GetPEL(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            if (_DirInverseAry[(int)eAxisName])
                return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.MEL);

            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.PEL);
        }

        /// <summary>負極限</summary>
        public bool GetMEL(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            if (_DirInverseAry[(int)eAxisName])
                return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.PEL);

            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.MEL);
        }

        /// <summary>軟體正極限</summary>
        public bool GetSoftPEL(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            if (_DirInverseAry[(int)eAxisName])
                return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.SoftMEL);

            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.SoftPEL);
        }

        /// <summary>軟體負極限</summary>
        public bool GetSoftMEL(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            if (_DirInverseAry[(int)eAxisName])
                return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.SoftPEL);

            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.SoftMEL);
        }

        public bool GetINP(EAXIS_NAME eAxisName, bool bValNow)
        {
            if (!_AxisAry[(int)eAxisName].DeviceValid())
                return bValNow;
            return _AxisAry[(int)eAxisName].GetMtnIOStatus(EMOTION_IO.INP);
        }

        public void SetServoOn(EAXIS_NAME eAxisName, bool bOn) { _AxisAry[(int)eAxisName].SetServoOn(bOn); }
        public void SetHmVel(EAXIS_NAME eAxisName, double fVel) { _AxisAry[(int)eAxisName].SetHomeMaxVel(fVel); }
        public double GetHmVel(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetHomeMaxVel(); }
        public void SetHmMode(EAXIS_NAME eAxisName, AxisBaseDef.EHOME_MODE eMode) { _AxisAry[(int)eAxisName].SetHomeMode(eMode); }
        public AxisBaseDef.EHOME_MODE GetHmMode(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetHomeMode(); }
        public void SetEncoderRes(EAXIS_NAME eAxisName, int nVal) { _AxisAry[(int)eAxisName].SetEncRes(nVal); }
        public int GetEncoderRes(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetEncRes(); }
        public void SetEGear(EAXIS_NAME eAxisName, int nVal) { _AxisAry[(int)eAxisName].SetEGear(nVal); }
        public int GetEGear(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetEGear(); }
        public void SetELMode(EAXIS_NAME eAxisName, AxisBaseDef.ESTOP_MODE eMode) { _AxisAry[(int)eAxisName].SetELMode(eMode); }
        public AxisBaseDef.ESTOP_MODE GetELMode(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetELMode(); }
        public void SetIptReverse(EAXIS_NAME eAxisName, bool bVal) { _AxisAry[(int)eAxisName].SetIPTReverse(bVal); }
        public bool GetIptReverse(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetIPTReverse(); }
        public void SetFbSrc(EAXIS_NAME eAxisName, AxisBaseDef.EFEEDBACK_SRC eSrc) { _AxisAry[(int)eAxisName].SetFeedbackSrc(eSrc); }
        public AxisBaseDef.EFEEDBACK_SRC GetFbSrc(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetFeedbackSrc(); }
        public void SetPlsOptMode(EAXIS_NAME eAxisName, AxisBaseDef.EPULSE_OUTPUT_MODE eMode) { _AxisAry[(int)eAxisName].SetPlsOptMode(eMode); }
        public AxisBaseDef.EPULSE_OUTPUT_MODE GetPlsOptMode(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetPlsOptMode(); }
        public void SetPlsIptMode(EAXIS_NAME eAxisName, AxisBaseDef.EPULSE_INPUT_MODE eMode) { _AxisAry[(int)eAxisName].SetPlsIptMode(eMode); }
        public AxisBaseDef.EPULSE_INPUT_MODE GetPlsIptMode(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetPlsIptMode(); }
        public void SetServoLogic(EAXIS_NAME eAxisName, AxisBaseDef.EACITVE_LOGIC eLgc) { _AxisAry[(int)eAxisName].SetServoLgc(eLgc); }
        public AxisBaseDef.EACITVE_LOGIC GetServoLogic(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetServoLgc(); }
        public void SetOrgLogic(EAXIS_NAME eAxisName, AxisBaseDef.EACITVE_LOGIC eLgc) { _AxisAry[(int)eAxisName].SetOrgLgc(eLgc); }
        public AxisBaseDef.EACITVE_LOGIC GetOrgLogic(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetOrgLgc(); }
        public void SetELLogic(EAXIS_NAME eAxisName, AxisBaseDef.EACITVE_LOGIC eLgc) { _AxisAry[(int)eAxisName].SetELLgc(eLgc); }
        public void EnableEL(EAXIS_NAME eAxisName, bool On) { _AxisAry[(int)eAxisName].EnableEL(On); }
        public AxisBaseDef.EACITVE_LOGIC GetELLogic(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetELLgc(); }
        public void SetAlmLogic(EAXIS_NAME eAxisName, AxisBaseDef.EACITVE_LOGIC eLgc) { _AxisAry[(int)eAxisName].SetAlmLgc(eLgc); }
        public AxisBaseDef.EACITVE_LOGIC GetAlmLogic(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetAlmLgc(); }

        public void SetSoftELEnable(EAXIS_NAME eAxisName, bool bEnb)
        {
            _SoftELEnable[(int)eAxisName] = bEnb;
            _AxisAry[(int)eAxisName].SetSoftELEnable(bEnb);
        }

        public void SetELEnable(EAXIS_NAME eAxisName, bool bEnb)
        {
            _ELEnable[(int)eAxisName] = bEnb;
            _AxisAry[(int)eAxisName].EnableEL(bEnb);
        }

        public bool GetELEnable(EAXIS_NAME eAxisName) { return _ELEnable[(int)eAxisName]; }

        public bool GetSoftELEnable(EAXIS_NAME eAxisName) { return _SoftELEnable[(int)eAxisName]; }
        public void SetSMELVal(EAXIS_NAME eAxisName, double fPos)
        {
            if (_DirInverseAry[(int)eAxisName])
                _AxisAry[(int)eAxisName].SetSPEL(fPos * -1);
            else
                _AxisAry[(int)eAxisName].SetSMEL(fPos);

            _SMEL[(int)eAxisName] = fPos;
        }
        public void SetSPELVal(EAXIS_NAME eAxisName, double fPos)
        {
            if (_DirInverseAry[(int)eAxisName])
                _AxisAry[(int)eAxisName].SetSMEL(fPos * -1);
            else
                _AxisAry[(int)eAxisName].SetSPEL(fPos);

            _SPEL[(int)eAxisName] = fPos;
        }
        public void ChangeSpeed(EAXIS_NAME eAxisName, ESPEED_TYPE Speed)
        {
            if (_AxisAry[(int)eAxisName].GetType() == typeof(AxisGJS))
                ((AxisGJS)_AxisAry[(int)eAxisName]).ChangeSpeed(_SpdLst[(int)Speed][(int)eAxisName]);
            else if (_AxisAry[(int)eAxisName].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_AxisAry[(int)eAxisName]).ChangeSpeed(_SpdLst[(int)Speed][(int)eAxisName]);
        }
        public void ChangeDistance(EAXIS_NAME eAxisName, double Distance)
        {
            if (_DirInverseAry[(int)eAxisName])
                Distance *= -1;

            if (_AxisAry[(int)eAxisName].GetType() == typeof(AxisGJS))
                ((AxisGJS)_AxisAry[(int)eAxisName]).ChangeDistance(Distance);
            else if (_AxisAry[(int)eAxisName].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_AxisAry[(int)eAxisName]).ChangeDistance(Distance);
        }
        public void ChangeDestination(EAXIS_NAME eAxisName, double Destination)
        {
            if (_DirInverseAry[(int)eAxisName])
                Destination *= -1;


            if (_AxisAry[(int)eAxisName].GetType() == typeof(AxisGJS))
                ((AxisGJS)_AxisAry[(int)eAxisName]).ChangeDestination(Destination);
            else if (_AxisAry[(int)eAxisName].GetType() == typeof(AxisGJS485))
                ((AxisGJS485)_AxisAry[(int)eAxisName]).ChangeDestination(Destination);
        }

        public double GetSMELVal(EAXIS_NAME eAxisName)
        {

            return _SMEL[(int)eAxisName];
        }

        public double GetSPELVal(EAXIS_NAME eAxisName)
        {

            return _SPEL[(int)eAxisName];
        }

        public void SetSpd(ESPEED_TYPE eSpdTyp, EAXIS_NAME eAxisName, SpeedDef stSpd) { _SpdLst[(int)eSpdTyp][(int)eAxisName] = stSpd; }
        public SpeedDef GetSpd(ESPEED_TYPE eSpdTyp, EAXIS_NAME eAxisName) { return _SpdLst[(int)eSpdTyp][(int)eAxisName]; }
        public void SetPlsRto(EAXIS_NAME eAxisName, double fRto) { _AxisAry[(int)eAxisName].SetPlsRto(fRto); }
        public double GetPlsRto(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetPlsRto(); }
        public int GetStepsPerCircle(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetStepsPerCircle(); }
        public int GetRatedRpm(EAXIS_NAME eAxisName) { return _AxisAry[(int)eAxisName].GetRatedRpm(); }

        public double GetRecomSpd(EAXIS_NAME eAxisName)
        {
            double _PerCircle = (double)GetStepsPerCircle(eAxisName);
            double _RatedRpm = (double)GetRatedRpm(eAxisName);

            if (_PerCircle == 0)
                _PerCircle = 3000.0;
            if (_RatedRpm == 0)
                _RatedRpm = 3000.0;

            return _AxisAry[(int)eAxisName].GetPlsRto() * (_RatedRpm / 60.0) * _PerCircle;
        }

        #region Move
        #region AbsMv
        public void AbsMv(EAXIS_NAME eAxisName, double fPos, ESPEED_TYPE eSpdTYp)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            lock (_objLock)
            {
                _AxisAry[(int)eAxisName].SetSpd(_SpdLst[(int)eSpdTYp][(int)eAxisName]);

                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].AbsMv(-fPos);
                else
                    _AxisAry[(int)eAxisName].AbsMv(fPos);
            }
        }
        public void ArcMv(EAXIS_NAME eAxisName1, EAXIS_NAME eAxisName2, double dst, double dst2, double center, double center2, short dir, ESPEED_TYPE speed)
        {
            if (ReadEncoderFail(eAxisName1) || ReadEncoderFail(eAxisName2))
                return;

            SpeedDef stSpeedVal = _SpdLst[(int)speed][(int)eAxisName2];

            if (_DirInverseAry[(int)eAxisName1])
            {
                dst = -dst;
                center = -center;
            }

            if (_DirInverseAry[(int)eAxisName2])
            {
                dst2 = -dst2;
                center2 = -center2;
            }

            _AxisAry[(int)eAxisName1].GroupAbsArc(_AxisAry[(int)eAxisName2].GetAxisIndex(), 1 / _AxisAry[(int)eAxisName2].GetPlsRto(), dst, dst2, center, center2, dir, stSpeedVal._MaxSpd);
        }
        public void AbsMv(EAXIS_NAME eAxisName, double fPos, ESPEED_TYPE eRefSpdType, double fSpd, double fAcc, double fDec)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            lock (_objLock)
            {
                SpeedDef stSpeedVal = _SpdLst[(int)eRefSpdType][(int)eAxisName];
                stSpeedVal._MaxSpd = fSpd;
                stSpeedVal._AccRate = fAcc;
                stSpeedVal._DecRate = fDec;
                _AxisAry[(int)eAxisName].SetSpd(stSpeedVal);

                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].AbsMv(-fPos);
                else
                    _AxisAry[(int)eAxisName].AbsMv(fPos);
            }
        }
        #endregion

        #region RelMv
        public void RelMv(EAXIS_NAME eAxisName, double fDis, ESPEED_TYPE eSpdTYp)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            lock (_objLock)
            {
                _AxisAry[(int)eAxisName].SetSpd(_SpdLst[(int)eSpdTYp][(int)eAxisName]);

                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].RelMv(-fDis);
                else
                    _AxisAry[(int)eAxisName].RelMv(fDis);
            }
        }
        public void RelMv(EAXIS_NAME eAxisName, double fDis, ESPEED_TYPE eRefSpdType, double fSpd, double fAcc, double fDec)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            lock (_objLock)
            {
                SpeedDef stSpeedVal = _SpdLst[(int)eRefSpdType][(int)eAxisName];
                stSpeedVal._MaxSpd = fSpd;
                stSpeedVal._AccRate = fAcc;
                stSpeedVal._DecRate = fDec;
                _AxisAry[(int)eAxisName].SetSpd(stSpeedVal);

                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].RelMv(-fDis);
                else
                    _AxisAry[(int)eAxisName].RelMv(fDis);
            }
        }
        #endregion

        #region ConMv
        /// <summary>連續移動</summary>
        /// <param name="eAxisName"></param>
        /// <param name="bPosDir">False: 往負極限方向</param>
        /// <param name="eSpdTYp"></param>
        public void ConMv(EAXIS_NAME eAxisName, bool bPosDir, ESPEED_TYPE eSpdTYp)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            _AxisAry[(int)eAxisName].SetSpd(_SpdLst[(int)eSpdTYp][(int)eAxisName]);

            if (_DirInverseAry[(int)eAxisName])
                _AxisAry[(int)eAxisName].ConMv(!bPosDir);
            else
                _AxisAry[(int)eAxisName].ConMv(bPosDir);
        }
        /// <summary>連續移動</summary>
        /// <param name="eAxisName"></param>
        /// <param name="bPosDir">False: 往負極限方向</param>
        /// <param name="eSpdTypReference"></param>
        /// <param name="fSpdVal"></param>
        /// <param name="fAccVal"></param>
        /// <param name="fDecVal"></param>
        public void ConMv(EAXIS_NAME eAxisName, bool bPosDir, ESPEED_TYPE eSpdTypReference, double fSpdVal, double fAccVal, double fDecVal)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            lock (_objLock)
            {
                SpeedDef stSpeedVal = _SpdLst[(int)eSpdTypReference][(int)eAxisName];
                stSpeedVal._MaxSpd = fSpdVal;
                stSpeedVal._AccRate = fAccVal;
                stSpeedVal._DecRate = fDecVal;
                _AxisAry[(int)eAxisName].SetSpd(stSpeedVal);

                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].ConMv(!bPosDir);
                else
                    _AxisAry[(int)eAxisName].ConMv(bPosDir);
            }
        }
        #endregion

        #region HmMv
        public void HmMv(EAXIS_NAME eAxisName, bool bPosDir)
        {
            if (ReadEncoderFail(eAxisName))
                return;

            lock (_objLock)
            {
                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].HmMv(!bPosDir);
                else
                    _AxisAry[(int)eAxisName].HmMv(bPosDir);
            }
        }
        #endregion

        private bool ReadEncoderFail(EAXIS_NAME eAxisName)
        {
            if (_AxisReadEncoderErrAry[(int)eAxisName])
                AlarmTextDisplay.Add((int)AlarmCode.Alarm_AxisError, AlarmType.Alarm, eAxisName.ToString() + " Read encoder fail");

            return _AxisReadEncoderErrAry[(int)eAxisName];
        }
        #endregion

        public void ChangeSpeed(EAXIS_NAME eAxisName, ESPEED_TYPE eSpdTYp, double fPlusSpeed)
        {
            lock (_objLock)
            {
                _AxisAry[(int)eAxisName].SetSpd(_SpdLst[(int)eSpdTYp][(int)eAxisName]);
                _AxisAry[(int)eAxisName].ChangeSpeed(fPlusSpeed);
            }
        }

        public void ChangeTargetPos(EAXIS_NAME eAxisName, double fPos)
        {
            lock (_objLock)
            {
                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].ChangeTargetPos(-fPos);
                else
                    _AxisAry[(int)eAxisName].ChangeTargetPos(fPos);
            }
        }

        public bool Stop(EAXIS_NAME eAxisName, bool bStopNow)
        {
            bool bStop = false;
            lock (_objLock)
            {
                if (!_AxisAry[(int)eAxisName].DeviceValid())
                    return bStopNow;
                bStop = _AxisAry[(int)eAxisName].Stop(bStopNow);
            }
            return bStop;
        }

        public bool HomeDone(EAXIS_NAME eAxisName) { return _AxisHomeDoneAry[(int)eAxisName]; }
        public bool SetHomeDone(EAXIS_NAME eAxisName, bool bHomeDone) { return _AxisHomeDoneAry[(int)eAxisName] = bHomeDone; }

        public double GetPos(EAXIS_NAME eAxisName, double fPosNow)
        {
            lock (_objLock)
            {
                if (_DirInverseAry[(int)eAxisName])
                    return -_AxisAry[(int)eAxisName].GetPos(-fPosNow);
                else
                    return _AxisAry[(int)eAxisName].GetPos(fPosNow);
            }
        }

        public void SdStop(EAXIS_NAME eAxisName, bool all = false)
        {
            lock (_objLock)
            {
                if (_AxisAry[(int)eAxisName] != null)
                    _AxisAry[(int)eAxisName].SdStop(all);
            }
        }

        public void EmgStop(EAXIS_NAME eAxisName)
        {
            lock (_objLock)
            {
                _AxisAry[(int)eAxisName].EmgStop();
            }
        }

        public void SetPos(EAXIS_NAME eAxisName, double fPos)
        {
            lock (_objLock)
            {
                if (_DirInverseAry[(int)eAxisName])
                    _AxisAry[(int)eAxisName].SetPos(-fPos);
                else
                    _AxisAry[(int)eAxisName].SetPos(fPos);
            }
        }
        #endregion

        #region File
        public void WriteFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);

            String sSection = "Speed";
            for (int i = 0; i < _SpdLst.Count; i++)
            {
                for (int j = 0; j < _SpdLst[i].Count; j++)
                {
                    String sKeyFront = ((ESPEED_TYPE)i).ToString() + "_" + ((EAXIS_NAME)j).ToString() + "_";
                    iniFileInfo.WriteDouble(sSection, sKeyFront + "m_fStartSpd", (double)_SpdLst[i][j]._StartSpd);
                    iniFileInfo.WriteDouble(sSection, sKeyFront + "m_fMaxSpd", (double)_SpdLst[i][j]._MaxSpd);
                    iniFileInfo.WriteDouble(sSection, sKeyFront + "m_fEndSpd", (double)_SpdLst[i][j]._EndSpd);
                    iniFileInfo.WriteDouble(sSection, sKeyFront + "m_fAccRate", (double)_SpdLst[i][j]._AccRate);
                    iniFileInfo.WriteDouble(sSection, sKeyFront + "m_fDecRate", (double)_SpdLst[i][j]._DecRate);
                    iniFileInfo.WriteInt(sSection, sKeyFront + "m_nStblTm", _SpdLst[i][j]._StblTm);
                }
            }

            sSection = "Motion";
            for (int i = 0; i < (int)EAXIS_NAME.Count; i++)
            {
                String sKeyFront = ((EAXIS_NAME)i).ToString() + "_";
                iniFileInfo.WriteStr(sSection, sKeyFront + "Name", GetAxisName((EAXIS_NAME)i));

                iniFileInfo.WriteInt(sSection, sKeyFront + "AlmLogic", (int)GetAlmLogic((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "OrgLogic", (int)GetOrgLogic((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "ELLogic", (int)GetELLogic((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "ServoLogic", (int)GetServoLogic((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "ELMode", (int)GetELMode((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "FbSrc", (int)GetFbSrc((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "HmMode", (int)GetHmMode((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "PlsIptMode", (int)GetPlsIptMode((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "PlsOptMode", (int)GetPlsOptMode((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "EGear", (int)GetEGear((EAXIS_NAME)i));
                iniFileInfo.WriteInt(sSection, sKeyFront + "EncoderRes", (int)GetEncoderRes((EAXIS_NAME)i));
                iniFileInfo.WriteDouble(sSection, sKeyFront + "HmVel", (double)GetHmVel((EAXIS_NAME)i));
                iniFileInfo.WriteDouble(sSection, sKeyFront + "m_fPlsRto", (double)GetPlsRto((EAXIS_NAME)i));
                iniFileInfo.WriteDouble(sSection, sKeyFront + "SPEL", (double)GetSPELVal((EAXIS_NAME)i));
                iniFileInfo.WriteDouble(sSection, sKeyFront + "SMEL", (double)GetSMELVal((EAXIS_NAME)i));
                iniFileInfo.WriteBool(sSection, sKeyFront + "SoftELEnable", GetSoftELEnable((EAXIS_NAME)i));
                iniFileInfo.WriteBool(sSection, sKeyFront + "ELEnable", GetELEnable((EAXIS_NAME)i));

                iniFileInfo.WriteBool(sSection, sKeyFront + "IptReverse", GetIptReverse((EAXIS_NAME)i));
            }

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }

        public void ReadMotionFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, true);
            String sSection = "Speed";
            for (int i = 0; i < _SpdLst.Count; i++)
            {
                for (int j = 0; j < _SpdLst[i].Count; j++)
                {
                    SpeedDef stSpd = new SpeedDef();
                    String sKeyFront = ((ESPEED_TYPE)i).ToString() + "_" + ((EAXIS_NAME)j).ToString() + "_";
                    stSpd._StartSpd = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fStartSpd", (double)_SpdLst[i][j]._StartSpd);
                    stSpd._MaxSpd = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fMaxSpd", (double)_SpdLst[i][j]._MaxSpd);
                    stSpd._EndSpd = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fEndSpd", (double)_SpdLst[i][j]._EndSpd);
                    stSpd._AccRate = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fAccRate", (double)_SpdLst[i][j]._AccRate);
                    stSpd._DecRate = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fDecRate", (double)_SpdLst[i][j]._DecRate);
                    stSpd._StblTm = iniFileInfo.ReadInt(sSection, sKeyFront + "m_nStblTm", _SpdLst[i][j]._StblTm);

                    _SpdLst[i][j] = stSpd;
                }
            }

            sSection = "Motion";
            bool[] Initialed = new bool[(int)EAXIS_TYPE.AxisTypeCount];

            List<ModbusRTU> modbus = new List<ModbusRTU>();
            for (int i = 0; i < (int)EAXIS_NAME.Count; i++)
            {
                try
                {
                    String sKeyFront = ((EAXIS_NAME)i).ToString() + "_";
                    _AxisNameAry[i] = iniFileInfo.ReadStr(sSection, sKeyFront + "Name", ((EAXIS_NAME)i).ToString());
                    int axisNo = iniFileInfo.ReadInt(sSection, sKeyFront + "AxisNo", i);
                    string comport = iniFileInfo.ReadStr(sSection, sKeyFront + "Comport", "COM7");
                    EAXIS_TYPE axisType = (EAXIS_TYPE)Enum.Parse(typeof(EAXIS_TYPE), iniFileInfo.ReadStr(sSection, sKeyFront + "AxisType", EAXIS_TYPE.AxisSimulate.ToString()), false);
                    int stationID = iniFileInfo.ReadInt(sSection, sKeyFront + "StationID", 1);

                    if (axisType == EAXIS_TYPE.AxisMrJeA)
                    {
                        bool same = false;
                        for (int j = 0; j < modbus.Count; j++)
                        {
                            if (modbus[j].GetSerialPort().PortName.ToUpper() == comport.ToUpper())
                            {
                                _AxisAry[i] = new Axis_mr_je_a(modbus[j], (byte)axisNo);
                                same = true;
                            }
                        }

                        if (!same)
                        {
                            _AxisAry[i] = new Axis_mr_je_a(comport, 115200, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One, (byte)axisNo);
                            modbus.Add(_AxisAry[i].GetModbus());
                        }

                    }
                    else if (axisType == EAXIS_TYPE.AxisGJS)
                    {
                        bool same = false;
                        for (int j = 0; j < GJSSerialPort.Count; j++)
                        {
                            if (GJSSerialPort[j].PortName.ToUpper() == comport.ToUpper())
                            {
                                _AxisAry[i] = new AxisGJS(GJSSerialPort[j], (byte)axisNo);
                                same = true;
                            }
                        }

                        if (!same)
                        {
                            _AxisAry[i] = new AxisGJS(comport, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, (int)axisNo);
                            GJSSerialPort.Add(((AxisGJS)_AxisAry[i]).GetSerialPort());
                        }
                    }
                    else if (axisType == EAXIS_TYPE.AxisGJS485)
                    {
                        bool same = false;
                        for (int j = 0; j < GJSSerialPort.Count; j++)
                        {
                            if (GJSSerialPort[j].PortName.ToUpper() == comport.ToUpper())
                            {
                                _AxisAry[i] = new AxisGJS485(GJSSerialPort[j], (byte)axisNo, stationID);
                                same = true;
                            }
                        }

                        if (!same)
                        {
                            _AxisAry[i] = new AxisGJS485(comport, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, (int)axisNo, stationID);
                            GJSSerialPort.Add(((AxisGJS485)_AxisAry[i]).GetSerialPort());
                        }
                    }
                    else if (axisType == EAXIS_TYPE.AxisA6_ABS)
                    {
                        bool same = false;
                        for (int j = 0; j < GJSSerialPort.Count; j++)
                        {
                            if (GJSSerialPort[j].PortName.ToUpper() == comport.ToUpper())
                            {
                                _AxisAry[i] = new AxisGJS(GJSSerialPort[j], (byte)axisNo);
                                same = true;
                            }
                        }

                        if (!same)
                        {
                            _AxisAry[i] = new AxisGJS(comport, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, (int)axisNo);
                            GJSSerialPort.Add(((AxisGJS)_AxisAry[i]).GetSerialPort());
                        }

                        string _PanasonicA6COMPort = iniFileInfo.ReadStr(sSection, sKeyFront + "EncoderComport", "COM99");
                        int _StationNo = iniFileInfo.ReadInt(sSection, sKeyFront + "StationNo", 1);

                        AxisA6Info _A6Info = new AxisA6Info();
                        _A6Info.PanasonicA6 = new PanasonicA6Def(_PanasonicA6COMPort, _StationNo);
                        _A6Info.PanasonicA6.ZeroLap = iniFileInfo.ReadDouble(sSection, sKeyFront + "ZeroLap", 0);
                        _A6Info.PanasonicA6.PulsePerCircle = iniFileInfo.ReadDouble(sSection, sKeyFront + "PulsePerCircle", 3000);
                        _A6Info.PanasonicA6.Ratio_PulsePerMm = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fPlsRto", (double)1);
                        _A6Info.AxisGJS = (EAXIS_NAME)i;

                        _ServorDriverA6.Add(_A6Info);
                    }
                    else if (axisType == EAXIS_TYPE.AxisA6_ABS485)
                    {
                        bool same = false;
                        for (int j = 0; j < GJSSerialPort.Count; j++)
                        {
                            if (GJSSerialPort[j].PortName.ToUpper() == comport.ToUpper())
                            {
                                _AxisAry[i] = new AxisGJS485(GJSSerialPort[j], (byte)axisNo, stationID);
                                same = true;
                            }
                        }

                        if (!same)
                        {
                            _AxisAry[i] = new AxisGJS485(comport, 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One, (int)axisNo, stationID);
                            GJSSerialPort.Add(((AxisGJS485)_AxisAry[i]).GetSerialPort());
                        }

                        string _PanasonicA6COMPort = iniFileInfo.ReadStr(sSection, sKeyFront + "EncoderComport", "COM99");
                        int _StationNo = iniFileInfo.ReadInt(sSection, sKeyFront + "StationNo", 1);

                        AxisA6Info _A6Info = new AxisA6Info();
                        _A6Info.PanasonicA6 = new PanasonicA6Def(_PanasonicA6COMPort, _StationNo);
                        _A6Info.PanasonicA6.ZeroLap = iniFileInfo.ReadDouble(sSection, sKeyFront + "ZeroLap", 0);
                        _A6Info.PanasonicA6.PulsePerCircle = iniFileInfo.ReadDouble(sSection, sKeyFront + "PulsePerCircle", 3000);
                        _A6Info.PanasonicA6.Ratio_PulsePerMm = iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fPlsRto", (double)1);
                        _A6Info.AxisGJS = (EAXIS_NAME)i;

                        _ServorDriverA6.Add(_A6Info);
                    }
                    else if (axisType == EAXIS_TYPE.AxisSimulate)
                    {
                        _AxisAry[i] = new AxisSimulationDef();
                    }
                    else
                    {
                        _AxisAry[i] = new AxisSimulationDef();
                    }

                    _DirInverseAry[i] = iniFileInfo.ReadBool(sSection, sKeyFront + "DirInverse", false);
                    SetPlsRto((EAXIS_NAME)i, iniFileInfo.ReadDouble(sSection, sKeyFront + "m_fPlsRto", (double)GetPlsRto((EAXIS_NAME)i)));
                    SetAlmLogic((EAXIS_NAME)i, (AxisBaseDef.EACITVE_LOGIC)iniFileInfo.ReadInt(sSection, sKeyFront + "AlmLogic", (int)GetAlmLogic((EAXIS_NAME)i)));
                    SetOrgLogic((EAXIS_NAME)i, (AxisBaseDef.EACITVE_LOGIC)iniFileInfo.ReadInt(sSection, sKeyFront + "OrgLogic", (int)GetOrgLogic((EAXIS_NAME)i)));
                    SetELLogic((EAXIS_NAME)i, (AxisBaseDef.EACITVE_LOGIC)iniFileInfo.ReadInt(sSection, sKeyFront + "ELLogic", (int)GetELLogic((EAXIS_NAME)i)));
                    SetServoLogic((EAXIS_NAME)i, (AxisBaseDef.EACITVE_LOGIC)iniFileInfo.ReadInt(sSection, sKeyFront + "ServoLogic", (int)GetServoLogic((EAXIS_NAME)i)));
                    SetELMode((EAXIS_NAME)i, (AxisBaseDef.ESTOP_MODE)iniFileInfo.ReadInt(sSection, sKeyFront + "ELMode", (int)GetELMode((EAXIS_NAME)i)));
                    SetFbSrc((EAXIS_NAME)i, (AxisBaseDef.EFEEDBACK_SRC)iniFileInfo.ReadInt(sSection, sKeyFront + "FbSrc", (int)GetFbSrc((EAXIS_NAME)i)));
                    SetHmMode((EAXIS_NAME)i, (AxisBaseDef.EHOME_MODE)iniFileInfo.ReadInt(sSection, sKeyFront + "HmMode", (int)GetHmMode((EAXIS_NAME)i)));
                    SetPlsIptMode((EAXIS_NAME)i, (AxisBaseDef.EPULSE_INPUT_MODE)iniFileInfo.ReadInt(sSection, sKeyFront + "PlsIptMode", (int)GetPlsIptMode((EAXIS_NAME)i)));
                    SetPlsOptMode((EAXIS_NAME)i, (AxisBaseDef.EPULSE_OUTPUT_MODE)iniFileInfo.ReadInt(sSection, sKeyFront + "PlsOptMode", (int)GetPlsOptMode((EAXIS_NAME)i)));
                    SetEGear((EAXIS_NAME)i, iniFileInfo.ReadInt(sSection, sKeyFront + "EGear", (int)GetEGear((EAXIS_NAME)i)));
                    SetEncoderRes((EAXIS_NAME)i, iniFileInfo.ReadInt(sSection, sKeyFront + "EncoderRes", (int)GetEncoderRes((EAXIS_NAME)i)));
                    SetHmVel((EAXIS_NAME)i, iniFileInfo.ReadDouble(sSection, sKeyFront + "HmVel", (double)GetHmVel((EAXIS_NAME)i)));
                    SetSPELVal((EAXIS_NAME)i, iniFileInfo.ReadDouble(sSection, sKeyFront + "SPEL", (double)GetSPELVal((EAXIS_NAME)i)));
                    SetSMELVal((EAXIS_NAME)i, iniFileInfo.ReadDouble(sSection, sKeyFront + "SMEL", (double)GetSMELVal((EAXIS_NAME)i)));
                    SetSoftELEnable((EAXIS_NAME)i, iniFileInfo.ReadBool(sSection, sKeyFront + "SoftELEnable", GetSoftELEnable((EAXIS_NAME)i)));
                    SetELEnable((EAXIS_NAME)i, iniFileInfo.ReadBool(sSection, sKeyFront + "ELEnable", GetELEnable((EAXIS_NAME)i)));
                    SetIptReverse((EAXIS_NAME)i, iniFileInfo.ReadBool(sSection, sKeyFront + "IptReverse", GetIptReverse((EAXIS_NAME)i)));
                    #region GetAbsPositionFromPanasonicA6
                    CheckA6Battery((EAXIS_NAME)i);
                    ReSetA6AbsPosition((EAXIS_NAME)i);
                    #endregion
                }
                catch (Exception ex)
                {
                    IsValid = false;
                    _AxisAry[i] = new AxisSimulationDef();
                    var frame = (new StackTrace(ex, true)).GetFrame(0);
                    var className = frame.GetMethod().DeclaringType.FullName;
                    var methodName = frame.GetMethod().Name;

                    string msg = "ERROR CLASS : " + className + Environment.NewLine +
                        "ERROR FUNCTION : " + methodName + "()" + Environment.NewLine +
                        "ERROR KEY : " + ((EAXIS_NAME)i).ToString() + Environment.NewLine +
                        "ERROR CONTENT : " + Environment.NewLine + ex.ToString();

                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_AxisError, AlarmType.Alarm, msg);
                    MessageBox.Show(
                        msg,
                        this.GetType().Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }

        #endregion

        private void ReSetA6AbsPosition(EAXIS_NAME eAxis)
        {
            for (int i = 0; i < _ServorDriverA6.Count; i++)
            {
                if (_ServorDriverA6[i].AxisGJS == eAxis)
                {
                    double absLap = _ServorDriverA6[i].PanasonicA6.GetAbslap();
                    double pos = (absLap - _ServorDriverA6[i].PanasonicA6.ZeroLap) * _ServorDriverA6[i].PanasonicA6.PulsePerCircle * _ServorDriverA6[i].PanasonicA6.Ratio_PulsePerMm * -1;

                    _AxisReadEncoderErrAry[(int)eAxis] = _ServorDriverA6[i].PanasonicA6.IsReadEncoderFail();
                    _AxisAry[(int)eAxis].SetPos(pos);
                    break;
                }
            }
        }
        public void ReSetAllA6AbsPosition()
        {
            for (int i = 0; i < _ServorDriverA6.Count; i++)
            {
                double absLap = _ServorDriverA6[i].PanasonicA6.GetAbslap();
                double pos = (absLap - _ServorDriverA6[i].PanasonicA6.ZeroLap) * _ServorDriverA6[i].PanasonicA6.PulsePerCircle * _ServorDriverA6[i].PanasonicA6.Ratio_PulsePerMm * -1;

                _AxisReadEncoderErrAry[(int)(_ServorDriverA6[i].AxisGJS)] = _ServorDriverA6[i].PanasonicA6.IsReadEncoderFail();
                _AxisAry[(int)(_ServorDriverA6[i].AxisGJS)].SetPos(pos);
            }
        }
        private void CheckA6Battery(EAXIS_NAME eAxis)
        {
            for (int i = 0; i < _ServorDriverA6.Count; i++)
            {
                if (_ServorDriverA6[i].AxisGJS == eAxis)
                {
                    string errname = "";
                    if (_ServorDriverA6[i].PanasonicA6.GetBatteryStatus(ref errname))
                    {
                        LogDef.Add(ELogFileName.Alarm, "LowPower", eAxis.ToString(), errname);
                        AlarmTextDisplay.Add((int)AlarmCode.Warning, AlarmType.Warning, eAxis.ToString() + " " + errname);
                    }
                    break;
                }
            }
        }
        public void CheckAllA6Battery()
        {
            for (int i = 0; i < _ServorDriverA6.Count; i++)
            {
                string errname = "";
                if (_ServorDriverA6[i].PanasonicA6.GetBatteryStatus(ref errname))
                {
                    LogDef.Add(ELogFileName.Alarm, "LowPower", _ServorDriverA6[i].AxisGJS.ToString(), errname);
                    AlarmTextDisplay.Add((int)AlarmCode.Warning, AlarmType.Warning, _ServorDriverA6[i].AxisGJS.ToString() + " " + errname);
                }
            }
        }
    }
}
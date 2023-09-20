using FileStreamLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace CommonLibrary
{
    public enum EDIO_SingleEdge
    {
        /// <summary>上升緣</summary>
        RisingEdge,
        /// <summary>下降緣</summary>
        FallingEdge,
        On,
        Off,
        Nothing
    }
    public class IOCtrlDef : IDisposable
    {
        private struct DIOInfo
        {
            public MachineModules _Module;
            public String _Name;
            public int _ModID;
            public int _ModNo;
            public EDIOModuleType _ModuleType;
            public bool _BType;
        }
        private struct DIOModuleInfo
        {
            public int _ModID;
            public int _DINum;
            public int _DONum;
            public EDIOModuleType _ModuleType;
        }

        public enum EDIOModuleType
        {
            DIO_Dinkle,

            Count
        }
        private String _FileDIOPath;
        private DIOBaseDef _DIODinkle;

        private int _DIODinkleNum;

        private DIOModuleInfo[] _DIOModuleInfoAry;
        private DIOInfo[] _DIInfoAry;
        private DIOInfo[] _DOInfoAry;
        private bool[] _PreviousDOStatus;
        private bool[] _PreviousDIStatus;
        private bool[] _DIPass;
        private bool[] _DOPass;
        private int[] _DIHoldTickCount;
        private int[] _DOHoldTickCount;
        private object _objLock;
        public bool IsValid { get; private set; }

        public IOCtrlDef(string sFName)
        {
            IsValid = true;
            _FileDIOPath = sFName + "\\MotionList_DIO.ini";
            _objLock = new object();
            _DIInfoAry = new DIOInfo[(int)EDI_TYPE.DI_COUNT];
            _DIHoldTickCount = new int[(int)EDI_TYPE.DI_COUNT];
            _DIPass = new bool[(int)EDI_TYPE.DI_COUNT];
            _PreviousDIStatus = new bool[(int)EDI_TYPE.DI_COUNT];
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                _DIPass[i] = false;
                _PreviousDIStatus[i] = false;
            }

            _DOInfoAry = new DIOInfo[(int)EDO_TYPE.DO_COUNT];
            _DOHoldTickCount = new int[(int)EDO_TYPE.DO_COUNT];
            _DOPass = new bool[(int)EDO_TYPE.DO_COUNT];
            _PreviousDOStatus = new bool[(int)EDO_TYPE.DO_COUNT];
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                _DOPass[i] = false;
                _PreviousDOStatus[i] = false;
            }

            ReadDIOFile();
            try
            {
                if (_DIODinkleNum > 0)
                {
                    IniFile iniFileInfo = new IniFile(_FileDIOPath, true);
                    string com = iniFileInfo.ReadStr("DIOModInfo", "DinkleComport", "COM99");
                    iniFileInfo.FileClose();
                    iniFileInfo.Dispose();
                    int diNum = 0;
                    int doNum = 0;
                    for (int i = 0; i < _DIOModuleInfoAry.Length; i++)
                    {
                        diNum += _DIOModuleInfoAry[i]._DINum;
                        doNum += _DIOModuleInfoAry[i]._DONum;
                    }

                    _DIODinkle = new DIODinkleDef(com, 115200, Parity.None, 8, StopBits.One, (byte)0x01, (doNum / 16) + 1, (diNum / 16) + 1);
                    if (!_DIODinkle.DeviceValid())
                    {
                        _DIODinkle.Dispose();
                        _DIODinkle = null;
                        _DIODinkle = new cDIOSimulationDef();
                    }
                }
            }
            catch (Exception ex)
            {
                IsValid = false;
                var frame = (new StackTrace(ex, true)).GetFrame(0);
                var className = frame.GetMethod().DeclaringType.FullName;
                var methodName = frame.GetMethod().Name;

                string msg = "ERROR CLASS : " + className + Environment.NewLine +
                    "ERROR FUNCTION : " + methodName + "()" + Environment.NewLine +
                    "ERROR KEY : " + "None" + Environment.NewLine +
                    "ERROR CONTENT : " + Environment.NewLine + ex.ToString();

                AlarmTextDisplay.Add((int)AlarmCode.Alarm_IOError, AlarmType.Alarm, msg);

                MessageBox.Show(
                    msg,
                    this.GetType().Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void Dispose()
        {
            if (_DIODinkle != null)
                _DIODinkle.Dispose();
            _DIPass = null;
            _DOPass = null;
            _DIHoldTickCount = null;
            _DOHoldTickCount = null;
            _objLock = null;
        }

        #region DIO
        public int GetDINum() { return (int)EDI_TYPE.DI_COUNT; }
        public int GetDONum() { return (int)EDO_TYPE.DO_COUNT; }
        public String GetDIName(EDI_TYPE eDI)
        {
            if (eDI == EDI_TYPE.DI_COUNT)
                return "None";

            return _DIInfoAry[(int)eDI]._Name;
        }
        public String GetDOName(EDO_TYPE eDO) 
        {
            if (eDO == EDO_TYPE.DO_COUNT)
                return "None";

            return _DOInfoAry[(int)eDO]._Name; 
        }
        public MachineModules GetDIModule(EDI_TYPE eDI) { return _DIInfoAry[(int)eDI]._Module; }
        public MachineModules GetDOModule(EDO_TYPE eDO) { return _DOInfoAry[(int)eDO]._Module; }
        public void SetDIPass(EDI_TYPE eDITyp, bool bVal)
        {
            _DIPass[(int)eDITyp] = bVal;
        }
        public void SetDOPass(EDO_TYPE eDOType, bool bValue) { _DOPass[(int)eDOType] = bValue; }
        public bool GetDIPass(EDI_TYPE eDITyp) { return _DIPass[(int)eDITyp]; }
        public bool GetDOPass(EDO_TYPE eDOType) { return _DOPass[(int)eDOType]; }
        public int GetDIModId(EDI_TYPE eDITyp) { return _DIInfoAry[(int)eDITyp]._ModID; }
        public int GetDOModId(EDO_TYPE eDOTyp) { return _DOInfoAry[(int)eDOTyp]._ModID; }

        public void GetDIModId_ModNo(EDI_TYPE eDITyp, ref int nModId, ref int nModNo)
        {
            nModId = _DIInfoAry[(int)eDITyp]._ModID;
            nModNo = _DIInfoAry[(int)eDITyp]._ModNo;
        }
        public void GetDOModId_ModNo(EDO_TYPE eDOTyp, ref int nModId, ref int nModNo)
        {
            nModId = _DOInfoAry[(int)eDOTyp]._ModID;
            nModNo = _DOInfoAry[(int)eDOTyp]._ModNo;
        }

        /// <summary>取得DI保持後訊號</summary>
        public void ResetDIHoldTickCount(EDI_TYPE eDIType) { _DIHoldTickCount[(int)eDIType] = Environment.TickCount; }

        /// <summary>取得DI保持後訊號</summary>
        public bool GetDI_Hold(EDI_TYPE eDIType, bool bDINow, bool eHoldStatus = false, int eTick = 100)
        {
            bool _nowStatus = GetDI(eDIType, bDINow);

            if (_nowStatus == eHoldStatus)
            {
                if (Environment.TickCount - _DIHoldTickCount[(int)eDIType] >= eTick)
                    return true;
            }
            else
                _DIHoldTickCount[(int)eDIType] = Environment.TickCount;

            return false;
        }

        /// <summary>取得DI邊緣訊號</summary>
        public EDIO_SingleEdge GetDIEdge(EDI_TYPE eDIType, bool bDINow = false)
        {
            bool _nowStatus = GetDI(eDIType, bDINow);

            if (_PreviousDIStatus[(int)eDIType] != _nowStatus)
            {
                _PreviousDIStatus[(int)eDIType] = _nowStatus;

                if (_nowStatus)
                {
                    LogDef.Add(ELogFileName.Operate, eDIType.ToString(), eDIType.ToString(), "Off to On");
                    return EDIO_SingleEdge.RisingEdge;
                }
                else
                {
                    LogDef.Add(ELogFileName.Operate, eDIType.ToString(), eDIType.ToString(), "On To Off");
                    return EDIO_SingleEdge.FallingEdge;
                }
            }
            else if (_nowStatus)
                return EDIO_SingleEdge.On;
            else if (!_nowStatus)
                return EDIO_SingleEdge.Off;
            else
                return EDIO_SingleEdge.Nothing;
        }
        /// <summary>取得DO邊緣訊號</summary>
        public EDIO_SingleEdge GetDOEdge(EDO_TYPE eDOType, bool bDONow = false)
        {
            bool _nowStatus = GetDO(eDOType, bDONow);

            if (_PreviousDIStatus[(int)eDOType] != _nowStatus)
            {
                _PreviousDIStatus[(int)eDOType] = _nowStatus;

                if (_nowStatus)
                    return EDIO_SingleEdge.RisingEdge;
                else
                    return EDIO_SingleEdge.FallingEdge;
            }
            else if (_nowStatus)
                return EDIO_SingleEdge.On;
            else if (!_nowStatus)
                return EDIO_SingleEdge.Off;
            else
                return EDIO_SingleEdge.Nothing;
        }

        public bool GetDI(EDI_TYPE eDITyp, bool bDINow, ref string NameAndStatus, bool bReality = false)
        {
            if (eDITyp == EDI_TYPE.DI_COUNT)
                return bDINow;

            bool on = false;
            lock (_objLock)
            {
                if (_DIInfoAry[(int)eDITyp]._ModuleType == EDIOModuleType.DIO_Dinkle)
                {
                    if (_DIODinkle == null || !_DIODinkle.DeviceValid() || GetDIPass(eDITyp))
                        on = bDINow;

                    if (_DIInfoAry[(int)eDITyp]._BType && !bReality)
                        on = !_DIODinkle.GetDI(_DIInfoAry[(int)eDITyp]._ModID, _DIInfoAry[(int)eDITyp]._ModNo, bDINow);
                    else
                        on = _DIODinkle.GetDI(_DIInfoAry[(int)eDITyp]._ModID, _DIInfoAry[(int)eDITyp]._ModNo, bDINow);
                }//Dinkle

                on = false;
            }

            if (on)
                NameAndStatus = GetDIName(eDITyp) + ":On\n";
            else
                NameAndStatus = GetDIName(eDITyp) + ":Off\n";

            return on;
        }

        public bool GetDI(EDI_TYPE eDITyp, bool bDINow, bool bReality = false)
        {
            if (eDITyp == EDI_TYPE.DI_COUNT)
                return bDINow;

            lock (_objLock)
            {
                if (_DIInfoAry[(int)eDITyp]._ModuleType == EDIOModuleType.DIO_Dinkle)
                {
                    if (_DIODinkle == null || !_DIODinkle.DeviceValid() || GetDIPass(eDITyp))
                        return bDINow;

                    if (_DIInfoAry[(int)eDITyp]._BType && !bReality)
                        return !_DIODinkle.GetDI(_DIInfoAry[(int)eDITyp]._ModID, _DIInfoAry[(int)eDITyp]._ModNo, bDINow);
                    else
                        return _DIODinkle.GetDI(_DIInfoAry[(int)eDITyp]._ModID, _DIInfoAry[(int)eDITyp]._ModNo, bDINow);
                }//Dinkle

                return false;
            }
        }
        public bool GetDO(EDO_TYPE eDOTyp, bool bDONow)
        {
            if (eDOTyp == EDO_TYPE.DO_COUNT)
                return bDONow;

            if (_DOInfoAry[(int)eDOTyp]._ModuleType == EDIOModuleType.DIO_Dinkle)
            {
                if (_DIODinkle == null)
                    return bDONow;
                if (!_DIODinkle.DeviceValid())
                    return bDONow;

                return _DIODinkle.GetDO(_DOInfoAry[(int)eDOTyp]._ModID, _DOInfoAry[(int)eDOTyp]._ModNo, bDONow);
            }//Dinkle

            return false;
        }

        public void SetDO(EDO_TYPE eDOTyp, bool bDOVal)
        {
            if (eDOTyp == EDO_TYPE.DO_COUNT)
                return;

            if (_DOInfoAry[(int)eDOTyp]._ModuleType == EDIOModuleType.DIO_Dinkle)
            {
                if (_DIODinkle == null || _DOPass[(int)eDOTyp])
                    return;

                lock (_objLock)
                {
                    _DIODinkle.SetDO(_DOInfoAry[(int)eDOTyp]._ModID, _DOInfoAry[(int)eDOTyp]._ModNo, bDOVal);
                }
            }//Dinkle
        }

        public string GetDINameWithStatus(EDI_TYPE eDITyp, bool bDINow)
        {
            if (eDITyp == EDI_TYPE.DI_COUNT)
                return "No set DI\n";

            if (GetDI(eDITyp, bDINow))
                return GetDIName(eDITyp) + ":On\n";
            else
                return GetDIName(eDITyp) + ":Off\n";
        }
        #endregion

        public void ReadDIOFile()
        {
            if (!File.Exists(_FileDIOPath))
                return;

            String sDefaultInfo;
            String sInfo;
            String sName;
            String sModId;
            String sModNo;
            String sModType;
            String sSection = "DIOModInfo";

            IniFile iniFileInfo = new IniFile(_FileDIOPath, true);

            _DIODinkleNum = iniFileInfo.ReadInt(sSection, "DinkleModNum", _DIODinkleNum);
            if (_DIODinkleNum > 0)
            {
                String sDoNum;
                String sDINum;

                _DIOModuleInfoAry = new DIOModuleInfo[_DIODinkleNum];

                for (int i = 0; i < _DIODinkleNum; i++)
                {
                    sDefaultInfo = i.ToString("0") + ",0,0";
                    sInfo = iniFileInfo.ReadStr(sSection, "ModuleDinkle" + i.ToString("0"), sDefaultInfo);
                    sModId = sInfo.Substring(0, sInfo.IndexOf(","));
                    sInfo = sInfo.Substring(sInfo.IndexOf(",") + 1, sInfo.Length - (sInfo.IndexOf(",") + 1));
                    sDoNum = sInfo.Substring(0, sInfo.IndexOf(","));
                    sInfo = sInfo.Substring(sInfo.IndexOf(",") + 1, sInfo.Length - (sInfo.IndexOf(",") + 1));
                    sDINum = sInfo;

                    int nModId = 0;
                    int nDONum = 0;
                    int nDINum = 0;
                    if (!int.TryParse(sModId, out nModId))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                            AlarmType.Alarm,
                            "DIODinkle:" + i.ToString("0") + ",資料讀取錯誤");
                        break;
                    }
                    if (!int.TryParse(sDoNum, out nDONum))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                            AlarmType.Alarm,
                            "DIODinkle:" + i.ToString("0") + ",資料讀取錯誤");
                        break;
                    }
                    if (!int.TryParse(sDINum, out nDINum))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                            AlarmType.Alarm,
                            "DIODinkle:" + i.ToString("0") + ",資料讀取錯誤");
                        break;
                    }

                    _DIOModuleInfoAry[i]._ModID = nModId;
                    _DIOModuleInfoAry[i]._DONum = nDONum;
                    _DIOModuleInfoAry[i]._DINum = nDINum;
                    _DIOModuleInfoAry[i]._ModuleType = EDIOModuleType.DIO_Dinkle;
                }//for Dinkle
            }

            sSection = "DIInfo";
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                sDefaultInfo = ((EDI_TYPE)i).ToString() + "," + _DIInfoAry[i]._ModID.ToString("0") + "," + _DIInfoAry[i]._ModNo.ToString("0") + ",7433";
                sInfo = iniFileInfo.ReadStr(sSection, ((EDI_TYPE)i).ToString(), sDefaultInfo);

                string[] array = sInfo.Split(',');

                bool bType = false;
                if (array.Length >= 4)
                {
                    if (array[0].Trim().IndexOf("(" + array[1].Trim() + "X" + array[2].Trim() + ")", 0) >= 0 || array[0].Trim().IndexOf("(" + array[1].Trim() + "X" + array[2].Trim().PadLeft(2, '0') + ")", 0) >= 0)
                        sName = array[0].Trim();
                    else
                        sName = array[0].Trim() + "(" + array[1].Trim() + "X" + array[2].Trim().PadLeft(2, '0') + ")";
                    sModId = array[1].Trim();
                    sModNo = array[2].Trim();
                    sModType = array[3].Trim();

                    if (array.Length == 5)
                    {
                        if (array[4].Length == 1 && array[4].ToUpper() == "B")
                        {
                            bType = true;
                        }
                    }
                }
                else
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DI:" + _DIInfoAry[i]._Name + ",資料讀取錯誤");
                    break;
                }

                if (array[0].Trim().IndexOf("(" + array[1].Trim() + "X" + array[2].Trim() + ")", 0) >= 0 || array[0].Trim().IndexOf("(" + array[1].Trim() + "X" + array[2].Trim().PadLeft(2, '0') + ")", 0) >= 0)
                    sName = array[0].Trim();
                else
                    sName = array[0].Trim() + "(" + array[1].Trim() + "X" + array[2].Trim().PadLeft(2, '0') + ")";
                sModId = array[1].Trim();
                sModNo = array[2].Trim();
                sModType = array[3].Trim();

                _DIInfoAry[i]._Name = sName;
                int nModId;
                if (!int.TryParse(sModId, out nModId))
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DI:" + _DIInfoAry[i]._Name + ",資料讀取錯誤");
                    break;
                }
                _DIInfoAry[i]._ModID = nModId;

                int nModNo;
                if (!int.TryParse(sModNo, out nModNo))
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DI:" + _DIInfoAry[i]._Name + ",資料讀取錯誤");
                    break;
                }
                _DIInfoAry[i]._ModNo = nModNo;
                _DIInfoAry[i]._BType = bType;
                _DIInfoAry[i]._Module = MachineModules.Main;

                if (sModType == "Dinkle")
                    _DIInfoAry[i]._ModuleType = EDIOModuleType.DIO_Dinkle;
                else
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DI:" + _DIInfoAry[i]._Name + ",資料讀取錯誤");

                    break;
                }
            }//DI Info

            sSection = "DOInfo";
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                sDefaultInfo = ((EDO_TYPE)i).ToString() + "," + _DOInfoAry[i]._ModID.ToString("0") + "," + _DOInfoAry[i]._ModNo.ToString("0") + ",7434";
                sInfo = iniFileInfo.ReadStr(sSection, ((EDO_TYPE)i).ToString(), sDefaultInfo);

                string[] array = sInfo.Split(',');

                if (array[0].Trim().IndexOf("(" + array[1].Trim() + "Y" + array[2].Trim() + ")", 0) >= 0 || array[0].Trim().IndexOf("(" + array[1].Trim() + "Y" + array[2].Trim().PadLeft(2, '0') + ")", 0) >= 0)
                    sName = array[0].Trim();
                else
                    sName = array[0].Trim() + "(" + array[1].Trim() + "Y" + array[2].Trim().PadLeft(2, '0') + ")";
                sModId = array[1].Trim();
                sModNo = array[2].Trim();
                sModType = array[3].Trim();

                _DOInfoAry[i]._Name = sName;
                int nModId;
                if (!int.TryParse(sModId, out nModId))
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DO:" + _DOInfoAry[i]._Name + ",資料讀取錯誤");

                    break;
                }
                _DOInfoAry[i]._ModID = nModId;

                int nModNo;
                if (!int.TryParse(sModNo, out nModNo))
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DO:" + _DOInfoAry[i]._Name + ",資料讀取錯誤");

                    break;
                }
                _DOInfoAry[i]._ModNo = nModNo;
                _DOInfoAry[i]._Module = MachineModules.Main;

                if (sModType == "Dinkle")
                    _DOInfoAry[i]._ModuleType = EDIOModuleType.DIO_Dinkle;
                else
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm,
                        AlarmType.Alarm,
                        "DO:" + _DOInfoAry[i]._Name + ",資料讀取錯誤");

                    break;
                }
            }//DO Info

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using FileStreamLibrary;
using static CommonLibrary.MtnCtrlDef;

namespace CommonLibrary
{
    public enum EDoorDir
    {
        All,
        Front,
        Back,
        Lifts,

        Count
    }
    public enum EDoorElement
    {
        Lock,
        Snesor,
    }

    public class DoorDef : IDisposable
    {
        private struct DoorDirInfo
        {
            public EDoorDir Dir;
            public bool ShareDO;
            public EDI_TYPE DoorBtn;
            public EDO_TYPE BtnLED;
        }

        private string _FilePath;
        private SingleDoor[] _AllDoor;
        private EDI_TYPE[] _DoorSensor;
        private EDO_TYPE[] _DoorLock;
        private DoorDirInfo[] _DoorDir;
        private double _AutoLockTime;
        private bool _OnlyAutoAlarm;
        private int _BtnLED_TC;
        List<EDI_TYPE> _DoorBtnList;
        List<EDO_TYPE> _BtnLEDList;

        /// <summary>門檢Def</summary>
        public DoorDef(string sFilePath = "C:\\Automation\\Mod.ini")// BoWei DoorDef
        {
            _OnlyAutoAlarm = false;
            _AutoLockTime = 0.0;
            _DoorSensor = new EDI_TYPE[(int)EDoorPos.Count];
            _DoorLock = new EDO_TYPE[(int)EDoorPos.Count];
            for (int i = 0; i < (int)EDoorPos.Count; i++)
            {
                _DoorSensor[i] = EDI_TYPE.DI_COUNT;
                _DoorLock[i] = EDO_TYPE.DO_COUNT;
            }

            _DoorDir = new DoorDirInfo[(int)EDoorPos.Count];
            for (int i = 0; i < (int)EDoorPos.Count; i++)
            {
                _DoorDir[i].Dir = EDoorDir.Count;
                _DoorDir[i].ShareDO = false;
                _DoorDir[i].DoorBtn = EDI_TYPE.DI_COUNT;
                _DoorDir[i].BtnLED = EDO_TYPE.DO_COUNT;
            }

            _AllDoor = new SingleDoor[(int)EDoorPos.Count];

            _FilePath = sFilePath;
            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();
        }

        ~DoorDef() { }
        public void Dispose()
        {
            for (int i = 0; i < _AllDoor.Length; i++)
                _AllDoor[i].Dispose();

            _AllDoor = null;
        }

        #region 確認檔案與建立資料夾、建立新檔案、讀取檔案
        private void DirExistsAndCreate(string iniPath)
        {
            if (!Directory.Exists(iniPath))
                Directory.CreateDirectory(iniPath);
        }
        private bool CheckFile(string sFilePath)
        {
            if (!File.Exists(sFilePath))
            {
                string[] _SfilePathArr = sFilePath.Split('\\');
                if (_SfilePathArr[_SfilePathArr.Length - 1].Contains(".ini"))
                {
                    string _iniPath = "";
                    for (int i = 0; i < _SfilePathArr.Length - 1; i++)
                        _iniPath += _SfilePathArr[i];

                    DirExistsAndCreate(_iniPath);
                }

                return false;
            }

            return true;
        }
        private void CreateFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);
            for (int i = 0; i < (int)EDoorPos.Count; i++)
            {
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), EDoorElement.Snesor.ToString(), EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), EDoorElement.Lock.ToString(), EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), "Dir", EDoorDir.Count.ToString());
                iniFileInfo.WriteBool(((EDoorPos)i).ToString(), "ShareDO", false);
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), "DoorButton", EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), "ButtonLED", EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteBool(((EDoorPos)i).ToString(), "OnlyAutoAlarm", _OnlyAutoAlarm);
                iniFileInfo.WriteDouble(((EDoorPos)i).ToString(), "AutoLockTime", _AutoLockTime);
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            string _BufStr = "";

            IniFile iniFileInfo = new IniFile(_FilePath, true);
            for (int i = 0; i < (int)EDoorPos.Count; i++)
            {
                #region DI
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EDoorPos)i).ToString(), EDoorElement.Snesor.ToString(), EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _DoorSensor[i] = (EDI_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EDoorPos)i).ToString(), "DoorButton", EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _DoorDir[i].DoorBtn = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region DO
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EDoorPos)i).ToString(), EDoorElement.Lock.ToString(), EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _DoorLock[i] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EDoorPos)i).ToString(), "ButtonLED", EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _DoorDir[i].BtnLED = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region Dir
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EDoorPos)i).ToString(), "Dir", EDoorDir.Count.ToString()).Trim();
                for (int j = 0; j < (int)EDoorDir.Count; j++)
                {
                    if (((EDoorDir)j).ToString() == _BufStr)
                    {
                        _DoorDir[i].Dir = (EDoorDir)j;
                        break;
                    }
                }
                #endregion

                #region Share
                _DoorDir[i].ShareDO = iniFileInfo.ReadBool(((EDoorPos)i).ToString(), "ShareDO", false);
                #endregion

                #region Onle Auto Alarm
                _OnlyAutoAlarm = iniFileInfo.ReadBool(((EDoorPos)i).ToString(), "OnlyAutoAlarm", true);
                #endregion

                #region Auto Lock Time
                _AutoLockTime = iniFileInfo.ReadDouble(((EDoorPos)i).ToString(), "AutoLockTime", 0.0);
                #endregion
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);
            for (int i = 0; i < (int)EDoorPos.Count; i++)
            {
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), EDoorElement.Snesor.ToString(), _DoorSensor[i].ToString());
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), EDoorElement.Lock.ToString(), _DoorLock[i].ToString());
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), "Dir", _DoorDir[i].Dir.ToString());
                iniFileInfo.WriteBool(((EDoorPos)i).ToString(), "ShareDO", _DoorDir[i].ShareDO);
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), "DoorButton", _DoorDir[i].DoorBtn.ToString());
                iniFileInfo.WriteStr(((EDoorPos)i).ToString(), "ButtonLED", _DoorDir[i].BtnLED.ToString());
                iniFileInfo.WriteBool(((EDoorPos)i).ToString(), "OnlyAutoAlarm", _OnlyAutoAlarm);
                iniFileInfo.WriteDouble(((EDoorPos)i).ToString(), "AutoLockTime", _AutoLockTime);
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        #endregion

        public void Save()
        {
            WriteFile();
            ReNew();
        }
        private void ReNew()
        {
            for (int i = 0; i < (int)EDoorPos.Count; i++)
                _AllDoor[i] = new SingleDoor(_DoorSensor[i], _DoorLock[i]);

            RenewBtnLEDList();
            RenewDoorBtnList();
        }
        private void RenewBtnLEDList()
        {
            _BtnLEDList = new List<EDO_TYPE>();
            for (int i = 0; i < _DoorDir.Length; i++)
            {
                if (_DoorDir[i].BtnLED != EDO_TYPE.DO_COUNT)
                {
                    bool Find = false;
                    for (int j = 0; j < _BtnLEDList.Count; j++)
                    {
                        if (_BtnLEDList[j] == _DoorDir[i].BtnLED)
                        {
                            Find = true;
                            break;
                        }
                    }
                    if (!Find)
                    {
                        if ((int)_DoorDir[i].BtnLED >= 0 && _DoorDir[i].BtnLED != EDO_TYPE.DO_COUNT)
                            _BtnLEDList.Add(_DoorDir[i].BtnLED);
                    }
                }
            }
        }
        private void RenewDoorBtnList()
        {
            _DoorBtnList = new List<EDI_TYPE>();
            for (int i = 0; i < _DoorDir.Length; i++)
            {
                if (_DoorDir[i].DoorBtn != EDI_TYPE.DI_COUNT)
                {
                    bool Find = false;
                    for (int j = 0; j < _DoorBtnList.Count; j++)
                    {
                        if (_DoorBtnList[j] == _DoorDir[i].DoorBtn)
                        {
                            Find = true;
                            break;
                        }
                    }
                    if (!Find)
                    {
                        if ((int)_DoorDir[i].DoorBtn >= 0 && _DoorDir[i].DoorBtn != EDI_TYPE.DI_COUNT)
                            _DoorBtnList.Add(_DoorDir[i].DoorBtn);
                    }
                }
            }
        }
        public void SetDoorDI(EDoorPos eDoorPos, EDI_TYPE eDI_TYPE) { _DoorSensor[(int)eDoorPos] = eDI_TYPE; }
        public void SetDoorDO(EDoorPos eDoorPos, EDO_TYPE eDO_TYPE) { _DoorLock[(int)eDoorPos] = eDO_TYPE; }
        public void SetDoorDir(EDoorPos eDoorPos, EDoorDir eDoorDir) { _DoorDir[(int)eDoorPos].Dir = eDoorDir; }
        public void SetDoorDirShare(EDoorPos eDoorPos, bool eShareDO) { _DoorDir[(int)eDoorPos].ShareDO = eShareDO; }
        public void SetDoorBtn(EDoorPos eDoorPos, EDI_TYPE eDI_TYPE) { _DoorDir[(int)eDoorPos].DoorBtn = eDI_TYPE; }
        public void SetBtnLED(EDoorPos eDoorPos, EDO_TYPE eDO_TYPE) { _DoorDir[(int)eDoorPos].BtnLED = eDO_TYPE; }
        public void SetOnlyAutoAlarm(bool eOnlyAutoAlarm) { _OnlyAutoAlarm = eOnlyAutoAlarm; }
        public void SetLockTime(double eLocktime) { _AutoLockTime = eLocktime; }
        public EDI_TYPE GetDoorDI(EDoorPos eDoorPos) { return _DoorSensor[(int)eDoorPos]; }
        public EDO_TYPE GetDoorDO(EDoorPos eDoorPos) { return _DoorLock[(int)eDoorPos]; }
        public EDoorDir GetDoorDir(EDoorPos eDoorPos) { return _DoorDir[(int)eDoorPos].Dir; }
        public bool GetDoorDirShare(EDoorPos eDoorPos) { return _DoorDir[(int)eDoorPos].ShareDO; }
        public EDI_TYPE GetDoorBtn(EDoorPos eDoorPos) { return _DoorDir[(int)eDoorPos].DoorBtn; }
        public EDO_TYPE GetBtnLED(EDoorPos eDoorPos) { return _DoorDir[(int)eDoorPos].BtnLED; }
        public bool GetOnlyAutoAlarm() { return _OnlyAutoAlarm; }
        public double GetLockTime() { return _AutoLockTime; }

        public void PassSensor(EDoorPos eDoorPos = EDoorPos.Count, bool bVal = false)
        {
            if (eDoorPos == EDoorPos.Count)
            {
                for (int i = 0; i < _AllDoor.Length; i++)
                    _AllDoor[i].PassSensor(bVal);
            }
            else
                _AllDoor[(int)eDoorPos].PassSensor(bVal);
        }

        /// <summary>門解鎖</summary>
        public void Unlock(EDoorDir eEDoorDir = EDoorDir.All)
        {
            if (eEDoorDir == EDoorDir.All)
            {
                for (int i = 0; i < _AllDoor.Length; i++)
                    _AllDoor[i].Unlock();
            }
            else
            {
                List<SingleDoor> _Door = new List<SingleDoor>();
                for (int i = 0; i < _AllDoor.Length; i++)
                {
                    if (_DoorDir[i].Dir == eEDoorDir)
                    {
                        if (_DoorDir[i].ShareDO)
                            _Door.Add(_AllDoor[i]);
                        else
                            _AllDoor[i].Unlock();
                    }
                }
                for (int i = 0; i < _Door.Count; i++)
                    _Door[i].RenewTime();
                for (int i = 0; i < _Door.Count; i++)
                    _Door[i].Unlock();
            }
        }
        /// <summary>門上鎖</summary>
        public void Lock(EDoorDir eEDoorDir = EDoorDir.All)
        {
            if (eEDoorDir == EDoorDir.All)
            {
                for (int i = 0; i < _AllDoor.Length; i++)
                    _AllDoor[i].Lock();
            }
            else
            {
                for (int i = 0; i < _AllDoor.Length; i++)
                {
                    if (_DoorDir[i].Dir == eEDoorDir)
                        _AllDoor[i].Lock();
                }
            }
        }
        /// <summary>單片門上鎖</summary>
        public void SingleLock(EDoorPos eDoorPos)
        {
            if (eDoorPos >= 0 && eDoorPos != EDoorPos.Count)
                _AllDoor[(int)eDoorPos].Lock();
        }
        /// <summary>單片門解鎖</summary>
        public void SingleUnlock(EDoorPos eDoorPos)
        {
            if (eDoorPos >= 0 && eDoorPos != EDoorPos.Count)
                _AllDoor[(int)eDoorPos].Unlock();
        }
        /// <summary>所有門自動上鎖</summary>
        public void AutoLock()
        {
            for (int i = 0; i < _AllDoor.Length; i++)
                _AllDoor[i].AutoLock(_AutoLockTime);
        }
        /// <summary>所有門是否關閉</summary>
        public bool DoorSafty(out string sDoorPos)
        {
            sDoorPos = "";
            List<string> _sBuf = new List<string>();

            for (int i = 0; i < _AllDoor.Length; i++)
            {
                if (!_AllDoor[i].IsClose(false))
                    _sBuf.Add(((EDoorPos)i).ToString());
            }

            if (_sBuf.Count > 0)
            {
                sDoorPos = String.Join(",", _sBuf.ToArray());

                if (_BtnLED_TC < 0)
                    _BtnLED_TC = Environment.TickCount;
                if (Environment.TickCount - _BtnLED_TC >= 500)
                {
                    _BtnLED_TC = Environment.TickCount;

                    for (int i = 0; i < _BtnLEDList.Count; i++)
                        G.Comm.IOCtrl.SetDO(_BtnLEDList[i], !G.Comm.IOCtrl.GetDO(_BtnLEDList[i], false));
                }

                return false;
            }

            _BtnLED_TC = Environment.TickCount;
            return true;
        }
        /// <summary>確認門鎖按鈕動作</summary>
        public void CheckBtnMotion()
        {
            for (int i = 0; i < _DoorDir.Length; i++)
            {
                if ((int)_DoorDir[i].DoorBtn >= 0 && _DoorDir[i].DoorBtn != EDI_TYPE.DI_COUNT)
                {
                    switch (G.Comm.IOCtrl.GetDIEdge(_DoorDir[i].DoorBtn))
                    {
                        case EDIO_SingleEdge.RisingEdge:
                            if ((int)_DoorDir[i].BtnLED >= 0 && _DoorDir[i].BtnLED != EDO_TYPE.DO_COUNT)
                                G.Comm.IOCtrl.SetDO(_DoorDir[i].BtnLED, true);
                            G.Comm.Door.Unlock(_DoorDir[i].Dir);
                            break;
                        case EDIO_SingleEdge.FallingEdge:
                            if ((int)_DoorDir[i].BtnLED >= 0 && _DoorDir[i].BtnLED != EDO_TYPE.DO_COUNT)
                                G.Comm.IOCtrl.SetDO(_DoorDir[i].BtnLED, false);
                            break;
                        case EDIO_SingleEdge.Off:
                            if (DoorSafty(out string sDoorPos))
                            {
                                if ((int)_DoorDir[i].BtnLED >= 0 && _DoorDir[i].BtnLED != EDO_TYPE.DO_COUNT)
                                {
                                    if (G.Comm.IOCtrl.GetDO(_DoorDir[i].BtnLED, true))
                                        G.Comm.IOCtrl.SetDO(_DoorDir[i].BtnLED, false);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class SingleDoor : IDisposable
    {
        private EDI_TYPE _DoorDI;
        private EDO_TYPE _DoorDO;
        private int _DoorClose_TC;

        /// <summary>單一門檢</summary>
        /// <param name="eDoorDI">門鎖DI</param>
        /// <param name="eDoorDO">門鎖DO</param>
        /// <remarks>
        /// <list type="門鎖DI">eDoorDI: 門鎖DI</list>
        /// <list type="門鎖DO">eDoorDO: 門鎖DO</list>
        /// </remarks>
        public SingleDoor(EDI_TYPE eDoorDI, EDO_TYPE eDoorDO)
        {
            _DoorDI = eDoorDI;
            _DoorDO = eDoorDO;

            _DoorClose_TC = Environment.TickCount - 1000;
        }

        ~SingleDoor() { }
        public void Dispose() { }

        private void RenewStatue()
        {
            if (_DoorDI >= 0 && _DoorDI != EDI_TYPE.DI_COUNT)
            {
                if (!G.Comm.IOCtrl.GetDI(_DoorDI, true))
                    RenewTime();
            }
        }

        public void RenewTime() { _DoorClose_TC = Environment.TickCount; }

        /// <summary>Pass 感測器</summary>
        /// <param name="bVal">布林值</param>
        /// <remarks>
        /// <list type="布林值">bVal: True為Pass, False為啟用</list>
        /// </remarks>
        public void PassSensor(bool bVal)
        {
            if (_DoorDI >= 0 && _DoorDI != EDI_TYPE.DI_COUNT)
                G.Comm.IOCtrl.SetDIPass(_DoorDI, bVal);
        }

        /// <summary>是否關門</summary>
        public bool IsClose(bool eUseTime = true)
        {
            if (eUseTime)
            {
                RenewStatue();
                return Environment.TickCount - _DoorClose_TC > 50;
            }
            else
            {
                if (_DoorDI >= 0 && _DoorDI != EDI_TYPE.DI_COUNT)
                    return G.Comm.IOCtrl.GetDI(_DoorDI, true);
                else
                    return true;
            }
        }
        /// <summary>門解鎖</summary>
        public void Unlock()
        {
            if (_DoorDO >= 0 && _DoorDO != EDO_TYPE.DO_COUNT)
            {
                if (G.Comm.IOCtrl.GetDO(_DoorDO, true))
                {
                    RenewTime();
                    G.Comm.IOCtrl.SetDO(_DoorDO, false);
                }
            }
        }
        /// <summary>門檢自動上鎖</summary>
        /// <param name="eHoldTime">門關閉時間(秒)</param>
        /// <remarks>
        /// <list type="門關閉時間(秒)">eHoldTime: 門關閉時間(秒)</list>
        /// </remarks>
        public void AutoLock(double eHoldTime = 0)
        {
            if (_DoorDO >= 0 && _DoorDO != EDO_TYPE.DO_COUNT)
            {
                if (IsClose() && (Environment.TickCount - _DoorClose_TC > (int)(eHoldTime * 1000)))
                    G.Comm.IOCtrl.SetDO(_DoorDO, true);
            }
        }
        /// <summary>門上鎖</summary>
        public void Lock()
        {
            if (_DoorDO >= 0 && _DoorDO != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_DoorDO, true);
        }
    }
}
using System;
using System.IO;
using FileStreamLibrary;

namespace CommonLibrary
{
    public class TrolleyDef : IDisposable
    {
        private string _FilePath;
        private SingleTrolley[] _AllTrolley;
        private EDI_TYPE[] _TrolleySensor;
        private ECylName[][] _TrolleyCyl;
        private double _AutoLockTime;

        /// <summary>台車Def</summary>
        public TrolleyDef(string sFilePath = "C:\\Automation\\Mod.ini")// BoWei TrolleyDef
        {
            _AutoLockTime = 0.0;
            _TrolleySensor = new EDI_TYPE[(int)ETrolley.Count];
            _TrolleyCyl = new ECylName[(int)ETrolley.Count][];
            for (int i = 0; i < (int)ETrolley.Count; i++)
            {
                _TrolleySensor[i] = EDI_TYPE.DI_COUNT;
                _TrolleyCyl[i] = new ECylName[2];
                for (int j = 0; j < _TrolleyCyl[i].Length; j++)
                {
                    _TrolleyCyl[i][j] = ECylName.Count;
                }
            }

            _AllTrolley = new SingleTrolley[(int)ETrolley.Count];

            _FilePath = sFilePath;
            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();
        }

        ~TrolleyDef() { }
        public void Dispose()
        {
            for (int i = 0; i < _AllTrolley.Length; i++)
                _AllTrolley[i].Dispose();

            _AllTrolley = null;
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
            for (int i = 0; i < (int)ETrolley.Count; i++)
            {
                for (int j = 0; j < _TrolleyCyl[i].Length; j++)
                    iniFileInfo.WriteStr(((ETrolley)i).ToString(), "Cylinder_" + j.ToString(), _TrolleyCyl[i][j].ToString());

                iniFileInfo.WriteStr(((ETrolley)i).ToString(), "Sensor", _TrolleySensor.ToString());
                iniFileInfo.WriteDouble(((ETrolley)i).ToString(), "AutoLockTime", _AutoLockTime);
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            string _BufStr = "";

            IniFile iniFileInfo = new IniFile(_FilePath, true);
            for (int i = 0; i < (int)ETrolley.Count; i++)
            {
                #region 氣缸
                for (int j = 0; j < _TrolleyCyl[i].Length; j++)
                {
                    _BufStr = "";
                    _BufStr = iniFileInfo.ReadStr(((ETrolley)i).ToString(), "Cylinder_" + j.ToString(), _TrolleyCyl[i][j].ToString().Trim());

                    for (int k = 0; k < (int)ECylName.Count; k++)
                    {
                        if (((ECylName)k).ToString() == _BufStr)
                        {
                            _TrolleyCyl[i][j] = (ECylName)k;
                            break;
                        }
                    }
                }
                #endregion

                #region Sensor DI
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((ETrolley)i).ToString(), "Sensor", _TrolleySensor[i].ToString().Trim());

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _TrolleySensor[i] = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region Auto Lock Time
                _AutoLockTime = iniFileInfo.ReadDouble(((ETrolley)i).ToString(), "AutoLockTime", 0.0);
                #endregion
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);
            for (int i = 0; i < (int)ETrolley.Count; i++)
            {
                for (int j = 0; j < _TrolleyCyl[i].Length; j++)
                    iniFileInfo.WriteStr(((ETrolley)i).ToString(), "Cylinder_" + j.ToString(), _TrolleyCyl[i][j].ToString());

                iniFileInfo.WriteStr(((ETrolley)i).ToString(), "Sensor", _TrolleySensor.ToString());
                iniFileInfo.WriteDouble(((ETrolley)i).ToString(), "AutoLockTime", _AutoLockTime);
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
            for (int i = 0; i < (int)ETrolley.Count; i++)
                _AllTrolley[i] = new SingleTrolley(_TrolleyCyl[i][0], _TrolleyCyl[i][1], _TrolleySensor[i]);
        }
        public void SetTrolleySensor(ETrolley eTrolley, EDI_TYPE eDI_TYPE) { _TrolleySensor[(int)eTrolley] = eDI_TYPE; }
        public void SetTrolleyCyl(ETrolley eTrolley, ECylName eCylNameA, ECylName eCylNameB)
        {
            _TrolleyCyl[(int)eTrolley][0] = eCylNameA;
            _TrolleyCyl[(int)eTrolley][1] = eCylNameB;
        }
        public void SetLockTime(double eLocktime) { _AutoLockTime = eLocktime; }
        public EDI_TYPE GetTrolleySensor(ETrolley eTrolley) { return _TrolleySensor[(int)eTrolley]; }
        public ECylName GetTrolleyCylA(ETrolley eTrolley) { return _TrolleyCyl[(int)eTrolley][0]; }
        public ECylName GetTrolleyCylB(ETrolley eTrolley) { return _TrolleyCyl[(int)eTrolley][1]; }
        public double GetLockTime() { return _AutoLockTime; }

        /// <summary>單台車解鎖</summary>
        public bool Unlock(ETrolley eTrolley)
        {
            if (_AllTrolley.Length <= 0)
                return true;

            _AllTrolley[(int)eTrolley].Unlock();

            return G.Comm.CYL.CheckIO(_TrolleyCyl[(int)eTrolley][0]) && G.Comm.CYL.CheckIO(_TrolleyCyl[(int)eTrolley][1]);
        }
        /// <summary>單台車上鎖</summary>
        public void Lock(ETrolley eTrolley)
        {
            _AllTrolley[(int)eTrolley].Lock();
        }
        /// <summary>所有台車自動上鎖</summary>
        public void AutoLock()
        {
            for (int i = 0; i < _AllTrolley.Length; i++)
                _AllTrolley[i].AutoLock(_AutoLockTime);
        }
        /// <summary>台車是否在位</summary>
        public bool IsInPlace(ETrolley eTrolley)
        {
            if (_AllTrolley.Length <= 0)
                return true;

            return _AllTrolley[(int)eTrolley].IsInPlace();
        }
        /// <summary>台車是否上鎖</summary>
        public bool IsLock(ETrolley eTrolley)
        {
            if (_AllTrolley.Length <= 0)
                return true;

            return _AllTrolley[(int)eTrolley].IsLock();
        }
    }

    public class SingleTrolley : IDisposable
    {
        private ECylName _TrolleyCylA;
        private ECylName _TrolleyCylB;
        private EDI_TYPE _InplaceSensor;
        private int _TrolleyLock_TC;

        /// <summary>單一台車</summary>
        /// <param name="eCylNameA">氣缸A</param>
        /// <param name="eCylNameB">氣缸B</param>
        /// <param name="eDI_TYPE">到位感測器</param>
        /// <remarks>
        /// <list type="氣缸A">eCylNameA: 氣缸A</list>
        /// <list type="氣缸B">eCylNameB: 氣缸B</list>
        /// <list type="到位感測器">eDI_TYPE: 到位感測器</list>
        /// </remarks>
        public SingleTrolley(ECylName eCylNameA, ECylName eCylNameB, EDI_TYPE eDI_TYPE)
        {
            _TrolleyCylA = eCylNameA;
            _TrolleyCylB = eCylNameB;
            _InplaceSensor = eDI_TYPE;

            _TrolleyLock_TC = Environment.TickCount - 1000;
        }

        ~SingleTrolley() { }
        public void Dispose() { }

        public void RenewTime() { _TrolleyLock_TC = Environment.TickCount; }

        /// <summary>是否到位</summary>
        public bool IsInPlace()
        {
            if (_InplaceSensor >= 0 && _InplaceSensor != EDI_TYPE.DI_COUNT)
            {
                if (!G.Comm.IOCtrl.GetDI(_InplaceSensor, true))
                {
                    RenewTime();
                    return false;
                }
                else
                    return true;
            }
            else
                return false;
        }
        /// <summary>台車解鎖</summary>
        public void Unlock()
        {
            RenewTime();

            if (_TrolleyCylA != ECylName.Count)
                G.Comm.CYL.Extension(_TrolleyCylA);
            if (_TrolleyCylB != ECylName.Count)
                G.Comm.CYL.Extension(_TrolleyCylB);
        }
        /// <summary>台車自動上鎖</summary>
        /// <param name="eHoldTime">台車上鎖時間(秒)</param>
        /// <remarks>
        /// <list type="台車上鎖時間(秒)">eHoldTime: 台車上鎖時間(秒)</list>
        /// </remarks>
        public void AutoLock(double eHoldTime = 0)
        {
            if (_InplaceSensor >= 0 && _InplaceSensor != EDI_TYPE.DI_COUNT)
            {
                if (IsInPlace() && (Environment.TickCount - _TrolleyLock_TC > (int)(eHoldTime * 1000)))
                    Lock();
            }
        }
        /// <summary>台車上鎖</summary>
        public void Lock()
        {
            if (_TrolleyCylA != ECylName.Count)
                G.Comm.CYL.Retract(_TrolleyCylA);
            if (_TrolleyCylB != ECylName.Count)
                G.Comm.CYL.Retract(_TrolleyCylB);
        }
        /// <summary>台車是否上鎖</summary>
        public bool IsLock()
        {
            if (_TrolleyCylA != ECylName.Count && _TrolleyCylB != ECylName.Count)
            {
                if (G.Comm.CYL.GetCYLStatus(_TrolleyCylA) == ECylMotion.Extension && G.Comm.CYL.GetCYLStatus(_TrolleyCylB) == ECylMotion.Extension)
                    return G.Comm.CYL.CheckIO(_TrolleyCylA) && G.Comm.CYL.CheckIO(_TrolleyCylB);
            }
            else if (_TrolleyCylA != ECylName.Count && _TrolleyCylB == ECylName.Count)
            {
                if (G.Comm.CYL.GetCYLStatus(_TrolleyCylA) == ECylMotion.Extension)
                    return G.Comm.CYL.CheckIO(_TrolleyCylA);
            }
            else if (_TrolleyCylA == ECylName.Count && _TrolleyCylB != ECylName.Count)
            {
                if (G.Comm.CYL.GetCYLStatus(_TrolleyCylB) == ECylMotion.Extension)
                    return G.Comm.CYL.CheckIO(_TrolleyCylB);
            }

            return false;
        }
    }
}
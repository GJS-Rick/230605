using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using FileStreamLibrary;

namespace CommonLibrary
{
    public class VibrationBowDef : IDisposable
    {
        private string _FilePath;
        private SingleVibrationBow[] _AllVibrationBow;
        private EDI_TYPE[] _EmptySensor;
        private EDO_TYPE[] _Power;
        private int[] _StartDelay;
        private int[] _StopDelay;
        private int[] _AlarmTime;
        private bool[] _EmptyObject;

        private bool _ThVibrationBowEnd;
        private Thread _ThVibrationBow;

        /// <summary>震動送料機Def</summary>
        /// <param name="eDoorDI">門鎖DI</param>
        /// <param name="eDoorDO">門鎖DO</param>
        /// <remarks>
        /// <list type="門鎖DI">eDoorDI: 門鎖DI</list>
        /// <list type="門鎖DO">eDoorDO: 門鎖DO</list>
        /// </remarks>
        public VibrationBowDef(string sFilePath = "C:\\Automation\\Mod.ini")// BoWei VibrationBowDef
        {
            _AllVibrationBow = new SingleVibrationBow[(int)EVibBow.Count];
            _EmptySensor = new EDI_TYPE[(int)EVibBow.Count];
            _Power = new EDO_TYPE[(int)EVibBow.Count];
            _StartDelay = new int[(int)EVibBow.Count];
            _StopDelay = new int[(int)EVibBow.Count];
            _AlarmTime = new int[(int)EVibBow.Count];
            _EmptyObject = new bool[(int)EVibBow.Count];

            for (int i = 0; i < (int)EVibBow.Count; i++)
            {
                _EmptySensor[i] = EDI_TYPE.DI_COUNT;
                _Power[i] = EDO_TYPE.DO_COUNT;
                _StartDelay[i] = -1;
                _StopDelay[i] = -1;
                _AlarmTime[i] = -1;
                _EmptyObject[i] = false;
            }

            _FilePath = sFilePath;
            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();
        }

        ~VibrationBowDef() { }
        public void Dispose() 
        {
            for (int i = 0; i < _AllVibrationBow.Length; i++)
                _AllVibrationBow[i].Dispose();

            _AllVibrationBow = null;
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
            for (int i = 0; i < (int)EVibBow.Count; i++)
            {
                iniFileInfo.WriteStr(((EVibBow)i).ToString(), "EmptySensor_DI", EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((EVibBow)i).ToString(), "Power_DO", EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteDouble(((EVibBow)i).ToString(), "StartDelay(Sec)", -1.0);
                iniFileInfo.WriteDouble(((EVibBow)i).ToString(), "StopDelay(Sec)", -1.0);
                iniFileInfo.WriteDouble(((EVibBow)i).ToString(), "EmptyAlarmTime(Sec)", -1.0);
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            string _BufStr = "";

            IniFile iniFileInfo = new IniFile(_FilePath, true);
            for (int i = 0; i < (int)EVibBow.Count; i++)
            {
                #region EmptySensor
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVibBow)i).ToString(), "EmptySensor_DI", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _EmptySensor[i] = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region Power
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVibBow)i).ToString(), "Power_DO", EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _Power[i] = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region Start delay time
                _StartDelay[i] = (int)(iniFileInfo.ReadDouble(((EVibBow)i).ToString(), "StartDelay(Sec)", -1.0) * 1000);
                #endregion

                #region Stop delay time
                _StopDelay[i] = (int)(iniFileInfo.ReadDouble(((EVibBow)i).ToString(), "StopDelay(Sec)", -1.0) * 1000);
                #endregion

                #region Empty Alarm time
                _AlarmTime[i] = (int)(iniFileInfo.ReadDouble(((EVibBow)i).ToString(), "EmptyAlarmTime(Sec)", -1.0) * 1000);
                #endregion
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);
            for (int i = 0; i < (int)EVibBow.Count; i++)
            {
                iniFileInfo.WriteStr(((EVibBow)i).ToString(), "EmptySensor_DI", _EmptySensor[i].ToString());
                iniFileInfo.WriteStr(((EVibBow)i).ToString(), "Power_DO", _Power[i].ToString());
                iniFileInfo.WriteDouble(((EVibBow)i).ToString(), "StartDelay(Sec)", _StartDelay[i] / 1000);
                iniFileInfo.WriteDouble(((EVibBow)i).ToString(), "StopDelay(Sec)", _StopDelay[i] / 1000);
                iniFileInfo.WriteDouble(((EVibBow)i).ToString(), "EmptyAlarmTime(Sec)", _AlarmTime[i] / 1000);
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
            _ThVibrationBowEnd = true;
            while (_ThVibrationBow != null && _ThVibrationBow.IsAlive)
            {
                Thread.Sleep(5);
            }

            for (int i = 0; i < (int)EVibBow.Count; i++)
            {
                _AllVibrationBow[i] = new SingleVibrationBow(_EmptySensor[i], _Power[i]);
                _AllVibrationBow[i].AutoRun(true);
            }

            if (_AllVibrationBow.Length > 0)
            {
                _ThVibrationBowEnd = false;
                _ThVibrationBow = new Thread(Start)
                {
                    Priority = ThreadPriority.BelowNormal
                };
                _ThVibrationBow.Start();
            }
            else
                _ThVibrationBowEnd = true;
        }

        #region Get/Set
        public void SetEmptySensor(EVibBow eVibrationBow, EDI_TYPE eDI_TYPE)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _EmptySensor[(int)eVibrationBow] = eDI_TYPE;
        }
        public void SetPower(EVibBow eVibrationBow, EDO_TYPE eDO_TYPE)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _Power[(int)eVibrationBow] = eDO_TYPE;
        }
        public void SetStartDelay(EVibBow eVibrationBow, double eTime)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _StartDelay[(int)eVibrationBow] = (int)(eTime * 1000);
        }
        public void SetStopDelay(EVibBow eVibrationBow, double eTime)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _StopDelay[(int)eVibrationBow] = (int)(eTime * 1000);
        }
        public void SetEmptyAlarmTime(EVibBow eVibrationBow, double eTime)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _AlarmTime[(int)eVibrationBow] = (int)(eTime * 1000);
        }
        public void ForceRunOn(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _AllVibrationBow[(int)eVibrationBow].ForceRun(true);
        }
        public void ForceRunOff(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return;

            _AllVibrationBow[(int)eVibrationBow].ForceRun(false);
        }

        public EDI_TYPE GetEmptySensor(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return EDI_TYPE.DI_COUNT;

            return _EmptySensor[(int)eVibrationBow];
        }
        public EDO_TYPE GetPower(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return EDO_TYPE.DO_COUNT;

            return _Power[(int)eVibrationBow];
        }
        public double GetStartDelay(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return -1.0;

            return _StartDelay[(int)eVibrationBow] / 1000;
        }
        public double GetStopDelay(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return -1.0;

            return _StopDelay[(int)eVibrationBow] / 1000;
        }
        public double GetEmptyAlarmTime(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return -1.0;

            return _AlarmTime[(int)eVibrationBow] / 1000;
        }
        public bool IsEmpty(EVibBow eVibrationBow)
        {
            if (eVibrationBow == EVibBow.Count)
                return false;

            return _EmptyObject[(int)eVibrationBow];
        }
        #endregion

        private void Start()
        {
            int StartT;
            int StopT;
            int AlarmT;

            while (!_ThVibrationBowEnd)
            {
                StartT = 5000;
                StopT = 5000;
                AlarmT = 30000;

                for (int i = 0; i < (int)EVibBow.Count; i++)
                {
                    if (_StartDelay[i] >= 0)
                        StartT = _StartDelay[i];
                    if (_StopDelay[i] >= 0)
                        StopT = _StopDelay[i];
                    if (_AlarmTime[i] >= 0)
                        AlarmT = _AlarmTime[i];
                        
                    _AllVibrationBow[i].CheckObject(StartT, StopT, AlarmT);
                    _EmptyObject[i] = _AllVibrationBow[i].IsEmpty();

                    Thread.Sleep(50);
                }
            }
        }
    }

    public class SingleVibrationBow : IDisposable
    {
        private EDI_TYPE _EmptySensor;
        private EDO_TYPE _Power;
        private bool _EmptyObject;
        private bool _ForceRun;
        private int _FullStop_TC;
        private int _NullStart_TC;
        private int _NullAlarm_TC;

        /// <summary>單一震動送料機</summary>
        /// <param name="eEmptySensor">滿料感測器</param>
        /// <param name="ePower">震盤啟動開關</param>
        /// <remarks>
        /// <list type="滿料感測器">eEmptySensor: 滿料感測器</list>
        /// <list type="震盤啟動開關">ePower: 震盤啟動開關</list>
        /// </remarks>
        public SingleVibrationBow(EDI_TYPE eEmptySensor, EDO_TYPE ePower)
        {
            _EmptySensor = eEmptySensor;
            _Power = ePower;

            _EmptyObject = false;
            _ForceRun = false;
            _FullStop_TC = Environment.TickCount - 1000;
            _NullStart_TC = Environment.TickCount - 1000;
            _NullAlarm_TC = Environment.TickCount - 1000;
        }

        ~SingleVibrationBow() { }
        public void Dispose() { }

        public void CheckObject(int eStartTime, int eStopTime, int eAlarmTime)
        {
            if (_Power == EDO_TYPE.DO_COUNT)
                return;

            if (!_ForceRun)
            {
                if (G.Comm.IOCtrl.GetDO(_Power, true))
                {
                    if (G.Comm.IOCtrl.GetDI(_EmptySensor, false))
                    {
                        _NullAlarm_TC = Environment.TickCount;
                        _EmptyObject = false;

                        if (Environment.TickCount - _FullStop_TC >= eStopTime)
                        {
                            _NullStart_TC = Environment.TickCount;
                            G.Comm.IOCtrl.SetDO(_Power, false);
                        }
                    }
                    else
                    {
                        _FullStop_TC = Environment.TickCount;
                        if (Environment.TickCount - _NullAlarm_TC >= eAlarmTime)
                            _EmptyObject = true;
                    }
                }
                else
                {
                    if (G.Comm.IOCtrl.GetDI(_EmptySensor, false))
                        _NullStart_TC = Environment.TickCount;
                    else
                    {
                        if (Environment.TickCount - _NullStart_TC >= eStartTime)
                        {
                            _NullAlarm_TC = Environment.TickCount;
                            _FullStop_TC = Environment.TickCount;
                            G.Comm.IOCtrl.SetDO(_Power, true);
                        }
                    }
                    _EmptyObject = false;
                }
            }
        }

        public bool IsEmpty() { return _EmptyObject; }
        public void ForceRun(bool eOn)
        {
            if (_Power == EDO_TYPE.DO_COUNT)
                return;

            _ForceRun = true;
            G.Comm.IOCtrl.SetDO(_Power, eOn);
        }
        public void AutoRun(bool eOn) { _ForceRun = !eOn; }
    }
}
using System;
using System.IO;
using FileStreamLibrary;
using static CommonLibrary.SingleVacSuckerDef;

namespace CommonLibrary
{
    /// <summary>真空動作</summary>
    public enum EVacMotion
    {
        VacOn,
        VacOff,
        VacBreak
    }

    /// <summary>震動缸動作</summary>
    public enum ESuckerMotion
    {
        Up,
        Down,
        Shock
    }

    /// <summary>真空Sensor</summary>
    public enum EVacSensorFm
    {
        VaccumSensor1,
        VaccumSensor2
    }
    /// <summary>吸盤DO</summary>
    public enum EVacDOFm
    {
        VaccumOn,
        VaccumOff,
        VaccumBreak,
        Shock1,
        Shock2
    }

    public class VacSuckerDef : IDisposable
    {
        private string _FilePath;
        private SingleVacSuckerDef[] _AllVacSucker;
        private VacSuckerInfo[] _AllVacSuckerInfo;
        private VacSuckerInfo[] _PreAllVacSuckerInfo;
        private EDI_TYPE[] _VacBtn;
        private EDO_TYPE[] _VacBtnLED;
        private EDI_TYPE[][] _VacIn;
        private EDO_TYPE[][] _VacOut;
        private double[] _VacOffDelay;
        private int[] _VacShockIntervals;
        private int[] _VacShockTimes;

        public VacSuckerDef(string sFilePath = "C:\\Automation\\Mod.ini")// BoWei VacSuckerDef
        {
            _PreAllVacSuckerInfo = new VacSuckerInfo[(int)EVacSuckerName.Count];
            _AllVacSuckerInfo = new VacSuckerInfo[(int)EVacSuckerName.Count];
            _VacOffDelay = new double[(int)EVacSuckerName.Count];
            _VacShockIntervals = new int[(int)EVacSuckerName.Count];
            _VacShockTimes = new int[(int)EVacSuckerName.Count];
            _VacBtn = new EDI_TYPE[(int)EVacSuckerName.Count];
            _VacBtnLED = new EDO_TYPE[(int)EVacSuckerName.Count];
            _VacIn = new EDI_TYPE[(int)EVacSuckerName.Count][];
            _VacOut = new EDO_TYPE[(int)EVacSuckerName.Count][];
            for (int i = 0; i < (int)EVacSuckerName.Count; i++)
            {
                _AllVacSuckerInfo[i].BtnVac = EDI_TYPE.DI_COUNT;
                _VacBtn[i] = EDI_TYPE.DI_COUNT;

                _AllVacSuckerInfo[i].VacInArr = new EDI_TYPE[2];
                _VacIn[i] = new EDI_TYPE[2];
                for (int j = 0; j < _VacIn[i].Length; j++)
                {
                    _AllVacSuckerInfo[i].VacInArr[j] = EDI_TYPE.DI_COUNT;
                    _VacIn[i][j] = EDI_TYPE.DI_COUNT;
                }

                _AllVacSuckerInfo[i].BtnVacLED = EDO_TYPE.DO_COUNT;
                _VacBtnLED[i] = EDO_TYPE.DO_COUNT;

                _AllVacSuckerInfo[i].VacOn = EDO_TYPE.DO_COUNT;
                _AllVacSuckerInfo[i].VacOff = EDO_TYPE.DO_COUNT;
                _AllVacSuckerInfo[i].VacBreak = EDO_TYPE.DO_COUNT;
                _AllVacSuckerInfo[i].Sucker1 = EDO_TYPE.DO_COUNT;
                _AllVacSuckerInfo[i].Sucker2 = EDO_TYPE.DO_COUNT;
                _VacOut[i] = new EDO_TYPE[5];
                for (int j = 0; j < _VacOut[i].Length; j++)
                    _VacOut[i][j] = EDO_TYPE.DO_COUNT;

                _AllVacSuckerInfo[i].VacOffDelay = 0.0;
                _VacOffDelay[i] = 0.0;
                _VacShockIntervals[i] = 250;
                _VacShockTimes[i] = 4;
            }

            _AllVacSucker = new SingleVacSuckerDef[(int)EVacSuckerName.Count];

            _FilePath = sFilePath;
            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();
        }

        ~VacSuckerDef() { }
        public void Dispose()
        {
            for (int i = 0; i < _AllVacSucker.Length; i++)
                _AllVacSucker[i].Dispose();

            _AllVacSucker = null;
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
            for (int i = 0; i < (int)EVacSuckerName.Count; i++)
            {
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), "Button", EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacSensorFm.VaccumSensor1.ToString(), EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacSensorFm.VaccumSensor2.ToString(), EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), "ButtonLED", EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumOn.ToString(), EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumOff.ToString(), EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumBreak.ToString(), EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.Shock1.ToString(), EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.Shock2.ToString(), EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteDouble(((EVacSuckerName)i).ToString(), "Delay_VacOff", _VacOffDelay[i]);
                iniFileInfo.WriteInt(((EVacSuckerName)i).ToString(), "ShockIntervals", _VacShockIntervals[i]);
                iniFileInfo.WriteInt(((EVacSuckerName)i).ToString(), "ShockTimes", _VacShockTimes[i]);

            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            string _BufStr = "";

            IniFile iniFileInfo = new IniFile(_FilePath, true);
            for (int i = 0; i < (int)EVacSuckerName.Count; i++)
            {
                #region DI
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), "Button", EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _VacBtn[i] = (EDI_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacSensorFm.VaccumSensor1.ToString(), EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _VacIn[i][(int)EVacSensorFm.VaccumSensor1] = (EDI_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacSensorFm.VaccumSensor2.ToString(), EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _VacIn[i][(int)EVacSensorFm.VaccumSensor2] = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region DO
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), "ButtonLED", EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _VacBtnLED[i] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumOn.ToString(), EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _VacOut[i][(int)EVacDOFm.VaccumOn] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumOff.ToString(), EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _VacOut[i][(int)EVacDOFm.VaccumOff] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumBreak.ToString(), EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _VacOut[i][(int)EVacDOFm.VaccumBreak] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacDOFm.Shock1.ToString(), EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _VacOut[i][(int)EVacDOFm.Shock1] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((EVacSuckerName)i).ToString(), EVacDOFm.Shock2.ToString(), EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _VacOut[i][(int)EVacDOFm.Shock2] = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion

                _VacOffDelay[i] = iniFileInfo.ReadDouble(((EVacSuckerName)i).ToString(), "Delay_VacOff", 0.0);
                _VacShockIntervals[i] = iniFileInfo.ReadInt(((EVacSuckerName)i).ToString(), "ShockIntervals", 200);
                _VacShockTimes[i] = iniFileInfo.ReadInt(((EVacSuckerName)i).ToString(), "ShockTimes", 4);
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);
            for (int i = 0; i < (int)EVacSuckerName.Count; i++)
            {
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), "Button", _VacBtn[i].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacSensorFm.VaccumSensor1.ToString(), _VacIn[i][(int)EVacSensorFm.VaccumSensor1].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacSensorFm.VaccumSensor2.ToString(), _VacIn[i][(int)EVacSensorFm.VaccumSensor2].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), "ButtonLED", _VacBtnLED[i].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumOn.ToString(), _VacOut[i][(int)EVacDOFm.VaccumOn].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumOff.ToString(), _VacOut[i][(int)EVacDOFm.VaccumOff].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.VaccumBreak.ToString(), _VacOut[i][(int)EVacDOFm.VaccumBreak].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.Shock1.ToString(), _VacOut[i][(int)EVacDOFm.Shock1].ToString());
                iniFileInfo.WriteStr(((EVacSuckerName)i).ToString(), EVacDOFm.Shock2.ToString(), _VacOut[i][(int)EVacDOFm.Shock2].ToString());
                iniFileInfo.WriteDouble(((EVacSuckerName)i).ToString(), "Delay_VacOff", _VacOffDelay[i]);
                iniFileInfo.WriteInt(((EVacSuckerName)i).ToString(), "ShockIntervals", _VacShockIntervals[i]);
                iniFileInfo.WriteInt(((EVacSuckerName)i).ToString(), "ShockTimes", _VacShockTimes[i]);
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
            for (int i = 0; i < (int)EVacSuckerName.Count; i++)
                _AllVacSucker[i] = new SingleVacSuckerDef(
                    _VacIn[i],
                    _VacBtn[i],
                    _VacBtnLED[i],
                    _VacOut[i][(int)EVacDOFm.VaccumOn],
                    _VacOut[i][(int)EVacDOFm.VaccumOff],
                    _VacOut[i][(int)EVacDOFm.VaccumBreak],
                    _VacOut[i][(int)EVacDOFm.Shock1],
                    _VacOut[i][(int)EVacDOFm.Shock2],
                    _VacOffDelay[i],
                    _VacShockIntervals[i],
                    _VacShockTimes[i]);
        }
        public void SetVacBtn(EVacSuckerName eVacSuckerName, EDI_TYPE eBtn, EDO_TYPE eBtnLED)
        {
            _VacBtn[(int)eVacSuckerName] = eBtn;
            _VacBtnLED[(int)eVacSuckerName] = eBtnLED;
        }
        public void SetVacDI(EVacSuckerName eVacSuckerName, EVacSensorFm eVacSensor, EDI_TYPE eDI_TYPE) { _VacIn[(int)eVacSuckerName][(int)eVacSensor] = eDI_TYPE; }
        public void SetVacDO(EVacSuckerName eVacSuckerName, EVacDOFm eVacDO, EDO_TYPE eDO_TYPE) { _VacOut[(int)eVacSuckerName][(int)eVacDO] = eDO_TYPE; }
        public void SetVacOffDelay(EVacSuckerName eVacSuckerName,double eDelayTime) { _VacOffDelay[(int)eVacSuckerName] = eDelayTime; }
        public void SetVacShockIntervals(EVacSuckerName eVacSuckerName, int iTime) { _VacShockIntervals[(int)eVacSuckerName] = iTime; }
        public void SetVacShockTimes(EVacSuckerName eVacSuckerName, int iTimes) { _VacShockTimes[(int)eVacSuckerName] = iTimes; }
        public EDI_TYPE GetVacBtn(EVacSuckerName eVacSuckerName) { return _VacBtn[(int)eVacSuckerName]; }
        public EDO_TYPE GetVacBtnLED(EVacSuckerName eVacSuckerName) { return _VacBtnLED[(int)eVacSuckerName]; }
        public EDI_TYPE GetVacDI(EVacSuckerName eVacSuckerName, EVacSensorFm eVacSensor) { return _VacIn[(int)eVacSuckerName][(int)eVacSensor]; }
        public EDO_TYPE GetVacDO(EVacSuckerName eVacSuckerName, EVacDOFm eVacDO) { return _VacOut[(int)eVacSuckerName][(int)eVacDO]; }
        public double GetVacOffDelay(EVacSuckerName eVacSuckerName) { return _VacOffDelay[(int)eVacSuckerName]; }
        public int GetVacShockIntervals(EVacSuckerName eVacSuckerName) { return _VacShockIntervals[(int)eVacSuckerName]; }
        public int GetVacShockTimes(EVacSuckerName eVacSuckerName) { return _VacShockTimes[(int)eVacSuckerName]; }

        #region 震動氣缸
        /// <summary>振動完成</summary>
        /// <param name="eSucker">吸盤名稱</param>
        /// <param name="eSuckerTimes">震動次數</param>
        /// <param name="eShockTime">震動間隔時間(單位ms)</param>
        /// <remarks>
        /// <list type="振動完成">震動若未未完成，會自動切會氣缸，直到完成</list>
        /// <list type="震動次數">eSuckerTimes: 震動次數，未給值則使用預設值</list>
        /// <list type="震動間隔時間">eShockTime: 震動間隔時間(單位ms)，未給值則使用預設值</list>
        /// </remarks>
        public bool SuckerDone(EVacSuckerName eSucker, int eSuckerTimes = -1, int eShockTime = -1) { return _AllVacSucker[(int)eSucker].SuckerDone(eSuckerTimes, eShockTime); }
        /// <summary>震動時間初始化</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public void ShockTimeReset(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].ShockTimeReset(); }
        /// <summary>震動氣缸上升</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public void Up(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].Up(); }
        /// <summary>震動氣缸下降</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public void Down(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].Down(); }
        /// <summary>取得前震缸狀態</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public ESuckerMotion GetPreSuckerAction(EVacSuckerName eSucker) { return _AllVacSucker[(int)eSucker].GetPreSuckerAction(); }
        /// <summary>取得震缸狀態</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public ESuckerMotion GetSuckerAction(EVacSuckerName eSucker) { return _AllVacSucker[(int)eSucker].GetSuckerAction(); }
        #endregion

        #region 真空
        /// <summary>取得真空狀態</summary>
        /// <param name="eSucker">吸盤名稱</param>
        /// <param name="eHoldTime">訊號保持時間</param>
        /// <remarks>
        /// <list type="訊號保持時間">eHoldTime: DI On保持時間, On的時間大於指定值輸出On(單位ms)</list>
        /// </remarks>
        /// <returns>對應DI布林值</returns>
        public bool CheckVac(EVacSuckerName eSucker, int eHoldTime = 0) { return _AllVacSucker[(int)eSucker].CheckVac(eHoldTime); }
        /// <summary>取得真空狀態</summary>
        /// <param name="eSucker">吸盤名稱</param>
        /// <returns>對應DI名稱</returns>
        public string GetVacName(EVacSuckerName eSucker) { return _AllVacSucker[(int)eSucker].GetVacName(); }
        /// <summary>按鈕真空Off 開始</summary>
        /// <remarks>
        /// <list type="檢測真空按鈕是否按下">依現行吸盤狀態決定按下按鈕時的動作</list>
        /// </remarks>
        public void CheckBtnClick()
        {
            for (int i = 0; i < _AllVacSucker.Length; i++)
                _AllVacSucker[i].CheckBtnClick();
        }
        /// <summary>按鈕真空Off 開始</summary>
        /// <param name="eSucker">吸盤名稱</param>
        /// <param name="eDelayTime">延遲時間(單位ms)</param>
        /// <remarks>
        /// <list type="按鈕真空Off開始">按鈕按下後開始計時，需搭配BtnOff_Check檢測，否則只是復歸計時器</list>
        /// </remarks>
        public void BtnOff_Start(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].BtnVacOffStart(); }
        /// <summary>按鈕真空Off</summary>
        /// <param name="eSucker">吸盤名稱</param>
        /// <remarks>
        /// <list type="按鈕真空Off">檢測計時時間是否抵達，需搭配BtnOff_Start，否則不會有任何動作</list>
        /// </remarks>
        public void BtnOff_Check(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].BtnVacOff(); }
        /// <summary>真空On</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public void On(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].On(); }
        /// <summary>真空Off</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public void Off(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].Off(); }
        /// <summary>破真空</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public void Break(EVacSuckerName eSucker) { _AllVacSucker[(int)eSucker].Break(); }
        /// <summary>取得前真空狀態</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public EVacMotion GetPreVacAction(EVacSuckerName eSucker) { return _AllVacSucker[(int)eSucker].GetPreVacAction(); }
        /// <summary>取得真空狀態</summary>
        /// <param name="eSucker">吸盤名稱</param>
        public EVacMotion GetVacAction(EVacSuckerName eSucker) { return _AllVacSucker[(int)eSucker].GetVacAction(); }
        #endregion
    }

    public class SingleVacSuckerDef : IDisposable
    {
        public struct VacSuckerInfo
        {
            /// <summary>真空Sensor</summary>
            public EDI_TYPE[] VacInArr;
            public bool[] Readity_VacInArr;
            /// <summary>真空On</summary>
            public EDO_TYPE VacOn;
            public bool Readity_VacOn;
            /// <summary>真空Off</summary>
            public EDO_TYPE VacOff;
            public bool Readity_VacOff;
            /// <summary>真空Break</summary>
            public EDO_TYPE VacBreak;
            public bool Readity_VacBreak;
            /// <summary>震動缸1</summary>
            public EDO_TYPE Sucker1;
            public bool Readity_Sucker1;
            /// <summary>震動缸2</summary>
            public EDO_TYPE Sucker2;
            public bool Readity_Sucker2;
            /// <summary>真空按鈕燈</summary>
            public EDO_TYPE BtnVacLED;
            public bool Readity_BtnVacLED;
            /// <summary>真空按鈕</summary>
            public EDI_TYPE BtnVac;
            public bool Readity_BtnVac;

            /// <summary>真空關閉延遲(Sec)</summary>
            public double VacOffDelay;
        }

        private EVacMotion _PreVacAction;
        private EVacMotion _VacAction;
        private ESuckerMotion _PreSuckerAction;
        private ESuckerMotion _ShockAction;
        private EDO_TYPE _VacOn;
        private EDO_TYPE _VacOff;
        private EDO_TYPE _VacBreak;
        private EDO_TYPE _Sucker1;
        private EDO_TYPE _Sucker2;
        private EDO_TYPE _BtnVacLED;
        private EDI_TYPE _BtnVac;
        private EDI_TYPE[] _VacInArr;
        private bool[] _VacDoneArr;
        private bool _SuckerDone;
        private bool _BtnVacDelayOffStart;
        private int _BtnVacDelayOffStartTime;
        private int _BrnVacDelayOffTime;
        private int[] _VacDoneTimeArr;
        /// <summary>預設震動間隔時間(ms)</summary>
        private int _DefaultShockIntervals;
        /// <summary>預設目前震動次數</summary>
        private int _DefaultShockTimes;
        /// <summary>震動起始TickCount</summary>
        private int _ShockTime;
        /// <summary>目前震動次數</summary>
        private int _ShockTimes;
        private double _VacOffDelay;

        /// <summary>真空吸盤Def</summary>
        /// <param name="eVac_DI">真空SensorDI</param>
        /// <param name="eBtn_DI">真空按鈕DI</param>
        /// <param name="eBtnLED_DO">真空按鈕LED DO</param>
        /// <param name="eVacOn_DO">真空開DO</param>
        /// <param name="eVacOff_DO">真空關DO</param>
        /// <param name="eVacBreak_DO">真空吹DO</param>
        /// <param name="eShock1">振動缸1DO</param>
        /// <param name="eShock2">振動缸2DO</param>
        /// <param name="iShockIntervals">振動間隔時間(ms)</param>
        /// <param name="iShockTimes">振動次數</param>
        /// <remarks>
        /// <list type="真空SensorDI">eVac_DI: 真空SensorDI</list>
        /// <list type="真空按鈕DI">eBtn_DI: 真空按鈕DI</list>
        /// <list type="真空按鈕LED DO">eBtnLED_DO: 真空按鈕LED DO</list>
        /// <list type="真空開DO">eVacOn_DO: 真空開DO</list>
        /// <list type="真空關DO">eVacOff_DO: 真空關DO</list>
        /// <list type="真空吹DO">eVacBreak_DO: 真空吹DO</list>
        /// <list type="振動缸1DO">eShock1: 振動缸1DO</list>
        /// <list type="振動缸2DO">eShock2: 振動缸2DO</list>
        /// <list type="振動間隔時間(ms)">iShockIntervals: 振動間隔時間(ms)</list>
        /// <list type="振動次數">iShockTimes: 振動次數</list>
        /// </remarks>
        public SingleVacSuckerDef(EDI_TYPE[] eVac_DI, EDI_TYPE eBtn_DI, EDO_TYPE eBtnLED_DO, EDO_TYPE eVacOn_DO, EDO_TYPE eVacOff_DO, EDO_TYPE eVacBreak_DO, EDO_TYPE eShock1, EDO_TYPE eShock2, double eDelayTime, int iShockIntervals, int iShockTimes)
        {
            _BtnVacDelayOffStart = false;
            _SuckerDone = true;

            _BtnVacLED = eBtnLED_DO;
            _Sucker1 = eShock1;
            _Sucker2 = eShock2;
            _VacOn = eVacOn_DO;
            _VacOff = eVacOff_DO;
            _VacBreak = eVacBreak_DO;

            _VacOffDelay = eDelayTime;

            _DefaultShockIntervals = iShockIntervals;
            _DefaultShockTimes = iShockTimes;

            _BtnVac = eBtn_DI;
            _VacInArr = eVac_DI;
            _VacDoneArr = new bool[_VacInArr.Length];
            _VacDoneTimeArr = new int[_VacInArr.Length];
            for (int i = 0; i < _VacInArr.Length; i++)
            {
                _VacDoneArr[i] = true;
                _VacDoneTimeArr[i] = Environment.TickCount;
            }

            _PreVacAction = EVacMotion.VacOff;
            _VacAction = EVacMotion.VacOff;
            _PreSuckerAction = ESuckerMotion.Up;
            _ShockAction = ESuckerMotion.Up;
        }

        ~SingleVacSuckerDef() { }
        public void Dispose() { }

        public void CheckBtnClick()
        {
            if (_BtnVac != EDI_TYPE.DI_COUNT)
            {
                switch (G.Comm.IOCtrl.GetDIEdge(_BtnVac))
                {
                    case EDIO_SingleEdge.RisingEdge:
                        if (_BtnVacLED != EDO_TYPE.DO_COUNT)
                            G.Comm.IOCtrl.SetDO(_BtnVacLED, true);
                        if (_VacAction == EVacMotion.VacOn)
                            BtnVacOffStart();
                        else
                            On();
                        break;
                    case EDIO_SingleEdge.FallingEdge:
                        if (_BtnVacLED != EDO_TYPE.DO_COUNT)
                            G.Comm.IOCtrl.SetDO(_BtnVacLED, false);
                        break;
                    default:
                        if (_VacAction == EVacMotion.VacOn)
                            BtnVacOff();
                        break;
                }
            }
        }

        /// <summary>震動氣缸上升</summary>
        public void Up()
        {
            _SuckerDone = false;
            _PreSuckerAction = _ShockAction;

            if (_Sucker1 != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_Sucker1, true);
            if (_Sucker2 != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_Sucker2, true);

            _SuckerDone = true;
            _ShockAction = ESuckerMotion.Up;
        }
        /// <summary>震動氣缸下降</summary>
        public void Down()
        {
            _SuckerDone = false;
            _PreSuckerAction = _ShockAction;

            if (_Sucker1 != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_Sucker1, false);
            if (_Sucker2 != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_Sucker2, false);

            _SuckerDone = true;
            _ShockAction = ESuckerMotion.Down;
        }
        /// <summary>震動氣缸時間初始化</summary>
        public void ShockTimeReset()
        {
            _ShockTimes = 0;
            _ShockTime = Environment.TickCount;
            _SuckerDone = false;
        }
        /// <summary>震動氣缸震動</summary>
        private void Shock(int eShockTime)
        {
            _PreSuckerAction = _ShockAction;

            if (Environment.TickCount - _ShockTime >= eShockTime)
            {
                if (_Sucker1 != EDO_TYPE.DO_COUNT && _Sucker2 != EDO_TYPE.DO_COUNT)
                {
                    if (G.Comm.IOCtrl.GetDO(_Sucker1, true) == G.Comm.IOCtrl.GetDO(_Sucker2, true))
                    {
                        G.Comm.IOCtrl.SetDO(_Sucker1, true);
                        G.Comm.IOCtrl.SetDO(_Sucker2, false);
                    }
                    else
                    {
                        G.Comm.IOCtrl.SetDO(_Sucker1, !G.Comm.IOCtrl.GetDO(_Sucker1, false));
                        G.Comm.IOCtrl.SetDO(_Sucker2, !G.Comm.IOCtrl.GetDO(_Sucker2, true));
                    }
                }
                else
                {
                    if (_Sucker1 != EDO_TYPE.DO_COUNT)
                        G.Comm.IOCtrl.SetDO(_Sucker1, !G.Comm.IOCtrl.GetDO(_Sucker1, false));
                    if (_Sucker2 != EDO_TYPE.DO_COUNT)
                        G.Comm.IOCtrl.SetDO(_Sucker2, !G.Comm.IOCtrl.GetDO(_Sucker2, true));
                }

                _ShockTime = Environment.TickCount;
                _ShockTimes++;
            }
            
            _ShockAction = ESuckerMotion.Shock;
        }
        /// <summary>振動完成</summary>
        public bool SuckerDone(int eSuckerTimes = -1, int eShockTime = -1)
        {
            if (eSuckerTimes < 0)
                eSuckerTimes = _DefaultShockTimes;
            if (eShockTime < 0)
                eShockTime = _DefaultShockIntervals;

            _SuckerDone = _ShockTimes >= eSuckerTimes;
            if (_SuckerDone)
                return true;

            Shock(eShockTime);
            return false;
        }
        /// <summary>取得前震缸狀態</summary>
        public ESuckerMotion GetPreSuckerAction() { return _PreSuckerAction; }
        /// <summary>取得震缸狀態</summary>
        public ESuckerMotion GetSuckerAction() { return _ShockAction; }

        /// <summary>真空On</summary>
        public void On()
        {
            for (int i = 0; i < _VacDoneArr.Length; i++)
                _VacDoneArr[i] = false;

            _PreVacAction = _VacAction;

            if (_VacOn != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacOn, true);
            if (_VacOff != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacOff, false);
            if (_VacBreak != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacBreak, false);

            _VacAction = EVacMotion.VacOn;
        }
        /// <summary>真空Off</summary>
        public void Off()
        {
            for (int i = 0; i < _VacDoneArr.Length; i++)
                _VacDoneArr[i] = false;
            _PreVacAction = _VacAction;

            if (_VacOn != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacOn, false);
            if (_VacOff != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacOff, true);
            if (_VacBreak != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacBreak, false);

            _VacAction = EVacMotion.VacOff;
        }
        /// <summary>破真空</summary>
        public void Break()
        {
            for (int i = 0; i < _VacDoneArr.Length; i++)
                _VacDoneArr[i] = false;
            _PreVacAction = _VacAction;

            if (_VacOn != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacOn, false);
            if (_VacOff != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacOff, true);
            if (_VacBreak != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_VacBreak, true);

            _VacAction = EVacMotion.VacBreak;
        }
        /// <summary>真空延遲Off開始</summary>
        public void BtnVacOffStart()
        {
            if (!_BtnVacDelayOffStart)
            {
                _BtnVacDelayOffStart = true;
                _BrnVacDelayOffTime = (int)(_VacOffDelay * 1000);
                _BtnVacDelayOffStartTime = Environment.TickCount;
            }
        }
        /// <summary>真空延遲Off</summary>
        public void BtnVacOff()
        {
            if (_VacAction == EVacMotion.VacOn)
            {
                if (_BtnVacDelayOffStart)
                {
                    if (Environment.TickCount - _BtnVacDelayOffStartTime >= _BrnVacDelayOffTime)
                    {
                        _BtnVacDelayOffStart = false;
                        Off();
                    }
                }
            }
        }


        /// <summary>取得前真空狀態</summary>
        public EVacMotion GetPreVacAction() { return _PreVacAction; }
        /// <summary>取得真空狀態</summary>
        public EVacMotion GetVacAction() { return _VacAction; }

        /// <summary>真空確認</summary>
        /// <param name="eHoldTime">訊號保持時間</param>
        /// <remarks>
        /// <list type="訊號保持時間">eHoldTime: DI On保持時間, On的時間大於指定值輸出On(單位ms)</list>
        /// </remarks>
        /// <returns>對應DI布林值</returns>
        public bool CheckVac(int eHoldTime = 0)
        {
            bool _Bool = true;
            for (int i = 0; i < _VacInArr.Length; i++)
            {
                if (_VacAction == EVacMotion.VacOn)
                {
                    if (_VacInArr[i] != EDI_TYPE.DI_COUNT)
                    {
                        if (G.Comm.IOCtrl.GetDI(_VacInArr[i], true))
                            _VacDoneArr[i] = true;
                        else
                        {
                            _VacDoneArr[i] = false;
                            _VacDoneTimeArr[i] = Environment.TickCount;
                        }

                        if (!(_VacDoneArr[i] && Environment.TickCount - _VacDoneTimeArr[i] >= eHoldTime))
                            _Bool = false;
                    }
                }
                else if (_VacAction == EVacMotion.VacOff || _VacAction == EVacMotion.VacBreak)
                {
                    if (_VacInArr[i] != EDI_TYPE.DI_COUNT)
                    {
                        if (!G.Comm.IOCtrl.GetDI(_VacInArr[i], false))
                            _VacDoneArr[i] = true;
                        else
                        {
                            _VacDoneArr[i] = false;
                            _VacDoneTimeArr[i] = Environment.TickCount;
                        }

                        if (!(_VacDoneArr[i] && Environment.TickCount - _VacDoneTimeArr[i] >= eHoldTime))
                            _Bool = false;
                    }
                }
            }

            return _Bool;
        }

        /// <summary>真空名稱</summary>
        public string GetVacName()
        {
            string _Str = "";
            for (int i = 0; i < _VacInArr.Length; i++)
            {
                if (_VacInArr[i] != EDI_TYPE.DI_COUNT)
                {
                    if (_VacAction == EVacMotion.VacOn)
                        _Str += G.Comm.IOCtrl.GetDINameWithStatus(_VacInArr[i], true) + "\n";
                    else if (_VacAction == EVacMotion.VacOff || _VacAction == EVacMotion.VacBreak)
                        _Str += G.Comm.IOCtrl.GetDINameWithStatus(_VacInArr[i], false) + "\n";
                    else
                        _Str += "Get " + _VacInArr[i].ToString() + " name fail" + "\n";
                }
            }

            if (_Str == "" || _Str == null)
                return "No setting vaccum sensor";
            else
                return _Str.Substring(0, _Str.LastIndexOf("\n"));
        }
    }
}
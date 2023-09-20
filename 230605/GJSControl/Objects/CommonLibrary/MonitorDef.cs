using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using FileStreamLibrary;

namespace CommonLibrary
{
    public class MonitorDef : IDisposable
    {
        private string _FilePath;
        private MonitorDataTypeDef.EMonitorName[] _Name;
        private string[] _StationNum;
        private string[] _PortName;
        private int[] _BaudRate;
        private Parity[] _Parity;
        private int[] _DataBits;
        private StopBits[] _StopBits;
        private int[] _DelayTime;
        private MonitorDataTypeDef[] _Monitor;

        public List<J_4500Def> MonitorL_J4500 = new List<J_4500Def>();
        public List<Advantech_96PD_THS16BDef> MonitorL_96PDTHS16B = new List<Advantech_96PD_THS16BDef>();
        public List<MESD_SE02Def> MonitorL_SE02 = new List<MESD_SE02Def>();
        public List<ShinLinSPM3Def> MonitorL_SPM3 = new List<ShinLinSPM3Def>();
        public List<EP70Def> MonitorL_EP70 = new List<EP70Def>();
        public SK1000Def MonitorL_SK1000;

        public List<string[]> Value;
        private Thread _ThMonitor;
        private bool _ThMonitorEnd;

        private int Monitor_TickCount = -1;
        private int Monitor_ReadIndex = 0;

        /// <summary>監視器</summary>
        /// <param name="sFilePath">檔案路徑</param>
        /// <remarks>
        /// <list type="檔案路徑">sFilePath: 檔案路徑</list>
        /// </remarks>
        public MonitorDef(string sFilePath = "C:\\Automation\\Monitor.ini")// BoWei Monitor
        {
            _FilePath = sFilePath;

            CreateMonitor();
        }

        ~MonitorDef() { }
        public void Dispose()
        {
            MonitorDispose();
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
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();

            _Monitor = new MonitorDataTypeDef[0];

            _Name = new MonitorDataTypeDef.EMonitorName[0];
            _StationNum = new string[0];
            _PortName = new string[0];
            _BaudRate = new int[0];
            _Parity = new Parity[0];
            _DataBits = new int[0];
            _StopBits = new StopBits[0];
            _DelayTime = new int[0];
        }
        private void ReadFile()
        {
            string _BufStr = "";

            IniFile iniFileInfo = new IniFile(_FilePath, true);
            string[] _AllSection = iniFileInfo.GetSection();
            _Monitor = new MonitorDataTypeDef[_AllSection.Length];

            _Name = new MonitorDataTypeDef.EMonitorName[_Monitor.Length];
            _StationNum = new string[_Monitor.Length];
            _PortName = new string[_Monitor.Length];
            _BaudRate = new int[_Monitor.Length];
            _Parity = new Parity[_Monitor.Length];
            _DataBits = new int[_Monitor.Length];
            _StopBits = new StopBits[_Monitor.Length];
            _DelayTime = new int[_Monitor.Length];

            for (int i = 0; i < _AllSection.Length; i++)
            {
                string[] _SectionArr = _AllSection[i].Split('#');
                if (_SectionArr.Length != 2)
                {
                    _Monitor = new MonitorDataTypeDef[0];
                    break;
                }

                _Name[i] = MonitorDataTypeDef.EMonitorName.Count;
                _StationNum[i] = "";
                _Parity[i] = Parity.Space;
                _StopBits[i] = StopBits.None;

                for (int j = 0; j < (int)MonitorDataTypeDef.EMonitorName.Count; j++)
                {
                    if (_SectionArr[0] == ((MonitorDataTypeDef.EMonitorName)j).ToString())
                    {
                        _Name[i] = (MonitorDataTypeDef.EMonitorName)j;
                        _StationNum[i] = _SectionArr[1];
                        _BufStr = iniFileInfo.ReadStr(_AllSection[i], "Parity", Parity.Even.ToString());
                        for (int k = 0; k < (int)Parity.Space; k++)
                        {
                            if (_BufStr == ((Parity)k).ToString())
                            {
                                _Parity[i] = (Parity)k;
                                break;
                            }
                        }
                        _BufStr = iniFileInfo.ReadStr(_AllSection[i], "StopBits", StopBits.One.ToString());
                        for (int k = 0; k < (int)StopBits.OnePointFive; k++)
                        {
                            if (_BufStr == ((StopBits)k).ToString())
                            {
                                _StopBits[i] = (StopBits)k;
                                break;
                            }
                        }

                        _PortName[i] = iniFileInfo.ReadStr(_AllSection[i], "PortName", "COM99");
                        _BaudRate[i] = iniFileInfo.ReadInt(_AllSection[i], "BaudRate", 9600);
                        _DataBits[i] = iniFileInfo.ReadInt(_AllSection[i], "DataBits", 7);
                        _DelayTime[i] = iniFileInfo.ReadInt(_AllSection[i], "DelayTime", 50);

                        break;
                    }
                }
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            if (CheckFile(_FilePath))
                File.WriteAllText(_FilePath, string.Empty);

            IniFile iniFileInfo = new IniFile(_FilePath, false);

            for (int i = 0; i < _Monitor.Length; i++)
            {
                string _Section = _Name[i].ToString() + "#" + _StationNum[i];
                iniFileInfo.WriteStr(_Section, "PortName", _PortName[i]);
                iniFileInfo.WriteInt(_Section, "BaudRate", _BaudRate[i]);
                iniFileInfo.WriteStr(_Section, "Parity", _Parity[i].ToString());
                iniFileInfo.WriteInt(_Section, "DataBits", _DataBits[i]);
                iniFileInfo.WriteStr(_Section, "StopBits", _StopBits[i].ToString());
                iniFileInfo.WriteInt(_Section, "DelayTime", _DelayTime[i]);
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
            for (int i = 0; i < _Monitor.Length; i++)
                _Monitor[i] = new MonitorDataTypeDef(_Name[i], _StationNum[i], _PortName[i], _BaudRate[i], _Parity[i], _DataBits[i], _StopBits[i], _DelayTime[i]);
        }

        public void AddMonitor(MonitorDataTypeDef.EMonitorName eName, string eStationNum, string ePortName, int eBaudRate, Parity eParity, int eDataBits, StopBits eStopBits, int eDelayTime)
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);

            string _Section = eName.ToString() + "#" + eStationNum;
            iniFileInfo.WriteStr(_Section, "PortName", ePortName);
            iniFileInfo.WriteInt(_Section, "BaudRate", eBaudRate);
            iniFileInfo.WriteStr(_Section, "Parity", eParity.ToString());
            iniFileInfo.WriteInt(_Section, "DataBits", eDataBits);
            iniFileInfo.WriteStr(_Section, "StopBits", eStopBits.ToString());
            iniFileInfo.WriteInt(_Section, "DelayTime", eDelayTime * 50);

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();

            ReadFile();
            ReNew();
        }
        public void RemoveMonitor(int eIndex)
        {
            if (CheckFile(_FilePath))
                File.WriteAllText(_FilePath, string.Empty);

            IniFile iniFileInfo = new IniFile(_FilePath, false);

            for (int i = 0; i < _Monitor.Length; i++)
            {
                if (i != eIndex)
                {
                    string _Section = _Name[i].ToString() + "#" + _StationNum[i];
                    iniFileInfo.WriteStr(_Section, "PortName", _PortName[i]);
                    iniFileInfo.WriteInt(_Section, "BaudRate", _BaudRate[i]);
                    iniFileInfo.WriteStr(_Section, "Parity", _Parity[i].ToString());
                    iniFileInfo.WriteInt(_Section, "DataBits", _DataBits[i]);
                    iniFileInfo.WriteStr(_Section, "StopBits", _StopBits[i].ToString());
                    iniFileInfo.WriteInt(_Section, "DelayTime", _DelayTime[i]);
                }
            }

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();

            ReadFile();
        }
        public int GetMonitorNum() { return _Monitor.Length; }
        public string[] GetMonitorSimpleData(int eIndex)
        {
            string[] _StrBuf = new string[8];
            _StrBuf[0] = GetMonitorName(eIndex).ToString();
            _StrBuf[1] = GetPortName(eIndex);
            _StrBuf[2] = GetStationNum(eIndex);
            _StrBuf[3] = GetBaudRate(eIndex).ToString();
            _StrBuf[4] = GetParity(eIndex).ToString();
            _StrBuf[5] = GetDataBits(eIndex).ToString();
            _StrBuf[6] = GetStopBits(eIndex).ToString();
            _StrBuf[7] = GetDelayTime(eIndex).ToString();

            return _StrBuf;
        }

        public void SetMonitorName(int eIndex, MonitorDataTypeDef.EMonitorName eValue) { _Name[eIndex] = eValue; }
        public void SetStationNum(int eIndex, string eValue) { _StationNum[eIndex] = eValue; }
        public void SetPortName(int eIndex, string eValue) { _PortName[eIndex] = eValue; }
        public void SetBaudRate(int eIndex, int eValue) { _BaudRate[eIndex] = eValue; }
        public void SetParity(int eIndex, Parity eValue) { _Parity[eIndex] = eValue; }
        public void SetDataBits(int eIndex, int eValue) { _DataBits[eIndex] = eValue; }
        public void SetStopBits(int eIndex, StopBits eValue) { _StopBits[eIndex] = eValue; }
        public void SetDelayTime(int eIndex, int eValue) { _DelayTime[eIndex] = eValue; }

        public MonitorDataTypeDef.EMonitorName GetMonitorName(int eIndex) { return _Name[eIndex]; }
        public string GetStationNum(int eIndex) { return _StationNum[eIndex]; }
        public string GetPortName(int eIndex) { return _PortName[eIndex]; }
        public int GetBaudRate(int eIndex) { return _BaudRate[eIndex]; }
        public Parity GetParity(int eIndex) { return _Parity[eIndex]; }
        public int GetDataBits(int eIndex) { return _DataBits[eIndex]; }
        public StopBits GetStopBits(int eIndex) { return _StopBits[eIndex]; }
        public int GetDelayTime(int eIndex) { return _DelayTime[eIndex]; }

        public string[] GetFileSection()
        {
            string[] _AllSection;
            IniFile iniFileInfo = new IniFile(_FilePath, true);
            _AllSection = iniFileInfo.GetSection();
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();

            return _AllSection;
        }

        public void CreateMonitor()
        {
            MonitorDispose();

            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();

            Value = new List<string[]>();
            AllMonitorListClear();

            List<ModbusRTU> modbus = new List<ModbusRTU>();

            for (int i = 0; i < _Monitor.Length; i++)
            {
                switch (GetMonitorName(i))
                {
                    case MonitorDataTypeDef.EMonitorName._96PDTHS16B:
                        MonitorL_96PDTHS16B.Add(new Advantech_96PD_THS16BDef(ref modbus, GetStationNum(i), GetPortName(i), GetBaudRate(i), GetParity(i), GetDataBits(i), GetStopBits(i), GetDelayTime(i)));
                        break;
                    case MonitorDataTypeDef.EMonitorName.J4500:
                        MonitorL_J4500.Add(new J_4500Def(ref modbus, GetStationNum(i), GetPortName(i), GetBaudRate(i), GetParity(i), GetDataBits(i), GetStopBits(i), GetDelayTime(i)));
                        break;
                    case MonitorDataTypeDef.EMonitorName.MESDSE02:
                        MonitorL_SE02.Add(new MESD_SE02Def(ref modbus, GetStationNum(i), GetPortName(i), GetBaudRate(i), GetParity(i), GetDataBits(i), GetStopBits(i), GetDelayTime(i)));
                        break;
                    case MonitorDataTypeDef.EMonitorName.SPM3:
                        MonitorL_SPM3.Add(new ShinLinSPM3Def(ref modbus, GetStationNum(i), GetPortName(i), GetBaudRate(i), GetParity(i), GetDataBits(i), GetStopBits(i), GetDelayTime(i)));
                        break;
                    case MonitorDataTypeDef.EMonitorName.EP70:
                        MonitorL_EP70.Add(new EP70Def(ref modbus, GetStationNum(i), GetPortName(i), GetBaudRate(i), GetParity(i), GetDataBits(i), GetStopBits(i), GetDelayTime(i)));
                        break;
                    case MonitorDataTypeDef.EMonitorName.SK1000:
                        MonitorL_SK1000 = new SK1000Def(GetStationNum(i), GetPortName(i), GetBaudRate(i), GetParity(i), GetDataBits(i), GetStopBits(i), GetDelayTime(i));
                        break;
                }
            }

            _ThMonitorEnd = false;
            _ThMonitor = new Thread(MonitorReNew)
            {
                Priority = ThreadPriority.Lowest
            };
            _ThMonitor.Start();
        }
        public void MonitorReNew()
        {
            while (!_ThMonitorEnd)
            {
                int _AllMonitorNum = MonitorL_J4500.Count +
                MonitorL_96PDTHS16B.Count +
                MonitorL_SE02.Count +
                MonitorL_SPM3.Count +
                MonitorL_EP70.Count;

                if (Monitor_TickCount < 0 || Environment.TickCount - Monitor_TickCount > 50)
                {
                    Monitor_TickCount = Environment.TickCount;

                    if (Monitor_ReadIndex < MonitorL_J4500.Count)
                        MonitorL_J4500[Monitor_ReadIndex].RenewValue();
                    else if (Monitor_ReadIndex - MonitorL_J4500.Count >= 0 && Monitor_ReadIndex - MonitorL_J4500.Count < MonitorL_96PDTHS16B.Count)
                        MonitorL_96PDTHS16B[Monitor_ReadIndex - MonitorL_J4500.Count].RenewValue();
                    else if (Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count >= 0 && Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count < MonitorL_SE02.Count)
                        MonitorL_SE02[Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count].RenewValue();
                    else if (Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count - MonitorL_SE02.Count >= 0 && Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count - MonitorL_SE02.Count < MonitorL_SPM3.Count)
                        MonitorL_SPM3[Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count - MonitorL_SE02.Count].RenewValue();
                    else if (Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count - MonitorL_SE02.Count - MonitorL_SPM3.Count >= 0 && Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count - MonitorL_SE02.Count - MonitorL_SPM3.Count < MonitorL_EP70.Count)
                        MonitorL_EP70[Monitor_ReadIndex - MonitorL_J4500.Count - MonitorL_96PDTHS16B.Count - MonitorL_SE02.Count - MonitorL_SPM3.Count].RenewValue();

                    Monitor_ReadIndex++;

                    if (Monitor_ReadIndex > _AllMonitorNum)
                        Monitor_ReadIndex = 0;
                }

                if (MonitorL_SK1000 != null)
                    MonitorL_SK1000.RenewValue();

                #region MonitorValue Update
                #region RS485
                #region SPM3
                for (int i = 0; i < MonitorL_SPM3.Count; i++)
                    MonitorL_SPM3[i].FindValue(ref Value, i + 1, MonitorL_SPM3[i].GetInedx(ShinLinSPM3Def.RealTimeData.kW_total));
                #endregion

                #region J4500
                for (int i = 0; i < MonitorL_J4500.Count; i++)
                    MonitorL_J4500[i].FindValue(ref Value, i + 1, 0);
                #endregion

                #region SE02(靜電)
                for (int i = 0; i < MonitorL_SE02.Count; i++)
                    MonitorL_SE02[i].FindValue(ref Value, i + 1, 0);
                #endregion

                #region 96PDTHS16B(研華溫濕度)
                for (int i = 0; i < MonitorL_96PDTHS16B.Count; i++)
                {
                    MonitorL_96PDTHS16B[i].FindValue(ref Value, i + 1, (int)Advantech_96PD_THS16BDef.RealTimeData.Temperature);
                    MonitorL_96PDTHS16B[i].FindValue(ref Value, i + 1, (int)Advantech_96PD_THS16BDef.RealTimeData.Humidity);
                }
                #endregion

                #region EP70
                for (int i = 0; i < MonitorL_EP70.Count; i++)
                    MonitorL_EP70[i].FindValue(ref Value, i + 1, 0);
                #endregion
                #endregion

                #region RS232
                #region SK1000(基恩斯溫濕度)
                if (MonitorL_SK1000 != null)
                {
                    for (int i = 0; i < 3; i++)
                        MonitorL_SK1000.FindValue(ref Value, 1, i);
                }
                #endregion
                #endregion
                #endregion

                Thread.Sleep(1);
            }
        }
        private void AllMonitorListClear()
        {
            #region RS485
            if (MonitorL_J4500 != null)
                MonitorL_J4500.Clear();
            if (MonitorL_96PDTHS16B != null)
                MonitorL_96PDTHS16B.Clear();
            if (MonitorL_SE02 != null)
                MonitorL_SE02.Clear();
            if (MonitorL_SPM3 != null)
                MonitorL_SPM3.Clear();
            if (MonitorL_EP70 != null)
                MonitorL_EP70.Clear();
            #endregion

            #region RS232
            MonitorL_SK1000 = null;
            #endregion
        }
        private void MonitorDispose()
        {
            _ThMonitorEnd = true;
            while (_ThMonitor != null && _ThMonitor.IsAlive)
            {
                Thread.Sleep(5);
            }

            #region RS485
            for (int i = 0; i < MonitorL_J4500.Count; i++)
                MonitorL_J4500[i].Dispose();
            for (int i = 0; i < MonitorL_96PDTHS16B.Count; i++)
                MonitorL_96PDTHS16B[i].Dispose();
            for (int i = 0; i < MonitorL_SE02.Count; i++)
                MonitorL_SE02[i].Dispose();
            for (int i = 0; i < MonitorL_SPM3.Count; i++)
                MonitorL_SPM3[i].Dispose();
            for (int i = 0; i < MonitorL_EP70.Count; i++)
                MonitorL_EP70[i].Dispose();
            #endregion

            #region RS232
            if (MonitorL_SK1000 != null)
                MonitorL_SK1000.Dispose();
            #endregion
        }
    }
}
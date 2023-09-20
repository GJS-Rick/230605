using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileStreamLibrary;

namespace CommonLibrary
{
    public class DeltaVFD : IDisposable
    {
        public enum Basic
        {
            VFDReset,

            External,
            RS485,
            MaxFrequency,

            OtherSet
        }
        public enum AccAndDec
        {
            First,
            Second,
            Third,
            Fourth
        }
        public enum SpeedName
        {
            Speed_Main,
            Speed_1st,
            Speed_2nd,
            Speed_3rd,
            Speed_4th,
            Speed_5th,
            Speed_6th,
            Speed_7th,
            Speed_8th,
            Speed_9th,
            Speed_10th,
            Speed_11th,
            Speed_12th,
            Speed_13th,
            Speed_14th,
            Speed_15th,

            Count
        }

        readonly String _filePath;
        readonly String _fileName;

        readonly SerialPort _SerialPort;
        int _station;
        string _portName;
        int _baudRate = 9600;
        Parity _parity = Parity.None;
        int _dataBits = 8;
        StopBits _stopBits = StopBits.One;
        int _delayTimeAfterSendMessage;
        int[] _speed = new int[(int)SpeedName.Count];

        const int _ReadTimeout = 50;
        const int _WriteTimeout = 50;
        const int _HandShakeTimeout = 50;
        readonly int _DelayTimeAfterSendMessage;
        ushort[] _paramValue;

        #region IniFile
        private enum InfoList
        {
            StationNo,
            PortName,
            DelayTimeAfterSendMessage,
            
            Count
        }
        private void DirExistsAndCreate()
        {
            if (!Directory.Exists(_filePath))
                try { Directory.CreateDirectory(_filePath); } catch (Exception) { }

            if (File.Exists(_filePath + "\\" + _fileName)) GetParam();
            else try { CreateIniInfo(); } catch (Exception) { }
        }
        private void CreateIniInfo()
        {
            IniFile cIniFileInfo = new IniFile(_filePath + "\\" + _fileName, false);

            for (int i = 0; i < (int)InfoList.Count; i++)
            {
                switch ((InfoList)i)
                {
                    case InfoList.StationNo:
                        cIniFileInfo.WriteInt("Info", "StationNo", 1);
                        break;
                    case InfoList.PortName:
                        cIniFileInfo.WriteStr("Info", "PortName", "COM2");
                        break;
                    case InfoList.DelayTimeAfterSendMessage:
                        cIniFileInfo.WriteInt("Info", "DelayTimeAfterSendMessage", 50);
                        break;
                }
            }

            for (int i = 0; i < (int)SpeedName.Count; i++)
            {
                if (i == 0)
                    cIniFileInfo.WriteDouble("Parameter", Enum.GetName(typeof(SpeedName), i), 60);
                else
                    cIniFileInfo.WriteDouble("Parameter", Enum.GetName(typeof(SpeedName), i), 0);
            }

            cIniFileInfo.FileClose();
            cIniFileInfo.Dispose();

            GetParam();
        }
        private void GetParam()
        {
            IniFile cIniFileInfo = new IniFile(_filePath + "\\" + _fileName, false);

            for (int i = 0; i < (int)InfoList.Count; i++)
            {
                switch ((InfoList)i)
                {
                    case InfoList.StationNo:
                        _station = cIniFileInfo.ReadInt("Info", Enum.GetName(typeof(InfoList), i), 1);
                        break;
                    case InfoList.PortName:
                        _portName = cIniFileInfo.ReadStr("Info", Enum.GetName(typeof(InfoList), i), "COM2");
                        break;
                    case InfoList.DelayTimeAfterSendMessage:
                        _delayTimeAfterSendMessage = cIniFileInfo.ReadInt("Info", Enum.GetName(typeof(InfoList), i), 50);
                        break;
                }
            }

            for (int i = 0; i < (int)SpeedName.Count; i++)
                _speed[i] = (int)(Math.Round(cIniFileInfo.ReadDouble("Parameter", Enum.GetName(typeof(SpeedName), i), 0.0) * 100, 0));

            cIniFileInfo.FileClose();
            cIniFileInfo.Dispose();
        }
        public void SaveSpeed(SpeedName eSpeedName, double Value)
        {
            _speed[(int)eSpeedName] = (int)Math.Round((Value * 100), 0);
            if (_speed[(int)eSpeedName] < 0) _speed[(int)eSpeedName] = 0;
            if (_speed[(int)eSpeedName] > 60 * 100) _speed[(int)eSpeedName] = 60 * 100;

            IniFile cIniFileInfo = new IniFile(_filePath + "\\" + _fileName, false);

            for (int i = 0; i < (int)SpeedName.Count; i++)
                cIniFileInfo.WriteDouble("Parameter", Enum.GetName(typeof(SpeedName), i), (double)(_speed[i] / 100.0));

            cIniFileInfo.FileClose();
            cIniFileInfo.Dispose();

            SetToVFD(eSpeedName);
        }

        public double ReadSpeedByFile(SpeedName eSpeedName)
        {
            double speedBuf = 0;

            IniFile cIniFileInfo = new IniFile(_filePath + "\\" + _fileName, true);

            for (int i = 0; i < (int)SpeedName.Count; i++)
                _speed[i] = (int)(Math.Round(cIniFileInfo.ReadDouble("Parameter", Enum.GetName(typeof(SpeedName), i), 0.0) * 100, 0));

            cIniFileInfo.FileClose();
            cIniFileInfo.Dispose();

            speedBuf = _speed[(int)eSpeedName] / 100.0;

            return speedBuf;
        }

        public void SetAllToVFD()
        {
            IniFile cIniFileInfo = new IniFile(_filePath + "\\" + _fileName, true);

            for (int i = 0; i < (int)SpeedName.Count; i++)
            {
                _speed[i] = (int)(Math.Round(cIniFileInfo.ReadDouble("Parameter", Enum.GetName(typeof(SpeedName), i), 0.0) * 100, 0));
                if (_speed[i] < 0) _speed[i] = 0;
                if (_speed[i] > 6000) _speed[i] = 6000;

                try { SetSpeed(_station, (SpeedName)i); }
                catch (Exception) { throw; }
            }

            cIniFileInfo.FileClose();
            cIniFileInfo.Dispose();
        }
        public void SetToVFD(SpeedName eSpeedName)
        {
            IniFile cIniFileInfo = new IniFile(_filePath + "\\" + _fileName, true);

            _speed[(int)eSpeedName] = (int)(Math.Round(cIniFileInfo.ReadDouble("Parameter", Enum.GetName(typeof(SpeedName), (int)eSpeedName), 0.0) * 100, 0));
            if (_speed[(int)eSpeedName] < 0) _speed[(int)eSpeedName] = 0;
            if (_speed[(int)eSpeedName] > 6000) _speed[(int)eSpeedName] = 6000;

            try
            {
                SetSpeed(_station, eSpeedName);
            }
            catch (Exception)
            {
                throw;
            }

            cIniFileInfo.FileClose();
            cIniFileInfo.Dispose();
        }
        public void SetToVFD(SpeedName eSpeedName, double Value)
        {
            _speed[(int)eSpeedName] = (int)(Math.Round(Value * 100, 0));
            if (_speed[(int)eSpeedName] < 0) _speed[(int)eSpeedName] = 0;
            if (_speed[(int)eSpeedName] > 6000) _speed[(int)eSpeedName] = 6000;

            try
            {
                SetSpeed(_station, eSpeedName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public DeltaVFD(String sfilePath, SerialPort[] ConnectedPorts)
        {
            bool _portUsed = false;

            _filePath = sfilePath;
            _fileName = "DeltaVFD.ini";

            _baudRate = 9600;
            _parity = Parity.None;
            _dataBits = 8;
            _stopBits = StopBits.One;
            
            DirExistsAndCreate();

            try
            {
                foreach (SerialPort UsedPort in ConnectedPorts)
                {
                    if (_portName == UsedPort.PortName)
                    {
                        _SerialPort = UsedPort;
                        _portUsed = true;
                        break;
                    }
                }
                
                if (!_portUsed)
                {
                    _SerialPort = new SerialPort(_portName, _baudRate, _parity, _dataBits, _stopBits);
                    _SerialPort.ReadTimeout = _ReadTimeout;
                    _SerialPort.WriteTimeout = _WriteTimeout;

                    _SerialPort.Open();
                }

                _DelayTimeAfterSendMessage = _delayTimeAfterSendMessage;

                _BasicSet(_station, Basic.RS485);
                SetAccAndDec(_station, 50, AccAndDec.First);
                _BasicSet(_station, Basic.MaxFrequency);
                _BasicSet(_station, Basic.OtherSet);
            }
            catch
            {
                throw;
            }

            #region 變頻器需變更參數
            /*
                00-20(頻率控制選擇)：1
                00-23(運轉方向選擇)：0
                09-00(站號)：1
                09-01(傳輸速度)：9.6
                09-04(通訊格式)：12
            */
            #endregion
        }

        /// <summary>基本設定</summary>
        private void _BasicSet(int Station, Basic CtlMode)
        {
            switch (CtlMode)
            {
                case Basic.VFDReset:
                    //00-02 參數管理設定
                    _paramValue = new ushort[] { 10 };
                    WriteWords(Station, "0002", _paramValue);
                    break;
                case Basic.External:
                    //00-20 頻率指令來源設定（AUTO、REMOTE）
                    _paramValue = new ushort[] { 0 };
                    WriteWords(Station, "0014", _paramValue);

                    //00-21 運轉指令來源設定（AUTO、REMOTE）
                    _paramValue = new ushort[] { 0 };//數位操作器操作
                    WriteWords(Station, "0015", _paramValue);

                    //00-30 頻率指令來源設定（HAND、LOCAL）
                    _paramValue = new ushort[] { 0 };
                    WriteWords(Station, "001E", _paramValue);

                    //00-31 運轉指令來源設定（HAND、LOCAL）
                    _paramValue = new ushort[] { 0 };//數位操作器操作
                    WriteWords(Station, "001F", _paramValue);
                    break;
                case Basic.RS485:
                    //00-20 頻率指令來源設定（AUTO、REMOTE）
                    _paramValue = new ushort[] { 1 };
                    WriteWords(Station, "0014", _paramValue);

                    //00-21 運轉指令來源設定（AUTO、REMOTE）
                    _paramValue = new ushort[] { 1 };//外部端子操作
                    WriteWords(Station, "0015", _paramValue);

                    //00-30 頻率指令來源設定（HAND、LOCAL）
                    _paramValue = new ushort[] { 1 };
                    WriteWords(Station, "001E", _paramValue);

                    //00-31 運轉指令來源設定（HAND、LOCAL）
                    _paramValue = new ushort[] { 1 };//外部端子操作
                    WriteWords(Station, "001F", _paramValue);
                    break;
                case Basic.MaxFrequency:
                    //01-00 最高操作頻率
                    _paramValue = new ushort[] { 6000 };
                    WriteWords(Station, "0100", _paramValue);
                    break;
                case Basic.OtherSet:
                    //00-22 停車方式
                    _paramValue = new ushort[] { 0 };
                    WriteWords(Station, "0016", _paramValue);

                    //00-23 運轉方向選擇
                    _paramValue = new ushort[] { 0 };
                    WriteWords(Station, "0017", _paramValue);
                    break;
            }
        }
        private void _SetAccDec_DeltaVFD(int Station, AccAndDec CtlMode, ushort Value)
        {
            _paramValue = new ushort[] { Value };
            switch (CtlMode)
            {
                case AccAndDec.First:
                    {
                        //01-12 第⼀加速時間設定
                        WriteWords(Station, "010C", _paramValue);
                        //01-13 第⼀減速時間設定
                        WriteWords(Station, "010D", _paramValue);
                    }
                    break;

                case AccAndDec.Second:
                    {
                        //01-14 第二加速時間設定
                        WriteWords(Station, "010E", _paramValue);
                        //01-15 第二減速時間設定
                        WriteWords(Station, "010F", _paramValue);
                    }
                    break;

                case AccAndDec.Third:
                    {
                        //01-16 第三加速時間設定
                        WriteWords(Station, "0110", _paramValue);
                        //01-17 第三減速時間設定
                        WriteWords(Station, "0111", _paramValue);
                    }
                    break;

                case AccAndDec.Fourth:
                    {
                        //01-18 第四加速時間設定
                        WriteWords(Station, "0112", _paramValue);
                        //01-19 第四減速時間設定
                        WriteWords(Station, "0113", _paramValue);
                    }
                    break;
            }
        }
        private void _SetSpeed_DeltaVFD(int Station, SpeedName CtlMode, ushort Value)
        {
            _paramValue = new ushort[] { Value };
            switch (CtlMode)
            {
                case SpeedName.Speed_Main:
                    {
                        //20-01 頻率命令（XXX.XX Hz）
                        WriteWords(Station, "2001", _paramValue);
                    }
                    break;

                case SpeedName.Speed_1st:
                    {
                        //04-00 第⼀段速
                        WriteWords(Station, "0400", _paramValue);
                    }
                    break;

                case SpeedName.Speed_2nd:
                    {
                        //04-01 第二段速
                        WriteWords(Station, "0401", _paramValue);
                    }
                    break;

                case SpeedName.Speed_3rd:
                    {
                        //04-02 第三段速
                        WriteWords(Station, "0402", _paramValue);
                    }
                    break;

                case SpeedName.Speed_4th:
                    {
                        //04-03 第四段速
                        WriteWords(Station, "0403", _paramValue);
                    }
                    break;

                case SpeedName.Speed_5th:
                    {
                        //04-04 第五段速
                        WriteWords(Station, "0404", _paramValue);
                    }
                    break;

                case SpeedName.Speed_6th:
                    {
                        //04-05 第六段速
                        WriteWords(Station, "0405", _paramValue);
                    }
                    break;

                case SpeedName.Speed_7th:
                    {
                        //04-06 第七段速
                        WriteWords(Station, "0406", _paramValue);
                    }
                    break;

                case SpeedName.Speed_8th:
                    {
                        //04-07 第八段速
                        WriteWords(Station, "0407", _paramValue);
                    }
                    break;

                case SpeedName.Speed_9th:
                    {
                        //04-08 第九段速
                        WriteWords(Station, "0408", _paramValue);
                    }
                    break;

                case SpeedName.Speed_10th:
                    {
                        //04-09 第十段速
                        WriteWords(Station, "0409", _paramValue);
                    }
                    break;

                case SpeedName.Speed_11th:
                    {
                        //04-10 第十一段速
                        WriteWords(Station, "040A", _paramValue);
                    }
                    break;

                case SpeedName.Speed_12th:
                    {
                        //04-11 第十二段速
                        WriteWords(Station, "040B", _paramValue);
                    }
                    break;

                case SpeedName.Speed_13th:
                    {
                        //04-12 第十三段速
                        WriteWords(Station, "040C", _paramValue);
                    }
                    break;

                case SpeedName.Speed_14th:
                    {
                        //04-13 第十四段速
                        WriteWords(Station, "040D", _paramValue);
                    }
                    break;

                case SpeedName.Speed_15th:
                    {
                        //04-14 第十五段速
                        WriteWords(Station, "040E", _paramValue);
                    }
                    break;
            }
        }
        public void SetAccAndDec(int Station, ushort AccDectime, AccAndDec Sort)
        {
            switch (Sort)
            {
                case AccAndDec.First: _SetAccDec_DeltaVFD(Station, AccAndDec.First, AccDectime); break;
                case AccAndDec.Second: _SetAccDec_DeltaVFD(Station, AccAndDec.Second, AccDectime); break;
                case AccAndDec.Third: _SetAccDec_DeltaVFD(Station, AccAndDec.Third, AccDectime); break;
                case AccAndDec.Fourth: _SetAccDec_DeltaVFD(Station, AccAndDec.Fourth, AccDectime); break;
            }
        }
        public void SetSpeed(int Station, SpeedName Sort)
        {
            _SetSpeed_DeltaVFD(Station, Sort, (ushort)_speed[(int)Sort]);
        }

        /// <summary>取得站號</summary>
        public int GetStationNo()
        {
            return _station;
        }
        public int GetSpeed(int Station, SpeedName Name)
        {
            switch (Name)
            {
                case SpeedName.Speed_Main: return GetWordValue(Station, "2103", 1)[0];//21-02 參數管理設定
                case SpeedName.Speed_1st: return GetWordValue(Station, "0400", 1)[0];//04-00 參數管理設定
                case SpeedName.Speed_2nd: return GetWordValue(Station, "0401", 1)[0];//04-01 參數管理設定
                case SpeedName.Speed_3rd: return GetWordValue(Station, "0402", 1)[0];//04-02 參數管理設定
                case SpeedName.Speed_4th: return GetWordValue(Station, "0403", 1)[0];//04-03 參數管理設定
                case SpeedName.Speed_5th: return GetWordValue(Station, "0404", 1)[0];//04-04 參數管理設定
                case SpeedName.Speed_6th: return GetWordValue(Station, "0405", 1)[0];//04-05 參數管理設定
                case SpeedName.Speed_7th: return GetWordValue(Station, "0406", 1)[0];//04-06 參數管理設定
                case SpeedName.Speed_8th: return GetWordValue(Station, "0407", 1)[0];//04-07 參數管理設定
                case SpeedName.Speed_9th: return GetWordValue(Station, "0408", 1)[0];//04-08 參數管理設定
                case SpeedName.Speed_10th: return GetWordValue(Station, "0409", 1)[0];//04-09 參數管理設定
                case SpeedName.Speed_11th: return GetWordValue(Station, "0410", 1)[0];//04-10 參數管理設定
                case SpeedName.Speed_12th: return GetWordValue(Station, "0411", 1)[0];//04-11 參數管理設定
                case SpeedName.Speed_13th: return GetWordValue(Station, "0412", 1)[0];//04-12 參數管理設定
                case SpeedName.Speed_14th: return GetWordValue(Station, "0413", 1)[0];//04-13 參數管理設定
                case SpeedName.Speed_15th: return GetWordValue(Station, "0414", 1)[0];//04-14 參數管理設定
                default: return 0;
            }
        }

        /// <summary>參數重置</summary><param name="Station">站號</param>
        public void Reset(int Station)
        {
            _BasicSet(Station, Basic.VFDReset);
        }
        /// <summary>外部控制</summary><param name="Station">站號</param>
        public void ExternalControl(int Station)
        {
            _BasicSet(Station, Basic.External);
        }

        #region 勿動
        public byte[] GenerateCRC(byte[] message)
        {
            uint crc16 = 0xFFFF;
            uint temp;
            uint flag;

            for (int i = 0; i < message.Length; i++)
            {
                temp = (uint)message[i]; // temp has the first byte 
                temp &= 0x00FF; // mask the MSB 
                crc16 ^= temp; //crc16 XOR with temp 
                for (uint c = 0; c < 8; c++)
                {
                    flag = crc16 & 0x01; // LSBit di crc16 is mantained
                    crc16 >>= 1; // Lsbit di crc16 is lost 
                    if (flag != 0)
                        crc16 ^= 0x0A001; // crc16 XOR with 0x0a001 
                }
            }
            //crc16 = (crc16 >> 8) | (crc16 << 8); // LSB is exchanged with MSB

            byte lowByte = (byte)(crc16 & 0xff);
            byte highByte = (byte)((crc16 >> 8) & 0xff);
            return new byte[] { lowByte, highByte };
        }

        public bool VerifyCRC(byte[] message)
        {
            if (!(message.Length > 4))
                return false;
            byte[] messageWithoutCRC = new byte[message.Length - 2];
            Array.Copy(message, messageWithoutCRC, message.Length - 2);

            byte[] messageCRC = { message[message.Length - 2], message[message.Length - 1] };
            byte[] verifyCRC = GenerateCRC(messageWithoutCRC);

            if (messageCRC[0] - verifyCRC[0] == 0 &&
                messageCRC[1] - verifyCRC[1] == 0)
                return true;
            else
                return false;
        }

        private byte[] SendForGet(byte[] queryMessage, int ResultLength)
        {
            if (!_SerialPort.IsOpen)
                return null;

            int retryCount = 0;
            List<byte> responseMessageList = new List<byte>();
        retrySend:
            try
            {

                _SerialPort.Write(queryMessage, 0, queryMessage.Length);
                Thread.Sleep(_DelayTimeAfterSendMessage);
                if (queryMessage[0] == 0)  //broadcast
                    return new byte[] { };
                responseMessageList.Clear();
                Stopwatch sw = Stopwatch.StartNew();
                while (true)
                {
                    System.Threading.Thread.Sleep(1);
                    byte[] recieveMessage = new byte[_SerialPort.BytesToRead];
                    int recieveCount = _SerialPort.Read(recieveMessage, 0, recieveMessage.Length);
                    if (recieveCount > 0)
                        responseMessageList.AddRange(recieveMessage);
                    if (VerifyCRC(responseMessageList.ToArray()) && responseMessageList.Count == ResultLength)
                    {
                        break;
                    }
                    if (sw.ElapsedMilliseconds > _HandShakeTimeout)
                    {
                        retryCount++;
                        if (retryCount < 10)
                            goto retrySend;
                        if (responseMessageList.Count == 0)
                            throw new Exception(_SerialPort.PortName + " # " + queryMessage[0].ToString() + " : No response.");
                        throw new Exception(_SerialPort.PortName + " # " + queryMessage[0].ToString() + " : HandShakeTimeout time out");
                    }
                }

                return responseMessageList.ToArray();
            }

            catch
            {
                throw;
            }
        }

        private byte[] Send(byte[] queryMessage)
        {
            if (!_SerialPort.IsOpen)
                return null;

            int retryCount = 0;
            List<byte> responseMessageList = new List<byte>();
        retrySend:
            try
            {
                _SerialPort.Write(queryMessage, 0, queryMessage.Length);
                Thread.Sleep(_DelayTimeAfterSendMessage);
                if (queryMessage[0] == 0)  //broadcast
                    return new byte[] { };
                responseMessageList.Clear();
                Stopwatch sw = Stopwatch.StartNew();
                while (true)
                {
                    System.Threading.Thread.Sleep(1);
                    byte[] recieveMessage = new byte[_SerialPort.BytesToRead];
                    int recieveCount = _SerialPort.Read(recieveMessage, 0, recieveMessage.Length);
                    if (recieveCount > 0)
                        responseMessageList.AddRange(recieveMessage);
                    if (VerifyCRC(responseMessageList.ToArray()))
                    {
                        break;
                    }
                    if (sw.ElapsedMilliseconds > _HandShakeTimeout)
                    {
                        retryCount++;
                        if (retryCount < 10)
                            goto retrySend;
                        if (responseMessageList.Count == 0)
                            throw new Exception(_SerialPort.PortName + " # " + queryMessage[0].ToString() + " : No response.");
                        throw new Exception(_SerialPort.PortName + " # " + queryMessage[0].ToString() + " : HandShakeTimeout time out");
                    }
                }

                return responseMessageList.ToArray();
            }

            catch
            {
                throw;
            }
        }

        private byte[] Int16ToBytes(ushort value16)
        {
            byte[] bytes = BitConverter.GetBytes(value16);
            Array.Reverse(bytes);
            return bytes;
        }

        private byte[] Int32ToBytes(int value32)
        {
            byte[] bytes = BitConverter.GetBytes(value32);
            Array.Reverse(bytes);
            return bytes;
        }

        private int BytesToUint(byte[] bytes)
        {
            Array.Reverse(bytes);
            if (bytes.Length == 1)
                return bytes[0];
            else if (bytes.Length == 2)
                return BitConverter.ToInt16(bytes, 0);
            else if (bytes.Length == 4)
                return BitConverter.ToInt32(bytes, 0);
            else
                throw new Exception("Bytes[] length must to be between 1 and 4");
        }

        private byte[] BytesSplit(byte[] bytes, int startingIndex, int length)
        {
            List<byte> list = new List<byte>();
            for (int i = startingIndex; i < startingIndex + length; i++)
                list.Add(bytes[i]);
            return list.ToArray();
        }

        public byte[] HexToBytes(string src)
        {
            src = src.Replace("-", "");
            byte[] bytes = new byte[src.Length / 2];
            for (var x = 0; x < bytes.Length; x++)
            {
                var i = Convert.ToInt32(src.Substring(x * 2, 2), 16);
                bytes[x] = (byte)i;
            }

            return bytes;
        }

        public string BytesToHex(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public Single BytesToSingle(byte[] bytes)
        {
            if (bytes.Length == 4)
                return BitConverter.ToSingle(bytes, 0);
            else
                throw new Exception("Bytes[] length must to be between 4");
        }
        
        // read bit 
        public bool[] GeBitValue(int slaveID, string StartAddress, int DataNumber)
        {
            string hexSlaveID = slaveID.ToString("X");
            if (hexSlaveID.Length < 2)
                hexSlaveID = "0" + hexSlaveID;

            string function = "01";

            string hexStartAddress = StartAddress;
            for (int i = hexStartAddress.Length; i < 4; i++)
                hexStartAddress = "0" + hexStartAddress;

            string hexDataNumber = DataNumber.ToString("X");
            for (int i = hexDataNumber.Length; i < 4; i++)
                hexDataNumber = "0" + hexDataNumber;

            string sendMessage = hexSlaveID + function + hexStartAddress + hexDataNumber;
            List<byte> sendByte = new List<byte>();
            sendByte.AddRange(HexToBytes(sendMessage));
            sendByte.AddRange(GenerateCRC(sendByte.ToArray()));
            int len = 3 + (DataNumber / 8) + 2;
            if (DataNumber % 8 > 0)
                len = 3 + (DataNumber / 8) + 1 + 2;
            byte[] resultBytes = SendForGet(sendByte.ToArray(), len);

            string resultHex = BytesToHex(resultBytes);
            if (resultHex.Substring(2, 2) != function)
            {
                resultBytes = Send(sendByte.ToArray());
                resultHex = BytesToHex(resultBytes);
                if (resultHex.Substring(2, 2) != function)
                {
                    throw new Exception("DeltaVFD Return Function Code Error");
                }
            }


            int n = Convert.ToInt32(resultHex.Substring(4, 2), 16);
            byte[] value = new byte[n];
            for (int i = 0; i < n; i++)
            {
                value[i] = Convert.ToByte(resultHex.Substring(i * 2 + 6, 2), 16);
            }

            bool[] bits = new bool[DataNumber];
            for (int i = 0; i < DataNumber; i++)
                bits[i] = (value[i / 8] >> (i % 8) & 1) > 0;

            return bits;
        }

        public byte[] BoolArrayToByteArray(bool[] bits)
        {
            byte[] result = new byte[bits.Length / 8 + 1];
            for (int i = 0; i < bits.Length; i++)
                if (bits[i])
                    result[i / 8] |= (byte)(1 << (i % 8));
            return result;
        }
        // read bit 
        public void WriteBits(int slaveID, string StartAddress, bool[] BoolData)
        {
            string hexSlaveID = slaveID.ToString("X");
            if (hexSlaveID.Length < 2)
                hexSlaveID = "0" + hexSlaveID;

            string function = "0F";

            string hexStartAddress = StartAddress;
            for (int i = hexStartAddress.Length; i < 4; i++)
                hexStartAddress = "0" + hexStartAddress;

            string hexDataNumber = BoolData.Length.ToString("X");
            for (int i = hexDataNumber.Length; i < 4; i++)
                hexDataNumber = "0" + hexDataNumber;

            string hexDataLength;
            int hexDataLen = 0;
            if (BoolData.Length % 4 > 0)
                hexDataLen = (BoolData.Length / 4 + 1);
            else
                hexDataLen = (BoolData.Length / 4);

            hexDataLength = hexDataLen.ToString("X");

            if (hexDataLength.Length < 2)
                hexDataLength = "0" + hexDataLength;


            string hexDataContent = BitConverter.ToString(BoolArrayToByteArray(BoolData));
            hexDataContent = hexDataContent.Replace("-", "");
            for (int i = hexDataContent.Length; i < hexDataLen * 2; i++)
                hexDataContent += "0";

            string sendMessage = hexSlaveID + function + hexStartAddress + hexDataNumber + hexDataLength + hexDataContent;
            List<byte> sendByte = new List<byte>();
            sendByte.AddRange(HexToBytes(sendMessage));
            sendByte.AddRange(GenerateCRC(sendByte.ToArray()));
            byte[] resultBytes = Send(sendByte.ToArray());

            string resultHex = BytesToHex(resultBytes);
            if (resultHex.Substring(2, 2) != function)
            {
                resultBytes = Send(sendByte.ToArray());
                resultHex = BytesToHex(resultBytes);
                if (resultHex.Substring(2, 2) != function)
                {
                    throw new Exception("DeltaVFD Return Function Code Error");
                }
            }

        }
        // read word 
        public int[] GetWordValue(int slaveID, string StartAddress, int DataNumber)
        {
            string hexSlaveID = slaveID.ToString("X");
            if (hexSlaveID.Length < 2)
                hexSlaveID = "0" + hexSlaveID;

            string function = "03";

            string hexStartAddress = StartAddress;
            for (int i = hexStartAddress.Length; i < 4; i++)
                hexStartAddress = "0" + hexStartAddress;

            string hexDataNumber = DataNumber.ToString("X");
            for (int i = hexDataNumber.Length; i < 4; i++)
                hexDataNumber = "0" + hexDataNumber;

            string sendMessage = hexSlaveID + function + hexStartAddress + hexDataNumber;
            List<byte> sendByte = new List<byte>();
            sendByte.AddRange(HexToBytes(sendMessage));
            sendByte.AddRange(GenerateCRC(sendByte.ToArray()));
            byte[] resultBytes = SendForGet(sendByte.ToArray(), 3 + DataNumber * 2 + 2);

            string resultHex = BytesToHex(resultBytes);
            if (resultHex.Substring(2, 2) != function || resultHex.Length != (3 * 2 + DataNumber * 4 + 2 * 2))
            {
                resultBytes = Send(sendByte.ToArray());
                resultHex = BytesToHex(resultBytes);
                if (resultHex.Substring(2, 2) != function || resultHex.Length != (3 * 2 + DataNumber * 4 + 2 * 2))
                {
                    throw new Exception("DeltaVFD Return Function Code Error");
                }
            }

            int[] value = new int[DataNumber];
            for (int i = 0; i < DataNumber; i++)
            {
                value[i] = Convert.ToInt32(resultHex.Substring(resultHex.Length - (DataNumber - i) * 4 - 4, 4), 16);
            }
            return value;
        }

        public void WriteWords(int slaveID, string StartAddress, ushort[] Data)
        {
            string hexSlaveID = slaveID.ToString("X");
            if (hexSlaveID.Length < 2)
                hexSlaveID = "0" + hexSlaveID;

            string function = "10";

            string hexStartAddress = StartAddress;
            for (int i = hexStartAddress.Length; i < 4; i++)
                hexStartAddress = "0" + hexStartAddress;

            string hexDataNumber = Data.Length.ToString("X");
            for (int i = hexDataNumber.Length; i < 4; i++)
                hexDataNumber = "0" + hexDataNumber;

            string hexDataLength = (Data.Length * 2).ToString("X");
            if (hexDataLength.Length < 2)
                hexDataLength = "0" + hexDataLength;

            string hexDataContent = "";
            for (int i = 0; i < Data.Length; i++)
            {
                string Content = Data[i].ToString("X");
                for (int j = Content.Length; j < 4; j++)
                    Content = "0" + Content;

                hexDataContent += Content;
            }

            string sendMessage = hexSlaveID + function + hexStartAddress + hexDataNumber + hexDataLength + hexDataContent;

            List<byte> sendByte = new List<byte>();
            sendByte.AddRange(HexToBytes(sendMessage));
            sendByte.AddRange(GenerateCRC(sendByte.ToArray()));
            byte[] resultBytes = Send(sendByte.ToArray());

            string resultHex = BytesToHex(resultBytes);
            if (resultHex.Substring(2, 2) != function)
            {
                resultBytes = Send(sendByte.ToArray());
                resultHex = BytesToHex(resultBytes);
                if (resultHex.Substring(2, 2) != function)
                {
                    throw new Exception("DeltaVFD Return Function Code Error");
                }
            }
        }
        #endregion

        public SerialPort GetSerialPort()
        {
            return _SerialPort;
        }

        public bool CheckConnect()
        {
            return _SerialPort.IsOpen;
        }

        public void Dispose()
        {
            if (_SerialPort.IsOpen)
            {
                _SerialPort.Close();
            }
        }
    }
}

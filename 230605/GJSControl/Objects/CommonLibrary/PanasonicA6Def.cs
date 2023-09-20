using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Basler.Pylon;
using static CommonLibrary.PanasonicA6Def;

namespace CommonLibrary
{
    public class PanasonicA6Def
    {
        public enum EServoErr
        {
            /// <summary>電池低電量</summary>
            BatteryLowPower,
            /// <summary>電池警報</summary>
            Err400,
            /// <summary>計數器溢出</summary>
            Err410,
            /// <summary>過速度</summary>
            Err420,
            /// <summary>計數警報</summary>
            Err440,
            /// <summary>多圈警報</summary>
            Err450,
            /// <summary>全絕對式狀態</summary>
            Err470,

            Count
        }

        /// <summary>伺服錯誤警報</summary>
        private bool[] _ServoErr;

        public SerialPort Port { get; private set; }
        public int SlaveAddress { get; private set; }

        private int _HandShakeTimeout;

        private const int _EncoderPreCircle = 8388608;

        public double ZeroLap;
        public double PulsePerCircle;
        public double Ratio_PulsePerMm;
        private bool _ReadEncoderFail;

        public PanasonicA6Def(string portName, int slaveAddress)
        {
            _ReadEncoderFail = false;

            _ServoErr = new bool[(int)EServoErr.Count];
            for (int i = 0; i < _ServoErr.Length; i++)
                _ServoErr[i] = false;

            SlaveAddress = slaveAddress;
            Port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
            _HandShakeTimeout = 300;
            Port.ReadTimeout = 50;
            Port.WriteTimeout = 50;
            Port.Open();
        }
        public PanasonicA6Def(SerialPort port, int slaveAddress)
        {
            _ServoErr = new bool[(int)EServoErr.Count];
            for (int i = 0; i < _ServoErr.Length; i++)
                _ServoErr[i] = false;

            _HandShakeTimeout = 300;
            Port = port;
            SlaveAddress = slaveAddress;
        }

        public void Dispose()
        {
            if (Port != null && Port.IsOpen)
                Port.Close();
        }

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

        public bool VerifyCheckSumHex(byte[] message)
        {
            string hexString = ByteArrayToString(message);
            byte[] messageWithoutCRC = StringToByteArray(hexString.Substring(0, hexString.Length - 2));
            int chkSum = messageWithoutCRC.Aggregate(0, (s, b) => s += b) & 0xff;
            chkSum = (0x100 - chkSum) & 0xff;
            // byte[] messageCRC = { message[message.Length - 2], message[message.Length - 1] };

            string crc = chkSum.ToString("X2");

            if (hexString[hexString.Length - 2] - crc[0] == 0 &&
                hexString[hexString.Length - 1] - crc[1] == 0)
                return true;
            else
                return false;
        }
        public string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        private string GetSubHex(string ori, int sub)
        {
            int num = Convert.ToInt32(ori, 16);
            num += sub;
            return num.ToString("X2");
        }
        public double GetAbslap()
        {
            if (!Port.IsOpen)
            {
                ReadEncoderFail();
                return 0;
            }

            List<byte> responseMessageList = new List<byte>();
            string hex = GetSubHex("80", SlaveAddress) + "05";
            byte[] bytes = StringToByteArray(hex);

            Port.Write(bytes, 0, bytes.Length);


            Stopwatch sw = Stopwatch.StartNew();

            while (true)
            {
                System.Threading.Thread.Sleep(1);
                byte[] recieveMessage = new byte[Port.BytesToRead];
                int recieveCount = Port.Read(recieveMessage, 0, recieveMessage.Length);
                responseMessageList.AddRange(recieveMessage);
                if (responseMessageList.Count >= 2)
                    break;

                if (sw.ElapsedMilliseconds > _HandShakeTimeout)
                {
                    ReadEncoderFail();
                    return 0;
                }
            }

            hex = "00" + GetSubHex("00", SlaveAddress) + "D2" + GetSubHex("2E", -SlaveAddress);
            byte[] bytes2 = StringToByteArray(hex);
            responseMessageList.Clear();
            Port.Write(bytes2, 0, bytes2.Length);

            sw.Restart();

            while (true)
            {
                System.Threading.Thread.Sleep(1);
                byte[] recieveMessage = new byte[Port.BytesToRead];
                int recieveCount = Port.Read(recieveMessage, 0, recieveMessage.Length);
                responseMessageList.AddRange(recieveMessage);
                if (responseMessageList.Count > 2)
                    break;

                if (sw.ElapsedMilliseconds > _HandShakeTimeout)
                {
                    ReadEncoderFail();
                    return 0;
                }
            }

            byte[] bytes3 = StringToByteArray("8004");
            responseMessageList.Clear();
            Port.Write(bytes3, 0, bytes3.Length);

            sw.Restart();

            while (true)
            {
                System.Threading.Thread.Sleep(1);
                byte[] recieveMessage = new byte[Port.BytesToRead];
                int recieveCount = Port.Read(recieveMessage, 0, recieveMessage.Length);
                responseMessageList.AddRange(recieveMessage);
                if (responseMessageList.Count > 14 && VerifyCheckSumHex(responseMessageList.ToArray()))
                    break;

                if (sw.ElapsedMilliseconds > _HandShakeTimeout)
                {
                    ReadEncoderFail();
                    return 0;
                }
            }

            byte[] bytes4 = StringToByteArray("06");

            Port.Write(bytes4, 0, bytes4.Length);
            if (responseMessageList.Count < 15)
            {
                ReadEncoderFail();
                return 0;
            }

            long val5 = responseMessageList[responseMessageList.Count - 4] * long.Parse("100", System.Globalization.NumberStyles.HexNumber);
            long val4 = responseMessageList[responseMessageList.Count - 5] * long.Parse("1", System.Globalization.NumberStyles.HexNumber);
            long val3 = responseMessageList[responseMessageList.Count - 6] * long.Parse("10000", System.Globalization.NumberStyles.HexNumber);
            long val2 = responseMessageList[responseMessageList.Count - 7] * long.Parse("100", System.Globalization.NumberStyles.HexNumber);
            long val1 = responseMessageList[responseMessageList.Count - 8] * 1;
            long alarmStatusH = responseMessageList[responseMessageList.Count - 9];
            long alarmStatusL = responseMessageList[responseMessageList.Count - 10];

            double roundNum = ((val1 + val2 + val3) / ((double)_EncoderPreCircle)) + val4 + val5;
            System.Threading.Thread.Sleep(50);
            if (roundNum > 65536 / 2)
                roundNum -= 65536;

            _ServoErr[(int)EServoErr.BatteryLowPower] = ((alarmStatusL & 0b10000000) == 128);
            _ServoErr[(int)EServoErr.Err400] = ((alarmStatusL & 0b01000000) == 64);
            _ServoErr[(int)EServoErr.Err410] = ((alarmStatusL & 0b00001000) == 8);
            _ServoErr[(int)EServoErr.Err420] = ((alarmStatusL & 0b00000001) == 1);
            _ServoErr[(int)EServoErr.Err440] = ((alarmStatusL & 0b00000100) == 4);
            _ServoErr[(int)EServoErr.Err450] = ((alarmStatusL & 0b00100000) == 32);
            _ServoErr[(int)EServoErr.Err470] = ((alarmStatusL & 0b00000010) == 2);

            return roundNum;
        }

        public bool GetBatteryStatus(ref string eErrName)
        {
            bool _Err = false;
            eErrName = "";

            for (int i = 0; i < _ServoErr.Length; i++)
            {
                if (_ServoErr[i])
                {
                    if (eErrName == "")
                        eErrName += ((EServoErr)i).ToString();
                    else
                        eErrName += "," + ((EServoErr)i).ToString();
                    _Err = true;
                }
            }
            return _Err;
        }

        private void ReadEncoderFail()
        {
            _ReadEncoderFail = true;

            AlarmTextDisplay.Add((int)AlarmCode.Alarm_AxisError, AlarmType.Alarm, "Read encoder fail");
        }

        public bool IsReadEncoderFail() { return _ReadEncoderFail; }
    }
}
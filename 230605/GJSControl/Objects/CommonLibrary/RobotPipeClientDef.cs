using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Pipes;
using System.IO;
using FileStreamLibrary;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace CommonLibrary
{
    public struct stRobotPosValueDef
    {
        public Array fXYZWPR;
        public Array nConfig;
        public Array fJoint;
        public short nUF;
        public short nUT;
        public short nValidC;
        public short nValidJ;

        public bool bGo;
        public int nGoPercent;
        public int nMoveSpeed;
        public int nGoContinuePercent;

        public void Initial()
        {
            fXYZWPR = new float[9];
            nConfig = new short[7];
            fJoint = new float[9];
            nUF = 0;
            nUT = 0;
            nValidC = -1;
            nValidJ = -1;

            bGo = true;
            nGoPercent = 10;
            nGoContinuePercent = 10;
            nMoveSpeed = 100;
            nGoContinuePercent = 10;
        }

        public void CopyTo(ref stRobotPosValueDef stRobotPosArray)
        {
            this.fXYZWPR.CopyTo(stRobotPosArray.fXYZWPR, 0);
            this.nConfig.CopyTo(stRobotPosArray.nConfig, 0);
            this.fJoint.CopyTo(stRobotPosArray.fJoint, 0);

            stRobotPosArray.bGo = this.bGo;
            stRobotPosArray.nGoContinuePercent = this.nGoContinuePercent;
            stRobotPosArray.nGoPercent = this.nGoPercent;
            stRobotPosArray.nMoveSpeed = this.nMoveSpeed;
            stRobotPosArray.nUF = this.nUF;
            stRobotPosArray.nUT = this.nUT;
            stRobotPosArray.nValidC = this.nValidC;
            stRobotPosArray.nValidJ = this.nValidJ;
        }
    }



    public enum ERobotDO
    {
        IMPSTP,
        HOLD,
        SFSPD,
        CSTOPI,
        FAULT_RESET,
        START,
        HONE,
        ENBL,
        RSR1,
        RSR2,
        RSR3,
        RSR4,
        RSR5,
        RSR6,
        RSR7,
        RSR8,

        Count
    }
    public enum ERobotDI
    {
        Count
    }

    public class StreamString
    {
        private Stream m_cIOStreamSrc;
        private UTF8Encoding streamEncoding;

        public StreamString(Stream ioStreamSrc)
        {
            m_cIOStreamSrc = ioStreamSrc;
            streamEncoding = new UTF8Encoding();
        }

        public string ReadString()
        {
            byte[] nBuffer = new byte[1024];
            if (m_cIOStreamSrc.Read(nBuffer, 0, 1024) <= 0)
                return null;

            return streamEncoding.GetString(nBuffer);
        }

        public void WriteString(string outString)
        {
            byte[] nBuffer = streamEncoding.GetBytes(outString);

            m_cIOStreamSrc.Write(nBuffer, 0, nBuffer.Length);
        }
    }

    public class RobotPipeClientDef : IDisposable
    {
        enum ERobotCommand
        {
            WDI,        //  寫入DI點位
            WDO,        //  寫入DO點位
            RDI,        //  讀取DI點位
            RDO,        //  讀取DO點位
            WRN,        //  寫入暫存整數數值
            WRF,        //  寫入暫存浮點數值
            RRN,        //  讀取暫存整數數值
            RRF,        //  讀取暫存浮點數值
            WRP,        //  寫入暫存點位資訊
            WRJ,
            RCP,        //  讀取目前點位資訊
            RRP,        //  讀取暫存點位資訊
            Count,
        }

        private Array m_fXYZWPR;
        private Array m_nConfig;
        private Array m_fJoint;
        private short m_nUF;
        private short m_nUT;
        private short m_nValidC;
        private short m_nValidJ;
        private short m_nGoContinue;

        //private Thread m_thPolling;
        //private NamedPipeClientStream m_cPipeClient;
        //private StreamString m_cStreamStr;
        //private bool m_bDone;
        //private bool m_bOn;
        //private int m_nValue;
        //private float m_fValue;
        //private object m_cLock;
        private stRobotPosValueDef[] m_stRobotPosArray;
        private string m_sFolderPath;
        private Region _safeArea;
        public bool EnableSafeArea;
        public bool OutOfSafeArea { private set; get; }
        private FanucRobotDef _FanucRobot;

        public RobotPipeClientDef(string sFolderPath)
        {
            m_sFolderPath = sFolderPath;
            m_stRobotPosArray = new stRobotPosValueDef[(int)ERobotPosition.Count];
            for (int i = 0; i < (int)ERobotPosition.Count; i++)
                m_stRobotPosArray[i].Initial();

            _safeArea = null;
            ReadFile();

            m_fXYZWPR = new float[9];
            m_nConfig = new short[7];
            m_fJoint = new float[9];
            m_nUF = 0;
            m_nUT = 0;
            m_nValidC = 0;
            m_nValidJ = 0;
            m_nGoContinue = 0;

            //m_cLock = new object();
            //m_cPipeClient = new NamedPipeClientStream(
            //    ".", "RobotPipeServer",
            //      PipeDirection.InOut, 
            //      PipeOptions.None,
            //      System.Security.Principal.TokenImpersonationLevel.Impersonation);

            ////m_thPolling = new Thread(new ThreadStart(ThreadTask));
            ////m_thPolling.IsBackground = true;
            ////m_thPolling.Priority = ThreadPriority.Highest;
            ////m_thPolling.Start();
            ////m_bDone = true;

            EnableSafeArea = false;

            OutOfSafeArea = false;

            try
            {
                _FanucRobot = new FanucRobotDef(m_sFolderPath);
            }
            catch
            {

            }

        }

        private void ReadFile()
        {
            IniFile cIni = new IniFile(m_sFolderPath + "\\Robot.ini", true);

            for (int i = 0; i < (int)ERobotPosition.Count; i++)
            {
                String sSection = ((ERobotPosition)i).ToString();

                String str = cIni.ReadStr(sSection, "fXYZWPR", "0,0,0,0,0,0,0,0,0");
                m_stRobotPosArray[i].fXYZWPR = str.Split(',').Select(float.Parse).ToArray();

                str = cIni.ReadStr(sSection, "nConfig", "0,0,0,0,0,0,0");
                m_stRobotPosArray[i].nConfig = str.Split(',').Select(short.Parse).ToArray();

                str = cIni.ReadStr(sSection, "fJoint", "0,0,0,0,0,0,0,0,0");
                m_stRobotPosArray[i].fJoint = str.Split(',').Select(float.Parse).ToArray();

                m_stRobotPosArray[i].nUF = (short)cIni.ReadInt(sSection, "nUF", 0);

                m_stRobotPosArray[i].nUT = (short)cIni.ReadInt(sSection, "nUT", 0);

                m_stRobotPosArray[i].nValidC = (short)cIni.ReadInt(sSection, "nValidC", 0);

                m_stRobotPosArray[i].nValidJ = (short)cIni.ReadInt(sSection, "nValidJ", 0);

                m_stRobotPosArray[i].bGo = cIni.ReadBool(sSection, "bGo", true);

                m_stRobotPosArray[i].nGoPercent = cIni.ReadInt(sSection, "nGoPercent", 10);

                m_stRobotPosArray[i].nGoContinuePercent = cIni.ReadInt(sSection, "nGoContinuePercent", 10);

                m_stRobotPosArray[i].nMoveSpeed = cIni.ReadInt(sSection, "nMoveSpeed", 100);
            }

            EnableSafeArea = cIni.ReadBool("SafeArea", "EnableSafeArea", false);
            string strArea = cIni.ReadStr("SafeArea", "Polygon", "");
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

                _safeArea = new Region(path);
            }

            cIni.FileClose();
            cIni.Dispose();
        }

        public void SaveFile()
        {
            IniFile cIni = new IniFile(m_sFolderPath + "\\Robot.ini", false);

            for (int i = 0; i < (int)ERobotPosition.Count; i++)
            {
                String sSection = ((ERobotPosition)i).ToString();

                string[] sXYZWPR = new string[m_stRobotPosArray[i].fXYZWPR.Length];
                for (int j = 0; j < sXYZWPR.Count(); j++)
                    sXYZWPR[j] = m_stRobotPosArray[i].fXYZWPR.GetValue(j).ToString();
                cIni.WriteStr(sSection, "fXYZWPR", string.Join(",", sXYZWPR));

                string[] sConfig = new string[m_stRobotPosArray[i].nConfig.Length];
                for (int j = 0; j < sConfig.Count(); j++)
                    sConfig[j] = m_stRobotPosArray[i].nConfig.GetValue(j).ToString();
                cIni.WriteStr(sSection, "nConfig", string.Join(",", sConfig));

                string[] sJoint = new string[m_stRobotPosArray[i].fJoint.Length];
                for (int j = 0; j < sJoint.Count(); j++)
                    sJoint[j] = m_stRobotPosArray[i].fJoint.GetValue(j).ToString();
                cIni.WriteStr(sSection, "fJoint", string.Join(",", sJoint));

                cIni.WriteInt(sSection, "nUF", m_stRobotPosArray[i].nUF);
                cIni.WriteInt(sSection, "nUT", m_stRobotPosArray[i].nUT);
                cIni.WriteInt(sSection, "nValidC", m_stRobotPosArray[i].nValidC);
                cIni.WriteInt(sSection, "nValidJ", m_stRobotPosArray[i].nValidJ);
                cIni.WriteBool(sSection, "bGo", m_stRobotPosArray[i].bGo);
                cIni.WriteInt(sSection, "nGoPercent", m_stRobotPosArray[i].nGoPercent);
                cIni.WriteInt(sSection, "nGoContinuePercent", m_stRobotPosArray[i].nGoContinuePercent);
                cIni.WriteInt(sSection, "nMoveSpeed", m_stRobotPosArray[i].nMoveSpeed);
            }

            cIni.FileClose();
            cIni.Dispose();
        }

        //private void ThreadTask()
        //{ 
        //    try
        //    {
        //        m_cPipeClient.Connect(1000);

        //        m_cStreamStr = new StreamString(m_cPipeClient);
        //        //while (!m_bThreadEnd && !m_bDone)
        //        //{
        //        //    String sCommand = m_cStreamStr.ReadString();
        //        //    vDoCommand(sCommand);
        //        //    Thread.Sleep(1);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!m_cPipeClient.IsConnected)
        //        {
        //            AlarmTextDisplay.Add(
        //                AlarmCode.Machine_RobotConnectionError,
        //                AlarmType.Warning);
        //        }
        //    }
        //}

        //public bool Connected()
        //{
        //    return m_cPipeClient.IsConnected;
        //}

        public bool Stopped()
        {
            if (GetGo(false))
                return false;
            else
                return true;
        }
        public void Dispose()
        {
            _FanucRobot.Dispose();
            //m_cPipeClient.Close();
            //m_cPipeClient.Dispose();
            //m_cPipeClient = null;

            //while (m_thPolling != null && m_thPolling.IsAlive)
            //{
            //    Application.DoEvents();
            //    Thread.Sleep(15);
            //}

            //m_thPolling = null;
            //m_cStreamStr = null;
        }

        public stRobotPosValueDef GetRobotPos(ERobotPosition eIndex)
        {
            stRobotPosValueDef stRobotPosArray = new stRobotPosValueDef();
            stRobotPosArray.Initial();
            m_stRobotPosArray[(int)eIndex].fXYZWPR.CopyTo(stRobotPosArray.fXYZWPR, 0);
            m_stRobotPosArray[(int)eIndex].nConfig.CopyTo(stRobotPosArray.nConfig, 0);
            m_stRobotPosArray[(int)eIndex].fJoint.CopyTo(stRobotPosArray.fJoint, 0);

            stRobotPosArray.bGo = m_stRobotPosArray[(int)eIndex].bGo;
            stRobotPosArray.nGoContinuePercent = m_stRobotPosArray[(int)eIndex].nGoContinuePercent;
            stRobotPosArray.nGoPercent = m_stRobotPosArray[(int)eIndex].nGoPercent;
            stRobotPosArray.nMoveSpeed = m_stRobotPosArray[(int)eIndex].nMoveSpeed;
            stRobotPosArray.nUF = m_stRobotPosArray[(int)eIndex].nUF;
            stRobotPosArray.nUT = m_stRobotPosArray[(int)eIndex].nUT;
            stRobotPosArray.nValidC = m_stRobotPosArray[(int)eIndex].nValidC;
            stRobotPosArray.nValidJ = m_stRobotPosArray[(int)eIndex].nValidJ;
            return stRobotPosArray;
        }

        //public void SetCurrentRobotPos(ERobotPosition eIndex)   have bug
        //{
        //    stRobotPosValueDef stRobotValue = GetRobotPos(eIndex);
        //    GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);

        //    m_stRobotPosArray[(int)eIndex].bGo = stRobotValue.bGo;
        //    m_stRobotPosArray[(int)eIndex].nGoContinuePercent = stRobotValue.nGoContinuePercent;
        //    m_stRobotPosArray[(int)eIndex].nGoPercent = stRobotValue.nGoPercent;
        //    m_stRobotPosArray[(int)eIndex].nMoveSpeed = stRobotValue.nMoveSpeed;
        //    m_stRobotPosArray[(int)eIndex].nUF = stRobotValue.nUF;
        //    m_stRobotPosArray[(int)eIndex].nUT = stRobotValue.nUT;
        //    m_stRobotPosArray[(int)eIndex].nValidC = stRobotValue.nValidC;
        //    m_stRobotPosArray[(int)eIndex].nValidJ = stRobotValue.nValidJ;
        //}

        public void SetRobotPos(ERobotPosition eIndex, stRobotPosValueDef stRobotPosValue)
        {
            stRobotPosValue.fXYZWPR.CopyTo(m_stRobotPosArray[(int)eIndex].fXYZWPR, 0);
            stRobotPosValue.nConfig.CopyTo(m_stRobotPosArray[(int)eIndex].nConfig, 0);
            stRobotPosValue.fJoint.CopyTo(m_stRobotPosArray[(int)eIndex].fJoint, 0);

            m_stRobotPosArray[(int)eIndex].bGo = stRobotPosValue.bGo;
            m_stRobotPosArray[(int)eIndex].nGoContinuePercent = stRobotPosValue.nGoContinuePercent;
            m_stRobotPosArray[(int)eIndex].nGoPercent = stRobotPosValue.nGoPercent;
            m_stRobotPosArray[(int)eIndex].nMoveSpeed = stRobotPosValue.nMoveSpeed;
            m_stRobotPosArray[(int)eIndex].nUF = stRobotPosValue.nUF;
            m_stRobotPosArray[(int)eIndex].nUT = stRobotPosValue.nUT;
            m_stRobotPosArray[(int)eIndex].nValidC = stRobotPosValue.nValidC;
            m_stRobotPosArray[(int)eIndex].nValidJ = stRobotPosValue.nValidJ;
        }

        public void SetAllPosition()
        {
            for (int i = 0; i < m_stRobotPosArray.Length; i++)
            {
                if (!SetRegisterPos(i + 3,
                    m_stRobotPosArray[i].fXYZWPR,
                    m_stRobotPosArray[i].nConfig,
                    m_stRobotPosArray[i].fJoint,
                    m_stRobotPosArray[i].nUF,
                    m_stRobotPosArray[i].nUT,
                    m_stRobotPosArray[i].nValidC,
                    m_stRobotPosArray[i].nValidJ))
                    return;

                if (m_stRobotPosArray[i].bGo)
                    SetGoSpeed(i + 2, (int)m_stRobotPosArray[i].nGoPercent);
                else
                    SetGoSpeed(i + 2, (int)m_stRobotPosArray[i].nMoveSpeed);
            }
        }

        private bool IsOnSafeArea(PointF Destination)
        {
            if (_safeArea == null)
                return true;

            OutOfSafeArea = !_safeArea.IsVisible(Destination);
            return !OutOfSafeArea;
        }

        public void Go(ERobotPosition eIndex, bool fastSkip = false)
        {
            if (EnableSafeArea)
            {
                if (!IsOnSafeArea(new PointF(Convert.ToSingle(m_stRobotPosArray[(int)eIndex].fXYZWPR.GetValue(0)), Convert.ToSingle(m_stRobotPosArray[(int)eIndex].fXYZWPR.GetValue(1)))))
                    return;
            }
            if (m_stRobotPosArray[(int)eIndex].nGoContinuePercent != m_nGoContinue)
            {
                m_nGoContinue = (short)m_stRobotPosArray[(int)eIndex].nGoContinuePercent;
                SetGoContinue(m_nGoContinue);
            }

            SetMove(!m_stRobotPosArray[(int)eIndex].bGo, fastSkip);
            SetGo((int)eIndex, true);
        }

        public void RelGo(int GoPercent, float[] shift)
        {
            stRobotPosValueDef stRobotValue = GetRobotPos((ERobotPosition)(0));
            GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);

            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(0)) + shift[0], 0);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(1)) + shift[1], 1);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(2)) + shift[2], 2);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(3)) + shift[3], 3);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(4)) + shift[4], 4);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(5)) + shift[5], 5);
            stRobotValue.bGo = true;
            stRobotValue.nGoPercent = GoPercent;
            stRobotValue.nGoContinuePercent = 0;

            if (EnableSafeArea)
            {
                if (!IsOnSafeArea(new PointF(Convert.ToSingle(stRobotValue.fXYZWPR.GetValue(0)), Convert.ToSingle(stRobotValue.fXYZWPR.GetValue(1)))))
                    return;
            }

            if (!SetRegisterPos(
                stRobotValue.fXYZWPR,
                stRobotValue.nConfig,
                stRobotValue.fJoint,
                -1,
                -1,
                -1,
                -1))
                return;

            SetGoSpeed(stRobotValue.nGoPercent);
            SetGoContinue(stRobotValue.nGoContinuePercent);

            SetGo(true);
        }

        public void RelMove(int MoveSpeed, float[] shift)
        {
            stRobotPosValueDef stRobotValue = GetRobotPos((ERobotPosition)(0));
            GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);

            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(0)) + shift[0], 0);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(1)) + shift[1], 1);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(2)) + shift[2], 2);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(3)) + shift[3], 3);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(4)) + shift[4], 4);
            stRobotValue.fXYZWPR.SetValue((float)Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(5)) + shift[5], 5);
            stRobotValue.bGo = false;
            stRobotValue.nMoveSpeed = MoveSpeed;
            stRobotValue.nGoContinuePercent = 0;

            if (EnableSafeArea)
            {
                if (!IsOnSafeArea(new PointF(Convert.ToSingle(stRobotValue.fXYZWPR.GetValue(0)), Convert.ToSingle(stRobotValue.fXYZWPR.GetValue(1)))))
                    return;
            }
            if (!SetRegisterPos(
                stRobotValue.fXYZWPR,
                stRobotValue.nConfig,
                stRobotValue.fJoint,
                -1,
                -1,
                -1,
                -1))
                return;

            SetGoContinue(stRobotValue.nGoContinuePercent);
            SetGoSpeed(stRobotValue.nMoveSpeed);
            SetMove(true);

            SetGo(true);
        }

        public void Go(stRobotPosValueDef stRobot, bool fastSkip = false)
        {
            if (EnableSafeArea)
            {
                if (!IsOnSafeArea(new PointF(Convert.ToSingle(stRobot.fXYZWPR.GetValue(0)), Convert.ToSingle(stRobot.fXYZWPR.GetValue(1)))))
                    return;
            }

            if (!SetRegisterPos(
                stRobot.fXYZWPR,
                stRobot.nConfig,
                stRobot.fJoint,
                -1,
                -1,
                -1,
                -1))
                return;

            if (stRobot.bGo)
            {
                SetGoSpeed(stRobot.nGoPercent);
                SetGoContinue(stRobot.nGoContinuePercent);
                SetMove(false, fastSkip);
            }
            else
            {
                SetGoContinue(stRobot.nGoContinuePercent);
                SetGoSpeed(stRobot.nMoveSpeed);
                SetMove(true, fastSkip);
            }

            SetGo(true);
        }

        public void GoJ(ERobotPosition eIndex)
        {
            if (!SetRegisterPos(
                m_stRobotPosArray[(int)eIndex].fJoint,
                m_stRobotPosArray[(int)eIndex].nUF,
                m_stRobotPosArray[(int)eIndex].nUT,
                m_stRobotPosArray[(int)eIndex].nValidC,
                m_stRobotPosArray[(int)eIndex].nValidJ))
                return;

            if (m_stRobotPosArray[(int)eIndex].bGo)
            {
                SetGoSpeed(m_stRobotPosArray[(int)eIndex].nGoPercent);
                SetGoContinue(m_stRobotPosArray[(int)eIndex].nGoContinuePercent);
            }
            else
            {
                SetGoContinue(m_stRobotPosArray[(int)eIndex].nGoContinuePercent);
                SetGoSpeed(m_stRobotPosArray[(int)eIndex].nMoveSpeed);
                SetMove(true);
            }

            SetGo(true);
        }

        public void GoJ(stRobotPosValueDef stRobot)
        {
            if (!SetRegisterPos(
                stRobot.fJoint,
                stRobot.nUF,
                stRobot.nUT,
                stRobot.nValidC,
                stRobot.nValidJ))
                return;

            if (stRobot.bGo)
            {
                SetGoSpeed(stRobot.nGoPercent);
                SetGoContinue(stRobot.nGoContinuePercent);
            }
            else
            {
                SetGoContinue(stRobot.nGoContinuePercent);
                SetGoSpeed(stRobot.nMoveSpeed);
                SetMove(true);
            }
            SetGo(true);
        }

        public void SetDI(ERobotDI eIndex, bool bOn)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetDI((int)eIndex, bOn);
        }

        public void SetDO(ERobotDO eIndex, bool bOn)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetDO((int)eIndex, bOn);
        }

        public bool GetGo(bool bOn)
        {
            if (_FanucRobot == null) return false;
            bool result = _FanucRobot.nGetRegisterInt(0, 0) > 0;
            return result;
        }

        public bool GetDI(ERobotDI eIndex, bool bOn)
        {
            if (_FanucRobot == null) return false;
            bool result = _FanucRobot.bGetDI((int)eIndex, bOn);
            return result;
        }

        public bool GetDO(ERobotDO eIndex, bool bOn)
        {
            if (_FanucRobot == null) return false;
            bool result = _FanucRobot.bGetDO((int)eIndex, bOn);
            return result;
        }

        public void SetGo(bool bGo)
        {
            if (_FanucRobot == null) return;
            if (bGo)
                _FanucRobot.vSetRegisterInt(0, 2);
            else
                _FanucRobot.vSetRegisterInt(0, 0);
        }

        public void SetGo(int nIndex, bool bGo)
        {
            if (_FanucRobot == null) return;
            if (bGo)
                _FanucRobot.vSetRegisterInt(0, nIndex + 3);
            else
                _FanucRobot.vSetRegisterInt(0, 0);
        }

        public void SetGoSpeed(int nIndex, int nPercent)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetRegisterInt(nIndex, nPercent);
        }

        public void SetGoSpeed(int nPercent)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetRegisterInt(1, nPercent);
        }

        public void SetStop(bool bStop)
        {
            if (_FanucRobot == null) return;
            if (bStop)
                _FanucRobot.vSetRegisterInt(105, 1);
            else
                _FanucRobot.vSetRegisterInt(105, 0);
        }

        public void SetGoContinue(int nPercent)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetRegisterInt(101, nPercent);
        }

        public void SetMove(bool bMove, bool fastSkip = false)
        {
            if (_FanucRobot == null) return;
            //bMove = true 走直線, fastSkip 開啟高速跳過功能 手臂速度會被限制在250mm/s

            //0 : 關節運動 正常跳過
            //1 : 直線運動 正常跳過
            //2 : 關節運動 高速跳過
            //3 : 直線運動 高速跳過
            if (!bMove && !fastSkip)
                _FanucRobot.vSetRegisterInt(102, 0);
            else if (bMove && !fastSkip)
                _FanucRobot.vSetRegisterInt(102, 1);
            else if (!bMove && fastSkip)
                _FanucRobot.vSetRegisterInt(102, 2);
            else if (bMove && fastSkip)
                _FanucRobot.vSetRegisterInt(102, 3);
        }

        public void SetSpeedRate(int nPercent)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetRegisterInt(100, nPercent);
        }

        public void SetDelayTime(float fValue)
        {
            if (_FanucRobot == null) return;
            _FanucRobot.vSetRegisterFloat(0, fValue);
        }

        public float GetDelayTime(float fValue)
        {
            if (_FanucRobot == null) return fValue;
            float result = _FanucRobot.fGetRegisterFloat(0, fValue);
            return result;
        }

        public bool GetCurrentPos(ref Array fXYZWPR, ref Array nConfig, ref Array fJoint, ref short nUF, ref short nUT, ref short nValidC, ref short nValidJ)
        {
            if (_FanucRobot == null) return false;
            return _FanucRobot.bGetCurrentPos(ref fXYZWPR, ref nConfig, ref fJoint, ref nUF, ref nUT, ref nValidC, ref nValidJ);
        }

        public bool GetRegisterPos(ref Array fXYZWPR, ref Array nConfig, ref Array fJoint, ref short nUF, ref short nUT, ref short nValidC, ref short nValidJ)
        {
            if (_FanucRobot == null) return false;
            return _FanucRobot.bGetRegisterPos(2, ref fXYZWPR, ref nConfig, ref fJoint, ref nUF, ref nUT, ref nValidC, ref nValidJ);
        }

        public bool SetRegisterPos(Array fXYZWPR, Array nConfig, Array fJoint, short nUF, short nUT, short nValidC, short nValidJ)
        {
            if (_FanucRobot == null) return false;
            return _FanucRobot.bSetRegisterPos(2, ref fXYZWPR, ref nConfig, nUF, nUT);
        }

        public bool SetRegisterPos(int nIndex, Array fXYZWPR, Array nConfig, Array fJoint, short nUF, short nUT, short nValidC, short nValidJ)
        {
            if (_FanucRobot == null) return false;
            return _FanucRobot.bSetRegisterPos(nIndex, ref fXYZWPR, ref nConfig, nUF, nUT);
        }

        public bool SetRegisterPos(Array fJoint, short nUF, short nUT, short nValidC, short nValidJ)
        {
            if (_FanucRobot == null) return false;
            //UF UT 是啥!!!!!!!!!!!!!!!!!!!!!!
            return _FanucRobot.bSetRegisterPos(2, ref fJoint, 15, 15);

        }
    }

    public class FanucRobotDef : IDisposable
    {
        private FRRJIf.Core m_cObjCore;
        private FRRJIf.DataTable m_cObjDataTable;

        private FRRJIf.DataCurPos m_cObjCurPos;

        private FRRJIf.DataPosReg m_cObjPosReg;

        private FRRJIf.DataNumReg m_cObjNumRegN;
        private FRRJIf.DataNumReg m_cObjNumRegF;


        private int m_nDIStartIndex;
        private int m_nDOStartIndex;

        private int m_nRegNStart;
        private int m_nRegNEnd;

        private int m_nRegFStart;
        private int m_nRegFEnd;
        private bool m_bConnected;
        private String m_sIP;
        public FanucRobotDef(String sFolderPath)
        {
            vReadSetting(sFolderPath);
            m_cObjCore = new FRRJIf.Core();

            m_cObjDataTable = m_cObjCore.get_DataTable();
            // You need to set data table before connecting.
            {
                m_cObjCurPos = m_cObjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1);
                m_cObjPosReg = m_cObjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 1, 50);
                m_cObjNumRegN = m_cObjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, m_nRegNStart, m_nRegNEnd);
                m_cObjNumRegF = m_cObjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, m_nRegFStart, m_nRegFEnd);
            }



            m_bConnected = m_cObjCore.Connect(m_sIP);
        }

        private void vReadSetting(String sFolderPath)
        {
            IniFile ini = new IniFile(sFolderPath + "\\Robot.ini", true);

            m_sIP = ini.ReadStr("System", "IP", "192.168.0.1");

            m_nDIStartIndex = ini.ReadInt("System", "DIStart", 101);
            m_nDOStartIndex = ini.ReadInt("System", "DOStart", 101);

            m_nRegNStart = ini.ReadInt("System", "RegNStart", 1);
            m_nRegNEnd = ini.ReadInt("System", "RegNEnd", 150);
            m_nRegFStart = ini.ReadInt("System", "RegFStart", 151);
            m_nRegFEnd = ini.ReadInt("System", "RegFEnd", 200);

            ini.FileClose();
            ini.Dispose();
        }


        public void Dispose()
        {
            if (m_bConnected)
                m_cObjCore.Disconnect();

        }

        public bool bConnected()
        {
            return m_bConnected;
        }

        public void vSetDI(int nIndex, bool bOn)
        {
            if (!m_cObjCore.get_IsConnected())
                return;

            short[] nBuffer = new short[1];
            nBuffer[0] = 0;
            if (bOn)
                nBuffer[0] = 1;

            m_cObjCore.WriteSDI(m_nDIStartIndex + ((int)nIndex), nBuffer, 1);
        }

        public bool bGetDI(int nIndex, bool bOn)
        {
            if (!m_cObjCore.get_IsConnected())
                return bOn;

            short[] nBuffer = new short[1];

            m_cObjCore.ReadSDI(m_nDIStartIndex + ((int)nIndex), nBuffer, 1);

            if (nBuffer[0] > 0)
                return true;


            return false;
        }

        public void vSetDO(int nIndex, bool bOn)
        {
            if (!m_cObjCore.get_IsConnected())
                return;

            short[] nBuffer = new short[1];
            nBuffer[0] = 0;
            if (bOn)
                nBuffer[0] = 1;

            m_cObjCore.WriteSDO(m_nDOStartIndex + ((int)nIndex), nBuffer, 1);
        }

        public void vSetUI(int nIndex, bool bOn)
        {
            if (!m_cObjCore.get_IsConnected())
                return;

            short[] nBuffer = new short[1];
            nBuffer[0] = 0;
            if (bOn)
                nBuffer[0] = 1;

            m_cObjCore.WriteUI(((int)nIndex), nBuffer, 1);
        }

        public bool bGetUI(int nIndex, bool bOn)
        {
            if (!m_cObjCore.get_IsConnected())
                return bOn;
            short[] nBuffer = new short[1];

            m_cObjCore.ReadUI(((int)nIndex), nBuffer, 1);

            if (nBuffer[0] > 0)
                return true;

            return false;
        }

        public bool bGetDO(int nIndex, bool bOn)
        {
            if (!m_cObjCore.get_IsConnected())
                return bOn;
            short[] nBuffer = new short[1];

            m_cObjCore.ReadSDO(m_nDOStartIndex + ((int)nIndex), nBuffer, 1);

            if (nBuffer[0] > 0)
                return true;

            return false;
        }

        public void vSetRegisterInt(int nIndex, int nValue)
        {
            if (!m_cObjCore.get_IsConnected())
                return;

            int[] nIntValues = new int[1];
            nIntValues[0] = nValue;
            m_cObjNumRegN.SetValues(m_cObjNumRegN.StartIndex + (int)nIndex, nIntValues, 1);
        }

        public int nGetRegisterInt(int nIndex, int nValue)
        {
            if (!m_cObjCore.get_IsConnected())
                return nValue;

            m_cObjDataTable.Refresh();

            object cValue = null;
            m_cObjNumRegN.GetValue(m_cObjNumRegN.StartIndex + (int)nIndex, ref cValue);
            if (cValue == null)
                throw new Exception("Robot disconnected. | 手臂連線異常");

            return Convert.ToInt32(cValue);
        }

        public void vSetRegisterFloat(int nIndex, float fValue)
        {
            if (!m_cObjCore.get_IsConnected())
                return;

            float[] fValues = new float[1];
            fValues[0] = fValue;
            m_cObjNumRegF.SetValues(m_cObjNumRegF.StartIndex + (int)nIndex, fValues, 1);
        }

        public float fGetRegisterFloat(int nIndex, float fValue)
        {
            if (!m_cObjCore.get_IsConnected())
                return fValue;
            m_cObjDataTable.Refresh();

            object cValue = null;
            m_cObjNumRegF.GetValue(m_cObjNumRegF.StartIndex + (int)nIndex, ref cValue);

            return Convert.ToSingle(cValue);
        }

        public bool bGetCurrentPos(ref Array fXYZWPR, ref Array nConfig, ref Array fJoint, ref short nUF, ref short nUT, ref short nValidC, ref short nValidJ)
        {
            if (!m_cObjCore.get_IsConnected())
                return false;

            m_cObjDataTable.Refresh();
            m_cObjCurPos.GetValue(ref fXYZWPR, ref nConfig, ref fJoint, ref nUF, ref nUT, ref nValidC, ref nValidJ);

            return true;
        }

        public bool bGetRegisterPos(int nIndex, ref Array fXYZWPR, ref Array nConfig, ref Array fJoint, ref short nUF, ref short nUT, ref short nValidC, ref short nValidJ)
        {
            if (!m_cObjCore.get_IsConnected())
                return false;

            m_cObjDataTable.Refresh();


            m_cObjPosReg.GetValue(nIndex, ref fXYZWPR, ref nConfig, ref fJoint, ref nUF, ref nUT, ref nValidC, ref nValidJ);

            return true;
        }

        public bool bSetRegisterPos(int nIndex, ref Array fXYZWPR, ref Array nConfig, short nUF, short nUT)
        {
            if (!m_cObjCore.get_IsConnected())
                return false;

            m_cObjPosReg.SetValueXyzwpr(nIndex, ref fXYZWPR, ref nConfig, nUF, nUT);
            return true;
        }

        public bool bSetRegisterPos(int nIndex, ref Array fJoint, short nUF, short nUT)
        {
            if (!m_cObjCore.get_IsConnected())
                return false;

            m_cObjPosReg.SetValueJoint(nIndex, ref fJoint, 15, 15);
            return true;
        }
    }
}

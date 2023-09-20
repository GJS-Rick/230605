using FileStreamLibrary;
using nsUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CommonLibrary
{
    public class CommonManagerDef : IDisposable
    {
        public enum KeyboardCallMethod
        {
            /// <summary>原本方法</summary>
            Original,
            /// <summary>路徑呼叫</summary>
            PathCall,
            /// <summary>透過Dll呼叫，即使OS安裝在別的磁碟槽也依然找的到</summary>
            WindowsDll
        }
        public struct TIMECAPS
        {
            public UInt32 wPeriodMin;
            public UInt32 wPeriodMax;
        }

        public enum LoadMode
        {
            None,
            Pallet,
            PanelTray,
            LRack
        }

        public enum ProductType
        {
            Pcb,
            Mylar
        }

        TIMECAPS lpTimeCaps;
        uint Accuracy;
        const int TIMER_ACCURACY = 1;
        const int TIMERR_NOERROR = 0;

        [DllImport("winmm.dll")]
        public static extern int timeGetDevCaps(ref TIMECAPS lpTimeCaps, int uSize);
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        private static extern uint timeBeginPeriod(uint uMilliseconds);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
        private static extern uint timeEndPeriod(uint uMilliseconds);

        #region 呼叫虛擬鍵盤用
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);
        private const UInt32 WM_SYSCOMMAND = 0x112;
        private const UInt32 SC_RESTORE = 0xf120;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);



        public void KillVirtualKeyboard()
        {
            Process[] MyProcess = Process.GetProcessesByName("osk");
            if (MyProcess.Length > 0)
                MyProcess[0].Kill();
        }
        #endregion

        #region 變更MsgBox位置用
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(IntPtr classname, string title); // extern method: FindWindow
        [DllImport("user32.dll")]
        static extern void MoveWindow(IntPtr hwnd, int X, int Y,
            int nWidth, int nHeight, bool rePaint); // extern method: MoveWindow
        [DllImport("user32.dll")]
        static extern bool GetWindowRect
            (IntPtr hwnd, out Rectangle rect); // extern method: GetWindowRect

        /// <summary>尋找與移動MsgBox。
        /// 調用Messagebox.show(...)函数前，調用函数FindAndMoveMsgBox(...)即可。
        /// 需注意，由於FindAndMoveMsgBox(...)是通過Title來查找Messagebox
        /// 因此Messagebox.show(...)函數的Caption參數一定與函數FindAndMoveMsgBox(...)中的title相等。
        /// 範例：
        /// _comm.FindAndMoveMsgBox(0, 0, true, "程式停止");
        /// if (MessageBox.Show("是否停止運行?", "程式停止", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) == DialogResult.Yes)</summary>
        public void FindAndMoveMsgBox(int x, int y, bool repaint, string title)
        {
            Thread thr = new Thread(() => // create a new thread
            {
                IntPtr msgBox = IntPtr.Zero;
                // while there's no MessageBox, FindWindow returns IntPtr.Zero
                while ((msgBox = FindWindow(IntPtr.Zero, title)) == IntPtr.Zero) ;
                // after the while loop, msgBox is the handle of your MessageBox
                Rectangle r = new Rectangle();
                GetWindowRect(msgBox, out r); // Gets the rectangle of the message box
                MoveWindow(msgBox /* handle of the message box */, FmMain.ActiveForm.Location.X + x, FmMain.ActiveForm.Location.Y + y,
                   r.Width - r.X /* width of originally message box */,
                   r.Height - r.Y /* height of originally message box */,
                   repaint /* if true, the message box repaints */);
            });
            thr.Start(); // starts the thread
        }

        //將MsgBox鎖定在最上層範例：
        //MessageBox.Show("學習完成請取出樣本", "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        //if (MessageBox.Show("是否調整吸盤?", "吸盤調整", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) == DialogResult.Yes)
        #endregion

        public MtnCtrlDef MtnCtrl;
        public IOCtrlDef IOCtrl;
        public CylinderDef CYL;
        public VacSuckerDef Vac;
        public CycleTime Cycle;
        public DoorDef Door;
        public TrolleyDef Trolley;
        public MotorPosCollectionDef MotorPosCollection;
        public AlarmTextDisplay AlarmTextDisplay;
        public LogDef Log;
        public LoginDef Login;
        public MaintainDef Maintain;
        public TCPIPClientDef TCPIPClient;
        public DataTransferDef DataTransfer;
        public RobotPipeClientDef FanucRobot;
        public LanguageSwitch LanSwitch;
        public ScaraGJSDef Scara;//Scara設定
        public LiftsDef Lifts;
        public VibrationBowDef VibBow;
        //Monitor
        public MonitorDef Monitor;
        public DetectDef Detect;//讀頭

        public KeyboardDef Keyboard;

        public LoadMode Mode;
        public bool ComponentsReady;

        public bool VolumeMute = false;
        public ProductionQuantity ProductionQ;

        public float TemperatureValue;
        public float RelativeHumidityValue;
        public float CurrentPowerValue;
        public float DifferentialPressureValue;
        public float ElectricStaticValue;
        public TBotGJSDef TBot;
        public List<SerialPort> ComPorts;

        private FileDeleteDef _FileDelete;

        string _SystemDirPath;
        string _LogPath;
        int _LogKeepDay;

        #region 共用狀態
        /// <summary>前一使用者權限等級</summary>
        public ELoginLevel PreUserLv;
        /// <summary>目前使用者權限等級</summary>
        public ELoginLevel UserLv;
        /// <summary>設備機型</summary>
        public EMechanicalModel MechanicalModel;
        #endregion

        public CommonManagerDef()
        {
            ComPorts = new List<SerialPort>();

            try { Initiallize(); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { ProductionQ = new ProductionQuantity(); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Cycle = new CycleTime(); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Log = new LogDef(_LogPath, _LogKeepDay); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Keyboard = new KeyboardDef(); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try
            {
                string[] _AlarmCodeStr = new string[(int)AlarmCode.Count];
                for (int i = 0; i < (int)AlarmCode.Count; i++)
                    _AlarmCodeStr[i] = ((AlarmCode)i).ToString();
                AlarmTextDisplay = new AlarmTextDisplay(_SystemDirPath, _AlarmCodeStr);
            }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { LanSwitch = new LanguageSwitch(_SystemDirPath, ELanguages.Chinese_TW); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Maintain = new MaintainDef(_SystemDirPath); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Login = new LoginDef(_SystemDirPath); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { _FileDelete = new FileDeleteDef(_SystemDirPath); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            #region DataTransfer
            string[] StrArrBuf = new string[(int)EDataTransferName.Count];
            for (int i = 0; i < (int)EDataTransferName.Count; i++)
                StrArrBuf[i] = ((EDataTransferName)i).ToString();
            try { DataTransfer = new DataTransferDef(StrArrBuf); }
            catch (Exception ex) { GenerateExceptionContent(ex); }
            #endregion

            MtnCtrl = new MtnCtrlDef(ComPorts, _SystemDirPath);     // 物件內建Exception
            ComponentsReady = MtnCtrl.IsValid && ComponentsReady;

            IOCtrl = new IOCtrlDef(_SystemDirPath);
            ComponentsReady = IOCtrl.IsValid && ComponentsReady;    // 物件內建Exception

            try { CYL = new CylinderDef(_SystemDirPath + "\\Mod.ini"); }  //氣缸定義
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Vac = new VacSuckerDef(_SystemDirPath + "\\Mod.ini"); } //吸盤定義
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Door = new DoorDef(_SystemDirPath + "\\Mod.ini"); } //門鎖
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Trolley = new TrolleyDef(_SystemDirPath + "\\Mod.ini"); }  //台車 
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Lifts = new LiftsDef(_SystemDirPath + "\\Mod.ini"); }  //升降台 
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { VibBow = new VibrationBowDef(_SystemDirPath + "\\Mod.ini"); }  //震動送料機 
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { MotorPosCollection = new MotorPosCollectionDef(_SystemDirPath); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { Monitor = new MonitorDef(_SystemDirPath + "\\Monitor.ini"); }
            catch (Exception ex) { GenerateExceptionContent(ex); }
            try { Detect = new DetectDef(_SystemDirPath + "\\Detect.ini"); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            try { TBot = new TBotGJSDef(ComPorts, _SystemDirPath); }
            catch (Exception ex) { GenerateExceptionContent(ex); }

            //
            //TCPIPClient = new TCPIPClientDef(_SystemDirPath + "\\");

            //System.Threading.Thread.Sleep(300);

            //FanucRobot = new RobotPipeClientDef(_SystemDirPath);

            //以下物件內建Exception
            Scara = new ScaraGJSDef(ComPorts, _SystemDirPath);
            ComponentsReady = Scara.IsValid && ComponentsReady;
        }

        public void GenerateExceptionContent(Exception ex)
        {
            var frame = (new StackTrace(ex, true)).GetFrame(0);
            var className = frame.GetMethod().DeclaringType.FullName;
            var methodName = frame.GetMethod().Name;
            string msg = "ERROR CLASS : " + className + Environment.NewLine +
                "ERROR FUNCTION : " + methodName + "()" + Environment.NewLine +
                "ERROR KEY : " + "NONE" + Environment.NewLine +
                "ERROR CONTENT : " + Environment.NewLine + ex.ToString();

            AlarmTextDisplay.Add((int)AlarmCode.Alarm_CommonObjError, AlarmType.Alarm, msg);

            MessageBox.Show(
                msg,
                this.GetType().Name,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            ComponentsReady = false;
        }

        public void Initiallize()
        {
            lpTimeCaps = new TIMECAPS();
            if (timeGetDevCaps(ref lpTimeCaps, Marshal.SizeOf(lpTimeCaps)) == TIMERR_NOERROR)
            {
                Accuracy = Math.Min(Math.Max(lpTimeCaps.wPeriodMin, TIMER_ACCURACY), lpTimeCaps.wPeriodMax);
                timeBeginPeriod(Accuracy);
            }

            ComponentsReady = true;

            PreUserLv = ELoginLevel.Operator;
            UserLv = ELoginLevel.Operator;
            string ProIniPath = Application.ExecutablePath.Split('.')[0] + ".ini";

            if (!File.Exists(ProIniPath))
                throw new Exception("Can't find the EXE.ini file: " + ProIniPath);

            IniFile cExeIniInfo = new IniFile(ProIniPath, true);

            _SystemDirPath = cExeIniInfo.ReadStr("System", "SystemDirPath", "C:\\Automation");
            _LogPath = cExeIniInfo.ReadStr("System", "LogPath", "C:\\LogPath");
            _LogKeepDay = cExeIniInfo.ReadInt("System", "LogKeepDays", 30);

            cExeIniInfo.FileClose();
            cExeIniInfo.Dispose();

            if (!Directory.Exists(_SystemDirPath))
                throw new Exception("No System Directory : " + _SystemDirPath);
        }

        public bool ComponentReady() { return ComponentsReady; }

        public void Dispose()
        {
            Keyboard.Kill();

            LanSwitch = null;

            timeEndPeriod(Accuracy);

            if (Monitor != null)
                Monitor.Dispose();

            if (Maintain != null)
                Maintain.Dispose();

            if (DataTransfer != null)
                DataTransfer.Dispose();
            DataTransfer = null;

            if (TCPIPClient != null)
                TCPIPClient.Dispose();

            if (MotorPosCollection != null)
                MotorPosCollection.Dispose();

            if (Login != null)
                Login = null;

            if (_FileDelete != null)
                _FileDelete.Dispose();

            if (CYL != null)
                CYL.Dispose();
            CYL = null;

            if (Vac != null)
                Vac.Dispose();
            Vac = null;

            if (Door != null)
                Door.Dispose();
            Door = null;

            if (Trolley != null)
                Trolley.Dispose();
            Trolley = null;

            if (Monitor != null)
                Monitor.Dispose();
            Monitor = null;

            if (MtnCtrl != null)
                MtnCtrl.Dispose();
            MtnCtrl = null;

            if (IOCtrl != null)
                IOCtrl.Dispose();
            IOCtrl = null;

            if (AlarmTextDisplay != null)
                AlarmTextDisplay.Dispose();

            if (Log != null)
                Log.Dispose();
            Log = null;

            //Scara設定
            if (Scara != null)
                Scara.Dispose();

            if (TBot != null)
                TBot.Dispose();
        }

        public String GetSystemDirPath() { return _SystemDirPath; }

        #region Login狀態
        public void RenewState()
        {
            UserLv = Login.GetLevel();
        }

        public bool UserLvChange()
        {
            if (UserLv != PreUserLv)
            {
                PreUserLv = UserLv;
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
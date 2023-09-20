using CommonLibrary;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FileStreamLibrary;
using GJSControl.UI;
using nsSequence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using VisionLibrary;

namespace nsUI
{
    public partial class FmMain : Form
    {
        private GJSControl.WelcomeWindow _WelcomeWindow;

        private Form _inSubPanelForm;
        #region Insert SubPanel Form or UserControl
        private FmPicDisplay _FmPicDisplay;
        private FmFunctionPage _FmFuncPage;
        private int PreviousTabIndex;
        #endregion
        private ELoginLevel PreELoginLevel;
        private ELoginLevel NowELoginLevel;
        Label[] ExtConnectLbl;
        EDI_TYPE[] ExtConnectDI;
        EDO_TYPE[] ExtConnectDO;

        private int _AlarmDisplayTickCount;
        private int _AlarmDisplayStep;
        private AlarmType _LastAlarmType = AlarmType.None;
        private string _LastAlarmText;
        private int _FunctionDisplayCount;
        /// <summary>登入起始時間</summary>
        //private ref string QRCode = new 

        /// <summary>配方RBtn</summary>
        private RadioButton[] _RadioButtonRecipes;

        CommonManagerDef.LoadMode _TempMode = CommonManagerDef.LoadMode.None;

        public FmMain()
        {
            InitializeComponent();

            SetBtnClickEvnet();
        }

        private void FmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckUSBLock())
                G.Comm.Login.SetLevel(ELoginLevel.Operator);
            else
                G.Comm.Login.SetLevel(ELoginLevel.Developer);

            timerUpdate.Enabled = false;

            int nTickCount = Environment.TickCount;
            if (Environment.TickCount - nTickCount < 1000)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }

            if (G.UI != null)
                G.UI.Dispose();

            if (G.Seq != null)
                G.Seq.Dispose();

            if (G.Comm != null)
                G.Comm.Dispose();

            if (G.Vision != null)
                G.Vision.Dispose();

            if (G.FS != null)
                G.FS.Dispose();

            DisposeUserPanel();
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _WelcomeWindow = new GJSControl.WelcomeWindow();
                _WelcomeWindow.StartPosition = FormStartPosition.CenterScreen;

                _WelcomeWindow.Show();

                #region 參數載入
                _WelcomeWindow.ShowMessage("參數載入中...");

                #region 機台參數載入
                _WelcomeWindow.ShowMessage("機台參數載入中...");
                string sSystemDirPath = GetSystemDirPath();
                G.FS = new FileManagerDef(sSystemDirPath);
                _WelcomeWindow.ShowMessage("機台參數載入完成");
                #endregion

                #region 部件初始化
                _WelcomeWindow.ShowMessage("部件初始化中...");

                #region 共同元件載入
                _WelcomeWindow.ShowMessage("共同元件載入中...");
                G.Comm = new CommonManagerDef();
                _WelcomeWindow.ShowMessage("共同元件載入完成");
                #endregion

                #region 起始權限設定
                _WelcomeWindow.ShowMessage("起始權限設定中...");
                G.Comm.Login.SetLevel(ELoginLevel.Developer);
                _WelcomeWindow.ShowMessage("起始權限設定完成");
                #endregion

                #region Scara 載入
                _WelcomeWindow.ShowMessage("Scara 載入中...");

                _WelcomeWindow.ShowMessage("Scara 載入完成");
                #endregion Scara 運動參數載入

                #region 視覺模組載入
                _WelcomeWindow.ShowMessage("視覺模組載入中...");
                G.Vision = new VisionManagerDef(sSystemDirPath);
                _WelcomeWindow.ShowMessage("視覺模組載入完成");
                #endregion
                #region 視覺參數載入
                _WelcomeWindow.ShowMessage("視覺參數載入中...");
                //G.VisionManager.AlignAlgorithm.vLoad(sSystemDirPath + "\\Recipe" + "\\" + G.FileStream.RecipeCollection.GetDefaultRecipeName() + "\\");
                _WelcomeWindow.ShowMessage("視覺參數載入完成");
                #endregion

                #region 對位演算法參數載入
                _WelcomeWindow.ShowMessage("對位演算法參數載入中...");

                _WelcomeWindow.ShowMessage("對位演算法參數載入完成");
                #endregion

                #region 介面載入
                _WelcomeWindow.ShowMessage("介面載入中...");
                _FmPicDisplay = new FmPicDisplay();
                G.UI = new cUIManagerDef(this, _FmPicDisplay);
                _WelcomeWindow.ShowMessage("介面載入完成");
                #endregion

                #region 介面語言確認
                _WelcomeWindow.ShowMessage("介面語言確認中...");
                G.Comm.LanSwitch.CheckFmLanguageFile(this);
                _WelcomeWindow.ShowMessage("介面語言確認完成");
                #endregion

                #region 機台程序初始化
                _WelcomeWindow.ShowMessage("機台程序初始化...");
                G.Seq = new SequenceManagerDef();
                _WelcomeWindow.ShowMessage("機台程序初始化完成");
                #endregion

                #region 介面重繪
                _WelcomeWindow.ShowMessage("介面重繪中...");
                G.Comm.Login.UIEnable(this, ERunStatus.BeforeInitial);
                _WelcomeWindow.ShowMessage("起始權限設定完成");
                #endregion

                #region 控件初始化
                _WelcomeWindow.ShowMessage("控件初始化...");
                _WelcomeWindow.ShowMessage("控件初始化...基本控件");
                Button[] _fmMainBtn = new Button[] { BtnStop, BtnAuto, BtnPause, BtnLotEnd, BtnLogin, BtnAlarmReset, BtnFunction, BtnExit };
                BtnPositionSizeSetting(8, 8, 8, 8, 8, _fmMainBtn, 119, 106);
                _WelcomeWindow.ShowMessage("控件初始化...設備資訊");
                #region TblLP_Info
                TblLP_Info.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                #endregion
                #region Monitor
                DGV_Monitor.AlternatingRowsDefaultCellStyle.Font = new Font("Microsoft JhengHei UI", (float)14.25, FontStyle.Regular);
                DGV_Monitor.DefaultCellStyle.Font = new Font("Microsoft JhengHei UI", (float)14.25, FontStyle.Regular);
                #endregion
                #region mBtn
                #region BtnExtConnect(透明按鈕)
                BtnExtConnect.Left = 361;
                BtnExtConnect.Top = 226;
                BtnExtConnect.Width = 20;
                BtnExtConnect.Height = 20;
                BtnExtConnect.FlatStyle = FlatStyle.Flat;//樣式
                BtnExtConnect.ForeColor = Color.Transparent;//前景
                BtnExtConnect.BackColor = Color.Transparent;//去背景
                BtnExtConnect.FlatAppearance.BorderSize = 0;//去邊線
                BtnExtConnect.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠標經過
                BtnExtConnect.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠標按下
                #endregion
                #endregion
                #region External Connect
                #region RemotePC
                DGV_ExteralConnect.AlternatingRowsDefaultCellStyle.Font = new Font("Microsoft JhengHei UI", (float)12, FontStyle.Regular);
                DGV_ExteralConnect.DefaultCellStyle.Font = new Font("Microsoft JhengHei UI", (float)12, FontStyle.Regular);
                DGV_ExteralConnect.Rows.Clear();
                #endregion

                #region DIO
                ExtConnectDI = new EDI_TYPE[] {
                    EDI_TYPE.ExternalConnect_In1,
                     };//設定外部連線DI
                ExtConnectDO = new EDO_TYPE[] {
                    EDO_TYPE.External_Connect_Out1,
                     };//設定外部連線DO
                ExtConnectLbl = new Label[ExtConnectDI.Length + ExtConnectDO.Length];
                for (int i = 0; i < ExtConnectLbl.Length; i++)
                {
                    ExtConnectLbl[i] = new Label();

                    ExtConnectLbl[i].BorderStyle = BorderStyle.FixedSingle;
                    ExtConnectLbl[i].Name = "ECDIO_" + i.ToString();
                    ExtConnectLbl[i].Click += new EventHandler(ExtConnectIO_Click);

                    this.m.Controls.Add(ExtConnectLbl[i]);

                    ExtConnectLbl[i].BringToFront();
                }
                mLblPosSizeSetting(m.Width - 15, m.Height - 15, 0, 0, 1, ExtConnectLbl, 10, 10, true);
                #endregion
                #endregion
                _WelcomeWindow.ShowMessage("控件初始化...操作分頁");
                #region TabCtl
                #region MiniBtn
                BtnPrePage1.Enabled = false;
                BtnFirstPage1.Enabled = false;
                BtnNextPage1.Enabled = true;

                BtnPrePage2.Enabled = true;
                BtnFirstPage2.Enabled = true;
                BtnNextPage2.Enabled = true;

                BtnPrePage3.Enabled = true;
                BtnFirstPage3.Enabled = true;
                BtnNextPage3.Enabled = false;
                #endregion

                TabPageHide(this.TabCtl);
                PreviousTabIndex = 4;
                SelectUserPageHome();
                #endregion
                #region BtnDl(透明按鈕)
                BtnDl.FlatStyle = FlatStyle.Flat;//樣式
                BtnDl.ForeColor = Color.Transparent;//前景
                BtnDl.BackColor = Color.Transparent;//去背景
                BtnDl.FlatAppearance.BorderSize = 0;//去邊線
                BtnDl.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠標經過
                BtnDl.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠標按下
                #endregion

                _WelcomeWindow.ShowMessage("控件初始化完成");
                #endregion

                #region 子面板初始化
                _WelcomeWindow.ShowMessage("子面板初始化...");
                NewUserPanel();
                _WelcomeWindow.ShowMessage("子面板初始化...面板預載");
                PanelShow(G.UI.frmMotionFm, true);
                PanelShow(G.UI.frmIOFm, false);
                PanelShow(_FmPicDisplay, false);
                _WelcomeWindow.ShowMessage("子面板初始化完成");
                #endregion

                #region 其他初始化
                ChangBtnVolumeImg();
                #endregion

                _WelcomeWindow.ShowMessage("部件初始化完成");
                #endregion

                _WelcomeWindow.ShowMessage("參數載入完成");
                #endregion

                DGV_Monitor.Rows.Clear();
                timerUpdate.Enabled = true;

                _AlarmDisplayStep = 0;
                _AlarmDisplayTickCount = Environment.TickCount;

                _WelcomeWindow.Close();

                this.Size = new System.Drawing.Size(1024, 768);

                #region 起始權限設定
                PreELoginLevel = ELoginLevel.Operator;
                G.Comm.Login.SetLevel(ELoginLevel.Operator);
                #endregion
                #region 機型設定
                G.Comm.MechanicalModel = EMechanicalModel.UnLoad_Pin;
                #endregion
                G.Comm.LanSwitch.SetExceptionCtlName(new string[] {
                    NumUD_PanelNum.Name,
                    LblRecipe.Name });
                G.Comm.LanSwitch.ReadFmLanguageFile(this);//BoWei 讀取語言
                if (SubPanel.Controls.Count > 0)
                    G.Comm.LanSwitch.ReadFmLanguageFile(SubPanel.Controls[0]);//BoWei 子介面讀取語言
                ReLoadParam();

                LblVersionValue.Text = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
                this.Size = new Size(1024, 768);
                LogDef.Add(ELogFileName.Operate, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Start Program");
                _FunctionDisplayCount = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static string GetSystemDirPath()
        {
            string ProIniPath = Application.ExecutablePath.Split('.')[0] + ".ini";
            string SysDirPath;

            string[] NetAddressInfo = GetNetAddress();

            IniFile cExeIniInfo;
            if (File.Exists(ProIniPath))
            {
                cExeIniInfo = new IniFile(ProIniPath, true);
                SysDirPath = cExeIniInfo.ReadStr("System", "SystemDirPath", "C:\\Automation");
            }
            else
            {
                cExeIniInfo = new IniFile(ProIniPath, false);
                cExeIniInfo.WriteInt("System", "LogKeepDays", 365);
                cExeIniInfo.WriteStr("System", "SystemDirPath", "C:\\Automation");
                cExeIniInfo.WriteStr("System", "LogPath", "C:\\Log");
                cExeIniInfo.WriteBool("USB Port", "ExitLock", true);

                #region Local NetAddress Info
                for (int i = 0; i < NetAddressInfo.Length; i++)
                {
                    string key = "Other";
                    if (i == 0)
                        key = "Computer Name";
                    else if (i == 1)
                        key = "MAC Address";
                    else if (i == 2)
                        key = "Network address";

                    cExeIniInfo.WriteStr("Local NetAddress Info", key, NetAddressInfo[i]);
                }
                #endregion

                SysDirPath = "C:\\Automation";
            }
            cExeIniInfo.FileClose();
            cExeIniInfo.Dispose();
            cExeIniInfo = null;

            return SysDirPath;
        }

        private bool CheckUSBLock()
        {
            string ProIniPath = Application.ExecutablePath.Split('.')[0] + ".ini";
            bool USBLock = false;

            IniFile cExeIniInfo = new IniFile(ProIniPath, true);

            if (File.Exists(ProIniPath))
                USBLock = cExeIniInfo.ReadBool("USB Port", "ExitLock", true);

            cExeIniInfo.FileClose();
            cExeIniInfo.Dispose();
            cExeIniInfo = null;

            return USBLock;
        }
        private static string[] GetNetAddress()
        {
            List<string> NetAddressInfo = new List<string>();

            // 取得本機名稱
            String strHostName = Dns.GetHostName();
            // 取得本機的 IpHostEntry 類別實體
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            NetAddressInfo.Add(strHostName);

            // 取得所有 IP 位址
            int num = 1;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                NetAddressInfo.Add(ipaddress.ToString());
                num = num + 1;
            }

            return NetAddressInfo.ToArray();
        }

        /// <summary>顯示於子面板</summary>
        /// <param name="ePanel">要顯示的面板</param>
        /// <param name="eChangeLan">語言是否切換</param>
        /// <remarks>
        /// <list type="要顯示的面板">ePanel: 要顯示的面板</list>
        /// <list type="語言是否切換">eChangeLan: 語言是否切換</list>
        /// </remarks>
        public void PanelShow(Form ePanel, bool eChangeLan)//BoWei 顯示於子面板
        {
            SubPanel.Controls.Clear();
            SubPanel.AutoScroll = false;
            ePanel.TopLevel = false;
            ePanel.AutoSize = false;
            ePanel.AutoScroll = false;
            ePanel.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            SubPanel.Controls.Add(ePanel);
            _inSubPanelForm = ePanel;
            ePanel.Show();
            ePanel.WindowState = FormWindowState.Maximized;

            if (eChangeLan)
                G.Comm.LanSwitch.ReadFmLanguageFile(ePanel);

            G.Comm.Login.UIEnable();

            if (ePanel == _FmFuncPage)
                _FmFuncPage.Invoke(new Action(() => { _FmFuncPage.UIBtnSort(); }));
            if (ePanel == G.UI.frmPicFm)
                G.UI.frmPicFm.Invoke(new Action(() => { G.UI.frmPicFm.UpdateImage(); }));
        }

        private void NewUserPanel()
        {
            _FmFuncPage = new FmFunctionPage();
        }

        private void DisposeUserPanel()
        {
            if (_FmPicDisplay != null)
                _FmPicDisplay.Dispose();

            if (_FmFuncPage != null)
                _FmFuncPage.Dispose();
        }

        /// <summary>取得各Sequence狀態</summary>
        private void GetSequenceStatus()
        {
            String SeqStatusBuf = "";

            // 0: SequenceName, 1: _autoStep, 2: _moveStep
            foreach (string[] SequenceStatus in G.Seq.MainSequence.GetAllSequenceStep())
                SeqStatusBuf = SeqStatusBuf + SequenceStatus[0].Split('.')[1] + " : " + "[" + SequenceStatus[1] + "]" + SequenceStatus[2] + "\r\n";
            SeqStatusBuf = SeqStatusBuf + "\r\n";

            #region 預新增的顯示項
            #region CycleTime
            string[] AllCycleName = G.Comm.Cycle.GetAllName();
            for (int i = 0; i < AllCycleName.Length; i++)
                SeqStatusBuf = SeqStatusBuf + AllCycleName[i] + " CT: " + G.Comm.Cycle.GetUsedTime(AllCycleName[i]).ToString("0.0 Sec") + "\r\n";
            #endregion
            #endregion

            try { SeqStatusBuf = SeqStatusBuf.Substring(0, SeqStatusBuf.Length - 2); } catch (Exception) { }//刪除字串最後的\r\n

            TxtBxSequenceStatus.Text = SeqStatusBuf;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (m.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = m.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        if ((int)G.Comm.UserLv <= (int)ELoginLevel.Operator)
                            return;
                    }

                    break;
            }
            base.WndProc(ref m);
        }

        private void BtnPositionSizeSetting(int StartX, int StartY, int DisX, int DisY, int BtnNum, Button[] _fmMainBtn, int W, int H)
        {
            int _offset = 0;
            for (int i = 0; i < _fmMainBtn.Length; i++)
            {
                _fmMainBtn[i].Width = W;
                _fmMainBtn[i].Height = H;

                if (!_fmMainBtn[i].Visible)
                    _offset = _offset - 1;

                _fmMainBtn[i].Left = StartX + ((i + _offset) % BtnNum) * (_fmMainBtn[i].Width + DisX);
                _fmMainBtn[i].Top = StartY + ((i + _offset) / BtnNum) * (_fmMainBtn[i].Height + DisY);
            }
        }

        private void mLblPosSizeSetting(int StartX, int StartY, int DisX, int DisY, int BtnNum, Label[] _fmMainLbl, int W, int H, bool _arrRevers = false)
        {
            int _offset = 0;

            if (_arrRevers)
                Array.Reverse(_fmMainLbl);

            for (int i = 0; i < _fmMainLbl.Length; i++)
            {
                _fmMainLbl[i].Width = W;
                _fmMainLbl[i].Height = H;

                //if (!_fmMainLbl[i].Visible)
                //    _offset--;

                _fmMainLbl[i].Left = StartX + ((i + _offset) % BtnNum) * (_fmMainLbl[i].Width + DisX);
                if (_arrRevers)
                    _fmMainLbl[i].Top = StartY - ((i + _offset) / BtnNum) * (_fmMainLbl[i].Height + DisY);
                else
                    _fmMainLbl[i].Top = StartY + ((i + _offset) / BtnNum) * (_fmMainLbl[i].Height + DisY);
            }
        }

        /// <summary>更新遠端PC與外部DIO DGV</summary>
        private void RenewExtConnectDGV()
        {
            string[] _BufIP = G.Comm.DataTransfer.GetRemotePCIP();
            bool _DIOBuf = false;

            for (int i = 0; i < ExtConnectLbl.Length + _BufIP.Length; i++)
            {
                if (i >= DGV_ExteralConnect.Rows.Count)
                    DGV_ExteralConnect.Rows.Add(new string[3]);

                if (i < ExtConnectLbl.Length)
                {
                    if (i < ExtConnectDI.Length)
                    {
                        DGV_ExteralConnect.Rows[i].Cells[0].Value = G.Comm.IOCtrl.GetDIName(ExtConnectDI[i]);
                        DGV_ExteralConnect.Rows[i].Cells[0].Style.BackColor = Color.White;

                        DGV_ExteralConnect.Rows[i].Cells[1].Value = "On";
                        DGV_ExteralConnect.Rows[i].Cells[2].Value = "Off";

                        _DIOBuf = G.Comm.IOCtrl.GetDI(ExtConnectDI[i], false);
                        if (_DIOBuf)
                        {
                            ExtConnectLbl[i].BackColor = Color.Lime;

                            DGV_ExteralConnect.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                            DGV_ExteralConnect.Rows[i].Cells[2].Style.BackColor = Color.DarkSeaGreen;
                        }
                        else
                        {
                            ExtConnectLbl[i].BackColor = Color.DarkSeaGreen;

                            DGV_ExteralConnect.Rows[i].Cells[1].Style.BackColor = Color.DarkSeaGreen;
                            DGV_ExteralConnect.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                        }
                    }
                    else
                    {
                        DGV_ExteralConnect.Rows[i].Cells[0].Value = G.Comm.IOCtrl.GetDOName(ExtConnectDO[i - ExtConnectDI.Length]);
                        DGV_ExteralConnect.Rows[i].Cells[0].Style.BackColor = Color.White;

                        DGV_ExteralConnect.Rows[i].Cells[1].Value = "On";
                        DGV_ExteralConnect.Rows[i].Cells[2].Value = "Off";

                        _DIOBuf = G.Comm.IOCtrl.GetDO(ExtConnectDO[i - ExtConnectDI.Length], false);
                        if (_DIOBuf)
                        {
                            ExtConnectLbl[i].BackColor = Color.Lime;

                            DGV_ExteralConnect.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                            DGV_ExteralConnect.Rows[i].Cells[2].Style.BackColor = Color.DarkSeaGreen;
                        }
                        else
                        {
                            ExtConnectLbl[i].BackColor = Color.DarkSeaGreen;

                            DGV_ExteralConnect.Rows[i].Cells[1].Style.BackColor = Color.DarkSeaGreen;
                            DGV_ExteralConnect.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                        }
                    }
                }
                else
                {
                    int _i = i - ExtConnectLbl.Length;

                    DGV_ExteralConnect.Rows[i].Cells[0].Value = _BufIP[_i];
                    DGV_ExteralConnect.Rows[i].Cells[0].Style.BackColor = Color.White;

                    DGV_ExteralConnect.Rows[i].Cells[1].Value = "DiskLink";
                    if (G.Comm.DataTransfer.IsDiskConnect(_BufIP[_i]))
                        DGV_ExteralConnect.Rows[i].Cells[1].Style.BackColor = Color.Lime;
                    else
                        DGV_ExteralConnect.Rows[i].Cells[1].Style.BackColor = Color.DarkSeaGreen;

                    DGV_ExteralConnect.Rows[i].Cells[2].Value = "Alive";
                    if (G.Comm.DataTransfer.IsAutoAlive(_BufIP[_i]))
                        DGV_ExteralConnect.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                    else
                        DGV_ExteralConnect.Rows[i].Cells[2].Style.BackColor = Color.DarkSeaGreen;
                }
            }
        }
        /// <summary>更新Monitor DGV</summary>
        private void RenewMonitorDGV()
        {
            for (int i = 0; i < G.Comm.Monitor.Value.Count; i++)
            {
                try
                {
                    if (DGV_Monitor.RowCount > G.Comm.Monitor.Value.Count)
                        DGV_Monitor.Rows.Clear();

                    for (int j = 0; j < DGV_Monitor.ColumnCount; j++)
                        DGV_Monitor.Rows[i].Cells[j].Value = G.Comm.Monitor.Value[i][j];
                }
                catch
                {
                    DGV_Monitor.Rows.Add(G.Comm.Monitor.Value[i]);
                }
            }
        }

        /// <summary>TabPage隱藏</summary>
        private void TabPageHide(TabControl TabCtl)
        {
            for (int i = 0; i < TabCtl.TabPages.Count; i++)
            {
                TabCtl.Region = new Region(new RectangleF(TabCtl.TabPages[i].Left, TabCtl.TabPages[i].Top, TabCtl.TabPages[i].Width, TabCtl.TabPages[i].Height));
                TabCtl.TabPages[i].BackColor = Color.FromArgb(0xdd2378);
            }
        }

        /// <summary>TabPage選擇</summary>
        /// <param name="TabPageIndex">Tab頁數</param>
        /// <remarks>
        /// <list type="Tab頁數">TabPageIndex: Tab頁數</list>
        /// <list type="Page0">Page0: Sequencq</list>
        /// <list type="Page1">Page1: Monitor</list>
        /// <list type="Page2">Page2: Recipe</list>
        /// <list type="Page3">Page3: Test</list>
        /// <list type="Page4">Page4: ExternalConnect_DIO</list>
        /// <list type="Page5">Page5: UserPage1</list>
        /// <list type="Page6">Page6: UserPage2</list>
        /// <list type="Page7">Page7: UserPage3</list>
        /// </remarks>
        private void SelectTabPage(int TabPageIndex)
        {
            PreviousTabIndex = TabCtl.SelectedIndex;
            TabCtl.SelectedIndex = TabPageIndex;
        }

        private void ShowPreviousTabPage()
        {
            TabCtl.SelectedIndex = PreviousTabIndex;
        }

        /// <summary>參數重載</summary>
        /// <remarks>變更語系或切換介面時，若有數值因此變更，建議從此處加入需重新載入的參數</remarks>
        public void ReLoadParam()
        {

        }

        public void ChangeLanguage()
        {
            G.Comm.LanSwitch.NextLanguage();
            G.Comm.LanSwitch.ReadFmLanguageFile(this);
            if (_inSubPanelForm != null)
                G.Comm.LanSwitch.ReadFmLanguageFile(_inSubPanelForm);

            RenewUserValue();
            LblVersionValue.Text = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
        }

        private string[] GetLoginLevel()
        {
            List<string> LoginNames = new List<string>();

            for (int i = 0; i < (int)ELoginLevel.Count; i++)
                LoginNames.Add(((ELoginLevel)i).ToString());

            return LoginNames.ToArray();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (G.Comm.Scara != null && G.Comm.Scara.BatteryLowPowerStatus())
            {
                if (MessageBox.Show("電池低電量(Battery low power)\n是否繼續?", "SCARA", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }

            if (MessageBox.Show("是否離開?", "Exit", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                #region 塔燈關閉
                G.Comm.IOCtrl.SetDO(EDO_TYPE.StackLight_Buzzer, false);
                G.Comm.IOCtrl.SetDO(EDO_TYPE.StackLight_Green, false);
                G.Comm.IOCtrl.SetDO(EDO_TYPE.StackLight_Red, false);
                G.Comm.IOCtrl.SetDO(EDO_TYPE.StackLight_Yellow, false);
                #endregion

                //#region HEPA關閉

                //#endregion
                //#region 靜電消除器關閉

                //#endregion
                //#region 入料吸嘴移載停止
                //G.Common.IOCtrl.SetDO(EDO_TYPE.CV_CW, false);

                //G.Common.IOCtrl.SetDO(EDO_TYPE.CV_Slow, false);
                //#endregion

                //#region 斷外部連結
                //G.Common.IOCtrl.SetDO(EDO_TYPE.ExternalConnection1, false);

                //#endregion

                Close();
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            G.UI.frmLogin.Visible = true;
            G.Comm.LanSwitch.ReadFmLanguageFile(G.UI.frmLogin);
            G.Comm.LanSwitch.GetLoginName(G.UI.frmLogin, G.UI.frmLogin.GetcbxUserID(), GetLoginLevel(), G.Comm.UserLv);
            G.UI.frmLogin.BringToFront();
        }

        private void BtnEngineering_Click(object sender, EventArgs e)
        {
            if (_inSubPanelForm != _FmFuncPage)
            {
                _FunctionDisplayCount++;
                if (_FunctionDisplayCount % 2 == 1 && G.Seq.MainSequence.TimeoutFormIsExsit())
                    PanelShow(G.Seq.MainSequence.GetTimeoutForm(), true);
                else
                    PanelShow(_FmFuncPage, true);
            }
            else
                PanelShow(_FmPicDisplay, false);
        }

        private void BtnAlarmReset_Click(object sender, EventArgs e)
        {
            if (G.Seq.MainSequence.TimeoutFormIsExsit())
            {
                PanelShow(G.Seq.MainSequence.GetTimeoutForm(), true);
                return;
            }

            if (G.Seq.MainSequence.GetStatus() == ERunStatus.Alarm)
                G.Seq.MainSequence.UserSetStatus(ERunStatus.Stop);
            G.Comm.AlarmTextDisplay.ClearFirstAlarmText();
        }

        public void BtnAutoColorChange(Color EColor)
        {
            BtnAuto.BackColor = EColor;
        }

        private void BtnAuto_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("EN: Sure to auto run?\nCH: 是否要運轉 ?", "AUTO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            string list = string.Empty;
            for (int i = 0; i < G.Comm.IOCtrl.GetDINum(); i++)
            {
                if (G.Comm.IOCtrl.GetDIPass((EDI_TYPE)i))
                    list += (G.Comm.IOCtrl.GetDIName((EDI_TYPE)i)) + Environment.NewLine;
            }

            if (list.Length > 0)
                MessageBox.Show("有檢知未偵測(Have DI pass):" + list, "Pass", MessageBoxButtons.OK);

            G.Seq.MainSequence.UserSetStatus(ERunStatus.Auto);
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            G.Seq.MainSequence.UserSetStatus(ERunStatus.Pause);
        }

        private void BtnLotEnd_Click(object sender, EventArgs e)
        {
            if (G.Seq.MainSequence.GetStatus() == ERunStatus.Auto)
                G.Seq.MainSequence.UserSetStatus(ERunStatus.End);
        }

        private void AlarmDisplaySequence()
        {
            switch (_AlarmDisplayStep)
            {
                case 0:
                    LblAlarm.Text = G.Comm.AlarmTextDisplay.GetFirstAlarmText();
                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm)
                        LblAlarm.BackColor = Color.Red;
                    else if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Warning)
                        LblAlarm.BackColor = Color.Yellow;
                    else if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Msg)
                        LblAlarm.BackColor = Color.Green;
                    else
                        LblAlarm.BackColor = Color.Gainsboro;

                    if ((G.Comm.AlarmTextDisplay.GetFirstAlarmType() != _LastAlarmType || G.Comm.AlarmTextDisplay.GetFirstAlarmText() != _LastAlarmText))
                    {
                        if (G.Comm.AlarmTextDisplay.GetFirstAlarmText() == string.Empty)
                        {
                            _LastAlarmType = AlarmType.None;
                            _LastAlarmText = string.Empty;
                        }
                        else
                        {
                            _LastAlarmType = G.Comm.AlarmTextDisplay.GetFirstAlarmType();
                            _LastAlarmText = G.Comm.AlarmTextDisplay.GetFirstAlarmText();
                            string time = "[" + String.Format("{0:yyyy/MM/dd HH:mm:ss:fff}", DateTime.Now) + "] ";
                            RTBoxAlarmHistory.AppendText(time + G.Comm.AlarmTextDisplay.GetFirstAlarmText() + Environment.NewLine);
                            RTBoxAlarmHistory.ScrollToCaret();
                        }
                    }

                    _AlarmDisplayStep++;
                    _AlarmDisplayTickCount = Environment.TickCount;
                    break;

                case 1:
                    if (Environment.TickCount - _AlarmDisplayTickCount > 500)
                    {
                        _AlarmDisplayTickCount = Environment.TickCount;
                        LblAlarm.BackColor = Color.Gainsboro;
                        LblAlarm.Text = G.Comm.AlarmTextDisplay.GetFirstAlarmText();
                        _AlarmDisplayStep++;
                    }
                    break;

                case 2:
                    if (Environment.TickCount - _AlarmDisplayTickCount > 500)
                    {
                        LblAlarm.Text = G.Comm.AlarmTextDisplay.GetFirstAlarmText();
                        _AlarmDisplayStep = 0;
                    }
                    break;
            }
        }

        #region 配方選項按鈕
        public void UpdateRecipeSelect()
        {
            SubPanel_RecipeSelect.Controls.Clear();
            _RadioButtonRecipes = new RadioButton[G.FS.RecipeCollection.GetRecipeNum()];
            for (int i = 0; i < G.FS.RecipeCollection.GetRecipeNum(); i++)
            {
                _RadioButtonRecipes[i] = new RadioButton();
                _RadioButtonRecipes[i].Font = new Font("Microsoft JhengHei UI", 14.25F, FontStyle.Bold);
                _RadioButtonRecipes[i].AutoSize = true;
                _RadioButtonRecipes[i].Text = G.FS.RecipeCollection.GetRecipeNames()[i];
                if (i == 0)
                    _RadioButtonRecipes[i].Location = new Point(5, i * _RadioButtonRecipes[i].Height);
                else
                    _RadioButtonRecipes[i].Location = new Point(5, _RadioButtonRecipes[i - 1].Location.Y + _RadioButtonRecipes[i - 1].Height);

                _RadioButtonRecipes[i].MouseDown += new MouseEventHandler(RadioButtonClick);

                SubPanel_RecipeSelect.Controls.Add(_RadioButtonRecipes[i]);
                if (_RadioButtonRecipes[i].Text == G.FS.RecipeCollection.GetCurrentRecipeName())
                    _RadioButtonRecipes[i].Checked = true;
            }
        }
        private void RadioButtonClick(object sender, EventArgs e)
        {
            int lastIndex = -1;
            for (int i = 0; i < _RadioButtonRecipes.Length; i++)
                if (_RadioButtonRecipes[i].Checked) lastIndex = i;

            if (MessageBox.Show("EN:Sure to change recipe ?\n" + "CH:是否確認更換配方 ?", "RECIPE CHANGE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for (int i = 0; i < _RadioButtonRecipes.Length; i++)
                {
                    if ((RadioButton)sender == _RadioButtonRecipes[i])
                    {
                        string error = string.Empty;
                        if (!G.FS.RecipeCollection.Load(_RadioButtonRecipes[i].Text, ref error))
                        {
                            MessageBox.Show(error);
                            return;
                        }

                        if (MessageBox.Show("EN:Recipe changed successfully.\n" + "CH:更換配方成功", "RECIPE CHANGE", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                            SelectUserPageHome();
                    }
                }
                ((RadioButton)sender).Checked = true;
            }
            else
            {
                if (lastIndex == -1)
                    return;
                ((RadioButton)sender).Checked = false;
                _RadioButtonRecipes[lastIndex].Checked = true;
            }
        }
        #endregion

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            NowELoginLevel = G.Comm.UserLv;
            #region 確認是否登出並顯示 FmPicDisplay
            if (PreELoginLevel != NowELoginLevel)
            {
                PreELoginLevel = NowELoginLevel;
                if (NowELoginLevel == ELoginLevel.Operator)
                {
                    PanelShow(_FmPicDisplay, false);
                    SelectUserPageHome();
                }
            }
            #endregion

            #region 日期時間與使用者
            if (G.Comm.UserLvChange())
                RenewUserValue();
            LblDateValue.Text = DateTime.Now.ToString("yyyy/MM/dd");
            LblTimeValue.Text = DateTime.Now.ToString("HH:mm:ss");
            #endregion

            LblMachineStatusValue.Text = G.Seq.MainSequence.GetStatus().ToString();

            #region 部件與運轉狀態
            if (G.Seq.MainSequence.GetStatus() == ERunStatus.Auto)
                LblMachineStatusValue.BackColor = Color.Lime;
            else
                LblMachineStatusValue.BackColor = Color.Orange;

            if (G.Comm.ComponentReady() && G.Vision.ComponentsReady)
            {
                LblPartsStatusValue.Text = "Ready";
                LblPartsStatusValue.BackColor = Color.Lime;
            }
            else
            {
                LblPartsStatusValue.Text = "Fail";
                LblPartsStatusValue.BackColor = Color.Orange;
            }
            #endregion

            #region 產能
            LblLotNum.Text = G.Comm.ProductionQ.GetQP(EQuantityProduced.Current).ToString();
            LblTotalNum.Text = G.Comm.ProductionQ.GetQP(EQuantityProduced.Total).ToString();
            #endregion

            RenewExtConnectDGV();
            RenewMonitorDGV();

            #region Recipe
            if (_RadioButtonRecipes == null ||
                _RadioButtonRecipes.Length != G.FS.RecipeCollection.GetRecipeNum() ||
                LblRecipe.Text != G.FS.RecipeCollection.GetCurrentRecipeName())
                UpdateRecipeSelect();
            LblRecipe.Text = G.FS.RecipeCollection.GetCurrentRecipeName();
            #endregion

            #region 板材數據
            double h = G.FS.RecipeCollection.GetRecipeValue(ERecipeDouble.BoardHeight);
            double w = G.FS.RecipeCollection.GetRecipeValue(ERecipeDouble.BoardWidth);
            double dis = G.FS.RecipeCollection.GetRecipeValue(ERecipeDouble.BoardHoleDistance_UnL);
            LblPanelSizeValue.Text = h.ToString() + " X " + w.ToString() + " mm";
            #endregion

            #region 板數
            double PanelNum = G.FS.RecipeCollection.GetRecipeValue(ERecipeInt.PCBNumBySet_UnL);

            if ((double)NumUD_PanelNum.Value != PanelNum)
                NumUD_PanelNum.BackColor = Color.LightPink;
            else
                NumUD_PanelNum.BackColor = SystemColors.Window;
            #endregion

            G.Comm.Login.SignOut();

            GetSequenceStatus();

            AlarmDisplaySequence();
        }

        private void RenewUserValue()
        {
            LblUserValue.Text = G.Comm.LanSwitch.GetLoginName(G.UI.frmLogin, G.Comm.UserLv);
        }

        private void BtnMainFm_Click(object sender, EventArgs e)
        {
            if (G.Comm != null)
            {
                if (G.Comm.Log != null)
                {
                    LogDef.Add(
                        ELogFileName.Operate,
                        this.GetType().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ((Button)sender).Name.ToString() + " Click");
                }
            }
        }

        private void SetBtnClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(BtnMainFm_Click);
            }
        }

        private void BtnEmgStop_Click(object sender, EventArgs e)
        {
            G.Seq.MainSequence.UserSetStatus(ERunStatus.Stop);
        }

        private void BtnKeyboard_Click(object sender, EventArgs e)
        {
            G.Comm.Keyboard.Call();
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            PanelShow(G.UI.frmLogData, false);
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            if (G.Seq.MainSequence.GetStatus() == ERunStatus.Stop)
                G.Seq.MainSequence.UserSetStatus(ERunStatus.Manual);
        }

        private void SelectUserPageHome()
        {
            SelectTabPage(5);
        }
        private void GoPreUserPage(object sender, EventArgs e)
        {
            int LastPageIndex = TabCtl.TabPages.Count - 1;

            if (TabCtl.SelectedIndex > 5 && TabCtl.SelectedIndex < LastPageIndex + 1)
                SelectTabPage(TabCtl.SelectedIndex - 1);
        }
        private void GoUserPage1(object sender, EventArgs e)
        {
            SelectUserPageHome();
        }
        private void GoNextUserPage(object sender, EventArgs e)
        {
            int LastPageIndex = TabCtl.TabPages.Count - 1;

            if (TabCtl.SelectedIndex > 4 && TabCtl.SelectedIndex < LastPageIndex)
                SelectTabPage(TabCtl.SelectedIndex + 1);
        }

        private void ShowNumKeyboard(object sender, EventArgs e)
        {
            FmNumKeyboard k = new FmNumKeyboard((NumericUpDown)sender);
            k.StartPosition = FormStartPosition.CenterScreen;
            k.ShowDialog();
        }

        private void NumUD_PanelNum_Click(object sender, EventArgs e)
        {
            FmNumKeyboard k = new FmNumKeyboard((NumericUpDown)sender);
            k.StartPosition = FormStartPosition.CenterScreen;
            k.ShowDialog();
        }

        private void BtnResetLotNum_Click(object sender, EventArgs e)
        {
            G.Comm.ProductionQ.ResetQP(EQuantityProduced.Current);
        }

        private void BtnResetTotalNum_Click(object sender, EventArgs e)
        {
            G.Comm.ProductionQ.ResetQP(EQuantityProduced.Total);
        }

        private void BtnShowSequence_S_Click(object sender, EventArgs e)
        {
            if (TabCtl.SelectedIndex != 0)
                SelectTabPage(0);
            else
                ShowPreviousTabPage();
        }

        public void ShowFuncPage()
        {
            PanelShow(_FmFuncPage, true);
        }

        private void textBoxAlarmText_Click(object sender, EventArgs e)
        {
            if (G.Seq.MainSequence.TimeoutFormIsExsit())
                PanelShow(G.Seq.MainSequence.GetTimeoutForm(), true);
        }



        private void radioButtonPalletMode_CheckedChanged(object sender, EventArgs e)
        {
            G.Comm.Mode = CommonManagerDef.LoadMode.Pallet;
        }

        private void radioButtonPanelTrayMode_CheckedChanged(object sender, EventArgs e)
        {
            G.Comm.Mode = CommonManagerDef.LoadMode.PanelTray;
        }

        private void radioButtonLRackMode_CheckedChanged(object sender, EventArgs e)
        {
            G.Comm.Mode = CommonManagerDef.LoadMode.LRack;
        }

        private void NumUD_PanelNum_ValueChanged(object sender, EventArgs e)
        {
            G.FS.RecipeCollection.SetRecipeContent(ERecipeInt.PCBNumBySet_UnL, (int)NumUD_PanelNum.Value);
        }

        public void ChangBtnVolumeImg()
        {
            if (G.Comm.VolumeMute)
                BtnMute.Image = GJSControl.Properties.Resources.mute;
            else
                BtnMute.Image = GJSControl.Properties.Resources.volume;
        }

        private void BtnMute_Click(object sender, EventArgs e)
        {
            G.Comm.VolumeMute = !G.Comm.VolumeMute;
            ChangBtnVolumeImg();
        }

        private void ExtConnectIO_Click(object sender, EventArgs e)
        {
            if (TabCtl.SelectedIndex != 4)
                SelectTabPage(4);
            else
                SelectTabPage(5);
        }

        private void buttonMonitor_Click(object sender, EventArgs e)
        {
            if (TabCtl.SelectedIndex != 1)
                SelectTabPage(1);
            else
                SelectTabPage(5);
        }

        private void BtnDl_Click(object sender, EventArgs e)
        {
            if (G.Comm.UserLv == ELoginLevel.Developer)
            {
                if (TabCtl.SelectedIndex != 3)
                    SelectTabPage(3);
                else
                    SelectTabPage(5);
            }
        }

        private void ShowRecipeSelectPage(object sender, EventArgs e)
        {
            if (TabCtl.SelectedIndex != 2)
                SelectTabPage(2);
            else
                SelectTabPage(5);
        }
    }
}
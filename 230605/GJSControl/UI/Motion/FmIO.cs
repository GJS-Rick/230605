using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using CommonLibrary;
using FileStreamLibrary;

namespace nsFmMotion
{
    public partial class frmIO : Form
    {
        private cIOAnalysisDef _IOAnalysis;
        private TabPage[] m_tabPgModuleAry;
        private cDIOUIDef[][] _DOUIAry;
        private cDIUIDef[][] _DIUIAry;
        private Panel[] m_pnlDIAry;
        private Panel[] m_pnlDOAry;
        private Label[] m_lbDOHdrAry;
        private Label[] m_lbDIHdrAry;
        private int _FixRowNum = 16;
        public frmIO()
        {
            InitializeComponent();


            if (G.Comm.MtnCtrl == null)
                return;

            _IOAnalysis = new cIOAnalysisDef(G.Comm.MtnCtrl);
            m_pnlDIAry = new Panel[_IOAnalysis.nGetModuleNameNum()];
            m_pnlDOAry = new Panel[_IOAnalysis.nGetModuleNameNum()];
            m_lbDOHdrAry = new Label[_IOAnalysis.nGetModuleNameNum()];
            m_lbDIHdrAry = new Label[_IOAnalysis.nGetModuleNameNum()];

            _DOUIAry = new cDIOUIDef[_IOAnalysis.nGetModuleNameNum()][];
            _DIUIAry = new cDIUIDef[_IOAnalysis.nGetModuleNameNum()][];
            m_tabPgModuleAry = new TabPage[_IOAnalysis.nGetModuleNameNum()];
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                m_pnlDIAry[i] = new Panel();
                m_pnlDOAry[i] = new Panel();
                m_lbDOHdrAry[i] = new Label();
                m_lbDIHdrAry[i] = new Label();
                m_pnlDIAry[i].BorderStyle = BorderStyle.FixedSingle;
                m_pnlDOAry[i].BorderStyle = BorderStyle.FixedSingle;

                m_tabPgModuleAry[i] = new TabPage();
                _DOUIAry[i] = new cDIOUIDef[_IOAnalysis.nGetDONum((MachineModules)i)];
                for (int j = 0; j < _IOAnalysis.nGetDONum((MachineModules)i); j++)
                    _DOUIAry[i][j] = new cDIOUIDef();

                _DIUIAry[i] = new cDIUIDef[_IOAnalysis.nGetDINum((MachineModules)i)];
                for (int j = 0; j < _IOAnalysis.nGetDINum((MachineModules)i); j++)
                    _DIUIAry[i][j] = new cDIUIDef();
            }
        }

        private void frmIO_Load(object sender, EventArgs e)
        {
            if (G.Comm.MtnCtrl == null)
                return;

            this.Font = new Font("新細明體", 9);

            vCreateModuleTab();

            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                vLayoutDO((MachineModules)i, m_tabPgModuleAry[i]);
                vLayoutDI((MachineModules)i, m_tabPgModuleAry[i]);
            }

            vRefreshDOName();
            vRefreshDIName();
        }

        private void vCreateModuleTab()
        {
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                m_tabPgModuleAry[i].Text = _IOAnalysis.sGetModuleName((MachineModules)i);
                m_tabPgModuleAry[i].VerticalScroll.Visible = false;
                m_tabPgModuleAry[i].AutoScroll = true;
                tabCtrlDIO.Controls.Add(m_tabPgModuleAry[i]);
                m_tabPgModuleAry[i].Controls.Add(m_pnlDIAry[i]);
                m_tabPgModuleAry[i].Controls.Add(m_pnlDOAry[i]);
                m_tabPgModuleAry[i].Controls.Add(m_lbDIHdrAry[i]);
                m_tabPgModuleAry[i].Controls.Add(m_lbDOHdrAry[i]);
                m_lbDIHdrAry[i].Text = "Input";
                m_lbDOHdrAry[i].Text = "Output";
                m_lbDIHdrAry[i].BringToFront();
                m_lbDOHdrAry[i].BringToFront();
                m_lbDIHdrAry[i].AutoSize = true;
                m_lbDOHdrAry[i].AutoSize = true;
            }
        }

        private void vLayoutDI(MachineModules eModule, TabPage tabPgModule)
        {

            Point ptStartPt = new Point(10, 20);
            // initialize DO Array
            int nCurClmnNo = 0;
            for (int i = 0; i < _IOAnalysis.nGetDINum(eModule); i++)
            {
                nCurClmnNo = i / _FixRowNum;
                m_pnlDIAry[(int)eModule].Controls.Add(_DIUIAry[(int)eModule][i].m_pnl);
                _DIUIAry[(int)eModule][i].m_pnl.Location = new Point(ptStartPt.X + _DIUIAry[(int)eModule][i].m_pnl.Size.Width * nCurClmnNo, ptStartPt.Y + i % _FixRowNum * _DIUIAry[(int)eModule][i].m_pnl.Size.Height);
                _DIUIAry[(int)eModule][i].chkGetPass().Click += new EventHandler(vDIPass_Click);
            }

            int nDICulomnNum = _IOAnalysis.nGetDINum(eModule) / _FixRowNum;
            if ((_IOAnalysis.nGetDINum(eModule) % _FixRowNum) > 0)
                nDICulomnNum += 1;

            cDIUIDef cDIUI = new cDIUIDef();
            m_pnlDIAry[(int)eModule].Location = new Point(5 + m_pnlDOAry[(int)eModule].Size.Width + m_pnlDOAry[(int)eModule].Location.X, m_pnlDOAry[(int)eModule].Location.Y);
            m_pnlDIAry[(int)eModule].ClientSize = new Size(ptStartPt.X + (nDICulomnNum) * cDIUI.m_pnl.Size.Width + 10, ptStartPt.Y * 2 + cDIUI.m_pnl.Size.Height * _FixRowNum);
            m_lbDIHdrAry[(int)eModule].Location = m_pnlDIAry[(int)eModule].Location;
        }

        private void vLayoutDO(MachineModules eModule, TabPage tabPgModule)
        {
            Point ptStartPt = new Point(10, 20);
            // initialize DO Array
            int nCurClmnNo = 0;
            for (int i = 0; i < _IOAnalysis.nGetDONum(eModule); i++)
            {
                nCurClmnNo = i / _FixRowNum;
                _DOUIAry[(int)eModule][i] = new cDIOUIDef();
                m_pnlDOAry[(int)eModule].Controls.Add(_DOUIAry[(int)eModule][i].m_pnl);
                _DOUIAry[(int)eModule][i].m_pnl.Location = new Point(ptStartPt.X + _DOUIAry[(int)eModule][i].m_pnl.Size.Width * nCurClmnNo, ptStartPt.Y + i % _FixRowNum * _DOUIAry[(int)eModule][i].m_pnl.Size.Height);
                Label OvlShp = _DOUIAry[(int)eModule][i].OvlShpGetShp();
                OvlShp.Click += new EventHandler(vDO_Click);
                _DOUIAry[(int)eModule][i].vSetShp(OvlShp);
                //_DOUIAry[(int)eModule][i].chkGetPass().Click += new EventHandler(vDOPass_Click);
            }

            int nDOColumnNum = _IOAnalysis.nGetDONum(eModule) / _FixRowNum;
            if ((_IOAnalysis.nGetDONum(eModule) % _FixRowNum) > 0)
                nDOColumnNum += 1;

            cDIOUIDef cDIOUI = new cDIOUIDef();
            m_pnlDOAry[(int)eModule].Location = new Point(5, 5);
            m_pnlDOAry[(int)eModule].ClientSize = new Size(ptStartPt.X + (nDOColumnNum) * cDIOUI.m_pnl.Size.Width + 10, ptStartPt.Y * 2 + cDIOUI.m_pnl.Size.Height * _FixRowNum);
            m_lbDOHdrAry[(int)eModule].Location = m_pnlDOAry[(int)eModule].Location;
        }

        private void vRefreshDOName()
        {
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                for (int j = 0; j < _IOAnalysis.nGetDONum((MachineModules)i); j++)
                {
                    Label lblName = _DOUIAry[i][j].lblGetName();
                    lblName.Text = G.Comm.IOCtrl.GetDOName((EDO_TYPE)_IOAnalysis.nGetDOIdx((MachineModules)i, j));

                    int nModID = 0;
                    int nModNo = 0;

                    G.Comm.IOCtrl.GetDOModId_ModNo((EDO_TYPE)_IOAnalysis.nGetDOIdx((MachineModules)i, j), ref nModID, ref nModNo);
                    _DOUIAry[i][j].lblGetModID().Text = nModID.ToString();
                    _DOUIAry[i][j].lblGetModNo().Text = nModNo.ToString();

                    Label lblOrderNo = _DOUIAry[i][j].lblGetOrderNo();
                    lblOrderNo.Text = _IOAnalysis.nGetDOIdx((MachineModules)i, j).ToString();
                }
            }
        }

        private void vRefreshDIName()
        {
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                for (int j = 0; j < _IOAnalysis.nGetDINum((MachineModules)i); j++)
                {
                    Label lblName = _DIUIAry[i][j].lblGetName();
                    lblName.Text = G.Comm.IOCtrl.GetDIName((EDI_TYPE)_IOAnalysis.nGetDIIdx((MachineModules)i, j));
                    _DIUIAry[i][j].vSetName(lblName);

                    int nModID = 0;
                    int nModNo = 0;

                    G.Comm.IOCtrl.GetDIModId_ModNo((EDI_TYPE)_IOAnalysis.nGetDIIdx((MachineModules)i, j), ref nModID, ref nModNo);
                    _DIUIAry[i][j].lblGetModID().Text = nModID.ToString();
                    _DIUIAry[i][j].lblGetModNo().Text = nModNo.ToString();

                    Label lblOrderNo = _DIUIAry[i][j].lblGetOrderNo();
                    lblOrderNo.Text = _IOAnalysis.nGetDIIdx((MachineModules)i, j).ToString();
                }
            }
        }

        private void vDOPass_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                for (int j = 0; j < _IOAnalysis.nGetDONum((MachineModules)i); j++)
                {
                    if (sender == _DOUIAry[i][j].chkGetPass())
                    {
                        G.Comm.IOCtrl.SetDOPass((EDO_TYPE)_IOAnalysis.nGetDOIdx((MachineModules)i, j), ((CheckBox)sender).Checked);
                        break;
                    }
                }
            }
        }

        private void vDIPass_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                for (int j = 0; j < _IOAnalysis.nGetDINum((MachineModules)i); j++)
                {
                    if (sender == _DIUIAry[i][j].chkGetPass())
                    {
                        G.Comm.IOCtrl.SetDIPass((EDI_TYPE)_IOAnalysis.nGetDIIdx((MachineModules)i, j), ((CheckBox)sender).Checked);
                        break;
                    }
                }
            }
        }

        private void vDO_Click(object sender, EventArgs e)
        {
            if (G.Comm.Login.GetERunStatus() == ERunStatus.Auto)//自動中不能手動切換DO************
                return;

            EDO_TYPE eDO;
            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                for (int j = 0; j < _IOAnalysis.nGetDONum((MachineModules)i); j++)
                {
                    Label OvlShp = _DOUIAry[i][j].OvlShpGetShp();
                    if ((Label)sender == OvlShp)
                    {
                        eDO = (EDO_TYPE)_IOAnalysis.nGetDOIdx((MachineModules)i, j);
                        MotionPrecaution(eDO);//保護**************

                        bool bVal = false;

                        // 設定DO
                        if (OvlShp.BackColor == Color.DarkSeaGreen)
                            bVal = true;

                        G.Comm.IOCtrl.SetDO(eDO, bVal);

                        // 讀取DO
                        Thread.Sleep(50);
                        bool actualDO = G.Comm.IOCtrl.GetDO(eDO, bVal);
                        //if (bVal)
                        if (actualDO)//*********
                            OvlShp.BackColor = Color.Lime;
                        else
                            OvlShp.BackColor = Color.DarkSeaGreen;

                        _DOUIAry[i][j].vSetShp(OvlShp);
                        break;
                    }
                }
            }
        }

        private void frmIO_Shown(object sender, EventArgs e)
        {
            tmrDIUpdate.Enabled = true;
        }

        private void tmrDIUpdate_Tick(object sender, EventArgs e)
        {
            if (G.Comm.MtnCtrl != null)
            {
                for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
                {
                    for (int j = 0; j < _IOAnalysis.nGetDINum((MachineModules)i); j++)
                    {
                        Label OvlShp = _DIUIAry[i][j].OvlShpGetShp();
                        if (G.Comm.IOCtrl.GetDI(((EDI_TYPE)_IOAnalysis.nGetDIIdx((MachineModules)i, j)), false, true))
                            OvlShp.BackColor = Color.Lime;
                        else
                            OvlShp.BackColor = Color.DarkSeaGreen;
                        _DIUIAry[i][j].vSetShp(OvlShp);

                        _DIUIAry[i][j].chkGetPass().Checked = G.Comm.IOCtrl.GetDIPass((EDI_TYPE)_IOAnalysis.nGetDIIdx((MachineModules)i, j));
                    }

                    for (int j = 0; j < _IOAnalysis.nGetDONum((MachineModules)i); j++)
                    {
                        if (G.Comm.IOCtrl.GetDO((EDO_TYPE)_IOAnalysis.nGetDOIdx((MachineModules)i, j), false))
                            _DOUIAry[i][j].OvlShpGetShp().BackColor = Color.Lime;
                        else
                            _DOUIAry[i][j].OvlShpGetShp().BackColor = Color.DarkSeaGreen;

                        if (G.Comm.UserLv == ELoginLevel.Developer)//不給開發者以外使用*************
                            _DOUIAry[i][j].chkGetPass().Visible = true;
                        else
                            _DOUIAry[i][j].chkGetPass().Visible = false;

                    }
                }
            }
        }

        private void frmIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void frmIO_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == false)
                tmrDIUpdate.Enabled = false;
            else
                tmrDIUpdate.Enabled = true;

            for (int i = 0; i < _IOAnalysis.nGetModuleNameNum(); i++)
            {
                for (int j = 0; j < _IOAnalysis.nGetDONum((MachineModules)i); j++)
                {
                    Label OvlShp = _DOUIAry[i][j].OvlShpGetShp();

                    // 讀取DO
                    if (G.Comm.IOCtrl.GetDO(((EDO_TYPE)_IOAnalysis.nGetDOIdx((MachineModules)i, j)), false))
                        OvlShp.BackColor = Color.Lime;
                    else
                        OvlShp.BackColor = Color.DarkSeaGreen;
                }
            }
        }

        private bool MotionPrecaution(EDO_TYPE eDO)
        {
            //if (G.Common.IOCtrl.GetDI(EDI_TYPE.CylinderLowerLimit_Pin_A1, true) && G.Common.IOCtrl.GetDO(EDO_TYPE.Down_Pin_A1, true))
            //{
            //    if (EDO_TYPE.PinForward_Pin_A1 == eDO
            //     || EDO_TYPE.PinBackward_Pin_A1 == eDO)
            //    {
            //        AlarmTextDisplay.Add(
            //          AlarmCode.Warning,
            //          AlarmType.Warning,
            //          "A1側打PIN氣缸上升後才可移動A1側上PIN入PIN汽缸");
            //        return true;
            //    }
            //}
            //if (G.Common.IOCtrl.GetDI(EDI_TYPE.CylinderLowerLimit_Pin_A2, true) && G.Common.IOCtrl.GetDO(EDO_TYPE.Down_Pin_A2, true))
            //{
            //    if (EDO_TYPE.PinForward_Pin_A2 == eDO
            //     || EDO_TYPE.PinBackward_Pin_A2 == eDO)
            //    {
            //        AlarmTextDisplay.Add(
            //          AlarmCode.Warning,
            //          AlarmType.Warning,
            //          "A2側打PIN氣缸上升後才可移動A2側上PIN入PIN汽缸");
            //        return true;
            //    }
            //}

            //if (eDO == EDO_TYPE.Down_Pin_A1)
            //{
            //    if (G.Common.MtnCtrl.GetPos(EAXIS_NAME.Drill_A1, 0.1) > 1)
            //    {
            //        AlarmTextDisplay.Add(
            //            AlarmCode.Warning,
            //            AlarmType.Warning,
            //            "請將A1側鑽頭退至底部才可下降打PIN氣缸");
            //        return true;
            //    }
            //}
            //if (eDO == EDO_TYPE.Down_Pin_A2)
            //{
            //    if (G.Common.MtnCtrl.GetPos(EAXIS_NAME.Drill_A2, 0.1) > 1)
            //    {
            //        AlarmTextDisplay.Add(
            //            AlarmCode.Warning,
            //            AlarmType.Warning,
            //            "請將A2側鑽頭退至底部才可下降打PIN氣缸");
            //        return true;
            //    }
            //}

            return false;
        }
    }

    public class cDIOUIDef
    {
        public Panel m_pnl;

        private Label m_OvlShp;
        private Label m_lblName;
        private Label m_lblModNo;
        private Label m_lblOrderNo;
        private Label m_lblModID;
        private CheckBox _DOPass;

        public cDIOUIDef()
        {
            m_pnl = new Panel();
            m_lblName = new Label();
            m_lblModID = new Label();
            m_lblModNo = new Label();
            m_lblOrderNo = new Label();
            _DOPass = new CheckBox();

            m_OvlShp = new Label();

            m_pnl.SuspendLayout();

            m_pnl.Controls.Add(m_lblName);
            m_pnl.Controls.Add(m_lblModID);
            m_pnl.Controls.Add(m_lblModNo);
            m_pnl.Controls.Add(m_lblOrderNo);
            m_pnl.Controls.Add(m_OvlShp);
            m_pnl.Controls.Add(_DOPass);

            int nShpW = 20;
            int nShpH = 20;
            int nGap = 4;
            int nBond = 4;
            Point ptStaratPt = new Point(nBond, nBond);
            m_OvlShp.Size = new Size(nShpW, nShpH);
            m_OvlShp.Location = ptStaratPt;
            m_OvlShp.FlatStyle = FlatStyle.Popup;
            m_OvlShp.BackColor = Color.DarkSeaGreen;

            m_lblName.Location = new Point(ptStaratPt.X + m_OvlShp.Width + nGap, ptStaratPt.Y);
            m_lblName.Size = new Size(160, nShpH);
            m_lblName.TextAlign = ContentAlignment.MiddleLeft;

            m_lblModID.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + nGap, ptStaratPt.Y);
            m_lblModID.Size = new Size(20, nShpH);
            m_lblModID.BackColor = Color.Silver;
            m_lblModID.TextAlign = ContentAlignment.MiddleLeft;

            m_lblModNo.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + +m_lblModID.Width + nGap, ptStaratPt.Y);
            m_lblModNo.Size = new Size(20, nShpH);
            m_lblModNo.BackColor = Color.Silver;
            m_lblModNo.TextAlign = ContentAlignment.MiddleLeft;

            m_lblOrderNo.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + m_lblModID.Width + m_lblModNo.Width + nGap, ptStaratPt.Y);
            m_lblOrderNo.Size = new Size(25, nShpH);
            m_lblOrderNo.BackColor = Color.Gainsboro;
            m_lblOrderNo.TextAlign = ContentAlignment.MiddleLeft;

            _DOPass.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + m_lblModID.Width + m_lblModNo.Width + m_lblOrderNo.Width + nGap, ptStaratPt.Y);
            _DOPass.Size = new Size(20, nShpH);
            _DOPass.BackColor = Color.Gainsboro;
            _DOPass.Checked = false;
            _DOPass.Text = String.Empty;

            m_pnl.Size = new Size(nBond * 2 + m_OvlShp.Width + m_lblName.Width + nGap + m_lblModID.Width + m_lblModNo.Width + m_lblOrderNo.Width + _DOPass.Width, nShpH + nBond * 2);
            //    _hpCtnr.ResumeLayout();
            m_pnl.ResumeLayout();
        }

        public void vDestroy()
        {
            m_pnl = null;
            m_lblName = null;
            m_lblModID = null;
            m_lblModNo = null;
            m_lblOrderNo = null;

            m_OvlShp = null;

            _DOPass = null;
        }

        ~cDIOUIDef() { }

        public CheckBox chkGetPass() { return _DOPass; }
        public Label lblGetModID() { return m_lblModID; }

        public Label lblGetModNo() { return m_lblModNo; }

        public void vSetModID(Label lblVal) { m_lblModID = lblVal; }

        public Label OvlShpGetShp() { return m_OvlShp; }

        public void vSetShp(Label OvlShp) { m_OvlShp = OvlShp; }

        public Label lblGetName() { return m_lblName; }

        public void vSetName(Label lblName) { m_lblName = lblName; }

        public Label lblGetOrderNo() { return m_lblOrderNo; }
    }

    public class cDIUIDef
    {
        public Panel m_pnl;
        private Label m_OvlShp;
        private Label m_lblName;
        private Label m_lblModNo;
        private Label m_lblOrderNo;
        private Label m_lblModID;
        private CheckBox _hkPass;

        public cDIUIDef()
        {
            m_pnl = new Panel();
            m_lblName = new Label();
            m_lblModID = new Label();
            m_lblModNo = new Label();
            m_lblOrderNo = new Label();
            _hkPass = new CheckBox();

            m_OvlShp = new Label();

            m_pnl.SuspendLayout();

            m_pnl.Controls.Add(m_lblName);
            m_pnl.Controls.Add(m_lblModID);
            m_pnl.Controls.Add(m_lblModNo);
            m_pnl.Controls.Add(m_lblOrderNo);
            m_pnl.Controls.Add(m_OvlShp);
            m_pnl.Controls.Add(_hkPass);

            int nShpW = 20;
            int nShpH = 20;
            int nGap = 4;
            int nBond = 4;
            Point ptStaratPt = new Point(nBond, nBond);
            m_OvlShp.Size = new Size(nShpW, nShpH);
            m_OvlShp.Location = ptStaratPt;
            m_OvlShp.FlatStyle = FlatStyle.Popup;
            m_OvlShp.BackColor = Color.DarkSeaGreen;

            m_lblName.Location = new Point(ptStaratPt.X + m_OvlShp.Width + nGap, ptStaratPt.Y);
            m_lblName.Size = new Size(160, nShpH);
            m_lblName.TextAlign = ContentAlignment.MiddleLeft;

            m_lblModID.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + nGap, ptStaratPt.Y);
            m_lblModID.Size = new Size(20, nShpH);
            m_lblModID.BackColor = Color.Silver;
            m_lblModID.TextAlign = ContentAlignment.MiddleLeft;

            m_lblModNo.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + +m_lblModID.Width + nGap, ptStaratPt.Y);
            m_lblModNo.Size = new Size(20, nShpH);
            m_lblModNo.BackColor = Color.Silver;
            m_lblModNo.TextAlign = ContentAlignment.MiddleLeft;

            m_lblOrderNo.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + m_lblModID.Width + m_lblModNo.Width + nGap, ptStaratPt.Y);
            m_lblOrderNo.Size = new Size(25, nShpH);
            m_lblOrderNo.BackColor = Color.Gainsboro;
            m_lblOrderNo.TextAlign = ContentAlignment.MiddleLeft;

            _hkPass.Location = new Point(ptStaratPt.X + m_OvlShp.Width + m_lblName.Width + m_lblModID.Width + m_lblModNo.Width + m_lblOrderNo.Width + nGap, ptStaratPt.Y);
            _hkPass.Size = new Size(20, nShpH);
            _hkPass.BackColor = Color.Gainsboro;
            _hkPass.Checked = false;
            _hkPass.Text = String.Empty;

            m_pnl.Size = new Size(nBond * 2 + m_OvlShp.Width + m_lblName.Width + nGap + m_lblModID.Width + m_lblModNo.Width + m_lblOrderNo.Width + _hkPass.Width, nShpH + nBond * 2);

            m_pnl.ResumeLayout();
        }

        public void vDestroy()
        {
            m_pnl = null;
            m_lblName = null;
            m_lblModID = null;
            m_lblModNo = null;
            m_lblOrderNo = null;

            m_OvlShp = null;

            _hkPass = null;
        }

        ~cDIUIDef() { }

        public CheckBox chkGetPass() { return _hkPass; }

        public Label lblGetModID() { return m_lblModID; }

        public Label lblGetModNo() { return m_lblModNo; }

        public void vSetModID(Label lblVal) { m_lblModID = lblVal; }

        public Label OvlShpGetShp() { return m_OvlShp; }

        public void vSetShp(Label OvlShp) { m_OvlShp = OvlShp; }

        public Label lblGetName() { return m_lblName; }

        public void vSetName(Label lblName) { m_lblName = lblName; }

        public Label lblGetOrderNo() { return m_lblOrderNo; }
    }
}
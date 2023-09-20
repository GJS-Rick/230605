using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibrary;
using FileStreamLibrary;
using nsSequence;
using VisionLibrary;

namespace nsUI
{
    public partial class FmFunctionPage : Form
    {
        private FmMain _FmMain;

        public FmFunctionPage()
        {
            InitializeComponent();
        }

        private void FmNextPage_Load(object sender, EventArgs e)
        {
            this.Size = new Size(635, 456);
            _FmMain = new FmMain();
        }

        public void UIBtnSort()
        {
            BtnPositionSizeSetting(4, 4, 8, 8, 5, BtnSort(FindButton()), 119, 106);
        }

        private void BtnPositionSizeSetting(int StartX, int StartY, int DisX, int DisY, int BtnNum, Button[] _fmMainBtn, int W, int H)
        {
            int _offset = 0;
            for (int i = 0; i < _fmMainBtn.Length; i++)
            {
                if (_fmMainBtn[i] == null)
                    return;

                _fmMainBtn[i].Width = W;
                _fmMainBtn[i].Height = H;

                if (!_fmMainBtn[i].Visible)
                    _offset = _offset - 1;

                _fmMainBtn[i].Left = StartX + ((i + _offset) % BtnNum) * (_fmMainBtn[i].Width + DisX);
                _fmMainBtn[i].Top = StartY + ((i + _offset) / BtnNum) * (_fmMainBtn[i].Height + DisY);
            }
        }

        private List<Button> FindButton()
        {
            List<Button> BtnList = new List<Button>();
            foreach (Control ctl in this.Controls)
            {
                if (ctl is Button)//找按鈕
                    if (ctl.Visible)//非隱藏按鈕才作排列
                        BtnList.Add((Button)ctl);
            }
            return BtnList;
        }

        private Button[] BtnSort(List<Button> BtnList)//依TabIndex由小到大排列
        {
            Button BtnBuf;
            Button[] BtnArr = BtnList.ToArray();
            for (int i = 0; i < (BtnArr.Length - 1); i++)
            {
                for (int j = i + 1; j < BtnArr.Length; j++)
                {
                    if (BtnArr[i].TabIndex >= BtnArr[j].TabIndex)
                    {
                        BtnBuf = BtnArr[i];
                        BtnArr[i] = BtnArr[j];
                        BtnArr[j] = BtnBuf;
                    }
                }
            }
            return BtnArr;
        }

        private void BtnVision_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmAreaCCD, true); }));
        }

        private void BtnMaintenance_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmMaintain, false); }));
        }

        private void BtnSCARAMove_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.fmSCARAMov, false); }));
            G.UI.fmSCARAMov.Visible = true;
            G.UI.fmSCARAMov.BringToFront();
        }

        private void BtnObjectRemoval_Click(object sender, EventArgs e)
        {
            //真空OFF
            G.Comm.Vac.Off(EVacSuckerName.SCARA);
            //氣缸1
            G.Comm.CYL.Extension(ECylName.Cyl_1);
        }

        private void BtnGoStandby_Click(object sender, EventArgs e)
        {
            if (G.Seq.MainSequence.GetStatus() == ERunStatus.Stop)
                G.Seq.MainSequence.UserSetStatus(ERunStatus.GoStandby);
        }

        private void BtnRobotAdjustment_Click(object sender, EventArgs e)
        {
            if (G.Seq.MainSequence.GetStatus() == ERunStatus.Stop)
            {
                //_Vision.SetAlignPos(
                //    EAlignIndex.Align1,
                //    new System.Drawing.PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterY]),
                //    new System.Drawing.PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterY]),
                //    new System.Drawing.PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterY]),
                //    new System.Drawing.PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.RobotCenterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.RobotCenterY]),
                //    (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelXLength],
                //    (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelYLength]);
                
            }
        }

        private void BtnSCARAAdjustment_Click(object sender, EventArgs e)
        {

        }

        private void BtnMotorMove_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmMotionFm, true); }));
        }

        private void BtnIO_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmIOFm, false); }));
        }

        private void BtnMotorPoint_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmMotorPosData, true); }));
        }

        private void BtnMachineParameters_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmMachineData, true); }));
        }

        private void BtnTeach_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("EN: Sure to Teach?\nCH: 是否進行教導 ?", "TEACH", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            G.Seq.MainSequence.UserSetStatus(ERunStatus.Teach);
        }

        private void BtnModSet_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmModSet, false); }));
        }

        private void BtnManual_Click(object sender, EventArgs e)
        {
            //G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmModSetFm, true); }));
        }

        private void BtnRobotPoint_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.fmRobot, true); }));
        }

        private void BtnGoHome_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("EN: Sure to initial?\nCH: 是否要復歸 ?", "INITIAL", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            
            G.Seq.MainSequence.UserSetStatus(ERunStatus.Initial);
        }

        private void BtnLanguageSwitch_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.ChangeLanguage(); }));
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.ReLoadParam(); }));
        }

        private void BtnSCARAPoint_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.fmSCARAPos, true); }));
        }

        private void BtnRecipe_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmRecipe, true); }));
        }

        private void BtnMonitorSet_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmMonitorSet, false); }));
        }

        private void BtnTBotMove_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.FrmTBot, true); }));
        }

        private void BtnTBotPoint_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.FrmTBotMotorPos, true); }));
        }

        private void BtnDetectSet_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmDetectSet, false); }));
        }

        private void BtnDataTransfer_Click(object sender, EventArgs e)
        {
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.PanelShow(G.UI.frmDataTransfer, false); }));
        }
    }
}
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
        private FmMain _fmMain;
        private SequenceManagerDef _sequenceMngr;
        private CommonManagerDef _comm;
        private VisionManagerDef _visionMngr;
        private FileManagerDef _fileMngr;
        private cUIManagerDef _ui;

        public FmFunctionPage(CommonManagerDef cMngr, cUIManagerDef cUIMngr, SequenceManagerDef SequenceMngr, VisionManagerDef VisionMngr, FileManagerDef FileMngr)
        {
            InitializeComponent();
            _comm = cMngr;
            _ui = cUIMngr;
            _sequenceMngr = SequenceMngr;
            _visionMngr = VisionMngr;
            _fileMngr = FileMngr;
        }

        private void FmNextPage_Load(object sender, EventArgs e)
        {
            _fmMain = new FmMain();
            #region 隱藏按鈕
            BtnRobotMove.Visible=false;
            BtnRecipe.Visible = true;
            #endregion

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
            _ui.frmAreaCCD.Visible = true;
            _ui.frmAreaCCD.BringToFront();
        }

        private void BtnMaintenance_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.frmMaintain); }));
            //_fmMain.cUIMngr.frmMaintain.Visible = true;
            //_ui.frmMaintain.Visible = true;
        }

        private void BtnRobotMove_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.fmSCARAMov); }));
            //_ui.fmScaraMove.Visible = true;
            //_ui.fmScaraMove.BringToFront();
        }

        private void BtnObjectRemoval_Click(object sender, EventArgs e)
        {
            //手臂真空OFF
            _comm.MtnCtrl.SetDO(EDO_TYPE.ArmVacuum_Break, false);
            _comm.MtnCtrl.SetDO(EDO_TYPE.ArmVacuum_Off, true);
            _comm.MtnCtrl.SetDO(EDO_TYPE.ArmVacuum_On, false);
            //Mylar吸盤真空OFF
            _comm.MtnCtrl.SetDO(EDO_TYPE.MylarSuckerVacuum_Break, false);
            _comm.MtnCtrl.SetDO(EDO_TYPE.MylarSuckerVacuum_Off, true);
            _comm.MtnCtrl.SetDO(EDO_TYPE.MylarSuckerVacuum_On, false);
        }

        private void BtnGoStandby_Click(object sender, EventArgs e)
        {
            if (_sequenceMngr.MainSequence.GetStatus() == ERunStatus.Stop)
                _sequenceMngr.MainSequence.UserSetStatus(ERunStatus.GoStandby);
        }

        private void BtnAdjustment_Click(object sender, EventArgs e)
        {
            if (_sequenceMngr.MainSequence.GetStatus() == ERunStatus.Stop)
            {
                _visionMngr.SetAlignPos(
                    new System.Drawing.PointF((float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterX], (float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterY]),
                    new System.Drawing.PointF((float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterX], (float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterY]),
                    new System.Drawing.PointF((float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterX], (float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterY]),
                    (float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.PanelWidth],
                    (float)_fileMngr.MachineData.ValueDouble[(int)EMachineDouble.PanelHeight]);

                _visionMngr.AlignAlgorithm.vSetCenterPos(
                    _fileMngr.MachineData.ValueDouble[(int)EMachineDouble.RobotCenterX],
                    _fileMngr.MachineData.ValueDouble[(int)EMachineDouble.RobotCenterY]);

                _sequenceMngr.MainSequence.ForkTeach.Start();
            }
        }

        private void BtnMotorMove_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.frmMotionFm); }));
            //_ui.frmMotionFm.Visible = true;
            //_ui.frmMotionFm.BringToFront();
        }

        private void BtnIO_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.frmIOFm); }));
            //_ui.frmIOFm.Visible = true;
            //_ui.frmIOFm.BringToFront();
        }

        private void BtnMotorPoint_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.frmMotorPosData); }));
            //_ui.frmMotorPosData.Visible = true;
            //_ui.frmMotorPosData.BringToFront();
        }

        private void BtnMachineParameters_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.frmMachineData); }));
            //_ui.frmMachineData.Visible = true;
            //_ui.frmMachineData.BringToFront();
        }

        private void BtnTeach_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否進行教導", "確認", MessageBoxButtons.OKCancel) == DialogResult.OK)
                _sequenceMngr.MainSequence.UserSetStatus(ERunStatus.Teach);
        }

        private void BtnRobotPoint_Click(object sender, EventArgs e)
        {
            _ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.fmSCARAPos); }));
            //_ui.frmMainFm.Invoke(new Action(() => { _ui.frmMainFm.PanelShow(_ui.fmRobot); }));
            //_ui.fmRobot.Visible = true;
            //_ui.fmRobot.BringToFront();
        }

        private void BtnGoHome_Click(object sender, EventArgs e)
        {
            _sequenceMngr.MainSequence.UserSetStatus(ERunStatus.Initial);
        }

        private void BtnLanguageSwitch_Click(object sender, EventArgs e)
        {
            //_comm.LanSwitch.SwitchLanguage(this);

            //if (_comm.LanSwitch.GetCurrentLanguage() == LanguageSwitch.ELanguages.Chinese_TW)
            //    buttonLanguageSwitch.Text = "English";
            //if (_comm.LanSwitch.GetCurrentLanguage() == LanguageSwitch.ELanguages.English)
            //    buttonLanguageSwitch.Text = "中文";
        }
    }
}

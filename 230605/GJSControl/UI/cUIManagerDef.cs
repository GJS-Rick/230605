using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary;
using nsSequence;
using nsFmMotion;
using System.Windows.Forms;
using GJSControl;
using nsUIMotorPosData;
using FileStreamLibrary;
using VisionLibrary;

namespace nsUI
{
    public class cUIManagerDef : IDisposable
    {
        public FmMain frmMainFm;
        public FmPicDisplay frmPicFm;
        public frmIO frmIOFm;
        public frmMotion frmMotionFm;
        public FmMachineData frmMachineData;
        public FmLogData frmLogData;
        public FmMotorPosData frmMotorPosData;
        public FmAreaCCD frmAreaCCD;
        public FmLogin frmLogin;
        public FmRecipe frmRecipe;
        public FmMaintain frmMaintain;
        public FmModSet frmModSet;
        public FmMonitorSet frmMonitorSet;
        public FmDetectSet frmDetectSet;
        public FmDataTransfer frmDataTransfer;
        #region FANUC Robot
        public FmRobot fmRobot;
        #endregion
        #region SCARA
        public FmScaraMove fmSCARAMov;
        public FmScaraPosData fmSCARAPos;
        #endregion

        #region TBOT
        public FmTBotMove FrmTBot;
        public FmTBotMotorPos FrmTBotMotorPos;
        #endregion

        public cUIManagerDef(FmMain main, FmPicDisplay frmPicDisplay)
        {
            frmRecipe = new FmRecipe();
            frmRecipe.FormClosed += new FormClosedEventHandler(frmRecipe_FormClosed);
            frmMachineData = new FmMachineData();
            frmMachineData.FormClosed += new FormClosedEventHandler(frmMachineData_FormClosed);
            frmLogData = new FmLogData();
            frmLogData.FormClosed += new FormClosedEventHandler(frmLogData_FormClosed);
            frmMotorPosData = new FmMotorPosData();
            frmMotorPosData.FormClosed += new FormClosedEventHandler(frmMotorPosData_FormClosed);
            frmIOFm = new frmIO();
            frmIOFm.FormClosed += new FormClosedEventHandler(frmIOFm_FormClosed);
            frmMotionFm = new frmMotion();
            frmMotionFm.FormClosed += new FormClosedEventHandler(frmMotionFm_FormClosed);
            frmMainFm = main;
            frmMainFm.FormClosed += new FormClosedEventHandler(frmMainFm_FormClosed);
            frmPicFm = frmPicDisplay;
            frmPicFm.FormClosed += new FormClosedEventHandler(frmPicDisplay_FormClosed);
            frmAreaCCD = new FmAreaCCD();
            frmAreaCCD.FormClosed += new FormClosedEventHandler(frmAreaCCD_FormClosed);
            frmLogin = new FmLogin();
            frmLogin.FormClosed += new FormClosedEventHandler(frmLogin_FormClosed);
            frmMaintain = new FmMaintain();
            frmMaintain.FormClosed += new FormClosedEventHandler(frmMaintain_FormClosed);
            frmModSet = new FmModSet();
            frmModSet.FormClosed += new FormClosedEventHandler(frmModSet_FormClosing);
            frmMonitorSet = new FmMonitorSet();
            frmMonitorSet.FormClosed += new FormClosedEventHandler(frmMonitorSet_FormClosing);
            frmDetectSet = new FmDetectSet();
            frmDetectSet.FormClosed += new FormClosedEventHandler(frmDetectSet_FormClosing);
            frmDataTransfer = new FmDataTransfer();
            frmDataTransfer.FormClosed += new FormClosedEventHandler(frmDataTransfer_FormClosing);
            #region FANUC Robot
            fmRobot = new FmRobot();
            fmRobot.FormClosed += new FormClosedEventHandler(fmRobot_FormClosed);
            #endregion
            #region SCARA

            if (G.Comm.Scara != null && G.Comm.Scara._Valid)
            {
                fmSCARAMov = new FmScaraMove();
                fmSCARAMov.FormClosed += new FormClosedEventHandler(fmSCARAMov_FormClosed);
                fmSCARAPos = new FmScaraPosData();
                fmSCARAPos.FormClosed += new FormClosedEventHandler(fmSCARAPos_FormClosed);
            }
            #endregion

            #region TBot

            if (G.Comm.TBot != null)
            {
                FrmTBot = new FmTBotMove();
                FrmTBot.FormClosed += new FormClosedEventHandler(FmTBotMove_FormClosed);
                FrmTBotMotorPos = new FmTBotMotorPos();
                FrmTBotMotorPos.FormClosed += new FormClosedEventHandler(FrmTBotMotorPos_FormClosed);
            }
            #endregion
        }
        public void frmMaintain_FormClosed(object sender, FormClosedEventArgs e) { frmMaintain = null; }
        public void frmModSet_FormClosing(object sender, FormClosedEventArgs e) { frmModSet = null; }
        public void frmMonitorSet_FormClosing(object sender, FormClosedEventArgs e) { frmMonitorSet = null; }
        public void frmDetectSet_FormClosing(object sender, FormClosedEventArgs e) { frmDetectSet = null; }
        public void frmDataTransfer_FormClosing(object sender, FormClosedEventArgs e) { frmDataTransfer.TimeStop(); frmDataTransfer = null; }
        public void frmRecipe_FormClosed(object sender, FormClosedEventArgs e) { frmRecipe = null; }
        public void frmLogin_FormClosed(object sender, FormClosedEventArgs e) { frmLogin = null; }
        private void frmAreaCCD_FormClosed(object sender, FormClosedEventArgs e) { frmAreaCCD = null; }
        private void frmMainFm_FormClosed(object sender, FormClosedEventArgs e) { frmMainFm = null; }
        private void frmPicDisplay_FormClosed(object sender, FormClosedEventArgs e) { frmPicFm = null; }
        private void frmIOFm_FormClosed(object sender, FormClosedEventArgs e) { frmIOFm = null; }
        private void frmMotionFm_FormClosed(object sender, FormClosedEventArgs e) { frmMotionFm = null; }
        private void frmMachineData_FormClosed(object sender, FormClosedEventArgs e) { frmMachineData = null; }
        private void frmLogData_FormClosed(object sender, FormClosedEventArgs e) { frmLogData = null; }
        private void frmMotorPosData_FormClosed(object sender, FormClosedEventArgs e) { frmMotorPosData = null; }

        #region FANUC Robot
        private void fmRobot_FormClosed(object sender, FormClosedEventArgs e)
        {
            fmRobot = null;
        }
        #endregion
        #region SCARA
        private void fmSCARAMov_FormClosed(object sender, FormClosedEventArgs e)
        {
            fmSCARAMov = null;
        }
        private void fmSCARAPos_FormClosed(object sender, FormClosedEventArgs e)
        {
            fmSCARAMov = null;
        }
        #endregion

        #region TBot
        private void FmTBotMove_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmTBot = null;
        }
        private void FrmTBotMotorPos_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrmTBotMotorPos = null;
        }
        #endregion

        public void Dispose()
        {
            if (frmMotorPosData != null)
                frmMotorPosData.Close();

            if (frmRecipe != null)
                frmRecipe.Close();

            if (frmLogData != null)
                frmLogData.Close();

            if (frmLogin != null)
                frmLogin.Close();
            if (frmAreaCCD != null)
                frmAreaCCD.Close();

            if (frmIOFm != null)
                frmIOFm.Close();

            if (frmMotionFm != null)
                frmMotionFm.Close();
            if (frmMachineData != null)
                frmMachineData.Close();

            if (fmRobot != null)
                fmRobot.Close();

            if (fmSCARAMov != null)
                fmSCARAMov.Close();

            if (fmSCARAPos != null)
                fmSCARAPos.Close();

            if (FrmTBot != null)
                FrmTBot.Close();

            if (FrmTBotMotorPos != null)
                FrmTBotMotorPos.Close();
        }
    }
}

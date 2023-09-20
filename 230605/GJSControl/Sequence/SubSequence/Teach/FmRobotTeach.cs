using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;


namespace nsSequence
{
    public partial class FmRobotTeach : Form
    {
        private double m_fPitch;
        public bool IsNext;
        public bool m_bIsPrev;
        public bool IsCancel;
     
        private int m_nTeachStepIndex;
        private bool m_bHasEnableControlButton;
        private bool m_bButtonPrevStatus;

        public FmRobotTeach()
        {
            InitializeComponent();
            btnPitch_005_Click(btnPitch_10, null);
            buttonPrev.Enabled = false;
            m_nTeachStepIndex = 0;
        }

        public bool bCheckIfNext()
        {
            if (IsNext)
            {
                IsNext = false;
                return true;
            }
            else
            {
                if(!m_bHasEnableControlButton)
                {
                    Invoke((MethodInvoker)delegate () { vEnableControl(true); });
                    m_bHasEnableControlButton = true;
                }
                if(m_bButtonPrevStatus)
                {
                    Invoke((MethodInvoker)delegate () { buttonPrev.Enabled = true; });
                    m_bButtonPrevStatus = false;
                }
                    
                return false;
            }
                
        }

        public bool bCheckIfCancel()
        {
            if (IsCancel)
            {
                IsCancel = false;
                return true; ;
            }
            else
                return false;
        }

        public bool bCheckIfPrev()
        {
            if (m_bIsPrev)
            {
                m_bIsPrev = false;
                return true; ;
            }
            else
                return false;
        }

       

        public void vSetStopNextPrevButtonEnabled(bool bSwitch)
        {
            Invoke((MethodInvoker)delegate () { buttonStop.Enabled = bSwitch; });
            Invoke((MethodInvoker)delegate () { buttonNext.Enabled = bSwitch; });
            Invoke((MethodInvoker)delegate () { buttonPrev.Enabled = bSwitch; });
        }

        

     

        private void vEnableControl(bool bSwitch)
        {
            buttonNext.Enabled = bSwitch;
            groupBoxX.Enabled = bSwitch;
            groupBoxY.Enabled = bSwitch;
            groupBoxZ.Enabled = bSwitch;
            groupBoxW.Enabled = bSwitch;
            groupBoxP.Enabled = bSwitch;
            groupBoxR.Enabled = bSwitch;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            String sErrorCode = String.Empty;
            //vEnableControl(false);
            m_bHasEnableControlButton = false;
            IsNext = true;

            Visible = false;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            m_bHasEnableControlButton = false;
            m_nTeachStepIndex--;
            if (m_nTeachStepIndex <= 0)
                buttonPrev.Enabled = false;
            m_bIsPrev = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
           G.Comm.FanucRobot.SetStop(true);
        }
        private void btnPitch_005_Click(object sender, EventArgs e)
        {
            if (sender == btnPitch_01)
            {
                m_fPitch = 0.1;
                btnPitch_01.BackColor = Color.LightGray;
                btnPitch_1.BackColor = Color.Gray;
                btnPitch_10.BackColor = Color.Gray;
                btnPitch_20.BackColor = Color.Gray;
            }
            else if (sender == btnPitch_1)
            {
                m_fPitch = 1;
                btnPitch_01.BackColor = Color.Gray;
                btnPitch_1.BackColor = Color.LightGray;
                btnPitch_10.BackColor = Color.Gray;
                btnPitch_20.BackColor = Color.Gray;
            }
            else if (sender == btnPitch_10)
            {
                m_fPitch = 10;
                btnPitch_01.BackColor = Color.Gray;
                btnPitch_1.BackColor = Color.Gray;
                btnPitch_10.BackColor = Color.LightGray;
                btnPitch_20.BackColor = Color.Gray;
            }
            else if (sender == btnPitch_20)
            {
                m_fPitch = 20;
                btnPitch_01.BackColor = Color.Gray;
                btnPitch_1.BackColor = Color.Gray;
                btnPitch_10.BackColor = Color.Gray;
                btnPitch_20.BackColor = Color.LightGray;
            }
        }

        private void btnXMinus_Click(object sender, EventArgs e)
        {
            stRobotPosValueDef stRobotValue =G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(0));
           G.Comm.FanucRobot.GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);


            if (radioWorldCoordinate.Checked)
            {
                if (sender == btnXMinus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(0)) - m_fPitch), 0);
                if (sender == btnXPlus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(0)) + m_fPitch), 0);
                if (sender == btnYMinus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(1)) - m_fPitch), 1);
                if (sender == btnYPlus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(1)) + m_fPitch), 1);
                if (sender == btnZMinus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(2)) - m_fPitch), 2);
                if (sender == btnZPlus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(2)) + m_fPitch), 2);

                if (sender == btnWMinus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(3)) - m_fPitch), 3);
                if (sender == btnWPlus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(3)) + m_fPitch), 3);
                if (sender == btnPMinus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(4)) - m_fPitch), 4);
                if (sender == btnPPlus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(4)) + m_fPitch), 4);
                if (sender == btnRMinus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(5)) - m_fPitch), 5);
                if (sender == btnRPlus)
                    stRobotValue.fXYZWPR.SetValue((float)(Convert.ToDouble(stRobotValue.fXYZWPR.GetValue(5)) + m_fPitch), 5);


                stRobotValue.bGo = true;
                stRobotValue.nGoPercent = 10;
                stRobotValue.nGoContinuePercent = 0;

               G.Comm.FanucRobot.Go(stRobotValue);
            }
            else if(radioJointCoordinate.Checked)
            {
                if (sender == btnXMinus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(0)) - m_fPitch), 0);
                if (sender == btnXPlus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(0)) + m_fPitch), 0);
                if (sender == btnYMinus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(1)) - m_fPitch), 1);
                if (sender == btnYPlus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(1)) + m_fPitch), 1);
                if (sender == btnZMinus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(2)) - m_fPitch), 2);
                if (sender == btnZPlus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(2)) + m_fPitch), 2);

                if (sender == btnWMinus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(3)) - m_fPitch), 3);
                if (sender == btnWPlus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(3)) + m_fPitch), 3);
                if (sender == btnPMinus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(4)) - m_fPitch), 4);
                if (sender == btnPPlus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(4)) + m_fPitch), 4);
                if (sender == btnRMinus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(5)) - m_fPitch), 5);
                if (sender == btnRPlus)
                    stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(5)) + m_fPitch), 5);

                stRobotValue.bGo = true;
                stRobotValue.nGoPercent = 10;
                stRobotValue.nGoContinuePercent = 0;

               G.Comm.FanucRobot.GoJ(stRobotValue);
            }

            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            Visible = false;
        }

        private void radioWorldCoordinate_CheckedChanged(object sender, EventArgs e)
        {
            if (radioWorldCoordinate.Checked)
            {
                groupBoxX.Text = "X";
                groupBoxY.Text = "Y";
                groupBoxZ.Text = "Z";
                groupBoxW.Text = "W";
                groupBoxP.Text = "P";
                groupBoxR.Text = "R";

            }
            else if (radioJointCoordinate.Checked)
            {
                groupBoxX.Text = "J1";
                groupBoxY.Text = "J2";
                groupBoxZ.Text = "J3";
                groupBoxW.Text = "J4";
                groupBoxP.Text = "J5";
                groupBoxR.Text = "J6";
            }
        }
    }
}

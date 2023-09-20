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

namespace nsSequence
{
    public partial class FormMotorTeach : Form
    {

        public bool IsNext;
        public bool IsCancel;
        EAXIS_NAME _axis;
        EAXIS_NAME _axis2;
        EAXIS_NAME _axis3;
        public FormMotorTeach(CommonManagerDef Common)
        {
            InitializeComponent();
        }

            public void ShowStyle(EAXIS_NAME Axis)
        {
          
            _axis = Axis;
            _axis2 = EAXIS_NAME.Count;
            _axis3 = EAXIS_NAME.Count;
           
        }

        public void ShowStyle(EAXIS_NAME Axis, EAXIS_NAME Axis2)
        {
           
            _axis = Axis;
            _axis2 = Axis2;
            _axis3 = EAXIS_NAME.Count;
        }

        public void ShowStyle(EAXIS_NAME Axis, EAXIS_NAME Axis2, EAXIS_NAME Axis3)
        {
         
            _axis = Axis;
            _axis2 = Axis2;
            _axis3 = Axis3;
            
            
        }

       

        private void buttonStop_Click(object sender, EventArgs e)
        {
            G.Comm.MtnCtrl.SdStop(_axis);
            if (_axis2 != EAXIS_NAME.Count)
                G.Comm.MtnCtrl.SdStop(_axis2);

            if (_axis3 != EAXIS_NAME.Count)
                G.Comm.MtnCtrl.SdStop(_axis3);
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            double dis = 0;
            if (!double.TryParse(textBoxMoveDistance.Text, out dis))
                return;

            G.Comm.MtnCtrl.RelMv(_axis, dis, ESPEED_TYPE.Low);  
        }

        private void FormMotorTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            IsNext = true;
            Visible = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            Visible = false;
        }

        private void FormMotorTeach_Load(object sender, EventArgs e)
        {

        }

        private void buttonGo2_Click(object sender, EventArgs e)
        {
            double dis = 0;
            if (!double.TryParse(textBoxMoveDistance2.Text, out dis))
                return;

            G.Comm.MtnCtrl.RelMv(_axis2, dis, ESPEED_TYPE.Low);  
        }

        private void buttonGo3_Click(object sender, EventArgs e)
        {
            double dis = 0;
            if (!double.TryParse(textBoxMoveDistance3.Text, out dis))
                return;

            G.Comm.MtnCtrl.RelMv(_axis3, dis, ESPEED_TYPE.Low);  
        }

        private void FormMotorTeach_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {

                labelAxisPos.Text = G.Comm.MtnCtrl.GetAxisName(_axis).ToString();


                if (_axis2 != EAXIS_NAME.Count)
                {
                    labelAxis2Pos.Text = G.Comm.MtnCtrl.GetAxisName(_axis2).ToString();
                    labelAxis2Pos.Visible = true;
                    labelAxis2.Visible = true;
                    textBoxMoveDistance2.Visible = true;
                    buttonGo2.Visible = true;
                }
                else
                {
                   // labelAxis2Pos.Text = G.Commonon.MtnCtrl.GetAxisName(_axis2).ToString();
                    labelAxis2Pos.Visible = false;
                    labelAxis2.Visible = false;
                    textBoxMoveDistance2.Visible = false;
                    buttonGo2.Visible = false;
                }
                if (_axis3 != EAXIS_NAME.Count)
                {
                    labelAxis3Pos.Text = G.Comm.MtnCtrl.GetAxisName(_axis3).ToString();
                    labelAxis3Pos.Visible = true;
                    labelAxis3.Visible = true;
                    textBoxMoveDistance3.Visible = true;
                    buttonGo3.Visible = true;
                }
                else
                {
                   // labelAxis3Pos.Text = G.Commonon.MtnCtrl.GetAxisName(_axis3).ToString();
                    labelAxis3Pos.Visible = false;
                    labelAxis3.Visible = false;
                    textBoxMoveDistance3.Visible = false;
                    buttonGo3.Visible = false;
                }
            }
        }
    }
}

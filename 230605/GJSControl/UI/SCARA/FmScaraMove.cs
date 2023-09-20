using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary;
using System.Diagnostics;
using System.IO;
using GJSControl.UI;

namespace nsUI
{
    public partial class FmScaraMove : Form
    {
        public FmScaraMove()
        {
            InitializeComponent();

            for (int i = 0; i < (int)EScaraPosition.Count; i++)
                cbxMotorPos.Items.Add(((EScaraPosition)i).ToString());

            cbxMotorPos.SelectedIndex = 0;

            SetSoftLimit();
        }

        double[] _position = new double[4];

        private void BtnXOrJ1Minus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetCartesianCoordinatePostion();
                _currentPos[0] += -(double)numericUpDownMovePitch.Value;
                double[] _targetPos = _currentPos;
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎?", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;
               
               G.Comm.Scara.RelativeMove(-(double)numericUpDownMovePitch.Value, 0, 0, 0, (ushort)numericUpDownScaraSpeed.Value);
            }
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetJ1ToJ4Position();
                _currentPos[0] += -(double)numericUpDownMovePitch.Value;
                double[] _targetPos =G.Comm.Scara.ConvertToCartesianCoordinatePostion(_currentPos);
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J1, -(double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
            }
        }

        private void BtnXOrJ1Plus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetCartesianCoordinatePostion();
                _currentPos[0] += (double)numericUpDownMovePitch.Value;
                double[] _targetPos = _currentPos;
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove((double)numericUpDownMovePitch.Value, 0, 0, 0, (ushort)numericUpDownScaraSpeed.Value);
            }
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetJ1ToJ4Position();
                _currentPos[0] += (double)numericUpDownMovePitch.Value;
                double[] _targetPos =G.Comm.Scara.ConvertToCartesianCoordinatePostion(_currentPos);
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J1, (double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
            }
        }

        private void BtnYOrJ2Minus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetCartesianCoordinatePostion();
                _currentPos[1] += -(double)numericUpDownMovePitch.Value;
                double[] _targetPos = _currentPos;
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove(0, -(double)numericUpDownMovePitch.Value, 0, 0, (ushort)numericUpDownScaraSpeed.Value);
            }
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetJ1ToJ4Position();
                _currentPos[1] += -(double)numericUpDownMovePitch.Value;
                double[] _targetPos =G.Comm.Scara.ConvertToCartesianCoordinatePostion(_currentPos);
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J2, -(double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
            }
        }

        private void BtnYOrJ2Plus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetCartesianCoordinatePostion();
                _currentPos[1] += (double)numericUpDownMovePitch.Value;
                double[] _targetPos = _currentPos;
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove(0, (double)numericUpDownMovePitch.Value, 0, 0, (ushort)numericUpDownScaraSpeed.Value);
            }
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
            {
                double[] _currentPos =G.Comm.Scara.GetJ1ToJ4Position();
                _currentPos[1] += (double)numericUpDownMovePitch.Value;
                double[] _targetPos =G.Comm.Scara.ConvertToCartesianCoordinatePostion(_currentPos);
                if (!GetSoftLimit(_targetPos) && MessageBox.Show("此移動已超出軟體極限，有可能碰撞，請問要繼續移動嗎 ? ", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J2, (double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
            }
        }
        private void BtnZOrJ4Minus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(0, 0, -(double)numericUpDownMovePitch.Value, 0, (ushort)numericUpDownScaraSpeed.Value);
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J4, -(double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
        }

        private void BtnZOrJ4Plus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(0, 0, (double)numericUpDownMovePitch.Value, 0, (ushort)numericUpDownScaraSpeed.Value);
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J4, (double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
        }
        private void BtnAngleOrJ3Minus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(0, 0, 0, -(double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J3, -(double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);

        }
        private void BtnAngleOrJ3Plus_Click(object sender, EventArgs e)
        {
            if (!G.Comm.Scara.IsStopped(false))
                return;

            if (RBtnCartesianCoordinate.Checked && !RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(0, 0, 0, (double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
            if (!RBtnCartesianCoordinate.Checked && RBtnSolarCoordinate.Checked)
               G.Comm.Scara.RelativeMove(ScaraGJSDef.Axis.J3, (double)numericUpDownMovePitch.Value, (ushort)numericUpDownScaraSpeed.Value);
        }
   
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (G.Comm.Scara.GetLsnSignal(ScaraGJSDef.Axis.J1, true))
                light_J1MEL.BackColor = Color.Lime;
            else
                light_J1MEL.BackColor = Color.DarkSeaGreen;
            
            if (G.Comm.Scara.GetLspSignal(ScaraGJSDef.Axis.J1, true))
                light_J1PEL.BackColor = Color.Lime;
            else
                light_J1PEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetLsnSignal(ScaraGJSDef.Axis.J2, true))
                light_J2MEL.BackColor = Color.Lime;
            else
                light_J2MEL.BackColor = Color.DarkSeaGreen;
            
            if (G.Comm.Scara.GetLspSignal(ScaraGJSDef.Axis.J2, true))
                light_J2PEL.BackColor = Color.Lime;
            else 
                light_J2PEL.BackColor = Color.DarkSeaGreen;
            
            if (G.Comm.Scara.GetLsnSignal(ScaraGJSDef.Axis.J3, true))
                light_J3MEL.BackColor = Color.Lime;
            else
                light_J3MEL.BackColor = Color.DarkSeaGreen;
            
            if (G.Comm.Scara.GetLspSignal(ScaraGJSDef.Axis.J3, true))
                light_J3PEL.BackColor = Color.Lime;
            else
                light_J3PEL.BackColor = Color.DarkSeaGreen;
                
            if (G.Comm.Scara.GetLsnSignal(ScaraGJSDef.Axis.J4,true))
                light_J4MEL.BackColor = Color.Lime;
            else
                light_J4MEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetLspSignal(ScaraGJSDef.Axis.J4, true))
                light_J4PEL.BackColor = Color.Lime;
            else
                light_J4PEL.BackColor = Color.DarkSeaGreen;

            #region Soft end limit
            if (G.Comm.Scara.GetSoftLsnSignal(ScaraGJSDef.Axis.J1, true))
                light_J1SMEL.BackColor = Color.Lime;
            else
                light_J1SMEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLspSignal(ScaraGJSDef.Axis.J1, true))
                light_J1SPEL.BackColor = Color.Lime;
            else
                light_J1SPEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLsnSignal(ScaraGJSDef.Axis.J2, true))
                light_J2SMEL.BackColor = Color.Lime;
            else
                light_J2SMEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLspSignal(ScaraGJSDef.Axis.J2, true))
                light_J2SPEL.BackColor = Color.Lime;
            else
                light_J2SPEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLsnSignal(ScaraGJSDef.Axis.J3, true))
                light_J3SMEL.BackColor = Color.Lime;
            else
                light_J3SMEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLspSignal(ScaraGJSDef.Axis.J3, true))
                light_J3SPEL.BackColor = Color.Lime;
            else
                light_J3SPEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLsnSignal(ScaraGJSDef.Axis.J4, true))
                light_J4SMEL.BackColor = Color.Lime;
            else
                light_J4SMEL.BackColor = Color.DarkSeaGreen;

            if (G.Comm.Scara.GetSoftLspSignal(ScaraGJSDef.Axis.J4, true))
                light_J4SPEL.BackColor = Color.Lime;
            else
                light_J4SPEL.BackColor = Color.DarkSeaGreen;
            #endregion

            if (RBtnCartesianCoordinate.Checked)
            {
                _position =G.Comm.Scara.GetCartesianCoordinatePostion();

                LblXPos.Text = _position[0].ToString("0.000");
                LblYPos.Text = _position[1].ToString("0.000");
                LblZPos.Text = _position[2].ToString("0.000");
                LblAnglePos.Text = _position[3].ToString("0.000");
            }
            if (RBtnSolarCoordinate.Checked)
            {
                _position =G.Comm.Scara.GetJ1ToJ4Position();

                LblXPos.Text = _position[0].ToString("0.000");
                LblYPos.Text = _position[1].ToString("0.000");
                LblZPos.Text = _position[3].ToString("0.000");
                LblAnglePos.Text = _position[2].ToString("0.000");
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            timerUpdate.Enabled = false;

            Visible = false;
        }

        private void FmScaraMove_VisibleChanged(object sender, EventArgs e)
        {
            timerUpdate.Enabled = Visible;

            if(Visible)
            {
                double J1SELPlus = 0;
                double J1SELMinus = 0;
                double J2SELPlus = 0;
                double J2SELMinus = 0;
                double J3SELPlus = 0;
                double J3SELMinus = 0;
                double J4SELPlus = 0;
                double J4SELMinus = 0;

                G.Comm.Scara.GetSEL(ScaraGJSDef.Axis.J1, ref J1SELPlus, ref J1SELMinus);
                G.Comm.Scara.GetSEL(ScaraGJSDef.Axis.J2, ref J2SELPlus, ref J2SELMinus);
                G.Comm.Scara.GetSEL(ScaraGJSDef.Axis.J3, ref J3SELPlus, ref J3SELMinus);
                G.Comm.Scara.GetSEL(ScaraGJSDef.Axis.J4, ref J4SELPlus, ref J4SELMinus);

                textBoxJ1SELPlus.Text = J1SELPlus.ToString("0.000");
                textBoxJ1SELMinus.Text = J1SELMinus.ToString("0.000");

                textBoxJ2SELPlus.Text = J2SELPlus.ToString("0.000");
                textBoxJ2SELMinus.Text = J2SELMinus.ToString("0.000");

                textBoxJ3SELPlus.Text = J3SELPlus.ToString("0.000");
                textBoxJ3SELMinus.Text = J3SELMinus.ToString("0.000");

                textBoxJ4SELPlus.Text = J4SELPlus.ToString("0.000");
                textBoxJ4SELMinus.Text = J4SELMinus.ToString("0.000");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
           G.Comm.Scara.StopAll();
        }

        private void BtnSetPos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否將點位設定為目前位置", "點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                 double[] pos =G.Comm.Scara.GetJ1ToJ4Position();
                 for (int i = 0; i < pos.Length; i++)
                    G.Comm.Scara.MotorPositionArray[cbxMotorPos.SelectedIndex].Value[i] = pos[i];
            }
        }

        private void BtnMoveToPos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否移至該點位，請注意機構是否干涉", "移至點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
               G.Comm.Scara.Move((EScaraPosition)cbxMotorPos.SelectedIndex);
            }
        }

        private void radioButtonCartesianCoordinate_CheckedChanged(object sender, EventArgs e)
        {
            if (RBtnCartesianCoordinate.Checked)
            {
                GBox1.Text = "X";
                GBox2.Text = "Y";
                GBox3.Text = "Z";
                GBox4.Text = "Angle";

                LblAxisX.Text = "X座標 : ";
                LblAxisY.Text = "Y座標 : ";
                LblAxisZ.Text = "Z座標 : ";
                LblAngle.Text = "θ座標 : ";
            }
        }

        private void radioButtonSolarCoordinate_CheckedChanged(object sender, EventArgs e)
        {
            if (RBtnSolarCoordinate.Checked)
            {
                GBox1.Text = "J1";
                GBox2.Text = "J2";
                GBox3.Text = "J4";
                GBox4.Text = "J3";

                LblAxisX.Text = "J1座標 : ";
                LblAxisY.Text = "J2座標 : ";
                LblAxisZ.Text = "J4座標 : ";
                LblAngle.Text = "J3座標 : ";
            }
        }

        Region _myRegion = new Region();
        Point _XYtargetPos = new Point();
        private void SetSoftLimit()
        {
            List<PointF> pointList = new List<PointF>();
            pointList.Add(new System.Drawing.PointF(-1000, -1000));
            pointList.Add(new System.Drawing.PointF(-1000,  1000));
            pointList.Add(new System.Drawing.PointF( 1000,  1000));
            pointList.Add(new System.Drawing.PointF( 1000, -1000));

            System.Drawing.Drawing2D.GraphicsPath myGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            //Region _myRegion = new Region();
            myGraphicsPath.Reset();
            //_CurrentPos = new Point((int)GetPosition(EGJSAxisName.M1), (int)GetPosition(EGJSAxisName.M2));
            myGraphicsPath.AddPolygon(pointList.ToArray());
            _myRegion.MakeEmpty();
            _myRegion.Union(myGraphicsPath);
        }

        /*
        private double[] _currentCoordinatePosition;
        public bool GetSoftLimit()
        {
            _currentCoordinatePosition =G.CommononManager.Scara.ConvertToCartesianCoordinatePostion(G.CommononManager.Scara.Gej1Toj4Position());
            _CurrentPos = new Point((int)_currentCoordinatePosition[0],(int)_currentCoordinatePosition[1]);
            bool IsInside = _myRegion.IsVisible(_CurrentPos);
            return IsInside;
        }
        */
        private bool GetSoftLimit(double[] targetPos)
        {
            _XYtargetPos = new Point((int)targetPos[0], (int)targetPos[1]);
            bool IsInside = _myRegion.IsVisible(_XYtargetPos);
            return IsInside;
        }

        private void FmScaraMove_Load(object sender, EventArgs e)
        {
            this.Font = new Font("微軟正黑體", 14);
        }

        private void BtnSetABS_Click(object sender, EventArgs e)
        {
            G.Comm.Scara.SetZeroPosition(0);
            G.Comm.Scara.SetZeroPosition(1);
            G.Comm.Scara.SetZeroPosition(2);
            G.Comm.Scara.SetZeroPosition(3);
        }

        private void ShowNumKeyboard(object sender, EventArgs e)
        {
            FmNumKeyboard k = new FmNumKeyboard((NumericUpDown)sender);
            k.StartPosition = FormStartPosition.CenterScreen;
            k.ShowDialog();
        }

        private void LblAxisZ_Click(object sender, EventArgs e)
        {

        }

        private void buttonSetJ1SEL_Click(object sender, EventArgs e)
        {
            double SELP = 0;
            double SELM = 0;
            if (!double.TryParse(textBoxJ1SELPlus.Text, out SELP))
            {
                MessageBox.Show("J1 SEL Plus Error");
                return;
            }

            if (!double.TryParse(textBoxJ1SELMinus.Text, out SELM))
            {
                MessageBox.Show("J1 SEL Minus Error");
                return;
            }

            G.Comm.Scara.SetSEL(ScaraGJSDef.Axis.J1, SELP, SELM);
        }

        private void buttonSetJ2SEL_Click(object sender, EventArgs e)
        {
            double SELP = 0;
            double SELM = 0;
            if (!double.TryParse(textBoxJ2SELPlus.Text, out SELP))
            {
                MessageBox.Show("J2 SEL Plus Error");
                return;
            }

            if (!double.TryParse(textBoxJ2SELMinus.Text, out SELM))
            {
                MessageBox.Show("J2 SEL Minus Error");
                return;
            }

            G.Comm.Scara.SetSEL(ScaraGJSDef.Axis.J2, SELP, SELM);
        }

        private void buttonSetJ3SEL_Click(object sender, EventArgs e)
        {
            double SELP = 0;
            double SELM = 0;
            if (!double.TryParse(textBoxJ3SELPlus.Text, out SELP))
            {
                MessageBox.Show("J3 SEL Plus Error");
                return;
            }

            if (!double.TryParse(textBoxJ3SELMinus.Text, out SELM))
            {
                MessageBox.Show("J3 SEL Minus Error");
                return;
            }

            G.Comm.Scara.SetSEL(ScaraGJSDef.Axis.J3, SELP, SELM);
        }

        private void buttonSetJ4SEL_Click(object sender, EventArgs e)
        {
            double SELP = 0;
            double SELM = 0;
            if (!double.TryParse(textBoxJ4SELPlus.Text, out SELP))
            {
                MessageBox.Show("J4 SEL Plus Error");
                return;
            }

            if (!double.TryParse(textBoxJ4SELMinus.Text, out SELM))
            {
                MessageBox.Show("J4 SEL Minus Error");
                return;
            }

            G.Comm.Scara.SetSEL(ScaraGJSDef.Axis.J4, SELP, SELM);
        }
    }
}
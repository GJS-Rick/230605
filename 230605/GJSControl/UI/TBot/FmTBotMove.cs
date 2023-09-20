using CommonLibrary;
using System;
using System.Windows.Forms;

namespace nsUI
{
    public partial class FmTBotMove : Form
    {

        public FmTBotMove()
        {
            InitializeComponent();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            //座標顯示 +++
            double M1Pos = G.Comm.TBot.GetPosition(TBotGJSDef.Axis.M1);
            double M2Pos = G.Comm.TBot.GetPosition(TBotGJSDef.Axis.M2);

            Lbl_M1Pos.Text = M1Pos.ToString("0.000");
            Lbl_M2Pos.Text = M2Pos.ToString("0.000");
            //label_M3Pos.Text = _MotorPosMnger.GetPosition(EGJSAxisName.M3).ToString();
            //label_M4Pos.Text = _MotorPosMnger.GetPosition(EGJSAxisName.M4).ToString();

            double yPos = 0, zPos = 0;
            G.Comm.TBot.ConvertMCoordinateToCartesianCoordinate(M1Pos, M2Pos, ref yPos, ref zPos);

            Lbl_XPos.Text = yPos.ToString("0.000");
            Lbl_ZPos.Text = zPos.ToString("0.000");
        }

        private void FmScaraMove_VisibleChanged(object sender, EventArgs e)
        {
            timerUpdate.Enabled = Visible;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            G.Comm.TBot.StopAll();
        }

        private void button_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (!G.Comm.TBot.IsStopped(false))
                return;
            double distance = (double)NumUD_Distance.Value;
            ushort speed = (ushort)NumUD_Speed.Value;
            double M1distance = 0, M2distance = 0;
            G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(0, distance, ref M1distance, ref M2distance);
            G.Comm.TBot.RelativeMove(M1distance, M2distance, speed);
        }

        private void BtnDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!G.Comm.TBot.IsStopped(false))
                return;
            double distance = (double)NumUD_Distance.Value;
            ushort speed = (ushort)NumUD_Speed.Value;
            double M1distance = 0, M2distance = 0;
            G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(0, -distance, ref M1distance, ref M2distance);
            G.Comm.TBot.RelativeMove(M1distance, M2distance, speed);
        }

        private void button_Left_MouseDown(object sender, MouseEventArgs e)
        {
            if (!G.Comm.TBot.IsStopped(false))
                return;
            double distance = (double)NumUD_Distance.Value;
            ushort speed = (ushort)NumUD_Speed.Value;
            double M1distance = 0, M2distance = 0;
            G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(-distance, 0, ref M1distance, ref M2distance);
            G.Comm.TBot.RelativeMove(M1distance, M2distance, speed);
        }

        private void button_Right_MouseDown(object sender, MouseEventArgs e)
        {
            if (!G.Comm.TBot.IsStopped(false))
                return;
            double distance = (double)NumUD_Distance.Value;
            ushort speed = (ushort)NumUD_Speed.Value;
            double M1distance = 0, M2distance = 0;
            G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(distance, 0, ref M1distance, ref M2distance);
            G.Comm.TBot.RelativeMove(M1distance, M2distance, speed);
        }
    }
}
using CommonLibrary;
using System;
using System.Windows.Forms;

namespace nsUI
{
    public partial class FmTBotMotorPos : Form
    {
        public FmTBotMotorPos()
        {
            InitializeComponent();
            vCreateMotorPosHdr();
            vRefreshMotorPos();

            vSetClickEvnet();
        }

        private void vCreateMotorPosHdr()
        {
            if ((int)ETBotPosition.Count < 1)
                return;

            DGVMotorPos.RowCount = (int)ETBotPosition.Count;                 //工作點位
            DGVMotorPos.ColumnCount = (int)TBotGJSDef.Axis.Count + 2;        //軸數 + speed

            // ---------------------------axis header--------------------------------
            for (int i = 0; i < (int)TBotGJSDef.Axis.Count; i++)
                DGVMotorPos.Columns[i + 1].Name = ((TBotGJSDef.Axis)i).ToString();

            DGVMotorPos.Columns[(int)TBotGJSDef.Axis.Count + 1].Name = "Speed(mm/s)";

            DGVMotorPos.Columns[0].HeaderText = "點位";
            DGVMotorPos.Columns[0].Width = 250;
            DGVMotorPos.Columns[0].ReadOnly = true;

            //-------------------------working place header---------------------------
            for (int i = 0; i < DGVMotorPos.RowCount; i++)
                DGVMotorPos.Rows[i].Cells[0].Value = ((ETBotPosition)i).ToString();
        }

        private void vRefreshMotorPos()
        {
            //-------------------------working place header---------------------------
            for (int i = 0; i < (int)ETBotPosition.Count; i++)
                DGVMotorPos.Rows[i].Cells[0].Value = ((ETBotPosition)i).ToString();
            //-------------------------working place position-------------------------
            for (int i = 0; i < (int)ETBotPosition.Count; i++)
            {
                for (int j = 0; j < (int)TBotGJSDef.Axis.Count; j++)
                    DGVMotorPos.Rows[i].Cells[j + 1].Value = "";

                for (int j = 0; j < (int)G.Comm.TBot.NumberOfAxis; j++)
                    DGVMotorPos.Rows[i].Cells[j + 1].Value = G.Comm.TBot._MotorPositionArray[i]._Value[j];

                DGVMotorPos.Rows[i].Cells[DGVMotorPos.ColumnCount - 1].Value = G.Comm.TBot._MotorPositionArray[i]._SpeedPercentage.ToString("0.00");
            }
        }
        // --------點擊儲存-------//
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否儲存點位", "儲存", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (!bSaveParam())
                {
                    MessageBox.Show("點位儲存失敗", "儲存", MessageBoxButtons.OK);
                    return;
                }

                G.Comm.TBot.Save();
                vRefreshMotorPos();

                TextBx_Record(true);
                DGV_Record(true);
            }
        }

        //---------判斷是否能儲存-----------//
        private bool bSaveParam()
        {
            for (int i = 0; i < (int)ETBotPosition.Count; i++)
            {
                for (int j = 0; j < G.Comm.TBot.NumberOfAxis; j++)
                {
                    double fVal = 0;

                    if (double.TryParse(DGVMotorPos.Rows[i].Cells[j + 1].Value.ToString(), out fVal))
                        G.Comm.TBot._MotorPositionArray[i]._Value[j] = fVal;
                    else
                        return false;
                }

                double speed = 0;
                if (double.TryParse(DGVMotorPos.Rows[i].Cells[DGVMotorPos.ColumnCount - 1].Value.ToString(), out speed))
                    G.Comm.TBot._MotorPositionArray[i]._SpeedPercentage = (ushort)speed;
            }

            return true;
        }


        private void BtnExit_Click(object sender, EventArgs e) { Visible = false; }

        private void FmMotorPosData_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                vRefreshMotorPos();

                TextBx_Record(false);
                DGV_Record(false);
                timerUpdate.Enabled = Visible;
            }
            else
                timerUpdate.Enabled = false;
        }

        private void BtnSetMotorPos_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = DGVMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否將點位設定為目前位置", "點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    G.Comm.TBot.SetCurrentPosition((ETBotPosition)DGVMotorPos.SelectedRows[0].Index);
                    vRefreshMotorPos();
                }
            }
        }

        private void BtnPosMove_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = DGVMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否移至該點位，請注意機構是否干涉", "移至點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    G.Comm.TBot.Go((ETBotPosition)DGVMotorPos.SelectedRows[0].Index);
                    //_MotorPosMnger.LinearMove((ETBotPosition)dgvMotorPos.SelectedRows[0].Index);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            LogDef.Add(
                ELogFileName.Operate,
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ((Button)sender).Name.ToString() + " Click");
        }

        private void TextBx_Record(bool bNew)
        {
            foreach (Control ctrl in this.Controls)
            {
                String sMsg = " old value";
                if (bNew)
                    sMsg = " new value";

                if (ctrl is TextBox)
                    LogDef.Add(
                        ELogFileName.Operate,
                        this.GetType().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ((TextBox)ctrl).Text + sMsg);
            }

        }

        private void DGV_Record(bool bNew)
        {
            foreach (Control ctrl in this.Controls)
            {
                String sLastMsg = " old value";
                if (bNew)
                    sLastMsg = " new value";

                if (ctrl is DataGridView)
                {
                    for (int i = 0; i < ((DataGridView)ctrl).RowCount; i++)
                    {
                        String sMsg = ":";
                        for (int j = 0; j < ((DataGridView)ctrl).ColumnCount; j++)
                        {
                            if (((DataGridView)ctrl).Rows[i].Cells[j].Value == null)
                                continue;
                            sMsg += ((DataGridView)ctrl).Rows[i].Cells[j].Value.ToString() + ",";
                        }

                        LogDef.Add(
                            ELogFileName.Operate,
                            this.GetType().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            ((DataGridView)ctrl).Name + sMsg + sLastMsg);
                    }
                }
            }
        }


        private void vSetClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(Btn_Click);
            }
        }

        private void BtnM1M2AbsMove_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = DGVMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否移至該點位，請注意機構是否干涉", "移至點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    G.Comm.TBot.Go((ETBotPosition)DGVMotorPos.SelectedRows[0].Index);
                }
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            double m1 = G.Comm.TBot.GetPosition(TBotGJSDef.Axis.M1);
            double m2 = G.Comm.TBot.GetPosition(TBotGJSDef.Axis.M2);

            double x = 0;
            double z = 0;
            G.Comm.TBot.ConvertMCoordinateToCartesianCoordinate(m1, m2, ref x, ref z);

            Lbl_yPos.Text = x.ToString("0");
            Lbl_zPos.Text = z.ToString("0");
        }

        private void BtnStop_Click(object sender, EventArgs e) { G.Comm.TBot.StopAll(); }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = DGVMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否增加距離", "增加距離", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    double distance = (double)NumUD_Distance.Value;
                    double M1distance = 0, M2distance = 0;
                    G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(0, distance, ref M1distance, ref M2distance);

                    G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[0] += M1distance;
                    G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[1] += M2distance;

                    vRefreshMotorPos();
                }
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否增加距離", "增加距離", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                double distance = (double)NumUD_Distance.Value;
                double M1distance = 0, M2distance = 0;
                G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(0, -distance, ref M1distance, ref M2distance);

                G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[0] += M1distance;
                G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[1] += M2distance;

                vRefreshMotorPos();
            }
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否增加距離", "增加距離", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                double distance = (double)NumUD_Distance.Value;
                double M1distance = 0, M2distance = 0;
                G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(-distance, 0, ref M1distance, ref M2distance);

                G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[0] += M1distance;
                G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[1] += M2distance;

                vRefreshMotorPos();
            }
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否增加距離", "增加距離", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                double distance = (double)NumUD_Distance.Value;
                double M1distance = 0, M2distance = 0;
                G.Comm.TBot.ConvertCartesianCoordinateToMCoordinate(distance, 0, ref M1distance, ref M2distance);

                G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[0] += M1distance;
                G.Comm.TBot._MotorPositionArray[DGVMotorPos.SelectedRows[0].Index]._Value[1] += M2distance;

                vRefreshMotorPos();
            }
        }
    }
}
using CommonLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nsUI
{
    public partial class FmScaraPosData : Form
    {
        public FmScaraPosData()
        {
            InitializeComponent();
            vCreateMotorPosHdr();
            vRefreshMotorPos();
            vSetClickEvnet();
        }

        private void vCreateMotorPosHdr()
        {
            dgvMotorPos.RowCount = (int)EScaraPosition.Count;//工作點位
            dgvMotorPos.ColumnCount = (int)ScaraGJSDef.Axis.Count + 2;//軸數 

            // ---------------------------axis header--------------------------------
            for (int i = 0; i < (int)ScaraGJSDef.Axis.Count; i++)
            {
                dgvMotorPos.Columns[i + 1].Name = ((ScaraGJSDef.Axis)i).ToString();
                dgvMotorPos.Columns[i + 1].Width = 160;
            }

            dgvMotorPos.Columns[0].HeaderText = "點位";
            dgvMotorPos.Columns[0].Width = 250;
            dgvMotorPos.Columns[0].ReadOnly = true;
            dgvMotorPos.Columns[(int)ScaraGJSDef.Axis.Count + 1].Name = "Speed%";
            dgvMotorPos.Columns[dgvMotorPos.ColumnCount - 1].Width = 80;

            for (int i = 0; i < dgvMotorPos.ColumnCount; i++)
                dgvMotorPos.Columns[i].Resizable = DataGridViewTriState.False;

            //-------------------------working place header---------------------------
            for (int i = 0; i < dgvMotorPos.RowCount; i++)
                dgvMotorPos.Rows[i].Cells[0].Value = ((EScaraPosition)i).ToString();
        }

        private void vRefreshMotorPos()
        {
            //-------------------------working place header---------------------------
            for (int i = 0; i < (int)EScaraPosition.Count; i++)
                dgvMotorPos.Rows[i].Cells[0].Value = ((EScaraPosition)i).ToString();

            //-------------------------working place position-------------------------
            for (int i = 0; i < (int)EScaraPosition.Count; i++)
            {
                for (int j = 0; j < (int)ScaraGJSDef.Axis.Count; j++)
                    dgvMotorPos.Rows[i].Cells[j + 1].Value = "";

                for (int j = 0; j < (int)ScaraGJSDef.Axis.Count; j++)
                    dgvMotorPos.Rows[i].Cells[j + 1].Value = G.Comm.Scara.MotorPositionArray[i].Value[j];

                dgvMotorPos.Rows[i].Cells[dgvMotorPos.ColumnCount - 1].Value = G.Comm.Scara.MotorPositionArray[i].SpeedPercentage;
            }
        }

        // --------點擊儲存-------//
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!bSaveParam())
                return;

            G.Comm.Scara.Save();
            vRefreshMotorPos();

            TextBx_Record(true);
            DataGridView_Record(true);
        }

        //---------判斷是否能儲存-----------//
        private bool bSaveParam()
        {
            for (int i = 0; i < (int)EScaraPosition.Count; i++)
            {
                for (int j = 0; j < (int)ScaraGJSDef.Axis.Count; j++)
                {
                    double dVal = 0;
                    if (double.TryParse(dgvMotorPos.Rows[i].Cells[j + 1].Value.ToString(), out dVal))
                    {
                        G.Comm.Scara.MotorPositionArray[i].Value[j] = dVal;
                    }
                    else
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                        AlarmType.Alarm,
                        "儲存格式錯誤");

                        return false;
                    }
                }
                ushort uVal = 0;
                if (ushort.TryParse(dgvMotorPos.Rows[i].Cells[dgvMotorPos.ColumnCount - 1].Value.ToString(), out uVal))
                {
                    G.Comm.Scara.MotorPositionArray[i].SpeedPercentage = (ushort)uVal;
                    if (uVal > 151 || uVal < 1)
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                            AlarmType.Alarm,
                            "Speed%範圍為0-150");

                        return false;
                    }
                }
                else
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                        AlarmType.Alarm,
                        "儲存格式錯誤");

                    return false;
                }
            }

            return true;
        }

        private void FmMotorPosData_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                vRefreshMotorPos();
                TextBx_Record(false);
                DataGridView_Record(false);
            }
        }

        private void BtnSetMotorPos_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dgvMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否將點位設定為目前位置", "點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    double[] pos = G.Comm.Scara.GetJ1ToJ4Position();
                    for (int i = 0; i < pos.Length; i++)
                        G.Comm.Scara.MotorPositionArray[dgvMotorPos.SelectedRows[0].Index].Value[i] = pos[i];

                    vRefreshMotorPos();
                }
            }
            else
            {
                AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                    AlarmType.Alarm,
                    "請選擇一個點位");
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

        private void DataGridView_Record(bool bNew)
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

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dgvMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否移至該點位，請注意機構是否干涉", "移至點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        G.Comm.Scara.Move((EScaraPosition)dgvMotorPos.SelectedRows[0].Index);
                    }
                    catch (Exception ex)
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                        AlarmType.Alarm,
                        ex.Message.ToString());
                    }
                }
            }
            else
            {
                AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                    AlarmType.Alarm,
                    "請選擇一個點位");
            }
        }

        private void FmMotorPosData_Load(object sender, EventArgs e)
        {
            this.Font = new Font("微軟正黑體", 12);
            dgvMotorPos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            G.Comm.Scara.StopAll();
            for (int i = 0; i < (int)ScaraGJSDef.Axis.Count; i++)
                G.Comm.Scara.Stop((ScaraGJSDef.Axis)i);
        }

        private void buttonLineMove_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dgvMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否移至該點位，請注意機構是否干涉", "移至點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        G.Comm.Scara.SendLinearArray(
                            G.Comm.Scara.GetJ1ToJ4Position(),
                            G.Comm.Scara.MotorPositionArray[dgvMotorPos.SelectedRows[0].Index].Value,
                            300,
                            G.Comm.Scara.MotorPositionArray[dgvMotorPos.SelectedRows[0].Index].SpeedPercentage);

                        G.Comm.Scara.LinearMoveStart();
                    }
                    catch (Exception ex)
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                        AlarmType.Alarm,
                        ex.ToString());
                    }
                }
            }
            else
            {
                AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError,
                    AlarmType.Alarm,
                    "請選擇一個點位");
            }
        }
    }
}
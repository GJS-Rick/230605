using CommonLibrary;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace nsUIMotorPosData
{
    public partial class FmMotorPosData : Form
    {
        public FmMotorPosData()
        {
            InitializeComponent();
            vCreateMotorPosHdr();
            vRefreshMotorPos();
            vSetClickEvnet();
        }

        private void vCreateMotorPosHdr()
        {
            if ((int)EMotorPos.Count < 1)
                return;

            DGVMotorPos.RowCount = (int)EMotorPos.Count;//工作點位
            DGVMotorPos.ColumnCount = (int)EAXIS_NAME.Count + 1;//軸數 

            // ---------------------------axis header--------------------------------
            for (int i = 0; i < (int)EAXIS_NAME.Count; i++)
            {
                DGVMotorPos.Columns[i + 1].Name = ((EAXIS_NAME)i).ToString();
                DGVMotorPos.Columns[i + 1].Width = 160;
            }

            DGVMotorPos.Columns[0].HeaderText = "點位";
            DGVMotorPos.Columns[0].Width = 250;
            DGVMotorPos.Columns[0].ReadOnly = true;
            DGVMotorPos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            DataTable dtSpeed = new DataTable();
            dtSpeed.Columns.Add("Speed", typeof(String));

            for (int i = 0; i < (int)ESPEED_TYPE.SPEED_COUNT; i++)
                dtSpeed.Rows.Add(((ESPEED_TYPE)i).ToString());

            DataGridViewComboBoxColumn comboxColumn = new DataGridViewComboBoxColumn
            {
                DataSource = dtSpeed,
                ValueMember = "Speed",
                DisplayMember = "Speed",
                HeaderText = "Speed"
            };
            DGVMotorPos.Columns.Insert(DGVMotorPos.ColumnCount, comboxColumn);
            DGVMotorPos.Columns[DGVMotorPos.ColumnCount - 1].Width = 80;

            for (int i = 0; i < DGVMotorPos.ColumnCount; i++)
                DGVMotorPos.Columns[i].Resizable = DataGridViewTriState.False;
            //-------------------------working place header---------------------------
            for (int i = 0; i < DGVMotorPos.RowCount; i++)
                DGVMotorPos.Rows[i].Cells[0].Value = ((EMotorPos)i).ToString();
        }

        private void vRefreshMotorPos()
        {
            if (G.Comm.MotorPosCollection == null)
                return;
            //-------------------------working place header---------------------------
            for (int i = 0; i < (int)EMotorPos.Count; i++)
            {
                DGVMotorPos.Rows[i].Cells[0].Value = ((EMotorPos)i).ToString();
            }
            //-------------------------working place position-------------------------
            for (int i = 0; i < (int)EMotorPos.Count; i++)
            {
                for (int j = 0; j < (int)EAXIS_NAME.Count; j++)
                    DGVMotorPos.Rows[i].Cells[j + 1].Value = "";

                for (int j = 0; j < (int)G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i).GetAxisNum(); j++)
                {
                    DGVMotorPos.Rows[i].Cells[(int)G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i).GetAxis(j) + 1].Value = G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i)._Value[j];
                }

                DGVMotorPos.Rows[i].Cells[DGVMotorPos.ColumnCount - 1].Value = G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i)._ESpeedType.ToString();
            }
        }

        // --------點擊儲存-------//
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!bSaveParam())
                return;

            G.Comm.MotorPosCollection.Save();
            vRefreshMotorPos();

            TextBx_Record(true);
            DGV_Record(true);
        }

        //---------判斷是否能儲存-----------//
        private bool bSaveParam()
        {
            for (int i = 0; i < (int)EMotorPos.Count; i++)
            {
                for (int j = 0; j < G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i).GetAxisNum(); j++)
                {
                    double fVal = 0;

                    if (double.TryParse(DGVMotorPos.Rows[i].Cells[(int)G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i).GetAxis(j) + 1].Value.ToString(), out fVal))
                        G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i)._Value[j] = fVal;
                    else
                        return false;
                }

                for (int j = 0; j < (int)ESPEED_TYPE.SPEED_COUNT; j++)
                {
                    if (DGVMotorPos.Rows[i].Cells[DGVMotorPos.ColumnCount - 1].Value.ToString() == ((ESPEED_TYPE)j).ToString())
                        G.Comm.MotorPosCollection.GetMotorPos((EMotorPos)i)._ESpeedType = (ESPEED_TYPE)j;
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
                DGV_Record(false);
            }
        }

        private void BtnSetMotorPos_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = DGVMotorPos.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount == 1)
            {
                if (MessageBox.Show("是否將點位設定為目前位置", "點位", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    G.Comm.MotorPosCollection.SetCurrentPos((EMotorPos)DGVMotorPos.SelectedRows[0].Index);
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
                    G.Comm.MotorPosCollection.Move((EMotorPos)DGVMotorPos.SelectedRows[0].Index);
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

        private void FmMotorPosData_Load(object sender, EventArgs e)
        {
            this.Font = new Font("微軟正黑體", 12);
        }

        private void BtnPosStop_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EAXIS_NAME.Count; i++)
                G.Comm.MtnCtrl.Stop((EAXIS_NAME)i, true);

        }
    }
}
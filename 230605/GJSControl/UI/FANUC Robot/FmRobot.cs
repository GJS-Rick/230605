using CommonLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nsUI
{
    public partial class FmRobot : Form
    {
        private float m_fPitch;
        public FmRobot()
        {
            InitializeComponent();
        }

        private void FmRobot_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                vUpdate();

                BtnPitch_01_Click(BtnPitch_5, null);
                G.Comm.FanucRobot.SetDO(ERobotDO.IMPSTP, true);
                G.Comm.FanucRobot.SetDO(ERobotDO.SFSPD, true);
                G.Comm.FanucRobot.SetDO(ERobotDO.ENBL, true);
                G.Comm.FanucRobot.SetDO(ERobotDO.HOLD, true);
                G.Comm.FanucRobot.SetDO(ERobotDO.RSR1, false);
                G.Comm.FanucRobot.SetDO(ERobotDO.RSR2, false);
                G.Comm.FanucRobot.SetDO(ERobotDO.FAULT_RESET, true);
                G.Comm.FanucRobot.SetSpeedRate(10);
                System.Threading.Thread.Sleep(600);

                G.Comm.FanucRobot.SetDO(ERobotDO.FAULT_RESET, false);
                G.Comm.FanucRobot.SetDO(ERobotDO.CSTOPI, true);
                System.Threading.Thread.Sleep(600);

                G.Comm.FanucRobot.SetDO(ERobotDO.CSTOPI, false);
                G.Comm.FanucRobot.SetDO(ERobotDO.RSR1, true);
            }
            else
            {
                G.Comm.FanucRobot.SetDO(ERobotDO.RSR1, false);
                G.Comm.FanucRobot.SetDO(ERobotDO.CSTOPI, true);
            }
        }

        private void vUpdate()
        {
            DGVValue.Rows.Clear();
            DGVValue.RowCount = (int)ERobotPosition.Count + 1;
            for (int i = 0; i < (int)ERobotPosition.Count; i++)
            {
                DGVValue.Rows[i].Cells[0].ReadOnly = true;

                DGVValue.Rows[i].Cells[0].Value = ((ERobotPosition)(i)).ToString();
                DGVValue.Rows[i].Cells[1].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).bGo.ToString();
                DGVValue.Rows[i].Cells[2].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).nGoPercent.ToString();
                DGVValue.Rows[i].Cells[3].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).nGoContinuePercent.ToString();
                DGVValue.Rows[i].Cells[4].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).nMoveSpeed.ToString();

                DGVValue.Rows[i].Cells[5].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).fXYZWPR.GetValue(0).ToString();
                DGVValue.Rows[i].Cells[6].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).fXYZWPR.GetValue(1).ToString();
                DGVValue.Rows[i].Cells[7].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).fXYZWPR.GetValue(2).ToString();
                DGVValue.Rows[i].Cells[8].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).fXYZWPR.GetValue(3).ToString();
                DGVValue.Rows[i].Cells[9].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).fXYZWPR.GetValue(4).ToString();
                DGVValue.Rows[i].Cells[10].Value = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i)).fXYZWPR.GetValue(5).ToString();
            }
        }

        private void TabPageSetColor(TabControl TabCtl)
        {
            for (int i = 0; i < TabCtl.TabPages.Count; i++)
                TabCtl.TabPages[i].BackColor = Color.FromArgb(0xdd2378);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否儲存點位", "Save", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;

            for (int i = 0; i < (int)ERobotPosition.Count; i++)
            {
                stRobotPosValueDef stRobotValue = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(i));

                bool bGo = false;
                if (!bool.TryParse(DGVValue.Rows[i].Cells[1].Value.ToString(), out bGo))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":bGo 格式錯誤");
                    return;
                }
                stRobotValue.bGo = bGo;

                int nGoPercent = 0;
                if (!int.TryParse(DGVValue.Rows[i].Cells[2].Value.ToString(), out nGoPercent))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":nGoPercent 格式錯誤");
                    return;
                }
                stRobotValue.nGoPercent = nGoPercent;

                int nGoContinuePercent = 0;
                if (!int.TryParse(DGVValue.Rows[i].Cells[3].Value.ToString(), out nGoContinuePercent))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":nGoContinuePercent 格式錯誤");
                    return;
                }
                stRobotValue.nGoContinuePercent = nGoContinuePercent;

                int nMoveSpeed = 0;
                if (!int.TryParse(DGVValue.Rows[i].Cells[4].Value.ToString(), out nMoveSpeed))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":nMoveSpeed 格式錯誤");
                    return;
                }
                stRobotValue.nMoveSpeed = nMoveSpeed;


                float fX = 0;
                if (!float.TryParse(DGVValue.Rows[i].Cells[5].Value.ToString(), out fX))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":fX 格式錯誤");
                    return;
                }
                stRobotValue.fXYZWPR.SetValue(fX, 0);

                float fY = 0;
                if (!float.TryParse(DGVValue.Rows[i].Cells[6].Value.ToString(), out fY))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":fY 格式錯誤");
                    return;
                }
                stRobotValue.fXYZWPR.SetValue(fY, 1);

                float fZ = 0;
                if (!float.TryParse(DGVValue.Rows[i].Cells[7].Value.ToString(), out fZ))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":fZ 格式錯誤");
                    return;
                }
                stRobotValue.fXYZWPR.SetValue(fZ, 2);

                float fW = 0;
                if (!float.TryParse(DGVValue.Rows[i].Cells[8].Value.ToString(), out fW))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":fW 格式錯誤");
                    return;
                }
                stRobotValue.fXYZWPR.SetValue(fW, 3);

                float fP = 0;
                if (!float.TryParse(DGVValue.Rows[i].Cells[9].Value.ToString(), out fP))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":fP 格式錯誤");
                    return;
                }
                stRobotValue.fXYZWPR.SetValue(fP, 4);

                float fR = 0;
                if (!float.TryParse(DGVValue.Rows[i].Cells[10].Value.ToString(), out fR))
                {
                    MessageBox.Show(((ERobotPosition)(i)).ToString() + ":fR 格式錯誤");
                    return;
                }
                stRobotValue.fXYZWPR.SetValue(fR, 5);

                G.Comm.FanucRobot.SetRobotPos((ERobotPosition)(i), stRobotValue);
            }

            G.Comm.FanucRobot.SaveFile();
            G.Comm.FanucRobot.SetAllPosition();
        }

        private void BtnPitch_01_Click(object sender, EventArgs e)
        {
            BtnPitch_01.BackColor = Color.Gray;
            BtnPitch_1.BackColor = Color.Gray;
            BtnPitch_5.BackColor = Color.Gray;
            BtnPitch_10.BackColor = Color.Gray;
            BtnPitch_30.BackColor = Color.Gray;

            if (sender == BtnPitch_01)
            {
                BtnPitch_01.BackColor = Color.LightGray;
                m_fPitch = (float)0.1;
            }
            else if (sender == BtnPitch_1)
            {
                BtnPitch_1.BackColor = Color.LightGray;
                m_fPitch = 1;
            }
            else if (sender == BtnPitch_5)
            {
                BtnPitch_5.BackColor = Color.LightGray;
                m_fPitch = 5;
            }
            else if (sender == BtnPitch_10)
            {
                BtnPitch_10.BackColor = Color.LightGray;
                m_fPitch = 10;
            }
            else if (sender == BtnPitch_30)
            {
                BtnPitch_30.BackColor = Color.LightGray;
                m_fPitch = 30;
            }
        }

        private void BtnMovePosition_Click(object sender, EventArgs e)
        {
            if (DGVValue.CurrentRow == null || DGVValue.CurrentRow.Index < 0 || DGVValue.CurrentRow.Index >= (int)ERobotPosition.Count)
            {
                MessageBox.Show("請選擇點位");
                return;
            }

            if (MessageBox.Show("請確認路徑是否干涉，是否移至點位", "移動", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;

            G.Comm.FanucRobot.Go((ERobotPosition)(DGVValue.CurrentRow.Index));
        }

        private void BtnSetSelectToCurrentPos_Click(object sender, EventArgs e)
        {
            if (DGVValue.CurrentRow == null || DGVValue.CurrentRow.Index < 0 || DGVValue.CurrentRow.Index >= (int)ERobotPosition.Count)
            {
                MessageBox.Show("請選擇點位");
                return;
            }

            if (MessageBox.Show("請確認是否設定該點位位置為目前座標", "設定", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                return;

            int nIndex = DGVValue.CurrentRow.Index;

            stRobotPosValueDef stRobotValue = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(nIndex));
            G.Comm.FanucRobot.GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);
            G.Comm.FanucRobot.SetRobotPos((ERobotPosition)(nIndex), stRobotValue);
            DGVValue.Rows[nIndex].Cells[0].Value = ((ERobotPosition)(nIndex)).ToString();
            DGVValue.Rows[nIndex].Cells[1].Value = stRobotValue.bGo.ToString();
            DGVValue.Rows[nIndex].Cells[2].Value = stRobotValue.nGoPercent.ToString();
            DGVValue.Rows[nIndex].Cells[3].Value = stRobotValue.nGoContinuePercent.ToString();
            DGVValue.Rows[nIndex].Cells[4].Value = stRobotValue.nMoveSpeed.ToString();
            DGVValue.Rows[nIndex].Cells[5].Value = stRobotValue.fXYZWPR.GetValue(0).ToString();
            DGVValue.Rows[nIndex].Cells[6].Value = stRobotValue.fXYZWPR.GetValue(1).ToString();
            DGVValue.Rows[nIndex].Cells[7].Value = stRobotValue.fXYZWPR.GetValue(2).ToString();
            DGVValue.Rows[nIndex].Cells[8].Value = stRobotValue.fXYZWPR.GetValue(3).ToString();
            DGVValue.Rows[nIndex].Cells[9].Value = stRobotValue.fXYZWPR.GetValue(4).ToString();
            DGVValue.Rows[nIndex].Cells[10].Value = stRobotValue.fXYZWPR.GetValue(5).ToString();
        }

        private void BtnXMinus_Click(object sender, EventArgs e)
        {
            float[] shift = new float[6];
            if (sender == BtnXMinus)
                shift[0] = -m_fPitch;
            if (sender == BtnXPlus)
                shift[0] = m_fPitch;
            if (sender == BtnYMinus)
                shift[1] = -m_fPitch;
            if (sender == BtnYPlus)
                shift[1] = m_fPitch;
            if (sender == BtnZMinus)
                shift[2] = -m_fPitch;
            if (sender == BtnZPlus)
                shift[2] = m_fPitch;

            if (sender == BtnWMinus)
                shift[3] = -m_fPitch;
            if (sender == BtnWPlus)
                shift[3] = m_fPitch;
            if (sender == BtnPMinus)
                shift[4] = -m_fPitch;
            if (sender == BtnPPlus)
                shift[4] = m_fPitch;
            if (sender == BtnRMinus)
                shift[5] = -m_fPitch;
            if (sender == BtnRPlus)
                shift[5] = m_fPitch;

            G.Comm.FanucRobot.RelGo(10, shift);
        }

        private void BtnStop_Click(object sender, EventArgs e) { G.Comm.FanucRobot.SetStop(true); }

        private void BtnJ1Minus_Click(object sender, EventArgs e)
        {
            stRobotPosValueDef stRobotValue = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(0));
            G.Comm.FanucRobot.GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);

            if (sender == BtnJ1Minus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(0)) - m_fPitch), 0);
            if (sender == BtnJ1Plus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(0)) + m_fPitch), 0);
            if (sender == BtnJ2Minus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(1)) - m_fPitch), 1);
            if (sender == BtnJ2Plus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(1)) + m_fPitch), 1);
            if (sender == BtnJ3Minus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(2)) - m_fPitch), 2);
            if (sender == BtnJ3Plus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(2)) + m_fPitch), 2);

            if (sender == BtnJ4Minus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(3)) - m_fPitch), 3);
            if (sender == BtnJ4Plus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(3)) + m_fPitch), 3);
            if (sender == BtnJ5Minus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(4)) - m_fPitch), 4);
            if (sender == BtnJ5Plus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(4)) + m_fPitch), 4);
            if (sender == BtnJ6Minus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(5)) - m_fPitch), 5);
            if (sender == BtnJ6Plus)
                stRobotValue.fJoint.SetValue((float)(Convert.ToDouble(stRobotValue.fJoint.GetValue(5)) + m_fPitch), 5);

            stRobotValue.bGo = true;
            stRobotValue.nGoPercent = 10;
            stRobotValue.nGoContinuePercent = 0;

            G.Comm.FanucRobot.GoJ(stRobotValue);
        }

        private void BtnGetPosition_Click(object sender, EventArgs e)
        {
            stRobotPosValueDef stRobotValue = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(0));
            G.Comm.FanucRobot.GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);
            dataGridViewNowPosition.Rows[0].Cells[0].Value = stRobotValue.fXYZWPR.GetValue(0).ToString();
            dataGridViewNowPosition.Rows[0].Cells[1].Value = stRobotValue.fXYZWPR.GetValue(1).ToString();
            dataGridViewNowPosition.Rows[0].Cells[2].Value = stRobotValue.fXYZWPR.GetValue(2).ToString();
            dataGridViewNowPosition.Rows[0].Cells[3].Value = stRobotValue.fXYZWPR.GetValue(3).ToString();
            dataGridViewNowPosition.Rows[0].Cells[4].Value = stRobotValue.fXYZWPR.GetValue(4).ToString();
            dataGridViewNowPosition.Rows[0].Cells[5].Value = stRobotValue.fXYZWPR.GetValue(5).ToString();
        }

        private void BtnGetJPosition_Click(object sender, EventArgs e)
        {
            stRobotPosValueDef stRobotValue = G.Comm.FanucRobot.GetRobotPos((ERobotPosition)(0));
            G.Comm.FanucRobot.GetCurrentPos(ref stRobotValue.fXYZWPR, ref stRobotValue.nConfig, ref stRobotValue.fJoint, ref stRobotValue.nUF, ref stRobotValue.nUT, ref stRobotValue.nValidC, ref stRobotValue.nValidJ);
            dataGridViewJPosition.Rows[0].Cells[0].Value = stRobotValue.fJoint.GetValue(0).ToString();
            dataGridViewJPosition.Rows[0].Cells[1].Value = stRobotValue.fJoint.GetValue(1).ToString();
            dataGridViewJPosition.Rows[0].Cells[2].Value = stRobotValue.fJoint.GetValue(2).ToString();
            dataGridViewJPosition.Rows[0].Cells[3].Value = stRobotValue.fJoint.GetValue(3).ToString();
            dataGridViewJPosition.Rows[0].Cells[4].Value = stRobotValue.fJoint.GetValue(4).ToString();
            dataGridViewJPosition.Rows[0].Cells[5].Value = stRobotValue.fJoint.GetValue(5).ToString();
        }

        private void FmRobot_Load(object sender, EventArgs e)
        {
            this.Font = new Font("微軟正黑體", 12);
            TabPageSetColor(TabCtl);

            DGVValue.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGVValue.ScrollBars = ScrollBars.Both;

            for (int i = 0; i < DGVValue.ColumnCount; i++)
            {
                DGVValue.Columns[i].Resizable = DataGridViewTriState.False;
                if (i == 0)
                    DGVValue.Columns[i].Width = 250;
                else
                    DGVValue.Columns[i].Width = 160;
            }
        }
    }
}
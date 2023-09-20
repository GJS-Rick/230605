using CommonLibrary;
using FileStreamLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nsUI
{
    public partial class FmMachineData : Form
    {
        public FmMachineData()
        {
            InitializeComponent();

            vSetClickEvnet();
        }

        private void FmMachineData_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                vRefreshUI();
        }
        public void vRefreshUI()
        {
            DGVMachine.Rows.Clear();
            DGVMachine.RowCount = (int)EMachineDouble.Count + (int)EMachineInt.Count + (int)EMachineString.Count;

            for (int i = 0; i < (int)EMachineDouble.Count; i++)
            {
                DGVMachine.Rows[i].Cells[0].Value = "D" + i.ToString("00");
                DGVMachine.Rows[i].Cells[1].Value = G.FS.MachineData.GetCaption((EMachineDouble)i);
                DGVMachine.Rows[i].Cells[2].Value = G.FS.MachineData.GetValue((EMachineDouble)i).ToString();
            }

            for (int i = (int)EMachineDouble.Count; i < (int)EMachineInt.Count + (int)EMachineDouble.Count; i++)
            {
                DGVMachine.Rows[i].Cells[0].Value = "I" + (i - (int)EMachineDouble.Count).ToString("00");
                DGVMachine.Rows[i].Cells[1].Value = G.FS.MachineData.GetCaption((EMachineInt)(i - (int)EMachineDouble.Count));
                DGVMachine.Rows[i].Cells[2].Value = G.FS.MachineData.GetValue((EMachineInt)(i - (int)EMachineDouble.Count)).ToString();
            }

            for (int i = (int)EMachineDouble.Count + (int)EMachineInt.Count; i < (int)EMachineInt.Count + (int)EMachineDouble.Count + (int)EMachineString.Count; i++)
            {
                DGVMachine.Rows[i].Cells[0].Value = "S" + (i - ((int)EMachineInt.Count + (int)EMachineDouble.Count)).ToString("00");
                DGVMachine.Rows[i].Cells[1].Value = G.FS.MachineData.GetCaption((EMachineString)(i - (int)EMachineDouble.Count - (int)EMachineInt.Count));
                DGVMachine.Rows[i].Cells[2].Value = G.FS.MachineData.GetValue((EMachineString)(i - (int)EMachineDouble.Count - (int)EMachineInt.Count));
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)EMachineDouble.Count; i++)
            {
                double Num = 0;
                if (DGVMachine.Rows[i].Cells[2].Value != null && !double.TryParse(DGVMachine.Rows[i].Cells[2].Value.ToString(), out Num))
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError, AlarmType.Alarm, DGVMachine.Rows[i].Cells[0].Value + "儲存格式錯誤");
                    return;
                }

                G.FS.MachineData.SetValue((EMachineDouble)i, Num);
            }

            for (int i = (int)EMachineDouble.Count; i < (int)EMachineInt.Count + (int)EMachineDouble.Count; i++)
            {
                int Num = 0;
                if (!int.TryParse(DGVMachine.Rows[i].Cells[2].Value.ToString(), out Num))
                {
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_FormatError, AlarmType.Alarm, DGVMachine.Rows[i].Cells[0].Value + "儲存格式錯誤");
                    return;
                }

                G.FS.MachineData.SetValue((EMachineInt)(i - (int)EMachineDouble.Count), Num);
            }

            for (int i = (int)EMachineDouble.Count + (int)EMachineInt.Count; i < (int)EMachineInt.Count + (int)EMachineDouble.Count + (int)EMachineString.Count; i++)
            {
                if (DGVMachine.Rows[i].Cells[2].Value == null)
                    DGVMachine.Rows[i].Cells[2].Value = 0;
                G.FS.MachineData.SetValue((EMachineString)(i - (int)EMachineDouble.Count - (int)EMachineInt.Count), DGVMachine.Rows[i].Cells[2].Value.ToString());
            }

            G.FS.MachineData.Save();
        }

        #region
        TextBox _DGCtl;//定義輸入框控制元件物件
        private void DGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //只對TextBox型別的單元格進行驗證
            if (e.Control.GetType().BaseType.Name == "TextBox")
            {
                _DGCtl = new TextBox();
                _DGCtl = (TextBox)e.Control;

                _DGCtl.KeyPress += new KeyPressEventHandler(DGCtl_KeyPress);
                _DGCtl.Leave += new EventHandler(DGCtl_Leave);//非數字型別單元格
            }
        }
        void DGCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;//禁止空格鍵
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;//處理負數
            if (e.KeyChar > 0x20)
            {
                try
                {
                    if (double.TryParse(((TextBox)sender).Text, out double dValue))
                    {
                        if (e.KeyChar == 0x2D)
                        {
                            e.KeyChar = (char)0;

                            if (dValue > 0)
                                ((TextBox)sender).Text = "-" + ((TextBox)sender).Text;
                        }
                    }
                    else
                    {
                        if (e.KeyChar < 0x21 && e.KeyChar > 0x7E)
                            e.KeyChar = (char)0;
                    }
                }
                catch
                {
                    e.KeyChar = (char)0;
                }
            }
        }
        void DGCtl_Leave(object sender, EventArgs e)
        {
            if ((((TextBox)sender).Text == "-") || (((TextBox)sender).Text == ""))
                ((TextBox)sender).Text = "0";
        }
        #endregion

        private void Btn_Click(object sender, EventArgs e)
        {
            LogDef.Add(
                ELogFileName.Operate,
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ((Button)sender).Name.ToString() + " Click");
        }

        private void vSetClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(Btn_Click);
            }
        }

        private void FmMachineData_Load(object sender, EventArgs e)
        {
            this.Font = new Font("微軟正黑體", 14);
        }
    }
}
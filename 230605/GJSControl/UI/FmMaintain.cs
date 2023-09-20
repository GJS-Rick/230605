using CommonLibrary;
using FileStreamLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace nsUI
{
    public partial class FmMaintain : Form
    {
        /// <summary>已通知</summary>
        private bool[] _Notified;
        readonly bool _AlarmOn;
        public FmMaintain()
        {
            InitializeComponent();

            _AlarmOn = G.FS.MachineData.GetValue(EMachineInt.MaintainOvertimeAlarm) > 0;
            _Notified = new bool[G.Comm.Maintain.GetItemCount()];

            DGVMaintain.RowCount = G.Comm.Maintain.GetItemCount();
            for (int i = 0; i < G.Comm.Maintain.GetItemCount(); i++)
            {
                for (int j = 0; j < 3; j++)
                    DGVMaintain.Rows[i].Cells[j].Value = G.Comm.Maintain.GetItem(i)[j];

                DGVMaintain.Rows[i].Cells[3].Value = G.Comm.Maintain.GetIntervalDays(i);
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (_AlarmOn)
            {
                for (int i = 0; i < G.Comm.Maintain.GetItemCount(); i++)
                {
                    if (!_Notified[i])
                    {
                        if (G.Comm.Maintain.OverTime(i))
                        {
                            string[] strArr = G.Comm.Maintain.GetItem(i);
                            AlarmTextDisplay.Add("NeedMaintenance", AlarmType.Warning, strArr[0] + strArr[2]);
                            _Notified[i] = true;
                        }
                    }
                }
            }
        }

        private void DGVMaintain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < G.Comm.Maintain.GetItemCount() && e.ColumnIndex == 4)
            {
                G.Comm.Maintain.MaintainOn(e.RowIndex);
                DGVMaintain.Rows[e.RowIndex].Cells[3].Value = G.Comm.Maintain.GetIntervalDays(e.RowIndex);
                _Notified[e.RowIndex] = false;
            }
        }

        private void DGVMaintain_VisibleChanged(object sender, EventArgs e)
        {
            timerUpdate.Enabled = false;
        }

        private void FmMaintain_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12, style: FontStyle.Bold);
        }
    }
}
using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using CommonLibrary;
using static CommonLibrary.MonitorDataTypeDef;

namespace nsFmMotion
{
    public partial class FmMonitorSet : Form
    {
        int _DGVRow;

        public FmMonitorSet()
        {
            InitializeComponent();
        }

        private void frmMonitorSet_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);

            Renew();
        }

        private void Renew()
        {
            #region DGV
            _DGVRow = -1;
            DGVUseMonitor.Rows.Clear();

            for (int i = 0; i < DGVUseMonitor.ColumnCount; i++)
                DGVUseMonitor.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            for (int i = 0; i < G.Comm.Monitor.GetMonitorNum(); i++)
                DGVUseMonitor.Rows.Add(G.Comm.Monitor.GetMonitorSimpleData(i));
            #endregion

            #region ComboBox
            ResetComboBox();
            #endregion
        }
        private void ResetComboBox()
        {
            #region 元件名稱
            CB_MonitorName.Items.Clear();
            for (int i = 0; i < (int)EMonitorName.Count; i++)
                CB_MonitorName.Items.Add(((EMonitorName)i).ToString());
            CB_MonitorName.SelectedIndex = 0;
            #endregion
            #region 站號
            CB_StationNum.Items.Clear();
            for (int i = 0; i < 127; i++)
                CB_StationNum.Items.Add(i.ToString());
            CB_StationNum.SelectedIndex = 1;
            #endregion
            #region 通訊埠
            string[] ports = SerialPort.GetPortNames();
            CB_PortName.Items.Clear();
            for (int i = 0; i < ports.Length; i++)
                CB_PortName.Items.Add(ports[i]);
            CB_PortName.SelectedIndex = -1;
            #endregion
            ReloadPort();
            #region 鮑率
            CB_BaudRate.Items.Clear();
            CB_BaudRate.Items.Add("1200");
            CB_BaudRate.Items.Add("2400");
            CB_BaudRate.Items.Add("4800");
            CB_BaudRate.Items.Add("9600");
            CB_BaudRate.Items.Add("14400");
            CB_BaudRate.Items.Add("19200");
            CB_BaudRate.Items.Add("38400");
            CB_BaudRate.Items.Add("56000");
            CB_BaudRate.Items.Add("57600");
            CB_BaudRate.Items.Add("115200");
            CB_BaudRate.Items.Add("128000");
            CB_BaudRate.Items.Add("256000");
            CB_BaudRate.Items.Add("460800");
            CB_BaudRate.Items.Add("512000");
            CB_BaudRate.Items.Add("750000");
            CB_BaudRate.SelectedIndex = 3;
            #endregion
            #region 同位檢查
            CB_Parity.Items.Clear();
            for (int i = 0; i < (int)Parity.Space; i++)
                CB_Parity.Items.Add(((Parity)i).ToString());
            CB_Parity.SelectedIndex = 2;
            #endregion
            #region 資料長度
            CB_DataBits.Items.Clear();
            for (int i = 5; i <= 8; i++)
                CB_DataBits.Items.Add(i.ToString());
            CB_DataBits.SelectedIndex = 2;
            #endregion
            #region 停止位元
            CB_StopBits.Items.Clear();
            CB_StopBits.Items.Add(StopBits.None.ToString());
            CB_StopBits.Items.Add(StopBits.One.ToString());
            CB_StopBits.Items.Add(StopBits.OnePointFive.ToString());
            CB_StopBits.Items.Add(StopBits.Two.ToString());
            CB_StopBits.SelectedIndex = 1;
            #endregion
            #region 延遲時間
            CB_DelayTime.Items.Clear();
            CB_DelayTime.Items.Add("0 ms");
            CB_DelayTime.Items.Add("50 ms");
            CB_DelayTime.Items.Add("100 ms");
            CB_DelayTime.Items.Add("150 ms");
            CB_DelayTime.Items.Add("200 ms");
            CB_DelayTime.Items.Add("250 ms");
            CB_DelayTime.SelectedIndex = 0;
            #endregion
        }

        private void ReloadPort()
        {
            #region 通訊埠
            string[] ports = SerialPort.GetPortNames();
            CB_PortName.Items.Clear();
            for (int i = 0; i < ports.Length; i++)
                CB_PortName.Items.Add(ports[i]);
            CB_PortName.SelectedIndex = -1;
            #endregion

            if (ports.Length <= 0)
                AlarmTextDisplay.Add((int)AlarmCode.NoCOMPortCanUse, AlarmType.Warning, "無可用串口");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (CB_MonitorName.SelectedIndex >= 0 &&
                    CB_StationNum.SelectedIndex >= 0 &&
                    CB_PortName.SelectedIndex >= 0 &&
                    CB_BaudRate.SelectedIndex >= 0 &&
                    CB_Parity.SelectedIndex >= 0 &&
                    CB_DataBits.SelectedIndex >= 0 &&
                    CB_StopBits.SelectedIndex >= 0 &&
                    CB_DelayTime.SelectedIndex >= 0)
                {
                    BtnMonitorAdd.Enabled = true;
                }
                else
                    BtnMonitorAdd.Enabled = false;

                for (int i = 0; i < DGVUseMonitor.Rows.Count - 1; i++)
                {
                    if (G.Comm.Monitor != null)
                    {
                        if (G.Comm.Monitor.GetMonitorName(i).ToString() != DGVUseMonitor.Rows[i].Cells[0].Value.ToString() ||
                            G.Comm.Monitor.GetPortName(i) != DGVUseMonitor.Rows[i].Cells[1].Value.ToString() ||
                            G.Comm.Monitor.GetStationNum(i) != DGVUseMonitor.Rows[i].Cells[2].Value.ToString() ||
                            G.Comm.Monitor.GetBaudRate(i).ToString() != DGVUseMonitor.Rows[i].Cells[3].Value.ToString() ||
                            G.Comm.Monitor.GetParity(i).ToString() != DGVUseMonitor.Rows[i].Cells[4].Value.ToString() ||
                            G.Comm.Monitor.GetDataBits(i).ToString() != DGVUseMonitor.Rows[i].Cells[5].Value.ToString() ||
                            G.Comm.Monitor.GetStopBits(i).ToString() != DGVUseMonitor.Rows[i].Cells[6].Value.ToString() ||
                            G.Comm.Monitor.GetDelayTime(i).ToString() != DGVUseMonitor.Rows[i].Cells[7].Value.ToString())
                            DGVUseMonitor.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                        else
                            DGVUseMonitor.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            catch { }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            G.Comm.Monitor.Save();
            G.Comm.Monitor.CreateMonitor();

            Renew();
        }

        private void frmModSet_FormClosing(object sender, FormClosingEventArgs e) { }

        private void DGVUseMonitor_Click(object sender, System.EventArgs e)
        {
            int _Index = DGVUseMonitor.SelectedRows[0].Index;

            if (_Index >= 0 && _Index < DGVUseMonitor.RowCount - 1)
            {
                _DGVRow = _Index;
                CB_MonitorName.SelectedIndex = CB_MonitorName.Items.IndexOf(G.Comm.Monitor.GetMonitorName(_Index).ToString());
                CB_StationNum.SelectedIndex = CB_StationNum.Items.IndexOf(G.Comm.Monitor.GetStationNum(_Index));
                CB_PortName.SelectedIndex = CB_PortName.Items.IndexOf(G.Comm.Monitor.GetPortName(_Index));
                CB_BaudRate.SelectedIndex = CB_BaudRate.Items.IndexOf(G.Comm.Monitor.GetBaudRate(_Index).ToString());
                CB_Parity.SelectedIndex = CB_Parity.Items.IndexOf(G.Comm.Monitor.GetParity(_Index).ToString());
                CB_DataBits.SelectedIndex = CB_DataBits.Items.IndexOf(G.Comm.Monitor.GetDataBits(_Index).ToString());
                CB_StopBits.SelectedIndex = CB_StopBits.Items.IndexOf(G.Comm.Monitor.GetStopBits(_Index).ToString());
                CB_DelayTime.SelectedIndex = CB_DelayTime.Items.IndexOf(G.Comm.Monitor.GetDelayTime(_Index).ToString() + " ms");
            }
            else
            {
                _DGVRow = -1;
                CB_MonitorName.SelectedIndex = -1;
                CB_StationNum.SelectedIndex = 1;
                CB_PortName.SelectedIndex = -1;
                CB_BaudRate.SelectedIndex = 3;
                CB_Parity.SelectedIndex = 2;
                CB_DataBits.SelectedIndex = 2;
                CB_StopBits.SelectedIndex = 1;
                CB_DelayTime.SelectedIndex = 0;
            }
        }

        private void BtnMonitorAdd_Click(object sender, EventArgs e)
        {
            if (CB_MonitorName.SelectedIndex >= 0 &&
                    CB_StationNum.SelectedIndex >= 0 &&
                    CB_PortName.SelectedIndex >= 0 &&
                    CB_BaudRate.SelectedIndex >= 0 &&
                    CB_Parity.SelectedIndex >= 0 &&
                    CB_DataBits.SelectedIndex >= 0 &&
                    CB_StopBits.SelectedIndex >= 0 &&
                    CB_DelayTime.SelectedIndex >= 0)
            {
                G.Comm.Monitor.AddMonitor(
                    (EMonitorName)CB_MonitorName.SelectedIndex,
                    CB_StationNum.SelectedIndex.ToString(),
                    CB_PortName.Text,
                    Convert.ToInt32(CB_BaudRate.Text.ToString()),
                    (Parity)CB_Parity.SelectedIndex,
                    Convert.ToInt32(CB_DataBits.Text.ToString()),
                    (StopBits)CB_StopBits.SelectedIndex,
                    CB_DelayTime.SelectedIndex);

                Renew();
            }
        }
        private void BtnMonitorRemove_Click(object sender, EventArgs e)
        {
            if (_DGVRow < 0)
                return;

            G.Comm.Monitor.RemoveMonitor(_DGVRow);

            Renew();
        }

        private void BtnReloadPort(object sender, EventArgs e)
        {
            ReloadPort();
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_DGVRow < 0)
                return;

            if (sender == (ComboBox)CB_MonitorName)
                G.Comm.Monitor.SetMonitorName(_DGVRow, (EMonitorName)CB_MonitorName.SelectedIndex);
            else if (sender == (ComboBox)CB_StationNum)
                G.Comm.Monitor.SetStationNum(_DGVRow, CB_StationNum.Text);
            else if (sender == (ComboBox)CB_PortName)
                G.Comm.Monitor.SetPortName(_DGVRow, CB_PortName.Text);
            else if (sender == (ComboBox)CB_BaudRate)
                G.Comm.Monitor.SetBaudRate(_DGVRow, Convert.ToInt32(CB_BaudRate.Text));
            else if (sender == (ComboBox)CB_Parity)
                G.Comm.Monitor.SetParity(_DGVRow, (Parity)CB_Parity.SelectedIndex);
            else if (sender == (ComboBox)CB_DataBits)
                G.Comm.Monitor.SetDataBits(_DGVRow, Convert.ToInt32(CB_DataBits.Text));
            else if (sender == (ComboBox)CB_StopBits)
                G.Comm.Monitor.SetStopBits(_DGVRow, (StopBits)CB_StopBits.SelectedIndex);
            else if (sender == (ComboBox)CB_DelayTime)
                G.Comm.Monitor.SetDelayTime(_DGVRow, CB_DelayTime.SelectedIndex * 50);
            else { }

            //if (CB_MonitorName.SelectedIndex >= 0 &&
            //        CB_StationNum.SelectedIndex >= 0 &&
            //        CB_PortName.SelectedIndex >= 0 &&
            //        CB_BaudRate.SelectedIndex >= 0 &&
            //        CB_Parity.SelectedIndex >= 0 &&
            //        CB_DataBits.SelectedIndex >= 0 &&
            //        CB_StopBits.SelectedIndex >= 0 &&
            //        CB_DelayTime.SelectedIndex >= 0)
            //{
            //    G.Common.Monitor.Save();
            //}
        }
    }
}

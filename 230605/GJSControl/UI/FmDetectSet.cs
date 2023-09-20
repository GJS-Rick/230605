using CommonLibrary;
using System;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static CommonLibrary.DetectBaseDef;

namespace nsFmMotion
{
    public partial class FmDetectSet : Form
    {
        private enum EDGVColName
        {
            ComPort,
            BaudRate,
            Parity,
            DataBits,
            StopBits,
            DelayTime,

            TaskTimes,
            Offset,
            TestBtn,
            StrResult,

            Count
        }

        private DetectInfo[] DetectArr;
        private string[] _CanUsePort;
        private DataGridViewRow _NewRows;

        public FmDetectSet()
        {
            InitializeComponent();

            ReloadPort();

            DGVUseDetect.RowHeadersVisible = true;
            DGVUseDetect.RowHeadersWidth = 25;
            DGVUseDetect.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DGVUseDetect.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            DGVUseDetect.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            _NewRows = new DataGridViewRow();

            vCreateDGVColHdrAndBaseRow();
            vRefreshDetectObj();
        }

        private void vCreateDGVColHdrAndBaseRow()
        {
            DataGridViewButtonColumn btnColumn;
            DataGridViewComboBoxColumn comboxColumn;
            DataGridViewTextBoxColumn textColumn;
            DataTable dtTypeName;

            DGVUseDetect.ColumnCount = 0;
            DGVUseDetect.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGVUseDetect.ColumnHeadersHeight = 25;

            for (int i = 0; i < (int)EDGVColName.Count; i++)
            {
                dtTypeName = new DataTable();
                dtTypeName.Columns.Add(((EDGVColName)i).ToString(), typeof(String));

                switch ((EDGVColName)i)
                {
                    case EDGVColName.ComPort:
                        for (int j = 0; j < _CanUsePort.Length; j++)
                            dtTypeName.Rows.Add(_CanUsePort[j].ToString());
                        dtTypeName.Rows.Add("None");

                        comboxColumn = new DataGridViewComboBoxColumn
                        {
                            FlatStyle = FlatStyle.Flat,
                            DataSource = dtTypeName,
                            DisplayMember = ((EDGVColName)i).ToString(),
                            ValueMember = ((EDGVColName)i).ToString()
                        };

                        DGVUseDetect.Columns.Add(comboxColumn);
                        DGVUseDetect.Columns[i].Name = "通訊埠";
                        break;
                    case EDGVColName.BaudRate:
                        string[] br = G.Comm.Detect.BaudRateRange;
                        for (int j = 0; j < br.Length; j++)
                            dtTypeName.Rows.Add(br[j].ToString());

                        comboxColumn = new DataGridViewComboBoxColumn
                        {
                            FlatStyle = FlatStyle.Flat,
                            DataSource = dtTypeName,
                            DisplayMember = ((EDGVColName)i).ToString(),
                            ValueMember = ((EDGVColName)i).ToString()
                        };

                        DGVUseDetect.Columns.Add(comboxColumn);
                        DGVUseDetect.Columns[i].Name = "鮑率";
                        break;
                    case EDGVColName.Parity:
                        string[] par = G.Comm.Detect.ParityRange;
                        for (int j = 0; j < par.Length; j++)
                            dtTypeName.Rows.Add(par[j]);

                        comboxColumn = new DataGridViewComboBoxColumn
                        {
                            FlatStyle = FlatStyle.Flat,
                            DataSource = dtTypeName,
                            DisplayMember = ((EDGVColName)i).ToString(),
                            ValueMember = ((EDGVColName)i).ToString()
                        };

                        DGVUseDetect.Columns.Add(comboxColumn);
                        DGVUseDetect.Columns[i].Name = "同位檢查";
                        break;
                    case EDGVColName.DataBits:
                        string[] db = G.Comm.Detect.DataBitsRange;
                        for (int j = 0; j < db.Length; j++)
                            dtTypeName.Rows.Add(db[j].ToString());

                        comboxColumn = new DataGridViewComboBoxColumn
                        {
                            FlatStyle = FlatStyle.Flat,
                            DataSource = dtTypeName,
                            DisplayMember = ((EDGVColName)i).ToString(),
                            ValueMember = ((EDGVColName)i).ToString()
                        };

                        DGVUseDetect.Columns.Add(comboxColumn);
                        DGVUseDetect.Columns[i].Name = "資料長度";
                        break;
                    case EDGVColName.StopBits:
                        string[] sb = G.Comm.Detect.StopBitsRange;
                        for (int j = 0; j < sb.Length; j++)
                            dtTypeName.Rows.Add(sb[j].ToString());

                        comboxColumn = new DataGridViewComboBoxColumn
                        {
                            FlatStyle = FlatStyle.Flat,
                            DataSource = dtTypeName,
                            DisplayMember = ((EDGVColName)i).ToString(),
                            ValueMember = ((EDGVColName)i).ToString()
                        };

                        DGVUseDetect.Columns.Add(comboxColumn);
                        DGVUseDetect.Columns[i].Name = "停止位元";
                        break;
                    case EDGVColName.DelayTime:
                        int[] dt = G.Comm.Detect.DelayTimeRange;
                        for (int j = 0; j < dt.Length; j++)
                            dtTypeName.Rows.Add(dt[j].ToString());

                        comboxColumn = new DataGridViewComboBoxColumn
                        {
                            FlatStyle = FlatStyle.Flat,
                            DataSource = dtTypeName,
                            DisplayMember = ((EDGVColName)i).ToString(),
                            ValueMember = ((EDGVColName)i).ToString()
                        };

                        DGVUseDetect.Columns.Add(comboxColumn);
                        DGVUseDetect.Columns[i].Name = "延遲(ms)";
                        break;
                    case EDGVColName.TaskTimes:
                        textColumn = new DataGridViewTextBoxColumn();

                        DGVUseDetect.Columns.Add(textColumn);
                        DGVUseDetect.Columns[i].Name = "讀取次數";
                        break;
                    case EDGVColName.Offset:
                        textColumn = new DataGridViewTextBoxColumn();

                        DGVUseDetect.Columns.Add(textColumn);
                        DGVUseDetect.Columns[i].Name = "補償值";
                        break;
                    case EDGVColName.TestBtn:
                        dtTypeName.Rows.Add("測試");

                        btnColumn = new DataGridViewButtonColumn
                        {
                            UseColumnTextForButtonValue = true,
                            Text = "測試"
                        };

                        DGVUseDetect.Columns.Add(btnColumn);
                        DGVUseDetect.Columns[i].Name = "測試";
                        break;
                    case EDGVColName.StrResult:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.ReadOnly = true;

                        DGVUseDetect.Columns.Add(textColumn);
                        DGVUseDetect.Columns[i].Name = "讀取值";
                        break;
                }

                DGVUseDetect.Columns[i].Width = 200;
                DGVUseDetect.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            _NewRows = DGVUseDetect.Rows[0];

            DGVUseDetect.AllowUserToAddRows = false;
        }
        private void vRefreshDetectObj()
        {
            RenewList();

            if (DetectArr.Length < 1)
                return;

            DGVUseDetect.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DGVUseDetect.Rows.Clear();

            for (int i = 0; i < DetectArr.Length; i++)
            {
                if (DGVUseDetect.RowCount <= 0)
                    DGVUseDetect.Rows.Add(_NewRows);
                else
                    DGVUseDetect.Rows.AddCopy(0);

                for (int j = 0; j < (int)EDGVColName.Count; j++)
                {
                    string sValue = "";

                    switch ((EDGVColName)j)
                    {
                        case EDGVColName.ComPort:
                            if (_CanUsePort.Length > 0 && DetectArr[i].SPortInfo.PortNum > 0)
                                sValue = "COM" + DetectArr[i].SPortInfo.PortNum.ToString();
                            else
                                sValue = "None";
                            break;
                        case EDGVColName.BaudRate:
                            sValue = DetectArr[i].SPortInfo.BaudRate.ToString();
                            break;
                        case EDGVColName.Parity:
                            sValue = DetectArr[i].SPortInfo.Parity.ToString();
                            break;
                        case EDGVColName.DataBits:
                            sValue = DetectArr[i].SPortInfo.DataBits.ToString();
                            break;
                        case EDGVColName.StopBits:
                            sValue = DetectArr[i].SPortInfo.StopBits.ToString();
                            break;
                        case EDGVColName.DelayTime:
                            sValue = DetectArr[i].DelayTime.ToString();
                            break;
                        case EDGVColName.TaskTimes:
                            sValue = DetectArr[i].TaskTimes.ToString();
                            break;
                        case EDGVColName.Offset:
                            sValue = DetectArr[i].Offset.ToString("0.00");
                            break;
                        case EDGVColName.StrResult:
                            sValue = DetectArr[i].StrResult;
                            break;
                    }

                    DGVUseDetect.Rows[i].HeaderCell.Value = DetectArr[i].Name;
                    DGVUseDetect.Rows[i].Cells[j].Value = sValue;
                }
            }
        }

        private void frmDetectSet_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);

            ReloadPort();
            vRefreshDetectObj();
        }

        private void RenewList()
        {
            DetectArr = G.Comm.Detect.GetAllDetect();
        }

        private void ReloadPort()
        {
            _CanUsePort = SerialPort.GetPortNames();

            if (_CanUsePort.Length <= 0)
                AlarmTextDisplay.Add((int)AlarmCode.NoCOMPortCanUse, AlarmType.Warning, "無可用串口");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < DGVUseDetect.Rows.Count; i++)
            {
                string eValue = "";

                for (int j = 0; j < (int)EDetectName.None; j++)
                {
                    if (DGVUseDetect.Rows[i].HeaderCell.Value == null)
                        break;

                    if (DGVUseDetect.Rows[i].HeaderCell.Value.ToString() == ((EDetectName)j).ToString())
                    {
                        if (G.Comm.Detect.TakeDone((EDetectName)j, ref eValue))
                            DGVUseDetect.Rows[i].Cells[(int)EDGVColName.StrResult].Value = eValue;

                        break;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < DGVUseDetect.RowCount; i++)
            {
                DetectInfo InfoBuf = new DetectInfo();
                bool bNullData = false;
                string srtBuf = "";

                if (DGVUseDetect.Rows[i].HeaderCell.Value != null)
                    InfoBuf.Name = DGVUseDetect.Rows[i].HeaderCell.Value.ToString();
                else
                    continue;

                for (int j = 0; j < (int)EDGVColName.Count; j++)
                {
                    if (DGVUseDetect.Rows[i].Cells[j].Value != null)
                        srtBuf = DGVUseDetect.Rows[i].Cells[j].Value.ToString();
                    else
                    {
                        bNullData = true;
                        break;
                    }

                    switch ((EDGVColName)j)
                    {
                        case EDGVColName.ComPort:
                            if (srtBuf == "None" || String.IsNullOrEmpty(srtBuf))
                                srtBuf = "None";
                            else
                                srtBuf = Regex.Split(srtBuf, "COM")[1];

                            InfoBuf.SPortInfo.PortNum = 0;

                            if (uint.TryParse(srtBuf, out uint uComPort))
                                InfoBuf.SPortInfo.PortNum = uComPort;
                            break;
                        case EDGVColName.BaudRate:
                            InfoBuf.SPortInfo.BaudRate = 9600;
                            for (int k = 0; k < G.Comm.Detect.BaudRateRange.Length; k++)
                            {
                                if (srtBuf == G.Comm.Detect.BaudRateRange[k])
                                {
                                    if (int.TryParse(srtBuf, out int iBR))
                                    {
                                        InfoBuf.SPortInfo.BaudRate = iBR;
                                        break;
                                    }
                                }
                            }
                            break;
                        case EDGVColName.Parity:
                            InfoBuf.SPortInfo.Parity = Parity.None;
                            for (int k = 0; k < G.Comm.Detect.ParityRange.Length; k++)
                            {
                                if (srtBuf == G.Comm.Detect.ParityRange[k])
                                {
                                    InfoBuf.SPortInfo.Parity = (Parity)k;
                                    break;
                                }
                            }
                            break;
                        case EDGVColName.DataBits:
                            InfoBuf.SPortInfo.DataBits = 8;
                            for (int k = 0; k < G.Comm.Detect.DataBitsRange.Length; k++)
                            {
                                if (srtBuf == G.Comm.Detect.DataBitsRange[k])
                                {
                                    if (int.TryParse(srtBuf, out int iDB))
                                    {
                                        InfoBuf.SPortInfo.DataBits = iDB;
                                        break;
                                    }
                                }
                            }
                            break;
                        case EDGVColName.StopBits:
                            InfoBuf.SPortInfo.StopBits = StopBits.One;
                            for (int k = 0; k < G.Comm.Detect.StopBitsRange.Length; k++)
                            {
                                if (srtBuf == G.Comm.Detect.StopBitsRange[k])
                                {
                                    InfoBuf.SPortInfo.StopBits = (StopBits)k;
                                    break;
                                }
                            }
                            break;
                        case EDGVColName.DelayTime:
                            InfoBuf.DelayTime = 50;
                            for (int k = 0; k < G.Comm.Detect.DelayTimeRange.Length; k++)
                            {
                                if (srtBuf == G.Comm.Detect.DelayTimeRange[k].ToString())
                                {
                                    if (int.TryParse(srtBuf, out int iDT))
                                    {
                                        InfoBuf.DelayTime = iDT;
                                        break;
                                    }
                                }
                            }
                            break;
                        case EDGVColName.TaskTimes:
                            if (int.TryParse(srtBuf, out int iTT))
                                InfoBuf.TaskTimes = iTT;
                            else
                                InfoBuf.TaskTimes = 1;
                            break;
                        case EDGVColName.Offset:
                            if (double.TryParse(srtBuf, out double dOffset))
                                InfoBuf.Offset = dOffset;
                            else
                                InfoBuf.Offset = 0.000;
                            break;
                        case EDGVColName.StrResult:
                            InfoBuf.StrResult = "";
                            break;
                    }
                }

                if (bNullData)
                    bNullData = false;
                else
                    G.Comm.Detect.SetDetect(InfoBuf, i);
            }

            G.Comm.Detect.Save();
            vRefreshDetectObj();
        }

        private void frmDetectSet_FormClosing(object sender, FormClosingEventArgs e) { }

        private void DGVUseDetect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int _RowIndex = e.RowIndex;
            int _ColIndex = e.ColumnIndex;
            if (_ColIndex == (int)EDGVColName.TestBtn && DGVUseDetect.Rows[_RowIndex].Cells[_ColIndex].Value != null)
            {
                for (int i = 0; i < (int)EDetectName.None; i++)
                {
                    if (DGVUseDetect.Rows[_RowIndex].HeaderCell.Value.ToString() == ((EDetectName)i).ToString())
                        G.Comm.Detect.StartTask((EDetectName)i);
                }
            }
        }
    }
}
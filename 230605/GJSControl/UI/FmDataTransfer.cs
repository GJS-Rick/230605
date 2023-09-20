using CommonLibrary;
using System;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static CommonLibrary.DataTransferDef;

namespace nsUI
{
    public partial class FmDataTransfer : Form
    {
        private enum EDGVColName
        {
            LocalIP,
            RemoteIP,
            User,
            Password,
            Enable,
            DiskLink,
            ALive,
            Test,

            Count
        }

        private string[] _CaptionName;
        private DataTransferInfo[] _RemotePC;
        private DataGridViewRow _NewRows;
        private DataGridViewRow _CaptionNewRows;

        public FmDataTransfer()
        {
            InitializeComponent();

            #region DGV
            #region DGVDataTransfer
            DGVDataTransfer.RowHeadersVisible = true;
            DGVDataTransfer.RowHeadersWidth = 25;
            DGVDataTransfer.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DGVDataTransfer.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            DGVDataTransfer.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            #endregion
            #region DGVDTCaptionName
            DGVDTCaptionName.RowHeadersVisible = true;
            DGVDTCaptionName.RowHeadersWidth = 25;
            DGVDTCaptionName.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DGVDTCaptionName.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            DGVDTCaptionName.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            #endregion
            #endregion

            _CaptionName = new string[0];
            _RemotePC = new DataTransferInfo[0];
            _NewRows = new DataGridViewRow();

            vCreateDGVDTColHdrAndBaseRow();
            vRefreshCaptionObj();

            vCreateDGVColHdrAndBaseRow();
            vRefreshDetectObj();

            TimeStart();
        }

        private void TimeStart() { timer1.Enabled = true; }
        public void TimeStop() { timer1.Enabled = false; }

        private void vCreateDGVColHdrAndBaseRow()
        {
            DataGridViewButtonColumn btnColumn;
            DataGridViewTextBoxColumn textColumn;
            DataGridViewCheckBoxColumn cxbColumn;
            DataTable dtTypeName;

            DGVDataTransfer.ColumnCount = 0;
            DGVDataTransfer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGVDataTransfer.ColumnHeadersHeight = 25;
            DGVDataTransfer.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            for (int i = 0; i < (int)EDGVColName.Count; i++)
            {
                dtTypeName = new DataTable();
                dtTypeName.Columns.Add(((EDGVColName)i).ToString(), typeof(String));

                switch ((EDGVColName)i)
                {
                    case EDGVColName.LocalIP:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        textColumn.ReadOnly = false;
                        textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(textColumn);
                        DGVDataTransfer.Columns[i].Name = "本地位址";
                        DGVDataTransfer.Columns[i].Width = 120;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                        break;
                    case EDGVColName.RemoteIP:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        textColumn.ReadOnly = false;
                        textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(textColumn);
                        DGVDataTransfer.Columns[i].Name = "遠端位址";
                        DGVDataTransfer.Columns[i].Width = 120;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                        break;
                    case EDGVColName.User:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        textColumn.ReadOnly = false;
                        textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(textColumn);
                        DGVDataTransfer.Columns[i].Name = "使用者";
                        DGVDataTransfer.Columns[i].Width = 120;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                        break;
                    case EDGVColName.Password:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        textColumn.ReadOnly = false;
                        textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(textColumn);
                        DGVDataTransfer.Columns[i].Name = "密碼";
                        DGVDataTransfer.Columns[i].Width = 120;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                        break;
                    case EDGVColName.Enable:
                        cxbColumn = new DataGridViewCheckBoxColumn();
                        cxbColumn.ReadOnly = false;
                        cxbColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(cxbColumn);
                        DGVDataTransfer.Columns[i].Name = "啟用";
                        DGVDataTransfer.Columns[i].Width = 60;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;
                    case EDGVColName.DiskLink:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        textColumn.DefaultCellStyle.BackColor = Color.DarkGreen;
                        textColumn.ReadOnly = true;
                        textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(textColumn);
                        DGVDataTransfer.Columns[i].Name = "磁碟狀態";
                        DGVDataTransfer.Columns[i].Width = 100;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;
                    case EDGVColName.ALive:
                        textColumn = new DataGridViewTextBoxColumn();
                        textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        textColumn.DefaultCellStyle.BackColor = Color.DarkGreen;
                        textColumn.ReadOnly = true;
                        textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        DGVDataTransfer.Columns.Add(textColumn);
                        DGVDataTransfer.Columns[i].Name = "連線狀態";
                        DGVDataTransfer.Columns[i].Width = 100;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;
                    case EDGVColName.Test:
                        dtTypeName.Rows.Add("測試");

                        btnColumn = new DataGridViewButtonColumn
                        {
                            UseColumnTextForButtonValue = true,
                            Text = "測試",
                            SortMode = DataGridViewColumnSortMode.NotSortable
                        };

                        DGVDataTransfer.Columns.Add(btnColumn);
                        DGVDataTransfer.Columns[i].Name = "測試";
                        DGVDataTransfer.Columns[i].Width = 60;
                        DGVDataTransfer.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                        break;
                }
            }

            _NewRows = DGVDataTransfer.Rows[0];

            DGVDataTransfer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DGVDataTransfer.AllowUserToAddRows = false;
        }
        private void vCreateDGVDTColHdrAndBaseRow()
        {
            DataGridViewTextBoxColumn textColumn;
            DataTable dtTypeName;

            DGVDTCaptionName.ColumnCount = 0;
            DGVDTCaptionName.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGVDTCaptionName.ColumnHeadersHeight = 25;
            DGVDTCaptionName.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

            dtTypeName = new DataTable();
            dtTypeName.Columns.Add("說明", typeof(String));

            textColumn = new DataGridViewTextBoxColumn();
            textColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            textColumn.ReadOnly = false;
            textColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            DGVDTCaptionName.Columns.Add(textColumn);
            DGVDTCaptionName.Columns[0].Name = "說明";
            DGVDTCaptionName.Columns[0].Width = 120;
            DGVDTCaptionName.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            _CaptionNewRows = DGVDTCaptionName.Rows[0];

            DGVDTCaptionName.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DGVDTCaptionName.AllowUserToAddRows = false;
        }
        private void vRefreshCaptionObj()
        {
            _CaptionName = G.Comm.DataTransfer.GetCaption();

            DGVDTCaptionName.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DGVDTCaptionName.Rows.Clear();

            for (int i = 0; i < (int)EFileCaption.Count; i++)
            {
                if (DGVDTCaptionName.RowCount <= 0)
                    DGVDTCaptionName.Rows.Add(_CaptionNewRows);
                else
                    DGVDTCaptionName.Rows.AddCopy(0);

                DGVDTCaptionName.Rows[i].HeaderCell.Value = ((EFileCaption)i).ToString();

                if (i < _CaptionName.Length)
                    DGVDTCaptionName.Rows[i].Cells[0].Value = _CaptionName[i];
                else
                    DGVDTCaptionName.Rows[i].Cells[0].Value = ((EFileCaption)i).ToString();

                DGVDTCaptionName.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }
        private void vRefreshDetectObj()
        {
            RenewList();

            if (_RemotePC.Length < 1)
                return;

            DGVDataTransfer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DGVDataTransfer.Rows.Clear();

            for (int i = 0; i < _RemotePC.Length; i++)
            {
                if (DGVDataTransfer.RowCount <= 0)
                    DGVDataTransfer.Rows.Add(_NewRows);
                else
                    DGVDataTransfer.Rows.AddCopy(0);

                DGVDataTransfer.Rows[i].HeaderCell.Value = _RemotePC[i].Name;

                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.LocalIP].Value = _RemotePC[i].LocalIP;
                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.RemoteIP].Value = _RemotePC[i].RemoteIP;
                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.User].Value = _RemotePC[i].User;
                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.Password].Value = _RemotePC[i].Password;
                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.Enable].Value = _RemotePC[i].Enable;
                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.DiskLink].Value = "Link";
                DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.ALive].Value = "ALive";
            }
        }

        private void frmDataTransfer_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);
            TabLP_MainLine.Location = new Point(0, 0);
            DGVDataTransfer.Location = new Point(0, 24);

            #region 取得Mac與對應IP
            string[][] _MacAndIP = G.Comm.DataTransfer.GetLocalMac();
            for (int i = 0; i < _MacAndIP.Length; i++)
            {
                if (i == 0)
                {
                    Lbl_Mac1.Text = "Null";
                    Lbl_Mac1.Text = _MacAndIP[0][0];
                    Lbl_IP1.Text = "Null";
                    Lbl_IP1.Text = _MacAndIP[0][1];
                }
                else if (i == 1)
                {
                    Lbl_Mac2.Text = "Null";
                    Lbl_Mac2.Text = _MacAndIP[1][0];
                    Lbl_IP2.Text = "Null";
                    Lbl_IP2.Text = _MacAndIP[1][1];
                }
            }
            #endregion

            if (G.Comm.DataTransfer != null)
                LabLocalPathValue.Text = G.Comm.DataTransfer.GetFolderPath();
            else
                LabLocalPathValue.Text = "Null";

            vRefreshDetectObj();
        }

        private void RenewList() { _RemotePC = G.Comm.DataTransfer.GetInfo(); }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (G.Comm.DataTransfer == null)
                return;

            for (int i = 0; i < DGVDataTransfer.Rows.Count; i++)
            {
                for (int j = 0; j < (int)EDataTransferName.Count; j++)
                {
                    if (DGVDataTransfer.Rows[i].HeaderCell.Value.ToString() == ((EDataTransferName)j).ToString())
                    {
                        if (G.Comm.DataTransfer.IsDiskConnect(((EDataTransferName)j).ToString()))
                            DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.DiskLink].Style.BackColor = Color.LimeGreen;
                        else
                            DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.DiskLink].Style.BackColor = Color.DarkGreen;

                        if (G.Comm.DataTransfer.IsAutoAlive(((EDataTransferName)j).ToString()))
                            DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.ALive].Style.BackColor = Color.LimeGreen;
                        else
                            DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.ALive].Style.BackColor = Color.DarkGreen;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string[] sArrBuf = new string[(int)EFileCaption.Count];
            for (int i = 0; i < (int)EFileCaption.Count; i++)
                sArrBuf[i] = DGVDTCaptionName.Rows[i].Cells[0].Value.ToString();
            G.Comm.DataTransfer.SetCaption(sArrBuf);

            DataTransferInfo[] _InfoBuf = new DataTransferInfo[DGVDataTransfer.RowCount];
            for (int i = 0; i < _InfoBuf.Length; i++)
            {
                _InfoBuf[i].Name = DGVDataTransfer.Rows[i].HeaderCell.Value.ToString();
                _InfoBuf[i].LocalIP = CheckIPStr(DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.LocalIP].Value.ToString());
                _InfoBuf[i].RemoteIP = CheckIPStr(DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.RemoteIP].Value.ToString());
                _InfoBuf[i].User = DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.User].Value.ToString();
                _InfoBuf[i].Password = DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.Password].Value.ToString();
                _InfoBuf[i].Enable = Convert.ToBoolean(DGVDataTransfer.Rows[i].Cells[(int)EDGVColName.Enable].Value);
            }
            G.Comm.DataTransfer.SetInfoBuf(_InfoBuf);

            G.Comm.DataTransfer.Save();
            vRefreshCaptionObj();
            vRefreshDetectObj();
        }

        private string CheckIPStr(string sIP)
        {
            string sResultIP = "";
            string[] sArrIP = sIP.Split('.');

            for (int i = 0; i < 4; i++)
            {
                int iResult = 0;

                if (i < sArrIP.Length)
                {
                    sArrIP[i] = Regex.Replace(sArrIP[i], "[^0-9]", "");

                    if (Int32.TryParse(sArrIP[i], out iResult))
                    {
                        if (iResult > 255) iResult = 255;
                        if (iResult < 0) iResult = 0;
                    }
                    else
                        iResult = 0;
                }

                if (String.IsNullOrEmpty(sResultIP))
                    sResultIP += iResult.ToString();
                else
                    sResultIP += "." + iResult.ToString();
            }

            return sResultIP;
        }

        private void frmDetectSet_FormClosing(object sender, FormClosingEventArgs e) { }

        private void DGVUseDetect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int _RowIndex = e.RowIndex;
            int _ColIndex = e.ColumnIndex;

            if (_ColIndex == (int)EDGVColName.Test && _RowIndex >= 0)
            {
                DGVDataTransfer.Rows[_RowIndex].HeaderCell.Value = _RemotePC[_RowIndex].Name;
                G.Comm.DataTransfer.Send_Test(DGVDataTransfer.Rows[_RowIndex].HeaderCell.Value.ToString());
            }
        }
    }
}
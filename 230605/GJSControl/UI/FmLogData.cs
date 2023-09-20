using System;
using System.Linq;
using System.Windows.Forms;
using VisionLibrary;
using CommonLibrary;
using FileStreamLibrary;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Emgu.CV.Features2D;

namespace nsUI
{
    public partial class FmLogData : Form
    {
        readonly int ReadCountNum = 50;
        private string[] _logFiles;
        private string[] LogFileDataArr;
        private string[] FilteredLogFileDataArr;
        private int Segment;
        private int Residue;

        public FmLogData()
        {
            InitializeComponent();

            vSetClickEvnet();
        }

        #region ButtonEvent
        private void vSetClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(Btn_Click);
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
        #endregion

        private void FmLogData_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);
            dataGridViewLog.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            vRefreshUI();
        }

        private void RefreshDataArr(string LogFilePath)
        {
            LogFileDataArr = G.Comm.Log.Read(LogFilePath);
        }

        public void vRefreshUI()
        {
            _logFiles = G.Comm.Log.GetLogFiles();
            
            RefreshCbx();
        }

        private void RefreshCbx()
        {
            RefreshCbxLogDate();
            RefreshCbxLogMode();
        }

        private void RefreshCbxLogDate()
        {
            string Buf = "";

            //LogDate
            cbxLogDate.Items.Clear();
            foreach (string Logfile in _logFiles)
            {
                string LogDate = Logfile.Substring(0, 8);
                if (Buf != LogDate)
                {
                    try
                    {
                        DateTime FileTime = DateTime.ParseExact(LogDate.Substring(0, 8), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

                        cbxLogDate.Items.Add(LogDate);
                        Buf = LogDate;
                    }
                    catch (Exception) { }
                }
            }

            if (cbxLogDate.Items.Count > 0)
                cbxLogDate.SelectedIndex = cbxLogDate.Items.Count - 1;
        }
        private void RefreshCbxLogMode()
        {
            cbxLogMode.Items.Clear();
            string Buf = "";
            bool _find = false;

            Array.Reverse(_logFiles);

            foreach (string Logfile in _logFiles)
            {
                if (cbxLogDate.Text == Logfile.Substring(0, 8))
                {
                    try
                    {
                        _find = true;
                        DateTime FileTime = DateTime.ParseExact(Logfile.Substring(0, 8), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

                        string LogMode = Logfile.Substring(8, Logfile.Length - 8).Split('.')[0];
                        if (Buf != LogMode)
                        {
                            cbxLogMode.Items.Add(LogMode);
                            Buf = LogMode;
                        }
                    }
                    catch (Exception) { }
                }
                else
                    if (_find) break;
            }
            if (cbxLogMode.Items.Count > 0)
                cbxLogMode.SelectedIndex = 0;
        }
        private void RefreshCbxDataType1()
        {
            cbxDataType1.Items.Clear();
            cbxDataType1.Items.Add("All");

            foreach (string strTemp in LogFileDataArr)
            {
                bool Found = false;
                string TempType = Regex.Split(strTemp, "---")[1];
                foreach (string ItemStr in cbxDataType1.Items)
                {
                    if (ItemStr == TempType)
                    {
                        Found = true;
                        break;
                    }
                }

                if (!Found & TempType != "" & TempType != null)
                    cbxDataType1.Items.Add(TempType);
            }

            cbxDataType1.SelectedIndex = 0;
        }

        private void RefreshViewLog()
        {
            if (cbxLogDate.SelectedIndex < 0)
            {
                RefreshCbx();
            }
            else
            {
                if (cbxLogMode.SelectedIndex < 0)
                    cbxLogMode.SelectedIndex = 0;
            }

            string LogFilePath = cbxLogDate.Text.Substring(0, 6) + "\\" + cbxLogDate.Text.Substring(6, 2) + cbxLogMode.Text;
            RefreshDataArr(LogFilePath);
           // Array.Sort(LogFileDataArr);
            Array.Reverse(LogFileDataArr);

            RefreshCbxDataType1();

            ShowDataToViewLog();
        }

        private void ShowDataToViewLog()
        {
            string[] _strarr;

            dataGridViewLog.Rows.Clear();

            switch (cbxDataType1.Text)
            {
                case "All":
                    {
                        FilteredLogFileDataArr = LogFileDataArr;
                    }
                    break;

                default:
                    {
                        List<string> LogDataTemp = new List<string>();

                        for (int i = 0; i < LogFileDataArr.Length; i++)
                        {
                            _strarr = Regex.Split(LogFileDataArr[i], "---");

                            if (cbxDataType1.Text == _strarr[1])
                                LogDataTemp.Add(LogFileDataArr[i]);
                        }

                        FilteredLogFileDataArr = LogDataTemp.ToArray();
                    }
                    break;
            }

            RenewHeaderText();

            for (int i = 0; i < Segment * ReadCountNum + Residue; i++)
            {
                if (i < ReadCountNum)
                {
                    _strarr = Regex.Split(FilteredLogFileDataArr[i], "---");

                    for (int j = 0; j < _strarr.Length; j++)
                        dataGridViewLog.Rows[i].Cells[j].Value = _strarr[j];
                }
                else
                    break;
            }
        }

        /// <summary>讀取確認行數並更新[HeaderText]名稱</summary>
        private void RenewHeaderText()
        {
            Residue = FilteredLogFileDataArr.Length % ReadCountNum;
            Segment = (FilteredLogFileDataArr.Length - Residue) / ReadCountNum;

            if (Residue == 0 && Segment == 0)
            {
                dataGridViewLog.RowCount = 1;
                dataGridViewLog.Columns[3].HeaderText = "Data";
            }
            else
            {
                if (Segment > 0)
                    dataGridViewLog.RowCount = ReadCountNum;
                else
                    dataGridViewLog.RowCount = Residue;

                dataGridViewLog.Columns[3].HeaderText = "Data (" + FilteredLogFileDataArr.Length + ")";
            }
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Log");
        }

        private void cbxLogDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCbxLogMode();
        }

        private void cbxLogMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshViewLog();
        }

        private void cbxDataType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDataToViewLog();
        }

        private void BtnSync_Click(object sender, EventArgs e)
        {
            vRefreshUI();
        }

        private void ScrollReader(object sender, ScrollEventArgs e)
        {
            if (e.NewValue + dataGridViewLog.DisplayedRowCount(false) >= dataGridViewLog.RowCount)
            {
                if (FilteredLogFileDataArr.Length != 0)
                {
                    string[] _strarr;
                    int ResidueBuf = dataGridViewLog.RowCount % ReadCountNum;
                    int SegmentBuf = (dataGridViewLog.RowCount - ResidueBuf) / ReadCountNum;

                    if (dataGridViewLog.RowCount + ReadCountNum > FilteredLogFileDataArr.Length)
                        dataGridViewLog.RowCount = FilteredLogFileDataArr.Length;
                    else
                        dataGridViewLog.RowCount += ReadCountNum;

                    for (int i = (SegmentBuf * ReadCountNum + ResidueBuf - 1); i < Segment * ReadCountNum + Residue; i++)
                    {
                        if (i < dataGridViewLog.RowCount)
                        {
                            _strarr = Regex.Split(FilteredLogFileDataArr[i], "---");

                            for (int j = 0; j < _strarr.Length; j++)
                                dataGridViewLog.Rows[i].Cells[j].Value = _strarr[j];
                        }
                        else
                            break;
                    }
                }
            }
        }
    }
}

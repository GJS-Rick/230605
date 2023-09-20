namespace nsFmMotion
{
    partial class FmMonitorSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Lbl_FmName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.DGVUseMonitor = new System.Windows.Forms.DataGridView();
            this.BtnMonitorAdd = new System.Windows.Forms.Button();
            this.BtnMonitorRemove = new System.Windows.Forms.Button();
            this.Lbl_PortName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Lbl_StopBits = new System.Windows.Forms.Label();
            this.CB_StopBits = new System.Windows.Forms.ComboBox();
            this.Lbl_DataBits = new System.Windows.Forms.Label();
            this.CB_DataBits = new System.Windows.Forms.ComboBox();
            this.Lbl_Parity = new System.Windows.Forms.Label();
            this.CB_Parity = new System.Windows.Forms.ComboBox();
            this.CB_BaudRate = new System.Windows.Forms.ComboBox();
            this.Lbl_BaudRate = new System.Windows.Forms.Label();
            this.CB_PortName = new System.Windows.Forms.ComboBox();
            this.Lbl_StationNum = new System.Windows.Forms.Label();
            this.CB_StationNum = new System.Windows.Forms.ComboBox();
            this.BtnRenew = new System.Windows.Forms.Button();
            this.Lbl_MonitorName = new System.Windows.Forms.Label();
            this.CB_MonitorName = new System.Windows.Forms.ComboBox();
            this.Lbl_DelayTime = new System.Windows.Forms.Label();
            this.CB_DelayTime = new System.Windows.Forms.ComboBox();
            this.BtnSave_S = new System.Windows.Forms.Button();
            this.DGV_MonitorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_ComPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_StationNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_BaudRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Parity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_DataBits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_StopBits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_DelayTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGVUseMonitor)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lbl_FmName
            // 
            this.Lbl_FmName.AutoSize = true;
            this.Lbl_FmName.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_FmName.Location = new System.Drawing.Point(1, 1);
            this.Lbl_FmName.Name = "Lbl_FmName";
            this.Lbl_FmName.Size = new System.Drawing.Size(129, 19);
            this.Lbl_FmName.TabIndex = 20;
            this.Lbl_FmName.Text = "監控元件設定";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DGVUseMonitor
            // 
            this.DGVUseMonitor.AllowUserToAddRows = false;
            this.DGVUseMonitor.AllowUserToResizeColumns = false;
            this.DGVUseMonitor.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVUseMonitor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVUseMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVUseMonitor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVUseMonitor.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DGVUseMonitor.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVUseMonitor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DGVUseMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVUseMonitor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGV_MonitorName,
            this.DGV_ComPort,
            this.DGV_StationNum,
            this.DGV_BaudRate,
            this.DGV_Parity,
            this.DGV_DataBits,
            this.DGV_StopBits,
            this.DGV_DelayTime});
            this.DGVUseMonitor.Location = new System.Drawing.Point(2, 30);
            this.DGVUseMonitor.Margin = new System.Windows.Forms.Padding(2);
            this.DGVUseMonitor.MultiSelect = false;
            this.DGVUseMonitor.Name = "DGVUseMonitor";
            this.DGVUseMonitor.ReadOnly = true;
            this.DGVUseMonitor.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVUseMonitor.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.DGVUseMonitor.RowHeadersVisible = false;
            this.DGVUseMonitor.RowHeadersWidth = 40;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVUseMonitor.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.DGVUseMonitor.RowTemplate.Height = 24;
            this.DGVUseMonitor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVUseMonitor.Size = new System.Drawing.Size(300, 424);
            this.DGVUseMonitor.TabIndex = 21;
            this.DGVUseMonitor.Click += new System.EventHandler(this.DGVUseMonitor_Click);
            // 
            // BtnMonitorAdd
            // 
            this.BtnMonitorAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMonitorAdd.Location = new System.Drawing.Point(2, 178);
            this.BtnMonitorAdd.Margin = new System.Windows.Forms.Padding(2);
            this.BtnMonitorAdd.Name = "BtnMonitorAdd";
            this.BtnMonitorAdd.Size = new System.Drawing.Size(40, 40);
            this.BtnMonitorAdd.TabIndex = 22;
            this.BtnMonitorAdd.Text = "+";
            this.BtnMonitorAdd.UseVisualStyleBackColor = true;
            this.BtnMonitorAdd.Click += new System.EventHandler(this.BtnMonitorAdd_Click);
            // 
            // BtnMonitorRemove
            // 
            this.BtnMonitorRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMonitorRemove.Location = new System.Drawing.Point(2, 222);
            this.BtnMonitorRemove.Margin = new System.Windows.Forms.Padding(2);
            this.BtnMonitorRemove.Name = "BtnMonitorRemove";
            this.BtnMonitorRemove.Size = new System.Drawing.Size(40, 40);
            this.BtnMonitorRemove.TabIndex = 22;
            this.BtnMonitorRemove.Text = "-";
            this.BtnMonitorRemove.UseVisualStyleBackColor = true;
            this.BtnMonitorRemove.Click += new System.EventHandler(this.BtnMonitorRemove_Click);
            // 
            // Lbl_PortName
            // 
            this.Lbl_PortName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_PortName.AutoSize = true;
            this.Lbl_PortName.Location = new System.Drawing.Point(46, 102);
            this.Lbl_PortName.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_PortName.Name = "Lbl_PortName";
            this.Lbl_PortName.Size = new System.Drawing.Size(96, 16);
            this.Lbl_PortName.TabIndex = 23;
            this.Lbl_PortName.Text = "通訊埠：";
            this.Lbl_PortName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Lbl_StopBits, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.CB_StopBits, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_DataBits, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.CB_DataBits, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_Parity, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.CB_Parity, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.CB_BaudRate, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_BaudRate, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_PortName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.CB_PortName, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_StationNum, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.CB_StationNum, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnRenew, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_MonitorName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CB_MonitorName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Lbl_DelayTime, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.CB_DelayTime, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.BtnMonitorAdd, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.BtnMonitorRemove, 0, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(307, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49918F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49917F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49917F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49917F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49917F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.49918F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50167F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.50328F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(328, 360);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // Lbl_StopBits
            // 
            this.Lbl_StopBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_StopBits.AutoSize = true;
            this.Lbl_StopBits.Location = new System.Drawing.Point(46, 278);
            this.Lbl_StopBits.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_StopBits.Name = "Lbl_StopBits";
            this.Lbl_StopBits.Size = new System.Drawing.Size(96, 16);
            this.Lbl_StopBits.TabIndex = 23;
            this.Lbl_StopBits.Text = "停止位元：";
            this.Lbl_StopBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_StopBits
            // 
            this.CB_StopBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_StopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_StopBits.FormattingEnabled = true;
            this.CB_StopBits.Location = new System.Drawing.Point(146, 276);
            this.CB_StopBits.Margin = new System.Windows.Forms.Padding(2);
            this.CB_StopBits.Name = "CB_StopBits";
            this.CB_StopBits.Size = new System.Drawing.Size(180, 24);
            this.CB_StopBits.TabIndex = 25;
            this.CB_StopBits.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // Lbl_DataBits
            // 
            this.Lbl_DataBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_DataBits.AutoSize = true;
            this.Lbl_DataBits.Location = new System.Drawing.Point(46, 234);
            this.Lbl_DataBits.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_DataBits.Name = "Lbl_DataBits";
            this.Lbl_DataBits.Size = new System.Drawing.Size(96, 16);
            this.Lbl_DataBits.TabIndex = 23;
            this.Lbl_DataBits.Text = "資料長度：";
            this.Lbl_DataBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_DataBits
            // 
            this.CB_DataBits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_DataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_DataBits.FormattingEnabled = true;
            this.CB_DataBits.Location = new System.Drawing.Point(146, 232);
            this.CB_DataBits.Margin = new System.Windows.Forms.Padding(2);
            this.CB_DataBits.Name = "CB_DataBits";
            this.CB_DataBits.Size = new System.Drawing.Size(180, 24);
            this.CB_DataBits.TabIndex = 25;
            this.CB_DataBits.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // Lbl_Parity
            // 
            this.Lbl_Parity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_Parity.AutoSize = true;
            this.Lbl_Parity.Location = new System.Drawing.Point(46, 190);
            this.Lbl_Parity.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_Parity.Name = "Lbl_Parity";
            this.Lbl_Parity.Size = new System.Drawing.Size(96, 16);
            this.Lbl_Parity.TabIndex = 23;
            this.Lbl_Parity.Text = "同位檢查：";
            this.Lbl_Parity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Parity
            // 
            this.CB_Parity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Parity.FormattingEnabled = true;
            this.CB_Parity.Location = new System.Drawing.Point(146, 188);
            this.CB_Parity.Margin = new System.Windows.Forms.Padding(2);
            this.CB_Parity.Name = "CB_Parity";
            this.CB_Parity.Size = new System.Drawing.Size(180, 24);
            this.CB_Parity.TabIndex = 25;
            this.CB_Parity.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // CB_BaudRate
            // 
            this.CB_BaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_BaudRate.FormattingEnabled = true;
            this.CB_BaudRate.Location = new System.Drawing.Point(146, 144);
            this.CB_BaudRate.Margin = new System.Windows.Forms.Padding(2);
            this.CB_BaudRate.Name = "CB_BaudRate";
            this.CB_BaudRate.Size = new System.Drawing.Size(180, 24);
            this.CB_BaudRate.TabIndex = 25;
            this.CB_BaudRate.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // Lbl_BaudRate
            // 
            this.Lbl_BaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_BaudRate.AutoSize = true;
            this.Lbl_BaudRate.Location = new System.Drawing.Point(46, 146);
            this.Lbl_BaudRate.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_BaudRate.Name = "Lbl_BaudRate";
            this.Lbl_BaudRate.Size = new System.Drawing.Size(96, 16);
            this.Lbl_BaudRate.TabIndex = 23;
            this.Lbl_BaudRate.Text = "鮑率：";
            this.Lbl_BaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_PortName
            // 
            this.CB_PortName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_PortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_PortName.FormattingEnabled = true;
            this.CB_PortName.Location = new System.Drawing.Point(146, 100);
            this.CB_PortName.Margin = new System.Windows.Forms.Padding(2);
            this.CB_PortName.Name = "CB_PortName";
            this.CB_PortName.Size = new System.Drawing.Size(180, 24);
            this.CB_PortName.TabIndex = 25;
            this.CB_PortName.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            this.CB_PortName.Click += new System.EventHandler(this.BtnReloadPort);
            // 
            // Lbl_StationNum
            // 
            this.Lbl_StationNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_StationNum.AutoSize = true;
            this.Lbl_StationNum.Location = new System.Drawing.Point(46, 58);
            this.Lbl_StationNum.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_StationNum.Name = "Lbl_StationNum";
            this.Lbl_StationNum.Size = new System.Drawing.Size(96, 16);
            this.Lbl_StationNum.TabIndex = 23;
            this.Lbl_StationNum.Text = "站號：";
            this.Lbl_StationNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_StationNum
            // 
            this.CB_StationNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_StationNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_StationNum.FormattingEnabled = true;
            this.CB_StationNum.Location = new System.Drawing.Point(146, 56);
            this.CB_StationNum.Margin = new System.Windows.Forms.Padding(2);
            this.CB_StationNum.Name = "CB_StationNum";
            this.CB_StationNum.Size = new System.Drawing.Size(180, 24);
            this.CB_StationNum.TabIndex = 25;
            this.CB_StationNum.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // BtnRenew
            // 
            this.BtnRenew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRenew.Image = global::GJSControl.Properties.Resources.Sync_S;
            this.BtnRenew.Location = new System.Drawing.Point(2, 90);
            this.BtnRenew.Margin = new System.Windows.Forms.Padding(2);
            this.BtnRenew.Name = "BtnRenew";
            this.BtnRenew.Size = new System.Drawing.Size(40, 40);
            this.BtnRenew.TabIndex = 22;
            this.BtnRenew.UseVisualStyleBackColor = true;
            this.BtnRenew.Click += new System.EventHandler(this.BtnReloadPort);
            // 
            // Lbl_MonitorName
            // 
            this.Lbl_MonitorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_MonitorName.AutoSize = true;
            this.Lbl_MonitorName.Location = new System.Drawing.Point(46, 14);
            this.Lbl_MonitorName.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_MonitorName.Name = "Lbl_MonitorName";
            this.Lbl_MonitorName.Size = new System.Drawing.Size(96, 16);
            this.Lbl_MonitorName.TabIndex = 23;
            this.Lbl_MonitorName.Text = "元件名稱：";
            this.Lbl_MonitorName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_MonitorName
            // 
            this.CB_MonitorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_MonitorName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_MonitorName.FormattingEnabled = true;
            this.CB_MonitorName.Location = new System.Drawing.Point(146, 12);
            this.CB_MonitorName.Margin = new System.Windows.Forms.Padding(2);
            this.CB_MonitorName.Name = "CB_MonitorName";
            this.CB_MonitorName.Size = new System.Drawing.Size(180, 24);
            this.CB_MonitorName.TabIndex = 25;
            this.CB_MonitorName.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // Lbl_DelayTime
            // 
            this.Lbl_DelayTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_DelayTime.AutoSize = true;
            this.Lbl_DelayTime.Location = new System.Drawing.Point(46, 326);
            this.Lbl_DelayTime.Margin = new System.Windows.Forms.Padding(2);
            this.Lbl_DelayTime.Name = "Lbl_DelayTime";
            this.Lbl_DelayTime.Size = new System.Drawing.Size(96, 16);
            this.Lbl_DelayTime.TabIndex = 23;
            this.Lbl_DelayTime.Text = "延遲時間：";
            this.Lbl_DelayTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_DelayTime
            // 
            this.CB_DelayTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_DelayTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_DelayTime.FormattingEnabled = true;
            this.CB_DelayTime.Location = new System.Drawing.Point(146, 324);
            this.CB_DelayTime.Margin = new System.Windows.Forms.Padding(2);
            this.CB_DelayTime.Name = "CB_DelayTime";
            this.CB_DelayTime.Size = new System.Drawing.Size(180, 24);
            this.CB_DelayTime.TabIndex = 25;
            this.CB_DelayTime.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged);
            // 
            // BtnSave_S
            // 
            this.BtnSave_S.Image = global::GJSControl.Properties.Resources.Save;
            this.BtnSave_S.Location = new System.Drawing.Point(575, 396);
            this.BtnSave_S.Name = "BtnSave_S";
            this.BtnSave_S.Size = new System.Drawing.Size(60, 60);
            this.BtnSave_S.TabIndex = 15;
            this.BtnSave_S.UseVisualStyleBackColor = true;
            this.BtnSave_S.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // DGV_MonitorName
            // 
            this.DGV_MonitorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DGV_MonitorName.HeaderText = "監控名稱";
            this.DGV_MonitorName.Name = "DGV_MonitorName";
            this.DGV_MonitorName.ReadOnly = true;
            this.DGV_MonitorName.Width = 96;
            // 
            // DGV_ComPort
            // 
            this.DGV_ComPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGV_ComPort.DefaultCellStyle = dataGridViewCellStyle3;
            this.DGV_ComPort.HeaderText = "COM";
            this.DGV_ComPort.Name = "DGV_ComPort";
            this.DGV_ComPort.ReadOnly = true;
            this.DGV_ComPort.Width = 66;
            // 
            // DGV_StationNum
            // 
            this.DGV_StationNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGV_StationNum.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV_StationNum.HeaderText = "站號";
            this.DGV_StationNum.Name = "DGV_StationNum";
            this.DGV_StationNum.ReadOnly = true;
            this.DGV_StationNum.Width = 64;
            // 
            // DGV_BaudRate
            // 
            this.DGV_BaudRate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DGV_BaudRate.HeaderText = "鮑率";
            this.DGV_BaudRate.Name = "DGV_BaudRate";
            this.DGV_BaudRate.ReadOnly = true;
            this.DGV_BaudRate.Width = 64;
            // 
            // DGV_Parity
            // 
            this.DGV_Parity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DGV_Parity.HeaderText = "同位檢查";
            this.DGV_Parity.Name = "DGV_Parity";
            this.DGV_Parity.ReadOnly = true;
            this.DGV_Parity.Width = 96;
            // 
            // DGV_DataBits
            // 
            this.DGV_DataBits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DGV_DataBits.HeaderText = "資料長度";
            this.DGV_DataBits.Name = "DGV_DataBits";
            this.DGV_DataBits.ReadOnly = true;
            this.DGV_DataBits.Width = 96;
            // 
            // DGV_StopBits
            // 
            this.DGV_StopBits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DGV_StopBits.HeaderText = "停止位元";
            this.DGV_StopBits.Name = "DGV_StopBits";
            this.DGV_StopBits.ReadOnly = true;
            this.DGV_StopBits.Width = 96;
            // 
            // DGV_DelayTime
            // 
            this.DGV_DelayTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DGV_DelayTime.HeaderText = "延遲時間";
            this.DGV_DelayTime.Name = "DGV_DelayTime";
            this.DGV_DelayTime.ReadOnly = true;
            this.DGV_DelayTime.Width = 96;
            // 
            // FmMonitorSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.DGVUseMonitor);
            this.Controls.Add(this.Lbl_FmName);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmMonitorSet";
            this.Text = "Monitor Set";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmModSet_FormClosing);
            this.Load += new System.EventHandler(this.frmMonitorSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVUseMonitor)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Lbl_FmName;
        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView DGVUseMonitor;
        private System.Windows.Forms.Button BtnMonitorAdd;
        private System.Windows.Forms.Button BtnMonitorRemove;
        private System.Windows.Forms.Label Lbl_PortName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label Lbl_BaudRate;
        private System.Windows.Forms.Label Lbl_Parity;
        private System.Windows.Forms.Label Lbl_DataBits;
        private System.Windows.Forms.Label Lbl_StopBits;
        private System.Windows.Forms.Label Lbl_StationNum;
        private System.Windows.Forms.ComboBox CB_PortName;
        private System.Windows.Forms.ComboBox CB_BaudRate;
        private System.Windows.Forms.ComboBox CB_Parity;
        private System.Windows.Forms.ComboBox CB_DataBits;
        private System.Windows.Forms.ComboBox CB_StopBits;
        private System.Windows.Forms.ComboBox CB_StationNum;
        private System.Windows.Forms.Button BtnRenew;
        private System.Windows.Forms.Label Lbl_MonitorName;
        private System.Windows.Forms.ComboBox CB_MonitorName;
        private System.Windows.Forms.Label Lbl_DelayTime;
        private System.Windows.Forms.ComboBox CB_DelayTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_MonitorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_ComPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_StationNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_BaudRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_Parity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_DataBits;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_StopBits;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGV_DelayTime;
    }
}
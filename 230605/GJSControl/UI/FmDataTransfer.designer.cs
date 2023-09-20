namespace nsUI
{
    partial class FmDataTransfer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Lbl_FmName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.DGVDataTransfer = new System.Windows.Forms.DataGridView();
            this.BtnSave_S = new System.Windows.Forms.Button();
            this.TabCtlDataTransfer = new System.Windows.Forms.TabControl();
            this.MainLine_GJS = new System.Windows.Forms.TabPage();
            this.TabLP_MainLine = new System.Windows.Forms.TableLayoutPanel();
            this.LabLocalPathValue = new System.Windows.Forms.Label();
            this.LabLocalPath = new System.Windows.Forms.Label();
            this.Caption = new System.Windows.Forms.TabPage();
            this.DGVDTCaptionName = new System.Windows.Forms.DataGridView();
            this.Lbl_Mac1 = new System.Windows.Forms.Label();
            this.Lbl_IP1 = new System.Windows.Forms.Label();
            this.Lbl_Mac2 = new System.Windows.Forms.Label();
            this.Lbl_IP2 = new System.Windows.Forms.Label();
            this.TabLP_Mac = new System.Windows.Forms.TableLayoutPanel();
            this.Lbl_Mac = new System.Windows.Forms.Label();
            this.Lbl_PosNo1 = new System.Windows.Forms.Label();
            this.Lbl_PosNo2 = new System.Windows.Forms.Label();
            this.Lbl_IP = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGVDataTransfer)).BeginInit();
            this.TabCtlDataTransfer.SuspendLayout();
            this.MainLine_GJS.SuspendLayout();
            this.TabLP_MainLine.SuspendLayout();
            this.Caption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVDTCaptionName)).BeginInit();
            this.TabLP_Mac.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lbl_FmName
            // 
            this.Lbl_FmName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Lbl_FmName.AutoSize = true;
            this.Lbl_FmName.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_FmName.Location = new System.Drawing.Point(1, 1);
            this.Lbl_FmName.Name = "Lbl_FmName";
            this.Lbl_FmName.Size = new System.Drawing.Size(89, 19);
            this.Lbl_FmName.TabIndex = 20;
            this.Lbl_FmName.Text = "數據傳輸";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DGVDataTransfer
            // 
            this.DGVDataTransfer.AllowUserToResizeColumns = false;
            this.DGVDataTransfer.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVDataTransfer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVDataTransfer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DGVDataTransfer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVDataTransfer.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DGVDataTransfer.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVDataTransfer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DGVDataTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVDataTransfer.Location = new System.Drawing.Point(0, 24);
            this.DGVDataTransfer.Margin = new System.Windows.Forms.Padding(2);
            this.DGVDataTransfer.MultiSelect = false;
            this.DGVDataTransfer.Name = "DGVDataTransfer";
            this.DGVDataTransfer.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVDataTransfer.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGVDataTransfer.RowHeadersVisible = false;
            this.DGVDataTransfer.RowHeadersWidth = 40;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVDataTransfer.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.DGVDataTransfer.RowTemplate.Height = 24;
            this.DGVDataTransfer.Size = new System.Drawing.Size(622, 316);
            this.DGVDataTransfer.TabIndex = 21;
            this.DGVDataTransfer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVUseDetect_CellContentClick);
            // 
            // BtnSave_S
            // 
            this.BtnSave_S.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnSave_S.Image = global::GJSControl.Properties.Resources.Save;
            this.BtnSave_S.Location = new System.Drawing.Point(575, 396);
            this.BtnSave_S.Name = "BtnSave_S";
            this.BtnSave_S.Size = new System.Drawing.Size(60, 60);
            this.BtnSave_S.TabIndex = 15;
            this.BtnSave_S.UseVisualStyleBackColor = true;
            this.BtnSave_S.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // TabCtlDataTransfer
            // 
            this.TabCtlDataTransfer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TabCtlDataTransfer.Controls.Add(this.MainLine_GJS);
            this.TabCtlDataTransfer.Controls.Add(this.Caption);
            this.TabCtlDataTransfer.Location = new System.Drawing.Point(2, 20);
            this.TabCtlDataTransfer.Multiline = true;
            this.TabCtlDataTransfer.Name = "TabCtlDataTransfer";
            this.TabCtlDataTransfer.SelectedIndex = 0;
            this.TabCtlDataTransfer.Size = new System.Drawing.Size(630, 370);
            this.TabCtlDataTransfer.TabIndex = 22;
            // 
            // MainLine_GJS
            // 
            this.MainLine_GJS.Controls.Add(this.TabLP_MainLine);
            this.MainLine_GJS.Controls.Add(this.DGVDataTransfer);
            this.MainLine_GJS.Location = new System.Drawing.Point(4, 26);
            this.MainLine_GJS.Name = "MainLine_GJS";
            this.MainLine_GJS.Padding = new System.Windows.Forms.Padding(3);
            this.MainLine_GJS.Size = new System.Drawing.Size(622, 340);
            this.MainLine_GJS.TabIndex = 0;
            this.MainLine_GJS.Text = "主線(GJS)";
            this.MainLine_GJS.UseVisualStyleBackColor = true;
            // 
            // TabLP_MainLine
            // 
            this.TabLP_MainLine.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TabLP_MainLine.ColumnCount = 2;
            this.TabLP_MainLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.TabLP_MainLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_MainLine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_MainLine.Controls.Add(this.LabLocalPathValue, 1, 0);
            this.TabLP_MainLine.Controls.Add(this.LabLocalPath, 0, 0);
            this.TabLP_MainLine.Location = new System.Drawing.Point(0, 0);
            this.TabLP_MainLine.Name = "TabLP_MainLine";
            this.TabLP_MainLine.RowCount = 1;
            this.TabLP_MainLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_MainLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TabLP_MainLine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TabLP_MainLine.Size = new System.Drawing.Size(622, 24);
            this.TabLP_MainLine.TabIndex = 23;
            // 
            // LabLocalPathValue
            // 
            this.LabLocalPathValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabLocalPathValue.AutoSize = true;
            this.LabLocalPathValue.Location = new System.Drawing.Point(123, 4);
            this.LabLocalPathValue.Name = "LabLocalPathValue";
            this.LabLocalPathValue.Size = new System.Drawing.Size(496, 16);
            this.LabLocalPathValue.TabIndex = 23;
            this.LabLocalPathValue.Text = "C:\\DataTransfer";
            this.LabLocalPathValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabLocalPath
            // 
            this.LabLocalPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabLocalPath.AutoSize = true;
            this.LabLocalPath.Location = new System.Drawing.Point(3, 4);
            this.LabLocalPath.Name = "LabLocalPath";
            this.LabLocalPath.Size = new System.Drawing.Size(114, 16);
            this.LabLocalPath.TabIndex = 23;
            this.LabLocalPath.Text = "本地位置";
            this.LabLocalPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Caption
            // 
            this.Caption.Controls.Add(this.DGVDTCaptionName);
            this.Caption.Location = new System.Drawing.Point(4, 26);
            this.Caption.Name = "Caption";
            this.Caption.Size = new System.Drawing.Size(622, 340);
            this.Caption.TabIndex = 1;
            this.Caption.Text = "註解說明";
            this.Caption.UseVisualStyleBackColor = true;
            // 
            // DGVDTCaptionName
            // 
            this.DGVDTCaptionName.AllowUserToResizeColumns = false;
            this.DGVDTCaptionName.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVDTCaptionName.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.DGVDTCaptionName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DGVDTCaptionName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVDTCaptionName.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DGVDTCaptionName.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVDTCaptionName.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DGVDTCaptionName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVDTCaptionName.Location = new System.Drawing.Point(0, 0);
            this.DGVDTCaptionName.Margin = new System.Windows.Forms.Padding(2);
            this.DGVDTCaptionName.MultiSelect = false;
            this.DGVDTCaptionName.Name = "DGVDTCaptionName";
            this.DGVDTCaptionName.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVDTCaptionName.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.DGVDTCaptionName.RowHeadersVisible = false;
            this.DGVDTCaptionName.RowHeadersWidth = 40;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVDTCaptionName.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.DGVDTCaptionName.RowTemplate.Height = 24;
            this.DGVDTCaptionName.Size = new System.Drawing.Size(622, 340);
            this.DGVDTCaptionName.TabIndex = 26;
            // 
            // Lbl_Mac1
            // 
            this.Lbl_Mac1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_Mac1.AutoSize = true;
            this.Lbl_Mac1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Mac1.Location = new System.Drawing.Point(30, 24);
            this.Lbl_Mac1.Name = "Lbl_Mac1";
            this.Lbl_Mac1.Size = new System.Drawing.Size(161, 16);
            this.Lbl_Mac1.TabIndex = 24;
            this.Lbl_Mac1.Text = "Null";
            this.Lbl_Mac1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_IP1
            // 
            this.Lbl_IP1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_IP1.AutoSize = true;
            this.Lbl_IP1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_IP1.Location = new System.Drawing.Point(198, 24);
            this.Lbl_IP1.Name = "Lbl_IP1";
            this.Lbl_IP1.Size = new System.Drawing.Size(138, 16);
            this.Lbl_IP1.TabIndex = 24;
            this.Lbl_IP1.Text = "Null";
            this.Lbl_IP1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_Mac2
            // 
            this.Lbl_Mac2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_Mac2.AutoSize = true;
            this.Lbl_Mac2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Mac2.Location = new System.Drawing.Point(30, 45);
            this.Lbl_Mac2.Name = "Lbl_Mac2";
            this.Lbl_Mac2.Size = new System.Drawing.Size(161, 16);
            this.Lbl_Mac2.TabIndex = 24;
            this.Lbl_Mac2.Text = "Null";
            this.Lbl_Mac2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_IP2
            // 
            this.Lbl_IP2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_IP2.AutoSize = true;
            this.Lbl_IP2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_IP2.Location = new System.Drawing.Point(198, 45);
            this.Lbl_IP2.Name = "Lbl_IP2";
            this.Lbl_IP2.Size = new System.Drawing.Size(138, 16);
            this.Lbl_IP2.TabIndex = 24;
            this.Lbl_IP2.Text = "Null";
            this.Lbl_IP2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TabLP_Mac
            // 
            this.TabLP_Mac.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TabLP_Mac.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.TabLP_Mac.ColumnCount = 3;
            this.TabLP_Mac.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.TabLP_Mac.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_Mac.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.TabLP_Mac.Controls.Add(this.Lbl_Mac, 1, 0);
            this.TabLP_Mac.Controls.Add(this.Lbl_IP1, 2, 1);
            this.TabLP_Mac.Controls.Add(this.Lbl_PosNo1, 0, 1);
            this.TabLP_Mac.Controls.Add(this.Lbl_IP2, 2, 2);
            this.TabLP_Mac.Controls.Add(this.Lbl_Mac1, 1, 1);
            this.TabLP_Mac.Controls.Add(this.Lbl_Mac2, 1, 2);
            this.TabLP_Mac.Controls.Add(this.Lbl_PosNo2, 0, 2);
            this.TabLP_Mac.Controls.Add(this.Lbl_IP, 2, 0);
            this.TabLP_Mac.Location = new System.Drawing.Point(6, 390);
            this.TabLP_Mac.Name = "TabLP_Mac";
            this.TabLP_Mac.RowCount = 3;
            this.TabLP_Mac.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TabLP_Mac.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TabLP_Mac.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.TabLP_Mac.Size = new System.Drawing.Size(340, 64);
            this.TabLP_Mac.TabIndex = 25;
            // 
            // Lbl_Mac
            // 
            this.Lbl_Mac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_Mac.AutoSize = true;
            this.Lbl_Mac.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Mac.Location = new System.Drawing.Point(30, 3);
            this.Lbl_Mac.Name = "Lbl_Mac";
            this.Lbl_Mac.Size = new System.Drawing.Size(161, 16);
            this.Lbl_Mac.TabIndex = 26;
            this.Lbl_Mac.Text = "MAC";
            this.Lbl_Mac.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_PosNo1
            // 
            this.Lbl_PosNo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_PosNo1.AutoSize = true;
            this.Lbl_PosNo1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_PosNo1.Location = new System.Drawing.Point(4, 24);
            this.Lbl_PosNo1.Name = "Lbl_PosNo1";
            this.Lbl_PosNo1.Size = new System.Drawing.Size(19, 16);
            this.Lbl_PosNo1.TabIndex = 23;
            this.Lbl_PosNo1.Text = "1";
            this.Lbl_PosNo1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Lbl_PosNo2
            // 
            this.Lbl_PosNo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_PosNo2.AutoSize = true;
            this.Lbl_PosNo2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_PosNo2.Location = new System.Drawing.Point(4, 45);
            this.Lbl_PosNo2.Name = "Lbl_PosNo2";
            this.Lbl_PosNo2.Size = new System.Drawing.Size(19, 16);
            this.Lbl_PosNo2.TabIndex = 23;
            this.Lbl_PosNo2.Text = "2";
            this.Lbl_PosNo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Lbl_IP
            // 
            this.Lbl_IP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_IP.AutoSize = true;
            this.Lbl_IP.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_IP.Location = new System.Drawing.Point(198, 3);
            this.Lbl_IP.Name = "Lbl_IP";
            this.Lbl_IP.Size = new System.Drawing.Size(138, 16);
            this.Lbl_IP.TabIndex = 24;
            this.Lbl_IP.Text = "IP";
            this.Lbl_IP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FmDataTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.TabLP_Mac);
            this.Controls.Add(this.TabCtlDataTransfer);
            this.Controls.Add(this.Lbl_FmName);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmDataTransfer";
            this.Text = "Data Transfer";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDetectSet_FormClosing);
            this.Load += new System.EventHandler(this.frmDataTransfer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVDataTransfer)).EndInit();
            this.TabCtlDataTransfer.ResumeLayout(false);
            this.MainLine_GJS.ResumeLayout(false);
            this.TabLP_MainLine.ResumeLayout(false);
            this.TabLP_MainLine.PerformLayout();
            this.Caption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVDTCaptionName)).EndInit();
            this.TabLP_Mac.ResumeLayout(false);
            this.TabLP_Mac.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Lbl_FmName;
        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView DGVDataTransfer;
        private System.Windows.Forms.TabControl TabCtlDataTransfer;
        private System.Windows.Forms.TabPage MainLine_GJS;
        private System.Windows.Forms.TableLayoutPanel TabLP_MainLine;
        private System.Windows.Forms.Label LabLocalPath;
        private System.Windows.Forms.Label LabLocalPathValue;
        private System.Windows.Forms.Label Lbl_Mac1;
        private System.Windows.Forms.Label Lbl_IP1;
        private System.Windows.Forms.Label Lbl_Mac2;
        private System.Windows.Forms.Label Lbl_IP2;
        private System.Windows.Forms.TableLayoutPanel TabLP_Mac;
        private System.Windows.Forms.Label Lbl_PosNo1;
        private System.Windows.Forms.Label Lbl_PosNo2;
        private System.Windows.Forms.Label Lbl_Mac;
        private System.Windows.Forms.Label Lbl_IP;
        private System.Windows.Forms.DataGridView DGVDTCaptionName;
        private System.Windows.Forms.TabPage Caption;
    }
}
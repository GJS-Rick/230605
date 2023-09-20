namespace nsUI
{
    partial class FmLogData
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
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblLogTitle = new System.Windows.Forms.Label();
            this.cbxLogDate = new System.Windows.Forms.ComboBox();
            this.cbxLogMode = new System.Windows.Forms.ComboBox();
            this.BtnSync = new System.Windows.Forms.Button();
            this.BtnFolder = new System.Windows.Forms.Button();
            this.cbxDataType1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColMode,
            this.ColAction,
            this.ColValue});
            this.dataGridViewLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewLog.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dataGridViewLog.Location = new System.Drawing.Point(0, 23);
            this.dataGridViewLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.RowHeadersWidth = 25;
            this.dataGridViewLog.RowTemplate.Height = 24;
            this.dataGridViewLog.Size = new System.Drawing.Size(635, 370);
            this.dataGridViewLog.TabIndex = 2;
            this.dataGridViewLog.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrollReader);
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ColName.FillWeight = 370F;
            this.ColName.HeaderText = "Date";
            this.ColName.Name = "ColName";
            this.ColName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColName.Width = 42;
            // 
            // ColMode
            // 
            this.ColMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ColMode.FillWeight = 220F;
            this.ColMode.HeaderText = "Mode";
            this.ColMode.Name = "ColMode";
            this.ColMode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColMode.Width = 49;
            // 
            // ColAction
            // 
            this.ColAction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ColAction.HeaderText = "Action";
            this.ColAction.Name = "ColAction";
            this.ColAction.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColAction.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColAction.Width = 55;
            // 
            // ColValue
            // 
            this.ColValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ColValue.HeaderText = "Data";
            this.ColValue.Name = "ColValue";
            this.ColValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColValue.Width = 42;
            // 
            // LblLogTitle
            // 
            this.LblLogTitle.AutoSize = true;
            this.LblLogTitle.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold);
            this.LblLogTitle.Location = new System.Drawing.Point(1, 1);
            this.LblLogTitle.Name = "LblLogTitle";
            this.LblLogTitle.Size = new System.Drawing.Size(49, 19);
            this.LblLogTitle.TabIndex = 3;
            this.LblLogTitle.Text = "日誌";
            // 
            // cbxLogDate
            // 
            this.cbxLogDate.BackColor = System.Drawing.SystemColors.Window;
            this.cbxLogDate.DropDownWidth = 225;
            this.cbxLogDate.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbxLogDate.FormattingEnabled = true;
            this.cbxLogDate.ItemHeight = 24;
            this.cbxLogDate.Location = new System.Drawing.Point(0, 400);
            this.cbxLogDate.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cbxLogDate.Name = "cbxLogDate";
            this.cbxLogDate.Size = new System.Drawing.Size(160, 32);
            this.cbxLogDate.TabIndex = 49;
            this.cbxLogDate.SelectedIndexChanged += new System.EventHandler(this.cbxLogDate_SelectedIndexChanged);
            // 
            // cbxLogMode
            // 
            this.cbxLogMode.BackColor = System.Drawing.SystemColors.Window;
            this.cbxLogMode.DropDownWidth = 225;
            this.cbxLogMode.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbxLogMode.FormattingEnabled = true;
            this.cbxLogMode.Location = new System.Drawing.Point(170, 400);
            this.cbxLogMode.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cbxLogMode.Name = "cbxLogMode";
            this.cbxLogMode.Size = new System.Drawing.Size(160, 32);
            this.cbxLogMode.TabIndex = 50;
            this.cbxLogMode.SelectedIndexChanged += new System.EventHandler(this.cbxLogMode_SelectedIndexChanged);
            // 
            // BtnSync
            // 
            this.BtnSync.Image = global::GJSControl.Properties.Resources.Sync;
            this.BtnSync.Location = new System.Drawing.Point(505, 396);
            this.BtnSync.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSync.Name = "BtnSync";
            this.BtnSync.Size = new System.Drawing.Size(60, 60);
            this.BtnSync.TabIndex = 51;
            this.BtnSync.UseVisualStyleBackColor = true;
            this.BtnSync.Click += new System.EventHandler(this.BtnSync_Click);
            // 
            // BtnFolder
            // 
            this.BtnFolder.Image = global::GJSControl.Properties.Resources.Folder;
            this.BtnFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnFolder.Location = new System.Drawing.Point(575, 396);
            this.BtnFolder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnFolder.Name = "BtnFolder";
            this.BtnFolder.Size = new System.Drawing.Size(60, 60);
            this.BtnFolder.TabIndex = 48;
            this.BtnFolder.UseVisualStyleBackColor = true;
            this.BtnFolder.Click += new System.EventHandler(this.BtnLog_Click);
            // 
            // cbxDataType1
            // 
            this.cbxDataType1.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.cbxDataType1.FormattingEnabled = true;
            this.cbxDataType1.Location = new System.Drawing.Point(340, 400);
            this.cbxDataType1.Name = "cbxDataType1";
            this.cbxDataType1.Size = new System.Drawing.Size(160, 32);
            this.cbxDataType1.TabIndex = 52;
            this.cbxDataType1.SelectedIndexChanged += new System.EventHandler(this.cbxDataType1_SelectedIndexChanged);
            // 
            // FmLogData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.cbxDataType1);
            this.Controls.Add(this.BtnSync);
            this.Controls.Add(this.cbxLogMode);
            this.Controls.Add(this.cbxLogDate);
            this.Controls.Add(this.BtnFolder);
            this.Controls.Add(this.LblLogTitle);
            this.Controls.Add(this.dataGridViewLog);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FmLogData";
            this.Text = "Log";
            this.Load += new System.EventHandler(this.FmLogData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.Label LblLogTitle;
        private System.Windows.Forms.Button BtnFolder;
        private System.Windows.Forms.ComboBox cbxLogDate;
        private System.Windows.Forms.ComboBox cbxLogMode;
        private System.Windows.Forms.Button BtnSync;
        private System.Windows.Forms.ComboBox cbxDataType1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
    }
}
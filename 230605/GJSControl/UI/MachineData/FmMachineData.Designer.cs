namespace nsUI
{
    partial class FmMachineData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGVMachine = new System.Windows.Forms.DataGridView();
            this.ColNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LblMachineTitle = new System.Windows.Forms.Label();
            this.BtnSave_S = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVMachine)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVMachine
            // 
            this.DGVMachine.AllowUserToAddRows = false;
            this.DGVMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVMachine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColNum,
            this.ColName,
            this.ColValue});
            this.DGVMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGVMachine.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.DGVMachine.Location = new System.Drawing.Point(0, 24);
            this.DGVMachine.Name = "DGVMachine";
            this.DGVMachine.RowHeadersWidth = 25;
            this.DGVMachine.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGVMachine.RowTemplate.Height = 24;
            this.DGVMachine.Size = new System.Drawing.Size(635, 370);
            this.DGVMachine.TabIndex = 2;
            this.DGVMachine.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DGV_EditingControlShowing);
            // 
            // ColNum
            // 
            this.ColNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColNum.HeaderText = "NO.";
            this.ColNum.Name = "ColNum";
            this.ColNum.ReadOnly = true;
            this.ColNum.Width = 69;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.FillWeight = 370F;
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColValue
            // 
            dataGridViewCellStyle3.Format = "N6";
            dataGridViewCellStyle3.NullValue = "0";
            this.ColValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColValue.FillWeight = 220F;
            this.ColValue.HeaderText = "Value";
            this.ColValue.Name = "ColValue";
            this.ColValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColValue.Width = 160;
            // 
            // LblMachineTitle
            // 
            this.LblMachineTitle.AutoSize = true;
            this.LblMachineTitle.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold);
            this.LblMachineTitle.Location = new System.Drawing.Point(1, 1);
            this.LblMachineTitle.Name = "LblMachineTitle";
            this.LblMachineTitle.Size = new System.Drawing.Size(89, 19);
            this.LblMachineTitle.TabIndex = 3;
            this.LblMachineTitle.Text = "設備參數";
            // 
            // BtnSave_S
            // 
            this.BtnSave_S.Image = global::GJSControl.Properties.Resources.Save;
            this.BtnSave_S.Location = new System.Drawing.Point(575, 396);
            this.BtnSave_S.Name = "BtnSave_S";
            this.BtnSave_S.Size = new System.Drawing.Size(60, 60);
            this.BtnSave_S.TabIndex = 0;
            this.BtnSave_S.UseVisualStyleBackColor = true;
            this.BtnSave_S.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // FmMachineData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 458);
            this.ControlBox = false;
            this.Controls.Add(this.LblMachineTitle);
            this.Controls.Add(this.DGVMachine);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FmMachineData";
            this.Text = "設備參數";
            this.Load += new System.EventHandler(this.FmMachineData_Load);
            this.VisibleChanged += new System.EventHandler(this.FmMachineData_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DGVMachine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.DataGridView DGVMachine;
        private System.Windows.Forms.Label LblMachineTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
    }
}
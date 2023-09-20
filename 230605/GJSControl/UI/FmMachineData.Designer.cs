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
            this.dataGridViewMachine = new System.Windows.Forms.DataGridView();
            this.LblMachineTitle = new System.Windows.Forms.Label();
            this.BtnSave_S = new System.Windows.Forms.Button();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMachine)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMachine
            // 
            this.dataGridViewMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMachine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColValue});
            this.dataGridViewMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewMachine.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dataGridViewMachine.Location = new System.Drawing.Point(0, 24);
            this.dataGridViewMachine.Name = "dataGridViewMachine";
            this.dataGridViewMachine.RowHeadersWidth = 25;
            this.dataGridViewMachine.RowTemplate.Height = 24;
            this.dataGridViewMachine.Size = new System.Drawing.Size(635, 370);
            this.dataGridViewMachine.TabIndex = 2;
            this.dataGridViewMachine.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMachine_CellClick);
            this.dataGridViewMachine.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMachine_CellContentClick);
            // 
            // LblMachineTitle
            // 
            this.LblMachineTitle.AutoSize = true;
            this.LblMachineTitle.Location = new System.Drawing.Point(0, 0);
            this.LblMachineTitle.Name = "LblMachineTitle";
            this.LblMachineTitle.Size = new System.Drawing.Size(86, 24);
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
            // ColName
            // 
            this.ColName.FillWeight = 370F;
            this.ColName.HeaderText = "Name";
            this.ColName.Name = "ColName";
            this.ColName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColName.Width = 370;
            // 
            // ColValue
            // 
            this.ColValue.FillWeight = 220F;
            this.ColValue.HeaderText = "Value";
            this.ColValue.Name = "ColValue";
            this.ColValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColValue.Width = 220;
            // 
            // FmMachineData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 458);
            this.ControlBox = false;
            this.Controls.Add(this.LblMachineTitle);
            this.Controls.Add(this.dataGridViewMachine);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FmMachineData";
            this.Text = "設備參數";
            this.Load += new System.EventHandler(this.FmMachineData_Load);
            this.VisibleChanged += new System.EventHandler(this.FmMachineData_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMachine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.DataGridView dataGridViewMachine;
        private System.Windows.Forms.Label LblMachineTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
    }
}
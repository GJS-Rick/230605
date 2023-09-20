namespace nsFmMotion
{
    partial class FmDetectSet
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
            this.Lbl_FmName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.DGVUseDetect = new System.Windows.Forms.DataGridView();
            this.BtnSave_S = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVUseDetect)).BeginInit();
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
            this.Lbl_FmName.Text = "讀頭設定";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DGVUseDetect
            // 
            this.DGVUseDetect.AllowUserToResizeColumns = false;
            this.DGVUseDetect.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVUseDetect.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGVUseDetect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DGVUseDetect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVUseDetect.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DGVUseDetect.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVUseDetect.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DGVUseDetect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVUseDetect.Location = new System.Drawing.Point(2, 20);
            this.DGVUseDetect.Margin = new System.Windows.Forms.Padding(2);
            this.DGVUseDetect.MultiSelect = false;
            this.DGVUseDetect.Name = "DGVUseDetect";
            this.DGVUseDetect.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVUseDetect.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGVUseDetect.RowHeadersVisible = false;
            this.DGVUseDetect.RowHeadersWidth = 40;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DGVUseDetect.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.DGVUseDetect.RowTemplate.Height = 24;
            this.DGVUseDetect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVUseDetect.Size = new System.Drawing.Size(631, 371);
            this.DGVUseDetect.TabIndex = 21;
            this.DGVUseDetect.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVUseDetect_CellContentClick);
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
            // FmDetectSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.DGVUseDetect);
            this.Controls.Add(this.Lbl_FmName);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FmDetectSet";
            this.Text = "Detect Set";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDetectSet_FormClosing);
            this.Load += new System.EventHandler(this.frmDetectSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVUseDetect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Lbl_FmName;
        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView DGVUseDetect;
    }
}
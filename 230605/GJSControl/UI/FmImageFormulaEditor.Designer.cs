namespace nsUI
{
    partial class FmImageFormulaEditor
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioButtonTemplate = new System.Windows.Forms.RadioButton();
            this.radioButtonSample = new System.Windows.Forms.RadioButton();
            this.buttonTestImageWithFormula = new System.Windows.Forms.Button();
            this.textBoxFormula = new System.Windows.Forms.TextBox();
            this.buttonAddFormula = new System.Windows.Forms.Button();
            this.buttonDeleteFormula = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxTemplate = new System.Windows.Forms.ComboBox();
            this.comboBoxSample = new System.Windows.Forms.ComboBox();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxFormulaName = new System.Windows.Forms.TextBox();
            this.dataGridViewFormula = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOpenImage = new System.Windows.Forms.Button();
            this.checkBoxUseSelectedFormula = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFormula)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(288, 471);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // radioButtonTemplate
            // 
            this.radioButtonTemplate.AutoSize = true;
            this.radioButtonTemplate.Location = new System.Drawing.Point(101, 18);
            this.radioButtonTemplate.Name = "radioButtonTemplate";
            this.radioButtonTemplate.Size = new System.Drawing.Size(108, 26);
            this.radioButtonTemplate.TabIndex = 1;
            this.radioButtonTemplate.TabStop = true;
            this.radioButtonTemplate.Text = "Template";
            this.radioButtonTemplate.UseVisualStyleBackColor = true;
            this.radioButtonTemplate.Visible = false;
            // 
            // radioButtonSample
            // 
            this.radioButtonSample.AutoSize = true;
            this.radioButtonSample.Location = new System.Drawing.Point(230, 18);
            this.radioButtonSample.Name = "radioButtonSample";
            this.radioButtonSample.Size = new System.Drawing.Size(88, 26);
            this.radioButtonSample.TabIndex = 2;
            this.radioButtonSample.TabStop = true;
            this.radioButtonSample.Text = "Sample";
            this.radioButtonSample.UseVisualStyleBackColor = true;
            this.radioButtonSample.Visible = false;
            // 
            // buttonTestImageWithFormula
            // 
            this.buttonTestImageWithFormula.Location = new System.Drawing.Point(6, 163);
            this.buttonTestImageWithFormula.Name = "buttonTestImageWithFormula";
            this.buttonTestImageWithFormula.Size = new System.Drawing.Size(108, 36);
            this.buttonTestImageWithFormula.TabIndex = 3;
            this.buttonTestImageWithFormula.Text = "測試圖像";
            this.buttonTestImageWithFormula.UseVisualStyleBackColor = true;
            this.buttonTestImageWithFormula.Click += new System.EventHandler(this.buttonTestImageWithFormula_Click);
            // 
            // textBoxFormula
            // 
            this.textBoxFormula.Location = new System.Drawing.Point(102, 192);
            this.textBoxFormula.Name = "textBoxFormula";
            this.textBoxFormula.Size = new System.Drawing.Size(223, 29);
            this.textBoxFormula.TabIndex = 5;
            // 
            // buttonAddFormula
            // 
            this.buttonAddFormula.Location = new System.Drawing.Point(114, 225);
            this.buttonAddFormula.Name = "buttonAddFormula";
            this.buttonAddFormula.Size = new System.Drawing.Size(99, 29);
            this.buttonAddFormula.TabIndex = 6;
            this.buttonAddFormula.Text = "新增算式";
            this.buttonAddFormula.UseVisualStyleBackColor = true;
            this.buttonAddFormula.Click += new System.EventHandler(this.buttonAddFormula_Click);
            // 
            // buttonDeleteFormula
            // 
            this.buttonDeleteFormula.Location = new System.Drawing.Point(219, 225);
            this.buttonDeleteFormula.Name = "buttonDeleteFormula";
            this.buttonDeleteFormula.Size = new System.Drawing.Size(106, 29);
            this.buttonDeleteFormula.TabIndex = 7;
            this.buttonDeleteFormula.Text = "刪除算式";
            this.buttonDeleteFormula.UseVisualStyleBackColor = true;
            this.buttonDeleteFormula.Click += new System.EventHandler(this.buttonDeleteFormula_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(225, 121);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(99, 36);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "儲存參數";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxUseSelectedFormula);
            this.groupBox1.Controls.Add(this.buttonOpenImage);
            this.groupBox1.Controls.Add(this.comboBoxTemplate);
            this.groupBox1.Controls.Add(this.comboBoxSample);
            this.groupBox1.Controls.Add(this.buttonQuit);
            this.groupBox1.Controls.Add(this.radioButtonTemplate);
            this.groupBox1.Controls.Add(this.buttonSave);
            this.groupBox1.Controls.Add(this.radioButtonSample);
            this.groupBox1.Controls.Add(this.buttonTestImageWithFormula);
            this.groupBox1.Location = new System.Drawing.Point(307, 278);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 205);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vision";
            // 
            // comboBoxTemplate
            // 
            this.comboBoxTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTemplate.FormattingEnabled = true;
            this.comboBoxTemplate.Location = new System.Drawing.Point(101, 50);
            this.comboBoxTemplate.Name = "comboBoxTemplate";
            this.comboBoxTemplate.Size = new System.Drawing.Size(108, 30);
            this.comboBoxTemplate.TabIndex = 11;
            this.comboBoxTemplate.Visible = false;
            // 
            // comboBoxSample
            // 
            this.comboBoxSample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSample.FormattingEnabled = true;
            this.comboBoxSample.Location = new System.Drawing.Point(215, 50);
            this.comboBoxSample.Name = "comboBoxSample";
            this.comboBoxSample.Size = new System.Drawing.Size(113, 30);
            this.comboBoxSample.TabIndex = 10;
            this.comboBoxSample.Visible = false;
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(225, 163);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(99, 36);
            this.buttonQuit.TabIndex = 9;
            this.buttonQuit.Text = "離開";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxFormulaName);
            this.groupBox2.Controls.Add(this.dataGridViewFormula);
            this.groupBox2.Controls.Add(this.textBoxFormula);
            this.groupBox2.Controls.Add(this.buttonDeleteFormula);
            this.groupBox2.Controls.Add(this.buttonAddFormula);
            this.groupBox2.Location = new System.Drawing.Point(306, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 260);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Formula";
            // 
            // textBoxFormulaName
            // 
            this.textBoxFormulaName.Location = new System.Drawing.Point(7, 192);
            this.textBoxFormulaName.Name = "textBoxFormulaName";
            this.textBoxFormulaName.Size = new System.Drawing.Size(89, 29);
            this.textBoxFormulaName.TabIndex = 9;
            // 
            // dataGridViewFormula
            // 
            this.dataGridViewFormula.AllowUserToAddRows = false;
            this.dataGridViewFormula.AllowUserToDeleteRows = false;
            this.dataGridViewFormula.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFormula.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridViewFormula.Location = new System.Drawing.Point(7, 29);
            this.dataGridViewFormula.Name = "dataGridViewFormula";
            this.dataGridViewFormula.ReadOnly = true;
            this.dataGridViewFormula.RowTemplate.Height = 24;
            this.dataGridViewFormula.Size = new System.Drawing.Size(318, 157);
            this.dataGridViewFormula.TabIndex = 8;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Name";
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 55;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.DataPropertyName = "Formula";
            this.Column2.HeaderText = "Formula";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // buttonOpenImage
            // 
            this.buttonOpenImage.Location = new System.Drawing.Point(6, 110);
            this.buttonOpenImage.Name = "buttonOpenImage";
            this.buttonOpenImage.Size = new System.Drawing.Size(108, 36);
            this.buttonOpenImage.TabIndex = 12;
            this.buttonOpenImage.Text = "開啟圖像";
            this.buttonOpenImage.UseVisualStyleBackColor = true;
            this.buttonOpenImage.Click += new System.EventHandler(this.buttonOpenImage_Click);
            // 
            // checkBoxUseSelectedFormula
            // 
            this.checkBoxUseSelectedFormula.AutoSize = true;
            this.checkBoxUseSelectedFormula.Location = new System.Drawing.Point(6, 80);
            this.checkBoxUseSelectedFormula.Name = "checkBoxUseSelectedFormula";
            this.checkBoxUseSelectedFormula.Size = new System.Drawing.Size(229, 26);
            this.checkBoxUseSelectedFormula.TabIndex = 13;
            this.checkBoxUseSelectedFormula.Text = "Use Selected formula";
            this.checkBoxUseSelectedFormula.UseVisualStyleBackColor = true;
            // 
            // FmImageFormulaEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(651, 495);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Consolas", 14F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FmImageFormulaEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FmImageFormulaEditor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFormula)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioButtonTemplate;
        private System.Windows.Forms.RadioButton radioButtonSample;
        private System.Windows.Forms.Button buttonTestImageWithFormula;
        private System.Windows.Forms.TextBox textBoxFormula;
        private System.Windows.Forms.Button buttonAddFormula;
        private System.Windows.Forms.Button buttonDeleteFormula;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.DataGridView dataGridViewFormula;
        private System.Windows.Forms.TextBox textBoxFormulaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ComboBox comboBoxTemplate;
        private System.Windows.Forms.ComboBox comboBoxSample;
        private System.Windows.Forms.Button buttonOpenImage;
        private System.Windows.Forms.CheckBox checkBoxUseSelectedFormula;
    }
}
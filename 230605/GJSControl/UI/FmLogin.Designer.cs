namespace nsUI
{
    partial class FmLogin
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
            this.LblPassWord = new System.Windows.Forms.Label();
            this.LblUserID = new System.Windows.Forms.Label();
            this.maskTxtPw = new System.Windows.Forms.MaskedTextBox();
            this.cbxUserID = new System.Windows.Forms.ComboBox();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LblPassWord
            // 
            this.LblPassWord.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblPassWord.Location = new System.Drawing.Point(20, 100);
            this.LblPassWord.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LblPassWord.Name = "LblPassWord";
            this.LblPassWord.Size = new System.Drawing.Size(160, 40);
            this.LblPassWord.TabIndex = 36;
            this.LblPassWord.Text = "密碼";
            this.LblPassWord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblUserID
            // 
            this.LblUserID.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblUserID.Location = new System.Drawing.Point(20, 20);
            this.LblUserID.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LblUserID.Name = "LblUserID";
            this.LblUserID.Size = new System.Drawing.Size(160, 40);
            this.LblUserID.TabIndex = 35;
            this.LblUserID.Text = "使用者";
            this.LblUserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskTxtPw
            // 
            this.maskTxtPw.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.maskTxtPw.Location = new System.Drawing.Point(180, 97);
            this.maskTxtPw.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.maskTxtPw.Name = "maskTxtPw";
            this.maskTxtPw.Size = new System.Drawing.Size(320, 46);
            this.maskTxtPw.TabIndex = 34;
            this.maskTxtPw.UseSystemPasswordChar = true;
            this.maskTxtPw.Click += new System.EventHandler(this.MaskTxtPw_Click);
            // 
            // cbxUserID
            // 
            this.cbxUserID.BackColor = System.Drawing.SystemColors.Window;
            this.cbxUserID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUserID.DropDownWidth = 225;
            this.cbxUserID.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbxUserID.FormattingEnabled = true;
            this.cbxUserID.Location = new System.Drawing.Point(180, 19);
            this.cbxUserID.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cbxUserID.Name = "cbxUserID";
            this.cbxUserID.Size = new System.Drawing.Size(320, 44);
            this.cbxUserID.TabIndex = 33;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnConfirm.Location = new System.Drawing.Point(520, 15);
            this.BtnConfirm.Margin = new System.Windows.Forms.Padding(6, 11, 6, 11);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(180, 60);
            this.BtnConfirm.TabIndex = 32;
            this.BtnConfirm.Text = "確認";
            this.BtnConfirm.UseVisualStyleBackColor = true;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnExit.Location = new System.Drawing.Point(520, 95);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(6, 11, 6, 11);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(180, 60);
            this.BtnExit.TabIndex = 37;
            this.BtnExit.Text = "離開";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // FmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 166);
            this.ControlBox = false;
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.LblPassWord);
            this.Controls.Add(this.LblUserID);
            this.Controls.Add(this.maskTxtPw);
            this.Controls.Add(this.cbxUserID);
            this.Controls.Add(this.BtnConfirm);
            this.Font = new System.Drawing.Font("微軟正黑體", 18F);
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.Name = "FmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FmLogin_Load);
            this.VisibleChanged += new System.EventHandler(this.FmLogin_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblPassWord;
        private System.Windows.Forms.Label LblUserID;
        private System.Windows.Forms.MaskedTextBox maskTxtPw;
        private System.Windows.Forms.ComboBox cbxUserID;
        private System.Windows.Forms.Button BtnConfirm;
        private System.Windows.Forms.Button BtnExit;
    }
}
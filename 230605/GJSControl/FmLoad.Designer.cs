namespace GJSControl
{
    partial class FmLoad
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPw = new System.Windows.Forms.Label();
            this.lblUserLevel = new System.Windows.Forms.Label();
            this.maskTxtPw = new System.Windows.Forms.MaskedTextBox();
            this.cbxUserID = new System.Windows.Forms.ComboBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPw
            // 
            this.lblPw.AutoSize = true;
            this.lblPw.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPw.Location = new System.Drawing.Point(417, 121);
            this.lblPw.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPw.Name = "lblPw";
            this.lblPw.Size = new System.Drawing.Size(81, 40);
            this.lblPw.TabIndex = 31;
            this.lblPw.Text = "密碼";
           
            // 
            // lblUserLevel
            // 
            this.lblUserLevel.AutoSize = true;
            this.lblUserLevel.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblUserLevel.Location = new System.Drawing.Point(417, 40);
            this.lblUserLevel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblUserLevel.Name = "lblUserLevel";
            this.lblUserLevel.Size = new System.Drawing.Size(113, 40);
            this.lblUserLevel.TabIndex = 30;
            this.lblUserLevel.Text = "使用者";
            
            // 
            // maskTxtPw
            // 
            this.maskTxtPw.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.maskTxtPw.Location = new System.Drawing.Point(27, 118);
            this.maskTxtPw.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.maskTxtPw.Name = "maskTxtPw";
            this.maskTxtPw.Size = new System.Drawing.Size(346, 50);
            this.maskTxtPw.TabIndex = 29;
            this.maskTxtPw.UseSystemPasswordChar = true;
            
            // cbxUserID
            // 
            this.cbxUserID.BackColor = System.Drawing.SystemColors.Window;
            this.cbxUserID.DropDownWidth = 225;
            this.cbxUserID.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbxUserID.FormattingEnabled = true;
            this.cbxUserID.Location = new System.Drawing.Point(27, 36);
            this.cbxUserID.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.cbxUserID.Name = "cbxUserID";
            this.cbxUserID.Size = new System.Drawing.Size(346, 48);
            this.cbxUserID.TabIndex = 28;
            this.cbxUserID.Text = "使用者(User)";
            this.cbxUserID.SelectedIndexChanged += new System.EventHandler(this.cbxUserID_SelectedIndexChanged);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnExit.Location = new System.Drawing.Point(115, 229);
            this.btnExit.Margin = new System.Windows.Forms.Padding(6, 11, 6, 11);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(639, 97);
            this.btnExit.TabIndex = 27;
            this.btnExit.Text = "離開";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnStart.Location = new System.Drawing.Point(570, 36);
            this.btnStart.Margin = new System.Windows.Forms.Padding(6, 11, 6, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(241, 132);
            this.btnStart.TabIndex = 26;
            this.btnStart.Text = "開始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // FmLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 366);
            this.ControlBox = false;
            this.Controls.Add(this.lblPw);
            this.Controls.Add(this.lblUserLevel);
            this.Controls.Add(this.maskTxtPw);
            this.Controls.Add(this.cbxUserID);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "FmLoad";
            this.Text = "Load";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmLoad_FormClosed);
            this.Shown += new System.EventHandler(this.FmLoad_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPw;
        private System.Windows.Forms.Label lblUserLevel;
        private System.Windows.Forms.MaskedTextBox maskTxtPw;
        private System.Windows.Forms.ComboBox cbxUserID;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStart;
    }
}


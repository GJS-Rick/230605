namespace nsSequence
{
    partial class FmTimeoutMsg
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
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.labCode = new System.Windows.Forms.Label();
            this.lblCodeHdr = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtbDescription
            // 
            this.rtbDescription.AcceptsTab = true;
            this.rtbDescription.BackColor = System.Drawing.Color.Gainsboro;
            this.rtbDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDescription.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rtbDescription.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rtbDescription.Location = new System.Drawing.Point(10, 37);
            this.rtbDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new System.Drawing.Size(615, 358);
            this.rtbDescription.TabIndex = 18;
            this.rtbDescription.Text = "描述";
            // 
            // labCode
            // 
            this.labCode.AutoSize = true;
            this.labCode.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labCode.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labCode.Location = new System.Drawing.Point(75, 10);
            this.labCode.Name = "labCode";
            this.labCode.Size = new System.Drawing.Size(54, 24);
            this.labCode.TabIndex = 14;
            this.labCode.Text = "1234";
            // 
            // lblCodeHdr
            // 
            this.lblCodeHdr.AutoSize = true;
            this.lblCodeHdr.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCodeHdr.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCodeHdr.Location = new System.Drawing.Point(10, 10);
            this.lblCodeHdr.Name = "lblCodeHdr";
            this.lblCodeHdr.Size = new System.Drawing.Size(57, 24);
            this.lblCodeHdr.TabIndex = 13;
            this.lblCodeHdr.Text = "代碼 :";
            // 
            // FmTimeoutMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.labCode);
            this.Controls.Add(this.lblCodeHdr);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FmTimeoutMsg";
            this.Text = "異常處理";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FmTimeoutMsg_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.RichTextBox rtbDescription;
        public System.Windows.Forms.Label labCode;
        private System.Windows.Forms.Label lblCodeHdr;
    }
}
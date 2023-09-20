namespace nsUI
{
    partial class LearnPatter
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.zoomAndPanWindow1 = new nsUI.ZoomAndPanWindow();
            this.zoomAndPanWindow2 = new nsUI.ZoomAndPanWindow();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("Consolas", 18F);
            this.buttonOK.Location = new System.Drawing.Point(12, 541);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(124, 46);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Consolas", 18F);
            this.buttonCancel.Location = new System.Drawing.Point(434, 540);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(124, 46);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // zoomAndPanWindow1
            // 
            this.zoomAndPanWindow1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.zoomAndPanWindow1.Location = new System.Drawing.Point(291, 12);
            this.zoomAndPanWindow1.Name = "zoomAndPanWindow1";
            this.zoomAndPanWindow1.Size = new System.Drawing.Size(270, 480);
            this.zoomAndPanWindow1.TabIndex = 4;
            // 
            // zoomAndPanWindow2
            // 
            this.zoomAndPanWindow2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.zoomAndPanWindow2.Location = new System.Drawing.Point(12, 12);
            this.zoomAndPanWindow2.Name = "zoomAndPanWindow2";
            this.zoomAndPanWindow2.Size = new System.Drawing.Size(270, 480);
            this.zoomAndPanWindow2.TabIndex = 5;
            // 
            // LearnPatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 599);
            this.Controls.Add(this.zoomAndPanWindow2);
            this.Controls.Add(this.zoomAndPanWindow1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LearnPatter";
            this.Text = "LearnPatter";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private ZoomAndPanWindow zoomAndPanWindow1;
        private ZoomAndPanWindow zoomAndPanWindow2;
    }
}
namespace nsUI
{
    partial class FmSetManualAlignPosition
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
			this.cbxCCDIndex = new System.Windows.Forms.ComboBox();
			this.BtnExit = new System.Windows.Forms.Button();
			this.statusStripInfo = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.BtnSetPosition = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.BtnChoosePosition = new System.Windows.Forms.Button();
			this.ZoomWindow = new ZoomWindow.ZoomAndPanWindow();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.BtnCancelChoose = new System.Windows.Forms.Button();
			this.statusStripInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbxCCDIndex
			// 
			this.cbxCCDIndex.FormattingEnabled = true;
			this.cbxCCDIndex.Items.AddRange(new object[] {
            "Verify",
            "Punch",
            "Unload"});
			this.cbxCCDIndex.Location = new System.Drawing.Point(645, 227);
			this.cbxCCDIndex.Name = "cbxCCDIndex";
			this.cbxCCDIndex.Size = new System.Drawing.Size(132, 35);
			this.cbxCCDIndex.TabIndex = 2;
			this.cbxCCDIndex.Text = "選擇CCD";
			// 
			// BtnExit
			// 
			this.BtnExit.Location = new System.Drawing.Point(645, 427);
			this.BtnExit.Name = "BtnExit";
			this.BtnExit.Size = new System.Drawing.Size(132, 54);
			this.BtnExit.TabIndex = 3;
			this.BtnExit.Text = "結束";
			this.BtnExit.UseVisualStyleBackColor = true;
			this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
			// 
			// statusStripInfo
			// 
			this.statusStripInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStripInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStripInfo.Location = new System.Drawing.Point(0, 494);
			this.statusStripInfo.Name = "statusStripInfo";
			this.statusStripInfo.Size = new System.Drawing.Size(802, 22);
			this.statusStripInfo.TabIndex = 4;
			this.statusStripInfo.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(128, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// BtnSetPosition
			// 
			this.BtnSetPosition.Location = new System.Drawing.Point(646, 63);
			this.BtnSetPosition.Name = "BtnSetPosition";
			this.BtnSetPosition.Size = new System.Drawing.Size(132, 35);
			this.BtnSetPosition.TabIndex = 5;
			this.BtnSetPosition.Text = "設定定位點";
			this.BtnSetPosition.UseVisualStyleBackColor = true;
			this.BtnSetPosition.Click += new System.EventHandler(this.BtnSetPosition_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// BtnChoosePosition
			// 
			this.BtnChoosePosition.Location = new System.Drawing.Point(646, 26);
			this.BtnChoosePosition.Name = "BtnChoosePosition";
			this.BtnChoosePosition.Size = new System.Drawing.Size(132, 31);
			this.BtnChoosePosition.TabIndex = 23;
			this.BtnChoosePosition.Text = "點選定位點";
			this.BtnChoosePosition.UseVisualStyleBackColor = true;
			this.BtnChoosePosition.Click += new System.EventHandler(this.BtnChoosePosition_Click);
			// 
			// ZoomWindow
			// 
			this.ZoomWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ZoomWindow.Location = new System.Drawing.Point(0, 1);
			this.ZoomWindow.Name = "ZoomWindow";
			this.ZoomWindow.Size = new System.Drawing.Size(640, 480);
			this.ZoomWindow.TabIndex = 20;
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
			// 
			// BtnCancelChoose
			// 
			this.BtnCancelChoose.Location = new System.Drawing.Point(645, 104);
			this.BtnCancelChoose.Name = "BtnCancelChoose";
			this.BtnCancelChoose.Size = new System.Drawing.Size(132, 35);
			this.BtnCancelChoose.TabIndex = 35;
			this.BtnCancelChoose.Text = "點選取消";
			this.BtnCancelChoose.UseVisualStyleBackColor = true;
			this.BtnCancelChoose.Click += new System.EventHandler(this.BtnCancelChoose_Click);
			// 
			// FmSetManualAlignPosition
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(802, 516);
			this.ControlBox = false;
			this.Controls.Add(this.BtnCancelChoose);
			this.Controls.Add(this.BtnChoosePosition);
			this.Controls.Add(this.ZoomWindow);
			this.Controls.Add(this.BtnSetPosition);
			this.Controls.Add(this.statusStripInfo);
			this.Controls.Add(this.BtnExit);
			this.Controls.Add(this.cbxCCDIndex);
			this.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
			this.Name = "FmSetManualAlignPosition";
			this.Text = "AreaCCD";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FmAreaCCD_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmAreaCCD_FormClosed);
			this.Load += new System.EventHandler(this.FmAreaCCD_Load);
			this.VisibleChanged += new System.EventHandler(this.FmSetPosition_VisibleChanged);
			this.statusStripInfo.ResumeLayout(false);
			this.statusStripInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
        private System.Windows.Forms.ComboBox cbxCCDIndex;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnSetPosition;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private ZoomWindow.ZoomAndPanWindow ZoomWindow;
        private System.Windows.Forms.Button BtnChoosePosition;
        private System.Windows.Forms.Timer timer1;


		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStripInfo;
        private System.Windows.Forms.Button BtnCancelChoose;
    }
}
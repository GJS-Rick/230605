namespace nsUI
{
    partial class FmAreaCCD
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
            this.BtnLive = new System.Windows.Forms.Button();
            this.cbxCCDIndex = new System.Windows.Forms.ComboBox();
            this.statusStripInfo = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.BtnLearnCorner = new System.Windows.Forms.Button();
            this.BtnAlignTest = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.BtnSetExposure = new System.Windows.Forms.Button();
            this.BtnSaveImage = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.numericUpDownScore = new System.Windows.Forms.NumericUpDown();
            this.BtnRectCorner = new System.Windows.Forms.Button();
            this.BtnCancelRect = new System.Windows.Forms.Button();
            this.trackBarExposureTime = new System.Windows.Forms.TrackBar();
            this.BtnMakeMask = new System.Windows.Forms.Button();
            this.BtnAutoLearn = new System.Windows.Forms.Button();
            this.btnVisionReload = new System.Windows.Forms.Button();
            this.BtnZoomOut = new System.Windows.Forms.Button();
            this.BtnZoomIn = new System.Windows.Forms.Button();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnRight = new System.Windows.Forms.Button();
            this.BtnUp = new System.Windows.Forms.Button();
            this.BtnLeft = new System.Windows.Forms.Button();
            this.comboBoxAlignIndex = new System.Windows.Forms.ComboBox();
            this.comboBoxCorner = new System.Windows.Forms.ComboBox();
            this.BtnFormula = new System.Windows.Forms.Button();
            this.BtnCAMProperties = new System.Windows.Forms.Button();
            this.ZoomWindow = new nsUI.ZoomAndPanWindow();
            this.statusStripInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposureTime)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnLive
            // 
            this.BtnLive.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnLive.Location = new System.Drawing.Point(525, 3);
            this.BtnLive.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLive.Name = "BtnLive";
            this.BtnLive.Size = new System.Drawing.Size(110, 30);
            this.BtnLive.TabIndex = 1;
            this.BtnLive.Text = "Live";
            this.BtnLive.UseVisualStyleBackColor = true;
            this.BtnLive.Click += new System.EventHandler(this.BtnLive_Click);
            // 
            // cbxCCDIndex
            // 
            this.cbxCCDIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCCDIndex.Font = new System.Drawing.Font("新細明體", 12F);
            this.cbxCCDIndex.FormattingEnabled = true;
            this.cbxCCDIndex.Items.AddRange(new object[] {
            "Verify",
            "Punch",
            "Unload"});
            this.cbxCCDIndex.Location = new System.Drawing.Point(0, 366);
            this.cbxCCDIndex.Margin = new System.Windows.Forms.Padding(2);
            this.cbxCCDIndex.Name = "cbxCCDIndex";
            this.cbxCCDIndex.Size = new System.Drawing.Size(130, 28);
            this.cbxCCDIndex.TabIndex = 2;
            this.cbxCCDIndex.SelectedIndexChanged += new System.EventHandler(this.CbxCCDIndex_SelectedIndexChanged);
            // 
            // statusStripInfo
            // 
            this.statusStripInfo.BackColor = System.Drawing.SystemColors.Control;
            this.statusStripInfo.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.statusStripInfo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStripInfo.Location = new System.Drawing.Point(0, 425);
            this.statusStripInfo.Name = "statusStripInfo";
            this.statusStripInfo.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStripInfo.Size = new System.Drawing.Size(635, 31);
            this.statusStripInfo.TabIndex = 4;
            this.statusStripInfo.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(28, 25);
            this.toolStripStatusLabel1.Text = "\"\"";
            // 
            // BtnLearnCorner
            // 
            this.BtnLearnCorner.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnLearnCorner.Location = new System.Drawing.Point(525, 183);
            this.BtnLearnCorner.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLearnCorner.Name = "BtnLearnCorner";
            this.BtnLearnCorner.Size = new System.Drawing.Size(110, 30);
            this.BtnLearnCorner.TabIndex = 5;
            this.BtnLearnCorner.Text = "儲存定位點";
            this.BtnLearnCorner.UseVisualStyleBackColor = true;
            this.BtnLearnCorner.Click += new System.EventHandler(this.BtnLearnCorner_Click);
            // 
            // BtnAlignTest
            // 
            this.BtnAlignTest.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnAlignTest.Location = new System.Drawing.Point(295, 362);
            this.BtnAlignTest.Margin = new System.Windows.Forms.Padding(2);
            this.BtnAlignTest.Name = "BtnAlignTest";
            this.BtnAlignTest.Size = new System.Drawing.Size(90, 35);
            this.BtnAlignTest.TabIndex = 6;
            this.BtnAlignTest.Text = "定位測試";
            this.BtnAlignTest.UseVisualStyleBackColor = true;
            this.BtnAlignTest.Click += new System.EventHandler(this.BtnAlignTest_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // BtnSetExposure
            // 
            this.BtnSetExposure.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnSetExposure.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnSetExposure.Location = new System.Drawing.Point(480, 320);
            this.BtnSetExposure.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSetExposure.Name = "BtnSetExposure";
            this.BtnSetExposure.Size = new System.Drawing.Size(40, 40);
            this.BtnSetExposure.TabIndex = 13;
            this.BtnSetExposure.Text = "Set";
            this.BtnSetExposure.UseVisualStyleBackColor = true;
            this.BtnSetExposure.Click += new System.EventHandler(this.BtnSetExposure_Click);
            // 
            // BtnSaveImage
            // 
            this.BtnSaveImage.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnSaveImage.Location = new System.Drawing.Point(525, 63);
            this.BtnSaveImage.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSaveImage.Name = "BtnSaveImage";
            this.BtnSaveImage.Size = new System.Drawing.Size(110, 30);
            this.BtnSaveImage.TabIndex = 16;
            this.BtnSaveImage.Text = "Save";
            this.BtnSaveImage.UseVisualStyleBackColor = true;
            this.BtnSaveImage.Click += new System.EventHandler(this.BtnSaveImage_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnLoad.Location = new System.Drawing.Point(525, 33);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(110, 30);
            this.BtnLoad.TabIndex = 17;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // numericUpDownScore
            // 
            this.numericUpDownScore.DecimalPlaces = 2;
            this.numericUpDownScore.Font = new System.Drawing.Font("新細明體", 12F);
            this.numericUpDownScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownScore.Location = new System.Drawing.Point(134, 367);
            this.numericUpDownScore.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDownScore.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownScore.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownScore.Name = "numericUpDownScore";
            this.numericUpDownScore.Size = new System.Drawing.Size(82, 31);
            this.numericUpDownScore.TabIndex = 25;
            this.numericUpDownScore.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // BtnRectCorner
            // 
            this.BtnRectCorner.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnRectCorner.Location = new System.Drawing.Point(525, 93);
            this.BtnRectCorner.Margin = new System.Windows.Forms.Padding(2);
            this.BtnRectCorner.Name = "BtnRectCorner";
            this.BtnRectCorner.Size = new System.Drawing.Size(110, 30);
            this.BtnRectCorner.TabIndex = 23;
            this.BtnRectCorner.Text = "框選定位點";
            this.BtnRectCorner.UseVisualStyleBackColor = true;
            this.BtnRectCorner.Click += new System.EventHandler(this.ButtonLearnCorner_Click);
            // 
            // BtnCancelRect
            // 
            this.BtnCancelRect.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnCancelRect.Location = new System.Drawing.Point(525, 153);
            this.BtnCancelRect.Margin = new System.Windows.Forms.Padding(2);
            this.BtnCancelRect.Name = "BtnCancelRect";
            this.BtnCancelRect.Size = new System.Drawing.Size(110, 30);
            this.BtnCancelRect.TabIndex = 34;
            this.BtnCancelRect.Text = "框選取消";
            this.BtnCancelRect.UseVisualStyleBackColor = true;
            this.BtnCancelRect.Click += new System.EventHandler(this.ButtonCancelRect_Click);
            // 
            // trackBarExposureTime
            // 
            this.trackBarExposureTime.Location = new System.Drawing.Point(477, 85);
            this.trackBarExposureTime.Margin = new System.Windows.Forms.Padding(2);
            this.trackBarExposureTime.Name = "trackBarExposureTime";
            this.trackBarExposureTime.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarExposureTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarExposureTime.RightToLeftLayout = true;
            this.trackBarExposureTime.Size = new System.Drawing.Size(56, 230);
            this.trackBarExposureTime.TabIndex = 35;
            this.trackBarExposureTime.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarExposureTime.Scroll += new System.EventHandler(this.TrackBarExposureTime_Scroll);
            // 
            // BtnMakeMask
            // 
            this.BtnMakeMask.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnMakeMask.Location = new System.Drawing.Point(525, 123);
            this.BtnMakeMask.Margin = new System.Windows.Forms.Padding(2);
            this.BtnMakeMask.Name = "BtnMakeMask";
            this.BtnMakeMask.Size = new System.Drawing.Size(110, 30);
            this.BtnMakeMask.TabIndex = 39;
            this.BtnMakeMask.Text = "框選Mask";
            this.BtnMakeMask.UseVisualStyleBackColor = true;
            this.BtnMakeMask.Click += new System.EventHandler(this.buttonMakeMask_Click);
            // 
            // BtnAutoLearn
            // 
            this.BtnAutoLearn.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnAutoLearn.Location = new System.Drawing.Point(385, 362);
            this.BtnAutoLearn.Margin = new System.Windows.Forms.Padding(2);
            this.BtnAutoLearn.Name = "BtnAutoLearn";
            this.BtnAutoLearn.Size = new System.Drawing.Size(90, 35);
            this.BtnAutoLearn.TabIndex = 42;
            this.BtnAutoLearn.Text = "自動學習";
            this.BtnAutoLearn.UseVisualStyleBackColor = true;
            this.BtnAutoLearn.Click += new System.EventHandler(this.BtnAutoSetExposure_Click);
            // 
            // btnVisionReload
            // 
            this.btnVisionReload.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnVisionReload.Location = new System.Drawing.Point(385, 397);
            this.btnVisionReload.Margin = new System.Windows.Forms.Padding(2);
            this.btnVisionReload.Name = "btnVisionReload";
            this.btnVisionReload.Size = new System.Drawing.Size(90, 35);
            this.btnVisionReload.TabIndex = 44;
            this.btnVisionReload.Text = "參數重載";
            this.btnVisionReload.UseVisualStyleBackColor = true;
            this.btnVisionReload.Click += new System.EventHandler(this.btnVisionReload_Click);
            // 
            // BtnZoomOut
            // 
            this.BtnZoomOut.BackColor = System.Drawing.SystemColors.Control;
            this.BtnZoomOut.Image = global::GJSControl.Properties.Resources.ZoomOut;
            this.BtnZoomOut.Location = new System.Drawing.Point(480, 40);
            this.BtnZoomOut.Margin = new System.Windows.Forms.Padding(2);
            this.BtnZoomOut.Name = "BtnZoomOut";
            this.BtnZoomOut.Size = new System.Drawing.Size(40, 40);
            this.BtnZoomOut.TabIndex = 33;
            this.BtnZoomOut.UseVisualStyleBackColor = true;
            this.BtnZoomOut.Click += new System.EventHandler(this.WindowRoiRectangleControl);
            // 
            // BtnZoomIn
            // 
            this.BtnZoomIn.Image = global::GJSControl.Properties.Resources.ZoomIn;
            this.BtnZoomIn.Location = new System.Drawing.Point(480, 0);
            this.BtnZoomIn.Margin = new System.Windows.Forms.Padding(2);
            this.BtnZoomIn.Name = "BtnZoomIn";
            this.BtnZoomIn.Size = new System.Drawing.Size(40, 40);
            this.BtnZoomIn.TabIndex = 32;
            this.BtnZoomIn.UseVisualStyleBackColor = true;
            this.BtnZoomIn.Click += new System.EventHandler(this.WindowRoiRectangleControl);
            // 
            // BtnDown
            // 
            this.BtnDown.Image = global::GJSControl.Properties.Resources.DownArrow;
            this.BtnDown.Location = new System.Drawing.Point(555, 390);
            this.BtnDown.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(40, 40);
            this.BtnDown.TabIndex = 30;
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.WindowRoiRectangleControl);
            // 
            // BtnRight
            // 
            this.BtnRight.Image = global::GJSControl.Properties.Resources.RightArrow;
            this.BtnRight.Location = new System.Drawing.Point(595, 390);
            this.BtnRight.Margin = new System.Windows.Forms.Padding(2);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(40, 40);
            this.BtnRight.TabIndex = 29;
            this.BtnRight.UseVisualStyleBackColor = true;
            this.BtnRight.Click += new System.EventHandler(this.WindowRoiRectangleControl);
            // 
            // BtnUp
            // 
            this.BtnUp.Image = global::GJSControl.Properties.Resources.UpArrow;
            this.BtnUp.Location = new System.Drawing.Point(555, 350);
            this.BtnUp.Margin = new System.Windows.Forms.Padding(2);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(40, 40);
            this.BtnUp.TabIndex = 28;
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.WindowRoiRectangleControl);
            // 
            // BtnLeft
            // 
            this.BtnLeft.Image = global::GJSControl.Properties.Resources.LeftArrow;
            this.BtnLeft.Location = new System.Drawing.Point(515, 390);
            this.BtnLeft.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(40, 40);
            this.BtnLeft.TabIndex = 27;
            this.BtnLeft.UseVisualStyleBackColor = true;
            this.BtnLeft.Click += new System.EventHandler(this.WindowRoiRectangleControl);
            // 
            // comboBoxAlignIndex
            // 
            this.comboBoxAlignIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlignIndex.Font = new System.Drawing.Font("新細明體", 12F);
            this.comboBoxAlignIndex.FormattingEnabled = true;
            this.comboBoxAlignIndex.Location = new System.Drawing.Point(0, 398);
            this.comboBoxAlignIndex.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxAlignIndex.Name = "comboBoxAlignIndex";
            this.comboBoxAlignIndex.Size = new System.Drawing.Size(104, 28);
            this.comboBoxAlignIndex.TabIndex = 45;
            // 
            // comboBoxCorner
            // 
            this.comboBoxCorner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCorner.Font = new System.Drawing.Font("新細明體", 12F);
            this.comboBoxCorner.FormattingEnabled = true;
            this.comboBoxCorner.Items.AddRange(new object[] {
            "Corner1",
            "Corner2"});
            this.comboBoxCorner.Location = new System.Drawing.Point(108, 397);
            this.comboBoxCorner.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxCorner.Name = "comboBoxCorner";
            this.comboBoxCorner.Size = new System.Drawing.Size(108, 28);
            this.comboBoxCorner.TabIndex = 46;
            // 
            // BtnFormula
            // 
            this.BtnFormula.Location = new System.Drawing.Point(475, 362);
            this.BtnFormula.Margin = new System.Windows.Forms.Padding(2);
            this.BtnFormula.Name = "BtnFormula";
            this.BtnFormula.Size = new System.Drawing.Size(40, 70);
            this.BtnFormula.TabIndex = 47;
            this.BtnFormula.Text = "公式";
            this.BtnFormula.UseVisualStyleBackColor = true;
            this.BtnFormula.Click += new System.EventHandler(this.BtnFormula_Click);
            // 
            // BtnCAMProperties
            // 
            this.BtnCAMProperties.Font = new System.Drawing.Font("新細明體", 12F);
            this.BtnCAMProperties.Location = new System.Drawing.Point(385, 397);
            this.BtnCAMProperties.Margin = new System.Windows.Forms.Padding(2);
            this.BtnCAMProperties.Name = "BtnCAMProperties";
            this.BtnCAMProperties.Size = new System.Drawing.Size(90, 35);
            this.BtnCAMProperties.TabIndex = 49;
            this.BtnCAMProperties.Text = "相機屬性";
            this.BtnCAMProperties.UseVisualStyleBackColor = true;
            this.BtnCAMProperties.Click += new System.EventHandler(this.BtnCAMProperties_Click);
            // 
            // ZoomWindow
            // 
            this.ZoomWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ZoomWindow.Location = new System.Drawing.Point(0, 0);
            this.ZoomWindow.Name = "ZoomWindow";
            this.ZoomWindow.Size = new System.Drawing.Size(480, 360);
            this.ZoomWindow.TabIndex = 48;
            // 
            // FmAreaCCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.BtnFormula);
            this.Controls.Add(this.comboBoxCorner);
            this.Controls.Add(this.comboBoxAlignIndex);
            this.Controls.Add(this.btnVisionReload);
            this.Controls.Add(this.BtnAutoLearn);
            this.Controls.Add(this.BtnMakeMask);
            this.Controls.Add(this.BtnCancelRect);
            this.Controls.Add(this.BtnZoomOut);
            this.Controls.Add(this.BtnZoomIn);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.BtnLeft);
            this.Controls.Add(this.numericUpDownScore);
            this.Controls.Add(this.BtnRectCorner);
            this.Controls.Add(this.ZoomWindow);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.BtnSaveImage);
            this.Controls.Add(this.BtnSetExposure);
            this.Controls.Add(this.BtnAlignTest);
            this.Controls.Add(this.BtnLearnCorner);
            this.Controls.Add(this.statusStripInfo);
            this.Controls.Add(this.cbxCCDIndex);
            this.Controls.Add(this.BtnLive);
            this.Controls.Add(this.trackBarExposureTime);
            this.Controls.Add(this.BtnCAMProperties);
            this.Font = new System.Drawing.Font("新細明體", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FmAreaCCD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FmAreaCCD_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FmAreaCCD_FormClosed);
            this.Load += new System.EventHandler(this.FmAreaCCD_Load);
            this.VisibleChanged += new System.EventHandler(this.FmAreaCCD_VisibleChanged);
            this.statusStripInfo.ResumeLayout(false);
            this.statusStripInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposureTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button BtnLive;
        private System.Windows.Forms.ComboBox cbxCCDIndex;
        private System.Windows.Forms.Button BtnLearnCorner;
        private System.Windows.Forms.Button BtnAlignTest;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button BtnSetExposure;
        private System.Windows.Forms.Button BtnSaveImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.NumericUpDown numericUpDownScore;
        private System.Windows.Forms.Button BtnLeft;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Button BtnRight;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnZoomIn;
        private System.Windows.Forms.Button BtnZoomOut;
        private System.Windows.Forms.Button BtnRectCorner;
        private System.Windows.Forms.Button BtnCancelRect;
        private System.Windows.Forms.TrackBar trackBarExposureTime;
		private System.Windows.Forms.Button BtnMakeMask;


	
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStripInfo;

		private System.Windows.Forms.Button BtnAutoLearn;
		private System.Windows.Forms.Button btnVisionReload;
        private System.Windows.Forms.ComboBox comboBoxAlignIndex;
        private System.Windows.Forms.ComboBox comboBoxCorner;
        private System.Windows.Forms.Button BtnFormula;
        private ZoomAndPanWindow ZoomWindow;
        private System.Windows.Forms.Button BtnCAMProperties;
    }
}
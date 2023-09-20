namespace nsUI
{
    partial class FmTBotMove
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
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.BtnStop = new System.Windows.Forms.Button();
            this.NumUD_Speed = new System.Windows.Forms.NumericUpDown();
            this.NumUD_Distance = new System.Windows.Forms.NumericUpDown();
            this.Lbl_Speed = new System.Windows.Forms.Label();
            this.Lbl_MoveDistance = new System.Windows.Forms.Label();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnRight = new System.Windows.Forms.Button();
            this.BtnLeft = new System.Windows.Forms.Button();
            this.BtnUp = new System.Windows.Forms.Button();
            this.Lbl_M1Pos = new System.Windows.Forms.Label();
            this.Lbl_M1 = new System.Windows.Forms.Label();
            this.Lbl_M2Pos = new System.Windows.Forms.Label();
            this.Lbl_M2 = new System.Windows.Forms.Label();
            this.GBox1 = new System.Windows.Forms.GroupBox();
            this.Lbl_X = new System.Windows.Forms.Label();
            this.Lbl_XPos = new System.Windows.Forms.Label();
            this.Lbl_Z = new System.Windows.Forms.Label();
            this.Lbl_ZPos = new System.Windows.Forms.Label();
            this.Lbl_Status = new System.Windows.Forms.Label();
            this.Lbl_Normal = new System.Windows.Forms.Label();
            this.Lbl_AxisZ = new System.Windows.Forms.Label();
            this.Lbl_AxisX = new System.Windows.Forms.Label();
            this.Lbl_Corner = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Speed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Distance)).BeginInit();
            this.GBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(127, 151);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(89, 82);
            this.BtnStop.TabIndex = 10;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // NumUD_Speed
            // 
            this.NumUD_Speed.Location = new System.Drawing.Point(440, 25);
            this.NumUD_Speed.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumUD_Speed.Name = "NumUD_Speed";
            this.NumUD_Speed.Size = new System.Drawing.Size(112, 39);
            this.NumUD_Speed.TabIndex = 36;
            this.NumUD_Speed.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // NumUD_Distance
            // 
            this.NumUD_Distance.Location = new System.Drawing.Point(164, 29);
            this.NumUD_Distance.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NumUD_Distance.Name = "NumUD_Distance";
            this.NumUD_Distance.Size = new System.Drawing.Size(96, 39);
            this.NumUD_Distance.TabIndex = 35;
            this.NumUD_Distance.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Lbl_Speed
            // 
            this.Lbl_Speed.AutoSize = true;
            this.Lbl_Speed.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Speed.Location = new System.Drawing.Point(322, 25);
            this.Lbl_Speed.Name = "Lbl_Speed";
            this.Lbl_Speed.Size = new System.Drawing.Size(129, 34);
            this.Lbl_Speed.TabIndex = 30;
            this.Lbl_Speed.Text = "移動速度:";
            // 
            // Lbl_MoveDistance
            // 
            this.Lbl_MoveDistance.AutoSize = true;
            this.Lbl_MoveDistance.Font = new System.Drawing.Font("微軟正黑體", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_MoveDistance.Location = new System.Drawing.Point(-1, 30);
            this.Lbl_MoveDistance.Name = "Lbl_MoveDistance";
            this.Lbl_MoveDistance.Size = new System.Drawing.Size(197, 34);
            this.Lbl_MoveDistance.TabIndex = 29;
            this.Lbl_MoveDistance.Text = "移動距離(mm):";
            // 
            // BtnDown
            // 
            this.BtnDown.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnDown.Location = new System.Drawing.Point(140, 250);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(60, 60);
            this.BtnDown.TabIndex = 40;
            this.BtnDown.Text = "▼";
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnDown_MouseDown);
            // 
            // BtnRight
            // 
            this.BtnRight.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnRight.Location = new System.Drawing.Point(231, 163);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(60, 60);
            this.BtnRight.TabIndex = 39;
            this.BtnRight.Text = "▶";
            this.BtnRight.UseVisualStyleBackColor = true;
            this.BtnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Right_MouseDown);
            // 
            // BtnLeft
            // 
            this.BtnLeft.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnLeft.Location = new System.Drawing.Point(48, 163);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(60, 60);
            this.BtnLeft.TabIndex = 38;
            this.BtnLeft.Text = "◀";
            this.BtnLeft.UseVisualStyleBackColor = true;
            this.BtnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Left_MouseDown);
            // 
            // BtnUp
            // 
            this.BtnUp.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnUp.Location = new System.Drawing.Point(140, 75);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(60, 60);
            this.BtnUp.TabIndex = 37;
            this.BtnUp.Text = "▲";
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Up_MouseDown);
            // 
            // Lbl_M1Pos
            // 
            this.Lbl_M1Pos.AutoSize = true;
            this.Lbl_M1Pos.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_M1Pos.Location = new System.Drawing.Point(55, 29);
            this.Lbl_M1Pos.Name = "Lbl_M1Pos";
            this.Lbl_M1Pos.Size = new System.Drawing.Size(27, 30);
            this.Lbl_M1Pos.TabIndex = 1;
            this.Lbl_M1Pos.Text = "0";
            // 
            // Lbl_M1
            // 
            this.Lbl_M1.AutoSize = true;
            this.Lbl_M1.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_M1.Location = new System.Drawing.Point(10, 29);
            this.Lbl_M1.Name = "Lbl_M1";
            this.Lbl_M1.Size = new System.Drawing.Size(50, 30);
            this.Lbl_M1.TabIndex = 0;
            this.Lbl_M1.Text = "M1";
            // 
            // Lbl_M2Pos
            // 
            this.Lbl_M2Pos.AutoSize = true;
            this.Lbl_M2Pos.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_M2Pos.Location = new System.Drawing.Point(215, 29);
            this.Lbl_M2Pos.Name = "Lbl_M2Pos";
            this.Lbl_M2Pos.Size = new System.Drawing.Size(27, 30);
            this.Lbl_M2Pos.TabIndex = 3;
            this.Lbl_M2Pos.Text = "0";
            // 
            // Lbl_M2
            // 
            this.Lbl_M2.AutoSize = true;
            this.Lbl_M2.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_M2.Location = new System.Drawing.Point(157, 29);
            this.Lbl_M2.Name = "Lbl_M2";
            this.Lbl_M2.Size = new System.Drawing.Size(50, 30);
            this.Lbl_M2.TabIndex = 2;
            this.Lbl_M2.Text = "M2";
            // 
            // GBox1
            // 
            this.GBox1.Controls.Add(this.Lbl_X);
            this.GBox1.Controls.Add(this.Lbl_XPos);
            this.GBox1.Controls.Add(this.Lbl_Z);
            this.GBox1.Controls.Add(this.Lbl_ZPos);
            this.GBox1.Controls.Add(this.Lbl_M1);
            this.GBox1.Controls.Add(this.Lbl_M1Pos);
            this.GBox1.Controls.Add(this.Lbl_M2);
            this.GBox1.Controls.Add(this.Lbl_M2Pos);
            this.GBox1.Location = new System.Drawing.Point(4, 342);
            this.GBox1.Name = "GBox1";
            this.GBox1.Size = new System.Drawing.Size(548, 65);
            this.GBox1.TabIndex = 41;
            this.GBox1.TabStop = false;
            this.GBox1.Text = "坐標";
            // 
            // Lbl_X
            // 
            this.Lbl_X.AutoSize = true;
            this.Lbl_X.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_X.Location = new System.Drawing.Point(338, 29);
            this.Lbl_X.Name = "Lbl_X";
            this.Lbl_X.Size = new System.Drawing.Size(27, 30);
            this.Lbl_X.TabIndex = 4;
            this.Lbl_X.Text = "Y";
            // 
            // Lbl_XPos
            // 
            this.Lbl_XPos.AutoSize = true;
            this.Lbl_XPos.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_XPos.Location = new System.Drawing.Point(365, 29);
            this.Lbl_XPos.Name = "Lbl_XPos";
            this.Lbl_XPos.Size = new System.Drawing.Size(27, 30);
            this.Lbl_XPos.TabIndex = 5;
            this.Lbl_XPos.Text = "0";
            // 
            // Lbl_Z
            // 
            this.Lbl_Z.AutoSize = true;
            this.Lbl_Z.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Z.Location = new System.Drawing.Point(461, 29);
            this.Lbl_Z.Name = "Lbl_Z";
            this.Lbl_Z.Size = new System.Drawing.Size(28, 30);
            this.Lbl_Z.TabIndex = 6;
            this.Lbl_Z.Text = "Z";
            // 
            // Lbl_ZPos
            // 
            this.Lbl_ZPos.AutoSize = true;
            this.Lbl_ZPos.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_ZPos.Location = new System.Drawing.Point(489, 29);
            this.Lbl_ZPos.Name = "Lbl_ZPos";
            this.Lbl_ZPos.Size = new System.Drawing.Size(27, 30);
            this.Lbl_ZPos.TabIndex = 7;
            this.Lbl_ZPos.Text = "0";
            // 
            // Lbl_Status
            // 
            this.Lbl_Status.Font = new System.Drawing.Font("微軟正黑體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Status.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.Lbl_Status.Location = new System.Drawing.Point(321, 80);
            this.Lbl_Status.Name = "Lbl_Status";
            this.Lbl_Status.Size = new System.Drawing.Size(83, 40);
            this.Lbl_Status.TabIndex = 42;
            this.Lbl_Status.Text = "狀態:";
            // 
            // Lbl_Normal
            // 
            this.Lbl_Normal.AutoSize = true;
            this.Lbl_Normal.Font = new System.Drawing.Font("微軟正黑體", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_Normal.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.Lbl_Normal.Location = new System.Drawing.Point(463, 80);
            this.Lbl_Normal.Name = "Lbl_Normal";
            this.Lbl_Normal.Size = new System.Drawing.Size(87, 43);
            this.Lbl_Normal.TabIndex = 43;
            this.Lbl_Normal.Text = "正常";
            // 
            // Lbl_AxisZ
            // 
            this.Lbl_AxisZ.AutoSize = true;
            this.Lbl_AxisZ.Location = new System.Drawing.Point(219, 75);
            this.Lbl_AxisZ.Name = "Lbl_AxisZ";
            this.Lbl_AxisZ.Size = new System.Drawing.Size(52, 30);
            this.Lbl_AxisZ.TabIndex = 44;
            this.Lbl_AxisZ.Text = "Z轴";
            // 
            // Lbl_AxisX
            // 
            this.Lbl_AxisX.AutoSize = true;
            this.Lbl_AxisX.Location = new System.Drawing.Point(266, 116);
            this.Lbl_AxisX.Name = "Lbl_AxisX";
            this.Lbl_AxisX.Size = new System.Drawing.Size(51, 30);
            this.Lbl_AxisX.TabIndex = 45;
            this.Lbl_AxisX.Text = "Y轴";
            // 
            // Lbl_Corner
            // 
            this.Lbl_Corner.AutoSize = true;
            this.Lbl_Corner.Location = new System.Drawing.Point(231, 111);
            this.Lbl_Corner.Name = "Lbl_Corner";
            this.Lbl_Corner.Size = new System.Drawing.Size(37, 30);
            this.Lbl_Corner.TabIndex = 46;
            this.Lbl_Corner.Text = "∟";
            // 
            // FmTBotMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 419);
            this.ControlBox = false;
            this.Controls.Add(this.Lbl_Corner);
            this.Controls.Add(this.Lbl_AxisX);
            this.Controls.Add(this.Lbl_AxisZ);
            this.Controls.Add(this.Lbl_Normal);
            this.Controls.Add(this.Lbl_Status);
            this.Controls.Add(this.GBox1);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.BtnLeft);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.NumUD_Speed);
            this.Controls.Add(this.NumUD_Distance);
            this.Controls.Add(this.Lbl_Speed);
            this.Controls.Add(this.Lbl_MoveDistance);
            this.Controls.Add(this.BtnStop);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FmTBotMove";
            this.Text = "手臂移動";
            this.VisibleChanged += new System.EventHandler(this.FmScaraMove_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Speed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Distance)).EndInit();
            this.GBox1.ResumeLayout(false);
            this.GBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.NumericUpDown NumUD_Speed;
        private System.Windows.Forms.NumericUpDown NumUD_Distance;
        private System.Windows.Forms.Label Lbl_Speed;
        private System.Windows.Forms.Label Lbl_MoveDistance;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnRight;
        private System.Windows.Forms.Button BtnLeft;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Label Lbl_M1Pos;
        private System.Windows.Forms.Label Lbl_M1;
        private System.Windows.Forms.Label Lbl_M2Pos;
        private System.Windows.Forms.Label Lbl_M2;
        private System.Windows.Forms.GroupBox GBox1;
        private System.Windows.Forms.Label Lbl_Status;
        private System.Windows.Forms.Label Lbl_Normal;
        private System.Windows.Forms.Label Lbl_AxisZ;
        private System.Windows.Forms.Label Lbl_AxisX;
        private System.Windows.Forms.Label Lbl_Corner;
        private System.Windows.Forms.Label Lbl_X;
        private System.Windows.Forms.Label Lbl_XPos;
        private System.Windows.Forms.Label Lbl_Z;
        private System.Windows.Forms.Label Lbl_ZPos;
    }
}
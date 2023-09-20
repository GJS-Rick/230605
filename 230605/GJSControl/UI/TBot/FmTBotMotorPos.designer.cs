namespace nsUI
{
    partial class FmTBotMotorPos
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
            this.components = new System.ComponentModel.Container();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.DGVMotorPos = new System.Windows.Forms.DataGridView();
            this.BtnSetMotorPos = new System.Windows.Forms.Button();
            this.BtnM1M2AbsMove = new System.Windows.Forms.Button();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.BtnStop = new System.Windows.Forms.Button();
            this.GBox_Pos = new System.Windows.Forms.GroupBox();
            this.Lbl_M2 = new System.Windows.Forms.Label();
            this.Lbl_zPos = new System.Windows.Forms.Label();
            this.Lbl_M1 = new System.Windows.Forms.Label();
            this.Lbl_yPos = new System.Windows.Forms.Label();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnRight = new System.Windows.Forms.Button();
            this.BtnLeft = new System.Windows.Forms.Button();
            this.BtnUp = new System.Windows.Forms.Button();
            this.NumUD_Distance = new System.Windows.Forms.NumericUpDown();
            this.Lbl_MoveDistance = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGVMotorPos)).BeginInit();
            this.GBox_Pos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Distance)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnSave.Location = new System.Drawing.Point(399, 374);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(5);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(114, 36);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "儲存";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnExit.Location = new System.Drawing.Point(664, 435);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(5);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(165, 48);
            this.BtnExit.TabIndex = 1;
            this.BtnExit.Text = "离开";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // dgvMotorPos
            // 
            this.DGVMotorPos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVMotorPos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGVMotorPos.Location = new System.Drawing.Point(14, 14);
            this.DGVMotorPos.Margin = new System.Windows.Forms.Padding(5);
            this.DGVMotorPos.Name = "DGVMotorPos";
            this.DGVMotorPos.RowHeadersWidth = 51;
            this.DGVMotorPos.RowTemplate.Height = 24;
            this.DGVMotorPos.Size = new System.Drawing.Size(524, 275);
            this.DGVMotorPos.TabIndex = 2;
            // 
            // BtnSetMotorPos
            // 
            this.BtnSetMotorPos.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnSetMotorPos.Location = new System.Drawing.Point(12, 374);
            this.BtnSetMotorPos.Margin = new System.Windows.Forms.Padding(5);
            this.BtnSetMotorPos.Name = "BtnSetMotorPos";
            this.BtnSetMotorPos.Size = new System.Drawing.Size(106, 36);
            this.BtnSetMotorPos.TabIndex = 3;
            this.BtnSetMotorPos.Text = "設定點位";
            this.BtnSetMotorPos.UseVisualStyleBackColor = true;
            this.BtnSetMotorPos.Click += new System.EventHandler(this.BtnSetMotorPos_Click);
            // 
            // BtnM1M2AbsMove
            // 
            this.BtnM1M2AbsMove.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnM1M2AbsMove.Location = new System.Drawing.Point(131, 374);
            this.BtnM1M2AbsMove.Margin = new System.Windows.Forms.Padding(5);
            this.BtnM1M2AbsMove.Name = "BtnM1M2AbsMove";
            this.BtnM1M2AbsMove.Size = new System.Drawing.Size(111, 36);
            this.BtnM1M2AbsMove.TabIndex = 5;
            this.BtnM1M2AbsMove.Text = "移至點位";
            this.BtnM1M2AbsMove.UseVisualStyleBackColor = true;
            this.BtnM1M2AbsMove.Click += new System.EventHandler(this.BtnM1M2AbsMove_Click);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // BtnStop
            // 
            this.BtnStop.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnStop.Location = new System.Drawing.Point(276, 374);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(5);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(110, 36);
            this.BtnStop.TabIndex = 7;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // GBox_Pos
            // 
            this.GBox_Pos.Controls.Add(this.Lbl_M2);
            this.GBox_Pos.Controls.Add(this.Lbl_zPos);
            this.GBox_Pos.Controls.Add(this.Lbl_M1);
            this.GBox_Pos.Controls.Add(this.Lbl_yPos);
            this.GBox_Pos.Location = new System.Drawing.Point(12, 292);
            this.GBox_Pos.Name = "GBox_Pos";
            this.GBox_Pos.Size = new System.Drawing.Size(524, 74);
            this.GBox_Pos.TabIndex = 8;
            this.GBox_Pos.TabStop = false;
            this.GBox_Pos.Text = "點位座標(Z,Y)";
            // 
            // Lbl_M2
            // 
            this.Lbl_M2.AutoSize = true;
            this.Lbl_M2.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_M2.Location = new System.Drawing.Point(259, 30);
            this.Lbl_M2.Name = "Lbl_M2";
            this.Lbl_M2.Size = new System.Drawing.Size(76, 30);
            this.Lbl_M2.TabIndex = 9;
            this.Lbl_M2.Text = "Z坐标";
            // 
            // Lbl_zPos
            // 
            this.Lbl_zPos.AutoSize = true;
            this.Lbl_zPos.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_zPos.Location = new System.Drawing.Point(363, 30);
            this.Lbl_zPos.Name = "Lbl_zPos";
            this.Lbl_zPos.Size = new System.Drawing.Size(27, 30);
            this.Lbl_zPos.TabIndex = 10;
            this.Lbl_zPos.Text = "0";
            // 
            // Lbl_M1
            // 
            this.Lbl_M1.AutoSize = true;
            this.Lbl_M1.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_M1.Location = new System.Drawing.Point(10, 30);
            this.Lbl_M1.Name = "Lbl_M1";
            this.Lbl_M1.Size = new System.Drawing.Size(75, 30);
            this.Lbl_M1.TabIndex = 2;
            this.Lbl_M1.Text = "Y坐标";
            // 
            // Lbl_yPos
            // 
            this.Lbl_yPos.AutoSize = true;
            this.Lbl_yPos.Font = new System.Drawing.Font("微軟正黑體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_yPos.Location = new System.Drawing.Point(114, 30);
            this.Lbl_yPos.Name = "Lbl_yPos";
            this.Lbl_yPos.Size = new System.Drawing.Size(27, 30);
            this.Lbl_yPos.TabIndex = 3;
            this.Lbl_yPos.Text = "0";
            // 
            // BtnDown
            // 
            this.BtnDown.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnDown.Location = new System.Drawing.Point(566, 229);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(60, 60);
            this.BtnDown.TabIndex = 50;
            this.BtnDown.Text = "▼";
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // BtnRight
            // 
            this.BtnRight.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnRight.Location = new System.Drawing.Point(566, 155);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(60, 60);
            this.BtnRight.TabIndex = 49;
            this.BtnRight.Text = "▶";
            this.BtnRight.UseVisualStyleBackColor = true;
            this.BtnRight.Click += new System.EventHandler(this.BtnRight_Click);
            // 
            // BtnLeft
            // 
            this.BtnLeft.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnLeft.Location = new System.Drawing.Point(566, 89);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(60, 60);
            this.BtnLeft.TabIndex = 48;
            this.BtnLeft.Text = "◀";
            this.BtnLeft.UseVisualStyleBackColor = true;
            this.BtnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // BtnUp
            // 
            this.BtnUp.Font = new System.Drawing.Font("微軟正黑體", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnUp.Location = new System.Drawing.Point(566, 14);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(60, 60);
            this.BtnUp.TabIndex = 47;
            this.BtnUp.Text = "▲";
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // NumUD_Distance
            // 
            this.NumUD_Distance.Location = new System.Drawing.Point(542, 322);
            this.NumUD_Distance.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NumUD_Distance.Name = "NumUD_Distance";
            this.NumUD_Distance.Size = new System.Drawing.Size(53, 34);
            this.NumUD_Distance.TabIndex = 55;
            this.NumUD_Distance.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Lbl_MoveDistance
            // 
            this.Lbl_MoveDistance.AutoSize = true;
            this.Lbl_MoveDistance.Font = new System.Drawing.Font("微軟正黑體", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Lbl_MoveDistance.Location = new System.Drawing.Point(592, 322);
            this.Lbl_MoveDistance.Name = "Lbl_MoveDistance";
            this.Lbl_MoveDistance.Size = new System.Drawing.Size(46, 24);
            this.Lbl_MoveDistance.TabIndex = 54;
            this.Lbl_MoveDistance.Text = "mm";
            // 
            // FmTBotMotorPos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 422);
            this.ControlBox = false;
            this.Controls.Add(this.NumUD_Distance);
            this.Controls.Add(this.Lbl_MoveDistance);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.BtnLeft);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.GBox_Pos);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnM1M2AbsMove);
            this.Controls.Add(this.BtnSetMotorPos);
            this.Controls.Add(this.DGVMotorPos);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnSave);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FmTBotMotorPos";
            this.Text = "工作点位";
            this.VisibleChanged += new System.EventHandler(this.FmMotorPosData_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DGVMotorPos)).EndInit();
            this.GBox_Pos.ResumeLayout(false);
            this.GBox_Pos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Distance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.DataGridView DGVMotorPos;
        private System.Windows.Forms.Button BtnSetMotorPos;
        private System.Windows.Forms.Button BtnM1M2AbsMove;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.GroupBox GBox_Pos;
        private System.Windows.Forms.Label Lbl_M1;
        private System.Windows.Forms.Label Lbl_yPos;
        private System.Windows.Forms.Label Lbl_M2;
        private System.Windows.Forms.Label Lbl_zPos;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnRight;
        private System.Windows.Forms.Button BtnLeft;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.NumericUpDown NumUD_Distance;
        private System.Windows.Forms.Label Lbl_MoveDistance;
    }
}
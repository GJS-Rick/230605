namespace nsUI
{
    partial class FmScaraPosData
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
            this.BtnSave_S = new System.Windows.Forms.Button();
            this.dgvMotorPos = new System.Windows.Forms.DataGridView();
            this.BtnSetMotorPos = new System.Windows.Forms.Button();
            this.buttonMove = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.buttonLineMove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMotorPos)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnSave_S
            // 
            this.BtnSave_S.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnSave_S.Image = global::GJSControl.Properties.Resources.Save;
            this.BtnSave_S.Location = new System.Drawing.Point(575, 396);
            this.BtnSave_S.Margin = new System.Windows.Forms.Padding(5);
            this.BtnSave_S.Name = "BtnSave_S";
            this.BtnSave_S.Size = new System.Drawing.Size(60, 60);
            this.BtnSave_S.TabIndex = 0;
            this.BtnSave_S.UseVisualStyleBackColor = true;
            this.BtnSave_S.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // dgvMotorPos
            // 
            this.dgvMotorPos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMotorPos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvMotorPos.Location = new System.Drawing.Point(0, 0);
            this.dgvMotorPos.Margin = new System.Windows.Forms.Padding(5);
            this.dgvMotorPos.Name = "dgvMotorPos";
            this.dgvMotorPos.RowHeadersWidth = 25;
            this.dgvMotorPos.RowTemplate.Height = 24;
            this.dgvMotorPos.Size = new System.Drawing.Size(635, 386);
            this.dgvMotorPos.TabIndex = 2;
            // 
            // BtnSetMotorPos
            // 
            this.BtnSetMotorPos.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnSetMotorPos.Location = new System.Drawing.Point(0, 401);
            this.BtnSetMotorPos.Margin = new System.Windows.Forms.Padding(5);
            this.BtnSetMotorPos.Name = "BtnSetMotorPos";
            this.BtnSetMotorPos.Size = new System.Drawing.Size(110, 45);
            this.BtnSetMotorPos.TabIndex = 3;
            this.BtnSetMotorPos.Text = "設定點位";
            this.BtnSetMotorPos.UseVisualStyleBackColor = true;
            this.BtnSetMotorPos.Click += new System.EventHandler(this.BtnSetMotorPos_Click);
            // 
            // buttonMove
            // 
            this.buttonMove.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buttonMove.Location = new System.Drawing.Point(120, 401);
            this.buttonMove.Margin = new System.Windows.Forms.Padding(5);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(110, 45);
            this.buttonMove.TabIndex = 5;
            this.buttonMove.Text = "移至點位";
            this.buttonMove.UseVisualStyleBackColor = true;
            this.buttonMove.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnStop.Location = new System.Drawing.Point(290, 401);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(5);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(135, 45);
            this.BtnStop.TabIndex = 6;
            this.BtnStop.Text = "SCARA停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // buttonLineMove
            // 
            this.buttonLineMove.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buttonLineMove.Location = new System.Drawing.Point(435, 401);
            this.buttonLineMove.Margin = new System.Windows.Forms.Padding(5);
            this.buttonLineMove.Name = "buttonLineMove";
            this.buttonLineMove.Size = new System.Drawing.Size(135, 45);
            this.buttonLineMove.TabIndex = 7;
            this.buttonLineMove.Text = "line點位";
            this.buttonLineMove.UseVisualStyleBackColor = true;
            this.buttonLineMove.Click += new System.EventHandler(this.buttonLineMove_Click);
            // 
            // FmScaraPosData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.buttonLineMove);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.buttonMove);
            this.Controls.Add(this.BtnSetMotorPos);
            this.Controls.Add(this.dgvMotorPos);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FmScaraPosData";
            this.Text = "工作點位";
            this.Load += new System.EventHandler(this.FmMotorPosData_Load);
            this.VisibleChanged += new System.EventHandler(this.FmMotorPosData_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMotorPos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.DataGridView dgvMotorPos;
        private System.Windows.Forms.Button BtnSetMotorPos;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button buttonLineMove;
    }
}
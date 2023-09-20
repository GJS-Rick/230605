namespace nsUIMotorPosData
{
    partial class FmMotorPosData
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
            this.DGVMotorPos = new System.Windows.Forms.DataGridView();
            this.BtnSetMotorPos = new System.Windows.Forms.Button();
            this.BtnPosMove = new System.Windows.Forms.Button();
            this.LblMotorPosTitle = new System.Windows.Forms.Label();
            this.BtnPosStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVMotorPos)).BeginInit();
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
            // DGVMotorPos
            // 
            this.DGVMotorPos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVMotorPos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGVMotorPos.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.DGVMotorPos.Location = new System.Drawing.Point(0, 20);
            this.DGVMotorPos.Margin = new System.Windows.Forms.Padding(5);
            this.DGVMotorPos.Name = "DGVMotorPos";
            this.DGVMotorPos.RowHeadersWidth = 25;
            this.DGVMotorPos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGVMotorPos.RowTemplate.Height = 24;
            this.DGVMotorPos.Size = new System.Drawing.Size(635, 370);
            this.DGVMotorPos.TabIndex = 2;
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
            // BtnPosMove
            // 
            this.BtnPosMove.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnPosMove.Location = new System.Drawing.Point(120, 401);
            this.BtnPosMove.Margin = new System.Windows.Forms.Padding(5);
            this.BtnPosMove.Name = "BtnPosMove";
            this.BtnPosMove.Size = new System.Drawing.Size(110, 45);
            this.BtnPosMove.TabIndex = 4;
            this.BtnPosMove.Text = "移至點位";
            this.BtnPosMove.UseVisualStyleBackColor = true;
            this.BtnPosMove.Click += new System.EventHandler(this.BtnPosMove_Click);
            // 
            // LblMotorPosTitle
            // 
            this.LblMotorPosTitle.AutoSize = true;
            this.LblMotorPosTitle.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold);
            this.LblMotorPosTitle.Location = new System.Drawing.Point(1, 1);
            this.LblMotorPosTitle.Name = "LblMotorPosTitle";
            this.LblMotorPosTitle.Size = new System.Drawing.Size(89, 19);
            this.LblMotorPosTitle.TabIndex = 5;
            this.LblMotorPosTitle.Text = "馬達點位";
            // 
            // BtnPosStop
            // 
            this.BtnPosStop.Location = new System.Drawing.Point(239, 401);
            this.BtnPosStop.Name = "BtnPosStop";
            this.BtnPosStop.Size = new System.Drawing.Size(104, 45);
            this.BtnPosStop.TabIndex = 6;
            this.BtnPosStop.Text = "停止";
            this.BtnPosStop.UseVisualStyleBackColor = true;
            this.BtnPosStop.Click += new System.EventHandler(this.BtnPosStop_Click);
            // 
            // FmMotorPosData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 458);
            this.ControlBox = false;
            this.Controls.Add(this.BtnPosStop);
            this.Controls.Add(this.LblMotorPosTitle);
            this.Controls.Add(this.BtnPosMove);
            this.Controls.Add(this.BtnSetMotorPos);
            this.Controls.Add(this.DGVMotorPos);
            this.Controls.Add(this.BtnSave_S);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FmMotorPosData";
            this.Text = "馬達點位";
            this.Load += new System.EventHandler(this.FmMotorPosData_Load);
            this.VisibleChanged += new System.EventHandler(this.FmMotorPosData_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DGVMotorPos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.DataGridView DGVMotorPos;
        private System.Windows.Forms.Button BtnSetMotorPos;
        private System.Windows.Forms.Button BtnPosMove;
        private System.Windows.Forms.Label LblMotorPosTitle;
        private System.Windows.Forms.Button BtnPosStop;
    }
}
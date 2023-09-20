
namespace nsSequence
{
    partial class FormMotorTeach
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
            this.buttonGo = new System.Windows.Forms.Button();
            this.labelAxisPos = new System.Windows.Forms.Label();
            this.textBoxMoveDistance = new System.Windows.Forms.TextBox();
            this.labelAxis1 = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelAxis2Pos = new System.Windows.Forms.Label();
            this.labelAxis2 = new System.Windows.Forms.Label();
            this.textBoxMoveDistance2 = new System.Windows.Forms.TextBox();
            this.buttonGo2 = new System.Windows.Forms.Button();
            this.labelAxis3 = new System.Windows.Forms.Label();
            this.textBoxMoveDistance3 = new System.Windows.Forms.TextBox();
            this.buttonGo3 = new System.Windows.Forms.Button();
            this.labelAxis3Pos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(310, 8);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(93, 35);
            this.buttonGo.TabIndex = 0;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // labelAxisPos
            // 
            this.labelAxisPos.AutoSize = true;
            this.labelAxisPos.Location = new System.Drawing.Point(15, 23);
            this.labelAxisPos.Name = "labelAxisPos";
            this.labelAxisPos.Size = new System.Drawing.Size(61, 20);
            this.labelAxisPos.TabIndex = 1;
            this.labelAxisPos.Text = "Axis1:0";
            // 
            // textBoxMoveDistance
            // 
            this.textBoxMoveDistance.Location = new System.Drawing.Point(190, 12);
            this.textBoxMoveDistance.Name = "textBoxMoveDistance";
            this.textBoxMoveDistance.Size = new System.Drawing.Size(105, 29);
            this.textBoxMoveDistance.TabIndex = 2;
            // 
            // labelAxis1
            // 
            this.labelAxis1.AutoSize = true;
            this.labelAxis1.Location = new System.Drawing.Point(111, 19);
            this.labelAxis1.Name = "labelAxis1";
            this.labelAxis1.Size = new System.Drawing.Size(73, 20);
            this.labelAxis1.TabIndex = 3;
            this.labelAxis1.Text = "移動距離";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(12, 269);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(110, 35);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(149, 269);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(114, 35);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(290, 269);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(113, 35);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelAxis2Pos
            // 
            this.labelAxis2Pos.AutoSize = true;
            this.labelAxis2Pos.Location = new System.Drawing.Point(15, 66);
            this.labelAxis2Pos.Name = "labelAxis2Pos";
            this.labelAxis2Pos.Size = new System.Drawing.Size(61, 20);
            this.labelAxis2Pos.TabIndex = 7;
            this.labelAxis2Pos.Text = "Axis2:0";
            // 
            // labelAxis2
            // 
            this.labelAxis2.AutoSize = true;
            this.labelAxis2.Location = new System.Drawing.Point(111, 64);
            this.labelAxis2.Name = "labelAxis2";
            this.labelAxis2.Size = new System.Drawing.Size(73, 20);
            this.labelAxis2.TabIndex = 10;
            this.labelAxis2.Text = "移動距離";
            // 
            // textBoxMoveDistance2
            // 
            this.textBoxMoveDistance2.Location = new System.Drawing.Point(190, 57);
            this.textBoxMoveDistance2.Name = "textBoxMoveDistance2";
            this.textBoxMoveDistance2.Size = new System.Drawing.Size(105, 29);
            this.textBoxMoveDistance2.TabIndex = 9;
            // 
            // buttonGo2
            // 
            this.buttonGo2.Location = new System.Drawing.Point(310, 53);
            this.buttonGo2.Name = "buttonGo2";
            this.buttonGo2.Size = new System.Drawing.Size(93, 35);
            this.buttonGo2.TabIndex = 8;
            this.buttonGo2.Text = "Go";
            this.buttonGo2.UseVisualStyleBackColor = true;
            this.buttonGo2.Click += new System.EventHandler(this.buttonGo2_Click);
            // 
            // labelAxis3
            // 
            this.labelAxis3.AutoSize = true;
            this.labelAxis3.Location = new System.Drawing.Point(111, 112);
            this.labelAxis3.Name = "labelAxis3";
            this.labelAxis3.Size = new System.Drawing.Size(73, 20);
            this.labelAxis3.TabIndex = 14;
            this.labelAxis3.Text = "移動距離";
            // 
            // textBoxMoveDistance3
            // 
            this.textBoxMoveDistance3.Location = new System.Drawing.Point(190, 107);
            this.textBoxMoveDistance3.Name = "textBoxMoveDistance3";
            this.textBoxMoveDistance3.Size = new System.Drawing.Size(105, 29);
            this.textBoxMoveDistance3.TabIndex = 13;
            // 
            // buttonGo3
            // 
            this.buttonGo3.Location = new System.Drawing.Point(310, 101);
            this.buttonGo3.Name = "buttonGo3";
            this.buttonGo3.Size = new System.Drawing.Size(93, 35);
            this.buttonGo3.TabIndex = 12;
            this.buttonGo3.Text = "Go";
            this.buttonGo3.UseVisualStyleBackColor = true;
            this.buttonGo3.Click += new System.EventHandler(this.buttonGo3_Click);
            // 
            // labelAxis3Pos
            // 
            this.labelAxis3Pos.AutoSize = true;
            this.labelAxis3Pos.Location = new System.Drawing.Point(15, 114);
            this.labelAxis3Pos.Name = "labelAxis3Pos";
            this.labelAxis3Pos.Size = new System.Drawing.Size(61, 20);
            this.labelAxis3Pos.TabIndex = 11;
            this.labelAxis3Pos.Text = "Axis3:0";
            // 
            // FormMotorTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 316);
            this.Controls.Add(this.labelAxis3);
            this.Controls.Add(this.textBoxMoveDistance3);
            this.Controls.Add(this.buttonGo3);
            this.Controls.Add(this.labelAxis3Pos);
            this.Controls.Add(this.labelAxis2);
            this.Controls.Add(this.textBoxMoveDistance2);
            this.Controls.Add(this.buttonGo2);
            this.Controls.Add(this.labelAxis2Pos);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelAxis1);
            this.Controls.Add(this.textBoxMoveDistance);
            this.Controls.Add(this.labelAxisPos);
            this.Controls.Add(this.buttonGo);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMotorTeach";
            this.Text = "馬達教導";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMotorTeach_FormClosing);
            this.Load += new System.EventHandler(this.FormMotorTeach_Load);
            this.VisibleChanged += new System.EventHandler(this.FormMotorTeach_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Label labelAxisPos;
        private System.Windows.Forms.TextBox textBoxMoveDistance;
        private System.Windows.Forms.Label labelAxis1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelAxis2Pos;
        private System.Windows.Forms.Label labelAxis2;
        private System.Windows.Forms.TextBox textBoxMoveDistance2;
        private System.Windows.Forms.Button buttonGo2;
        private System.Windows.Forms.Label labelAxis3;
        private System.Windows.Forms.TextBox textBoxMoveDistance3;
        private System.Windows.Forms.Button buttonGo3;
        private System.Windows.Forms.Label labelAxis3Pos;
    }
}
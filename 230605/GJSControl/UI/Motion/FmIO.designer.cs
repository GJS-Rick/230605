namespace nsFmMotion
{
    partial class frmIO
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
            this.tmrDIUpdate = new System.Windows.Forms.Timer(this.components);
            this.tabCtrlDIO = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tmrDIUpdate
            // 
            this.tmrDIUpdate.Tick += new System.EventHandler(this.tmrDIUpdate_Tick);
            // 
            // tabCtrlDIO
            // 
            this.tabCtrlDIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtrlDIO.Location = new System.Drawing.Point(0, 0);
            this.tabCtrlDIO.Name = "tabCtrlDIO";
            this.tabCtrlDIO.SelectedIndex = 0;
            this.tabCtrlDIO.Size = new System.Drawing.Size(619, 417);
            this.tabCtrlDIO.TabIndex = 0;
            // 
            // frmIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(619, 417);
            this.Controls.Add(this.tabCtrlDIO);
            this.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIO";
            this.Text = "IO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmIO_FormClosing);
            this.Load += new System.EventHandler(this.frmIO_Load);
            this.Shown += new System.EventHandler(this.frmIO_Shown);
            this.VisibleChanged += new System.EventHandler(this.frmIO_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrDIUpdate;
        private System.Windows.Forms.TabControl tabCtrlDIO;
    }
}
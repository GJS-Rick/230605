namespace GJSControl.UI
{
    partial class FmWebCamMonikerString
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
            this.dataGridViewWebCamMonikerString = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWebCamMonikerString)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewWebCamMonikerString
            // 
            this.dataGridViewWebCamMonikerString.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWebCamMonikerString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewWebCamMonikerString.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewWebCamMonikerString.Name = "dataGridViewWebCamMonikerString";
            this.dataGridViewWebCamMonikerString.RowTemplate.Height = 24;
            this.dataGridViewWebCamMonikerString.Size = new System.Drawing.Size(413, 354);
            this.dataGridViewWebCamMonikerString.TabIndex = 0;
            // 
            // FmWebCamMonikerString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 354);
            this.Controls.Add(this.dataGridViewWebCamMonikerString);
            this.Name = "FmWebCamMonikerString";
            this.Text = "FmWebCamMonikerString";
            this.Load += new System.EventHandler(this.FmWebCamMonikerString_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWebCamMonikerString)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewWebCamMonikerString;
    }
}
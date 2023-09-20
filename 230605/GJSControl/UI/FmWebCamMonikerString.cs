using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GJSControl.UI
{
    public partial class FmWebCamMonikerString : Form
    {
        public FmWebCamMonikerString(int NumberOfWebCam, string[] MonikerString)
        {
            InitializeComponent();

            int firstColumnWidth = 50;
            dataGridViewWebCamMonikerString.RowCount = NumberOfWebCam;
            dataGridViewWebCamMonikerString.ColumnCount = 2;

            dataGridViewWebCamMonikerString.Columns[0].HeaderText = "Index";
            dataGridViewWebCamMonikerString.Columns[0].Width = firstColumnWidth;

            dataGridViewWebCamMonikerString.Columns[1].HeaderText = "Moniker string";
            dataGridViewWebCamMonikerString.Columns[1].Width = this.Width - firstColumnWidth;

            for(int i = 0; i < NumberOfWebCam; i++)
            {
                dataGridViewWebCamMonikerString.Rows[i].Cells[0].Value = i.ToString();
                dataGridViewWebCamMonikerString.Rows[i].Cells[1].Value = MonikerString[i];
            }
        }

        private void FmWebCamMonikerString_Load(object sender, EventArgs e)
        {

        }
    }
}

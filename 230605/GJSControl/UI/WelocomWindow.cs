using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GJSControl
{
    public partial class WelcomeWindow : Form
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            pictureBox1.Update();
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;

        }
        public void ShowMessage(string Message)
        {
            label1.Text = Message;
            label1.Update();
            pictureBox1.Update();
            Thread.Sleep(50);
        }
    }
}

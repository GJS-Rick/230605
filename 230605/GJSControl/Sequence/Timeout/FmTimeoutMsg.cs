using System;
using System.Drawing;
using System.Windows.Forms;

namespace nsSequence
{
    public partial class FmTimeoutMsg : Form
    {
        private String _ErrorCode;
        private String _Description;

        private Button[] _Btn;
        private TimeoutDef _timeout;

        public FmTimeoutMsg(BaseStep.BtnType eType, String ErrorCode, String Description, TimeoutDef timeout)
        {
            _ErrorCode = ErrorCode;
            _Description = Description;

            _Btn = new Button[(int)eType];

            for (int i = 0; i < _Btn.Length; i++)
            {
                _Btn[i] = new Button();

                if (i == 0)//第一個
                {
                    _Btn[i].Name = "BtnRetry";
                    _Btn[i].Click += new EventHandler(BtnRetry_Click);
                    _Btn[i].Location = new Point(70, 403);
                    _Btn[i].Text = "重試";
                }
                else if (i == _Btn.Length - 1)//最後一個
                {
                    _Btn[i].Name = "BtnStop";
                    _Btn[i].Click += new EventHandler(BtnStop_Click);
                    _Btn[i].Location = new Point(433, 403);
                    _Btn[i].Text = "中止";
                }
                else//其他
                {
                    switch (eType)
                    {
                        case BaseStep.BtnType.RetryIgnoreAbort:
                            _Btn[i].Name = "BtnIgnore";
                            _Btn[i].Click += new EventHandler(BtnIgnore_Click);
                            _Btn[i].Location = new Point(252, 403);
                            _Btn[i].Text = "忽略";
                            break;
                    }
                }

                _Btn[i].BackColor = Button.DefaultBackColor;
                _Btn[i].TextAlign = ContentAlignment.MiddleCenter;
                _Btn[i].Font = new Font("微軟正黑體", 15);
                _Btn[i].Size = new Size(120, 40);

                this.Controls.Add(_Btn[i]);
            }

            _timeout = timeout;
            InitializeComponent();
        }

        private void BtnRetry_Click(object sender, EventArgs e)
        {
            _timeout.Result = DialogResult.Retry;
            Close();
        }
        private void BtnStop_Click(object sender, EventArgs e)
        {
            _timeout.Result = DialogResult.Cancel;
            Close();
        }
        private void BtnIgnore_Click(object sender, EventArgs e)
        {
            _timeout.Result = DialogResult.Ignore;
            Close();
        }

        private void FmTimeoutMsg_Shown(object sender, EventArgs e)
        {
            labCode.Text = _ErrorCode.ToString();
            rtbDescription.Text = _Description;
        }
    }
}

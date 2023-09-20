using System;
using System.Windows.Forms;
using CommonLibrary;

namespace nsUI
{
    public partial class FmLogin : Form
    {
        private bool _ChangeLevel;
        public FmLogin()
        {
            _ChangeLevel = false;
            InitializeComponent();

            cbxUserID.Items.Clear();
            for (int i = 0; i < G.Comm.Login.GetLevelNum(); i++)
                cbxUserID.Items.Add(G.Comm.Login.GetName((ELoginLevel)i));

            cbxUserID.SelectedIndex = (int)G.Comm.UserLv;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (cbxUserID.SelectedIndex < 0)
                return;

            if (!G.Comm.Login.CheckPW((ELoginLevel)cbxUserID.SelectedIndex, maskTxtPw.Text))
                return;

            G.Comm.Login.SetLevel((ELoginLevel)cbxUserID.SelectedIndex);
            _ChangeLevel = true;

            G.Comm.Login.UIEnable();

            G.Comm.KillVirtualKeyboard();
            Visible = false;
        }

        public bool ChangeLevel()
        {
            return _ChangeLevel;
        }

        public void SetLevelDone()
        {
            _ChangeLevel = false;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            G.Comm.Login.UIEnable();
            G.Comm.KillVirtualKeyboard();
            Visible = false;
        }

        private void MaskTxtPw_Click(object sender, EventArgs e)
        {
            G.Comm.Keyboard.Call();
        }

        private void FmLogin_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                maskTxtPw.Text = "";
        }

        private void FmLogin_Load(object sender, EventArgs e)
        {

        }

        public ComboBox GetcbxUserID()
        {
            return this.cbxUserID;
        }
    }
}

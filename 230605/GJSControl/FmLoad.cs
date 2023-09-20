using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nsObjects;
using nsMsgFm;
using nsUI;
using nsSequence;

namespace GJSControl
{
    public partial class FmLoad : Form
    {
        public cObjManagerDef m_cObjMngr;
        public cUIManagerDef m_cUIMngr;
        public cSequenceManagerDef m_cSequenceMngr;

        public FmLoad()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (m_cObjMngr.m_cLogin.sGetCurrentPW().ToUpper() != maskTxtPw.Text.ToUpper())
            {
                nsMsgFm.cMsgFormDef.vAdd(
                    nsMsgFm.frmMsg.EFORM_STYLE.FORM_SHOW,
                    nsMsgFm.frmMsg.EBTN_STYLE.BTN_OK,
                    nsMsgFm.frmMsg.EMSG_TYPE.MSG_MSG,
                    "密碼錯誤!\n PW is Wrong",
                    true);
                return;
            }


            if (cbxUserID.SelectedIndex < 0)
            {
                nsMsgFm.cMsgFormDef.vAdd(
                       nsMsgFm.frmMsg.EFORM_STYLE.FORM_SHOW,
                       nsMsgFm.frmMsg.EBTN_STYLE.BTN_OK,
                       nsMsgFm.frmMsg.EMSG_TYPE.MSG_MSG,
                       "請選擇使用者帳號!\n Select user ID please",
                       true);

                return;
            }



            m_cSequenceMngr = new cSequenceManagerDef(this, m_cObjMngr);
            m_cUIMngr = new cUIManagerDef(m_cObjMngr, m_cSequenceMngr, this);
            m_cSequenceMngr.vSetUIManager(m_cUIMngr);
            m_cObjMngr.m_cMachineStatus.vInitial(m_cSequenceMngr, m_cUIMngr, m_cObjMngr);




            m_cObjMngr.m_cMainThread = new nsThread.cMainThreadDef(m_cSequenceMngr);
            m_cObjMngr.m_cShowThread = new nsThread.cShowThreadDef(m_cObjMngr);


            m_cUIMngr.frmMainFm.Visible = true;
            //     m_cUIMngr.frmMainFm.TopMost = true;

            Visible = false;

            //     m_cUIMngr.frmMainFm.TopMost = false;
        }

        private void FmLoad_Shown(object sender, EventArgs e)
        {
            m_cObjMngr = new cObjManagerDef(this);

            cbxUserID.Items.Clear();
            for (int i = 0; i < m_cObjMngr.m_cLogin.nGetLevelNum(); i++)
                cbxUserID.Items.Add(m_cObjMngr.m_cLogin.sGetName((nsLogin.enuLoginLevel)i));

            cbxUserID.SelectedIndex = (int)m_cObjMngr.m_cLogin.eGetLevel();

        }

        private void FmLoad_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_cObjMngr != null && m_cObjMngr.m_cMainThread != null)
            {
                m_cObjMngr.m_cMainThread.vDestroy();
                m_cObjMngr.m_cMainThread = null;
            }

            if (m_cObjMngr != null && m_cObjMngr.m_cShowThread != null)
            {
                m_cObjMngr.m_cShowThread.vDestroy();
                m_cObjMngr.m_cShowThread = null;
            }

            if (m_cUIMngr != null)
                m_cUIMngr.Dispose();

            if (m_cSequenceMngr != null)
                m_cSequenceMngr.Dispose();

            if (m_cObjMngr != null)
                m_cObjMngr.Dispose();
        }

        private void cbxUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxUserID.SelectedIndex < 0)
                return;

            m_cObjMngr.m_cLogin.vSetLevel((nsLogin.enuLoginLevel)cbxUserID.SelectedIndex);
        }


    }
}

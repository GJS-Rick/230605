using System;
using System.Drawing;
using System.Windows.Forms;
using CommonLibrary;

namespace nsFmMotion
{
    public partial class frmMotion : Form
    {

        public frmMotion()
        {
            InitializeComponent();
        }

        private void frmMotion_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);
            TabPageSetColor(TabCtl);

            // Reflesh AxisNo 
            cbxAxisNo.Items.Clear();
            for (int i = 0; i < G.Comm.MtnCtrl.GetAxisNum(); i++) 
            {
                cbxAxisNo.Items.Add("NULL");
                cbxAxisNo.Items[i] =  G.Comm.MtnCtrl.GetAxisName((EAXIS_NAME)i).ToString();
            }

            // Reflesh Speed Type 
            cbxSpdTyp.Items.Clear();
            for (int i = 0; i < (int) ESPEED_TYPE.SPEED_COUNT; i++)
            {
                cbxSpdTyp.Items.Add("NULL");
                cbxSpdTyp.Items[i] = ((ESPEED_TYPE) i).ToString();
            }

            cbxAlmLogic.Items.Clear();
            cbxELLogic.Items.Clear();
            cbxServoLogic.Items.Clear();
            cbxOrgLogic.Items.Clear();
            for (int i = 0; i < (int)AxisBaseDef.EACITVE_LOGIC.Count; i++)
            {
                cbxAlmLogic.Items.Add(((AxisBaseDef.EACITVE_LOGIC) i).ToString());
                cbxELLogic.Items.Add(((AxisBaseDef.EACITVE_LOGIC)i).ToString());
                cbxServoLogic.Items.Add(((AxisBaseDef.EACITVE_LOGIC)i).ToString());
                cbxOrgLogic.Items.Add(((AxisBaseDef.EACITVE_LOGIC)i).ToString());
            }

            cbxFBSrc.Items.Clear();
            for (int i = 0; i < (int)AxisBaseDef.EFEEDBACK_SRC.Count; i++)
                cbxFBSrc.Items.Add(((AxisBaseDef.EFEEDBACK_SRC)i).ToString());

            cbxHmMode.Items.Clear();
            for (int i = 0; i < (int)AxisBaseDef.EHOME_MODE.Count; i++)
                cbxHmMode.Items.Add(((AxisBaseDef.EHOME_MODE)i).ToString());


            cbxPlsIptMode.Items.Clear();
            for (int i = 0; i < (int)AxisBaseDef.EPULSE_INPUT_MODE.Count; i++)
                cbxPlsIptMode.Items.Add(((AxisBaseDef.EPULSE_INPUT_MODE)i).ToString());

            cbxPlsOptMode.Items.Clear();
            for (int i = 0; i < (int)AxisBaseDef.EPULSE_OUTPUT_MODE.Count; i++)
                cbxPlsOptMode.Items.Add(((AxisBaseDef.EPULSE_OUTPUT_MODE)i).ToString());

            cbxELMode.Items.Clear();
            for (int i = 0; i < (int)AxisBaseDef.ESTOP_MODE.Count; i++)
                cbxELMode.Items.Add(((AxisBaseDef.ESTOP_MODE)i).ToString());

            cbxFBReverse.Items.Clear();
            cbxFBReverse.Items.Add("False");
            cbxFBReverse.Items.Add("True");
        }

        private void vRefreshAxisSet(EAXIS_NAME eAxis)
        {
            cbxAlmLogic.SelectedIndex = (int)G.Comm.MtnCtrl.GetAlmLogic(eAxis);

            cbxELLogic.SelectedIndex = (int)G.Comm.MtnCtrl.GetELLogic(eAxis);

            cbxOrgLogic.SelectedIndex = (int)G.Comm.MtnCtrl.GetOrgLogic(eAxis);

            cbxServoLogic.SelectedIndex = (int)G.Comm.MtnCtrl.GetServoLogic(eAxis);

            if (G.Comm.MtnCtrl.GetIptReverse(eAxis))
                cbxFBReverse.SelectedIndex = 1;
            else
                cbxFBReverse.SelectedIndex = 0;

            cbxPlsIptMode.SelectedIndex = (int)G.Comm.MtnCtrl.GetPlsIptMode(eAxis);

            cbxPlsOptMode.SelectedIndex = (int)G.Comm.MtnCtrl.GetPlsOptMode(eAxis);

            txtBxHmVel.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetHmVel(eAxis));

            txtBxEGear.Text = String.Format("{0:0}", G.Comm.MtnCtrl.GetEGear(eAxis));

            txtBxEncoderRes.Text = String.Format("{0:0}", G.Comm.MtnCtrl.GetEncoderRes(eAxis));

            cbxFBSrc.SelectedIndex = (int)G.Comm.MtnCtrl.GetFbSrc(eAxis);

            cbxELMode.SelectedIndex = (int)G.Comm.MtnCtrl.GetELMode(eAxis);

            cbxHmMode.SelectedIndex = (int)G.Comm.MtnCtrl.GetHmMode(eAxis);

            txtBxPlsRto.Text = String.Format("{0:0.0000000000}", G.Comm.MtnCtrl.GetPlsRto(eAxis));
            txtBxSMELSet.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSMELVal(eAxis));
            txtBxSPELSet.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSPELVal(eAxis));
            chkSELEnb.Checked = G.Comm.MtnCtrl.GetSoftELEnable(eAxis);
            chkELEnable.Checked = G.Comm.MtnCtrl.GetELEnable(eAxis);
        }

        private void vRefreshSpdSet(ESPEED_TYPE eSpdTye, EAXIS_NAME eAxis)
        {
            txtBxStartSpd.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSpd(eSpdTye, eAxis)._StartSpd);
            txtBxMaxSpd.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSpd(eSpdTye, eAxis)._MaxSpd);
            txtBxEndSpd.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSpd(eSpdTye, eAxis)._EndSpd);
            txtBxAccRate.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSpd(eSpdTye, eAxis)._AccRate);
            txtBxDecRate.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetSpd(eSpdTye, eAxis)._DecRate);
        }

        private void cbxAxisNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAxisNo.SelectedIndex < 0)
                return;

            vRefreshAxisSet((EAXIS_NAME)cbxAxisNo.SelectedIndex);

            if (cbxSpdTyp.SelectedIndex < 0)
                return;

            vRefreshSpdSet(((ESPEED_TYPE)cbxSpdTyp.SelectedIndex), (EAXIS_NAME)cbxAxisNo.SelectedIndex);
        }

        private void cbxSpdTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxAxisNo.SelectedIndex < 0)
                return;
            if (cbxSpdTyp.SelectedIndex < 0)
                return;

            vRefreshSpdSet(((ESPEED_TYPE)cbxSpdTyp.SelectedIndex), (EAXIS_NAME)cbxAxisNo.SelectedIndex);
        }

        private void tmrIOUpdate_Tick(object sender, EventArgs e)
        {
            if (cbxAxisNo.SelectedIndex < 0)
                return;

            EAXIS_NAME eAxis = (EAXIS_NAME)cbxAxisNo.SelectedIndex;
            bool balm = G.Comm.MtnCtrl.GetAlm(eAxis, true);
            labelAlm.BackColor = G.Comm.MtnCtrl.GetAlm(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelEmg.BackColor = G.Comm.MtnCtrl.GetEmg(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelOrg.BackColor = G.Comm.MtnCtrl.GetOrg(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelStop.BackColor = G.Comm.MtnCtrl.Stop(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelPEL.BackColor = G.Comm.MtnCtrl.GetPEL(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelMEL.BackColor = G.Comm.MtnCtrl.GetMEL(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelSPEL.BackColor = G.Comm.MtnCtrl.GetSoftPEL(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelSMEL.BackColor = G.Comm.MtnCtrl.GetSoftMEL(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            labelInp.BackColor = G.Comm.MtnCtrl.GetINP(eAxis, true) ? Color.Lime : Color.DarkSeaGreen;
            lblPos.Text = String.Format("{0:0.0000}", G.Comm.MtnCtrl.GetPos(eAxis, 0.1));
        }

        #region BtnJog
        private void BtnJogLft_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (cbxAxisNo.SelectedIndex < 0)
                    return;
                if (cbxSpdTyp.SelectedIndex < 0)
                    return;

                if (G.Comm.MtnCtrl.Stop((EAXIS_NAME)cbxAxisNo.SelectedIndex, true))
                    G.Comm.MtnCtrl.ConMv((EAXIS_NAME)cbxAxisNo.SelectedIndex, false, (ESPEED_TYPE)cbxSpdTyp.SelectedIndex);
            }
            catch(Exception ex)
            {
                AlarmTextDisplay.Add((int)AlarmCode.Alarm, AlarmType.Alarm, ex.Message);
            }
            
        }
        private void BtnJogLft_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbxAxisNo.SelectedIndex < 0)
                return;

            //if (! G.Common.MtnCtrl.Stop((EAXIS_NAME)cbxAxisNo.SelectedIndex, true))
            G.Comm.MtnCtrl.SdStop((EAXIS_NAME)cbxAxisNo.SelectedIndex);
        }
        private void BtnJogRht_MouseDown(object sender, MouseEventArgs e)
        {
            try
            { 
                if (cbxAxisNo.SelectedIndex < 0)
                    return;
                if (cbxSpdTyp.SelectedIndex < 0)
                    return;

                if (G.Comm.MtnCtrl.Stop((EAXIS_NAME)cbxAxisNo.SelectedIndex, true))
                    G.Comm.MtnCtrl.ConMv((EAXIS_NAME)cbxAxisNo.SelectedIndex, true, (ESPEED_TYPE)cbxSpdTyp.SelectedIndex);
            }
            catch (Exception ex)
            {
                AlarmTextDisplay.Add((int)AlarmCode.Alarm, AlarmType.Alarm, ex.Message);
            }
        }
        private void BtnJogRht_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbxAxisNo.SelectedIndex < 0)
                return;

            //if (! G.Common.MtnCtrl.Stop((EAXIS_NAME)cbxAxisNo.SelectedIndex, true))
            G.Comm.MtnCtrl.SdStop((EAXIS_NAME)cbxAxisNo.SelectedIndex);
        }
        #endregion

        private void TabPageSetColor(TabControl TabCtl)
        {
            for (int i = 0; i < TabCtl.TabPages.Count; i++)
                TabCtl.TabPages[i].BackColor = Color.FromArgb(0xdd2378);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if(cbxAxisNo.SelectedIndex < 0)
                return;

            //if (! G.Common.MtnCtrl.Stop((EAXIS_NAME)cbxAxisNo.SelectedIndex, true))
            G.Comm.MtnCtrl.SdStop((EAXIS_NAME)cbxAxisNo.SelectedIndex);
        }

        private void BtnMv_Click(object sender, EventArgs e)
        {
            if(cbxAxisNo.SelectedIndex < 0)
                return;
            if(cbxSpdTyp.SelectedIndex < 0)
                return;

            double fPos = 0;
            if (double.TryParse(txtBxMvPos.Text, out fPos))
            {
                try
                {
                    G.Comm.MtnCtrl.AbsMv(
                    (EAXIS_NAME)cbxAxisNo.SelectedIndex,
                    fPos,
                    (ESPEED_TYPE)cbxSpdTyp.SelectedIndex);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void chkSELEnb_CheckedChanged(object sender, EventArgs e)
        {
            if(cbxAxisNo.SelectedIndex < 0)
            {
               
                return;
            }

            G.Comm.MtnCtrl.SetSoftELEnable((EAXIS_NAME)cbxAxisNo.SelectedIndex, chkSELEnb.Checked);
            
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cbxSpdTyp.SelectedIndex < 0 || cbxAxisNo.SelectedIndex < 0)
                return;

            double fStartSpd = 0, fMaxSpd = 0, fEndSpd = 0, fAccRate = 0, fDecRate = 0;
            double fPlsRto = 0, fSMEL = 0, fSPEL = 0, fHmVel = 0;
            int nEGear = 0, nEncoderRes = 0;

            if (!double.TryParse(txtBxStartSpd.Text, out fStartSpd) ||
                !double.TryParse(txtBxMaxSpd.Text, out fMaxSpd) ||
                !double.TryParse(txtBxEndSpd.Text, out fEndSpd) ||
                !double.TryParse(txtBxAccRate.Text, out fAccRate) ||
                !double.TryParse(txtBxDecRate.Text, out fDecRate) ||
                !double.TryParse(txtBxPlsRto.Text, out fPlsRto) ||
                !double.TryParse(txtBxSMELSet.Text, out fSMEL) ||
                !double.TryParse(txtBxSPELSet.Text, out fSPEL) ||
                !double.TryParse(txtBxHmVel.Text, out fHmVel) ||
                !int.TryParse(txtBxEGear.Text, out nEGear) ||
                !int.TryParse(txtBxEncoderRes.Text, out nEncoderRes))
            {
                AlarmTextDisplay.Add(
                    (int)AlarmCode.Machine_ParmError,
                    AlarmType.Warning,
                        "儲存失敗!請檢查輸入數值");
                return;
            }
            else
                AlarmTextDisplay.Add(
                    (int)AlarmCode.Success,
                    AlarmType.Warning,
                        "儲存成功");

            SpeedDef stSpd = G.Comm.MtnCtrl.GetSpd(
                (ESPEED_TYPE)cbxSpdTyp.SelectedIndex,
                (EAXIS_NAME)cbxAxisNo.SelectedIndex);

            stSpd._StartSpd = fStartSpd;
            stSpd._MaxSpd = fMaxSpd;
            stSpd._EndSpd = fEndSpd;
            stSpd._AccRate = fAccRate;
            stSpd._DecRate = fDecRate;


            G.Comm.MtnCtrl.SetSpd(
               (ESPEED_TYPE)cbxSpdTyp.SelectedIndex,
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               stSpd);

            G.Comm.MtnCtrl.SetAlmLogic(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EACITVE_LOGIC)cbxAlmLogic.SelectedIndex);

            G.Comm.MtnCtrl.SetELLogic(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EACITVE_LOGIC)cbxELLogic.SelectedIndex);

            G.Comm.MtnCtrl.SetOrgLogic(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EACITVE_LOGIC)cbxOrgLogic.SelectedIndex);

            G.Comm.MtnCtrl.SetServoLogic(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EACITVE_LOGIC)cbxServoLogic.SelectedIndex);

            G.Comm.MtnCtrl.SetIptReverse(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (cbxFBReverse.SelectedIndex > 0));

            G.Comm.MtnCtrl.SetPlsIptMode(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EPULSE_INPUT_MODE)cbxPlsIptMode.SelectedIndex);

            G.Comm.MtnCtrl.SetPlsOptMode(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EPULSE_OUTPUT_MODE)cbxPlsOptMode.SelectedIndex);

            G.Comm.MtnCtrl.SetFbSrc(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EFEEDBACK_SRC)cbxFBSrc.SelectedIndex);

            G.Comm.MtnCtrl.SetHmVel(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               fHmVel);

            G.Comm.MtnCtrl.SetHmMode(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.EHOME_MODE)cbxHmMode.SelectedIndex);

            G.Comm.MtnCtrl.SetEGear(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               nEGear);

            G.Comm.MtnCtrl.SetEncoderRes(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               nEncoderRes);

            G.Comm.MtnCtrl.SetELMode(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               (AxisBaseDef.ESTOP_MODE)cbxELMode.SelectedIndex);

            G.Comm.MtnCtrl.SetPlsRto(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               fPlsRto);

            G.Comm.MtnCtrl.SetSMELVal(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               fSMEL);

            G.Comm.MtnCtrl.SetSPELVal(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               fSPEL);

            G.Comm.MtnCtrl.SetSoftELEnable(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               chkSELEnb.Checked);

            G.Comm.MtnCtrl.SetELEnable(
               (EAXIS_NAME)cbxAxisNo.SelectedIndex,
               chkELEnable.Checked);

            G.Comm.MtnCtrl.WriteFile();
        }

        private void frmMotion_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmMotion_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == false)
                tmrIOUpdate.Enabled = false;
            else
                tmrIOUpdate.Enabled = true;
        }

        private void BtnHomeMinus_Click(object sender, EventArgs e)
        {
            G.Comm.MtnCtrl.HmMv((EAXIS_NAME)cbxAxisNo.SelectedIndex, false);
        }

        private void BtnHomPlus_Click(object sender, EventArgs e)
        {
            G.Comm.MtnCtrl.HmMv((EAXIS_NAME)cbxAxisNo.SelectedIndex, true);
        }

        private void buttonSON_Click(object sender, EventArgs e)
        {
            G.Comm.MtnCtrl.SetServoOn((EAXIS_NAME)cbxAxisNo.SelectedIndex, true);
        }

        private void buttonSOFF_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("請確認煞車是否有做動，若將Servo Off軸失去保持力且在無煞車狀態可能導致快速落下後撞擊，若要繼續將Servo Off，請點擊確認", "SERVO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            G.Comm.MtnCtrl.SetServoOn((EAXIS_NAME)cbxAxisNo.SelectedIndex, false);
        }

        private void BtnJogRht_Click(object sender, EventArgs e)
        {

        }

        private void chkELEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxAxisNo.SelectedIndex < 0)
            {
                return;
            }

            G.Comm.MtnCtrl.EnableEL((EAXIS_NAME)cbxAxisNo.SelectedIndex, chkELEnable.Checked);
          
        }
    }
}

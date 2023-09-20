namespace nsFmMotion
{
    partial class frmMotion
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
            this.cbxAxisNo = new System.Windows.Forms.ComboBox();
            this.lblPos = new System.Windows.Forms.Label();
            this.LblPosHdr = new System.Windows.Forms.Label();
            this.cbxSpdTyp = new System.Windows.Forms.ComboBox();
            this.LblDecRateHdr = new System.Windows.Forms.Label();
            this.txtBxDecRate = new System.Windows.Forms.TextBox();
            this.LblAccRateHdr = new System.Windows.Forms.Label();
            this.txtBxAccRate = new System.Windows.Forms.TextBox();
            this.LblEndSpdHdr = new System.Windows.Forms.Label();
            this.txtBxEndSpd = new System.Windows.Forms.TextBox();
            this.LblMaxSpdHdr = new System.Windows.Forms.Label();
            this.txtBxMaxSpd = new System.Windows.Forms.TextBox();
            this.LblStartSpdHdr = new System.Windows.Forms.Label();
            this.txtBxStartSpd = new System.Windows.Forms.TextBox();
            this.cbxFBSrc = new System.Windows.Forms.ComboBox();
            this.LblFBSrcHdr = new System.Windows.Forms.Label();
            this.LblHmVel = new System.Windows.Forms.Label();
            this.txtBxHmVel = new System.Windows.Forms.TextBox();
            this.cbxHmMode = new System.Windows.Forms.ComboBox();
            this.LblHmMode = new System.Windows.Forms.Label();
            this.cbxELMode = new System.Windows.Forms.ComboBox();
            this.LblELModHdr = new System.Windows.Forms.Label();
            this.LblEncoderResHdr = new System.Windows.Forms.Label();
            this.txtBxEncoderRes = new System.Windows.Forms.TextBox();
            this.LblEGearHdr = new System.Windows.Forms.Label();
            this.txtBxEGear = new System.Windows.Forms.TextBox();
            this.cbxPlsOptMode = new System.Windows.Forms.ComboBox();
            this.LblPlsOptModHdr = new System.Windows.Forms.Label();
            this.cbxPlsIptMode = new System.Windows.Forms.ComboBox();
            this.LblPlsIptModHdr = new System.Windows.Forms.Label();
            this.cbxFBReverse = new System.Windows.Forms.ComboBox();
            this.LblFBReverseHdr = new System.Windows.Forms.Label();
            this.cbxServoLogic = new System.Windows.Forms.ComboBox();
            this.LblServoLogicHdr = new System.Windows.Forms.Label();
            this.cbxOrgLogic = new System.Windows.Forms.ComboBox();
            this.LblOrgLogicHdr = new System.Windows.Forms.Label();
            this.cbxELLogic = new System.Windows.Forms.ComboBox();
            this.LblELLogicHdr = new System.Windows.Forms.Label();
            this.cbxAlmLogic = new System.Windows.Forms.ComboBox();
            this.LblAlmLogicHdr = new System.Windows.Forms.Label();
            this.chkSELEnb = new System.Windows.Forms.CheckBox();
            this.LblSMELSetHdr = new System.Windows.Forms.Label();
            this.txtBxSMELSet = new System.Windows.Forms.TextBox();
            this.LblSPELSetHdr = new System.Windows.Forms.Label();
            this.txtBxSPELSet = new System.Windows.Forms.TextBox();
            this.LblPlsRtoHdr = new System.Windows.Forms.Label();
            this.txtBxPlsRto = new System.Windows.Forms.TextBox();
            this.GBoxStatus = new System.Windows.Forms.GroupBox();
            this.TLP_Status = new System.Windows.Forms.TableLayoutPanel();
            this.labelAlm = new System.Windows.Forms.Label();
            this.labelEmg = new System.Windows.Forms.Label();
            this.LblAlmHdr = new System.Windows.Forms.Label();
            this.LblEmgHdr = new System.Windows.Forms.Label();
            this.labelStop = new System.Windows.Forms.Label();
            this.labelOrg = new System.Windows.Forms.Label();
            this.labelPEL = new System.Windows.Forms.Label();
            this.labelMEL = new System.Windows.Forms.Label();
            this.labelSPEL = new System.Windows.Forms.Label();
            this.LblStopHdr = new System.Windows.Forms.Label();
            this.labelSMEL = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LblOrgHdr = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelInp = new System.Windows.Forms.Label();
            this.LblPELHdr = new System.Windows.Forms.Label();
            this.LblInpHdr = new System.Windows.Forms.Label();
            this.LblMELHdr = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.txtBxMvPos = new System.Windows.Forms.TextBox();
            this.BtnJogRht = new System.Windows.Forms.Button();
            this.BtnMv = new System.Windows.Forms.Button();
            this.BtnJogLft = new System.Windows.Forms.Button();
            this.BtnSave_S = new System.Windows.Forms.Button();
            this.tmrIOUpdate = new System.Windows.Forms.Timer(this.components);
            this.BtnHomeMinus = new System.Windows.Forms.Button();
            this.BtnHomPlus = new System.Windows.Forms.Button();
            this.TabCtl = new System.Windows.Forms.TabControl();
            this.SpeedPage = new System.Windows.Forms.TabPage();
            this.buttonSOFF = new System.Windows.Forms.Button();
            this.buttonSON = new System.Windows.Forms.Button();
            this.AxisPage = new System.Windows.Forms.TabPage();
            this.chkELEnable = new System.Windows.Forms.CheckBox();
            this.LblMotionTitle = new System.Windows.Forms.Label();
            this.TLP_Pos_1 = new System.Windows.Forms.TableLayoutPanel();
            this.TLP_Pos = new System.Windows.Forms.TableLayoutPanel();
            this.TLP_pnlMove = new System.Windows.Forms.TableLayoutPanel();
            this.TLP_pnlMove_1 = new System.Windows.Forms.TableLayoutPanel();
            this.GBoxStatus.SuspendLayout();
            this.TLP_Status.SuspendLayout();
            this.TabCtl.SuspendLayout();
            this.SpeedPage.SuspendLayout();
            this.AxisPage.SuspendLayout();
            this.TLP_Pos_1.SuspendLayout();
            this.TLP_Pos.SuspendLayout();
            this.TLP_pnlMove.SuspendLayout();
            this.TLP_pnlMove_1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxAxisNo
            // 
            this.cbxAxisNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAxisNo.FormattingEnabled = true;
            this.cbxAxisNo.Location = new System.Drawing.Point(3, 94);
            this.cbxAxisNo.Margin = new System.Windows.Forms.Padding(4);
            this.cbxAxisNo.Name = "cbxAxisNo";
            this.cbxAxisNo.Size = new System.Drawing.Size(234, 24);
            this.cbxAxisNo.TabIndex = 0;
            this.cbxAxisNo.SelectedIndexChanged += new System.EventHandler(this.cbxAxisNo_SelectedIndexChanged);
            // 
            // lblPos
            // 
            this.lblPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPos.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPos.Location = new System.Drawing.Point(147, 2);
            this.lblPos.Margin = new System.Windows.Forms.Padding(1);
            this.lblPos.Name = "lblPos";
            this.lblPos.Size = new System.Drawing.Size(144, 22);
            this.lblPos.TabIndex = 3;
            this.lblPos.Text = "0";
            this.lblPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblPosHdr
            // 
            this.LblPosHdr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LblPosHdr.Font = new System.Drawing.Font("新細明體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblPosHdr.Location = new System.Drawing.Point(1, 1);
            this.LblPosHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblPosHdr.Name = "LblPosHdr";
            this.LblPosHdr.Size = new System.Drawing.Size(144, 24);
            this.LblPosHdr.TabIndex = 2;
            this.LblPosHdr.Text = "目前位置 :";
            this.LblPosHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxSpdTyp
            // 
            this.cbxSpdTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSpdTyp.FormattingEnabled = true;
            this.cbxSpdTyp.Location = new System.Drawing.Point(6, 6);
            this.cbxSpdTyp.Margin = new System.Windows.Forms.Padding(4);
            this.cbxSpdTyp.Name = "cbxSpdTyp";
            this.cbxSpdTyp.Size = new System.Drawing.Size(160, 24);
            this.cbxSpdTyp.TabIndex = 4;
            this.cbxSpdTyp.SelectedIndexChanged += new System.EventHandler(this.cbxSpdTyp_SelectedIndexChanged);
            // 
            // LblDecRateHdr
            // 
            this.LblDecRateHdr.Location = new System.Drawing.Point(6, 175);
            this.LblDecRateHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblDecRateHdr.Name = "LblDecRateHdr";
            this.LblDecRateHdr.Size = new System.Drawing.Size(84, 21);
            this.LblDecRateHdr.TabIndex = 11;
            this.LblDecRateHdr.Text = "減速度";
            this.LblDecRateHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxDecRate
            // 
            this.txtBxDecRate.Location = new System.Drawing.Point(95, 172);
            this.txtBxDecRate.Name = "txtBxDecRate";
            this.txtBxDecRate.Size = new System.Drawing.Size(107, 27);
            this.txtBxDecRate.TabIndex = 10;
            // 
            // LblAccRateHdr
            // 
            this.LblAccRateHdr.Location = new System.Drawing.Point(6, 145);
            this.LblAccRateHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAccRateHdr.Name = "LblAccRateHdr";
            this.LblAccRateHdr.Size = new System.Drawing.Size(84, 21);
            this.LblAccRateHdr.TabIndex = 9;
            this.LblAccRateHdr.Text = "加速度";
            this.LblAccRateHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxAccRate
            // 
            this.txtBxAccRate.Location = new System.Drawing.Point(95, 142);
            this.txtBxAccRate.Name = "txtBxAccRate";
            this.txtBxAccRate.Size = new System.Drawing.Size(107, 27);
            this.txtBxAccRate.TabIndex = 8;
            // 
            // LblEndSpdHdr
            // 
            this.LblEndSpdHdr.Location = new System.Drawing.Point(6, 115);
            this.LblEndSpdHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEndSpdHdr.Name = "LblEndSpdHdr";
            this.LblEndSpdHdr.Size = new System.Drawing.Size(84, 21);
            this.LblEndSpdHdr.TabIndex = 7;
            this.LblEndSpdHdr.Text = "結束速度";
            this.LblEndSpdHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxEndSpd
            // 
            this.txtBxEndSpd.Location = new System.Drawing.Point(95, 112);
            this.txtBxEndSpd.Name = "txtBxEndSpd";
            this.txtBxEndSpd.Size = new System.Drawing.Size(107, 27);
            this.txtBxEndSpd.TabIndex = 6;
            // 
            // LblMaxSpdHdr
            // 
            this.LblMaxSpdHdr.Location = new System.Drawing.Point(6, 85);
            this.LblMaxSpdHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblMaxSpdHdr.Name = "LblMaxSpdHdr";
            this.LblMaxSpdHdr.Size = new System.Drawing.Size(84, 21);
            this.LblMaxSpdHdr.TabIndex = 5;
            this.LblMaxSpdHdr.Text = "最大速度";
            this.LblMaxSpdHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxMaxSpd
            // 
            this.txtBxMaxSpd.Location = new System.Drawing.Point(95, 82);
            this.txtBxMaxSpd.Name = "txtBxMaxSpd";
            this.txtBxMaxSpd.Size = new System.Drawing.Size(107, 27);
            this.txtBxMaxSpd.TabIndex = 4;
            // 
            // LblStartSpdHdr
            // 
            this.LblStartSpdHdr.Location = new System.Drawing.Point(6, 55);
            this.LblStartSpdHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblStartSpdHdr.Name = "LblStartSpdHdr";
            this.LblStartSpdHdr.Size = new System.Drawing.Size(84, 21);
            this.LblStartSpdHdr.TabIndex = 3;
            this.LblStartSpdHdr.Text = "起始速度";
            this.LblStartSpdHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxStartSpd
            // 
            this.txtBxStartSpd.Location = new System.Drawing.Point(95, 52);
            this.txtBxStartSpd.Name = "txtBxStartSpd";
            this.txtBxStartSpd.Size = new System.Drawing.Size(107, 27);
            this.txtBxStartSpd.TabIndex = 2;
            // 
            // cbxFBSrc
            // 
            this.cbxFBSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFBSrc.FormattingEnabled = true;
            this.cbxFBSrc.Location = new System.Drawing.Point(95, 173);
            this.cbxFBSrc.Margin = new System.Windows.Forms.Padding(4);
            this.cbxFBSrc.Name = "cbxFBSrc";
            this.cbxFBSrc.Size = new System.Drawing.Size(107, 24);
            this.cbxFBSrc.TabIndex = 39;
            // 
            // LblFBSrcHdr
            // 
            this.LblFBSrcHdr.Location = new System.Drawing.Point(6, 175);
            this.LblFBSrcHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblFBSrcHdr.Name = "LblFBSrcHdr";
            this.LblFBSrcHdr.Size = new System.Drawing.Size(84, 21);
            this.LblFBSrcHdr.TabIndex = 38;
            this.LblFBSrcHdr.Text = "FB Src";
            this.LblFBSrcHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblHmVel
            // 
            this.LblHmVel.Location = new System.Drawing.Point(426, 25);
            this.LblHmVel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblHmVel.Name = "LblHmVel";
            this.LblHmVel.Size = new System.Drawing.Size(84, 21);
            this.LblHmVel.TabIndex = 37;
            this.LblHmVel.Text = "Home Vel";
            this.LblHmVel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxHmVel
            // 
            this.txtBxHmVel.Location = new System.Drawing.Point(515, 22);
            this.txtBxHmVel.Name = "txtBxHmVel";
            this.txtBxHmVel.Size = new System.Drawing.Size(107, 27);
            this.txtBxHmVel.TabIndex = 36;
            // 
            // cbxHmMode
            // 
            this.cbxHmMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxHmMode.FormattingEnabled = true;
            this.cbxHmMode.Location = new System.Drawing.Point(305, 175);
            this.cbxHmMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxHmMode.Name = "cbxHmMode";
            this.cbxHmMode.Size = new System.Drawing.Size(107, 24);
            this.cbxHmMode.TabIndex = 35;
            // 
            // LblHmMode
            // 
            this.LblHmMode.Location = new System.Drawing.Point(216, 175);
            this.LblHmMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblHmMode.Name = "LblHmMode";
            this.LblHmMode.Size = new System.Drawing.Size(84, 21);
            this.LblHmMode.TabIndex = 34;
            this.LblHmMode.Text = "Home Mode";
            this.LblHmMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxELMode
            // 
            this.cbxELMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxELMode.FormattingEnabled = true;
            this.cbxELMode.Location = new System.Drawing.Point(305, 145);
            this.cbxELMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxELMode.Name = "cbxELMode";
            this.cbxELMode.Size = new System.Drawing.Size(107, 24);
            this.cbxELMode.TabIndex = 33;
            // 
            // LblELModHdr
            // 
            this.LblELModHdr.Location = new System.Drawing.Point(216, 145);
            this.LblELModHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblELModHdr.Name = "LblELModHdr";
            this.LblELModHdr.Size = new System.Drawing.Size(84, 21);
            this.LblELModHdr.TabIndex = 32;
            this.LblELModHdr.Text = "EL Mode";
            this.LblELModHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblEncoderResHdr
            // 
            this.LblEncoderResHdr.Location = new System.Drawing.Point(216, 115);
            this.LblEncoderResHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEncoderResHdr.Name = "LblEncoderResHdr";
            this.LblEncoderResHdr.Size = new System.Drawing.Size(84, 21);
            this.LblEncoderResHdr.TabIndex = 31;
            this.LblEncoderResHdr.Text = "Endr Res";
            this.LblEncoderResHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxEncoderRes
            // 
            this.txtBxEncoderRes.Location = new System.Drawing.Point(305, 113);
            this.txtBxEncoderRes.Name = "txtBxEncoderRes";
            this.txtBxEncoderRes.Size = new System.Drawing.Size(107, 27);
            this.txtBxEncoderRes.TabIndex = 30;
            // 
            // LblEGearHdr
            // 
            this.LblEGearHdr.Location = new System.Drawing.Point(216, 85);
            this.LblEGearHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEGearHdr.Name = "LblEGearHdr";
            this.LblEGearHdr.Size = new System.Drawing.Size(84, 21);
            this.LblEGearHdr.TabIndex = 29;
            this.LblEGearHdr.Text = "EGear";
            this.LblEGearHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxEGear
            // 
            this.txtBxEGear.Location = new System.Drawing.Point(305, 83);
            this.txtBxEGear.Name = "txtBxEGear";
            this.txtBxEGear.Size = new System.Drawing.Size(107, 27);
            this.txtBxEGear.TabIndex = 28;
            // 
            // cbxPlsOptMode
            // 
            this.cbxPlsOptMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPlsOptMode.FormattingEnabled = true;
            this.cbxPlsOptMode.Location = new System.Drawing.Point(305, 55);
            this.cbxPlsOptMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxPlsOptMode.Name = "cbxPlsOptMode";
            this.cbxPlsOptMode.Size = new System.Drawing.Size(107, 24);
            this.cbxPlsOptMode.TabIndex = 27;
            // 
            // LblPlsOptModHdr
            // 
            this.LblPlsOptModHdr.Location = new System.Drawing.Point(216, 55);
            this.LblPlsOptModHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPlsOptModHdr.Name = "LblPlsOptModHdr";
            this.LblPlsOptModHdr.Size = new System.Drawing.Size(84, 21);
            this.LblPlsOptModHdr.TabIndex = 26;
            this.LblPlsOptModHdr.Text = "Pls OPT Mod";
            this.LblPlsOptModHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxPlsIptMode
            // 
            this.cbxPlsIptMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPlsIptMode.FormattingEnabled = true;
            this.cbxPlsIptMode.Location = new System.Drawing.Point(305, 25);
            this.cbxPlsIptMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxPlsIptMode.Name = "cbxPlsIptMode";
            this.cbxPlsIptMode.Size = new System.Drawing.Size(107, 24);
            this.cbxPlsIptMode.TabIndex = 25;
            // 
            // LblPlsIptModHdr
            // 
            this.LblPlsIptModHdr.Location = new System.Drawing.Point(216, 25);
            this.LblPlsIptModHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPlsIptModHdr.Name = "LblPlsIptModHdr";
            this.LblPlsIptModHdr.Size = new System.Drawing.Size(84, 21);
            this.LblPlsIptModHdr.TabIndex = 24;
            this.LblPlsIptModHdr.Text = "Pls IPT Mod";
            this.LblPlsIptModHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxFBReverse
            // 
            this.cbxFBReverse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFBReverse.FormattingEnabled = true;
            this.cbxFBReverse.Location = new System.Drawing.Point(95, 143);
            this.cbxFBReverse.Margin = new System.Windows.Forms.Padding(4);
            this.cbxFBReverse.Name = "cbxFBReverse";
            this.cbxFBReverse.Size = new System.Drawing.Size(107, 24);
            this.cbxFBReverse.TabIndex = 23;
            // 
            // LblFBReverseHdr
            // 
            this.LblFBReverseHdr.Location = new System.Drawing.Point(6, 145);
            this.LblFBReverseHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblFBReverseHdr.Name = "LblFBReverseHdr";
            this.LblFBReverseHdr.Size = new System.Drawing.Size(84, 21);
            this.LblFBReverseHdr.TabIndex = 22;
            this.LblFBReverseHdr.Text = "IPT Reverse";
            this.LblFBReverseHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxServoLogic
            // 
            this.cbxServoLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxServoLogic.FormattingEnabled = true;
            this.cbxServoLogic.Location = new System.Drawing.Point(95, 113);
            this.cbxServoLogic.Margin = new System.Windows.Forms.Padding(4);
            this.cbxServoLogic.Name = "cbxServoLogic";
            this.cbxServoLogic.Size = new System.Drawing.Size(107, 24);
            this.cbxServoLogic.TabIndex = 21;
            // 
            // LblServoLogicHdr
            // 
            this.LblServoLogicHdr.Location = new System.Drawing.Point(6, 115);
            this.LblServoLogicHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblServoLogicHdr.Name = "LblServoLogicHdr";
            this.LblServoLogicHdr.Size = new System.Drawing.Size(84, 21);
            this.LblServoLogicHdr.TabIndex = 20;
            this.LblServoLogicHdr.Text = "Servo Logic";
            this.LblServoLogicHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxOrgLogic
            // 
            this.cbxOrgLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOrgLogic.FormattingEnabled = true;
            this.cbxOrgLogic.Location = new System.Drawing.Point(95, 83);
            this.cbxOrgLogic.Margin = new System.Windows.Forms.Padding(4);
            this.cbxOrgLogic.Name = "cbxOrgLogic";
            this.cbxOrgLogic.Size = new System.Drawing.Size(107, 24);
            this.cbxOrgLogic.TabIndex = 19;
            // 
            // LblOrgLogicHdr
            // 
            this.LblOrgLogicHdr.Location = new System.Drawing.Point(6, 85);
            this.LblOrgLogicHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblOrgLogicHdr.Name = "LblOrgLogicHdr";
            this.LblOrgLogicHdr.Size = new System.Drawing.Size(84, 21);
            this.LblOrgLogicHdr.TabIndex = 18;
            this.LblOrgLogicHdr.Text = "Org Logic";
            this.LblOrgLogicHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxELLogic
            // 
            this.cbxELLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxELLogic.FormattingEnabled = true;
            this.cbxELLogic.Location = new System.Drawing.Point(95, 53);
            this.cbxELLogic.Margin = new System.Windows.Forms.Padding(4);
            this.cbxELLogic.Name = "cbxELLogic";
            this.cbxELLogic.Size = new System.Drawing.Size(107, 24);
            this.cbxELLogic.TabIndex = 17;
            // 
            // LblELLogicHdr
            // 
            this.LblELLogicHdr.Location = new System.Drawing.Point(6, 55);
            this.LblELLogicHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblELLogicHdr.Name = "LblELLogicHdr";
            this.LblELLogicHdr.Size = new System.Drawing.Size(84, 21);
            this.LblELLogicHdr.TabIndex = 16;
            this.LblELLogicHdr.Text = "EL Logic";
            this.LblELLogicHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxAlmLogic
            // 
            this.cbxAlmLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAlmLogic.FormattingEnabled = true;
            this.cbxAlmLogic.Location = new System.Drawing.Point(95, 23);
            this.cbxAlmLogic.Margin = new System.Windows.Forms.Padding(4);
            this.cbxAlmLogic.Name = "cbxAlmLogic";
            this.cbxAlmLogic.Size = new System.Drawing.Size(107, 24);
            this.cbxAlmLogic.TabIndex = 15;
            // 
            // LblAlmLogicHdr
            // 
            this.LblAlmLogicHdr.Location = new System.Drawing.Point(6, 25);
            this.LblAlmLogicHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAlmLogicHdr.Name = "LblAlmLogicHdr";
            this.LblAlmLogicHdr.Size = new System.Drawing.Size(84, 21);
            this.LblAlmLogicHdr.TabIndex = 14;
            this.LblAlmLogicHdr.Text = "Alm Logic";
            this.LblAlmLogicHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkSELEnb
            // 
            this.chkSELEnb.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSELEnb.Location = new System.Drawing.Point(492, 183);
            this.chkSELEnb.Name = "chkSELEnb";
            this.chkSELEnb.Size = new System.Drawing.Size(127, 20);
            this.chkSELEnb.TabIndex = 12;
            this.chkSELEnb.Text = "啟動軟體極限";
            this.chkSELEnb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSELEnb.UseVisualStyleBackColor = true;
            this.chkSELEnb.CheckedChanged += new System.EventHandler(this.chkSELEnb_CheckedChanged);
            // 
            // LblSMELSetHdr
            // 
            this.LblSMELSetHdr.Location = new System.Drawing.Point(426, 115);
            this.LblSMELSetHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSMELSetHdr.Name = "LblSMELSetHdr";
            this.LblSMELSetHdr.Size = new System.Drawing.Size(84, 21);
            this.LblSMELSetHdr.TabIndex = 11;
            this.LblSMELSetHdr.Text = "軟體負極限";
            this.LblSMELSetHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxSMELSet
            // 
            this.txtBxSMELSet.Location = new System.Drawing.Point(515, 112);
            this.txtBxSMELSet.Name = "txtBxSMELSet";
            this.txtBxSMELSet.Size = new System.Drawing.Size(107, 27);
            this.txtBxSMELSet.TabIndex = 10;
            // 
            // LblSPELSetHdr
            // 
            this.LblSPELSetHdr.Location = new System.Drawing.Point(426, 85);
            this.LblSPELSetHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblSPELSetHdr.Name = "LblSPELSetHdr";
            this.LblSPELSetHdr.Size = new System.Drawing.Size(84, 21);
            this.LblSPELSetHdr.TabIndex = 9;
            this.LblSPELSetHdr.Text = "軟體正極限";
            this.LblSPELSetHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxSPELSet
            // 
            this.txtBxSPELSet.Location = new System.Drawing.Point(515, 82);
            this.txtBxSPELSet.Name = "txtBxSPELSet";
            this.txtBxSPELSet.Size = new System.Drawing.Size(107, 27);
            this.txtBxSPELSet.TabIndex = 8;
            // 
            // LblPlsRtoHdr
            // 
            this.LblPlsRtoHdr.Location = new System.Drawing.Point(426, 55);
            this.LblPlsRtoHdr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPlsRtoHdr.Name = "LblPlsRtoHdr";
            this.LblPlsRtoHdr.Size = new System.Drawing.Size(84, 21);
            this.LblPlsRtoHdr.TabIndex = 3;
            this.LblPlsRtoHdr.Text = "Pulse單位";
            this.LblPlsRtoHdr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBxPlsRto
            // 
            this.txtBxPlsRto.Location = new System.Drawing.Point(515, 52);
            this.txtBxPlsRto.Name = "txtBxPlsRto";
            this.txtBxPlsRto.Size = new System.Drawing.Size(107, 27);
            this.txtBxPlsRto.TabIndex = 2;
            // 
            // GBoxStatus
            // 
            this.GBoxStatus.Controls.Add(this.TLP_Status);
            this.GBoxStatus.Font = new System.Drawing.Font("新細明體", 12F);
            this.GBoxStatus.Location = new System.Drawing.Point(0, 20);
            this.GBoxStatus.Name = "GBoxStatus";
            this.GBoxStatus.Size = new System.Drawing.Size(535, 70);
            this.GBoxStatus.TabIndex = 7;
            this.GBoxStatus.TabStop = false;
            this.GBoxStatus.Text = "I/O";
            // 
            // TLP_Status
            // 
            this.TLP_Status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TLP_Status.ColumnCount = 9;
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.TLP_Status.Controls.Add(this.labelAlm, 0, 1);
            this.TLP_Status.Controls.Add(this.labelEmg, 1, 1);
            this.TLP_Status.Controls.Add(this.LblAlmHdr, 0, 0);
            this.TLP_Status.Controls.Add(this.LblEmgHdr, 1, 0);
            this.TLP_Status.Controls.Add(this.labelStop, 2, 1);
            this.TLP_Status.Controls.Add(this.labelOrg, 3, 1);
            this.TLP_Status.Controls.Add(this.labelPEL, 4, 1);
            this.TLP_Status.Controls.Add(this.labelMEL, 5, 1);
            this.TLP_Status.Controls.Add(this.labelSPEL, 6, 1);
            this.TLP_Status.Controls.Add(this.LblStopHdr, 2, 0);
            this.TLP_Status.Controls.Add(this.labelSMEL, 7, 1);
            this.TLP_Status.Controls.Add(this.label4, 6, 0);
            this.TLP_Status.Controls.Add(this.LblOrgHdr, 3, 0);
            this.TLP_Status.Controls.Add(this.label3, 7, 0);
            this.TLP_Status.Controls.Add(this.labelInp, 8, 1);
            this.TLP_Status.Controls.Add(this.LblPELHdr, 4, 0);
            this.TLP_Status.Controls.Add(this.LblInpHdr, 8, 0);
            this.TLP_Status.Controls.Add(this.LblMELHdr, 5, 0);
            this.TLP_Status.Location = new System.Drawing.Point(6, 18);
            this.TLP_Status.Margin = new System.Windows.Forms.Padding(1);
            this.TLP_Status.Name = "TLP_Status";
            this.TLP_Status.RowCount = 2;
            this.TLP_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Status.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Status.Size = new System.Drawing.Size(523, 46);
            this.TLP_Status.TabIndex = 27;
            // 
            // labelAlm
            // 
            this.labelAlm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelAlm.BackColor = System.Drawing.Color.DarkGreen;
            this.labelAlm.Location = new System.Drawing.Point(19, 24);
            this.labelAlm.Margin = new System.Windows.Forms.Padding(1);
            this.labelAlm.Name = "labelAlm";
            this.labelAlm.Size = new System.Drawing.Size(20, 21);
            this.labelAlm.TabIndex = 16;
            this.labelAlm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEmg
            // 
            this.labelEmg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelEmg.BackColor = System.Drawing.Color.DarkGreen;
            this.labelEmg.Location = new System.Drawing.Point(77, 24);
            this.labelEmg.Margin = new System.Windows.Forms.Padding(1);
            this.labelEmg.Name = "labelEmg";
            this.labelEmg.Size = new System.Drawing.Size(20, 21);
            this.labelEmg.TabIndex = 17;
            this.labelEmg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblAlmHdr
            // 
            this.LblAlmHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblAlmHdr.Location = new System.Drawing.Point(9, 4);
            this.LblAlmHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblAlmHdr.Name = "LblAlmHdr";
            this.LblAlmHdr.Size = new System.Drawing.Size(40, 18);
            this.LblAlmHdr.TabIndex = 8;
            this.LblAlmHdr.Text = "Alm";
            this.LblAlmHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblEmgHdr
            // 
            this.LblEmgHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblEmgHdr.Location = new System.Drawing.Point(67, 4);
            this.LblEmgHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblEmgHdr.Name = "LblEmgHdr";
            this.LblEmgHdr.Size = new System.Drawing.Size(40, 18);
            this.LblEmgHdr.TabIndex = 15;
            this.LblEmgHdr.Text = "Emg";
            this.LblEmgHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStop
            // 
            this.labelStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelStop.BackColor = System.Drawing.Color.DarkGreen;
            this.labelStop.Location = new System.Drawing.Point(135, 24);
            this.labelStop.Margin = new System.Windows.Forms.Padding(1);
            this.labelStop.Name = "labelStop";
            this.labelStop.Size = new System.Drawing.Size(20, 21);
            this.labelStop.TabIndex = 18;
            this.labelStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOrg
            // 
            this.labelOrg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelOrg.BackColor = System.Drawing.Color.DarkGreen;
            this.labelOrg.Location = new System.Drawing.Point(193, 24);
            this.labelOrg.Margin = new System.Windows.Forms.Padding(1);
            this.labelOrg.Name = "labelOrg";
            this.labelOrg.Size = new System.Drawing.Size(20, 21);
            this.labelOrg.TabIndex = 19;
            this.labelOrg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPEL
            // 
            this.labelPEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelPEL.BackColor = System.Drawing.Color.DarkGreen;
            this.labelPEL.Location = new System.Drawing.Point(251, 24);
            this.labelPEL.Margin = new System.Windows.Forms.Padding(1);
            this.labelPEL.Name = "labelPEL";
            this.labelPEL.Size = new System.Drawing.Size(20, 21);
            this.labelPEL.TabIndex = 20;
            this.labelPEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMEL
            // 
            this.labelMEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelMEL.BackColor = System.Drawing.Color.DarkGreen;
            this.labelMEL.Location = new System.Drawing.Point(309, 24);
            this.labelMEL.Margin = new System.Windows.Forms.Padding(1);
            this.labelMEL.Name = "labelMEL";
            this.labelMEL.Size = new System.Drawing.Size(20, 21);
            this.labelMEL.TabIndex = 21;
            this.labelMEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSPEL
            // 
            this.labelSPEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelSPEL.BackColor = System.Drawing.Color.DarkGreen;
            this.labelSPEL.Location = new System.Drawing.Point(367, 24);
            this.labelSPEL.Margin = new System.Windows.Forms.Padding(1);
            this.labelSPEL.Name = "labelSPEL";
            this.labelSPEL.Size = new System.Drawing.Size(20, 21);
            this.labelSPEL.TabIndex = 25;
            this.labelSPEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblStopHdr
            // 
            this.LblStopHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblStopHdr.Location = new System.Drawing.Point(125, 4);
            this.LblStopHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblStopHdr.Name = "LblStopHdr";
            this.LblStopHdr.Size = new System.Drawing.Size(40, 18);
            this.LblStopHdr.TabIndex = 9;
            this.LblStopHdr.Text = "Stop";
            this.LblStopHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSMEL
            // 
            this.labelSMEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelSMEL.BackColor = System.Drawing.Color.DarkGreen;
            this.labelSMEL.Location = new System.Drawing.Point(425, 24);
            this.labelSMEL.Margin = new System.Windows.Forms.Padding(1);
            this.labelSMEL.Name = "labelSMEL";
            this.labelSMEL.Size = new System.Drawing.Size(20, 21);
            this.labelSMEL.TabIndex = 26;
            this.labelSMEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.Location = new System.Drawing.Point(357, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 18);
            this.label4.TabIndex = 23;
            this.label4.Text = "SEL+";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblOrgHdr
            // 
            this.LblOrgHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblOrgHdr.Location = new System.Drawing.Point(183, 4);
            this.LblOrgHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblOrgHdr.Name = "LblOrgHdr";
            this.LblOrgHdr.Size = new System.Drawing.Size(40, 18);
            this.LblOrgHdr.TabIndex = 10;
            this.LblOrgHdr.Text = "Org";
            this.LblOrgHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.Location = new System.Drawing.Point(415, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 18);
            this.label3.TabIndex = 24;
            this.label3.Text = "SEL-";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInp
            // 
            this.labelInp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelInp.BackColor = System.Drawing.Color.DarkGreen;
            this.labelInp.Location = new System.Drawing.Point(483, 24);
            this.labelInp.Margin = new System.Windows.Forms.Padding(1);
            this.labelInp.Name = "labelInp";
            this.labelInp.Size = new System.Drawing.Size(20, 21);
            this.labelInp.TabIndex = 22;
            this.labelInp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblPELHdr
            // 
            this.LblPELHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblPELHdr.Location = new System.Drawing.Point(241, 4);
            this.LblPELHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblPELHdr.Name = "LblPELHdr";
            this.LblPELHdr.Size = new System.Drawing.Size(40, 18);
            this.LblPELHdr.TabIndex = 11;
            this.LblPELHdr.Text = "EL+";
            this.LblPELHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblInpHdr
            // 
            this.LblInpHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblInpHdr.Location = new System.Drawing.Point(473, 4);
            this.LblInpHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblInpHdr.Name = "LblInpHdr";
            this.LblInpHdr.Size = new System.Drawing.Size(40, 18);
            this.LblInpHdr.TabIndex = 13;
            this.LblInpHdr.Text = "INP";
            this.LblInpHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblMELHdr
            // 
            this.LblMELHdr.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LblMELHdr.Location = new System.Drawing.Point(299, 4);
            this.LblMELHdr.Margin = new System.Windows.Forms.Padding(1);
            this.LblMELHdr.Name = "LblMELHdr";
            this.LblMELHdr.Size = new System.Drawing.Size(40, 18);
            this.LblMELHdr.TabIndex = 12;
            this.LblMELHdr.Text = "EL-";
            this.LblMELHdr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnStop
            // 
            this.BtnStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnStop.Location = new System.Drawing.Point(197, 3);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(1);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 45);
            this.BtnStop.TabIndex = 17;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // txtBxMvPos
            // 
            this.txtBxMvPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBxMvPos.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtBxMvPos.Location = new System.Drawing.Point(1, 12);
            this.txtBxMvPos.Margin = new System.Windows.Forms.Padding(1);
            this.txtBxMvPos.Name = "txtBxMvPos";
            this.txtBxMvPos.Size = new System.Drawing.Size(102, 27);
            this.txtBxMvPos.TabIndex = 16;
            // 
            // BtnJogRht
            // 
            this.BtnJogRht.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnJogRht.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnJogRht.Location = new System.Drawing.Point(341, 3);
            this.BtnJogRht.Margin = new System.Windows.Forms.Padding(1);
            this.BtnJogRht.Name = "BtnJogRht";
            this.BtnJogRht.Size = new System.Drawing.Size(45, 45);
            this.BtnJogRht.TabIndex = 15;
            this.BtnJogRht.Text = "+";
            this.BtnJogRht.UseVisualStyleBackColor = true;
            this.BtnJogRht.Click += new System.EventHandler(this.BtnJogRht_Click);
            this.BtnJogRht.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogRht_MouseDown);
            this.BtnJogRht.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogRht_MouseUp);
            // 
            // BtnMv
            // 
            this.BtnMv.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnMv.Location = new System.Drawing.Point(110, 3);
            this.BtnMv.Margin = new System.Windows.Forms.Padding(1);
            this.BtnMv.Name = "BtnMv";
            this.BtnMv.Size = new System.Drawing.Size(75, 45);
            this.BtnMv.TabIndex = 14;
            this.BtnMv.Text = "移動";
            this.BtnMv.UseVisualStyleBackColor = true;
            this.BtnMv.Click += new System.EventHandler(this.BtnMv_Click);
            // 
            // BtnJogLft
            // 
            this.BtnJogLft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnJogLft.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnJogLft.Location = new System.Drawing.Point(284, 3);
            this.BtnJogLft.Margin = new System.Windows.Forms.Padding(1);
            this.BtnJogLft.Name = "BtnJogLft";
            this.BtnJogLft.Size = new System.Drawing.Size(45, 45);
            this.BtnJogLft.TabIndex = 13;
            this.BtnJogLft.Text = "-";
            this.BtnJogLft.UseVisualStyleBackColor = true;
            this.BtnJogLft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogLft_MouseDown);
            this.BtnJogLft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogLft_MouseUp);
            // 
            // BtnSave_S
            // 
            this.BtnSave_S.Image = global::GJSControl.Properties.Resources.Save;
            this.BtnSave_S.Location = new System.Drawing.Point(575, 396);
            this.BtnSave_S.Name = "BtnSave_S";
            this.BtnSave_S.Size = new System.Drawing.Size(60, 60);
            this.BtnSave_S.TabIndex = 15;
            this.BtnSave_S.UseVisualStyleBackColor = true;
            this.BtnSave_S.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // tmrIOUpdate
            // 
            this.tmrIOUpdate.Enabled = true;
            this.tmrIOUpdate.Interval = 1000;
            this.tmrIOUpdate.Tick += new System.EventHandler(this.tmrIOUpdate_Tick);
            // 
            // BtnHomeMinus
            // 
            this.BtnHomeMinus.Location = new System.Drawing.Point(259, 167);
            this.BtnHomeMinus.Name = "BtnHomeMinus";
            this.BtnHomeMinus.Size = new System.Drawing.Size(115, 37);
            this.BtnHomeMinus.TabIndex = 17;
            this.BtnHomeMinus.Text = "Home-";
            this.BtnHomeMinus.UseVisualStyleBackColor = true;
            this.BtnHomeMinus.Click += new System.EventHandler(this.BtnHomeMinus_Click);
            // 
            // BtnHomPlus
            // 
            this.BtnHomPlus.Location = new System.Drawing.Point(380, 167);
            this.BtnHomPlus.Name = "BtnHomPlus";
            this.BtnHomPlus.Size = new System.Drawing.Size(115, 37);
            this.BtnHomPlus.TabIndex = 18;
            this.BtnHomPlus.Text = "Home+";
            this.BtnHomPlus.UseVisualStyleBackColor = true;
            this.BtnHomPlus.Click += new System.EventHandler(this.BtnHomPlus_Click);
            // 
            // TabCtl
            // 
            this.TabCtl.Controls.Add(this.SpeedPage);
            this.TabCtl.Controls.Add(this.AxisPage);
            this.TabCtl.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.TabCtl.Location = new System.Drawing.Point(0, 120);
            this.TabCtl.Multiline = true;
            this.TabCtl.Name = "TabCtl";
            this.TabCtl.SelectedIndex = 0;
            this.TabCtl.Size = new System.Drawing.Size(635, 270);
            this.TabCtl.TabIndex = 19;
            // 
            // SpeedPage
            // 
            this.SpeedPage.Controls.Add(this.buttonSOFF);
            this.SpeedPage.Controls.Add(this.buttonSON);
            this.SpeedPage.Controls.Add(this.txtBxDecRate);
            this.SpeedPage.Controls.Add(this.LblDecRateHdr);
            this.SpeedPage.Controls.Add(this.BtnHomPlus);
            this.SpeedPage.Controls.Add(this.txtBxAccRate);
            this.SpeedPage.Controls.Add(this.BtnHomeMinus);
            this.SpeedPage.Controls.Add(this.cbxSpdTyp);
            this.SpeedPage.Controls.Add(this.txtBxEndSpd);
            this.SpeedPage.Controls.Add(this.LblStartSpdHdr);
            this.SpeedPage.Controls.Add(this.txtBxMaxSpd);
            this.SpeedPage.Controls.Add(this.LblAccRateHdr);
            this.SpeedPage.Controls.Add(this.txtBxStartSpd);
            this.SpeedPage.Controls.Add(this.LblMaxSpdHdr);
            this.SpeedPage.Controls.Add(this.LblEndSpdHdr);
            this.SpeedPage.Location = new System.Drawing.Point(4, 26);
            this.SpeedPage.Name = "SpeedPage";
            this.SpeedPage.Padding = new System.Windows.Forms.Padding(3);
            this.SpeedPage.Size = new System.Drawing.Size(627, 240);
            this.SpeedPage.TabIndex = 0;
            this.SpeedPage.Text = "速度設定";
            this.SpeedPage.UseVisualStyleBackColor = true;
            // 
            // buttonSOFF
            // 
            this.buttonSOFF.Location = new System.Drawing.Point(498, 15);
            this.buttonSOFF.Name = "buttonSOFF";
            this.buttonSOFF.Size = new System.Drawing.Size(115, 37);
            this.buttonSOFF.TabIndex = 20;
            this.buttonSOFF.Text = "SOFF";
            this.buttonSOFF.UseVisualStyleBackColor = true;
            this.buttonSOFF.Click += new System.EventHandler(this.buttonSOFF_Click);
            // 
            // buttonSON
            // 
            this.buttonSON.Location = new System.Drawing.Point(377, 15);
            this.buttonSON.Name = "buttonSON";
            this.buttonSON.Size = new System.Drawing.Size(115, 37);
            this.buttonSON.TabIndex = 19;
            this.buttonSON.Text = "SON";
            this.buttonSON.UseVisualStyleBackColor = true;
            this.buttonSON.Click += new System.EventHandler(this.buttonSON_Click);
            // 
            // AxisPage
            // 
            this.AxisPage.Controls.Add(this.chkELEnable);
            this.AxisPage.Controls.Add(this.chkSELEnb);
            this.AxisPage.Controls.Add(this.txtBxHmVel);
            this.AxisPage.Controls.Add(this.txtBxSMELSet);
            this.AxisPage.Controls.Add(this.LblHmVel);
            this.AxisPage.Controls.Add(this.txtBxSPELSet);
            this.AxisPage.Controls.Add(this.cbxFBSrc);
            this.AxisPage.Controls.Add(this.txtBxPlsRto);
            this.AxisPage.Controls.Add(this.LblSMELSetHdr);
            this.AxisPage.Controls.Add(this.LblAlmLogicHdr);
            this.AxisPage.Controls.Add(this.cbxHmMode);
            this.AxisPage.Controls.Add(this.LblSPELSetHdr);
            this.AxisPage.Controls.Add(this.LblFBSrcHdr);
            this.AxisPage.Controls.Add(this.cbxELMode);
            this.AxisPage.Controls.Add(this.LblPlsRtoHdr);
            this.AxisPage.Controls.Add(this.LblHmMode);
            this.AxisPage.Controls.Add(this.txtBxEncoderRes);
            this.AxisPage.Controls.Add(this.LblELLogicHdr);
            this.AxisPage.Controls.Add(this.txtBxEGear);
            this.AxisPage.Controls.Add(this.LblOrgLogicHdr);
            this.AxisPage.Controls.Add(this.LblELModHdr);
            this.AxisPage.Controls.Add(this.LblServoLogicHdr);
            this.AxisPage.Controls.Add(this.LblEncoderResHdr);
            this.AxisPage.Controls.Add(this.cbxAlmLogic);
            this.AxisPage.Controls.Add(this.cbxELLogic);
            this.AxisPage.Controls.Add(this.LblEGearHdr);
            this.AxisPage.Controls.Add(this.cbxOrgLogic);
            this.AxisPage.Controls.Add(this.cbxServoLogic);
            this.AxisPage.Controls.Add(this.cbxPlsOptMode);
            this.AxisPage.Controls.Add(this.LblFBReverseHdr);
            this.AxisPage.Controls.Add(this.cbxPlsIptMode);
            this.AxisPage.Controls.Add(this.cbxFBReverse);
            this.AxisPage.Controls.Add(this.LblPlsIptModHdr);
            this.AxisPage.Controls.Add(this.LblPlsOptModHdr);
            this.AxisPage.Location = new System.Drawing.Point(4, 26);
            this.AxisPage.Name = "AxisPage";
            this.AxisPage.Padding = new System.Windows.Forms.Padding(3);
            this.AxisPage.Size = new System.Drawing.Size(627, 240);
            this.AxisPage.TabIndex = 1;
            this.AxisPage.Text = "軸設定";
            this.AxisPage.UseVisualStyleBackColor = true;
            // 
            // chkELEnable
            // 
            this.chkELEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkELEnable.Location = new System.Drawing.Point(492, 157);
            this.chkELEnable.Name = "chkELEnable";
            this.chkELEnable.Size = new System.Drawing.Size(127, 20);
            this.chkELEnable.TabIndex = 40;
            this.chkELEnable.Text = "啟動硬體極限";
            this.chkELEnable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkELEnable.UseVisualStyleBackColor = true;
            this.chkELEnable.CheckedChanged += new System.EventHandler(this.chkELEnable_CheckedChanged);
            // 
            // LblMotionTitle
            // 
            this.LblMotionTitle.AutoSize = true;
            this.LblMotionTitle.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold);
            this.LblMotionTitle.Location = new System.Drawing.Point(1, 1);
            this.LblMotionTitle.Name = "LblMotionTitle";
            this.LblMotionTitle.Size = new System.Drawing.Size(49, 19);
            this.LblMotionTitle.TabIndex = 20;
            this.LblMotionTitle.Text = "馬達";
            // 
            // TLP_Pos_1
            // 
            this.TLP_Pos_1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TLP_Pos_1.ColumnCount = 2;
            this.TLP_Pos_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Pos_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TLP_Pos_1.Controls.Add(this.lblPos, 1, 0);
            this.TLP_Pos_1.Controls.Add(this.LblPosHdr, 0, 0);
            this.TLP_Pos_1.Location = new System.Drawing.Point(2, 2);
            this.TLP_Pos_1.Margin = new System.Windows.Forms.Padding(1);
            this.TLP_Pos_1.Name = "TLP_Pos_1";
            this.TLP_Pos_1.RowCount = 1;
            this.TLP_Pos_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_Pos_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.TLP_Pos_1.Size = new System.Drawing.Size(292, 26);
            this.TLP_Pos_1.TabIndex = 21;
            // 
            // TLP_Pos
            // 
            this.TLP_Pos.BackColor = System.Drawing.SystemColors.ControlLight;
            this.TLP_Pos.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.TLP_Pos.ColumnCount = 1;
            this.TLP_Pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_Pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP_Pos.Controls.Add(this.TLP_Pos_1, 0, 0);
            this.TLP_Pos.Location = new System.Drawing.Point(239, 90);
            this.TLP_Pos.Name = "TLP_Pos";
            this.TLP_Pos.RowCount = 1;
            this.TLP_Pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_Pos.Size = new System.Drawing.Size(296, 30);
            this.TLP_Pos.TabIndex = 22;
            // 
            // TLP_pnlMove
            // 
            this.TLP_pnlMove.BackColor = System.Drawing.SystemColors.Control;
            this.TLP_pnlMove.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.TLP_pnlMove.ColumnCount = 1;
            this.TLP_pnlMove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_pnlMove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP_pnlMove.Controls.Add(this.TLP_pnlMove_1, 0, 0);
            this.TLP_pnlMove.Location = new System.Drawing.Point(3, 396);
            this.TLP_pnlMove.Name = "TLP_pnlMove";
            this.TLP_pnlMove.RowCount = 1;
            this.TLP_pnlMove.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_pnlMove.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP_pnlMove.Size = new System.Drawing.Size(400, 60);
            this.TLP_pnlMove.TabIndex = 23;
            // 
            // TLP_pnlMove_1
            // 
            this.TLP_pnlMove_1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TLP_pnlMove_1.ColumnCount = 5;
            this.TLP_pnlMove_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_pnlMove_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.TLP_pnlMove_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.TLP_pnlMove_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.TLP_pnlMove_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.TLP_pnlMove_1.Controls.Add(this.BtnJogRht, 4, 0);
            this.TLP_pnlMove_1.Controls.Add(this.txtBxMvPos, 0, 0);
            this.TLP_pnlMove_1.Controls.Add(this.BtnMv, 1, 0);
            this.TLP_pnlMove_1.Controls.Add(this.BtnJogLft, 3, 0);
            this.TLP_pnlMove_1.Controls.Add(this.BtnStop, 2, 0);
            this.TLP_pnlMove_1.Location = new System.Drawing.Point(4, 4);
            this.TLP_pnlMove_1.Margin = new System.Windows.Forms.Padding(1);
            this.TLP_pnlMove_1.Name = "TLP_pnlMove_1";
            this.TLP_pnlMove_1.RowCount = 1;
            this.TLP_pnlMove_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_pnlMove_1.Size = new System.Drawing.Size(392, 52);
            this.TLP_pnlMove_1.TabIndex = 24;
            // 
            // frmMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.TLP_pnlMove);
            this.Controls.Add(this.TLP_Pos);
            this.Controls.Add(this.LblMotionTitle);
            this.Controls.Add(this.TabCtl);
            this.Controls.Add(this.BtnSave_S);
            this.Controls.Add(this.GBoxStatus);
            this.Controls.Add(this.cbxAxisNo);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMotion";
            this.Text = "Motion";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMotion_FormClosing);
            this.Load += new System.EventHandler(this.frmMotion_Load);
            this.VisibleChanged += new System.EventHandler(this.frmMotion_VisibleChanged);
            this.GBoxStatus.ResumeLayout(false);
            this.TLP_Status.ResumeLayout(false);
            this.TabCtl.ResumeLayout(false);
            this.SpeedPage.ResumeLayout(false);
            this.SpeedPage.PerformLayout();
            this.AxisPage.ResumeLayout(false);
            this.AxisPage.PerformLayout();
            this.TLP_Pos_1.ResumeLayout(false);
            this.TLP_Pos.ResumeLayout(false);
            this.TLP_pnlMove.ResumeLayout(false);
            this.TLP_pnlMove_1.ResumeLayout(false);
            this.TLP_pnlMove_1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxAxisNo;
        private System.Windows.Forms.Label lblPos;
        private System.Windows.Forms.Label LblPosHdr;
        private System.Windows.Forms.ComboBox cbxSpdTyp;
        private System.Windows.Forms.Label LblDecRateHdr;
        private System.Windows.Forms.TextBox txtBxDecRate;
        private System.Windows.Forms.Label LblAccRateHdr;
        private System.Windows.Forms.TextBox txtBxAccRate;
        private System.Windows.Forms.Label LblEndSpdHdr;
        private System.Windows.Forms.TextBox txtBxEndSpd;
        private System.Windows.Forms.Label LblMaxSpdHdr;
        private System.Windows.Forms.TextBox txtBxMaxSpd;
        private System.Windows.Forms.Label LblStartSpdHdr;
        private System.Windows.Forms.TextBox txtBxStartSpd;
        private System.Windows.Forms.Label LblSMELSetHdr;
        private System.Windows.Forms.TextBox txtBxSMELSet;
        private System.Windows.Forms.Label LblSPELSetHdr;
        private System.Windows.Forms.TextBox txtBxSPELSet;
        private System.Windows.Forms.Label LblPlsRtoHdr;
        private System.Windows.Forms.TextBox txtBxPlsRto;
        private System.Windows.Forms.GroupBox GBoxStatus;
        private System.Windows.Forms.Label LblInpHdr;
        private System.Windows.Forms.Label LblMELHdr;
        private System.Windows.Forms.Label LblPELHdr;
        private System.Windows.Forms.Label LblOrgHdr;
        private System.Windows.Forms.Label LblStopHdr;
        private System.Windows.Forms.Label LblAlmHdr;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.TextBox txtBxMvPos;
        private System.Windows.Forms.Button BtnJogRht;
        private System.Windows.Forms.Button BtnMv;
        private System.Windows.Forms.Button BtnJogLft;
        private System.Windows.Forms.Button BtnSave_S;
        private System.Windows.Forms.Label LblEmgHdr;
     
        private System.Windows.Forms.CheckBox chkSELEnb;
        private System.Windows.Forms.Timer tmrIOUpdate;
        private System.Windows.Forms.ComboBox cbxELLogic;
        private System.Windows.Forms.Label LblELLogicHdr;
        private System.Windows.Forms.ComboBox cbxAlmLogic;
        private System.Windows.Forms.Label LblAlmLogicHdr;
        private System.Windows.Forms.ComboBox cbxServoLogic;
        private System.Windows.Forms.Label LblServoLogicHdr;
        private System.Windows.Forms.ComboBox cbxOrgLogic;
        private System.Windows.Forms.Label LblOrgLogicHdr;
        private System.Windows.Forms.ComboBox cbxPlsOptMode;
        private System.Windows.Forms.Label LblPlsOptModHdr;
        private System.Windows.Forms.ComboBox cbxPlsIptMode;
        private System.Windows.Forms.Label LblPlsIptModHdr;
        private System.Windows.Forms.ComboBox cbxFBReverse;
        private System.Windows.Forms.Label LblFBReverseHdr;
        private System.Windows.Forms.Label LblEGearHdr;
        private System.Windows.Forms.TextBox txtBxEGear;
        private System.Windows.Forms.Label LblEncoderResHdr;
        private System.Windows.Forms.TextBox txtBxEncoderRes;
        private System.Windows.Forms.ComboBox cbxELMode;
        private System.Windows.Forms.Label LblELModHdr;
        private System.Windows.Forms.Label LblHmVel;
        private System.Windows.Forms.TextBox txtBxHmVel;
        private System.Windows.Forms.ComboBox cbxHmMode;
        private System.Windows.Forms.Label LblHmMode;
        private System.Windows.Forms.ComboBox cbxFBSrc;
        private System.Windows.Forms.Label LblFBSrcHdr;
        private System.Windows.Forms.Button BtnHomeMinus;
        private System.Windows.Forms.Button BtnHomPlus;
        private System.Windows.Forms.Label labelInp;
        private System.Windows.Forms.Label labelMEL;
        private System.Windows.Forms.Label labelPEL;
        private System.Windows.Forms.Label labelOrg;
        private System.Windows.Forms.Label labelStop;
        private System.Windows.Forms.Label labelEmg;
        private System.Windows.Forms.Label labelAlm;
        private System.Windows.Forms.TabControl TabCtl;
        private System.Windows.Forms.TabPage SpeedPage;
        private System.Windows.Forms.TabPage AxisPage;
        private System.Windows.Forms.Label LblMotionTitle;
        private System.Windows.Forms.Button buttonSOFF;
        private System.Windows.Forms.Button buttonSON;
        private System.Windows.Forms.Label labelSMEL;
        private System.Windows.Forms.Label labelSPEL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkELEnable;
        private System.Windows.Forms.TableLayoutPanel TLP_Status;
        private System.Windows.Forms.TableLayoutPanel TLP_Pos_1;
        private System.Windows.Forms.TableLayoutPanel TLP_Pos;
        private System.Windows.Forms.TableLayoutPanel TLP_pnlMove;
        private System.Windows.Forms.TableLayoutPanel TLP_pnlMove_1;
    }
}
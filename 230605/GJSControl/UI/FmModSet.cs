using System;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CommonLibrary;
using FileStreamLibrary;
using GJSControl.UI;

namespace nsUI
{
    public partial class FmModSet : Form
    {
        public FmModSet()
        {
            InitializeComponent();
        }

        private void frmModSet_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);
            TabPageSetColor(TabCtl);

            #region 更新氣缸名稱清單
            CbxCylinderName.Items.Clear();
            for (int i = 0; i < (int)ECylName.Count; i++)
                CbxCylinderName.Items.Add(((ECylName)i).ToString());
            #endregion
            #region 更新氣缸DIO清單
            #region DI
            CbxCylinderDI_Retract.Items.Clear();
            CbxCylinderDI_Extension.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxCylinderDI_Retract.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxCylinderDI_Extension.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxCylinderDI_Retract.Items.Add("None");
            CbxCylinderDI_Extension.Items.Add("None");
            #endregion
            #region DO
            CbxCylinderDO_Retract.Items.Clear();
            CbxCylinderDO_Extension.Items.Clear();
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                CbxCylinderDO_Retract.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxCylinderDO_Extension.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
            }
            CbxCylinderDO_Retract.Items.Add("None");
            CbxCylinderDO_Extension.Items.Add("None");
            #endregion
            #endregion

            #region 更新吸盤名稱清單
            CbxVaccumSuckerName.Items.Clear();
            for (int i = 0; i < (int)EVacSuckerName.Count; i++)
                CbxVaccumSuckerName.Items.Add(((EVacSuckerName)i).ToString());
            #endregion
            #region 更新吸盤DIO清單
            #region DI
            CbxVacDI1.Items.Clear();
            CbxVacDI2.Items.Clear();
            CbxVacBtn.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxVacDI1.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxVacDI2.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxVacBtn.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxVacDI1.Items.Add("None");
            CbxVacDI2.Items.Add("None");
            CbxVacBtn.Items.Add("None");
            #endregion
            #region DO
            CbxShock1.Items.Clear();
            CbxShock2.Items.Clear();
            CbxVacOn.Items.Clear();
            CbxVacOff.Items.Clear();
            CbxVacBreak.Items.Clear();
            CbxVacBtnLED.Items.Clear();
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                CbxShock1.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxShock2.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxVacOn.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxVacOff.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxVacBreak.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxVacBtnLED.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
            }
            CbxShock1.Items.Add("None");
            CbxShock2.Items.Add("None");
            CbxVacOn.Items.Add("None");
            CbxVacOff.Items.Add("None");
            CbxVacBreak.Items.Add("None");
            CbxVacBtnLED.Items.Add("None");
            #endregion
            #endregion
            #region 更新吸盤真空Off時間
            NumUDVacOffDelayTimeValue.Value = 0;
            #endregion
            #region 更新吸盤振動缸預設值
            NumUDShockIntervalsValue.Value = 0;
            NumUDShockTimesValue.Value = 0;
            #endregion

            #region 更新門鎖名稱清單
            CbxDoorName.Items.Clear();
            for (int i = 0; i < (int)EDoorPos.Count; i++)
                CbxDoorName.Items.Add(((EDoorPos)i).ToString());
            #endregion
            #region 更新門鎖DIO清單
            #region DI
            CbxDoorDI.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxDoorDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxDoorDI.Items.Add("None");

            CbxDoorBtnDI.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxDoorBtnDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxDoorBtnDI.Items.Add("None");
            #endregion
            #region DO
            CbxDoorDO.Items.Clear();
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                CbxDoorDO.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
            }
            CbxDoorDO.Items.Add("None");

            CbxDoorBtnLEDDO.Items.Clear();
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                CbxDoorBtnLEDDO.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
            }
            CbxDoorBtnLEDDO.Items.Add("None");
            #endregion
            #region Dir
            CbxDoorDir.Items.Clear();
            for (int i = 0; i < (int)EDoorDir.Count; i++)
            {
                CbxDoorDir.Items.Add(((EDoorDir)i).ToString());
            }
            CbxDoorDir.Items.Add("None");
            #endregion
            #endregion
            #region 更新門鎖上鎖時間
            NymUDDoorAutoLockTimeValue.Value = 0;
            #endregion
            #region 更新門僅自動模式開啟警報狀態
            ChB_OnlyAutoAlarm.Checked = false;
            #endregion
            #region 更新共用門鎖
            ChB_ShareDO.Checked = false;
            #endregion

            #region 更新台車名稱清單
            CbxTrolleyName.Items.Clear();
            for (int i = 0; i < (int)ETrolley.Count; i++)
                CbxTrolleyName.Items.Add(((ETrolley)i).ToString());
            #endregion
            #region 更新台車感測器與氣缸清單
            #region 感測器
            CbxTrolleyDI.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxTrolleyDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxTrolleyDI.Items.Add("None");
            #endregion
            #region 氣缸A&B
            CbxTrolleyCylA.Items.Clear();
            CbxTrolleyCylB.Items.Clear();
            for (int i = 0; i < (int)ECylName.Count; i++)
            {
                CbxTrolleyCylA.Items.Add(((ECylName)i).ToString());
                CbxTrolleyCylB.Items.Add(((ECylName)i).ToString());
            }
            CbxTrolleyCylA.Items.Add("None");
            CbxTrolleyCylB.Items.Add("None");
            #endregion
            #endregion
            #region 更新台車上鎖時間
            NymUDTrolleyAutoLockTimeValue.Value = 0;
            #endregion

            #region 更新升降台名稱清單
            CbxLiftsName.Items.Clear();
            for (int i = 0; i < (int)ELifts.Count; i++)
                CbxLiftsName.Items.Add(((ELifts)i).ToString());
            #endregion
            #region 更新升降台感測器清單
            #region DI
            CbxLiftsMotorErrDI.Items.Clear();
            CbxLiftsULimBoardDI.Items.Clear();
            CbxLiftsInpPlaceBoardDI.Items.Clear();
            CbxLiftsHavePalletDI.Items.Clear();
            CbxLiftsOnPalletDI.Items.Clear();
            CbxLiftsWorkULimDI.Items.Clear();
            CbxLiftsWorkLLimDI.Items.Clear();
            CbxLiftsMoveULimDI.Items.Clear();
            CbxLiftsMoveLLimDI.Items.Clear();
            CbxLiftsSafeLockDI_1.Items.Clear();
            CbxLiftsSafeLockDI_2.Items.Clear();
            CbxLiftsBtnUp.Items.Clear();
            CbxLiftsBtnDown.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxLiftsMotorErrDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsULimBoardDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsInpPlaceBoardDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsHavePalletDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsOnPalletDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsWorkULimDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsWorkLLimDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsMoveULimDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsMoveLLimDI.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsSafeLockDI_1.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsSafeLockDI_2.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsBtnUp.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
                CbxLiftsBtnDown.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxLiftsMotorErrDI.Items.Add("None");
            CbxLiftsULimBoardDI.Items.Add("None");
            CbxLiftsInpPlaceBoardDI.Items.Add("None");
            CbxLiftsHavePalletDI.Items.Add("None");
            CbxLiftsOnPalletDI.Items.Add("None");
            CbxLiftsWorkULimDI.Items.Add("None");
            CbxLiftsWorkLLimDI.Items.Add("None");
            CbxLiftsMoveULimDI.Items.Add("None");
            CbxLiftsMoveLLimDI.Items.Add("None");
            CbxLiftsSafeLockDI_1.Items.Add("None");
            CbxLiftsSafeLockDI_2.Items.Add("None");
            CbxLiftsBtnUp.Items.Add("None");
            CbxLiftsBtnDown.Items.Add("None");
            #endregion
            #region DO
            CbxLiftsUp.Items.Clear();
            CbxLiftsDown.Items.Clear();
            CbxLiftsBtnLEDUp.Items.Clear();
            CbxLiftsBtnLEDDown.Items.Clear();
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                CbxLiftsUp.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxLiftsDown.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxLiftsBtnLEDUp.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
                CbxLiftsBtnLEDDown.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
            }
            CbxLiftsUp.Items.Add("None");
            CbxLiftsDown.Items.Add("None");
            CbxLiftsBtnLEDUp.Items.Add("None");
            CbxLiftsBtnLEDDown.Items.Add("None");
            #endregion
            #endregion

            #region 更新震動送料機清單
            CbxVibBowName.Items.Clear();
            for (int i = 0; i < (int)EVibBow.Count; i++)
                CbxVibBowName.Items.Add(((EVibBow)i).ToString());
            #endregion
            #region 更新震動送料機感測器與開關清單
            #region 感測器
            CbxVibBowEmptySensor.Items.Clear();
            for (int i = 0; i < (int)EDI_TYPE.DI_COUNT; i++)
            {
                CbxVibBowEmptySensor.Items.Add(G.Comm.IOCtrl.GetDIName((EDI_TYPE)i));
            }
            CbxVibBowEmptySensor.Items.Add("None");
            #endregion
            #region 開關
            CbxVibBowPower.Items.Clear();
            for (int i = 0; i < (int)EDO_TYPE.DO_COUNT; i++)
            {
                CbxVibBowPower.Items.Add(G.Comm.IOCtrl.GetDOName((EDO_TYPE)i));
            }
            CbxVibBowPower.Items.Add("None");
            #endregion
            #endregion
            #region 更新震動送料機所有時間
            NumUDVibBowStartDelayValue.Value = 0;
            NumUDVibBowStopDelayValue.Value = 0;
            NumUDVibBowAlarmTimeValue.Value = 0;
            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                #region 氣缸
                bool _CylValueSave = true;

                if (G.Comm.CYL != null)
                {
                    if (CbxCylinderName.SelectedIndex >= 0)
                    {
                        _CylValueSave =
                            CbxCylinderDI_Extension.SelectedIndex == (int)G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension) &&
                            CbxCylinderDI_Retract.SelectedIndex == (int)G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract) &&
                            CbxCylinderDO_Extension.SelectedIndex == (int)G.Comm.CYL.GetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension) &&
                            CbxCylinderDO_Retract.SelectedIndex == (int)G.Comm.CYL.GetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract);
                    }
                    else
                        _CylValueSave = false;

                    if (CbxCylinderName.SelectedIndex >= 0 && _CylValueSave)
                    {
                        #region 變更a0、a1顏色
                        if (G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension) != EDI_TYPE.DI_COUNT)
                        {
                            if (G.Comm.IOCtrl.GetDI(G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension), true))
                                Lbl_a1.BackColor = Color.LimeGreen;
                            else
                                Lbl_a1.BackColor = Color.Transparent;
                        }
                        else
                            Lbl_a1.BackColor = Color.Transparent;

                        if (G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract) != EDI_TYPE.DI_COUNT)
                        {
                            if (G.Comm.IOCtrl.GetDI(G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract), true))
                                Lbl_a0.BackColor = Color.LimeGreen;
                            else
                                Lbl_a0.BackColor = Color.Transparent;
                        }
                        else
                            Lbl_a0.BackColor = Color.Transparent;
                        #endregion
                    }
                    else
                    {
                        Lbl_a0.BackColor = Color.Transparent;
                        Lbl_a1.BackColor = Color.Transparent;
                    }
                }

                if (BtnRetract.Enabled != _CylValueSave)
                    BtnRetract.Enabled = _CylValueSave;
                if (BtnExtension.Enabled != _CylValueSave)
                    BtnExtension.Enabled = _CylValueSave;
                #endregion

                #region 吸盤
                bool _VacSuckerValueSave = true;

                if (G.Comm.Vac != null)
                {
                    if (CbxVaccumSuckerName.SelectedIndex >= 0)
                    {
                        _VacSuckerValueSave =
                            CbxVacBtn.SelectedIndex == (int)G.Comm.Vac.GetVacBtn((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex) &&
                            CbxVacBtnLED.SelectedIndex == (int)G.Comm.Vac.GetVacBtnLED((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex) &&
                            CbxVacDI1.SelectedIndex == (int)G.Comm.Vac.GetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor1) &&
                            CbxVacDI2.SelectedIndex == (int)G.Comm.Vac.GetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor2) &&
                            CbxVacOn.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOn) &&
                            CbxVacOff.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOff) &&
                            CbxVacBreak.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumBreak) &&
                            CbxShock1.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock1) &&
                            CbxShock2.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock2);
                    }
                    else
                        _VacSuckerValueSave = false;
                }

                if (BtnVacOn.Enabled != _VacSuckerValueSave)
                    BtnVacOn.Enabled = _VacSuckerValueSave;
                if (BtnVacOff.Enabled != _VacSuckerValueSave)
                    BtnVacOff.Enabled = _VacSuckerValueSave;
                if (BtnVacBreak.Enabled != _VacSuckerValueSave)
                    BtnVacBreak.Enabled = _VacSuckerValueSave;
                if (BtnShock.Enabled != _VacSuckerValueSave)
                    BtnShock.Enabled = _VacSuckerValueSave;
                #endregion

                #region 門鎖
                bool _DoorSave = true;

                if (G.Comm.Door != null)
                {
                    if (CbxDoorName.SelectedIndex >= 0)
                    {
                        _DoorSave =
                            CbxDoorDI.SelectedIndex == (int)G.Comm.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex) &&
                            CbxDoorDO.SelectedIndex == (int)G.Comm.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex);
                    }
                    else
                        _DoorSave = false;

                    if (CbxDoorName.SelectedIndex >= 0 && _DoorSave)
                    {
                        #region 變更門鎖圖案
                        if (G.Comm.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex) != EDI_TYPE.DI_COUNT)
                        {
                            switch (G.Comm.IOCtrl.GetDIEdge(G.Comm.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex), true))
                            {
                                case EDIO_SingleEdge.RisingEdge:
                                    DoorSubPanel.BackgroundImage = GJSControl.Properties.Resources.DoorLock_2;
                                    break;
                                case EDIO_SingleEdge.FallingEdge:
                                    DoorSubPanel.BackgroundImage = GJSControl.Properties.Resources.DoorLock_1;
                                    break;
                                case EDIO_SingleEdge.On:
                                    break;
                                case EDIO_SingleEdge.Off:
                                    break;
                                case EDIO_SingleEdge.Nothing:
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion

                        #region 門鎖LED
                        if (G.Comm.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex) != EDO_TYPE.DO_COUNT)
                        {
                            if (G.Comm.IOCtrl.GetDO(G.Comm.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex), false))
                                LblDoorLockLED.BackColor = Color.LimeGreen;
                            else
                                LblDoorLockLED.BackColor = Color.Transparent;
                        }
                        else
                            LblDoorLockLED.BackColor = Color.Transparent;
                        #endregion
                    }
                    else
                        LblDoorLockLED.BackColor = Color.Transparent;
                }

                if (BtnDoorUnLock.Enabled != _DoorSave)
                    BtnDoorUnLock.Enabled = _DoorSave;
                if (BtnDoorLock.Enabled != _DoorSave)
                    BtnDoorLock.Enabled = _DoorSave;
                #endregion

                #region 台車
                bool _TrolleySave = true;

                if (G.Comm.Trolley != null)
                {
                    if (CbxTrolleyName.SelectedIndex >= 0)
                    {
                        _TrolleySave =
                            CbxTrolleyDI.SelectedIndex == (int)G.Comm.Trolley.GetTrolleySensor((ETrolley)CbxTrolleyName.SelectedIndex) &&
                            CbxTrolleyCylA.SelectedIndex == (int)G.Comm.Trolley.GetTrolleyCylA((ETrolley)CbxTrolleyName.SelectedIndex) &&
                            CbxTrolleyCylB.SelectedIndex == (int)G.Comm.Trolley.GetTrolleyCylB((ETrolley)CbxTrolleyName.SelectedIndex);
                    }
                    else
                        _TrolleySave = false;
                }

                if (BtnTrolleyUnLock.Enabled != _TrolleySave)
                    BtnTrolleyUnLock.Enabled = _TrolleySave;
                if (BtnTrolleyLock.Enabled != _TrolleySave)
                    BtnTrolleyLock.Enabled = _TrolleySave;
                #endregion

                #region 升降台
                bool _LiftsSave = true;

                if (G.Comm.Lifts != null)
                {
                    if (CbxLiftsName.SelectedIndex >= 0)
                    {
                        _LiftsSave =
                            CbxLiftsMotorErrDI.SelectedIndex == (int)G.Comm.Lifts.GetMotorError((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsULimBoardDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsInpPlaceBoardDI.SelectedIndex == (int)G.Comm.Lifts.GetInPlace((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsHavePalletDI.SelectedIndex == (int)G.Comm.Lifts.GetPallet((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsOnPalletDI.SelectedIndex == (int)G.Comm.Lifts.GetOnPallet((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsWorkULimDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Work((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsWorkLLimDI.SelectedIndex == (int)G.Comm.Lifts.GetLLim_Work((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsMoveULimDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Move((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsMoveLLimDI.SelectedIndex == (int)G.Comm.Lifts.GetLLim_Move((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsSafeLockDI_1.SelectedIndex == (int)G.Comm.Lifts.GetSafeLock1((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsSafeLockDI_2.SelectedIndex == (int)G.Comm.Lifts.GetSafeLock2((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsUp.SelectedIndex == (int)G.Comm.Lifts.GetUp((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsDown.SelectedIndex == (int)G.Comm.Lifts.GetDown((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsBtnUp.SelectedIndex == (int)G.Comm.Lifts.GetBtnUp((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsBtnLEDUp.SelectedIndex == (int)G.Comm.Lifts.GetBtnUpLED((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsBtnDown.SelectedIndex == (int)G.Comm.Lifts.GetBtnDown((ELifts)CbxLiftsName.SelectedIndex) &&
                            CbxLiftsBtnLEDDown.SelectedIndex == (int)G.Comm.Lifts.GetBtnDownLED((ELifts)CbxLiftsName.SelectedIndex);
                    }
                    else
                        _LiftsSave = false;

                    /*
                    if (CbxTrolleyName.SelectedIndex >= 0 && _TrolleySave)
                    {
                        #region 變更門鎖圖案
                        if (G.Common.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex) != EDI_TYPE.DI_COUNT)
                        {
                            switch (G.Common.IOCtrl.GetDIEdge(G.Common.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex), true))
                            {
                                case EDIO_SingleEdge.RisingEdge:
                                    DoorSubPanel.BackgroundImage = GJSControl.Properties.Resources.DoorLock_2;
                                    break;
                                case EDIO_SingleEdge.FallingEdge:
                                    DoorSubPanel.BackgroundImage = GJSControl.Properties.Resources.DoorLock_1;
                                    break;
                                case EDIO_SingleEdge.On:
                                    break;
                                case EDIO_SingleEdge.Off:
                                    break;
                                case EDIO_SingleEdge.Nothing:
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion

                        #region 門鎖LED
                        if (G.Common.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex) != EDO_TYPE.DO_COUNT)
                        {
                            if (G.Common.IOCtrl.GetDO(G.Common.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex), false))
                                LblDoorLockLED.BackColor = Color.LimeGreen;
                            else
                                LblDoorLockLED.BackColor = Color.Transparent;
                        }
                        else
                            LblDoorLockLED.BackColor = Color.Transparent;
                        #endregion
                    }
                    else
                        LblDoorLockLED.BackColor = Color.Transparent;
                    */
                }

                if (BtnLiftsDown.Enabled != _LiftsSave)
                    BtnLiftsDown.Enabled = _LiftsSave;
                if (BtnLiftsUp.Enabled != _LiftsSave)
                    BtnLiftsUp.Enabled = _LiftsSave;
                #endregion

                #region 震動送料機
                bool _VibBowSave = true;

                if (G.Comm.VibBow != null)
                {
                    if (CbxVibBowName.SelectedIndex >= 0)
                    {
                        _VibBowSave = CbxVibBowEmptySensor.SelectedIndex == (int)G.Comm.VibBow.GetEmptySensor((EVibBow)CbxVibBowName.SelectedIndex);
                        _VibBowSave = CbxVibBowPower.SelectedIndex == (int)G.Comm.VibBow.GetPower((EVibBow)CbxVibBowName.SelectedIndex);
                    }
                    else
                        _VibBowSave = false;
                }

                if (BtnVibBowPowerOn.Enabled != _VibBowSave)
                    BtnVibBowPowerOn.Enabled = _VibBowSave;
                if (BtnVibBowPowerOff.Enabled != _VibBowSave)
                    BtnVibBowPowerOff.Enabled = _VibBowSave;
                #endregion
            }
            catch { }
        }

        private void CbxCylinderName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex < 0)
            {
                CbxCylinderDI_Extension.SelectedIndex = -1;
                CbxCylinderDI_Retract.SelectedIndex = -1;
                CbxCylinderDO_Extension.SelectedIndex = -1;
                CbxCylinderDO_Retract.SelectedIndex = -1;
                LblExtexsionDIName.Text = "None";
                LblRetractDIName.Text = "None";
                LblExtensionDOName.Text = "None";
                LblRetractDOName.Text = "None";
                return;
            }

            CbxCylinderDI_Extension.SelectedIndex = (int)G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension);
            CbxCylinderDI_Retract.SelectedIndex = (int)G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract);
            CbxCylinderDO_Extension.SelectedIndex = (int)G.Comm.CYL.GetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension);
            CbxCylinderDO_Retract.SelectedIndex = (int)G.Comm.CYL.GetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract);

            if (CbxCylinderDI_Extension.SelectedIndex >= 0 && (EDI_TYPE)CbxCylinderDI_Extension.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblExtexsionDIName.Text = ((EDI_TYPE)CbxCylinderDI_Extension.SelectedIndex).ToString();
            else
                LblExtexsionDIName.Text = "None";
            if (CbxCylinderDI_Retract.SelectedIndex >= 0 && (EDI_TYPE)CbxCylinderDI_Retract.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblRetractDIName.Text = ((EDI_TYPE)CbxCylinderDI_Retract.SelectedIndex).ToString();
            else
                LblRetractDIName.Text = "None";
            if (CbxCylinderDO_Extension.SelectedIndex >= 0 && (EDO_TYPE)CbxCylinderDO_Extension.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblExtensionDOName.Text = ((EDO_TYPE)CbxCylinderDO_Extension.SelectedIndex).ToString();
            else
                LblExtensionDOName.Text = "None";
            if (CbxCylinderDO_Retract.SelectedIndex >= 0 && (EDO_TYPE)CbxCylinderDO_Retract.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblRetractDOName.Text = ((EDO_TYPE)CbxCylinderDO_Retract.SelectedIndex).ToString();
            else
                LblRetractDOName.Text = "None";

            CbxCylinderDI_Extension.BackColor = SystemColors.Window;
            CbxCylinderDI_Retract.BackColor = SystemColors.Window;
            CbxCylinderDO_Extension.BackColor = SystemColors.Window;
            CbxCylinderDO_Retract.BackColor = SystemColors.Window;
        }

        private void CbxVaccumSuckerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex < 0)
            {
                CbxVacBtn.SelectedIndex = -1;
                CbxVacBtnLED.SelectedIndex = -1;
                CbxVacDI1.SelectedIndex = -1;
                CbxVacDI2.SelectedIndex = -1;
                CbxVacOn.SelectedIndex = -1;
                CbxVacOff.SelectedIndex = -1;
                CbxVacBreak.SelectedIndex = -1;
                CbxShock1.SelectedIndex = -1;
                CbxShock2.SelectedIndex = -1;
                NumUDVacOffDelayTimeValue.Value = 0;
                NumUDShockIntervalsValue.Value = 0;
                NumUDShockTimesValue.Value = 0;
                LblVacBtnName.Text = "None";
                LblVacBtnLEDName.Text = "None";
                LblVacName1.Text = "None";
                LblVacName2.Text = "None";
                LblVacOnName.Text = "None";
                LblVacOffName.Text = "None";
                LblVacBreakName.Text = "None";
                LblShockName1.Text = "None";
                LblShockName2.Text = "None";
                return;
            }

            CbxVacBtn.SelectedIndex = (int)G.Comm.Vac.GetVacBtn((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
            CbxVacBtnLED.SelectedIndex = (int)G.Comm.Vac.GetVacBtnLED((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
            CbxVacDI1.SelectedIndex = (int)G.Comm.Vac.GetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor1);
            CbxVacDI2.SelectedIndex = (int)G.Comm.Vac.GetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor2);
            CbxVacOn.SelectedIndex = (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOn);
            CbxVacOff.SelectedIndex = (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOff);
            CbxVacBreak.SelectedIndex = (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumBreak);
            CbxShock1.SelectedIndex = (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock1);
            CbxShock2.SelectedIndex = (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock2);
            NumUDVacOffDelayTimeValue.Value = (decimal)G.Comm.Vac.GetVacOffDelay((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
            NumUDShockIntervalsValue.Value = (decimal)((double)G.Comm.Vac.GetVacShockIntervals((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex) / 1000);
            NumUDShockTimesValue.Value = (decimal)G.Comm.Vac.GetVacShockTimes((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);

            if (CbxVacBtn.SelectedIndex >= 0 && (EDI_TYPE)CbxVacBtn.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblVacBtnName.Text = ((EDI_TYPE)CbxVacBtn.SelectedIndex).ToString();
            else
                LblVacBtnName.Text = "None";
            if (CbxVacBtnLED.SelectedIndex >= 0 && (EDO_TYPE)CbxVacBtnLED.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblVacBtnLEDName.Text = ((EDO_TYPE)CbxVacBtnLED.SelectedIndex).ToString();
            else
                LblVacBtnLEDName.Text = "None";

            if (CbxVacDI1.SelectedIndex >= 0 && (EDI_TYPE)CbxVacDI1.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblVacName1.Text = ((EDI_TYPE)CbxVacDI1.SelectedIndex).ToString();
            else
                LblVacName1.Text = "None";
            if (CbxVacDI2.SelectedIndex >= 0 && (EDI_TYPE)CbxVacDI2.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblVacName2.Text = ((EDI_TYPE)CbxVacDI2.SelectedIndex).ToString();
            else
                LblVacName2.Text = "None";

            if (CbxVacOn.SelectedIndex >= 0 && (EDO_TYPE)CbxVacOn.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblVacOnName.Text = ((EDO_TYPE)CbxVacOn.SelectedIndex).ToString();
            else
                LblVacOnName.Text = "None";
            if (CbxVacOff.SelectedIndex >= 0 && (EDO_TYPE)CbxVacOff.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblVacOffName.Text = ((EDO_TYPE)CbxVacOff.SelectedIndex).ToString();
            else
                LblVacOffName.Text = "None";
            if (CbxVacBreak.SelectedIndex >= 0 && (EDO_TYPE)CbxVacBreak.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblVacBreakName.Text = ((EDO_TYPE)CbxVacBreak.SelectedIndex).ToString();
            else
                LblVacBreakName.Text = "None";
            if (CbxShock1.SelectedIndex >= 0 && (EDO_TYPE)CbxShock1.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblShockName1.Text = ((EDO_TYPE)CbxShock1.SelectedIndex).ToString();
            else
                LblShockName1.Text = "None";
            if (CbxShock2.SelectedIndex >= 0 && (EDO_TYPE)CbxShock2.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblShockName2.Text = ((EDO_TYPE)CbxShock2.SelectedIndex).ToString();
            else
                LblShockName2.Text = "None";

            CbxVacBtn.BackColor = SystemColors.Window;
            CbxVacBtnLED.BackColor = SystemColors.Window;
            CbxVacDI1.BackColor = SystemColors.Window;
            CbxVacDI2.BackColor = SystemColors.Window;
            CbxVacOn.BackColor = SystemColors.Window;
            CbxVacOff.BackColor = SystemColors.Window;
            CbxVacBreak.BackColor = SystemColors.Window;
            CbxShock1.BackColor = SystemColors.Window;
            CbxShock2.BackColor = SystemColors.Window;
            NumUDVacOffDelayTimeValue.BackColor = SystemColors.Window;
            NumUDShockIntervalsValue.BackColor = SystemColors.Window;
            NumUDShockTimesValue.BackColor = SystemColors.Window;
        }

        private void CbxDoorName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex < 0)
            {
                CbxDoorDI.SelectedIndex = -1;
                CbxDoorDO.SelectedIndex = -1;
                CbxDoorDir.SelectedIndex = -1;
                CbxDoorBtnDI.SelectedIndex = -1;
                CbxDoorBtnLEDDO.SelectedIndex = -1;
                ChB_ShareDO.Visible = false;

                LblDoorSensorName.Text = "None";
                LblDoorLockName.Text = "None";
                LblDoorBtnDIName.Text = "None";
                LblDoorBtnLEDDOName.Text = "None";
                return;
            }

            CbxDoorDI.SelectedIndex = (int)G.Comm.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex);
            CbxDoorDO.SelectedIndex = (int)G.Comm.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex);
            CbxDoorDir.SelectedIndex = (int)G.Comm.Door.GetDoorDir((EDoorPos)CbxDoorName.SelectedIndex);
            CbxDoorBtnDI.SelectedIndex = (int)G.Comm.Door.GetDoorBtn((EDoorPos)CbxDoorName.SelectedIndex);
            CbxDoorBtnLEDDO.SelectedIndex = (int)G.Comm.Door.GetBtnLED((EDoorPos)CbxDoorName.SelectedIndex);
            ChB_ShareDO.Visible = true;
            ChB_ShareDO.Checked = G.Comm.Door.GetDoorDirShare((EDoorPos)CbxDoorName.SelectedIndex);
            ChB_OnlyAutoAlarm.Checked = G.Comm.Door.GetOnlyAutoAlarm();
            NymUDDoorAutoLockTimeValue.Value = (decimal)G.Comm.Door.GetLockTime();

            if (CbxDoorDI.SelectedIndex >= 0 && (EDI_TYPE)CbxDoorDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblDoorSensorName.Text = ((EDI_TYPE)CbxDoorDI.SelectedIndex).ToString();
            else
                LblDoorSensorName.Text = "None";
            if (CbxDoorDO.SelectedIndex >= 0 && (EDO_TYPE)CbxDoorDO.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblDoorLockName.Text = ((EDO_TYPE)CbxDoorDO.SelectedIndex).ToString();
            else
                LblDoorLockName.Text = "None";

            if (CbxDoorBtnDI.SelectedIndex >= 0 && (EDI_TYPE)CbxDoorBtnDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblDoorBtnDIName.Text = ((EDI_TYPE)CbxDoorBtnDI.SelectedIndex).ToString();
            else
                LblDoorBtnDIName.Text = "None";
            if (CbxDoorBtnLEDDO.SelectedIndex >= 0 && (EDO_TYPE)CbxDoorBtnLEDDO.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblDoorBtnLEDDOName.Text = ((EDO_TYPE)CbxDoorBtnLEDDO.SelectedIndex).ToString();
            else
                LblDoorBtnLEDDOName.Text = "None";

            CbxDoorDI.BackColor = SystemColors.Window;
            CbxDoorDO.BackColor = SystemColors.Window;
            CbxDoorDir.BackColor = SystemColors.Window;
            CbxDoorBtnDI.BackColor = SystemColors.Window;
            CbxDoorBtnLEDDO.BackColor = SystemColors.Window;
            ChB_ShareDO.BackColor = Color.Transparent;
            ChB_OnlyAutoAlarm.BackColor = Color.Transparent;
            NymUDDoorAutoLockTimeValue.BackColor = SystemColors.Window;
        }

        private void CbxTrolleyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex < 0)
            {
                CbxTrolleyDI.SelectedIndex = -1;
                CbxTrolleyCylA.SelectedIndex = -1;
                CbxTrolleyCylB.SelectedIndex = -1;

                LblTrolleySensorName.Text = "None";
                LblTrolleyCylAName.Text = "None";
                LblTrolleyCylBName.Text = "None";

                NymUDTrolleyAutoLockTimeValue.Value = 0;
                return;
            }

            CbxTrolleyDI.SelectedIndex = (int)G.Comm.Trolley.GetTrolleySensor((ETrolley)CbxTrolleyName.SelectedIndex);
            CbxTrolleyCylA.SelectedIndex = (int)G.Comm.Trolley.GetTrolleyCylA((ETrolley)CbxTrolleyName.SelectedIndex);
            CbxTrolleyCylB.SelectedIndex = (int)G.Comm.Trolley.GetTrolleyCylB((ETrolley)CbxTrolleyName.SelectedIndex);
            NymUDTrolleyAutoLockTimeValue.Value = (decimal)G.Comm.Trolley.GetLockTime();

            if (CbxTrolleyDI.SelectedIndex >= 0 && (EDI_TYPE)CbxTrolleyDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblTrolleySensorName.Text = ((EDI_TYPE)CbxTrolleyDI.SelectedIndex).ToString();
            else
                LblTrolleySensorName.Text = "None";
            if (CbxTrolleyCylA.SelectedIndex >= 0 && (ECylName)CbxTrolleyCylA.SelectedIndex != ECylName.Count)
                LblTrolleyCylAName.Text = ((ECylName)CbxTrolleyCylA.SelectedIndex).ToString();
            else
                LblTrolleyCylAName.Text = "None";
            if (CbxTrolleyCylB.SelectedIndex >= 0 && (ECylName)CbxTrolleyCylB.SelectedIndex != ECylName.Count)
                LblTrolleyCylBName.Text = ((ECylName)CbxTrolleyCylB.SelectedIndex).ToString();
            else
                LblTrolleyCylBName.Text = "None";

            CbxTrolleyDI.BackColor = SystemColors.Window;
            CbxTrolleyCylA.BackColor = SystemColors.Window;
            CbxTrolleyCylB.BackColor = SystemColors.Window;
            NymUDTrolleyAutoLockTimeValue.BackColor = SystemColors.Window;
        }

        private void CbxLiftsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex < 0)
            {
                CbxLiftsMotorErrDI.SelectedIndex = -1;
                CbxLiftsULimBoardDI.SelectedIndex = -1;
                CbxLiftsInpPlaceBoardDI.SelectedIndex = -1;
                CbxLiftsHavePalletDI.SelectedIndex = -1;
                CbxLiftsOnPalletDI.SelectedIndex = -1;
                CbxLiftsWorkULimDI.SelectedIndex = -1;
                CbxLiftsWorkLLimDI.SelectedIndex = -1;
                CbxLiftsMoveULimDI.SelectedIndex = -1;
                CbxLiftsMoveLLimDI.SelectedIndex = -1;
                CbxLiftsSafeLockDI_1.SelectedIndex = -1;
                CbxLiftsSafeLockDI_2.SelectedIndex = -1;
                CbxLiftsUp.SelectedIndex = -1;
                CbxLiftsDown.SelectedIndex = -1;
                CbxLiftsBtnUp.SelectedIndex = -1;
                CbxLiftsBtnLEDUp.SelectedIndex = -1;
                CbxLiftsBtnDown.SelectedIndex = -1;
                CbxLiftsBtnLEDDown.SelectedIndex = -1;

                LblLiftsMotorErrSensorName.Text = "None";
                LblLiftsULimBoardSensorName.Text = "None";
                LblLiftsInPlaceSensorName.Text = "None";
                LblLiftsHavePalletSensorName.Text = "None";
                LblLiftsOnPalletSensorName.Text = "None";
                LblLiftsWorkULimSensorName.Text = "None";
                LblLiftsWorkLLimSensorName.Text = "None";
                LblLiftsMoveULimSensorName.Text = "None";
                LblLiftsMoveLLimSensorName.Text = "None";
                LblLiftsUpName.Text = "None";
                LblLiftsDownName.Text = "None";
                LblLiftsBtnUpName.Text = "None";
                LblLiftsBtnLEDUpName.Text = "None";
                LblLiftsBtnDownName.Text = "None";
                LblLiftsBtnLEDDownName.Text = "None";
                return;
            }

            CbxLiftsMotorErrDI.SelectedIndex = (int)G.Comm.Lifts.GetMotorError((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsULimBoardDI.SelectedIndex = (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsInpPlaceBoardDI.SelectedIndex = (int)G.Comm.Lifts.GetInPlace((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsHavePalletDI.SelectedIndex = (int)G.Comm.Lifts.GetPallet((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsOnPalletDI.SelectedIndex = (int)G.Comm.Lifts.GetOnPallet((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsWorkULimDI.SelectedIndex = (int)G.Comm.Lifts.GetULim_Work((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsWorkLLimDI.SelectedIndex = (int)G.Comm.Lifts.GetLLim_Work((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsMoveULimDI.SelectedIndex = (int)G.Comm.Lifts.GetULim_Move((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsMoveLLimDI.SelectedIndex = (int)G.Comm.Lifts.GetLLim_Move((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsSafeLockDI_1.SelectedIndex = (int)G.Comm.Lifts.GetSafeLock1((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsSafeLockDI_2.SelectedIndex = (int)G.Comm.Lifts.GetSafeLock2((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsUp.SelectedIndex = (int)G.Comm.Lifts.GetUp((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsDown.SelectedIndex = (int)G.Comm.Lifts.GetDown((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsBtnUp.SelectedIndex = (int)G.Comm.Lifts.GetBtnUp((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsBtnLEDUp.SelectedIndex = (int)G.Comm.Lifts.GetBtnUpLED((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsBtnDown.SelectedIndex = (int)G.Comm.Lifts.GetBtnDown((ELifts)CbxLiftsName.SelectedIndex);
            CbxLiftsBtnLEDDown.SelectedIndex = (int)G.Comm.Lifts.GetBtnDownLED((ELifts)CbxLiftsName.SelectedIndex);

            if (CbxLiftsMotorErrDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsMotorErrDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsMotorErrSensorName.Text = ((EDI_TYPE)CbxLiftsMotorErrDI.SelectedIndex).ToString();
            else
                LblLiftsMotorErrSensorName.Text = "None";
            if (CbxLiftsULimBoardDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsULimBoardDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsULimBoardSensorName.Text = ((EDI_TYPE)CbxLiftsULimBoardDI.SelectedIndex).ToString();
            else
                LblLiftsULimBoardSensorName.Text = "None";
            if (CbxLiftsInpPlaceBoardDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsInpPlaceBoardDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsInPlaceSensorName.Text = ((EDI_TYPE)CbxLiftsInpPlaceBoardDI.SelectedIndex).ToString();
            else
                LblLiftsInPlaceSensorName.Text = "None";
            if (CbxLiftsHavePalletDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsHavePalletDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsHavePalletSensorName.Text = ((EDI_TYPE)CbxLiftsHavePalletDI.SelectedIndex).ToString();
            else
                LblLiftsHavePalletSensorName.Text = "None";
            if (CbxLiftsOnPalletDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsOnPalletDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsOnPalletSensorName.Text = ((EDI_TYPE)CbxLiftsOnPalletDI.SelectedIndex).ToString();
            else
                LblLiftsOnPalletSensorName.Text = "None";
            if (CbxLiftsWorkULimDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsWorkULimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsWorkULimSensorName.Text = ((EDI_TYPE)CbxLiftsWorkULimDI.SelectedIndex).ToString();
            else
                LblLiftsWorkULimSensorName.Text = "None";
            if (CbxLiftsWorkLLimDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsWorkLLimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsWorkLLimSensorName.Text = ((EDI_TYPE)CbxLiftsWorkLLimDI.SelectedIndex).ToString();
            else
                LblLiftsWorkLLimSensorName.Text = "None";
            if (CbxLiftsMoveULimDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsMoveULimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsMoveULimSensorName.Text = ((EDI_TYPE)CbxLiftsMoveULimDI.SelectedIndex).ToString();
            else
                LblLiftsMoveULimSensorName.Text = "None";
            if (CbxLiftsMoveLLimDI.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsMoveLLimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsMoveLLimSensorName.Text = ((EDI_TYPE)CbxLiftsMoveLLimDI.SelectedIndex).ToString();
            else
                LblLiftsMoveLLimSensorName.Text = "None";
            if (CbxLiftsSafeLockDI_1.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsSafeLockDI_1.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsSafeLockSensorName_1.Text = ((EDI_TYPE)CbxLiftsSafeLockDI_1.SelectedIndex).ToString();
            else
                LblLiftsSafeLockSensorName_1.Text = "None";
            if (CbxLiftsSafeLockDI_2.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsSafeLockDI_2.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsSafeLockSensorName_2.Text = ((EDI_TYPE)CbxLiftsSafeLockDI_2.SelectedIndex).ToString();
            else
                LblLiftsSafeLockSensorName_2.Text = "None";
            if (CbxLiftsUp.SelectedIndex >= 0 && (EDO_TYPE)CbxLiftsUp.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblLiftsUpName.Text = ((EDO_TYPE)CbxLiftsUp.SelectedIndex).ToString();
            else
                LblLiftsUpName.Text = "None";
            if (CbxLiftsDown.SelectedIndex >= 0 && (EDO_TYPE)CbxLiftsDown.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblLiftsDownName.Text = ((EDO_TYPE)CbxLiftsDown.SelectedIndex).ToString();
            else
                LblLiftsDownName.Text = "None";
            if (CbxLiftsBtnUp.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsBtnUp.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsBtnUpName.Text = ((EDO_TYPE)CbxLiftsBtnUp.SelectedIndex).ToString();
            else
                LblLiftsBtnUpName.Text = "None";
            if (CbxLiftsBtnLEDUp.SelectedIndex >= 0 && (EDO_TYPE)CbxLiftsBtnLEDUp.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblLiftsBtnLEDUpName.Text = ((EDO_TYPE)CbxLiftsBtnLEDUp.SelectedIndex).ToString();
            else
                LblLiftsBtnLEDUpName.Text = "None";
            if (CbxLiftsBtnDown.SelectedIndex >= 0 && (EDI_TYPE)CbxLiftsBtnDown.SelectedIndex != EDI_TYPE.DI_COUNT)
                LblLiftsBtnDownName.Text = ((EDO_TYPE)CbxLiftsBtnDown.SelectedIndex).ToString();
            else
                LblLiftsBtnDownName.Text = "None";
            if (CbxLiftsBtnLEDDown.SelectedIndex >= 0 && (EDO_TYPE)CbxLiftsBtnLEDDown.SelectedIndex != EDO_TYPE.DO_COUNT)
                LblLiftsBtnLEDDownName.Text = ((EDO_TYPE)CbxLiftsBtnLEDDown.SelectedIndex).ToString();
            else
                LblLiftsBtnLEDDownName.Text = "None";

            CbxLiftsMotorErrDI.BackColor = SystemColors.Window;
            CbxLiftsULimBoardDI.BackColor = SystemColors.Window;
            CbxLiftsInpPlaceBoardDI.BackColor = SystemColors.Window;
            CbxLiftsHavePalletDI.BackColor = SystemColors.Window;
            CbxLiftsOnPalletDI.BackColor = SystemColors.Window;
            CbxLiftsWorkULimDI.BackColor = SystemColors.Window;
            CbxLiftsWorkLLimDI.BackColor = SystemColors.Window;
            CbxLiftsMoveULimDI.BackColor = SystemColors.Window;
            CbxLiftsMoveLLimDI.BackColor = SystemColors.Window;
            CbxLiftsSafeLockDI_1.BackColor = SystemColors.Window;
            CbxLiftsSafeLockDI_2.BackColor = SystemColors.Window;
            CbxLiftsUp.BackColor = SystemColors.Window;
            CbxLiftsDown.BackColor = SystemColors.Window;
            CbxLiftsBtnUp.BackColor = SystemColors.Window;
            CbxLiftsBtnLEDUp.BackColor = SystemColors.Window;
            CbxLiftsBtnDown.BackColor = SystemColors.Window;
            CbxLiftsBtnLEDDown.BackColor = SystemColors.Window;
        }

        private void ShowNumKeyboard(object sender, EventArgs e)
        {
            FmNumKeyboard k = new FmNumKeyboard((NumericUpDown)sender);
            k.StartPosition = FormStartPosition.CenterScreen;
            k.ShowDialog();
        }

        private void TabPageSetColor(TabControl TabCtl)
        {
            for (int i = 0; i < TabCtl.TabPages.Count; i++)
                TabCtl.TabPages[i].BackColor = Color.FromArgb(0xdd2378);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (TabCtl.SelectedIndex == 0)//氣缸
            {
                G.Comm.CYL.SetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension, (EDI_TYPE)CbxCylinderDI_Extension.SelectedIndex);
                G.Comm.CYL.SetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract, (EDI_TYPE)CbxCylinderDI_Retract.SelectedIndex);
                G.Comm.CYL.SetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension, (EDO_TYPE)CbxCylinderDO_Extension.SelectedIndex);
                G.Comm.CYL.SetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract, (EDO_TYPE)CbxCylinderDO_Retract.SelectedIndex);

                G.Comm.CYL.Save();

                CbxCylinderDI_Extension.BackColor = SystemColors.Window;
                CbxCylinderDI_Retract.BackColor = SystemColors.Window;
                CbxCylinderDO_Extension.BackColor = SystemColors.Window;
                CbxCylinderDO_Retract.BackColor = SystemColors.Window;
            }//氣缸
            else if (TabCtl.SelectedIndex == 1)//吸盤組
            {
                G.Comm.Vac.SetVacBtn((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, (EDI_TYPE)CbxVacBtn.SelectedIndex, (EDO_TYPE)CbxVacBtnLED.SelectedIndex);
                G.Comm.Vac.SetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor1, (EDI_TYPE)CbxVacDI1.SelectedIndex);
                G.Comm.Vac.SetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor2, (EDI_TYPE)CbxVacDI2.SelectedIndex);
                G.Comm.Vac.SetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOn, (EDO_TYPE)CbxVacOn.SelectedIndex);
                G.Comm.Vac.SetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOff, (EDO_TYPE)CbxVacOff.SelectedIndex);
                G.Comm.Vac.SetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumBreak, (EDO_TYPE)CbxVacBreak.SelectedIndex);
                G.Comm.Vac.SetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock1, (EDO_TYPE)CbxShock1.SelectedIndex);
                G.Comm.Vac.SetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock2, (EDO_TYPE)CbxShock2.SelectedIndex);
                G.Comm.Vac.SetVacOffDelay((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, (double)NumUDVacOffDelayTimeValue.Value);
                G.Comm.Vac.SetVacShockIntervals((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, (int)(NumUDShockIntervalsValue.Value * 1000));
                G.Comm.Vac.SetVacShockTimes((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, (int)NumUDShockTimesValue.Value);

                G.Comm.Vac.Save();

                CbxVacBtn.BackColor = SystemColors.Window;
                CbxVacBtnLED.BackColor = SystemColors.Window;
                CbxVacDI1.BackColor = SystemColors.Window;
                CbxVacDI2.BackColor = SystemColors.Window;
                CbxVacOn.BackColor = SystemColors.Window;
                CbxVacOff.BackColor = SystemColors.Window;
                CbxVacBreak.BackColor = SystemColors.Window;
                CbxShock1.BackColor = SystemColors.Window;
                CbxShock2.BackColor = SystemColors.Window;
                NumUDVacOffDelayTimeValue.BackColor = SystemColors.Window;
                NumUDShockIntervalsValue.BackColor = SystemColors.Window;
                NumUDShockTimesValue.BackColor = SystemColors.Window;
            }//吸盤組
            else if (TabCtl.SelectedIndex == 2)//門
            {
                G.Comm.Door.SetDoorDI((EDoorPos)CbxDoorName.SelectedIndex, (EDI_TYPE)CbxDoorDI.SelectedIndex);
                G.Comm.Door.SetDoorDO((EDoorPos)CbxDoorName.SelectedIndex, (EDO_TYPE)CbxDoorDO.SelectedIndex);
                G.Comm.Door.SetDoorDir((EDoorPos)CbxDoorName.SelectedIndex, (EDoorDir)CbxDoorDir.SelectedIndex);
                G.Comm.Door.SetDoorBtn((EDoorPos)CbxDoorName.SelectedIndex, (EDI_TYPE)CbxDoorBtnDI.SelectedIndex);
                G.Comm.Door.SetBtnLED((EDoorPos)CbxDoorName.SelectedIndex, (EDO_TYPE)CbxDoorBtnLEDDO.SelectedIndex);
                G.Comm.Door.SetDoorDirShare((EDoorPos)CbxDoorName.SelectedIndex, ChB_ShareDO.Checked);
                G.Comm.Door.SetOnlyAutoAlarm(ChB_OnlyAutoAlarm.Checked);
                G.Comm.Door.SetLockTime((double)NymUDDoorAutoLockTimeValue.Value);

                G.Comm.Door.Save();

                CbxDoorDI.BackColor = SystemColors.Window;
                CbxDoorDO.BackColor = SystemColors.Window;
                CbxDoorDir.BackColor = SystemColors.Window;
                CbxDoorBtnDI.BackColor = SystemColors.Window;
                CbxDoorBtnLEDDO.BackColor = SystemColors.Window;
                ChB_ShareDO.BackColor = Color.Transparent;
                ChB_OnlyAutoAlarm.BackColor = Color.Transparent;
                NymUDDoorAutoLockTimeValue.BackColor = SystemColors.Window;
            }//門
            else if (TabCtl.SelectedIndex == 3)//台車
            {
                G.Comm.Trolley.SetTrolleySensor((ETrolley)CbxTrolleyName.SelectedIndex, (EDI_TYPE)CbxTrolleyDI.SelectedIndex);
                G.Comm.Trolley.SetTrolleyCyl((ETrolley)CbxTrolleyName.SelectedIndex, (ECylName)CbxTrolleyCylA.SelectedIndex, (ECylName)CbxTrolleyCylB.SelectedIndex);
                G.Comm.Trolley.SetLockTime((double)NymUDTrolleyAutoLockTimeValue.Value);

                G.Comm.Trolley.Save();

                CbxTrolleyDI.BackColor = SystemColors.Window;
                CbxTrolleyCylA.BackColor = SystemColors.Window;
                CbxTrolleyCylB.BackColor = SystemColors.Window;
                NymUDTrolleyAutoLockTimeValue.BackColor = SystemColors.Window;
            }//台車
            else if (TabCtl.SelectedIndex == 4)//升降台
            {
                G.Comm.Lifts.SetMotorError((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsMotorErrDI.SelectedIndex);
                G.Comm.Lifts.SetULim_Board((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsULimBoardDI.SelectedIndex);
                G.Comm.Lifts.SetInPlace((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsInpPlaceBoardDI.SelectedIndex);
                G.Comm.Lifts.SetPallet((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsHavePalletDI.SelectedIndex);
                G.Comm.Lifts.SetOnPallet((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsOnPalletDI.SelectedIndex);
                G.Comm.Lifts.SetULim_Work((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsWorkULimDI.SelectedIndex);
                G.Comm.Lifts.SetLLim_Work((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsWorkLLimDI.SelectedIndex);
                G.Comm.Lifts.SetULim_Move((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsMoveULimDI.SelectedIndex);
                G.Comm.Lifts.SetLLim_Move((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsMoveLLimDI.SelectedIndex);
                G.Comm.Lifts.SetSafeLock1((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsSafeLockDI_1.SelectedIndex);
                G.Comm.Lifts.SetSafeLock2((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsSafeLockDI_2.SelectedIndex);
                G.Comm.Lifts.SetUp((ELifts)CbxLiftsName.SelectedIndex, (EDO_TYPE)CbxLiftsUp.SelectedIndex);
                G.Comm.Lifts.SetDown((ELifts)CbxLiftsName.SelectedIndex, (EDO_TYPE)CbxLiftsDown.SelectedIndex);
                G.Comm.Lifts.SetBtnUp((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsBtnUp.SelectedIndex);
                G.Comm.Lifts.SetBtnUpLED((ELifts)CbxLiftsName.SelectedIndex, (EDO_TYPE)CbxLiftsBtnLEDUp.SelectedIndex);
                G.Comm.Lifts.SetBtnDown((ELifts)CbxLiftsName.SelectedIndex, (EDI_TYPE)CbxLiftsBtnDown.SelectedIndex);
                G.Comm.Lifts.SetBtnDownLED((ELifts)CbxLiftsName.SelectedIndex, (EDO_TYPE)CbxLiftsBtnLEDDown.SelectedIndex);

                G.Comm.Lifts.Save();

                CbxLiftsMotorErrDI.BackColor = SystemColors.Window;
                CbxLiftsULimBoardDI.BackColor = SystemColors.Window;
                CbxLiftsInpPlaceBoardDI.BackColor = SystemColors.Window;
                CbxLiftsHavePalletDI.BackColor = SystemColors.Window;
                CbxLiftsOnPalletDI.BackColor = SystemColors.Window;
                CbxLiftsWorkULimDI.BackColor = SystemColors.Window;
                CbxLiftsWorkLLimDI.BackColor = SystemColors.Window;
                CbxLiftsMoveULimDI.BackColor = SystemColors.Window;
                CbxLiftsMoveLLimDI.BackColor = SystemColors.Window;
                CbxLiftsSafeLockDI_1.BackColor = SystemColors.Window;
                CbxLiftsSafeLockDI_2.BackColor = SystemColors.Window;
                CbxLiftsUp.BackColor = SystemColors.Window;
                CbxLiftsDown.BackColor = SystemColors.Window;
                CbxLiftsBtnUp.BackColor = SystemColors.Window;
                CbxLiftsBtnLEDUp.BackColor = SystemColors.Window;
                CbxLiftsBtnDown.BackColor = SystemColors.Window;
                CbxLiftsBtnLEDDown.BackColor = SystemColors.Window;
            }//升降台
            else if (TabCtl.SelectedIndex == 5)//震動送料機
            {
                G.Comm.VibBow.SetEmptySensor((EVibBow)CbxVibBowName.SelectedIndex, (EDI_TYPE)CbxVibBowEmptySensor.SelectedIndex);
                G.Comm.VibBow.SetPower((EVibBow)CbxVibBowName.SelectedIndex, (EDO_TYPE)CbxVibBowPower.SelectedIndex);
                G.Comm.VibBow.SetStartDelay((EVibBow)CbxVibBowName.SelectedIndex, (double)NumUDVibBowStartDelayValue.Value);
                G.Comm.VibBow.SetStopDelay((EVibBow)CbxVibBowName.SelectedIndex, (double)NumUDVibBowStopDelayValue.Value);
                G.Comm.VibBow.SetEmptyAlarmTime((EVibBow)CbxVibBowName.SelectedIndex, (double)NumUDVibBowAlarmTimeValue.Value);

                G.Comm.VibBow.Save();

                CbxVibBowEmptySensor.BackColor = SystemColors.Window;
                CbxVibBowPower.BackColor = SystemColors.Window;
                NumUDVibBowStartDelayValue.BackColor = SystemColors.Window;
                NumUDVibBowStopDelayValue.BackColor = SystemColors.Window;
                NumUDVibBowAlarmTimeValue.BackColor = SystemColors.Window;
            }//震動送料機
        }

        private void frmModSet_FormClosing(object sender, FormClosingEventArgs e) { }

        private void BtnRetract_Click(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex < 0)
                return;

            G.Comm.CYL.Retract((ECylName)CbxCylinderName.SelectedIndex);
            panel1.BackgroundImage = GJSControl.Properties.Resources.Cylinder_5;
        }
        private void BtnExtension_Click(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex < 0)
                return;

            G.Comm.CYL.Extension((ECylName)CbxCylinderName.SelectedIndex);
            panel1.BackgroundImage = GJSControl.Properties.Resources.Cylinder_5e;
        }

        private void BtnVacOn_Click(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex < 0)
                return;

            G.Comm.Vac.On((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
        }
        private void BtnVacOff_Click(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex < 0)
                return;

            G.Comm.Vac.Off((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
        }
        private void BtnVacBreak_Click(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex < 0)
                return;

            G.Comm.Vac.Break((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
        }
        private void BtnShock_Click(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex < 0)
                return;

            switch (G.Comm.Vac.GetSuckerAction((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex))
            {
                case ESuckerMotion.Up:
                    G.Comm.Vac.Down((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
                    break;
                case ESuckerMotion.Down:
                    G.Comm.Vac.Up((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
                    break;
                default:
                    G.Comm.Vac.Up((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
                    break;
            }
        }
        private void BtnShockTest_Click(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex < 0)
                return;

            if (G.Comm.Vac.GetVacShockTimes((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex) > 0)
            {
                CbxVaccumSuckerName.Enabled = false;

                int iTimeOut = Environment.TickCount;
                G.Comm.Vac.ShockTimeReset((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);

                while (!G.Comm.Vac.SuckerDone((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex) &&
                    Environment.TickCount - iTimeOut < 5000)
                {
                    Thread.Sleep(1);
                }

                G.Comm.Vac.Down((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex);
                CbxVaccumSuckerName.Enabled = true;
            }
        }

        private void BtnDoorUnLock_Click(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex < 0)
                return;

            G.Comm.Door.SingleUnlock((EDoorPos)CbxDoorName.SelectedIndex);
        }
        private void BtnDoorLock_Click(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex < 0)
                return;

            G.Comm.Door.SingleLock((EDoorPos)CbxDoorName.SelectedIndex);
        }

        private void BtnTrolleyUnLock_Click(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex < 0)
                return;

            G.Comm.Trolley.Unlock((ETrolley)CbxTrolleyName.SelectedIndex);
        }
        private void BtnTrolleyLock_Click(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex < 0)
                return;

            G.Comm.Trolley.Lock((ETrolley)CbxTrolleyName.SelectedIndex);
        }

        bool LiftsUpMoving = false;
        bool LiftsDownMoving = false;
        private void BtnLiftsDown_MouseDown(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex < 0)
                return;

            if (!LiftsDownMoving)
            {
                LiftsDownMoving = true;
                G.Comm.Lifts.Down((ELifts)CbxLiftsName.SelectedIndex);
            }
        }
        private void BtnLiftsUp_MouseDown(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex < 0)
                return;

            if (!LiftsUpMoving)
            {
                LiftsUpMoving = true;
                G.Comm.Lifts.Up((ELifts)CbxLiftsName.SelectedIndex);
            }
        }
        private void BtnLifts_MouseUp(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex < 0)
                return;

            if (LiftsUpMoving || LiftsDownMoving)
            {
                LiftsUpMoving = false;
                LiftsDownMoving = false;
                G.Comm.Lifts.Stop((ELifts)CbxLiftsName.SelectedIndex);
            }
        }

        #region 氣缸
        private void CbxCylinderDI_Retract_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex >= 0)
            {
                if (CbxCylinderDI_Retract.SelectedIndex == (int)G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract))
                    CbxCylinderDI_Retract.BackColor = SystemColors.Window;
                else
                    CbxCylinderDI_Retract.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxCylinderDI_Retract.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblRetractDIName.Text = ((EDI_TYPE)CbxCylinderDI_Retract.SelectedIndex).ToString();
                else
                    LblRetractDIName.Text = "None";
            }
            else
            {
                CbxCylinderDI_Retract.BackColor = SystemColors.Window;
                LblRetractDIName.Text = "None";
            }
        }
        private void CbxCylinderDI_Extension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex >= 0)
            {
                if (CbxCylinderDI_Extension.SelectedIndex == (int)G.Comm.CYL.GetCylinderDI((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension))
                    CbxCylinderDI_Extension.BackColor = SystemColors.Window;
                else
                    CbxCylinderDI_Extension.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxCylinderDI_Extension.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblExtexsionDIName.Text = ((EDI_TYPE)CbxCylinderDI_Extension.SelectedIndex).ToString();
                else
                    LblExtexsionDIName.Text = "None";
            }
            else
            {
                CbxCylinderDI_Extension.BackColor = SystemColors.Window;
                LblExtexsionDIName.Text = "None";
            }
        }
        private void CbxCylinderDO_Retract_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex >= 0)
            {
                if (CbxCylinderDO_Retract.SelectedIndex == (int)G.Comm.CYL.GetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Retract))
                    CbxCylinderDO_Retract.BackColor = SystemColors.Window;
                else
                    CbxCylinderDO_Retract.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxCylinderDO_Retract.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblRetractDOName.Text = ((EDO_TYPE)CbxCylinderDO_Retract.SelectedIndex).ToString();
                else
                    LblRetractDOName.Text = "None";
            }
            else
            {
                CbxCylinderDO_Retract.BackColor = SystemColors.Window;
                LblRetractDOName.Text = "None";
            }
        }
        private void CbxCylinderDO_Extension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxCylinderName.SelectedIndex >= 0)
            {
                if (CbxCylinderDO_Extension.SelectedIndex == (int)G.Comm.CYL.GetCylinderDO((ECylName)CbxCylinderName.SelectedIndex, ECylMotion.Extension))
                    CbxCylinderDO_Extension.BackColor = SystemColors.Window;
                else
                    CbxCylinderDO_Extension.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxCylinderDO_Extension.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblExtensionDOName.Text = ((EDO_TYPE)CbxCylinderDO_Extension.SelectedIndex).ToString();
                else
                    LblExtensionDOName.Text = "None";
            }
            else
            {
                CbxCylinderDO_Extension.BackColor = SystemColors.Window;
                LblExtensionDOName.Text = "None";
            }
        }
        #endregion

        #region 吸盤組
        private void CbxVacBtn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)//CbxVacBtn
            {
                if (CbxVacBtn.SelectedIndex == (int)G.Comm.Vac.GetVacBtn((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex))
                    CbxVacBtn.BackColor = SystemColors.Window;
                else
                    CbxVacBtn.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxVacBtn.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblVacBtnName.Text = ((EDI_TYPE)CbxVacBtn.SelectedIndex).ToString();
                else
                    LblVacBtnName.Text = "None";
            }
            else
            {
                CbxVacBtn.BackColor = SystemColors.Window;
                LblVacBtnName.Text = "None";
            }
        }
        private void CbxVacBtnLED_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxVacBtnLED.SelectedIndex == (int)G.Comm.Vac.GetVacBtnLED((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex))
                    CbxVacBtnLED.BackColor = SystemColors.Window;
                else
                    CbxVacBtnLED.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxVacBtnLED.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblVacBtnLEDName.Text = ((EDO_TYPE)CbxVacBtnLED.SelectedIndex).ToString();
                else
                    LblVacBtnLEDName.Text = "None";
            }
            else
            {
                CbxVacBtnLED.BackColor = SystemColors.Window;
                LblVacBtnLEDName.Text = "None";
            }
        }

        private void CbxVacDI1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxVacDI1.SelectedIndex == (int)G.Comm.Vac.GetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor1))
                    CbxVacDI1.BackColor = SystemColors.Window;
                else
                    CbxVacDI1.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxVacDI1.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblVacName1.Text = ((EDI_TYPE)CbxVacDI1.SelectedIndex).ToString();
                else
                    LblVacName1.Text = "None";
            }
            else
            {
                CbxVacDI1.BackColor = SystemColors.Window;
                LblVacName1.Text = "None";
            }
        }
        private void CbxVacDI2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxVacDI2.SelectedIndex == (int)G.Comm.Vac.GetVacDI((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacSensorFm.VaccumSensor2))
                    CbxVacDI2.BackColor = SystemColors.Window;
                else
                    CbxVacDI2.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxVacDI2.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblVacName2.Text = ((EDI_TYPE)CbxVacDI2.SelectedIndex).ToString();
                else
                    LblVacName2.Text = "None";
            }
            else
            {
                CbxVacDI2.BackColor = SystemColors.Window;
                LblVacName2.Text = "None";
            }
        }
        private void CbxVacOn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxVacOn.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOn))
                    CbxVacOn.BackColor = SystemColors.Window;
                else
                    CbxVacOn.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxVacOn.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblVacOnName.Text = ((EDO_TYPE)CbxVacOn.SelectedIndex).ToString();
                else
                    LblVacOnName.Text = "None";
            }
            else
            {
                CbxVacOn.BackColor = SystemColors.Window;
                LblVacOnName.Text = "None";
            }
        }
        private void CbxVacOff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxVacOff.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumOff))
                    CbxVacOff.BackColor = SystemColors.Window;
                else
                    CbxVacOff.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxVacOff.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblVacOffName.Text = ((EDO_TYPE)CbxVacOff.SelectedIndex).ToString();
                else
                    LblVacOffName.Text = "None";
            }
            else
            {
                CbxVacOff.BackColor = SystemColors.Window;
                LblVacOffName.Text = "None";
            }
        }
        private void CbxVacBreak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxVacBreak.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.VaccumBreak))
                    CbxVacBreak.BackColor = SystemColors.Window;
                else
                    CbxVacBreak.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxVacBreak.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblVacBreakName.Text = ((EDO_TYPE)CbxVacBreak.SelectedIndex).ToString();
                else
                    LblVacBreakName.Text = "None";
            }
            else
            {
                CbxVacBreak.BackColor = SystemColors.Window;
                LblVacBreakName.Text = "None";
            }
        }
        private void CbxShock1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxShock1.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock1))
                    CbxShock1.BackColor = SystemColors.Window;
                else
                    CbxShock1.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxShock1.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblShockName1.Text = ((EDO_TYPE)CbxShock1.SelectedIndex).ToString();
                else
                    LblShockName1.Text = "None";
            }
            else
            {
                CbxShock1.BackColor = SystemColors.Window;
                LblShockName1.Text = "None";
            }
        }
        private void CbxShock2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (CbxShock2.SelectedIndex == (int)G.Comm.Vac.GetVacDO((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex, EVacDOFm.Shock2))
                    CbxShock2.BackColor = SystemColors.Window;
                else
                    CbxShock2.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxShock2.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblShockName2.Text = ((EDO_TYPE)CbxShock2.SelectedIndex).ToString();
                else
                    LblShockName2.Text = "None";
            }
            else
            {
                CbxShock2.BackColor = SystemColors.Window;
                LblShockName2.Text = "None";
            }
        }
        private void NumUDVacOffDelayTimeValue_ValueChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (NumUDVacOffDelayTimeValue.Value == (decimal)G.Comm.Vac.GetVacOffDelay((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex))
                    NumUDVacOffDelayTimeValue.BackColor = SystemColors.Window;
                else
                    NumUDVacOffDelayTimeValue.BackColor = Color.LightPink;
            }
            else
                NumUDVacOffDelayTimeValue.BackColor = SystemColors.Window;
        }
        private void NumUDShockIntervalsValue_ValueChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (NumUDShockIntervalsValue.Value == (decimal)((double)G.Comm.Vac.GetVacShockIntervals((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex)) / 1000)
                    NumUDShockIntervalsValue.BackColor = SystemColors.Window;
                else
                    NumUDShockIntervalsValue.BackColor = Color.LightPink;
            }
            else
                NumUDShockIntervalsValue.BackColor = SystemColors.Window;
        }
        private void NumUDShockTimesValue_ValueChanged(object sender, EventArgs e)
        {
            if (CbxVaccumSuckerName.SelectedIndex >= 0)
            {
                if (NumUDShockTimesValue.Value == (decimal)G.Comm.Vac.GetVacShockTimes((EVacSuckerName)CbxVaccumSuckerName.SelectedIndex))
                    NumUDShockTimesValue.BackColor = SystemColors.Window;
                else
                    NumUDShockTimesValue.BackColor = Color.LightPink;
            }
            else
                NumUDShockTimesValue.BackColor = SystemColors.Window;
        }
        #endregion

        #region 門鎖
        private void CbxDoorDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex >= 0)
            {
                if (CbxDoorDI.SelectedIndex == (int)G.Comm.Door.GetDoorDI((EDoorPos)CbxDoorName.SelectedIndex))
                    CbxDoorDI.BackColor = SystemColors.Window;
                else
                    CbxDoorDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxDoorDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblDoorSensorName.Text = ((EDI_TYPE)CbxDoorDI.SelectedIndex).ToString();
                else
                    LblDoorSensorName.Text = "None";
            }
            else
            {
                CbxDoorDI.BackColor = SystemColors.Window;
                LblDoorSensorName.Text = "None";
            }
        }
        private void CbxDoorDO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex >= 0)
            {
                if (CbxDoorDO.SelectedIndex == (int)G.Comm.Door.GetDoorDO((EDoorPos)CbxDoorName.SelectedIndex))
                    CbxDoorDO.BackColor = SystemColors.Window;
                else
                    CbxDoorDO.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxDoorDO.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblDoorLockName.Text = ((EDO_TYPE)CbxDoorDO.SelectedIndex).ToString();
                else
                    LblDoorLockName.Text = "None";
            }
            else
            {
                CbxDoorDO.BackColor = SystemColors.Window;
                LblDoorLockName.Text = "None";
            }
        }
        private void CbxDoorDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex >= 0)
            {
                if (CbxDoorDir.SelectedIndex == (int)G.Comm.Door.GetDoorDir((EDoorPos)CbxDoorName.SelectedIndex))
                    CbxDoorDir.BackColor = SystemColors.Window;
                else
                    CbxDoorDir.BackColor = Color.LightPink;
            }
            else
            {
                CbxDoorDir.BackColor = SystemColors.Window;
            }
        }
        private void NymUDDoorAutoLockTimeValue_ValueChanged(object sender, EventArgs e)
        {
            if (CbxDoorName.SelectedIndex >= 0)
            {
                if (NymUDDoorAutoLockTimeValue.Value == (decimal)G.Comm.Door.GetLockTime())
                    NymUDDoorAutoLockTimeValue.BackColor = SystemColors.Window;
                else
                    NymUDDoorAutoLockTimeValue.BackColor = Color.LightPink;
            }
            else
            {
                NymUDDoorAutoLockTimeValue.BackColor = SystemColors.Window;
            }
        }
        private void ChB_OnlyAutoAlarm_CheckedChanged(object sender, EventArgs e)
        {
            if (ChB_OnlyAutoAlarm.Checked != G.Comm.Door.GetOnlyAutoAlarm())
                ChB_OnlyAutoAlarm.BackColor = Color.LightPink;
            else
                ChB_OnlyAutoAlarm.BackColor = Color.Transparent;
        }
        private void ChB_ShareDO_CheckedChanged(object sender, EventArgs e)
        {
            if (ChB_ShareDO.Checked != G.Comm.Door.GetDoorDirShare((EDoorPos)CbxDoorName.SelectedIndex))
                ChB_ShareDO.BackColor = Color.LightPink;
            else
                ChB_ShareDO.BackColor = Color.Transparent;
        }
        #endregion

        #region 台車
        private void CbxTrolleyDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex >= 0)
            {
                if (CbxTrolleyDI.SelectedIndex == (int)G.Comm.Trolley.GetTrolleySensor((ETrolley)CbxTrolleyName.SelectedIndex))
                    CbxTrolleyDI.BackColor = SystemColors.Window;
                else
                    CbxTrolleyDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxTrolleyDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblTrolleySensorName.Text = ((EDI_TYPE)CbxTrolleyDI.SelectedIndex).ToString();
                else
                    LblTrolleySensorName.Text = "None";
            }
            else
            {
                CbxTrolleyDI.BackColor = SystemColors.Window;
                LblTrolleySensorName.Text = "None";
            }
        }
        private void CbxTrolleyCylA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex >= 0)
            {
                if (CbxTrolleyCylA.SelectedIndex == (int)G.Comm.Trolley.GetTrolleyCylA((ETrolley)CbxTrolleyName.SelectedIndex))
                    CbxTrolleyCylA.BackColor = SystemColors.Window;
                else
                    CbxTrolleyCylA.BackColor = Color.LightPink;

                if ((ECylName)CbxTrolleyCylA.SelectedIndex != ECylName.Count)
                    LblTrolleyCylAName.Text = ((ECylName)CbxTrolleyCylA.SelectedIndex).ToString();
                else
                    LblTrolleyCylAName.Text = "None";
            }
            else
            {
                CbxTrolleyCylA.BackColor = SystemColors.Window;
                LblTrolleyCylAName.Text = "None";
            }
        }
        private void CbxTrolleyCylB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex >= 0)
            {
                if (CbxTrolleyCylB.SelectedIndex == (int)G.Comm.Trolley.GetTrolleyCylB((ETrolley)CbxTrolleyName.SelectedIndex))
                    CbxTrolleyCylB.BackColor = SystemColors.Window;
                else
                    CbxTrolleyCylB.BackColor = Color.LightPink;

                if ((ECylName)CbxTrolleyCylB.SelectedIndex != ECylName.Count)
                    LblTrolleyCylBName.Text = ((ECylName)CbxTrolleyCylB.SelectedIndex).ToString();
                else
                    LblTrolleyCylBName.Text = "None";
            }
            else
            {
                CbxTrolleyCylB.BackColor = SystemColors.Window;
                LblTrolleyCylBName.Text = "None";
            }
        }
        private void NymUDTrolleyAutoLockTimeValue_ValueChanged(object sender, EventArgs e)
        {
            if (CbxTrolleyName.SelectedIndex >= 0)
            {
                if (NymUDTrolleyAutoLockTimeValue.Value == (decimal)G.Comm.Trolley.GetLockTime())
                    NymUDTrolleyAutoLockTimeValue.BackColor = SystemColors.Window;
                else
                    NymUDTrolleyAutoLockTimeValue.BackColor = Color.LightPink;
            }
            else
            {
                NymUDTrolleyAutoLockTimeValue.BackColor = SystemColors.Window;
            }
        }

        #endregion

        #region 升降台
        private void CbxLiftsMotorErrDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsMotorErrDI.SelectedIndex == (int)G.Comm.Lifts.GetMotorError((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsMotorErrDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsMotorErrDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsMotorErrDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsMotorErrSensorName.Text = ((EDI_TYPE)CbxLiftsMotorErrDI.SelectedIndex).ToString();
                else
                    LblLiftsMotorErrSensorName.Text = "None";
            }
            else
            {
                CbxLiftsMotorErrDI.BackColor = SystemColors.Window;
                LblLiftsMotorErrSensorName.Text = "None";
            }
        }
        private void CbxLiftsULimBoardDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsULimBoardDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsULimBoardDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsULimBoardDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsULimBoardDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsULimBoardSensorName.Text = ((EDI_TYPE)CbxLiftsULimBoardDI.SelectedIndex).ToString();
                else
                    LblLiftsULimBoardSensorName.Text = "None";
            }
            else
            {
                CbxLiftsULimBoardDI.BackColor = SystemColors.Window;
                LblLiftsULimBoardSensorName.Text = "None";
            }
        }
        private void CbxLiftsInpPlaceBoardDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsInpPlaceBoardDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsInpPlaceBoardDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsInpPlaceBoardDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsInpPlaceBoardDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsInPlaceSensorName.Text = ((EDI_TYPE)CbxLiftsInpPlaceBoardDI.SelectedIndex).ToString();
                else
                    LblLiftsInPlaceSensorName.Text = "None";
            }
            else
            {
                CbxLiftsInpPlaceBoardDI.BackColor = SystemColors.Window;
                LblLiftsInPlaceSensorName.Text = "None";
            }
        }
        private void CbxLiftsHavePalletDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsHavePalletDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsHavePalletDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsHavePalletDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsHavePalletDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsHavePalletSensorName.Text = ((EDI_TYPE)CbxLiftsHavePalletDI.SelectedIndex).ToString();
                else
                    LblLiftsHavePalletSensorName.Text = "None";
            }
            else
            {
                CbxLiftsHavePalletDI.BackColor = SystemColors.Window;
                LblLiftsHavePalletSensorName.Text = "None";
            }
        }
        private void CbxLiftsOnPalletDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsOnPalletDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsOnPalletDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsOnPalletDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsOnPalletDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsOnPalletSensorName.Text = ((EDI_TYPE)CbxLiftsOnPalletDI.SelectedIndex).ToString();
                else
                    LblLiftsOnPalletSensorName.Text = "None";
            }
            else
            {
                CbxLiftsOnPalletDI.BackColor = SystemColors.Window;
                LblLiftsOnPalletSensorName.Text = "None";
            }
        }
        private void CbxLiftsWorkULimDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsWorkULimDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsWorkULimDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsWorkULimDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsWorkULimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsWorkULimSensorName.Text = ((EDI_TYPE)CbxLiftsWorkULimDI.SelectedIndex).ToString();
                else
                    LblLiftsWorkULimSensorName.Text = "None";
            }
            else
            {
                CbxLiftsWorkULimDI.BackColor = SystemColors.Window;
                LblLiftsWorkULimSensorName.Text = "None";
            }
        }
        private void CbxLiftsWorkLLimDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsWorkLLimDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsWorkLLimDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsWorkLLimDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsWorkLLimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsWorkLLimSensorName.Text = ((EDI_TYPE)CbxLiftsWorkLLimDI.SelectedIndex).ToString();
                else
                    LblLiftsWorkLLimSensorName.Text = "None";
            }
            else
            {
                CbxLiftsWorkLLimDI.BackColor = SystemColors.Window;
                LblLiftsWorkLLimSensorName.Text = "None";
            }
        }
        private void CbxLiftsMoveULimDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsMoveULimDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsMoveULimDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsMoveULimDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsMoveULimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsMoveULimSensorName.Text = ((EDI_TYPE)CbxLiftsMoveULimDI.SelectedIndex).ToString();
                else
                    LblLiftsMoveULimSensorName.Text = "None";
            }
            else
            {
                CbxLiftsMoveULimDI.BackColor = SystemColors.Window;
                LblLiftsMoveULimSensorName.Text = "None";
            }
        }
        private void CbxLiftsMoveLLimDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsMoveLLimDI.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsMoveLLimDI.BackColor = SystemColors.Window;
                else
                    CbxLiftsMoveLLimDI.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsMoveLLimDI.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsMoveLLimSensorName.Text = ((EDI_TYPE)CbxLiftsMoveLLimDI.SelectedIndex).ToString();
                else
                    LblLiftsMoveLLimSensorName.Text = "None";
            }
            else
            {
                CbxLiftsMoveLLimDI.BackColor = SystemColors.Window;
                LblLiftsMoveLLimSensorName.Text = "None";
            }
        }
        private void CbxLiftsUp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsUp.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsUp.BackColor = SystemColors.Window;
                else
                    CbxLiftsUp.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxLiftsUp.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblLiftsUpName.Text = ((EDO_TYPE)CbxLiftsUp.SelectedIndex).ToString();
                else
                    LblLiftsUpName.Text = "None";
            }
            else
            {
                CbxLiftsUp.BackColor = SystemColors.Window;
                LblLiftsUpName.Text = "None";
            }
        }
        private void CbxLiftsDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsDown.SelectedIndex == (int)G.Comm.Lifts.GetULim_Board((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsDown.BackColor = SystemColors.Window;
                else
                    CbxLiftsDown.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxLiftsDown.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblLiftsDownName.Text = ((EDO_TYPE)CbxLiftsDown.SelectedIndex).ToString();
                else
                    LblLiftsDownName.Text = "None";
            }
            else
            {
                CbxLiftsDown.BackColor = SystemColors.Window;
                LblLiftsDownName.Text = "None";
            }
        }
        private void CbxLiftsBtnUp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsBtnUp.SelectedIndex == (int)G.Comm.Lifts.GetBtnUp((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsBtnUp.BackColor = SystemColors.Window;
                else
                    CbxLiftsBtnUp.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsBtnUp.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsBtnUpName.Text = ((EDI_TYPE)CbxLiftsBtnUp.SelectedIndex).ToString();
                else
                    LblLiftsBtnUpName.Text = "None";
            }
            else
            {
                CbxLiftsBtnUp.BackColor = SystemColors.Window;
                LblLiftsBtnUpName.Text = "None";
            }
        }
        private void CbxLiftsBtnLEDUp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsBtnLEDUp.SelectedIndex == (int)G.Comm.Lifts.GetBtnUp((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsBtnLEDUp.BackColor = SystemColors.Window;
                else
                    CbxLiftsBtnLEDUp.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxLiftsBtnLEDUp.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblLiftsBtnLEDUpName.Text = ((EDI_TYPE)CbxLiftsBtnLEDUp.SelectedIndex).ToString();
                else
                    LblLiftsBtnLEDUpName.Text = "None";
            }
            else
            {
                CbxLiftsBtnLEDUp.BackColor = SystemColors.Window;
                LblLiftsBtnLEDUpName.Text = "None";
            }
        }
        private void CbxLiftsBtnDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsBtnDown.SelectedIndex == (int)G.Comm.Lifts.GetBtnUp((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsBtnDown.BackColor = SystemColors.Window;
                else
                    CbxLiftsBtnDown.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsBtnDown.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsBtnDownName.Text = ((EDI_TYPE)CbxLiftsBtnDown.SelectedIndex).ToString();
                else
                    LblLiftsBtnDownName.Text = "None";
            }
            else
            {
                CbxLiftsBtnDown.BackColor = SystemColors.Window;
                LblLiftsBtnDownName.Text = "None";
            }
        }
        private void CbxLiftsBtnLEDDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsBtnLEDDown.SelectedIndex == (int)G.Comm.Lifts.GetBtnUp((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsBtnLEDDown.BackColor = SystemColors.Window;
                else
                    CbxLiftsBtnLEDDown.BackColor = Color.LightPink;

                if ((EDO_TYPE)CbxLiftsBtnLEDDown.SelectedIndex != EDO_TYPE.DO_COUNT)
                    LblLiftsBtnLEDDownName.Text = ((EDI_TYPE)CbxLiftsBtnLEDDown.SelectedIndex).ToString();
                else
                    LblLiftsBtnLEDDownName.Text = "None";
            }
            else
            {
                CbxLiftsBtnLEDDown.BackColor = SystemColors.Window;
                LblLiftsBtnLEDDownName.Text = "None";
            }
        }
        #endregion

        #region 震動送料機
        private void CbxVibBowEmptySensor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void CbxVibBowPower_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void NumUDVibBowStartDelayValue_ValueChanged(object sender, EventArgs e)
        {

        }
        private void NumUDVibBowStopDelayValue_ValueChanged(object sender, EventArgs e)
        {

        }
        private void NumUDVibBowAlarmTimeValue_ValueChanged(object sender, EventArgs e)
        {

        }
        #endregion

        private void CbxLiftsSafeLockDI_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsSafeLockDI_1.SelectedIndex == (int)G.Comm.Lifts.GetSafeLock1((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsSafeLockDI_1.BackColor = SystemColors.Window;
                else
                    CbxLiftsSafeLockDI_1.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsSafeLockDI_1.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsSafeLockSensorName_1.Text = ((EDI_TYPE)CbxLiftsSafeLockDI_1.SelectedIndex).ToString();
                else
                    LblLiftsSafeLockSensorName_1.Text = "None";
            }
            else
            {
                CbxLiftsSafeLockDI_1.BackColor = SystemColors.Window;
                LblLiftsSafeLockSensorName_1.Text = "None";
            }
        }

        private void CbxLiftsSafeLockDI_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbxLiftsName.SelectedIndex >= 0)
            {
                if (CbxLiftsSafeLockDI_2.SelectedIndex == (int)G.Comm.Lifts.GetSafeLock2((ELifts)CbxLiftsName.SelectedIndex))
                    CbxLiftsSafeLockDI_2.BackColor = SystemColors.Window;
                else
                    CbxLiftsSafeLockDI_2.BackColor = Color.LightPink;

                if ((EDI_TYPE)CbxLiftsSafeLockDI_2.SelectedIndex != EDI_TYPE.DI_COUNT)
                    LblLiftsSafeLockSensorName_2.Text = ((EDI_TYPE)CbxLiftsSafeLockDI_2.SelectedIndex).ToString();
                else
                    LblLiftsSafeLockSensorName_2.Text = "None";
            }
            else
            {
                CbxLiftsSafeLockDI_2.BackColor = SystemColors.Window;
                LblLiftsSafeLockSensorName_2.Text = "None";
            }
        }
    }
}
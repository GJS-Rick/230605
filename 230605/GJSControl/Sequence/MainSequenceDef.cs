using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace nsSequence
{
    public class MainSequenceDef : IDisposable
    {
        public enum ESequence
        {
            Robot,
            Lift,
            TakeCylinder,
            PutCylinder,
            Drills,
            InputCV,
            OutputCV,

            Count
        }

        readonly private HandshakeDef _Handshake;
        readonly private TimeoutDef _Timeout;

        private ERunStatus _Status;
        private ERunStatus _TempStatus;
        private ERunStatus _LastStatus;

        private int _MainPressureTickCount;
        private int[] _TrolleyTickCount;

        public BaseSequenceDef[] _Sequence;

        private StackLightDef _StackLight;
        public MainSequenceDef()
        {
            _Handshake = new HandshakeDef(EHandshake.Count, EDataBool.Count, EDataInt.Count, EDataDouble.Count);
            _Status = ERunStatus.Stop;
            _TempStatus = ERunStatus.None;
            _StackLight = new StackLightDef();

            #region New Sequence
            _Sequence = new BaseSequenceDef[(int)ESequence.Count];
            _Sequence[(int)ESequence.Robot] = new RobotDef(_Handshake);
            _Sequence[(int)ESequence.Lift] = new LiftDef(_Handshake);
            _Sequence[(int)ESequence.TakeCylinder] = new TakeCylinderDef(_Handshake);
            _Sequence[(int)ESequence.PutCylinder] = new PutCylinderDef(_Handshake);
            _Sequence[(int)ESequence.Drills] = new DrillsDef(_Handshake);
            _Sequence[(int)ESequence.InputCV] = new InputCVDef(_Handshake);
            _Sequence[(int)ESequence.OutputCV] = new OutputCVDef(_Handshake);

            //_Sequence[(int)ESequence.Teach] = new TeachDef(cCommonManager, cVisionManager, _fileStream, cUIManager, _handshake);
            #endregion

            _Timeout = new TimeoutDef(G.UI.frmMainFm, G.Comm.GetSystemDirPath(), _Sequence);

            _TrolleyTickCount = new int[(int)ETrolley.Count];
            for (int i = 0; i < _TrolleyTickCount.Length; i++)
                _TrolleyTickCount[i] = Environment.TickCount;

            ResetParameter();
        }

        public string[][] GetAllSequenceStep()
        {
            List<string[]> lSeq = new List<string[]>();
            try
            {
                foreach (BaseSequenceDef Seq in _Sequence)
                {
                    string[] data;

                    switch (_Status)
                    {
                        case ERunStatus.Initial:
                            data = new string[] { Seq.ToString(), Seq.GetInitialStep().ToString(), Seq.GetStep().ToString() };
                            break;
                        case ERunStatus.Auto:
                            data = new string[] { Seq.ToString(), Seq.GetAutoStep().ToString(), Seq.GetStep().ToString() };
                            break;
                        case ERunStatus.GoStandby:
                            data = new string[] { Seq.ToString(), Seq.GetStandbyStep().ToString(), Seq.GetStep().ToString() };
                            break;
                        default:
                            data = new string[] { Seq.ToString(), Seq.GetAutoStep().ToString(), Seq.GetStep().ToString() };
                            break;
                    }

                    lSeq.Add(data);
                }
            }
            catch (Exception) { }

            return lSeq.ToArray();
        }

        private void ResetParameter()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                _Sequence[i].Reset_Parameters();
        }
        private void InitialExecuteSequence()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                _Sequence[i].InitialExecute();
        }
        private void AutoExecuteSequence()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                _Sequence[i].AutoExecute();
        }
        private void GoStandbyExecuteSequence()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                _Sequence[i].StandbyExecute();
        }
        private void StopFunction()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                _Sequence[i].Stop();

            for (int i = 0; i < (int)EDataBool.Count; i++)
                _Handshake.SetDataBool((EDataBool)(i), false);


            ResetParameter();
        }

        public ERunStatus GetStatus() { return _Status; }
        public void UserSetStatus(ERunStatus eStatus)
        {
            switch (eStatus)
            {
                case ERunStatus.Alarm:
                    if (_Status != ERunStatus.Alarm)
                        RunStatusSwitch(eStatus);
                    break;

                case ERunStatus.Auto:
                    if (_Status == ERunStatus.Stop)
                        RunStatusSwitch(eStatus);
                    if (_Status == ERunStatus.Pause)
                    {
                        if (G.Comm.Door.DoorSafty(out string sDoorPos))
                            RunStatusSwitch(eStatus);
                        else
                            AlarmTextDisplay.Add((int)AlarmCode.Alarm_SafetyDoorOpen, AlarmType.Warning, sDoorPos);
                    }
                    break;

                case ERunStatus.Pause:
                    if (_Status == ERunStatus.Auto || _Status == ERunStatus.End)
                        RunStatusSwitch(eStatus);
                    break;

                case ERunStatus.GoStandby:
                    if (_Status == ERunStatus.Stop)
                        RunStatusSwitch(eStatus);
                    break;

                case ERunStatus.Stop:
                    RunStatusSwitch(eStatus);
                    G.Comm.Cycle.Stop();
                    break;

                case ERunStatus.Teach:
                    if (_Status == ERunStatus.Stop)
                        RunStatusSwitch(eStatus);
                    break;

                case ERunStatus.End:
                    if (_Status == ERunStatus.Auto)
                        RunStatusSwitch(eStatus);
                    break;

                case ERunStatus.Initial:
                    if (_Status == ERunStatus.Stop)
                        RunStatusSwitch(eStatus);
                    break;

                case ERunStatus.Manual:
                    if (_Status == ERunStatus.Stop)
                        RunStatusSwitch(eStatus);
                    break;
            }
        }
        public void Run()
        {
            try
            {
                AllStatusExecute();
                Execute();
                Thread.Sleep(1);
            }
            catch (Exception ex)
            {
                string msg = "MainSequence failed!: \n" + ex.ToString();
                LogDef.Add(
                    ELogFileName.Alarm,
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    msg);

                RunStatusSwitch(ERunStatus.Alarm);

                AlarmTextDisplay.Add(
                    (int)AlarmCode.Alarm,
                    AlarmType.Msg,
                    ex.Message);
            }
        }

        public FmTimeoutMsg GetTimeoutForm()
        {
            return _Timeout.GetForm();
        }

        public bool TimeoutFormIsExsit()
        {
            return _Timeout.GetForm() != null
                && !_Timeout.GetForm().IsDisposed;
        }

        public void SetTimeoutResult(DialogResult dialogResult)
        {
            _Timeout.SetResult(dialogResult);
        }

        private void AllStatusExecute()
        {
            BtnEMOPush();
            AirCheck();
            BtnResetPush();
            DoorAutoLock();
            DoorSafty();
            TrolleyAutoLock();
            TrolleyInPlace();
            DetectErr();

            G.Comm.RenewState();
        }

        #region BasicAction
        /// <summary>EMO按下</summary>
        private void BtnEMOPush()
        {
            switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.EmgStop, false))
            {
                case EDIO_SingleEdge.RisingEdge:
                    break;
                case EDIO_SingleEdge.FallingEdge:
                    break;
                case EDIO_SingleEdge.On:
                    for (int i = 0; i < _Sequence.Length; i++)
                        _Sequence[i].ResetInitialComplete();

                    AllDoMotorStop();
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_EMOPressed, AlarmType.Alarm);
                    RunStatusSwitch(ERunStatus.Alarm);
                    break;
                case EDIO_SingleEdge.Off:
                    break;
                default:
                    break;
            }
        }
        /// <summary>異常訊息解除</summary>
        private void BtnResetPush()
        {
            switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.BtnReset))
            {
                case EDIO_SingleEdge.RisingEdge:
                    G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnReset, true);
                    break;
                case EDIO_SingleEdge.FallingEdge:
                    G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnReset, false);
                    if (_Status == ERunStatus.Alarm)
                        RunStatusSwitch(ERunStatus.Stop);
                    G.Comm.AlarmTextDisplay.ClearFirstAlarmText();
                    break;
                default:
                    break;
            }
        }
        /// <summary>啟動鈕按下</summary>
        private void BtnStartPush()
        {
            switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.BtnStart))
            {
                case EDIO_SingleEdge.RisingEdge:
                    G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnStart, true);
                    break;
                case EDIO_SingleEdge.FallingEdge:
                    G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnStart, false);
                    UserSetStatus(ERunStatus.Auto);
                    break;
                default:
                    break;
            }
        }
        /// <summary>停止鈕按下</summary>
        private void BtnStopPush()
        {
            switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.BtnStop))
            {
                case EDIO_SingleEdge.RisingEdge:
                    G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnStop, true);
                    break;
                case EDIO_SingleEdge.FallingEdge:
                    G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnStop, false);
                    UserSetStatus(ERunStatus.Stop);
                    break;
                default:
                    break;
            }
        }
        /// <summary>入氣壓力偵測</summary>
        private void AirCheck()
        {
            if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.AirPressure_Main, true))
            {
                if (Environment.TickCount - _MainPressureTickCount > 500)
                {
                    //運轉中氣壓不足=>暫停
                    if (_Status == ERunStatus.Auto || _Status == ERunStatus.Pause)
                        RunStatusSwitch(ERunStatus.Pause);
                    else
                        RunStatusSwitch(ERunStatus.Stop);

                    AlarmTextDisplay.Add(
                        (int)AlarmCode.Alarm_AirPressureNotEnough,
                        AlarmType.Warning);
                }
            }
            else
                _MainPressureTickCount = Environment.TickCount;
        }
        /// <summary>喇叭靜音解除並更新主介面靜音圖示</summary>
        private void ResetVolumeMute()
        {
            G.Comm.VolumeMute = false;
            G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.ChangBtnVolumeImg(); }));
        }
        private void BtnDoorPush()
        {
            //switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.BtnFrontDoor))
            //{
            //    case EDIO_SingleEdge.RisingEdge:
            //        G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnDoor, true);
            //        DoorUnlock();
            //        break;
            //    case EDIO_SingleEdge.FallingEdge:
            //        G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnDoor, false);
            //        break;
            //    default:
            //        break;
            //}
            //switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.BtnBackDoor))
            //{
            //    case EDIO_SingleEdge.RisingEdge:
            //        G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnDoor, true);
            //        DoorUnlock();
            //        break;
            //    case EDIO_SingleEdge.FallingEdge:
            //        G.Comm.IOCtrl.SetDO(EDO_TYPE.LED_BtnDoor, false);
            //        break;
            //    default:
            //        break;
            //}
        }
        private void BtnTrolleyPush()
        {
            //switch (G.Common.IOCtrl.GetDIEdge(EDI_TYPE.BtnTrolley1))
            //{
            //    case EDIO_SingleEdge.RisingEdge:
            //        G.Common.IOCtrl.SetDO(EDO_TYPE.LED_BtnTrolley1, true);

            //        if (G.Common.Trolley.IsLock(ETrolley.Trolley1))
            //            G.Common.Trolley.Unlock(ETrolley.Trolley1);
            //        else
            //            G.Common.Trolley.Lock(ETrolley.Trolley1);
            //        break;
            //    case EDIO_SingleEdge.FallingEdge:
            //        G.Common.IOCtrl.SetDO(EDO_TYPE.LED_BtnTrolley1, false);
            //        break;
            //    default:
            //        break;
            //}
            //switch (G.Common.IOCtrl.GetDIEdge(EDI_TYPE.BtnTrolley2))
            //{
            //    case EDIO_SingleEdge.RisingEdge:
            //        G.Common.IOCtrl.SetDO(EDO_TYPE.LED_BtnTrolley2, true);

            //        if (G.Common.Trolley.IsLock(ETrolley.Trolley2))
            //            G.Common.Trolley.Unlock(ETrolley.Trolley2);
            //        else
            //            G.Common.Trolley.Lock(ETrolley.Trolley2);
            //        break;
            //    case EDIO_SingleEdge.FallingEdge:
            //        G.Common.IOCtrl.SetDO(EDO_TYPE.LED_BtnTrolley2, false);
            //        break;
            //    default:
            //        break;
            //}
        }
        private void BtnVacPush()
        {
            //switch (G.Comm.IOCtrl.GetDIEdge(EDI_TYPE.BtnVac))
            //{
            //    case EDIO_SingleEdge.RisingEdge:
            //        G.Comm.Vac.BtnOff_Start(EVacSuckerName.SCARA);
            //        break;
            //    default:
            //        G.Comm.Vac.BtnOff_Check(EVacSuckerName.SCARA);
            //        break;
            //}
        }

        #region 過載
        private void DetectErr()
        {
            string _MsgBuf = "";
            bool AxisErr = false;

            EDI_TYPE[] AxisErrDI = new EDI_TYPE[]
            {
                //EDI_TYPE.AxisErr_Import,
                //EDI_TYPE.AxisErr_ExportCV,
                //EDI_TYPE.AxisErr_PinA1,
                //EDI_TYPE.AxisErr_PinA2,
                //EDI_TYPE.AxisErr_SDR1,
                //EDI_TYPE.AxisErr_SDR2,
                //EDI_TYPE.AxisErr_SDR3,
                //EDI_TYPE.AxisErr_SDR4
            };

            for (int i = 0; i < AxisErrDI.Length; i++)
            {
                if (G.Comm.IOCtrl.GetDI(AxisErrDI[i], false))
                {
                    AxisErr = true;
                    if (_MsgBuf != "")
                        _MsgBuf += ",";

                    _MsgBuf += G.Comm.IOCtrl.GetDIName(AxisErrDI[i]);
                }
            }

            if (AxisErr)
                AlarmTextDisplay.Add((int)AlarmCode.Alarm_AxisError, AlarmType.Alarm, _MsgBuf);
        }
        #endregion

        #region 門檢
        /// <summary>門磁檢測</summary>
        private void DoorSafty()
        {
            if (!G.Comm.Door.DoorSafty(out string sDoorPos))
            {


                if (_Status == ERunStatus.Idle ||
                    _Status == ERunStatus.Teach ||
                    _Status == ERunStatus.Pause ||
                    _Status == ERunStatus.Stop ||
                    _Status == ERunStatus.Manual)
                {
                    if (!G.Comm.Door.GetOnlyAutoAlarm())
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_SafetyDoorOpen, AlarmType.Warning, sDoorPos);
                }
                else
                {
                    RunStatusSwitch(ERunStatus.Alarm);
                    AlarmTextDisplay.Add((int)AlarmCode.Alarm_SafetyDoorOpen, AlarmType.Alarm, sDoorPos);
                }
            }
        }
        /// <summary>自動上鎖</summary>
        private void DoorAutoLock(bool eLockAll = false)
        {
            if (eLockAll)
                G.Comm.Door.Lock();
            else
                G.Comm.Door.AutoLock();
        }
        /// <summary>解門鎖</summary>
        private void DoorUnlock(EDoorDir eDoorDir = EDoorDir.All) { G.Comm.Door.Unlock(eDoorDir); }
        #endregion

        #region 台車Trolley
        /// <summary>台車到位檢測</summary>
        private void TrolleyInPlace()
        {
            for (int i = 0; i < (int)ETrolley.Count; i++)
            {
                if (!G.Comm.Trolley.IsInPlace((ETrolley)i))
                {
                    if (_Status == ERunStatus.Idle ||
                        _Status == ERunStatus.Teach ||
                        _Status == ERunStatus.Pause ||
                        _Status == ERunStatus.Stop ||
                        _Status == ERunStatus.Manual)
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_TrolleyLeaves, AlarmType.Warning, ((ETrolley)i).ToString());
                    }
                    else
                    {
                        RunStatusSwitch(ERunStatus.Alarm);
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_TrolleyLeaves, AlarmType.Alarm, ((ETrolley)i).ToString());
                    }
                }
            }
        }
        /// <summary>台車自動上鎖</summary>
        private void TrolleyAutoLock(bool eLockAll = false)
        {
            if (eLockAll)
            {
                for (int i = 0; i < (int)ETrolley.Count; i++)
                    G.Comm.Trolley.Lock((ETrolley)i);
            }
            else
                G.Comm.Trolley.AutoLock();
        }
        #endregion
        #endregion

        /// <summary>DO控制的所有馬達停止</summary>
        private void AllDoMotorStop()
        {
            //所有軸停止
            for (int i = 0; i < (int)EAXIS_NAME.Count; i++)
                G.Comm.MtnCtrl.SdStop((EAXIS_NAME)i, false);

            //SCARA停止
            if (G.Comm.Scara != null)
                G.Comm.Scara.StopAll();
        }

        #region 自訂動作與偵測

        #endregion

        public bool IsAllSequenceInitialComplete()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                if (!_Sequence[i].IsInitialComplete())
                    return false;

            return true;
        }
        public bool IsAllSequenceStandbyDone()
        {
            for (int i = 0; i < _Sequence.Length; i++)
                if (!_Sequence[i].IsStandbyDone())
                    return false;

            return true;
        }

        private void RunStatusFirstRun()
        {
            switch (_Status)
            {
                case ERunStatus.Alarm:
                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Alarm); }));
                    StopFunction();

                    G.Comm.DataTransfer.SendStop();
                    break;

                case ERunStatus.Stop:
                    G.UI.frmMainFm.Invoke(new Action(() => { G.UI.frmMainFm.BtnAutoColorChange(SystemColors.ControlLight); }));
                    if (TimeoutFormIsExsit())
                    {
                        SetTimeoutResult(DialogResult.Cancel);
                    }

                    if (IsAllSequenceInitialComplete())
                        G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Stop); }));
                    else
                        G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.BeforeInitial); }));
                    StopFunction();
                    for (int i = 0; i < _Sequence.Length; i++)
                        _Sequence[i].SetPause(false);

                    G.Comm.DataTransfer.SendStop();
                    break;

                case ERunStatus.Initial:
                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Initial); }));
                    ResetVolumeMute();

                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() != AlarmType.None)
                    {
                        RunStatusSwitch(ERunStatus.Stop);
                        break;
                    }

                    _Handshake.Reset();
                    DoorAutoLock(true);
                    TrolleyAutoLock(true);

                    for (int i = 0; i < _Sequence.Length; i++)
                        _Sequence[i].InitialStart();
                    break;

                case ERunStatus.Auto:
                    if (_LastStatus != ERunStatus.Pause)
                    {
                        _Handshake.Reset();
                        if (!IsAllSequenceInitialComplete())
                        {
                            RunStatusSwitch(ERunStatus.Initial);
                            break;
                        }

                        DoorAutoLock(true);
                        TrolleyAutoLock(true);

                        for (int i = 0; i < _Sequence.Length; i++)
                            _Sequence[i].AutoStart();
                    }
                    G.Comm.DataTransfer.SendRun();
                    for (int i = 0; i < _Sequence.Length; i++)
                        _Sequence[i].SetPause(false);
                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Auto); }));
                    ResetVolumeMute();

                    break;

                case ERunStatus.GoStandby:

                    if (!IsAllSequenceInitialComplete())
                    {
                        RunStatusSwitch(ERunStatus.Stop);
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_NoInital, AlarmType.Alarm);
                        break;
                    }

                    _Handshake.Reset();
                    DoorAutoLock(true);
                    TrolleyAutoLock(true);

                    for (int i = 0; i < _Sequence.Length; i++)
                        _Sequence[i].StandbyStart();

                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.GoStandby); }));
                    ResetVolumeMute();

                    break;

                case ERunStatus.Teach:
                    if (!IsAllSequenceInitialComplete())
                    {
                        RunStatusSwitch(ERunStatus.Stop);
                        AlarmTextDisplay.Add((int)AlarmCode.Alarm_NoInital, AlarmType.Alarm);
                        break;
                    }

                    _Handshake.Reset();

                    //((TeachDef)_Sequence[(int)ESequence.Teach]).TeachStart();
                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Teach); }));
                    ResetVolumeMute();
                    break;


                case ERunStatus.Manual:
                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Manual); }));
                    break;

                case ERunStatus.Pause:
                    if (_LastStatus != ERunStatus.Auto)
                        break;

                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.Pause); }));
                    for (int i = 0; i < _Sequence.Length; i++)
                        _Sequence[i].SetPause(true);
                    break;

                case ERunStatus.End:
                    G.UI.frmMainFm.Invoke(new Action(() => { G.Comm.Login.UIEnable(G.UI.frmMainFm, ERunStatus.End); }));
                    ResetVolumeMute();
                    break;
            }
        }
        private void RunStatusKeepRun()
        {
            switch (_Status)
            {
                case ERunStatus.Alarm:
                    BtnDoorPush();
                    BtnTrolleyPush();

                    break;

                case ERunStatus.Stop:
                    BtnDoorPush();
                    BtnTrolleyPush();
                    BtnVacPush();
                    break;

                case ERunStatus.Initial:

                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm ||
                        _Timeout.Alarm())
                    {
                        RunStatusSwitch(ERunStatus.Alarm);
                        break;
                    }
                    if (IsAllSequenceInitialComplete())
                    {
                        if (_LastStatus == ERunStatus.Auto)
                            RunStatusSwitch(ERunStatus.Auto);
                        else
                            RunStatusSwitch(ERunStatus.Stop);
                    }
                    InitialExecuteSequence();

                    break;

                case ERunStatus.Auto:

                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm ||
                        _Timeout.Alarm())
                    {
                        RunStatusSwitch(ERunStatus.Alarm);

                        break;
                    }

                    if (TimeoutFormIsExsit())
                    {
                        //Timeoue時檢查

                    }
                    else
                    {
                        //Auto正常時檢查
                    }


                    AutoExecuteSequence();

                    break;

                case ERunStatus.GoStandby:
                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm ||
                        _Timeout.Alarm())
                    {
                        RunStatusSwitch(ERunStatus.Alarm);
                        break;
                    }



                    if (IsAllSequenceStandbyDone())
                        RunStatusSwitch(ERunStatus.Stop);

                    GoStandbyExecuteSequence();

                    break;

                case ERunStatus.Teach:
                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm
                        || _Timeout.Alarm())
                    {
                        RunStatusSwitch(ERunStatus.Alarm);
                        break;
                    }

                    BtnDoorPush();
                    break;


                case ERunStatus.Manual:

                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm
                        || _Timeout.Alarm())
                    {
                        RunStatusSwitch(ERunStatus.Alarm);
                        break;
                    }

                    //((ManaulCVDef)_Sequence[(int)ESequence.ManualCV]).ManualExecute();
                    BtnDoorPush();
                    BtnTrolleyPush();
                    break;

                case ERunStatus.Pause:


                    AutoExecuteSequence();
                    BtnDoorPush();
                    break;

                case ERunStatus.End:
                    if (G.Comm.AlarmTextDisplay.GetFirstAlarmType() == AlarmType.Alarm
                        || _Timeout.Alarm())
                    {
                        RunStatusSwitch(ERunStatus.Alarm);
                        break;
                    }

                    AutoExecuteSequence();

                    break;
            }
        }

        private void RunStatusSwitch(ERunStatus NewStatus)
        {
            if (NewStatus != _Status)
                _LastStatus = _Status;
            _Status = NewStatus;
        }
        private bool RunStatusOnFirst()
        {
            if (_Status != _TempStatus)
            {
                _TempStatus = _Status;
                return true;
            }

            return false;
        }

        private void Execute()
        {
            _StackLight.SetStatus(_Status, G.Comm.AlarmTextDisplay.GetFirstAlarmType(), _Timeout.Showing());
            if (RunStatusOnFirst())
                RunStatusFirstRun();
            else
                RunStatusKeepRun();
        }

        public void Dispose()
        {

            _StackLight.Dispose();

            for (int i = 0; i < _Sequence.Length; i++)
                _Sequence[i].Dispose();
            _Timeout.Dispose();
        }
    }
}
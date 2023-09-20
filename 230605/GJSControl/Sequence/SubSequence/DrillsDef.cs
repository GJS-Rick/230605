using CommonLibrary;
using System;

namespace nsSequence
{
    public class DrillsDef : BaseSequenceDef
    {
        public enum EMoveType
        {

            Init_GoStandby,
            Align,
            Drilling,

            None
        }

        public enum EInitial
        {
            Idle,

            AutoInit,

            Done,
            None,
        }



        public enum EStandby
        {
            Idle,
            AutoInit,

            Done,
            None,
        }

        public enum EAuto
        {
            Idle,
            AutoInit,
            WaitPanelPutDone,
            Align,
            Drilling,
            WaitPanelTakeaway,
            GoBackStandby,

            Done,
            None,
        }

        public enum EMove
        {
            Idle,
            GoPEL,
            StopMotor,
            LeavePEL,
            GoPELAgain,
            StopDelay500ms,
            ResetOrg,
            GoStandby,

            Align,
            Align_Delay300ms,
            CoverPress,

            DrillingUp,
            Drilling_Delay,
            DrillingDown,
            CoverUnpress,
            GoOpenPosition,






            Done,
            None
        }

        public DrillsDef(
                 HandshakeDef Handshake) :
             base(
                 Handshake,
                 EMoveType.None,
                 EHandshake.Count,
                 EMove.Idle, EMove.None, EMove.Done,
                 EInitial.Idle, EInitial.None, EInitial.Done,
                 EAuto.Idle, EAuto.None, EAuto.Done,
                 EStandby.Idle, EStandby.None, EStandby.Done)
        {
            Reset_Parameters();
        }


        public override void Reset_Parameters()
        {
            base.Reset_Parameters();
            _MoveType = EMoveType.None;

            _MoveStep.SetStep(EMove.None);
            _MoveStep.SetPreStep(EMove.Done);
        }

        public override void Dispose()
        {

        }

        public override void Stop()
        {
            _MoveStep.SetStep(EMove.None);
            _MoveStep.SetPreStep(EMove.Idle);

            G.Comm.Scara.StopAll();
        }

        public override void StandbyExecute()
        {
            switch ((EStandby)_StandbyStep.GetStep())
            {
                case EStandby.Idle:
                    {
                        _StandbyStep.SetStep(EStandby.AutoInit);
                    }
                    break;

                case EStandby.AutoInit:
                    if (_StandbyStep.FirstRun())
                    {
                        Move(EMoveType.Init_GoStandby);
                    }
                    if (MoveDone())
                    {
                        _StandbyStep.SetStep(EStandby.Done);
                    }
                    break;
            }

            Execute();
        }

        public override void InitialExecute()
        {
            switch ((EInitial)_InitialStep.GetStep())
            {
                case EInitial.Idle:
                    {
                        _InitialStep.SetStep(EInitial.AutoInit);
                    }
                    break;

                case EInitial.AutoInit:
                    if (_InitialStep.FirstRun())
                    {
                        Move(EMoveType.Init_GoStandby);
                    }
                    if (MoveDone())
                    {
                        _InitialComplete = true;
                        _InitialStep.SetStep(EInitial.Done);
                    }
                    break;
            }

            Execute();
        }

        public override void AutoExecute()
        {
            if (_Pause && _MoveStep.GetStep() != _MoveStep.GetPreStep())
                return;

            switch ((EAuto)_AutoStep.GetStep())
            {
                case EAuto.Idle:
                    {
                        _AutoStep.SetStep(EAuto.AutoInit);
                    }
                    break;

                case EAuto.AutoInit:
                    if (_AutoStep.FirstRun())
                    {
                     
                        Move(EMoveType.Init_GoStandby);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitPanelPutDone);
                    }
                    break;

                case EAuto.WaitPanelPutDone:
                    if (_AutoStep.FirstRun()) { }
                    if(_Handshake.IsOn(EHandshake.TakeCylinder_Drills_PutDone))
                    
                    {
                        _AutoStep.SetStep(EAuto.Align);
                    }
                    break;

                case EAuto.Align:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.Align);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.Drilling);
                    }
                    break;

                case EAuto.Drilling:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.Drilling);
                    }
                    if (MoveDone())
                    {
                        _Handshake.SetOn(EHandshake.Drills_PutCylinder_CanTake);
                        _AutoStep.SetStep(EAuto.WaitPanelTakeaway);
                    }
                    break;

                case EAuto.WaitPanelTakeaway:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.PutCylinder_Drill_TakeDone))
                    {
                        _AutoStep.SetStep(EAuto.WaitPanelPutDone);
                    }
                    break;
                    

            }

            Execute();
        }

        protected override void MoveSeqence()
        {
            switch ((EMove)_MoveStep.GetStep())
            {
                case EMove.Idle:
                    switch (_MoveType)
                    {
                        case EMoveType.Init_GoStandby:
                            _MoveStep.SetStep(EMove.GoPEL);
                            break;

                        case EMoveType.Align:
                             _MoveStep.SetStep(EMove.Align);
                            break;

                        case EMoveType.Drilling:
                            _MoveStep.SetStep(EMove.DrillingUp);
                            break;
                    }
                    break;

                case EMove.GoPEL:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.DrillsAlign_Servo, true, ESPEED_TYPE.Low);
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.DrillsAlign_Step, true, ESPEED_TYPE.Low);

                    }
                    if (_MoveStep.Success(10000,
                            G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.DrillsAlign_Servo, true), "AlignServo Go PEL Timeout, 對位伺服回正極限逾時",
                            G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.DrillsAlign_Step, true), "AlignStep Go PEL Timeout, 對位步近回正極限逾時"))
                    {
                        _MoveStep.SetStep(EMove.StopMotor);
                    }
                    break;

                case EMove.StopMotor:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.SdStop(EAXIS_NAME.DrillsAlign_Servo, true);
                        G.Comm.MtnCtrl.SdStop(EAXIS_NAME.DrillsAlign_Step, true);
                    }
                    if (_MoveStep.Success(1000,
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.DrillsAlign_Servo, true), "AlignServo Not Stop, 對位伺服未停止",
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.DrillsAlign_Step, true), "AlignStep Not Stop, 對位步近未停止"))
                    {
                        _MoveStep.SetStep(EMove.LeavePEL);
                    }
                    break;


                case EMove.LeavePEL:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.RelMv(EAXIS_NAME.DrillsAlign_Servo, -25, ESPEED_TYPE.Low);
                        G.Comm.MtnCtrl.RelMv(EAXIS_NAME.DrillsAlign_Step, -25, ESPEED_TYPE.Low);
                    }
                    if (_MoveStep.Success(5000,
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.DrillsAlign_Servo, true), "AlignServo Not Stop, 對位伺服未停止",
                           !G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.DrillsAlign_Servo, true), "AlignServo Not Leave LSP , 對位伺服未離開正極限",
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.DrillsAlign_Step, true), "AlignStep Not Stop, 對位步近未停止",
                           !G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.DrillsAlign_Step, true), "AlignStep Not Leave LSP , 對位步近未離開正極限"))

                    {
                        _MoveStep.SetStep(EMove.GoPELAgain);
                    }
                    break;

                case EMove.GoPELAgain:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.DrillsAlign_Servo, true, ESPEED_TYPE.Home);
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.DrillsAlign_Step, true, ESPEED_TYPE.Home);
                    }
                    if (_MoveStep.Success(10000,
                            G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.DrillsAlign_Servo, true), "AlignServo Go PEL Timeout, 對位伺服回正極限逾時",
                            G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.DrillsAlign_Step, true), "AlignStep Go PEL Timeout, 對位步近回正極限逾時"))
                    {
                        _MoveStep.SetStep(EMove.StopDelay500ms);
                    }
                    break;

                case EMove.StopDelay500ms:
                    if (_MoveStep.FirstRun())
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 500)
                    {
                        _MoveStep.SetStep(EMove.ResetOrg);
                    }

                    break;

                case EMove.ResetOrg:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.HmMv(EAXIS_NAME.DrillsAlign_Servo, true);
                        G.Comm.MtnCtrl.SetPos(EAXIS_NAME.DrillsAlign_Servo, 0);

                        G.Comm.MtnCtrl.HmMv(EAXIS_NAME.DrillsAlign_Step, true);
                        G.Comm.MtnCtrl.SetPos(EAXIS_NAME.DrillsAlign_Step, 0);
                    }

                    _MoveStep.SetStep(EMove.GoStandby);

                    break;

                case EMove.GoStandby:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.DrillsAlign_Servo_Open);
                        G.Comm.MotorPosCollection.Move(EMotorPos.DrillsAlign_Step_Open);
                    }
                    if (_MoveStep.Success(10000,
                      G.Comm.MotorPosCollection.Stop(EMotorPos.DrillsAlign_Servo_Open, out string AlignServoStandbyErr), AlignServoStandbyErr + "對位伺服至等待點未停止",
                      G.Comm.MotorPosCollection.Stop(EMotorPos.DrillsAlign_Step_Open, out string AlignStepStandbyErr), AlignStepStandbyErr + "對位步進至等待點未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;


                case EMove.Align:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.DrillsAlign_Servo_Close);
                        G.Comm.MotorPosCollection.Move(EMotorPos.DrillsAlign_Step_Close);

                    }
                    if (_MoveStep.Success(10000,
                      G.Comm.MotorPosCollection.Stop(EMotorPos.DrillsAlign_Servo_Close, out string AlignServoCloseErr), AlignServoCloseErr + "對位伺服至對位點未停止",
                      G.Comm.MotorPosCollection.Stop(EMotorPos.DrillsAlign_Step_Close, out string AlignStepCloseErr), AlignStepCloseErr + "對位步進至對位點未停止"))
                    {
                        _MoveStep.SetStep(EMove.Align_Delay300ms);

                    }
                    break;

                case EMove.Align_Delay300ms:
                    if (_MoveStep.FirstRun())
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 300)
                    {
                        _MoveStep.SetStep(EMove.CoverPress);
                    }

                    break;


                    case EMove.CoverPress:
                    if (_MoveStep.FirstRun()) 
                        { 
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.ClampCylinderOn,true);
                        }

                    if (_MoveStep.Success(3000,
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A1ClampCylinder_Top, true), "A1 Cover Not at Position , A1蓋板未到位(4X00) ",
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A2ClampCylinder_Top, true), "A2 Cover Not at Position , A2蓋板未到位(4X06)"))

                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;

                case EMove.DrillingUp:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.DrillsCylinderOn, true);
                    }

                    if (_MoveStep.Success(3000,
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A1DrillsCylinder_Top, true), "A1 DrillCylinder Not at Position(Up) , A1主軸上升汽缸未到位(4X03)",
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A2DrillsCylinder_Top, true), "A2 DrillCylinder Not at Position(Up) , A2主軸上升汽缸未到位(4X09)"))

                    {
                        _MoveStep.SetStep(EMove.Drilling_Delay);
                    }
                    break;


                    case EMove.Drilling_Delay:
                    if (_MoveStep.FirstRun())
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 300)
                    {
                        _MoveStep.SetStep(EMove.DrillingDown);
                    }

                    break;

                case EMove.DrillingDown:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.DrillsCylinderOn, false);
                    }

                    if (_MoveStep.Success(3000,
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A1DrillsCylinder_Down, true), "A1 DrillCylinder Not at Position(Down) , A1主軸下降汽缸未到位(4X04)",
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A2DrillsCylinder_Down, true), "A2 DrillCylinder Not at Position(Down) , A2主軸下降汽缸未到位(4X10)"))

                    {
                        _MoveStep.SetStep(EMove.CoverUnpress);
                    }
                    break;

                case EMove.CoverUnpress:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.ClampCylinderOn, false);
                    }

                    if (_MoveStep.Success(3000,
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A1ClampCylinder_Down, true), "A1 Cover Not at Position , A1蓋板未到位(4X01) ",
                     G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_A2ClampCylinder_Down, true), "A2 Cover Not at Position , A2蓋板未到位(4X07)"))

                    {
                        _MoveStep.SetStep(EMove.GoOpenPosition);
                    }
                    break;

                case EMove.GoOpenPosition:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.DrillsAlign_Servo_Open);
                        G.Comm.MotorPosCollection.Move(EMotorPos.DrillsAlign_Step_Open);
                    }
                    if (_MoveStep.Success(10000,
                     G.Comm.MotorPosCollection.Stop(EMotorPos.DrillsAlign_Servo_Open, out string AlignServoOpenErr), AlignServoOpenErr + "AlignServoOpen未停止",
                     G.Comm.MotorPosCollection.Stop(EMotorPos.DrillsAlign_Step_Open, out string AlignStepOpenErr), AlignStepOpenErr + "AlignStepOpen未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;

            }
        }
    }
}
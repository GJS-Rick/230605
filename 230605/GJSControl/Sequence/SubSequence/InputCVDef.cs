using CommonLibrary;
using System;

namespace nsSequence
{
    public class InputCVDef : BaseSequenceDef
    {
        public enum EMoveType
        {

            Init_StopCV,
            WaitPanelIn,
            PassThroughPanel,
            GoWaitPosition,
            MotorTurnOnAgain,

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
            WaitPanelIn,
            ThroughOrWait,
            PassThrough,
            GoWaitPosition,
            WaitForPassThrough,
            PassThroughAgain,
            Done,
            None,
        }

        public enum EMove
        {
            Idle,
            StopCV,
            StageCheck,

            LicenceOn,
            WaitPanelIn,
            MotorTurnOn,
            PrepareStopMotor,
            StopMotor,
            WaitPassThroughDone,
            GoWaitPosition,
            MotorTurnOnAgain,
            Done,
            None
        }

        public InputCVDef(
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
            G.Comm.MtnCtrl.SdStop(EAXIS_NAME.InputCV, true);
;           G.Comm.Scara.StopAll();
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
                        Move(EMoveType.Init_StopCV);
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
                        Move(EMoveType.Init_StopCV);
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
                        Move(EMoveType.Init_StopCV);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitPanelIn);
                    }
                    break;

                case EAuto.WaitPanelIn:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.WaitPanelIn);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.ThroughOrWait);
                    }
                    break;

                case EAuto.ThroughOrWait:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.ExportCV_ImportCV_CanPass))
                    {
                        _AutoStep.SetStep(EAuto.PassThrough);
                        break;
                    }
                    else
                    {
                        _AutoStep.SetStep(EAuto.GoWaitPosition);
                    }
                    break;

                case EAuto.PassThrough:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.PassThroughPanel);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitPanelIn);
                    }

                    break;

                case EAuto.GoWaitPosition:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.GoWaitPosition);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitForPassThrough);
                    }

                    break;


                case EAuto.WaitForPassThrough:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.ExportCV_ImportCV_CanPass))
                    {
                        _AutoStep.SetStep(EAuto.PassThroughAgain);
                    }
                    break;

                    case EAuto.PassThroughAgain: 
                    if (_AutoStep.FirstRun()) 
                    {
                        Move(EMoveType.MotorTurnOnAgain);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitPanelIn);
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
                        case EMoveType.Init_StopCV:
                            _MoveStep.SetStep(EMove.StopCV);
                            break;
                        case EMoveType.WaitPanelIn:
                            _MoveStep.SetStep(EMove.LicenceOn);
                            break;
                        case EMoveType.PassThroughPanel:
                            _MoveStep.SetStep(EMove.WaitPassThroughDone);
                            break;
                        case EMoveType.GoWaitPosition:
                            _MoveStep.SetStep(EMove.GoWaitPosition);
                            break;
                        case EMoveType.MotorTurnOnAgain:
                            _MoveStep.SetStep(EMove.MotorTurnOnAgain);
                            break;
                    }
                    break;

                case EMove.StopCV:
                    if (_MoveStep.FirstRun())
                    {

                        G.Comm.MtnCtrl.SdStop(EAXIS_NAME.InputCV, true);
                    }
                    if (_MoveStep.Success(3000,
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.InputCV,true),
                            "InputCV Not Stop"))
                    {
                        _MoveStep.SetStep(EMove.StageCheck);
                    }
                    break;

                    case EMove.StageCheck:
                    if (_MoveStep.FirstRun()) { }
                    if (_MoveStep.Success(1000,
                        !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ImportCV_PanelInPosition_1,true) &&
                        !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ImportCV_PanelInPosition_2,true) &&
                        !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ImportCV_PanelIn, true),
                        "Panel On ImportCV，Please Remove ,輸入滾輪組上有板子，請移除"))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    
                    break;

                    case EMove.LicenceOn:
                    if (_MoveStep.FirstRun()) 
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.External_Connect_Out1, true);
                    }
                    _MoveStep.SetStep(EMove.WaitPanelIn);

                    break;

                    case EMove.WaitPanelIn:
                    if (_MoveStep.FirstRun()) { }
                    if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ImportCV_PanelIn, true))
                    { 
                    _TickCount=Environment.TickCount;
                    }
                    if (Environment.TickCount-_TickCount>300)
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.External_Connect_Out1, false);
                        _MoveStep.SetStep(EMove.MotorTurnOn);
                    }
                    break;

                    case EMove.MotorTurnOn:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.InputCV, false, ESPEED_TYPE.Mid);
                    }
                    _MoveStep.SetStep(EMove.Done);

                    break;

                case EMove.WaitPassThroughDone:
                    if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ExportCV_PanelIn, true))
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (_MoveStep.Success(10000,
                        Environment.TickCount - _TickCount > 300,
                        "Panel Not PassThrough，Please Check,板子未移至出口滾輪組，請確認"))
                    {
                        _MoveStep.SetStep(EMove.PrepareStopMotor);
                    }

                    break;

                    case EMove.PrepareStopMotor:
                    if (_MoveStep.FirstRun()) { }
                 
                    if (_MoveStep.Success(5000,
                         !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ExportCV_PanelIn, true),
                         "Panel Not PassThrough，Please Check,板子未移至出口滾輪組，請確認"))
                    {
                        _MoveStep.SetStep(EMove.StopMotor);
                    }
                    break;



                    case EMove.StopMotor:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.SdStop(EAXIS_NAME.InputCV, true);

                    }
                    if (_MoveStep.Success(1500, 
                        G.Comm.MtnCtrl.Stop(EAXIS_NAME.InputCV,true),
                        "CV Not Stop ， CV未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }

                    break;

                    case EMove.GoWaitPosition:
                    if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ImportCV_PanelInPosition_2, true))
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (_MoveStep.Success(10000,
                        Environment.TickCount - _TickCount > 300,
                        "Panel Not at Stop Position，Please Check,板子未移至暫停點，請確認"))
                    {
                        _MoveStep.SetStep(EMove.StopMotor);
                    }

                    break;

                case EMove.MotorTurnOnAgain:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.InputCV, false, ESPEED_TYPE.Mid);
                    }
                    _MoveStep.SetStep(EMove.WaitPassThroughDone);

                    break;

            }
        }
    }
}
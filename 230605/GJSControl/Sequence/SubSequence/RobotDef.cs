using CommonLibrary;
using System;

namespace nsSequence
{
    public class RobotDef : BaseSequenceDef
    {
        public enum EMoveType
        {

           
            Init_GoStandby,
            GoTakePosition,
            GoPutPosition,

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
            WaitMoveToPutPosition,
            GoTakePosition,
            WaitTakeDone,
            GoPutPosition,
            WaitPutDone,

            Done,
            None,
        }

        public enum EMove
        {
            Idle,
            GoPEL,
            RobotStop,
            RobotLeavePEL,
            GoPELAgain,
            RobotStop_Delay500ms,
            Robot_ResetOrg,
            GoStandby,
            GoTake,
            GoPut,

            Done,
            None
        }
        public bool _FirstPanel;
        public bool _LastPanelPutDone;


        public RobotDef(
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
                    if (_InitialStep.FirstRun()) { }

                    if (_Handshake.IsOn(EHandshake.Putcylinder_Robot_InSafeZone) &&
                       _Handshake.IsOn(EHandshake.Takecylinder_Robot_InSafeZone))
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
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.Putcylinder_Robot_InSafeZone) &&
                       _Handshake.IsOn(EHandshake.Takecylinder_Robot_InSafeZone))
                    {
                        Move(EMoveType.Init_GoStandby); 
                    }

                    if (MoveDone())
                    {
                        _FirstPanel = true;
                        _LastPanelPutDone = false;
                        _AutoStep.SetStep(EAuto.GoTakePosition);
                    }
                    break;

                case EAuto.GoTakePosition:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.GoTakePosition);
                    }
                    if (MoveDone())
                    {
                        if (_LastPanelPutDone)
                        {
                            _AutoStep.SetStep(EAuto.Done);
                            break;
                        }

                        if (!_Handshake.GetDataBool(EDataBool.LastPanel))
                        {
                            _Handshake.SetOn(EHandshake.Robot_TakeCylinder_CanTake);
                        }
                        if (!_FirstPanel)
                        {
                            _Handshake.SetOn(EHandshake.Robot_PutCylinder_CanTake);
                        }

                        _AutoStep.SetStep(EAuto.WaitTakeDone);
                    }
                    break;

                case EAuto.WaitTakeDone:
                    if (_AutoStep.FirstRun()) { }
                    if (_FirstPanel &&
                        _Handshake.IsOn(EHandshake.TakeCylinder_Robot_TakeDone))
                    {
                        _AutoStep.SetStep(EAuto.GoPutPosition);
                        break;
                    }
                    if (_Handshake.GetDataBool(EDataBool.LastPanel) &&
                       _Handshake.IsOn(EHandshake.PutCylinder_Robot_TakeDone))
                    {
                        _AutoStep.SetStep(EAuto.GoPutPosition);
                        break;
                    }
                    if (_Handshake.IsOn(EHandshake.TakeCylinder_Robot_TakeDone) &&
                        _Handshake.IsOn(EHandshake.PutCylinder_Robot_TakeDone))
                    {
                        _AutoStep.SetStep(EAuto.GoPutPosition);
                    }
                    break;

                case EAuto.GoPutPosition:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.GoPutPosition);
                    }
                    if (MoveDone())
                    {
                        if (!_Handshake.GetDataBool(EDataBool.LastPanel))
                        {
                            _Handshake.SetOn(EHandshake.Robot_TakeCylinder_CanPut);
                        }

                        if (!_FirstPanel)
                        {
                            _Handshake.SetOn(EHandshake.Robot_PutCylinder_CanPut);
                        }

                        _AutoStep.SetStep(EAuto.WaitTakeDone);
                    }
                    break;

                case EAuto.WaitPutDone:
                    if (_AutoStep.FirstRun()) { }
                    if (_FirstPanel &&
                       _Handshake.IsOn(EHandshake.TakeCylinder_Robot_PutDone))
                    {
                        _FirstPanel = false;
                        _AutoStep.SetStep(EAuto.GoTakePosition);
                        break;
                    }
                    if (_Handshake.GetDataBool(EDataBool.LastPanel) &&
                        _Handshake.IsOn(EHandshake.PutCylinder_Robot_PutDone))
                    {
                        _LastPanelPutDone = true;
                        _AutoStep.SetStep(EAuto.GoTakePosition);
                        break;
                    }

                    if (_Handshake.IsOn(EHandshake.TakeCylinder_Robot_PutDone) &&
                        _Handshake.IsOn(EHandshake.PutCylinder_Robot_PutDone))
                    {
                        if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_Pallet_Inposition, false))
                            _Handshake.SetDataBool(EDataBool.LastPanel, true);
                        
                        _AutoStep.SetStep(EAuto.GoTakePosition);
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
                        case EMoveType.GoTakePosition:
                            _MoveStep.SetStep(EMove.GoTake);
                            break;
                        case EMoveType.GoPutPosition:
                            _MoveStep.SetStep(EMove.GoPut);
                            break;
                    }
                    break;

                case EMove.GoPEL:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.Robot, true, ESPEED_TYPE.Low);
                    }
                    if (_MoveStep.Success(10000,
                            G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.Robot,true), "Robot Go PEL Timeout, 手臂回正極限逾時"))
                    {
                        _MoveStep.SetStep(EMove.RobotStop);
                    }
                    break;


                case EMove.RobotStop:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.SdStop(EAXIS_NAME.Robot, true);
                    }
                    if (_MoveStep.Success(1000,
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.Robot, true), "Robot Not Stop, 手臂未停止"))
                    {
                        _MoveStep.SetStep(EMove.RobotStop);
                    }
                    break;

                case EMove.RobotLeavePEL:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.RelMv(EAXIS_NAME.Robot, -25, ESPEED_TYPE.Low);
                    }
                    if (_MoveStep.Success(5000,
                            G.Comm.MtnCtrl.Stop(EAXIS_NAME.Robot, true), "Robot Not Stop, 手臂未停止",
                           !G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.Robot,true),"Robot Not Leave LSP , 手臂未離開正極限"))
                        
                    {
                        _MoveStep.SetStep(EMove.RobotStop);
                    }
                    break;

                case EMove.GoPELAgain:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.Robot, true, ESPEED_TYPE.Home);
                    }
                    if (_MoveStep.Success(10000,
                            G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.Robot, true), "Robot Go PEL Timeout, 手臂回正極限逾時"))
                    {
                        _MoveStep.SetStep(EMove.RobotStop_Delay500ms);
                    }
                    break;


                case EMove.RobotStop_Delay500ms:
                    if (_MoveStep.FirstRun())
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 500)
                    {
                        _MoveStep.SetStep(EMove.Robot_ResetOrg);
                    }

                    break;

                case EMove.Robot_ResetOrg:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.HmMv(EAXIS_NAME.Robot, true);
                        G.Comm.MtnCtrl.SetPos(EAXIS_NAME.Robot, 0);
                    }

                    _MoveStep.SetStep(EMove.Done);

                    break;

                case EMove.GoStandby:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.Robot_Take);
                    }
                    if (_MoveStep.Success(10000,
                      G.Comm.MotorPosCollection.Stop(EMotorPos.Robot_Take, out string RobotStandbyErr), RobotStandbyErr + "RobotGoStandby未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);

                    }
                    break;


                case EMove.GoTake:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.Robot_Take);
                    }
                    if (_MoveStep.Success(10000,
                      G.Comm.MotorPosCollection.Stop(EMotorPos.Robot_Take, out string RobotTakeErr), RobotTakeErr + "RobotGoTake未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);

                    }
                    break;


                case EMove.GoPut:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.Robot_Put);
                    }
                    if (_MoveStep.Success(10000,
                      G.Comm.MotorPosCollection.Stop(EMotorPos.Robot_Take, out string RobotPutErr), RobotPutErr + "RobotGoPut未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);

                    }
                    break;

            }
        }
    }
}
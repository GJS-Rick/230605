using CommonLibrary;
using System;

namespace nsSequence
{
    public class LiftDef : BaseSequenceDef
    {
        public enum EMoveType
        {
            Init,
            Init_CheckPosition,
            GoProvide,
            CheckPosition,
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
            ProvidePanel,
            WaitTakeCylinderTakeaway,
            RenewLiftsPosition,

            Done,
            None,
        }

        public enum EMove
        {
            Idle,

            Init_CheckPosition,
            LeavePanelLimit,
            GoProvidePos,
            StopLifts,
            CheckPosition,
            Done,
            None
        }

        public LiftDef(
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

        private void LiftsUp()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsUp, true);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsDown, false);
        }
        private void LiftsDown()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsUp, false);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsDown, true);
        }

        private void LiftsStop()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsUp, false);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsDown, false);
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
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsUp, true);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.LiftsDown, true);

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
                        Move(EMoveType.Init);
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
                        Move(EMoveType.Init);
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
                       
                        Move(EMoveType.Init);
                    }
                    if (MoveDone())
                    {
                        
                        _AutoStep.SetStep(EAuto.ProvidePanel);
                    }
                    break;

                case EAuto.ProvidePanel:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.GoProvide);
                    }
                    if (MoveDone())
                    {
                        _Handshake.SetOn(EHandshake.Lifts_TakeCylinder_CanTake);
                        _AutoStep.SetStep(EAuto.WaitTakeCylinderTakeaway);

                    }
                    break;

                case EAuto.WaitTakeCylinderTakeaway:
                    if (_Handshake.IsOn(EHandshake.TakeCylinder_Lifts_TakeDone))
                    {
                        _AutoStep.SetStep(EAuto.RenewLiftsPosition);
                    }
                  
                    break;

                case EAuto.RenewLiftsPosition:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.CheckPosition);
                    }
                    {
                        _AutoStep.SetStep(EAuto.ProvidePanel);
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
                        case EMoveType.Init:
                            _MoveStep.SetStep(EMove.Init_CheckPosition);
                            break;

                        case EMoveType.GoProvide:
                            _MoveStep.SetStep(EMove.GoProvidePos);
                            break;

                        case EMoveType.CheckPosition:
                            _MoveStep.SetStep(EMove.CheckPosition);
                            break;

                    }
                    break;


                case EMove.Init_CheckPosition:
                    if (_MoveStep.FirstRun()) { }

                    if (
                        !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_Lifts_PanelLimit, true) ||
                        !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_Lifts_InPosition, true)
                       )
                    {
                        _MoveStep.SetStep(EMove.LeavePanelLimit);
                    }

                    else
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }

                    break;

                case EMove.LeavePanelLimit:
                    if (_MoveStep.FirstRun()) 
                    {
                        LiftsDown();
                    }

                    if (_MoveStep.Success(10000,G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_Lifts_InPosition, true)
                        , "Lift Go Down Timeout , 舉升下降逾時"))
                    
                    {
                        _MoveStep.SetStep(EMove.StopLifts);
                    }
                    
                    break;



                case EMove.GoProvidePos:

                    if (_MoveStep.FirstRun())
                    {
                       LiftsUp();
                    }

                    if (_MoveStep.Success(20000, !G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_Lifts_InPosition, true),
                       "Lift Go Up Timeout , 舉升上升逾時"))
                    {
                        _MoveStep.SetStep(EMove.StopLifts);
                    }

                    break;
                case EMove.StopLifts:
                    if (_MoveStep.FirstRun())
                    {
                        LiftsStop();
                    }
                    _MoveStep.SetStep(EMove.Done);

                    break;


                case EMove.CheckPosition:

                    if (_MoveStep.FirstRun())
                    {
                        if (G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_Lifts_InPosition, true))
                        {
                            _MoveStep.SetStep(EMove.GoProvidePos);
                        }

                        else 
                        {
                            _MoveStep.SetStep(EMove.LeavePanelLimit);
                        }
                    }

                    break;

            }
        }
    }
}
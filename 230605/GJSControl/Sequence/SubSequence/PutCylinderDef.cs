using CommonLibrary;
using System;

namespace nsSequence
{
    public class PutCylinderDef : BaseSequenceDef
    {
        public enum EMoveType
        {

            GoStandby,
            TakePanel,
            PutPanel,
            Init_CylinderUp,
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
            WaitTake,
            Take,
            WaitPut,
            Put,

            Done,
            None,
        }

        public enum EMove
        {
            Idle,
            CylinderReset,
            Take_CylinderDown,
            VacOn,
            VacOnDelay,
            Take_CylinderUp,
            
            Put_CylinderDown,
            VacOff,
            VacOffDelay,
            Put_CylinderUp,



            
            Done,
            None
        }
        private void CylinderUp()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelCylinderUp, true);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelCylinderDown, false);
        }

        private void CylinderDown()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelCylinderUp, false);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelCylinderDown, true);
        }

        private void VacOn()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVacOn, true);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVacOff, false);
        }

        private void VacOff()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVacOn, false);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVacOff, true);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelBreakVac, true);
        }

        private void BreakVacOff()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelBreakVac, false);
        }

        public PutCylinderDef(
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
                        Move(EMoveType.GoStandby);
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
                        Move(EMoveType.Init_CylinderUp);
                    }
                    if (MoveDone())
                    {
                        _Handshake.SetOn(EHandshake.Putcylinder_Robot_InSafeZone);
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
                        Move(EMoveType.Init_CylinderUp);
                    }
                    if (MoveDone())
                    {
                        _Handshake.SetOn(EHandshake.Putcylinder_Robot_InSafeZone);

                        _AutoStep.SetStep(EAuto.WaitTake);
                    }
                    break;

                case EAuto.WaitTake:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.Drills_PutCylinder_CanTake) &&
                        _Handshake.IsOn(EHandshake.Robot_PutCylinder_CanTake))
                    {
                        _AutoStep.SetStep(EAuto.Take);
                    }

                    break;


                case EAuto.Take:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.TakePanel);
                    }
                    if (MoveDone())
                    {
                        _Handshake.SetOn(EHandshake.PutCylinder_Robot_TakeDone);
                        _Handshake.SetOn(EHandshake.PutCylinder_Drill_TakeDone);
                        _AutoStep.SetStep(EAuto.WaitPut);
                    }
                    break;

                case EAuto.WaitPut:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.OutputCV_PutCylinder_CanPut) &&
                        _Handshake.IsOn(EHandshake.Robot_PutCylinder_CanPut))
                    {
                        _AutoStep.SetStep(EAuto.Put);
                    }

                    break;

                    case EAuto.Put: 
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.PutPanel);
                    }
                    if (MoveDone())
                    {
                        _Handshake.SetOn(EHandshake.PutCylinder_OutputCV_PutDone);
                        _Handshake.SetOn(EHandshake.PutCylinder_Robot_PutDone);
                        
                        _AutoStep.SetStep(EAuto.WaitTake);
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
                        case EMoveType.Init_CylinderUp:
                            _MoveStep.SetStep(EMove.CylinderReset);
                            break;

                        case EMoveType.TakePanel:
                            _MoveStep.SetStep(EMove.Take_CylinderDown);
                            break;

                        case EMoveType.PutPanel:
                            _MoveStep.SetStep(EMove.Take_CylinderDown);
                            break;

                    }
                    break;

                case EMove.CylinderReset:
                    if (_MoveStep.FirstRun())
                    {
                        CylinderUp();
                    }

                    if (_MoveStep.Success
                        (10000,
                         G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PutPanelCylinder_Top, true),
                         "Cylinder Go Up Timeout , 放板氣缸上升逾時(2X05)"
                         ))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;


                case EMove.Take_CylinderDown:
                    if (_MoveStep.FirstRun())
                    {
                        CylinderDown();
                    }

                    if (_MoveStep.Success
                        (10000,
                         G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PutPanelCylinder_Down, true),
                         "Cylinder Go Down Timeout , 放板氣缸下降逾時(2X06)"
                         ))
                    {
                        _MoveStep.SetStep(EMove.VacOn);
                    }
                    break;

                case EMove.VacOn:
                    if (_MoveStep.FirstRun())
                    {
                        VacOn();
                    }

                    if (_MoveStep.Success
                        (10000,
                         G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PutPanel_Vacuum, true),
                         "Vacuum Not Enough , 放板手臂真空不足(2X09)"
                         ))
                    {
                        _MoveStep.SetStep(EMove.VacOnDelay);
                    }

                    break;

                case EMove.VacOnDelay:
                    if (_MoveStep.FirstRun())
                    { 
                    _TickCount = Environment.TickCount;
                    }

                    if (Environment.TickCount - _TickCount > 200)

                    {
                        _MoveStep.SetStep(EMove.Take_CylinderUp);
                    }

                    break;

                case EMove.Take_CylinderUp:
                    if (_MoveStep.FirstRun())
                    {
                        CylinderUp();
                    }

                    if (_MoveStep.Success
                        (10000,
                         G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PutPanelCylinder_Top, true),
                         "Cylinder Go Up Timeout , 放板氣缸上升逾時(2X05)"
                         ))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;

                case EMove.Put_CylinderDown:
                    if (_MoveStep.FirstRun())
                    {
                        CylinderDown();
                    }
                    if (_MoveStep.Success
                       (10000,
                        G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PutPanelCylinder_Down, true),
                        "Cylinder Go Down Timeout , 放板氣缸下降逾時(2X06)"
                        ))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;

                case EMove.VacOff:
                    if (_MoveStep.FirstRun())
                    {
                        VacOff();
                        _TickCount = Environment.TickCount;
                    }

                    if (Environment.TickCount - _TickCount > 200)
                    {
                        _MoveStep.SetStep(EMove.VacOffDelay);
                    }
                    break;

                    case EMove.VacOffDelay:

                    if (_MoveStep.FirstRun())
                    {
                        BreakVacOff();
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 200)
                    {
                        _MoveStep.SetStep(EMove.Put_CylinderUp);
                    }
                    break;

                    case EMove.Put_CylinderUp:
                    if (_MoveStep.FirstRun())
                    { 
                    CylinderUp();
                    }
                    if (_MoveStep.Success
                       (10000,
                        G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PutPanelCylinder_Top, true),
                        "Cylinder Go Up Timeout , 放板氣缸上升逾時(2X05)"
                        ))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;
                    

            }
        }
    }
}
using CommonLibrary;
using FileStreamLibrary;
using System;

namespace nsSequence
{
    public class TakeCylinderDef : BaseSequenceDef
    {
        public enum EMoveType
        {

            Init_CylinderUp,
            TakePanel,
            PutPanel,

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
            VibrationOn,
            PressCylinderDown,
            Take_CylinderUp,
            VibrationShock,
            PressCylinderUp,
            Put_CylinderDown,
            VacOff,
            BreakVacDelay,
            Put_CylinderUp,

            Done,
            None
        }

        public TakeCylinderDef(
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

        public int ShockTimes;
        public bool _FirstShock;

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

        private void VibrationOff()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_1, false);
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_2, false);
        }
        private void PressCylinderUp()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanel_PressPanel_Down, false);
        }

        private void PressCylinderDown()
        {
            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanel_PressPanel_Down, true);
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
            ShockTimes = 0;
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
                        Move(EMoveType.Init_CylinderUp);
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
                        _Handshake.SetOn(EHandshake.Takecylinder_Robot_InSafeZone);
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
                        _Handshake.SetOn(EHandshake.Takecylinder_Robot_InSafeZone);
                        _AutoStep.SetStep(EAuto.WaitTake);
                    }
                    break;

                case EAuto.WaitTake:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.Lifts_TakeCylinder_CanTake) &&
                       _Handshake.IsOn(EHandshake.Robot_TakeCylinder_CanTake))
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
                        _Handshake.SetOn(EHandshake.TakeCylinder_Robot_TakeDone);
                        _Handshake.SetOn(EHandshake.TakeCylinder_Lifts_TakeDone);
                        _AutoStep.SetStep(EAuto.WaitPut);
                    }
                    break;

                case EAuto.WaitPut:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.Robot_TakeCylinder_CanPut))
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
                        _Handshake.SetOn(EHandshake.TakeCylinder_Robot_PutDone);
                        _Handshake.SetOn(EHandshake.TakeCylinder_Drills_PutDone);
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
                            ShockTimes = 0;
                            _MoveStep.SetStep(EMove.CylinderReset);
                            break;
                        case EMoveType.TakePanel:
                            _MoveStep.SetStep(EMove.Take_CylinderDown);
                            break;
                        case EMoveType.PutPanel:
                            _MoveStep.SetStep(EMove.Put_CylinderDown);
                            break;
                    }
                    break;

                case EMove.CylinderReset:
                    if (_MoveStep.FirstRun())
                    {
                        CylinderUp();
                        VibrationOff();
                        PressCylinderUp();
                    }

                    if (_MoveStep.Success
                        (10000,
                         G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_TakePanelCylinder_Top, true),
                         "Cylinder Go Up Timeout , 氣缸上升逾時(1X14)",
                         G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PressPanelCylinder_Top, true),
                         "PressCylinder Go Up Timeout , 壓板氣缸上升逾時(2X00)"
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
                        G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_TakePanelCylinder_Down, true),
                        "Cylinder Go Down Timeout , 氣缸下降逾時(1X15)"
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
                      (2000,
                       G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_TakePanel_Vacuum, true),
                       "Vac Not Enough , 真空不足(2X04)"
                      ))
                    {
                        _MoveStep.SetStep(EMove.VibrationOn);
                    }
                    break;

                case EMove.VibrationOn:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_1, true);
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_2, true);
                        _TickCount = Environment.TickCount;
                    }
                    if (_MoveStep.Success
                      (2000,
                      Environment.TickCount - _TickCount > 300,
                       ""
                      ))
                    {
                        _MoveStep.SetStep(EMove.PressCylinderDown);
                    }
                    break;

                case EMove.PressCylinderDown:
                    if (_MoveStep.FirstRun())
                    {
                        PressCylinderDown();
                    }
                    if (_MoveStep.Success
                      (2000,
                       G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PressPanelCylinder_Down, true),
                       "PressCylinder Not Down , 壓板汽缸未下降(2X01)"
                      ))
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
                      (2000,
                       G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_TakePanelCylinder_Top, true),
                       "Cylinder Go Up Timeout , 氣缸上升逾時(1X14)"
                      ))
                    {
                        _MoveStep.SetStep(EMove.VibrationShock);
                    }
                    break;

                case EMove.VibrationShock:
                    if (_MoveStep.FirstRun())
                    {
                        if (ShockTimes == 0)
                            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_1, !G.Comm.IOCtrl.GetDO(EDO_TYPE.TakePanelVibration_1, true));

                        else
                        {
                            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_1, !G.Comm.IOCtrl.GetDO(EDO_TYPE.TakePanelVibration_1, true));
                            G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_2, !G.Comm.IOCtrl.GetDO(EDO_TYPE.TakePanelVibration_2, true));
                        }

                        _TickCount = Environment.TickCount;
                        _FirstShock = true;
                    }

                    if (Environment.TickCount - _TickCount > G.FS.MachineData.GetValue(EMachineDouble.ShockTime) * 1000 &&
                        _FirstShock)
                    {
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_1, !G.Comm.IOCtrl.GetDO(EDO_TYPE.TakePanelVibration_1, true));
                        G.Comm.IOCtrl.SetDO(EDO_TYPE.TakePanelVibration_2, !G.Comm.IOCtrl.GetDO(EDO_TYPE.TakePanelVibration_2, true));

                        _FirstShock = false;
                    }

                    if (Environment.TickCount - _TickCount > G.FS.MachineData.GetValue(EMachineDouble.ShockTime) * 2000)
                    {

                        ShockTimes++;
                        if (ShockTimes < G.FS.MachineData.GetValue(EMachineDouble.ShockTimes))
                        {
                            _MoveStep.SetPreStep(EMove.None);
                            _MoveStep.SetStep(EMove.VibrationShock);
                            break;
                        }
                        else
                        {
                            ShockTimes = 0;
                            _MoveStep.SetStep(EMove.PressCylinderUp);
                        }
                    }

                    break;

                case EMove.PressCylinderUp:
                    if (_MoveStep.FirstRun())
                    {
                        CylinderUp();
                    }
                                   
                    if (_MoveStep.Success
                      (2000,
                       G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_PressPanelCylinder_Top, true),
                       "PressCylinder Not Up , 壓板汽缸未上升(2X00)"
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
                       (2000,
                        G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_TakePanelCylinder_Down, true),
                        "TakeCylinder Not Down , 取板汽缸未下降(1X15)"
                       ))
                    {
                        _MoveStep.SetStep(EMove.VacOff);
                    }

                    break;

                    case EMove.VacOff:
                    if (_MoveStep.FirstRun())
                    {
                        VacOff();
                        _TickCount = Environment.TickCount;
                    }

                    if(Environment.TickCount-_TickCount>200)

                    {
                        _MoveStep.SetStep(EMove.BreakVacDelay);
                    }

                    break;

                    case EMove.BreakVacDelay:

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
                       (2000,
                        G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_TakePanelCylinder_Top, true),
                        "TakeCylinder Not Up , 取板汽缸未上升(1X14)"
                       ))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }

                    break;
            }
        }
    }
}
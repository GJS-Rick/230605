using CommonLibrary;
using FileStreamLibrary;
using System;

namespace nsSequence
{
    public class OutputCVDef : BaseSequenceDef
    {
        public enum EMoveType
        {

            Init_ResetCVAlign,
            CVAlign,
            WaitExport,
            WaitPCB_In,
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
            PanelMode,
            WaitPutDone,
            CVAlign,
            WaitExport,
            Export,
            WaitPassTrough,
            
            Done,
            None,
        }

        public enum EMove
        {
            Idle,
            CVAlignGoPEL,
            CVAlignStop,
            CVAlign_LeaveLsn,
            CVAlign_GoPELAgain,
            CVAlign_Stop_Delay500ms,
            CVAlign_ResetOrg,

            CVAlign_Close,
            CVAlign_Delay,
            CVAlign_Open,

            WaitPCB_In,
            PCB_GoWaitPosition,
            PCB_Wait_MotorStop,

            WaitExportSignal,
            ExportMotorOn,
            ExportCheck,
            ExportMotorStop,


            Done,
            None
        }

        public enum Panel
        {
            Board,
            PCB,
        }

        public Panel PanelMode ;
        public int _PassPCBCount;

        public OutputCVDef(
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
            G.Comm.MtnCtrl.SdStop(EAXIS_NAME.OutputCV, true);
            G.Comm.MtnCtrl.SdStop(EAXIS_NAME.CV_Align, true);
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
                        Move(EMoveType.Init_ResetCVAlign);
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
                        Move(EMoveType.Init_ResetCVAlign);
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
                        Move(EMoveType.Init_ResetCVAlign);
                    }
                    if (MoveDone())
                    {
                        PanelMode = Panel.Board;
                        _PassPCBCount = 0;
                        _AutoStep.SetStep(EAuto.PanelMode);
                    }
                    break;

                case EAuto.PanelMode:
                    if (_AutoStep.FirstRun())
                    {
                        switch (PanelMode)
                        {
                            case Panel.Board:
                                _Handshake.SetOn(EHandshake.OutputCV_PutCylinder_CanPut);
                                _AutoStep.SetStep(EAuto.WaitPutDone);

                                break;

                            case Panel.PCB:
                                _Handshake.SetOn(EHandshake.ExportCV_ImportCV_CanPass);
                                _AutoStep.SetStep(EAuto.WaitPassTrough);
                                break;

                        }

                    }

                    break;

                case EAuto.WaitPutDone:
                    if (_AutoStep.FirstRun()) { }
                    if (_Handshake.IsOn(EHandshake.PutCylinder_OutputCV_PutDone))
                    {
                        _AutoStep.SetStep(EAuto.CVAlign);
                    }
                    break;

                    case EAuto.CVAlign:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.CVAlign);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitExport);
                    }

                    break;

                case EAuto.WaitPassTrough:
                    if (_AutoStep.FirstRun()) 
                    {
                        Move(EMoveType.WaitPCB_In);
                    }
                    if (MoveDone())
                    {
                        _AutoStep.SetStep(EAuto.WaitExport);
                    }


                    break;

                    case EAuto.WaitExport:
                    if (_AutoStep.FirstRun())
                    {
                        Move(EMoveType.WaitExport);
                    }
                    if (MoveDone())
                    {
                        if (PanelMode == Panel.Board)
                        {
                            PanelMode = Panel.PCB;
                            _AutoStep.SetStep(EAuto.PanelMode);
                            break;
                        }

                        if (PanelMode == Panel.PCB)
                        {
                            _PassPCBCount++;
                            if (_PassPCBCount == G.FS.MachineData.GetValue(EMachineInt.PassPCBTimes))
                            {
                                _PassPCBCount = 0;
                                PanelMode = Panel.Board;
                            }
                            _AutoStep.SetStep(EAuto.PanelMode);
                        }
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
                        case EMoveType.Init_ResetCVAlign:
                            _MoveStep.SetStep(EMove.CVAlignGoPEL);
                            break;

                        case EMoveType.CVAlign:
                            _MoveStep.SetStep(EMove.CVAlign_Close);
                            break;

                        case EMoveType.WaitPCB_In:
                            _MoveStep.SetStep(EMove.WaitPCB_In);
                            break;

                        case EMoveType.WaitExport:
                            _MoveStep.SetStep(EMove.WaitExportSignal);
                            break;

                    }
                    break;


                case EMove.CVAlignGoPEL:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.CV_Align, true, ESPEED_TYPE.Low);
                    }
                    if (_MoveStep.Success(10000,
                        !G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.CV_Align, true),
                         G.Comm.MtnCtrl.GetAxisName(EAXIS_NAME.CV_Align) + "not on PEL , 馬達未到極限"))
                    {
                        _MoveStep.SetStep(EMove.CVAlignStop);
                    }

                    break;

                case EMove.CVAlignStop:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.EmgStop(EAXIS_NAME.CV_Align);
                    }

                    if (_MoveStep.Success(1000,
                        G.Comm.MtnCtrl.Stop(EAXIS_NAME.CV_Align, true),
                        "CVAlign Not Stop , 滾輪整板馬達未停止 "))
                    {
                       
                        _MoveStep.SetStep(EMove.CVAlign_LeaveLsn);
                    }

                    break;

                case EMove.CVAlign_LeaveLsn:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.RelMv(EAXIS_NAME.CV_Align, -20, ESPEED_TYPE.Low);
                    }

                    if (_MoveStep.Success(1000,
                        G.Comm.MtnCtrl.Stop(EAXIS_NAME.CV_Align, true),
                        "CVAlign Not Stop , 滾輪整板馬達未停止 "))
                    {
                        _MoveStep.SetStep(EMove.CVAlign_GoPELAgain);
                    }

                    break;


                case EMove.CVAlign_GoPELAgain:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.CV_Align, true, ESPEED_TYPE.Home);
                    }
                    if (_MoveStep.Success(10000,
                        !G.Comm.MtnCtrl.GetPEL(EAXIS_NAME.CV_Align, true),
                         G.Comm.MtnCtrl.GetAxisName(EAXIS_NAME.CV_Align) + "not on PEL , 馬達未到極限"))
                    {
                        _MoveStep.SetStep(EMove.CVAlign_Stop_Delay500ms);
                    }

                    break;

                case EMove.CVAlign_Stop_Delay500ms:
                    if (_MoveStep.FirstRun())
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 500)
                    {
                        _MoveStep.SetStep(EMove.CVAlign_ResetOrg);
                    }

                    break;

                case EMove.CVAlign_ResetOrg:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.HmMv(EAXIS_NAME.CV_Align, true);
                        G.Comm.MtnCtrl.SetPos(EAXIS_NAME.CV_Align, 0);
                    }

                    _MoveStep.SetStep(EMove.Done);
                    break;


                case EMove.CVAlign_Close:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.CV_Align_Close);
                    }
                    if (_MoveStep.Success(10000,
                       G.Comm.MotorPosCollection.Stop(EMotorPos.CV_Align_Close, out string AlignCloseErr), AlignCloseErr + "整板馬達未停止"))
                    {
                        _MoveStep.SetStep(EMove.CVAlign_Delay);
                    }
                    break;

                case EMove.CVAlign_Delay:
                    if (_MoveStep.FirstRun())
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (Environment.TickCount - _TickCount > 200)
                    {
                        _MoveStep.SetStep(EMove.CVAlign_Open);
                    }

                    break;

                case EMove.CVAlign_Open:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MotorPosCollection.Move(EMotorPos.CV_Align_Open);
                    }
                    if (_MoveStep.Success(10000,
                       G.Comm.MotorPosCollection.Stop(EMotorPos.CV_Align_Close, out string AlignOpenErr), AlignOpenErr + "整板馬達未停止"))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;

                case EMove.WaitPCB_In:
                    if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ExportCV_PanelIn, true))
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if ( Environment.TickCount - _TickCount > 300)
                    {
                        _MoveStep.SetStep(EMove.PCB_GoWaitPosition);
                    }
                    break;

                case EMove.PCB_GoWaitPosition:
                    if (_MoveStep.FirstRun()) 
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.OutputCV, false, ESPEED_TYPE.Mid);
                    }

                    if (_MoveStep.Success(5000,
                    G.Comm.IOCtrl.GetDI(EDI_TYPE.Sensor_ExportCV_PanelInPosition_2, true), "PCB Not Locat , PCB 未至定位點"))
                    {
                        _MoveStep.SetStep(EMove.PCB_Wait_MotorStop);
                    }

                    break;

                case EMove.PCB_Wait_MotorStop:
                    if (_MoveStep.FirstRun())
                    { G.Comm.MtnCtrl.SdStop(EAXIS_NAME.OutputCV); }
                    if (_MoveStep.Success(1000,
                      G.Comm.MtnCtrl.Stop(EAXIS_NAME.OutputCV, true),
                      "ExportCV not Stop ，出口滾輪未停 "))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }

                    break;

                case EMove.WaitExportSignal:
                    if (_MoveStep.FirstRun()) { }
                    if (G.Comm.IOCtrl.GetDI(EDI_TYPE.ExternalConnect_In1, true))
                    { 
                        _MoveStep.SetStep(EMove.ExportMotorOn);
                    }

                    break;

                case EMove.ExportMotorOn:
                    if (_MoveStep.FirstRun())
                    {
                        G.Comm.MtnCtrl.ConMv(EAXIS_NAME.OutputCV, false, ESPEED_TYPE.Mid);
                    }
                    _MoveStep.SetStep(EMove.ExportCheck);

                    break;

                case EMove.ExportCheck:
                    if (!G.Comm.IOCtrl.GetDI(EDI_TYPE.Senesor_ExportCV_PanelOut, true))
                    {
                        _TickCount = Environment.TickCount;
                    }
                    if (_MoveStep.Success(10000,
                        Environment.TickCount - _TickCount > 300,
                        "Panel Not PassThrough，Please Check,板子未移至出口滾輪組，請確認"))
                    {
                        _MoveStep.SetStep(EMove.ExportMotorStop);
                    }
                    break;

                case EMove.ExportMotorStop:
                    if (_MoveStep.FirstRun()) 
                    {
                        G.Comm.MtnCtrl.SdStop(EAXIS_NAME.OutputCV);
                    }
                    if (_MoveStep.Success(1000,
                        G.Comm.MtnCtrl.Stop(EAXIS_NAME.OutputCV,true),
                        "ExportCV not Stop ，出口滾輪未停 "))
                    {
                        _MoveStep.SetStep(EMove.Done);
                    }


                    break;


            }

        }
    }
}
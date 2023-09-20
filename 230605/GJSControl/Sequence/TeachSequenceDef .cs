using CommonLibrary;
using FileStreamLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisionLibrary;

namespace nsSequence
{
    public class TeachSequenceDef
    {
        enum ETeachStep
        {
            Idle,

            Init_

            Done,
            None,
        }


        protected Enum  _CurrentStep;
        protected Enum  _PreStep;
        protected int  _TickCount;
        CommonManagerDef _CommonManager;
        VisionManagerDef _VisionManager;
        FileManagerDef _FileStreamManager;

        public TeachSequenceDef(CommonManagerDef cObjManager, VisionManagerDef cVisionManager, FileManagerDef cFileStreamManager)
        {
            _CommonManager = cObjManager;
            _VisionManager = cVisionManager;
            _FileStreamManager = cFileStreamManager;
            ResetSeqence();

        }

        public void ResetSeqence()
        {
            _CurrentStep = ETeachStep.Idle;
            _PreStep = ETeachStep.None;
        }
        public bool Done()
        {
            return (ETeachStep)_CurrentStep == ETeachStep.Done;
        }

        public void Dispose()
        {

        }

        public void Execute()
        {
            MoveFlow();
            WriteLog();
        }
        private void MoveFlow()
		{
            switch ((ETeachStep)_CurrentStep)
            {
                case EMove.AutoInitial_CheckPCBonFork:

                    if (_MoveStep.FirstRun())
                    {
                        if (_CommonManager.MtnCtrl.GetDI(EDI_TYPE.ScaraForkVacReached, false))
                        {
                            SetSequenceStatus(
                                     AlarmTextDisplay.AlarmCode.Auto_PcbOnScaraFork,
                                        AlarmTextDisplay.AlarmType.Warning,
                                        StatusCode.Stop);
                            break;
                        }
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraBlowOn, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraVacOn, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraVacOff, false);
                        _MoveStep.SetStep(EMove.AutoInitial_WaitMylarReady);
                    }
                    break;

                case EMove.AutoInitial_WaitMylarReady:

                    if (_FileStreamManager.SeqenceHandshake.GetMylarFlag(EMylarFlag.MylarReady))
                    {
                        _MoveStep.SetStep(EMove.AutoInitial_GoToZTop);
                    }
                    break;

                case EMove.AutoInitial_GoToZTop:
                    if (_MoveStep.FirstRun())
                    {
                        _CommonManager.MtnCtrl._Scara.AbsoluteMove(Scara_mr_je_a_modbus.Axis.J4, 0, 10);
                    }
                    if (_MoveStep.Success(
                        30000,
                        ref _timeoutIndex,
                        _CommonManager.MtnCtrl._Scara.IsStopped(Scara_mr_je_a_modbus.Axis.J4, true)))
                    {
                        _MoveStep.SetStep(EMove.AutoInitial_GoToMidPoint);
                    }
                    break;

                case EMove.AutoInitial_GoToMidPoint:
                    //Action
                    if (_MoveStep.FirstRun())
                    {
                        _CommonManager.MtnCtrl._Scara.SingleLinearMove(
                            _CommonManager._MotorPosCollection.GetMotorPos(MotroPosition.Scara_MidPoint)._Value,
                            (ushort)20);
                    }
                    //Condition
                    if (_MoveStep.Success(
                        10000,
                        ref _timeoutIndex,
                        _CommonManager.MtnCtrl._Scara.IsSingleLinearMoveArrived(1, true)))
                    {
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraBlowOn, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraVacOn, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraVacOff, true);

                        _MoveStep.SetStep(EMove.AutoInitial_GoToStandbyPosition);
                    }
                    break;

                case EMove.AutoInitial_GoToStandbyPosition:
                    //Action
                    if (_MoveStep.FirstRun())
                    {
                        _CommonManager.MtnCtrl._Scara.SingleLinearMove(
                            _CommonManager._MotorPosCollection.GetMotorPos(MotroPosition.Scara_StandbyPosition)._Value,
                            (ushort)20);
                    }
                    //Condition
                    if (_MoveStep.Success(
                        10000,
                        ref _timeoutIndex,
                        _CommonManager.MtnCtrl._Scara.IsSingleLinearMoveArrived(1, true)))
                    {
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraBlowOn, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraVacOn, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.ScaraVacOff, true);

                        _FileStreamManager.SeqenceHandshake.SetScaraFlag(EScaraFlag.ScrarReady, true);

                        _MoveStep.SetStep(EMove.Done);
                    }
                    break;
            }
        }

        public bool FirstRun()
        {
            if ( _CurrentStep !=  _PreStep)
            {
                 _PreStep =  _CurrentStep;
                return true;
            }
            return false;
        }

        public void SetStep(Enum eStep)
        {
            if (eStep.GetType() !=  _CurrentStep.GetType())
            {
                throw new Exception("Type Error");
               
            }

             _CurrentStep = eStep;
        }

        public void SetPreStep(Enum ePreStep)
        {
            if (ePreStep.GetType() !=  _CurrentStep.GetType())
            {
                throw new Exception("Type Error");
            }

             _PreStep = ePreStep;
        }

        public Enum GetStep()
        {
            return  _CurrentStep;
        }

        public Enum GetPreStep()
        {
            return  _PreStep;
        }
        protected void WriteLog()
        {
            if (GetPreStep() != GetStep() &&
                 _PreStep != GetStep())
            {
                _PreStep = GetStep();

                LogDef.Add(
                    ELogFileName.General,
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "," + GetStep().GetType().Name,
                     GetStep().ToString());
            }
        }

    }
}

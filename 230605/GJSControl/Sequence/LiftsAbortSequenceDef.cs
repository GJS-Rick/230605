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
    public class LiftsAbortSequenceDef
    {
        enum ELiftsAbortStep
        {
            Idle,
            Stop_WaitSignal,
            Stop_CheckImportSensor,
            Stop_LiftDown,
            Done,
            None,
        }


        protected Enum  _CurrentStep;
        protected Enum  _PreStep;
        protected int  _TickCount;
        CommonManagerDef _CommonManager;
        VisionManagerDef _VisionManager;
        FileManagerDef _FileStreamManager;

        public LiftsAbortSequenceDef(CommonManagerDef cObjManager, VisionManagerDef cVisionManager, FileManagerDef cFileStreamManager)
        {
            _CommonManager = cObjManager;
            _VisionManager = cVisionManager;
            _FileStreamManager = cFileStreamManager;
            ResetSeqence();

        }

        public void ResetSeqence()
        {
            _CurrentStep = ELiftsAbortStep.Idle;
            _PreStep = ELiftsAbortStep.None;
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
            switch ((ELiftsAbortStep)_CurrentStep)
            {
                case ELiftsAbortStep.Idle:

                    SetStep(ELiftsAbortStep.Stop_WaitSignal);

                    break;

                case ELiftsAbortStep.Stop_WaitSignal:
                    if (_FileStreamManager.SeqenceHandshake.GetButtonFlag(EButtonFlag.LiftsAbort))
                    {
                        //if (!_CommonManager.MtnCtrl.GetDI(EDI_TYPE.LiftsOrigin, true))
                        // {
                        SetStep(ELiftsAbortStep.Stop_CheckImportSensor);
                        //}
                    }
                    break;

                case ELiftsAbortStep.Stop_CheckImportSensor:
                    if (FirstRun())
                    {
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.LiftsDown, false);
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.LiftsUp, false);
                    }
                    if (!_CommonManager.MtnCtrl.GetDI(EDI_TYPE.LiftsOrigin, true))
                    {
                        SetStep(ELiftsAbortStep.Stop_LiftDown);
                    }
                    else
                    {
                        _FileStreamManager.SeqenceHandshake.SetButtonFlag(EButtonFlag.LiftsAbort, false);
                        SetStep(ELiftsAbortStep.Idle);
                    }
                    break;

                case ELiftsAbortStep.Stop_LiftDown:
                    if (FirstRun())
                    {
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.LiftsDown, true);
                    }
                    if (_CommonManager.MtnCtrl.GetDI(EDI_TYPE.LiftsOrigin, false))
                    {
                        _FileStreamManager.SeqenceHandshake.SetExportData(ExportData.CurrentLotCount, 0);//退料後計數歸零
                        _CommonManager.MtnCtrl.SetDO(EDO_TYPE.LiftsDown, false);
                        _FileStreamManager.SeqenceHandshake.SetButtonFlag(EButtonFlag.LiftsAbort, false);
                        SetStep(ELiftsAbortStep.Idle);
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

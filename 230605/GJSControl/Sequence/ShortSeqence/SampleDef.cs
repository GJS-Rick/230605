using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using VisionLibrary;
using nsUI;
using FileStreamLibrary;

namespace nsSequence
{
    /// <summary>
    /// 請在 MainSequenceDef.cs內
    /// <para>1. 宣告 : public SampleDef Sample;</para>
    /// <para>2. 在MainSequenceDef()內實例化 : Sample = new SampleDef();</para>
    /// <para>3. 在Dispose()內釋放 : Sample.Dispose();</para>
    /// 4. 開啟停止方法為 Sample.Start() / Sample.Stop()，建議在運轉前將所有ShortSequence停止
    /// </summary>
    public class SampleDef : ShortSeqenceDef
    {
        enum EShortStep
        {
            Idle,

            Start,
            Run,
            End,

            Done,
            None,
        }


        public SampleDef()
            : base()
        {
            _CurrentStep = EShortStep.None;
            _PreStep = EShortStep.Done;
            _NoneStep = EShortStep.None;
        }


        public void Start()
        {
            _CurrentStep = EShortStep.Idle;
        }

        public bool Done()
        {
            return (EShortStep)_CurrentStep == EShortStep.Done;
        }

        public void Stop()
        {
            _CurrentStep = EShortStep.None;
        }

        protected override void Run()
        {

            switch ((EShortStep)_CurrentStep)
            {
                case EShortStep.Idle:
                    SetStep(EShortStep.Start);
                    break;

                case EShortStep.Start:
                    if (FirstRun())
                    {
                        
                    }
                    if(Success(1000, 
                        true, "condition1",
                        true, "condition2",
                        true, "condition3",
                        true, "condition4"))
                    {
                        SetStep(EShortStep.Run);
                    }
                    break;

                case EShortStep.Run:
                    if (FirstRun())
                    {

                    }
                    if (Success(1000,
                        true, "condition1",
                        true, "condition2",
                        true, "condition3",
                        true, "condition4"))
                    {
                        SetStep(EShortStep.Run);
                    }
                    break;

                case EShortStep.End:
                    if (FirstRun())
                    {

                    }
                    if (Success(1000,
                        true, "condition1",
                        true, "condition2",
                        true, "condition3",
                        true, "condition4"))
                    {
                        SetStep(EShortStep.Done);
                    }
                    break;

            }
        }
    }
}

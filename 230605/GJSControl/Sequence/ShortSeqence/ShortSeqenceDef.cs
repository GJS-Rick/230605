using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace nsSequence
{
    public abstract class ShortSeqenceDef : IDisposable
    {
        protected Enum  _CurrentStep;
        protected Enum  _PreStep;
        protected int  _TickCount;
        protected Thread  _ThTask;
        protected bool  _ThreadEnd;
        protected int _TimeoutTick;
        protected Enum _NoneStep;

        public ShortSeqenceDef()
        {
             
            _ThTask = new Thread(DoLoop)
            {
                Priority = ThreadPriority.BelowNormal
            };

            _ThTask.Start();
        }





        public virtual void Dispose()
        {
             _ThreadEnd = true;
            while ( _ThTask != null &&  _ThTask.IsAlive)
            {
                Thread.Sleep(5);
            }
        }

       

        protected void DoLoop()
        {
            while (! _ThreadEnd)
            {
                Run();
                Thread.Sleep(1);
            }
        }

        protected virtual void Run()
        {
        }

        protected bool FirstRun()
        {
            if ( _CurrentStep !=  _PreStep)
            {
                _TimeoutTick = Environment.TickCount;
                _PreStep =  _CurrentStep;
                return true;
            }
            return false;
        }

        protected void SetStep(Enum eStep)
        {
            if (eStep.GetType() !=  _CurrentStep.GetType())
            {
                throw new Exception("Type Error");
               
            }

             _CurrentStep = eStep;
        }

        protected void SetPreStep(Enum ePreStep)
        {
            if (ePreStep.GetType() !=  _CurrentStep.GetType())
            {
                throw new Exception("Type Error");
            }

             _PreStep = ePreStep;
        }

        protected Enum GetStep()
        {
            return  _CurrentStep;
        }

        protected Enum GetPreStep()
        {
            return  _PreStep;
        }
        public bool Success(
                 int TimeoutTime,
                 bool Condition1, string condition1,
                 bool Condition2 = true, string condition2 = "",
                 bool Condition3 = true, string condition3 = "",
                 bool Condition4 = true, string condition4 = "")
        {
            if (_CurrentStep != _PreStep)
            {
                _TimeoutTick = Environment.TickCount;
                _PreStep = _CurrentStep;
            }

            if (Condition1 & Condition2 & Condition3 & Condition4)
                return true;

            DialogResult result = DialogResult.None;
            if (TimeoutTime > -1 && Environment.TickCount - _TimeoutTick > TimeoutTime)
            {
                if (!Condition1)
                {
                    result = MessageBox.Show(condition1, "TIMEOUT:" + _CurrentStep.ToString(), MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                else if (!Condition2)
                {
                    result = MessageBox.Show(condition2, "TIMEOUT:" + _CurrentStep.ToString(), MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                else if (!Condition3)
                {
                    result = MessageBox.Show(condition3, "TIMEOUT:" + _CurrentStep.ToString(), MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                else if (!Condition4)
                {
                    result = MessageBox.Show(condition4, "TIMEOUT:" + _CurrentStep.ToString(), MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }
            if (result == DialogResult.Retry)
                _TimeoutTick = Environment.TickCount;
            if (result == DialogResult.Cancel)
            {
                _CurrentStep = _NoneStep;
                _PreStep = _NoneStep;
            }
            return false;
        }

    }
}

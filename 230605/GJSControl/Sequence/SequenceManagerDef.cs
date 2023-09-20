using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUI;
using System.Threading;
using VisionLibrary;
using FileStreamLibrary;
using CommonLibrary;

namespace nsSequence
{
    public class SequenceManagerDef : IDisposable
    {
        private Thread _ThTask;
        private bool _ThreadEnd;

        public MainSequenceDef MainSequence;

        public SequenceManagerDef()
        {
            MainSequence = new MainSequenceDef();

            _ThTask = new Thread(DoLoop)
            {
                Priority = ThreadPriority.AboveNormal
            };

            _ThTask.Start();
        }

        public virtual void Dispose()
        {
            _ThreadEnd = true;
            while (_ThTask != null && _ThTask.IsAlive)
            {
                Thread.Sleep(5);
            }

            MainSequence.Dispose();
        }

        protected void DoLoop()
        {
            while (!_ThreadEnd)
            {
                MainSequence.Run();

                Thread.Sleep(1);
            }
        }
    }
}
using System;
using System.Threading;

namespace Zipper.Zipping.Workers
{
    public abstract class Worker
    {
        public bool HasFailed { get; protected set; }

        public Exception InternalException { get; protected set; }

        protected Thread Thread { get; }

        protected abstract string Name { get; }

        public Worker()
        {
            Thread = new Thread(() => DoWork())
            {
                Name = Name,
                IsBackground = true
            };
        }

        public void Start()
        {
            Thread.Start();
        }

        public void Join()
        {
            Thread.Join();
        }

        protected abstract void DoWork();
    }
}

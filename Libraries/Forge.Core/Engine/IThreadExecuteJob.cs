using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Engine
{
    /// <summary>
    /// A job to be processed by a set of threads.
    /// When executing, each thread will pass in its thread number.
    /// </summary>
    public interface IThreadExecuteJob
    {
        void Execute(int threadNumber);
    }
}

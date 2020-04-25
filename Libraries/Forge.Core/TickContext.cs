using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    public class TickContext
    {
        public uint DeltaTime { get; internal set; }
        public float DeltaTimeSeconds { get; internal set; }
        public uint TimeStamp { get; internal set; }
        public float TimeSeconds { get; internal set; }
        public IServiceProvider ServiceProvider { get; internal set; }
        //public EntityManager EntityManager { get; internal set; }
    }
}

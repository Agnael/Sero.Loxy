using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public interface ILoxy
    {
        void Raise(IEventCandidate evt);
        Task Flush();
    }

    public interface ILoxy<TContext> : ILoxy where TContext : class
    {
        void ConfigureContext(Action<TContext> configAction);
    }
}

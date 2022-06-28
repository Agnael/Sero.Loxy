using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public interface ISink
    {
        Task Send (IEvent evt);
        Task Send (IScope eventCandidates);

        /// <summary>
        ///     Gets the minimum scope level this sink is interested in 
        ///     receiving.
        /// </summary>
        LogLevel GetMinimumLevel();

        /// <summary>
        ///     Gets the level that, if found within a scope, will cause the
        ///     minimum level constraint to be ignored and dump the complete
        ///     event list into the sink.
        /// </summary>
        LogLevel GetDumpLevel();
    }
}

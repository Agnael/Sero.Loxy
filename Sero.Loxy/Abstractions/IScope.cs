using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public interface IScope
    {
        /// <summary>
        ///     The maximum level that can be found among it´s events.
        /// </summary>
        LogLevel Level { get; }

        Guid ScopeId { get; }
        string ContextTypeFullName { get; }
        object Context { get; }
        int ContextConfiguratorCount { get; }

        IEnumerable<IEvent> Events { get; }
    }
}

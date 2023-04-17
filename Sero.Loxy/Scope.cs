using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sero.Loxy
{
    public class Scope : IScope
    {
        private readonly Guid _scopeId;
        private readonly LogLevel _level;
        private readonly object _context;
        private readonly int _contextConfiguratorsUsedCount;
        private readonly string _contextTypeFullName;
        private readonly IEnumerable<IEvent> _events;

        public Scope(
            Guid scopeId,
            object context,
            int contextConfiguratorsUsedCount,
            IEnumerable<IEvent> events)
        {
            if (scopeId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(scopeId));

            _scopeId = scopeId;
            _context = context;
            _contextConfiguratorsUsedCount = contextConfiguratorsUsedCount;                        
            _events = events;

            _level = LogLevel.None;
            if (events.Count() > 0)
            {
                _level = events.Max(x => x.Level);
            }

            _contextTypeFullName = null;
            if (context != null)
            {
                _contextTypeFullName = context.GetType().GetFriendlyFullName();
            }
        }

        public LogLevel Level => _level;
        public Guid ScopeId => _scopeId;
        public string ContextTypeFullName => _contextTypeFullName;
        public object Context => _context;
        public int ContextConfiguratorCount => _contextConfiguratorsUsedCount;
        public IEnumerable<IEvent> Events => _events;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public class DefaultScopeBuilder : IScopeBuilder
    {
        private readonly IEventMapper _eventMapper;

        public DefaultScopeBuilder(
            IEventMapper eventMapper)
        {
            _eventMapper = eventMapper;
        }

        public async Task<IScope> Build(
            object context, 
            int contextConfiguratorCount,
            IEnumerable<IEvent> events)
        {
            Scope scope = 
                new Scope(
                    Guid.NewGuid(),
                    context,
                    contextConfiguratorCount,
                    events);

            return scope;
        }
    }
}

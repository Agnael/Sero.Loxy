using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public interface IScopeBuilder
    {
        Task<IScope> Build(
            object context,
            int contextConfiguratorCount,
            IEnumerable<IEvent> events);
    }
}

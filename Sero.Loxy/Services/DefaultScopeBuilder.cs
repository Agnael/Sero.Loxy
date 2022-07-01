using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sero.Loxy
{
   public class DefaultScopeBuilder : IScopeBuilder
   {
      public DefaultScopeBuilder()
      {

      }

      public async Task<IScope> Build(
         object context,
         int contextConfiguratorCount,
         IEnumerable<IEvent> events
      ) {
         Scope scope = new Scope(Guid.NewGuid(), context, contextConfiguratorCount, events);
         return scope;
      }
   }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
   public record EventCandidate(
      LogLevel Level, 
      string Category, 
      string Message, 
      Exception Exception = null
   ) : IEventCandidate
   {
      public virtual IEnumerable<string> GetDetails()
      {
         return Enumerable.Empty<string>();
      }
   }

   public record EventCandidate<TState>(
      LogLevel Level,
      string Category,
      string Message,
      TState State,
      Func<TState, Exception, string> StateFormatter,
      Exception Exception = null
   ) : EventCandidate(Level, Category, Message, Exception)
   {
      public override IEnumerable<string> GetDetails()
      {
         return new string[] { 
            StateFormatter(State, Exception) 
         };
      }
   }
}

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Sero.Loxy;

public record LoxyStateFormatter(
   EventId TargetEventId,
   // TODO: This lax "object" type for the state parameter is not bueno. Don't have to try to force strong
   // typing on it right now but it's kind of shitty without that.
   Func<object, Exception, IEnumerable<string>> CustomStateFormatter
);
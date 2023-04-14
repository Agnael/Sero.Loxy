using Microsoft.Extensions.Logging;
using System;

namespace Sero.Loxy;

public interface ILoxyStateFormatterProvider
{
   Maybe<LoxyStateFormatter> GetFor(EventId eventId);
}
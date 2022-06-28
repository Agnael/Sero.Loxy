using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public interface IEventMapper
    {
        IEvent Map(TimestampedEventCandidate candidate);
    }
}

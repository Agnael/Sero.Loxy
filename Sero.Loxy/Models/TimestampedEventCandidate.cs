using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sero.Loxy
{
    public class TimestampedEventCandidate
    {
        public readonly Instant CreationInstant;
        public readonly IEventCandidate Candidate;

        public TimestampedEventCandidate(
            Instant creationInstant, IEventCandidate candidate)
        {
            if (candidate == null)
                throw new ArgumentNullException(nameof(candidate));

            this.CreationInstant = creationInstant;
            this.Candidate = candidate;
        }
    }
}

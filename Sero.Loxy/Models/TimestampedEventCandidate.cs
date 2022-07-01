using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sero.Loxy;

public record TimestampedEventCandidate(Instant CreationInstant, IEventCandidate Candidate);

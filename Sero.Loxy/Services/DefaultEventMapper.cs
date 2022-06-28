using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public class DefaultEventMapper : IEventMapper
    {
        private readonly IExceptionMapper _exceptionMapper;

        public DefaultEventMapper(
            IExceptionMapper exceptionMapper)
        {
            _exceptionMapper = exceptionMapper;
        }

        public IEvent Map(TimestampedEventCandidate timestampedCandidate)
        {
            IEventCandidate candidate = timestampedCandidate.Candidate;

            Event mapped = new Event();
            mapped.Category = candidate.Category;
            mapped.CreationDtUtc = 
                timestampedCandidate.CreationInstant.ToDateTimeUtc();
            mapped.Level = candidate.Level;
            mapped.Message = candidate.Message;
            mapped.TypeFullName = candidate.GetType().GetFriendlyFullName();

            IEnumerable<string> candidateDetails = candidate.GetDetails();
            if (candidateDetails != null)
            {
                mapped.Details = candidateDetails;
            }
            else
            {
                mapped.Details = new List<string>();
            }

            if (candidate.Exception == null)
            {
                mapped.Exception = Enumerable.Empty<ExceptionOverview>();
            }
            else
            {
                mapped.Exception = _exceptionMapper.Map(candidate.Exception);
            }

            return mapped;
        }
    }
}

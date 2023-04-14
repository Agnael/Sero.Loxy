using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public class DefaultExceptionMapper : IExceptionMapper
    {
        public static readonly string[] STACK_TRACE_LINE_SEPARATORS =
            new string[] { "\n" };

        public DefaultExceptionMapper()
        {

        }

        public ExceptionOverview FormatExceptionSingle(Exception ex)
        {
            ExceptionOverview mapped = new ExceptionOverview();
            mapped.ExceptionTypeName = ex.GetType().FullName;
            mapped.Message = ex.Message;

            IEnumerable<string> stackStepList = Enumerable.Empty<string>();

            if (ex.StackTrace == null)
            {
                stackStepList =
                    new string[] {
                    "Empty stack trace. The exception was probably " +
                    "originated at an unmanaged source."
                    };
            }
            else
            {
                stackStepList = 
                    ex
                    .StackTrace
                    .Split(
                        STACK_TRACE_LINE_SEPARATORS, 
                        StringSplitOptions.RemoveEmptyEntries);
            }

            List<string> stackStepListFormatted = new List<string>();

            foreach (string stackStep in stackStepList)
            {
                string stepFormatted = stackStep.Trim();

                stackStepListFormatted.Add(stepFormatted);
            }

            mapped.StackTrace = stackStepListFormatted.ToArray();

            return mapped;
        }

        public void FillExceptionInfoListRecursive(ICollection<ExceptionOverview> destinationCollection, Exception ex)
        {
            if (destinationCollection == null) throw new ArgumentNullException(nameof(destinationCollection));
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            ExceptionOverview newInfo = FormatExceptionSingle(ex);
            destinationCollection.Add(newInfo);

            if (ex.InnerException != null)
                FillExceptionInfoListRecursive(destinationCollection, ex.InnerException);
        }

        public IEnumerable<ExceptionOverview> Map(Exception ex)
        {
            var mappedList = new List<ExceptionOverview>();

            if (ex is AggregateException)
            {
                IEnumerable<Exception> innerExceptions = 
                    (ex as AggregateException)
                    .Flatten()
                    .InnerExceptions;

                foreach (var innerException in innerExceptions)
                {
                    ExceptionOverview mapped = FormatExceptionSingle(innerException);
                    mappedList.Add(mapped);
                }
            }
            else // Default handling
            {
                FillExceptionInfoListRecursive(mappedList, ex);
            }

            return mappedList;
        }
    }
}

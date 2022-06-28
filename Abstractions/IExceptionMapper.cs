using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public interface IExceptionMapper
    {
        IEnumerable<ExceptionOverview> Map(Exception ex);
    }
}

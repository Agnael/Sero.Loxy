using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public class NonCompressibleSqlCommandException : Exception
    {
        public NonCompressibleSqlCommandException() 
            : base("It was not possible to get enough descriptive information from this SqlCommand.")
        {
        }
    }
}

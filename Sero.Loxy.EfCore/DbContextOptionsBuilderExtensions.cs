using Microsoft.EntityFrameworkCore;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseLoxyAsLoggerFactory(this DbContextOptionsBuilder options, ILoxy loxy, string logCategory)
        {
            options.UseLoggerFactory(
                loxy.AsLoggerFactory(proxy => 
                    proxy.WithCategory(logCategory)
                         .WithStateFormatter(new CleanStateFormatter())
                )
            );

            return options;
        }
    }
}

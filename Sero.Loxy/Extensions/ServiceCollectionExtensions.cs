using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
   public static class ServiceCollectionExtensions
   {
      public static IServiceCollection AddLoxy<TContext>(this IServiceCollection services)
         where TContext : class
      {
         services
            .AddSingleton<ILoggerProvider, LoxyLoggerProvider>()
            .AddSingleton<IExceptionMapper, DefaultExceptionMapper>()
            .AddSingleton<IScopeBuilder, DefaultScopeBuilder>()
            .AddSingleton<IEventMapper, DefaultEventMapper>()
            .AddScoped<ILoxy<TContext>, Loxy<TContext>>()
            .AddScoped<ILoxy>(sp => sp.GetService<ILoxy<TContext>>());

         return services;
      }

      public static IServiceCollection AddLoxyContextSynonym<TContext, TSynonym>(this IServiceCollection services)
         where TContext : class
         where TSynonym : class, ILoxy<TContext>
      {
         services.AddScoped<TSynonym>(sp => (TSynonym)sp.GetService<ILoxy<TContext>>());

         return services;
      }
   }
}

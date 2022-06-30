using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
   public static class ServiceCollectionExtensions
   {
      public static IServiceCollection TryAddLoxyDependencies(this IServiceCollection services)
      {
         services.TryAddSingleton<ILoggerProvider, LoxyLoggerProvider>();
         services.TryAddSingleton<IExceptionMapper, DefaultExceptionMapper>();
         services.TryAddSingleton<IScopeBuilder, DefaultScopeBuilder>();
         services.TryAddSingleton<IEventMapper, DefaultEventMapper>();

         return services;
      }

      public static IServiceCollection AddLoxy<TContext>(this IServiceCollection services)
         where TContext : class
      {
         services
            .TryAddLoxyDependencies()
            .AddScoped<ILoxy<TContext>, Loxy<TContext>>();
            
         
         services.TryAddScoped<ILoxy>(sp => sp.GetService<ILoxy<TContext>>());

         return services;
      }
   }
}

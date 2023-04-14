using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Sero.Loxy.EfCore;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddLoxyEfCoreStateFormatters(this IServiceCollection services)
   {
      services.AddSingleton<LoxyStateFormatter>(
         new LoxyStateFormatter(
            RelationalEventId.CommandExecuted,
            (state, ex) => FormattingUtils.FormatCommandExecuted(state)
         )
      );

      services.AddSingleton<LoxyStateFormatter>(
         new LoxyStateFormatter(
            RelationalEventId.CommandExecuting,
            (state, ex) => FormattingUtils.FormatCommandExecuting(state)
         )
      );

      return services;
   }
}

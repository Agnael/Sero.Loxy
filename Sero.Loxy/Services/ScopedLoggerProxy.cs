using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Sero.Loxy;

public class ScopedLoggerProxy : ILogger
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly string _categoryName;

	public ScopedLoggerProxy(
		IHttpContextAccessor httpContextAccessor,
		string categoryName)
	{
		_httpContextAccessor = httpContextAccessor;
		_categoryName = categoryName;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return state as IDisposable;
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public void Log<TState>(
	   LogLevel logLevel,
	   EventId eventId,
	   TState state,
	   Exception exception,
	   Func<TState, Exception, string> defaultFormatter)
	{
		// Getting the ILoxy instance on each call might seem inefficient but it's the only way to
		// do it, since this Logger instance will be saved as a singleton even if the ILogger's 
		// user class is not.

		// If you want efficiency, use an ILoxy directly instead of an ILogger.
		// This class should only be seen as a workaround for ILoxy unaware classes within an app.
		if (_httpContextAccessor.HttpContext == null)
		{
			throw new InvalidOperationException(
				$"The {nameof(ScopedLoggerProxy)} should be only used within HTTP requests."
			);
		}

		ILoxy loxy = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ILoxy>();

		loxy.Raise(
		   new LoggerProxyEventCandidate<TState>(
			  eventId,
			  logLevel,
			  _categoryName,
			  eventId.Name,
			  state,
			  defaultFormatter,
			  exception
		   )
		);
	}
}
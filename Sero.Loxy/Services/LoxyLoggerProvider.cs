using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace Sero.Loxy
{
    public class LoxyLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEnumerable<ISink> _sinks;
        private readonly IEventMapper _eventMapper;
        private readonly IClock _clock;

        public LoxyLoggerProvider(
            //ILoxy loxy,
            IHttpContextAccessor httpContextAccessor,
            IClock clock,
            IEventMapper eventMapper,
            IEnumerable<ISink> sinks)
        {
            //_loxy = loxy;
            _clock = clock;
            _httpContextAccessor = httpContextAccessor;
            _eventMapper = eventMapper;
            _sinks = sinks;
        }

        public ILogger CreateLogger(string categoryName)
        {
            HttpContext currentHttpContext = _httpContextAccessor.HttpContext;

            if (currentHttpContext == null)
            {
                // TODO: Should this dependencies get resolved on each call or
                // just assume they are singletons?

                // NOT processing an HTTP request.
                // It may be some system logging or a JOB, which SHOULD have
                // a scope, but TODO: how can we tell when its a JOB?
                return new HybridLoggerProxy(
                    categoryName, 
                    _httpContextAccessor, 
                    _sinks, 
                    _eventMapper, 
                    _clock);
            }
            else
            {
                ILoxy loxy = currentHttpContext.RequestServices.GetService<ILoxy>();

                return new ScopedLoggerProxy(loxy, _clock, categoryName);
            }
        }

        public void Dispose()
        {
            //_loxy.Dispose();
        }
    }
}

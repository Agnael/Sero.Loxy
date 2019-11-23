using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Core.Middleware;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public class EventLoggerMiddleware : AbstractMiddleware
    {
        private ILoxy _eventLogger;

        public EventLoggerMiddleware(RequestDelegate next) 
            : base(next)
        {

        }

        protected override async Task OnBefore(HttpContext context)
        {
            _eventLogger = (ILoxy)context.RequestServices.GetService(typeof(ILoxy));
        }

        protected override async Task OnAfter(HttpContext context)
        {
            await _eventLogger.PersistAsync();
        }

        protected override async Task OnError(Exception ex)
        {
            await _eventLogger.RaiseAsync(new Event(LogLevel.Critical, "Error", "An unexpected exception was catched in the middleware phase end.", ex));
            await _eventLogger.PersistAsync();
        }
    }
}

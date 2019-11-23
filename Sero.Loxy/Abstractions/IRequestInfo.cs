using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public interface IRequestInfo
    {
        LogLevel GetHighestLevel();

        string AppName { get; }

        string AppVersion { get; }

        /// <inheritdoc/>
        /// <summary>
        /// Application environment name. E.g.: (Development, Staging, Production, etc.)
        /// </summary>
        string Environment { get; }

        /// <summary>
        /// The level of the event with the highest LogLevel in the request
        /// </summary>
        string Level { get; }

        /// <summary>
        /// HTTP Verb used by this request
        /// </summary>
         string Verb { get; }

        /// <summary>
        /// Requested base URL (Without QueryString)
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Requested QueryString
        /// </summary>
        string QueryString { get; }

        string UserAgent { get; }

        string AcceptLanguage { get; }

        /// <summary>
        /// Gets or sets the per-request activity identifier.
        /// </summary>
        string RequestTraceIdentitfier { get; }

        /// <summary>
        /// Gets or sets the date (UTC) of the raised event.
        /// </summary>
        DateTime Datetime { get; }

        /// <summary>
        /// Seconds passed since Jan 01 1970 (UTC).
        /// </summary>
        long UnixTimestamp { get; }

        /// <summary>
        /// Gets or sets the server process identifier.
        /// </summary>
        int IdProcess { get; }

        /// <summary>
        /// Gets or sets the local ip address of the current request.
        /// </summary>
        string LocalIpAddress { get; }

        /// <summary>
        /// Gets or sets the remote ip address of the current request.
        /// </summary>
        string RemoteIpAddress { get; }

        IEnumerable<IEvent> Events { get; }
    }
}

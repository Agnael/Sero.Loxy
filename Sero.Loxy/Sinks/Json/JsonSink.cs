using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy.Sinks.Json
{
    public class JsonSink : ISink
    {
        private ILogger _logger;
        //private JsonSinkOptions _options;
        private JsonSinkBuilder _builder;

        //public LogLevel ExtendedLevel => _options.LevelExtended;
        //public LogLevel MinimumLevel => _options.LevelMinimum;
        public LogLevel ExtendedLevel => _builder.LevelExtended;
        public LogLevel MinimumLevel => _builder.LevelMinimum;

        public JsonSink(JsonSinkBuilder builder, ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException("logger", "A Microsoft.Extensions.Logging.ILogger<> instance must be provided.");

            _logger = logger;
            _builder = builder;
            //_options = new JsonSinkOptions();
        }

        public async Task PersistAsync(IRequestInfo requestInfo)
        {
            var dto = new RequestInfoDTO(requestInfo);
            string jsonMessage = JsonConvert.SerializeObject(dto, _builder.JsonFormatting);
            _logger.Log(requestInfo.GetHighestLevel(), jsonMessage);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class ExceptionOverview
    {
        [JsonProperty(Order = 1)]
        public string ExceptionTypeName { get; set; }

        [JsonProperty(Order = 2)]
        public string Message { get; set; }

        [JsonProperty(Order = 3)]
        public string[] StackTrace { get; set; }
    }
}

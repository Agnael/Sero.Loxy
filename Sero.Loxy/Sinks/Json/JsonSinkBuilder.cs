using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Sero.Loxy.Sinks.Json
{
    public class JsonSinkBuilder : AbstractSinkBuilder<JsonSinkBuilder>
    {
        public override JsonSinkBuilder CurrentInstance => this;

        public Formatting JsonFormatting { get; private set; }

        public JsonSinkBuilder(SinkManager currentSinkManager) 
            : base(currentSinkManager)
        {
            JsonFormatting = Formatting.Indented;
        }

        public JsonSinkBuilder WithJsonFormatting(Formatting formatting)
        {
            JsonFormatting = formatting;
            return this;
        }
    }
}

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sero.Loxy
{
    public class JsonEventCandidate<TState> : EventCandidate
    {
        public TState State { get; private set; }

        public JsonEventCandidate(
            LogLevel lvl, string cat, string msg, TState state)
            : base(lvl, cat, msg)
        {
            State = state;
        }

        public JsonEventCandidate(
            LogLevel lvl, string cat, string msg, TState state, Exception ex)
            : base(lvl, cat, msg, ex)
        {
            State = state;
        }

        private string GetSerialized(object obj)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';

                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(writer, obj);
            }

            return sb.ToString();
        }

        public override IEnumerable<string> GetDetails()
        {
            if (this.State is IEnumerable)
            {
                List<string> detailList = new List<string>();

                foreach (object item in (this.State as IEnumerable))
                {
                    detailList.Add(GetSerialized(item));
                }

                return detailList;
            }

            return new string[] { GetSerialized(this.State) };
        }
    }
}

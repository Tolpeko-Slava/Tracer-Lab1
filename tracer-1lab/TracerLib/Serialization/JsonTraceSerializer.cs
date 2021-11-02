using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using TracerLib.TracerResult;

namespace TracerLib.Serialization
{
    public class JsonTraceSerializer : ITraceSerializer
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            AllowTrailingCommas = false,
            WriteIndented = true
        };
        public string Serialize(TraceResult traceResult)
        {
            return JsonSerializer.Serialize(traceResult, Options);
        }

    }
}

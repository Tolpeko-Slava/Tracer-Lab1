using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracerLib.TracerResult;

namespace TracerLib.Serialization
{
    public  interface ITraceSerializer
    {
        string Serialize(TraceResult traceResult);
    }
}

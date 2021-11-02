using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TracerLib.TracerResult;


namespace TracerLib.Serialization
{
    public  class XMLTraceSerializer : ITraceSerializer
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(TraceResult));

        public string Serialize(TraceResult traceResult)
        {


            StringWriter stringWriter = new StringWriter();
            Serializer.Serialize(stringWriter, traceResult);
            return stringWriter.ToString();

        }
    }
}

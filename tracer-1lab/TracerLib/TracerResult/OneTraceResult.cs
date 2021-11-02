using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TracerLib.TracerResult
{
    [XmlType("method")]
    public  class OneTraceResult
    {

        private Stack<OneTraceResult> st = null;
        private long tickCount;
        private int milliSeconds;
        private string methodeName;
        private string className;

        [XmlIgnore]
        public int id;

        public OneTraceResult()
        {


        }

        public OneTraceResult(string classname, string methode, int MsCount, long tick, int id1)
        {
            tickCount = tick;
            className = classname;
            milliSeconds = MsCount;
            methodeName = methode;
            id = id1;
        }

        [JsonIgnore]
        [XmlIgnore]
        public long TickCount
        {
            get
            {
                return tickCount;
            }
            set { }
        }

        [JsonPropertyName("time")]
        [XmlAttribute("time")]
        public int MilliSeconds
        {
            get
            {
                return milliSeconds;
            }
            set { }
        }

        [JsonPropertyName("name")]
        [XmlAttribute("name")]
        public string MethodeName
        {
            get
            {
                return methodeName;
            }
            set { }

        }

        [JsonPropertyName("class")]
        [XmlAttribute("class")]
        public string ClassName
        {
            get
            {
                return className;
            }
            set { }

        }

        [JsonPropertyName("methods")]
        [XmlElement("method")]
        public List<OneTraceResult> Methods
        {
            get
            {
                if (st != null)
                    return st.ToList<OneTraceResult>();
                else
                    return null;
            }

        }

        public void PushChild(OneTraceResult child)
        {
            if (st == null)
            {
                st = new Stack<OneTraceResult>();
                st.Push(child);
            }
            else
            {
                if (id == child.id - 1)
                {
                    st.Push(child);

                }
                else
                {
                    st.Peek().PushChild(child);
                }

            }

        }

        internal OneTraceResult Copy()
        {
            OneTraceResult traceResult = new OneTraceResult(this.className, this.methodeName, this.milliSeconds, this.tickCount, this.id);
            return traceResult;
        }


    }
}

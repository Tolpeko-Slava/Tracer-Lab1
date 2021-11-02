using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TracerLib.TracerResult
{
    [XmlType("thread")]
    public class RecurTraceResults
    {
        private Stack<OneTraceResult> threadStack = null;
        private int time = 0;
        private int threadId = -1;

        [JsonPropertyName("time")]
        [XmlAttribute("time")]
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }

        [JsonPropertyName("id")]
        [XmlAttribute("id")]
        public int ThreadID
        {
            get
            {
                return threadId;
            }
            set
            {
                threadId = value;

            }

        }

        [JsonPropertyName("methods")]
        [XmlElement("method")]
        public List<OneTraceResult> Methods
        {

            get
            {
                return ThreadStack.ToList<OneTraceResult>();

            }

        }

        internal Stack<OneTraceResult> ThreadStack
        {
            get
            {
                return threadStack;
            }
            set
            {
                threadStack = value;

                foreach (OneTraceResult oneTraceResult in threadStack)
                {
                    if (oneTraceResult.id == 0)
                    {
                        time += oneTraceResult.MilliSeconds;
                    }

                }
            }

        }




        internal RecurTraceResults Copy()
        {
            RecurTraceResults newThreadResults = new RecurTraceResults();
            newThreadResults.threadId = this.threadId;
            newThreadResults.time = this.time;
            newThreadResults.threadStack = new Stack<OneTraceResult>();

            foreach (OneTraceResult oneTrace in threadStack.Reverse())
            {
                newThreadResults.threadStack.Push(oneTrace.Copy());


            }

            return newThreadResults;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracerLib.TracerResult;
using System.Diagnostics;
using System.Threading;


namespace TracerLib.Tracers
{
    public class TracerReal : ITracer
    {

        private static object Lock = new object();
        private TraceResult traceResult = null;

        private Stack<Stopwatch> stopWatch = new Stack<Stopwatch>();
        private Dictionary<int, int> callID = new Dictionary<int, int>();

        public void StartTrace()
        {

            lock (Lock)
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                stopWatch.Push(st);

                if (!callID.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                {
                    callID.Add(Thread.CurrentThread.ManagedThreadId, 0);
                }


                callID[Thread.CurrentThread.ManagedThreadId]++;
            }
        }


        public void StopTrace()
        {

            lock (Lock)
            {
                try
                {
                    TimeSpan ts = stopWatch.Pop().Elapsed;
                    int id = 0;

                    callID[Thread.CurrentThread.ManagedThreadId]--;
                    id = callID[Thread.CurrentThread.ManagedThreadId];


                    StackTrace stackTrace = new StackTrace(true);
                    var frame = stackTrace.GetFrame(1);
                    string methode = frame.GetMethod().Name,
                    ClassName = frame.GetMethod().DeclaringType.Name;


                    long tick = ts.Ticks;
                    int ms = (int)ts.TotalMilliseconds;

                    if (traceResult == null)
                    {
                        traceResult = new TraceResult();
                    }

                    {
                        traceResult.Push(ClassName, methode, ms, tick, id, Thread.CurrentThread.ManagedThreadId);
                    }


                }
                catch
                {
                    return;
                }

            }

        }


        public TraceResult GetTraceResult()
        {
            return traceResult.Copy();
        }


    }
}

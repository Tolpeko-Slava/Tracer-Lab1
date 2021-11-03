using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TracerLib.Tracers;
using TracerLib.Serialization;
using TracerLib.TracerResult;
using System.Threading;


namespace TracerTests
{
    [TestClass]
    public class TracerTest
    {
        private ITracer tracer;
        private  Foo foo;

        [TestInitialize]
        public void Initialize()
        {
            tracer = new TracerReal();
            foo = new Foo(tracer);

        }

        [TestMethod]
        public void OneThreadTest()
        {
            foo.MyMethod();
            TraceResult traceResult = tracer.GetTraceResult();
            Assert.AreEqual(traceResult.Threads.Count,1);

        }

        [TestMethod]
        public void TwoThreadsTest()
        {
            Thread t1 = new Thread(foo.MyMethod);
            t1.Start();
            t1.Join();
            foo.MyMethod();
            
            TraceResult traceResult = tracer.GetTraceResult();
            Assert.AreEqual(traceResult.Threads.Count, 2);

        }

        [TestMethod]
        public void SameTraceResultTest()
        {
            Thread t1 = new Thread(foo.MyMethod);
            t1.Start();
            t1.Join();
            foo.MyMethod();

            TraceResult res1 = tracer.GetTraceResult();
            TraceResult res2 = tracer.GetTraceResult();

           

            bool SameThreads = res1.Threads.TrueForAll((RecurTraceResults tr1 )=> (res2.Threads.Find((RecurTraceResults tr) => 
                                                                              (tr.ThreadID == tr1.ThreadID && tr.Time == tr1.Time))!=null));

            bool sameMethods = res1.Threads.TrueForAll((RecurTraceResults tr1) => (res2.Threads.Find((RecurTraceResults tr) =>
                                                                              (tr.Methods.TrueForAll((OneTraceResult oneTr) =>
                                                                              (tr1.Methods.Find((OneTraceResult one) => 
                                                                              (one.MilliSeconds == oneTr.MilliSeconds && one.MethodeName == oneTr.MethodeName))) != null))) != null)) ;
            Assert.IsTrue(SameThreads && sameMethods);
        }









    }

    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        public ITracer Tracer => _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            _bar.InnerMethod();
            _tracer.StopTrace();
        }

    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(30);
            _tracer.StopTrace();
        }
    }
}

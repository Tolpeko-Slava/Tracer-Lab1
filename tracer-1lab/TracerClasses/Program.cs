using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracerLib.TracerResult;
using TracerLib.Serialization;
using TracerLib.Tracers;
using System.Threading;
using System.IO;


namespace ConsoleApp
{
    class Program
    {
        private static readonly ITracer tracer = new TracerReal();

        static void Main(string[] args)
        {

            Foo foo = new Foo(tracer);
            Thread t1 = new Thread(foo.MyMethod);
            Thread t2 = new Thread(foo.MyMethod);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();


            Thread myThread = new Thread(new ThreadStart(MyMethode2));
            myThread.Start(); // запускаем поток
            myThread.Join();

            tracer.StartTrace();
            MyMethode();
            MyMethode();
            Thread.Sleep(500);
            tracer.StopTrace();

            TraceResult resultTracer = tracer.GetTraceResult();

            var xmlResultSerializer = new XMLTraceSerializer();
            var jsonResultSerializer = new JsonTraceSerializer();

            string jsonRes = jsonResultSerializer.Serialize(resultTracer),
                    xmlRes = xmlResultSerializer.Serialize(resultTracer);

            ResultWriter writer = new ResultWriter(Console.OpenStandardOutput());
            writer.WriteResults(xmlRes);
            writer.WriteResults("\n\n");
            writer.WriteResults(jsonRes);
          
            OutputResultsInFile("res.xml", xmlRes);
            OutputResultsInFile("res.json", jsonRes);
            
            Console.ReadLine();
            
        }


        static void MyMethode()
        {
            tracer.StartTrace();
            Thread.Sleep(500);
            MyMethode2();
            tracer.StopTrace();

        }

        static void MyMethode2()
        {

            tracer.StartTrace();
            Thread.Sleep(23);
            tracer.StopTrace();

        }

        static void OutputResultsInFile(string filepath,string results)
        {
            ResultWriter writer = new ResultWriter();
            writer.WriteResults(results);
            FileStream f = null;
            try
            {
                f = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                writer.OutputStream = f;
                writer.WriteResults(results);

            }
            finally
            {
                if (f != null)
                    f.Close();
            }
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
            Thread thread = new Thread(_bar.InnerMethod);
            thread.Start();
            thread.Join();
            Thread.Sleep(10);
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
            Thread.Sleep(20);
            _tracer.StopTrace();
        }
    }


}

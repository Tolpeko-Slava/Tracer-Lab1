using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ConsoleApp
{
    class ResultWriter
    {
        private Stream outputStream;

        public ResultWriter(Stream OutputStream )
        {
            outputStream = OutputStream;
        }

        public ResultWriter()
        {
            outputStream = OutputStream;
        }

        public Stream OutputStream
        {
            get
            {
                return outputStream;
            }
            set
            {
                outputStream = value;
            }
        }

        public void WriteResults(string result)
        {
            if (outputStream == null)
                return;
            if (outputStream.CanWrite)
            {
                var bytes = Encoding.UTF8.GetBytes(result);
                outputStream.Write(bytes,0,bytes.Length);


            }

        }


    }
}

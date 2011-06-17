using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LTGSimulator
{
    public class LTGReaderWriter
    {
        private StreamReader _iStream;
        private StreamWriter _oStream;

        public void SetStreams(Stream inputStream, Stream outputStream)
        {
            _iStream = new StreamReader(inputStream);
            _oStream = new StreamWriter(outputStream);
        }


    }
}

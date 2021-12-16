using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVisitAssesment
{
    public class EVisitLogger
    {
        public void Info(string message) => Console.WriteLine(message);
        public void Error(string message) => Console.WriteLine(message);
        public void Debug(string message) => Console.WriteLine(message);
        public void Warn(string message) => Console.WriteLine(message);
    }
}

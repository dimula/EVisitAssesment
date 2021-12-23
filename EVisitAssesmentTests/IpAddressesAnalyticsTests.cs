using Microsoft.VisualStudio.TestTools.UnitTesting;
using EVisitAssesment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace EVisitAssesment.Tests
{
    [TestClass()]
    public class IpAddressesAnalyticsTests
    {
        IpAddressesAnalytics ipAddressesAnalytics;

        [TestInitialize]
        public void Init()
        {
            //it has to be mock for logger
            EVisitLogger logger = new EVisitLogger();
            ipAddressesAnalytics = new IpAddressesAnalytics(logger, 100);
        }

        void GenerateRandomRequests(int count, int dispersion)
        {
            Random rnd = new Random();
            for (var i = 0; i < count; i++)
            {
                var ip = ipAddressesAnalytics.IpToString((uint)rnd.Next(dispersion));
                ipAddressesAnalytics.request_handled(ip);
            }
        }

        [TestMethod()]
        public void request_handledTest()
        {
            GenerateRandomRequests(1000, 200);

            var res = ipAddressesAnalytics.top100();
            Assert.AreEqual(100, res.Count);
        }

        [TestMethod()]
        public void IpToStringTest()
        {
            var str1 = "192.168.0.10";
            var ip = ipAddressesAnalytics.IpToInt(str1);
            var str2 = ipAddressesAnalytics.IpToString(ip);
            Assert.AreEqual(str1, str2);
        }

        [TestMethod()]
        public void RecalculateTopIpsLoadTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            //create 20.000.000 records
            GenerateRandomRequests(20 * 1000 * 1000, 20 * 1000 * 1000);

            //execution time measurement
            var res = ipAddressesAnalytics.top100();
            sw.Stop();

            Console.WriteLine("Elapsed (mls)={0}", sw.Elapsed.Milliseconds);
            Assert.AreEqual(100, res.Count);
        }
    }
}
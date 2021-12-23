using Microsoft.VisualStudio.TestTools.UnitTesting;
using EVisitAssesment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVisitAssesment.Tests
{
    [TestClass()]
    public class TopIPsHeapTests
    {
        [TestMethod()]
        public void TopIPsHeapTest()
        {
            var heap = new TopIPsHeap(5);

            heap.Add(2, "ip2");
            heap.Add(5, "ip5");
            heap.Add(1, "ip1");
            heap.Add(3, "ip3");
            heap.Add(8, "ip8");
            heap.Add(33, "ip3");
            heap.Add(333, "ip3");
            heap.Add(7, "ip7");

            Test(heap);
        }

        private static void Test(TopIPsHeap heap)
        {
            //Assert.AreEqual(1, heap.Pop().Counter);
            var item = heap.Pop();
            Assert.AreEqual(2, item.Counter);
            Assert.AreEqual("ip2", item.IP);

            item = heap.Pop();
            Assert.AreEqual(5, item.Counter);
            Assert.AreEqual("ip5", item.IP);

            item = heap.Pop();
            Assert.AreEqual(7, item.Counter);
            Assert.AreEqual("ip7", item.IP);

            item = heap.Pop();
            Assert.AreEqual(8, item.Counter);
            Assert.AreEqual("ip8", item.IP);

            item = heap.Pop();
            Assert.AreEqual(333, item.Counter);
            Assert.AreEqual("ip3", item.IP);
        }

        [TestMethod()]
        public void GetSortedCollectionTest()
        {
            var heap = new TopIPsHeap(5);

            heap.Add(2, "ip2");
            heap.Add(5, "ip5");
            heap.Add(1, "ip1");
            heap.Add(3, "ip3");
            heap.Add(8, "ip8");
            heap.Add(33, "ip3");
            heap.Add(333, "ip3");
            heap.Add(7, "ip7");

            var list = heap.GetSortedCollection();
            Assert.AreEqual("ip2", list[0]);
            Assert.AreEqual("ip5", list[1]);
            Assert.AreEqual("ip7", list[2]);
            Assert.AreEqual("ip8", list[3]);
            Assert.AreEqual("ip3", list[4]);

            Test(heap);
        }
    }
}
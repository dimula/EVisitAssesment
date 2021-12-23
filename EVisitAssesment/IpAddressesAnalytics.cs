using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace EVisitAssesment
{
    public class IpAddressesAnalytics
    {
        Object lockObj = new Object();

        //ConcurrentDictionary is Thread-Safe Collection
        ConcurrentDictionary<uint, int> ipCollection = new ConcurrentDictionary<uint, int>();

        EVisitLogger logger;

        TopIPsHeap topIPsHeap;

        //Inject logger in constructor for better Unit Testing
        public IpAddressesAnalytics(EVisitLogger logger, int topIPsCollectionSize)
        {
            this.logger = logger;
            logger.Info("IpAddressesAnalytics starting.");

            this.topIPsHeap = new TopIPsHeap(topIPsCollectionSize);
        }

        public uint IpToInt(string address)
        {
            return (uint)IPAddress.NetworkToHostOrder(
                 (int)IPAddress.Parse(address).Address);
        }

        public string IpToString(uint address)
        {
            return IPAddress.Parse(address.ToString()).ToString();
        }

        public void request_handled(string ip_address)
        {
            var ip = IpToInt(ip_address);

            //concurret collection lets us to forget about multi-threading
            if (!ipCollection.ContainsKey(ip))
            {
                ipCollection[ip] = 0;
            }

            var counter = ipCollection[ip] + 1;
            ipCollection[ip] = counter;

            lock (lockObj)
            {
                topIPsHeap.Add(counter, ip_address);
            }
        }

        public List<string> top100()
        {
            logger.Info("top100() was called.");
            List<string> list;
            lock (lockObj)
            {
                list = topIPsHeap.GetSortedCollection();
            }
            list.Reverse();
            return list;
        }

        public void clear()
        {
            logger.Info("clear() was called.");
            topIPsHeap.Clear();
            ipCollection.Clear();
        }
    }
}

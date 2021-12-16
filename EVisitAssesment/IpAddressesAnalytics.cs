using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Timers;
using ConcurrentPriorityQueue;

namespace EVisitAssesment
{
    public class IpAddressesAnalytics : IDisposable
    {
        const int TOP_IPS_COLLECTION_SIZE = 100;

        //ConcurrentDictionary is Thread-Safe Collection
        ConcurrentDictionary<uint, int> ipCollection = new ConcurrentDictionary<uint, int>();

        List<string> topIpsCollection = new List<string>();

        Timer timer;

        EVisitLogger logger;

        //Inject logger in constructor for better Unit Testing
        public IpAddressesAnalytics(EVisitLogger logger, double topIpsUpdateInterval = 1000 * 60)
        {
            this.logger = logger;
            logger.Info("IpAddressesAnalytics starting.");

            timer = new Timer(topIpsUpdateInterval);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            // Only raise the event the first time Interval elapses
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
        }

        public void Dispose()
        {
            logger.Info("IpAddressesAnalytics disposing.");
            // stop the timer when object is disposing
            timer.Enabled = false;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //TimerEvent is happening in background thread
            logger.Info("OnTimedEvent.");
            topIpsCollection = RecalculateTopIps();
            timer.Start();
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

        public List<string> RecalculateTopIps()
        {
            //Heap-based implementation of concurrent, fixed-size priority queue. Max priority is on top of the heap.
            var priorityQueue = new ConcurrentFixedSizePriorityQueue<uint, int>(TOP_IPS_COLLECTION_SIZE);
            foreach (var item in this.ipCollection)
            {
                priorityQueue.Enqueue(item.Key, item.Value);
            }

            var list = priorityQueue.Select(x => IpToString(x)).ToList();
            logger.Info($"topIpsCollection recalculated. Count={list.Count}");
            return list;
        }

        public void request_handled(string ip_address)
        {
            var ip = IpToInt(ip_address);

            //concurret collection lets us to forget about multi-threading
            if (!ipCollection.ContainsKey(ip))
            {
                ipCollection[ip] = 0;
            }

            ipCollection[ip] += 1;
        }

        public List<string> top100()
        {
            logger.Info("top100() was called.");
            //this collections updated periodically, by timer
            return this.topIpsCollection;
        }

        public void clear()
        {
            logger.Info("clear() was called.");
            topIpsCollection.Clear();
            ipCollection.Clear();
        }
    }
}

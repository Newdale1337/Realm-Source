using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Externals.Models.FirestoreModels;
using Externals.Resources;
using Externals.Utilities;
using Google.Api;

namespace Externals.Database
{
    /// <summary>
    /// Possibly usable in the future...
    /// </summary>
    [Obsolete]
    public class CachedCollections
    {
        private Thread DeliverDataThread { get; set; }
        public List<LoginModel> Logins { get; set; }
        public bool DeliverCache { get; set; }
        private ManualResetEvent WaitEvent { get; set; }
        private int DeliveryTimeout { get; set; }
        private Queue<FirestoreModel> DeliveryQueues { get; set; }
        public VirtualServerDatabase Database { get; set; }


        public CachedCollections()
        {
            //DeliveryTimeout = 10;
            //DeliveryQueues = new Queue<FirestoreModel>();
            //DeliverCache = true;

            //DeliverDataThread = new Thread(DeliverData);
            //DeliverDataThread.Start();
        }

        [Obsolete]
        public void CacheData<T>(T t) => DeliveryQueues.Enqueue(t as FirestoreModel);

        public void DeliverData()
        {
            WaitEvent = new ManualResetEvent(false);
            using (VirtualServerDatabase db = Database)
            {
                LoggingUtils.LogIfDebug("Database deliver thread setup and ready to use");

                while (DeliverCache)
                {
                    WaitEvent.WaitOne(DeliveryTimeout * 200);
                    int count = 0;

                    for (int i = 0; i < DeliveryQueues.Count / 2; i++)
                    {
                        var type = DeliveryQueues.Dequeue();
                        type.UpdateDocument(db);
                        count++;
                    }

                    LoggingUtils.LogIfDebug($"Done delivering data to database waiting {DeliveryTimeout} minutes ({count} cached documents)");
                }
            }
        }
    }
}
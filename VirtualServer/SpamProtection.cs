using System;
using System.Collections.Generic;
using System.Linq;
using Anna.Request;
using Externals.Utilities;

namespace VirtualServer
{
    /// <summary>
    /// Unfinished VirtualServer spam protection.
    /// </summary>
    public sealed class SpamProtection
    {
        public Dictionary<string, IpSpamFilter> IpSpamCollection { get; set; }
        public int TimeoutMinutes { get; set; }
        public int RefreshFirstRequest { get; set; }
        public int MaxRequests { get; set; }

        public SpamProtection()
        {
            IpSpamCollection = new Dictionary<string, IpSpamFilter>();
            TimeoutMinutes = 600 / 60;
            RefreshFirstRequest = 1;
            MaxRequests = 10;
        }

        public bool IsSpam(RequestContext c)
        {
            IpSpamFilter filter;
            string ip = c.Request.GetIp();

            if (!IpSpamCollection.TryGetValue(ip, out filter))
            {
                filter = new IpSpamFilter();
                filter.Count = 0;
                filter.FirstRequest = DateTime.UtcNow;
                IpSpamCollection.Add(ip, filter);

                return true;
            }

            int deltaMinutes = (DateTime.Now - filter.FirstRequest).Minutes;
            TimeSpan suspendedDelta = (DateTime.UtcNow - filter.SupsendedTime);

            if (filter.Suspended && suspendedDelta.Minutes <= TimeoutMinutes)
            {
                c.WriteError($"Ip is suspended for {(TimeoutMinutes * 60) - suspendedDelta.Seconds}s");
                return false;
            }
            else if (filter.Suspended && suspendedDelta.Minutes >= TimeoutMinutes)
            {
                filter.Count = 0;
                filter.Suspended = false;
                filter.FirstRequest = DateTime.UtcNow;
                return true;
            }

            if (deltaMinutes >= RefreshFirstRequest)
            {
                filter.FirstRequest = DateTime.UtcNow;
                filter.Count = 0;

                return true;
            }

            if (filter.Count > MaxRequests && deltaMinutes <= TimeoutMinutes)
            {
                filter.Suspended = true;
                filter.SupsendedTime = DateTime.UtcNow;
                c.WriteError($"Account is suspended for {(TimeoutMinutes * 60) - suspendedDelta.Seconds}s");
                LoggingUtils.LogWarningIfDebug($"Spam filter suspended @{c.Request.GetIp()} from sending requests for {TimeoutMinutes} min.");

                return false;
            }

            filter.Count++;
            return true;
        }
    }

    public class IpSpamFilter
    {
        public int Count { get; set; }
        public bool Suspended { get; set; }
        public DateTime FirstRequest { get; set; }
        public DateTime SupsendedTime { get; set; }
    }
}

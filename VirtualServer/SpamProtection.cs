using System;
using System.Collections.Generic;
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
        public int Timeout { get; set; }
        public int RefreshFirstRequest { get; set; }
        public int MaxRequests { get; set; }

        public SpamProtection()
        {
            IpSpamCollection = new Dictionary<string, IpSpamFilter>();
            Timeout = 20;
            RefreshFirstRequest = 1;
            MaxRequests = 10;
        }

        public bool IsSpam(RequestContext c)
        {
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

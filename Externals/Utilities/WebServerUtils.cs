using Anna.Request;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Externals.Utilities
{
    public static class WebServerUtils
    {
        public static string GetIp(this Request r)
        {
            if (r.Headers.ContainsKey("X-Forwarded-For"))
            {
                return r.Headers["X-Forwarded-For"].Last();
            }

            if (r.Headers.ContainsKey("remote_addr"))
            {
                return r.Headers["remote_addr"].Last();
            }

            return r.ListenerRequest.RemoteEndPoint?.Address.ToString();
        }

        public static void WriteError(this RequestContext r, string errorDescription, int errorCode = 404) => r.Respond($"<Error>{errorDescription}</Error>", errorCode);
    }
}
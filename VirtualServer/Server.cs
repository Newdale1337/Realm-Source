using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using Anna;
using Anna.Request;
using Anna.Responses;
using Externals.Database;
using Externals.Settings;
using Externals.Utilities;
using VirtualServer;

namespace VirtualServer
{
    public class Server
    {
        private Dictionary<string, WebRequest> WebRequests { get; set; }
        private Stopwatch Timer { get; set; }
        private ManualResetEvent KeepAlive { get; set; }
        private SpamProtection SpamProtection { get; set; }
        public VirtualServerDatabase Database { get; set; }

        public Server()
        {
            Initialize();
            Load();
            Start();
        }

        private void Initialize()
        {
            Timer = Stopwatch.StartNew();
            SpamProtection = new SpamProtection();
            WebRequests = new Dictionary<string, WebRequest>();
            KeepAlive = new ManualResetEvent(false);
            Database = new VirtualServerDatabase();
        }

        private void Load()
        {
            LoadRequests();
        }

        public void LoadRequests()
        {
            foreach (var type in Assembly.GetAssembly(typeof(Program)).GetTypes().Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(WebRequest)) && _.GetConstructor(Type.EmptyTypes) != null))
            {
                var webRequest = (WebRequest)Activator.CreateInstance(type);
                WebRequests.AddOrUpdate(webRequest.RequestType, webRequest);
            }
        }

        public void Start()
        {
            using (HttpServer server = new HttpServer($"http://{VirtualServerSettings.IpAddress}:{VirtualServerSettings.Port}/"))
            {
                server.GET("/").Subscribe(Response);

                foreach (var request in WebRequests)
                {
                    server.POST(request.Key).Subscribe(Response);
                    server.GET(request.Key).Subscribe(Response);
                }

                LoggingUtils.LogIfDebug($"Requests loaded ({WebRequests.Count})");
                LoggingUtils.LogIfDebug($"VirtualServer bound to http://{VirtualServerSettings.IpAddress}:{VirtualServerSettings.Port}/");
                LoggingUtils.LogIfDebug($"VirtualServer fully started {{{Timer.Elapsed.Seconds}s {Timer.Elapsed.Milliseconds}ms}}");
                KeepAlive.WaitOne();
            }
        }

        private void Response(RequestContext context)
        {
            try
            {
                if (!Database.Loaded)
                {
                    context.Respond($"<Error>Server is loading up.</Error>", 500);
                    return;
                }
                if (!SpamProtection.IsSpam(context)) return;

                LoggingUtils.LogIfDebug($"Handling '{context.Request.Url.LocalPath}' @{context.Request.GetIp()}");
                GetBody(context.Request, 4096).Subscribe(body =>
                {
                    try
                    {
                        var request = GetRequest(context.Request.Url.LocalPath, context);

                        request.Database = Database;
                        request?.Handle(context, HttpUtility.ParseQueryString(body));
                        request = null;
                    }
                    catch (Exception e)
                    {
                        OnError(e, context);
                    }
                });
            }
            catch (Exception e)
            {
                OnError(e, context);
            }
        }

        private WebRequest GetRequest(string requestType, RequestContext r)
        {
            if (!WebRequests.TryGetValue(requestType, out WebRequest request))
            {
                LoggingUtils.LogErrorIfDebug($"Error handling {requestType} request not found.");
                r.WriteError("Not found");
                return null;
            }

            return request;
        }

        private void OnError(Exception e, RequestContext context)
        {
            LoggingUtils.LogErrorIfDebug($"{e.Message}{Environment.NewLine}{e.StackTrace}");

            try
            {
                context.Respond($"<Error>Internal server error</Error>", 500);
            }
            catch (Exception exception)
            {
                LoggingUtils.LogIfDebug(exception.Message);
            }
        }

        private IObservable<string> GetBody(Request r, int maxContentLength = 50000)
        {
            int bufferSize = maxContentLength;
            if (r.Headers.ContainsKey("Content-Length"))
                bufferSize = Math.Min(maxContentLength, int.Parse(r.Headers["Content-Length"].First()));

            var buffer = new byte[bufferSize];
            return Observable.FromAsyncPattern<byte[], int, int, int>(r.InputStream.BeginRead, r.InputStream.EndRead)(buffer, 0, bufferSize)
                .Select(bytesRead => r.ContentEncoding.GetString(buffer, 0, bytesRead));
        }
    }
}
using System.Collections.Specialized;
using Anna.Request;
using Anna.Responses;
using Externals.Database;
using Externals.Utilities;

namespace VirtualServer
{
    public abstract class WebRequest
    {
        public abstract string RequestType { get; }

        public VirtualServerDatabase Database { get; set; }
        protected void WriteXml(RequestContext requestContext, string xml) => Write(requestContext.Response(xml), "application/xml");
        protected void WriteXml(RequestContext requestContext, byte[] bytes) => Write(requestContext.Response(bytes), "application/xml");
        protected void WriteJSON(RequestContext requestContext, byte[] bytes) => Write(requestContext.Response(bytes), "application/json");

        protected void Success(RequestContext requestContext) => Write(requestContext.Response("<Success/>"), "text/plain");

        protected void Write(RequestContext requestContext, byte[] bytes) => Write(requestContext.Response(bytes), "text/plain");
        protected void Write(RequestContext requestContext, string type, bool error = false, bool fatalError = false) => Write(requestContext.Response($"{(error ? $"<Error>{type}</Error>" : fatalError ? $"<FatalError>{type}</FatalError>" : type)}"), "text/plain");
        protected void Write(Response response, string type)
        {
            response.Headers["Content-Type"] = type;
            response.Headers["Access-Control-Allow-Origin"] = "*";
            response.Send();
        }

        public virtual void Handle(RequestContext requestContext, NameValueCollection query)
        {
            LoggingUtils.LogWarningIfDebug($"'{requestContext.Request.Url.LocalPath}' does not have a handler @{requestContext.Request.GetIp()}");
        }
    }
}
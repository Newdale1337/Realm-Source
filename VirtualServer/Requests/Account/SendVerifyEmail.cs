using System;
using System.Collections.Specialized;
using Anna.Request;
using Externals.Utilities;

namespace VirtualServer.Requests.Account
{
    public class SendVerifyEmail : WebRequest
    {
        public override string RequestType => "/account/sendVerifyEmail";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            LoggingUtils.LogIfDebug($"{query["guid"]} is requesting email verification mail.");
        }
    }
}
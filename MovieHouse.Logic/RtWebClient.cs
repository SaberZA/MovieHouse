using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SalesServiceHost.Common
{
    public class RtWebClient : WebClient
    {
        public RtWebClient(bool useProxy)
        {
            if (useProxy)
                SetLocalProxy();
        }

        public RtWebClient(){}

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }

        private void SetLocalProxy()
        {
            this.Proxy = new WebProxy("proxy", 8080){UseDefaultCredentials = true};
        }
    }
}
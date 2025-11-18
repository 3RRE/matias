using System;
using System.Net;
using System.Threading;

namespace CapaPresentacion.Utilitarios
{
    public class MyWebClientInfinite : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest webRequest = base.GetWebRequest(uri);
            webRequest.Timeout = Timeout.Infinite;

            return webRequest;
        }
    }
}
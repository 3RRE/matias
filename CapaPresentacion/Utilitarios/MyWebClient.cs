using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CapaPresentacion.Utilitarios
{
    public class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 300000;//5 minutos, 300000 milisegundos
            return w;
        }
    }
}
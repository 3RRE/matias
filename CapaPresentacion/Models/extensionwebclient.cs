using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CapaPresentacion.Models
{
    public class extensionwebclient
    {
    }
    public class WebDownload : WebClient
    {
        /// <summary> 
        /// Time in milliseconds 
        /// </summary> 
        public int Timeout { get; set; }

        public WebDownload() : this(60000) { }

        public WebDownload(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}
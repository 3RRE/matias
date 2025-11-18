using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;

namespace CapaPresentacion.Utilitarios
{
    public class CheckPortHelper
    {
        private string UrlAlternative { get; set; }

        public class TcpConnection
        {
            public string Url { get; set; }
            public bool IsOpen { get; set; }
            public bool IsVpn { get; set; }
        }

        public CheckPortHelper()
        {
            string urlAlternative = ConfigurationManager.AppSettings["VPN_UrlAlternative"] != null ? ConfigurationManager.AppSettings["VPN_UrlAlternative"].ToString() : "";

            UrlAlternative = RegexIsMatchUri(urlAlternative);
        }

        public bool TcpConnect(string host, int port)
        {
            bool isOpen = false;

            try
            {
                TcpClient tcpClient = new TcpClient();

                Task task = tcpClient.ConnectAsync(host, port);
                task.Wait(1000);

                if (tcpClient.Connected)
                {
                    isOpen = true;
                }

                tcpClient.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return isOpen;
        }

        public List<TcpConnection> TcpUrls(List<string> urls)
        {
            List<TcpConnection> list = new List<TcpConnection>();

            try
            {
                foreach(string url in urls)
                {
                    Uri uri = new Uri(url);

                    bool isOpen = TcpConnect(uri.Host, uri.Port);

                    list.Add(new TcpConnection
                    {
                        Url = url,
                        IsOpen = isOpen,
                        IsVpn = false
                    });
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return list;
        }

        public TcpConnection TcpUrl(string url, bool isVpn = false)
        {
            TcpConnection tcpConnection = new TcpConnection();

            try
            {
                Uri uri = new Uri(url);

                bool isOpen = TcpConnect(uri.Host, uri.Port);

                tcpConnection.Url = url;
                tcpConnection.IsOpen = isOpen;
                tcpConnection.IsVpn = isVpn;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return tcpConnection;
        }

        public TcpConnection TcpUrlMultiple(string url, List<string> urls)
        {
            TcpConnection tcpConnection = new TcpConnection();

            try
            {
                tcpConnection = TcpUrl(url);

                if (!tcpConnection.IsOpen)
                {
                    if (ValidHttpURL(UrlAlternative))
                    {
                        urls.Insert(0, UrlAlternative);
                    }

                    List<string> uurls = urls.Where(item => !string.IsNullOrEmpty(item) && !item.Equals(url)).Distinct().ToList();

                    foreach (string uurl in uurls)
                    {
                        tcpConnection = TcpUrl(uurl, true);

                        if (tcpConnection.IsOpen)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return tcpConnection;
        }

        public bool ValidHttpURL(string uri)
        {
            if (Uri.TryCreate(uri, UriKind.Absolute, out Uri resultUri))
            {
                return (resultUri.Scheme == Uri.UriSchemeHttp || resultUri.Scheme == Uri.UriSchemeHttps);
            }

            return false;
        }

        public string RegexIsMatchUri(string uri)
        {
            if (!Regex.IsMatch(uri, @"^https?:\/\/", RegexOptions.IgnoreCase))
            {
                uri = "http://" + uri;
            }

            return uri;
        }
    }
}
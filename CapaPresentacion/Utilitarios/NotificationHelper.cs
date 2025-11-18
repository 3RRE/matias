using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace CapaPresentacion.Utilitarios
{
    public class NotificationHelper
    {
        private readonly string _FIREBASE_SERVICE_KEY = ConfigurationManager.AppSettings["firebaseServiceKey"];

        public string SendToFirebase(string[] deviceToken, string title, string body)
        {
            string sResponseFromServer = string.Empty;

            try
            {
                WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", _FIREBASE_SERVICE_KEY));
                webRequest.Headers.Add(string.Format("Sender: id={0}", string.Join(",", deviceToken)));
                webRequest.ContentType = "application/json";

                object payload = new
                {
                    registration_ids = deviceToken,
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        title,
                        body,
                        badge = 1
                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();

                byte[] byteArray = Encoding.UTF8.GetBytes(postbody);

                webRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null)
                            {
                                using (StreamReader streamReader = new StreamReader(dataStreamResponse))
                                {
                                    sResponseFromServer = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return sResponseFromServer;
        }
    }
}
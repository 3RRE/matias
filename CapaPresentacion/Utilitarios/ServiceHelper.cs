using CapaEntidad;
using CapaEntidad.Response;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilitarios
{
    public class ServiceHelper
    {
        public async Task<ResultEntidad> PostAsync(string requestUri, string content)
        {
            ResultEntidad resultEntidad = new ResultEntidad();

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                    httpClient.Timeout = TimeSpan.FromMinutes(15);
                    using (HttpResponseMessage httpResponse = await httpClient.PostAsync(requestUri, httpContent))
                    {
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            string result = await httpResponse.Content.ReadAsStringAsync();

                            JObject objectData = JObject.Parse(result);

                            resultEntidad = new ResultEntidad
                            {
                                success = Convert.ToBoolean(objectData["success"]),
                                message = Convert.ToString(objectData["message"]),
                                data = Convert.ToString(objectData["data"])
                            };
                        }
                        else
                        {
                            resultEntidad = new ResultEntidad
                            {
                                success = false,
                                message = httpResponse.ReasonPhrase,
                                data = string.Empty
                            };
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                resultEntidad = new ResultEntidad
                {
                    success = false,
                    message = exception.Message,
                    data = string.Empty
                };
            }

            return resultEntidad;
        }

        public async Task<ResultEntidad> PostFileAsync(string requestUri, string filePath, string fileName) {
            ResultEntidad resultEntidad = new ResultEntidad();

            try {

                using(var httpClient = new HttpClient()) {
                    using(var httpContent = new MultipartFormDataContent()) {
                        // Abre el archivo como FileStream
                        using(var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                            var fileContent = new StreamContent(fileStream);
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                            // Agrega el contenido del archivo al multipart/form-data
                            httpContent.Add(fileContent, "file", fileName);

                            var httpResponse = await httpClient.PostAsync(requestUri, httpContent);

                            if(httpResponse.IsSuccessStatusCode) {
                                string result = await httpResponse.Content.ReadAsStringAsync();

                                JObject objectData = JObject.Parse(result);

                                resultEntidad = new ResultEntidad {
                                    success = Convert.ToBoolean(objectData["success"]),
                                    message = Convert.ToString(objectData["message"]),
                                    data = Convert.ToString(objectData["data"])
                                };
                            } else {
                                resultEntidad = new ResultEntidad {
                                    success = false,
                                    message = httpResponse.ReasonPhrase,
                                    data = string.Empty
                                };
                            }
                        }
                    }
                }

            } catch(Exception exception) {
                resultEntidad = new ResultEntidad {
                    success = false,
                    message = exception.Message,
                    data = string.Empty
                };
            }

            return resultEntidad;
        }

    }
}
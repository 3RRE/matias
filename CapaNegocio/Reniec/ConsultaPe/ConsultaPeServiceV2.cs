using CapaEntidad;
using CapaEntidad.Reniec.Request.ConsultaPe;
using CapaEntidad.Reniec.Response.ConsultaPe;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Reniec.ConsultaPe {
    public class ConsultaPeServiceV2 {
        private readonly HttpClient _httpClient;

        public ConsultaPeServiceV2(API_ReniecKeysEntidad key) {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = new Uri(key.ApiURL);
            _httpClient.MaxResponseContentBufferSize = int.MaxValue;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key.ApiKey);
        }

        public async Task<ResponseConsultaPeV2<ResponsePersonaConsultaPe>> BuscarPersona(RequestPersonaConsultaPe request) {
            ResponseConsultaPeV2<ResponsePersonaConsultaPe> response = new ResponseConsultaPeV2<ResponsePersonaConsultaPe>();

            const string url = "v2/consulta/personas";
            object payload = new {
                dni = request.NumeroDocumento
            };

            try {
                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseServer = await _httpClient.PostAsync(url, content);
                string contentResponse = await responseServer.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ResponseConsultaPeV2<ResponsePersonaConsultaPe>>(contentResponse) ?? new ResponseConsultaPeV2<ResponsePersonaConsultaPe>();
            } catch(Exception ex) {
                Console.WriteLine($"Error al consultar Consulta.Pe V2: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseConsultaPeV2<ResponseExtranjeroConsultaPe>> BuscarExtranjero(RequestExtranjeroConsultaPe request) {
            ResponseConsultaPeV2<ResponseExtranjeroConsultaPe> response = new ResponseConsultaPeV2<ResponseExtranjeroConsultaPe>();

            const string url = "v2/consulta/extranjeros";
            object payload = new {
                document_number = request.NumeroDocumento,
                birthdate = request.FechaNacimiento.ToString("yyyy-MM-dd")
            };

            try {
                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseServer = await _httpClient.PostAsync(url, content);
                string contentResponse = await responseServer.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ResponseConsultaPeV2<ResponseExtranjeroConsultaPe>>(contentResponse) ?? new ResponseConsultaPeV2<ResponseExtranjeroConsultaPe>();
            } catch(Exception ex) {
                Console.WriteLine($"Error al consultar Consulta.Pe V2: {ex.Message}");
            }

            return response;
        }
    }
}
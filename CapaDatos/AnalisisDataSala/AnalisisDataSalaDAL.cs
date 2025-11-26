

using CapaEntidad.AnalisisDataSala;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CapaDatos.AnalisisDataSala {
    public class AnalisisDataSalaDAL {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;

        public AnalisisDataSalaDAL() {
            // 1. Lee la URL de tu API desde el Web.config
            _apiBaseUrl = ConfigurationManager.AppSettings["ApiAnalisisSalaUrl"];

            // 2. Crea un cliente HttpClient (la forma antigua de MVC 5)
            // (En un proyecto moderno usaríamos IHttpClientFactory, pero esto respeta tu arquitectura)
            _httpClient = new HttpClient {
                BaseAddress = new Uri(_apiBaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Método genérico para llamadas GET
        private T GetApiData<T>(string endpointUrl) {
            // .Result es una mala práctica (bloquea el hilo), pero es común
            // en código DAL síncrono antiguo. La forma 100% correcta sería
            // hacer todo el stack async Task<> (DAL, BL, Controller).
            var response = _httpClient.GetAsync(endpointUrl).Result;

            if (response.IsSuccessStatusCode) {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(jsonString);
            } else {
                // Maneja el error
                var errorString = response.Content.ReadAsStringAsync().Result;
                throw new Exception($"Error de API ({response.StatusCode}): {errorString}");
            }
        }

        public IdleTimeKpiDto GetKpiGeneral(string codSala, DateTime fecha) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            string url = $"api/sala/{codSala}/idle-time/resumen-general?fecha={fechaString}";
            return GetApiData<IdleTimeKpiDto>(url);
        }

        public List<IdleTimePorHoraDto> GetUtilizacionPorHora(string codSala, DateTime fecha) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            string url = $"api/sala/{codSala}/idle-time/uso-total-por-hora?fecha={fechaString}";
            return GetApiData<List<IdleTimePorHoraDto>>(url);
        }

        public List<IdleTimeRankingDto> GetRankingMaquinas(string codSala, DateTime fecha) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            string url = $"api/sala/{codSala}/idle-time/resumen-por-maquina?fecha={fechaString}";
            return GetApiData<List<IdleTimeRankingDto>>(url);
        }

        public List<IdleTimeTimelineDto> GetTimelineMaquina(string codSala, DateTime fecha, string codMaq) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            string url = $"api/sala/{codSala}/idle-time/log-estado/{codMaq}?fecha={fechaString}";
            return GetApiData<List<IdleTimeTimelineDto>>(url);
        }

        // -----------------------------------------------------------------
        // --- NUEVOS MÉTODOS PARA HIT FREQUENCY ---
        // -----------------------------------------------------------------

        public HitFrecKpiDto GetHitFrecGeneral(string codSala, DateTime fecha) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            // Llama a: /api/sala/{codSala}/hit-frequency/resumen-general?fecha=...
            string url = $"api/sala/{codSala}/hit-frequency/resumen-general?fecha={fechaString}";
            return GetApiData<HitFrecKpiDto>(url);
        }

        public List<HitFrecRankingDto> GetHitFrecPorMaquina(string codSala, DateTime fecha) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            // Llama a: /api/sala/{codSala}/hit-frequency/resumen-por-maquina?fecha=...
            string url = $"api/sala/{codSala}/hit-frequency/resumen-por-maquina?fecha={fechaString}";
            return GetApiData<List<HitFrecRankingDto>>(url);
        }

        public List<HitFrecLogDto> GetHitFrecLogDetallado(string codSala, DateTime fecha, string codMaq) {
            string fechaString = fecha.ToString("yyyy-MM-dd");
            // Llama a: /api/sala/{codSala}/hit-frequency/log-detallado/{codMaq}?fecha=...
            string url = $"api/sala/{codSala}/hit-frequency/log-detallado/{codMaq}?fecha={fechaString}";
            return GetApiData<List<HitFrecLogDto>>(url);
        }
    }
}

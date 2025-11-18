using CapaDatos.Campaña;
using CapaEntidad;
using CapaEntidad.Campañas;
using CapaEntidad.Response;
using Newtonsoft.Json;
using S3k.Utilitario.Constants;
using S3k.Utilitario.Enum;
using S3k.Utilitario.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña {
    public class CMP_ClienteBL {
        private CMP_ClienteDAL cliente_dal = new CMP_ClienteDAL();
        private readonly HttpClient httpClient = new HttpClient();

        public List<CMP_ClienteEntidad> GetClientesCampaniaJson(Int64 campania_id) {
            return cliente_dal.GetClientesCampania(campania_id);
        }
        public List<CMP_ClienteEntidad> GetClientesCampaniaBuscar(string valor) {
            return cliente_dal.GetClientesCampaniaBuscar(valor);
        }
        public int GuardarClienteCampaniaJson(CMP_ClienteEntidad sala) {
            return cliente_dal.GuardarClienteCampania(sala);
        }
        public bool eliminarCampaniaClienteJson(Int64 id) {
            return cliente_dal.eliminarCampaniaCliente(id);
        }
        public List<CMP_ClienteEntidad> GetClientesCampaniaxCliente(string whereQuery, DateTime fechaIni, DateTime fechaFin) {
            return cliente_dal.GetClientesCampaniaxCliente(whereQuery, fechaIni, fechaFin);
        }

        public List<CMP_ClienteEntidad> ObtenerClientesDeCampaniaWhatsAppPorIdCampania(long idCampania) {
            return cliente_dal.ObtenerClientesDeCampaniaWhatsAppPorIdCampania(idCampania);
        }

        public CMP_ClienteEntidad ObtenerClienteDeCampaniaWhatsAppPorIdCampaniaNumeroDocumento(long idCampania, string numeroDocumento) {
            return cliente_dal.ObtenerClienteDeCampaniaWhatsAppPorIdCampaniaNumeroDocumento(idCampania, numeroDocumento);
        }

        #region Campaña WhatsApp
        public CMP_ClienteEntidad ObtenerClientePorCodigoPromocional(string promotionalCode) {
            return cliente_dal.ObtenerClientePorCodigoPromocional(promotionalCode);
        }

        public CMP_ClienteEntidad BuscarClienteExistenteCmpCliente(int idCliente, long idCampania) {
            return cliente_dal.BuscarClienteExistenteCMPCliente(idCliente, idCampania);
        }

        public CMP_ClienteEntidad ObtenerClientePorIdCampaniaCliente(long idCampaniaCliente) {
            return cliente_dal.ObtenerClientePorIdCampaniaCliente(idCampaniaCliente);
        }

        public bool ActualizarCelularCliente(CMP_ClienteEntidad cliente) {
            return cliente_dal.ActualizarCelularCliente(cliente) != 0;
        }

        public bool MarcarCodigoEnviado(long id) {
            return cliente_dal.MarcarCodigoEnviado(id);
        }

        public bool CanjearCodigoPromocional(long id, int codSala) {
            return cliente_dal.CanjearCodigoPromocional(id, codSala);
        }

        public bool ExisteCodigoPromocional(string promotionalCode) {
            return cliente_dal.ExisteCodigoPromocional(promotionalCode);
        }

        public string GenerarCodigoPromocional(int longitud = 6) {
            Random random = new Random();
            string code;

            do {
                code = GenerarCodigoAlfanumerico(random, longitud);
            } while(cliente_dal.ExisteCodigoPromocional(code));

            return code;
        }

        private string GenerarCodigoAlfanumerico(Random random, int longitud) {
            string code = string.Empty;
            for(int i = 0; i < longitud; i++) {
                var chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                int num = random.Next(0, chars.Length);
                code += chars[num];
            }
            return code;
        }

        public async Task<string> ObtenerProcedenciaRegistro(string provenance, string urlApiLanding) {
            string procedenciaRegistro = "Página Web";
            try {
                httpClient.BaseAddress = new Uri(urlApiLanding);
                string url = $"web/clientprovenance/{provenance}";
                var responseServer = await httpClient.GetAsync(url);
                if(responseServer.IsSuccessStatusCode) {
                    var contentResponse = await responseServer.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ResponseEntidad<CMP_ProcedenciaRegistroEntidad>>(contentResponse);
                    procedenciaRegistro = response.data.Name;
                }
            } catch {
                procedenciaRegistro = "Página Web";
            }
            return procedenciaRegistro;
        }

        public async Task<ResponseEntidad<ClienteVerificacionResponse>> VerificarClientePtkWigos(int idDocumentType, string documentNumber, string phoneNumber, int codSala) {
            ResponseEntidad<ClienteVerificacionResponse> response = new ResponseEntidad<ClienteVerificacionResponse>();

            DocumentTypePlayerTracking documentType = (DocumentTypePlayerTracking)idDocumentType;

            if(codSala == 9 || codSala == 65) {
                if(documentType == DocumentTypePlayerTracking.DocumentoIdentidad) {
                    idDocumentType = EnumHelper.GetDocumentTypePlayerTracking(DocumentTypePlayerTracking.DocumentoIdentidad);
                } else if(documentType == DocumentTypePlayerTracking.Pasarpote) {
                    idDocumentType = EnumHelper.GetDocumentTypePlayerTracking(DocumentTypePlayerTracking.CarneExtranjeria);
                } else if(documentType == DocumentTypePlayerTracking.CarneExtranjeria) {
                    idDocumentType = EnumHelper.GetDocumentTypePlayerTracking(DocumentTypePlayerTracking.Pasarpote);
                }
            }

            var data = new {
                idDocumentType,
                documentNumber,
                phoneNumber
            };

            try {
                string url = Constants.UrlVerificationClients[codSala];
                var jsonData = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var responseServer = await httpClient.PostAsync(url, jsonContent);
                var content = await responseServer.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ResponseEntidad<ClienteVerificacionResponse>>(content);
            } catch {
                response.success = false;
                response.data.clientExist = true;
                response.displayMessage = "Error al momento de realizar la verificación del cliente.";
            }
            return response;
        }
        #endregion
    }
}

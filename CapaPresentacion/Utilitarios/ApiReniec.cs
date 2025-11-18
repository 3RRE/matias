using CapaEntidad;
using CapaEntidad.Reniec.Request.ConsultaPe;
using CapaEntidad.Reniec.Response.ConsultaPe;
using CapaNegocio;
using CapaNegocio.Reniec.ConsultaPe;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilitarios {
    public class ApiReniec {
        private readonly API_ReniecKeys _reniecBL;
        private ConsultaPeServiceV2 _consultaPeServiceV2;

        public ApiReniec() {
            _reniecBL = new API_ReniecKeys();
        }

        public async Task<ApiReniecResponse> Busqueda(string dni) {
            List<API_ReniecKeysEntidad> listaKeys = _reniecBL.ListaKeys().OrderBy(x => x.Id).Where(x => x.Estado==1).ToList();
            bool respuestaLoop = false;
            ApiReniecResponse response = new ApiReniecResponse();

            foreach(API_ReniecKeysEntidad key in listaKeys) {
                string token = key.ApiKey;
                string uri = key.ApiURL;
                if(uri.ToLower().Contains("apisperu.com")) {
                    response= BusquedaApisPeru(dni, token);
                    if(response.Respuesta) {
                        response.API_USADA = API_USADA.APIS_PERU;
                        respuestaLoop = true;
                    }
                } else if(uri.ToLower().Contains("consulta.pe")) {
                    //consulta PE
                    response = BusquedaConsultaPe(dni, token);
                    if(response.Respuesta) {
                        response.API_USADA = API_USADA.CONSULTA_PE;
                        respuestaLoop = true;
                    }
                } else if(uri.ToLower().Contains("verifica.id")) {
                    response = dni.Length == 8 ? await BusquedaPersonaConsultaPeV2(key, dni) : await BusquedaExtranjeroConsultaPeV2(key, dni);
                    if(response.Respuesta) {
                        response.API_USADA = API_USADA.CONSULTA_PE_V2;
                        respuestaLoop = true;
                    }
                } else if(uri.ToLower().Contains("go.net.pe")) {
                    response = BusquedaApiDni(dni, token);
                    if(response.Respuesta) {
                        response.API_USADA = API_USADA.API_DNI;
                        respuestaLoop = true;
                    }
                }
                if(respuestaLoop) {
                    respuestaLoop = true; ;
                    break;
                }
            }
            return response;
        }

        private dynamic BusquedaApisPeru(string dni, string ApiKey) {
            ApiReniecResponse item = new ApiReniecResponse();
            string URL = $@"https://dniruc.apisperu.com/api/v1/dni/{dni}?token={ApiKey}";
            try {
                var client = new RestClient(URL);
                var request = new RestRequest(Method.GET);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);
                dynamic oData = JsonConvert.DeserializeObject(response.Content);
                if(oData != null) {
                    var tipoDato = oData.GetType();
                    if(tipoDato.FullName == "Newtonsoft.Json.Linq.JObject") {
                        if(oData.dni != null) {

                            item.NombreCompleto = oData.nombres + " " + oData.apellidoPaterno + " " + oData.apellidoMaterno;
                            item.DNI = oData.dni;
                            item.Nombre = oData.nombres;
                            item.ApellidoPaterno = oData.apellidoPaterno;
                            item.ApellidoMaterno = oData.apellidoMaterno;
                            item.Respuesta = true;
                        } else {
                            item.NombreCompleto = "Cliente No Encontrado";
                            item.ErrorMensaje = "Mensaje APISPERU: " + oData.message;

                        }
                    } else {
                        item.NombreCompleto = "Cliente No Encontrado";
                        item.ErrorMensaje = oData;
                    }
                } else {
                    item.NombreCompleto = "Cliente No Encontrado";
                    item.ErrorMensaje = "Error de Conexion a APISPERU.com";
                }
            } catch(Exception ex) {
                item = new ApiReniecResponse();
                item.NombreCompleto = "Cliente No Encontrado";
                item.ErrorMensaje = ex.Message;
            }
            return item;
        }

        private ApiReniecResponse BusquedaConsultaPe(string dni, string ApiKey) {
            ApiReniecResponse item = new ApiReniecResponse();
            const string URL = "https://consulta.pe/api/reniec/dni";

            try {
                var client = new RestClient(URL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + ApiKey);
                request.AddParameter("dni", dni);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);
                dynamic oData = JsonConvert.DeserializeObject(response.Content);
                if(oData != null) {
                    var tipoDato = oData.GetType();
                    if(tipoDato.FullName == "Newtonsoft.Json.Linq.JObject") {
                        if(oData.dni != null) {

                            item.NombreCompleto = oData.nombres + " " + oData.apellido_paterno + " " + oData.apellido_materno;
                            item.DNI = dni;
                            item.Nombre = oData.nombres;
                            item.ApellidoPaterno = oData.apellido_paterno;
                            item.ApellidoMaterno = oData.apellido_materno;
                            item.Respuesta = true;
                        } else {
                            item.NombreCompleto = "Cliente No Encontrado";
                            item.ErrorMensaje = "Mensaje CONSULTA.PE: " + oData.message;

                        }
                    } else {
                        item.NombreCompleto = "Cliente No Encontrado";
                        item.ErrorMensaje = oData;
                    }
                } else {
                    item.NombreCompleto = "Cliente No Encontrado";
                    item.ErrorMensaje = "Error de Conexion a Consulta.pe";
                }
            } catch(Exception ex) {
                item=new ApiReniecResponse();
                item.NombreCompleto = "Cliente No Encontrado";
                item.ErrorMensaje = ex.Message;
            }
            return item;
        }

        private async Task<ApiReniecResponse> BusquedaPersonaConsultaPeV2(API_ReniecKeysEntidad key, string dni) {
            ApiReniecResponse item = new ApiReniecResponse();

            try {
                _consultaPeServiceV2 = new ConsultaPeServiceV2(key);

                RequestPersonaConsultaPe request = new RequestPersonaConsultaPe() {
                    NumeroDocumento = dni
                };
                ResponseConsultaPeV2<ResponsePersonaConsultaPe> response = await _consultaPeServiceV2.BuscarPersona(request);
                item.Respuesta = response.data != null && response.data.Existe();
                if(item.Respuesta) {
                    item.NombreCompleto = response.data.nombre_completo;
                    item.DNI = response.data.dni;
                    item.Nombre = response.data.nombres;
                    item.ApellidoPaterno = response.data.apellido_paterno;
                    item.ApellidoMaterno = response.data.apellido_materno;
                } else {
                    item.NombreCompleto = response.message;
                    item.ErrorMensaje = "Mensaje CONSULTA.PE V2: " + response.message;
                }
            } catch(Exception ex) {
                item= new ApiReniecResponse();
                item.NombreCompleto = "Cliente No Encontrado";
                item.ErrorMensaje = ex.Message;
            }
            return item;
        }

        private async Task<ApiReniecResponse> BusquedaExtranjeroConsultaPeV2(API_ReniecKeysEntidad key, string numeroDocumento) {
            ApiReniecResponse item = new ApiReniecResponse();

            try {
                _consultaPeServiceV2 = new ConsultaPeServiceV2(key);

                RequestExtranjeroConsultaPe request = new RequestExtranjeroConsultaPe() {
                    NumeroDocumento = numeroDocumento,
                    FechaNacimiento = new DateTime(1999, 10, 12)
                };
                ResponseConsultaPeV2<ResponseExtranjeroConsultaPe> response = await _consultaPeServiceV2.BuscarExtranjero(request);
                item.Respuesta = response.data != null && response.data.Existe();
                if(item.Respuesta) {
                    item.NombreCompleto = response.data.nombre_completo;
                    item.DNI = response.data.numero_de_documento;
                    item.Nombre = response.data.nombres;
                    item.ApellidoPaterno = response.data.apellido_paterno;
                    item.ApellidoMaterno = response.data.apellido_materno;
                } else {
                    item.NombreCompleto = response.message;
                    item.ErrorMensaje = "Mensaje CONSULTA.PE EXTRANJERO V2: " + response.message;
                }
            } catch(Exception ex) {
                item= new ApiReniecResponse();
                item.NombreCompleto = "Cliente No Encontrado";
                item.ErrorMensaje = ex.Message;
            }
            return item;
        }

        private ApiReniecResponse BusquedaApiDni(string dni, string ApiKey) {
            ApiReniecResponse item = new ApiReniecResponse();
            string URL = $@"http://go.net.pe:3000/api/v2/dni/{dni}";
            try {
                var client = new RestClient(URL);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + ApiKey);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                IRestResponse response = client.Execute(request);
                dynamic oData = JsonConvert.DeserializeObject(response.Content);
                if(oData != null) {
                    var tipoDato = oData.GetType();
                    if(tipoDato.FullName == "Newtonsoft.Json.Linq.JObject") {
                        if(oData.codigo == "1") {
                            if(oData.data.dni != null) {
                                item.NombreCompleto = oData.data.nombres + " " + oData.data.apellido_paterno + " " + oData.data.apellido_materno;
                                item.DNI = dni;
                                item.Nombre = oData.data.nombres;
                                item.ApellidoPaterno = oData.data.apellido_paterno;
                                item.ApellidoMaterno = oData.data.apellido_materno;
                                item.Respuesta = true;
                            } else {
                                item.NombreCompleto = "Cliente No Encontrado";
                                item.ErrorMensaje = "Mensaje CONSULTA.PE: " + oData.respuesta;
                            }
                        } else {
                            item.NombreCompleto = "Cliente No Encontrado";
                            item.ErrorMensaje = oData.respuesta;
                        }
                    } else {
                        item.NombreCompleto = "Cliente No Encontrado";
                        item.ErrorMensaje = oData;
                    }
                } else {
                    item.NombreCompleto = "Cliente No Encontrado";
                    item.ErrorMensaje = "Error de Conexion a Consulta.pe";
                }
            } catch(Exception ex) {
                item = new ApiReniecResponse();
                item.NombreCompleto = "Cliente No Encontrado";
                item.ErrorMensaje = ex.Message;
            }
            return item;
        }
    }
}
using CapaDatos.MaquinasInoperativas;
using CapaEntidad.MaquinasInoperativas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.MaquinasInoperativas {
    public class MI_MaquinaInoperativaBL {

        MI_MaquinaInoperativaDAL capaDatos = new MI_MaquinaInoperativaDAL();

        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaxUsuario(int codUsuario)
        {
            return capaDatos.GetAllMaquinaInoperativaxUsuario(codUsuario);
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaxUsuarioxFechas(int codUsuario, DateTime fechaIni, DateTime fechaFin)
        {
            return capaDatos.GetAllMaquinaInoperativaxUsuarioxFechas(codUsuario,fechaIni,fechaFin);
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaxUsuarioxFechasxEstado(int codUsuario, DateTime fechaIni, DateTime fechaFin, int estado)
        {
            return capaDatos.GetAllMaquinaInoperativaxUsuarioxFechasxEstado(codUsuario,fechaIni,fechaFin, estado);
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaCreado(int codUsuario)
        {
            return capaDatos.GetAllMaquinaInoperativaCreado(codUsuario);
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaAtendidaOperativa(int codUsuario)
        {
            return capaDatos.GetAllMaquinaInoperativaAtendidaOperativa(codUsuario);
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaAtendidaInoperativa(int codUsuario)
        {
            return capaDatos.GetAllMaquinaInoperativaAtendidaInoperativa(codUsuario);
        }
        public List<MI_MaquinaInoperativaEntidad> GetAllMaquinaInoperativaAtendidaInoperativaSolicitud(int codUsuario)
        {
            return capaDatos.GetAllMaquinaInoperativaAtendidaInoperativaSolicitud(codUsuario);
        }
        public MI_MaquinaInoperativaEntidad MaquinaInoperativaCodObtenerJson(int cod) {
            return capaDatos.GetCodMaquinaInoperativa(cod);
        }
        public MI_MaquinaInoperativaEntidad MaquinaInoperativaCodHistoricoObtenerJson(int cod)
        {
            return capaDatos.GetCodMaquinaInoperativaHistorico(cod);
        }
        public int InsertarMaquinaInoperativaCreado(MI_MaquinaInoperativaEntidad Entidad) {
            var cod = capaDatos.InsertarMaquinaInoperativaCreado(Entidad);
            return cod;
        }
        public bool AtenderMaquinaInoperativaOperativaResuelto(MI_MaquinaInoperativaEntidad Entidad) {
            var status = capaDatos.AtenderMaquinaInoperativaOperativaResuelto(Entidad);
            return status;
        }
        public bool AtenderMaquinaInoperativaOperativa(MI_MaquinaInoperativaEntidad Entidad)
        {
            var status = capaDatos.AtenderMaquinaInoperativaOperativa(Entidad);
            return status;
        }
        public bool AtenderMaquinaInoperativaReparacion(MI_MaquinaInoperativaEntidad Entidad)
        {
            var status = capaDatos.AtenderMaquinaInoperativaReparacion(Entidad);
            return status;
        }
        public bool AtenderMaquinaInoperativaOperativaSinResolver(MI_MaquinaInoperativaEntidad Entidad)
        {
            var status = capaDatos.AtenderMaquinaInoperativaOperativaSinResolver(Entidad);
            return status;
        }
        public bool AtenderMaquinaInoperativaInoperativa(MI_MaquinaInoperativaEntidad Entidad)
        {
            var status = capaDatos.AtenderMaquinaInoperativaInoperativa(Entidad);
            return status;
        }
        public bool MaquinaInoperativaEliminarJson(int cod) {
            var status = capaDatos.EliminarMaquinaInoperativa(cod);

            return status;
        }
        public bool AtenderSolicitudMaquinaInoperativa(MI_MaquinaInoperativaEntidad Entidad)
        {
            var status = capaDatos.AtenderSolicitudMaquinaInoperativa(Entidad);
            return status;
        }
        public bool AprobarSolicitudMaquinaInoperativa(MI_MaquinaInoperativaEntidad Entidad)
        {
            var status = capaDatos.AprobarSolicitudMaquinaInoperativa(Entidad);
            return status;
        }

        public bool CambiarEstadoMaquinaInoperativa(int cod, int estado)
        {
            var status = capaDatos.CambiarEstadoMaquinaInoperativa(cod,estado);

            return status;
        }
        public bool AtencionPendienteEditarMaquinaInoperativa(int cod)
        {
            var status = capaDatos.AtencionPendienteEditarMaquinaInoperativa(cod);

            return status;
        }
        public bool AtencionPendienteRechazadasEditarMaquinaInoperativa(int cod)
        {
            var status = capaDatos.AtencionPendienteRechazadasEditarMaquinaInoperativa(cod);

            return status;
        }
        public List<MI_MaquinaInoperativaEntidad> ReporteCategoriaProblemasListaJsonxFechas(DateTime fechaIni, DateTime fechaFin,string strElementos)
        {
            var status = capaDatos.ReporteCategoriaProblemasListaJsonxFechas(fechaIni, fechaFin, strElementos);

            return status;
        }

    
        public List<MI_MaquinaInoperativaEntidad> ReporteMaquinaInoperativa(DateTime fecha_inicio, DateTime fecha_fin, int estado) {
            
            return capaDatos.ReporteMaquinaInoperativa(fecha_inicio, fecha_fin, estado);
        }
        public bool AgregarOrdenCompraMaquinaInoperativa(MI_MaquinaInoperativaEntidad Entidad) {
            var status = capaDatos.AgregarOrdenCompraMaquinaInoperativa(Entidad);

            return status;
        }

        //NUEVO TEST
        public List<MI_MaquinaDetalleEntidad> ObtenerTodasLasMaquinas() {
            var listaMaquinas = new List<MI_MaquinaDetalleEntidad>();

            try {
                string uriBase = ConfigurationManager.AppSettings["UriSistemaReclutamiento"].ToString();
                string uri = $"{uriBase}Administrativo/ListarMaquinasAdministrativo";

                // No enviamos ningún parámetro
                var requestData = new { };

                string jsonRequest = JsonConvert.SerializeObject(requestData);
                string jsonResponse = string.Empty;

                using(WebClient client = new WebClient()) {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=UTF-8";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    jsonResponse = client.UploadString(uri, "POST", jsonRequest);
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                MI_MaquinaDetalleListResponse responseObject = JsonConvert.DeserializeObject<MI_MaquinaDetalleListResponse>(jsonResponse, settings);

                if(responseObject != null && responseObject.respuesta) {
                    listaMaquinas = responseObject.data;
                }
            } catch(Exception ex) {
                Console.WriteLine($"Error al obtener todas las máquinas: {ex.Message}");
            }

            return listaMaquinas;
        }


    }
}

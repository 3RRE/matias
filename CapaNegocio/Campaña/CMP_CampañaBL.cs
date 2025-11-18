using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Campaña {
    public class CMP_CampañaBL {
        private CMP_CampañaDAL campa_dal = new CMP_CampañaDAL();
        public List<CMP_CampañaEntidad> CampañaListadoCompletoJson() {
            return campa_dal.GetListadoCampañacompleto();
        }

        public List<CMP_CampañaEntidad> CampañaListadoJson(string salas, DateTime fechaini, DateTime fechafin) {
            return campa_dal.GetListadoCampaña(salas, fechaini, fechafin);
        }

        public CMP_CampañaEntidad CampañaIdObtenerJson(Int64 id) {
            return campa_dal.GetCampañaID(id);
        }

        public int CampañaInsertarJson(CMP_CampañaEntidad campaña) {
            return campa_dal.GuardarCampaña(campaña);
        }

        public bool CampañaEditarJson(CMP_CampañaEntidad campaña) {
            return campa_dal.EditarCampaña(campaña);
        }

        public bool CampañaEditarEstadoJson(CMP_CampañaEntidad campaña) {
            return campa_dal.EditarEstadoCampaña(campaña);
        }

        public bool ActualizarMensajeWhatsAppCampania(string mensajeWhatsApp, string mensajeWhatsAppReactivacion, long idCamapania) {
            return campa_dal.ActualizarMensajeWhatsAppCampania(mensajeWhatsApp, mensajeWhatsAppReactivacion, idCamapania) != 0;
        }

        public bool EditarMultipleEstadoCampaña(string id_lista, int estado) {
            return campa_dal.EditarMultipleEstadoCampaña(id_lista, estado);
        }

        public List<CMP_CampañaEntidad> GetListadoCampaniaxTipoyFechas(string salas, DateTime fechaini, DateTime fechafin, int tipo) {
            return campa_dal.GetListadoCampaniaxTipoyFechas(salas, fechaini, fechafin, tipo);
        }
        public List<CMP_CampañaEntidad> GetListadoCampaniaSorteoReporte(string salas, DateTime fechaini, DateTime fechafin) {
            return campa_dal.GetListadoCampaniaSorteoReporte(salas, fechaini, fechafin);
        }

        public List<CMP_CampañaEntidad> ListarCampaniasEstadoTipo(int roomId, int cmpStatus, int cmpType) {
            return campa_dal.ListarCampaniasEstadoTipo(roomId, cmpStatus, cmpType);
        }
    }
}

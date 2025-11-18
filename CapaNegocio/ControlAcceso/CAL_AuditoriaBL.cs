using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using CapaEntidad.ControlAcceso.Filtro;
using System;
using System.Collections.Generic;

namespace CapaNegocio.ControlAcceso {
    public class CAL_AuditoriaBL {
        private readonly CAL_AuditoriaDAL capaDatos;

        public CAL_AuditoriaBL() {
            capaDatos = new CAL_AuditoriaDAL();
        }

        public List<CAL_AuditoriaEntidad> AuditoriaListadoCompletoJson() {
            return capaDatos.GetAllAuditoria();
        }

        public bool RegistrarBusqueda(CAL_AuditoriaEntidad auditoriaBusqueda) {
            return capaDatos.RegistrarBusqueda(auditoriaBusqueda);
        }

        public List<CAL_AuditoriaEntidad> ReporteAauditoriaBusqueda(string dni, int empresa, DateTime fechaIni, DateTime fechaFin) {
            return capaDatos.ReporteAauditoriaBusqueda(dni, empresa, fechaIni, fechaFin);
        }

        public CAL_AuditoriaEntidad BuscarClienteEnReporte(string dni) {
            return capaDatos.BuscarClienteEnReporte(dni);
        }

        public bool RegistrarBusquedaExterno(CAL_AuditoriaEntidad auditoriaBusqueda) {
            return capaDatos.RegistrarBusquedaExterno(auditoriaBusqueda);
        }

        public List<CAL_AuditoriaEntidad> GetAuditoriaSala(string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            return capaDatos.GetAuditoriaSala(ArraySalaId, fechaIni, fechaFin);
        }

        #region Migracion DWH
        public void ActualizarEstadoMigracionesDwh(List<int> ids, DateTime? fechaMigracionDwh) {
            foreach(int id in ids) {
                capaDatos.ActualizarEstadoMigracionDwhPorId(id, fechaMigracionDwh);
            }
        }

        public List<CAL_AuditoriaEntidad> ObtenerIngresosClientesSalasParaDwh(CAL_IngresoClienteSalaDwhFiltro filtro) {
            return capaDatos.ObtenerIngresosClientesSalasParaDwh(filtro);
        }
        #endregion
    }
}

using CapaDatos;
using CapaEntidad;
using CapaEntidad.Alertas;
using CapaEntidad.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio
{
    public class LogAlertaBilleterosBL {
        public LogAlertaBilleterosDAL logDAL = new LogAlertaBilleterosDAL();
        public int GuardarLogAlerta(LogAlertaBilleterosEntidad alerta) {
            return logDAL.GuardarLogAlerta(alerta);
        }
        public List<LogAlertaBilleterosEntidad> GetTop10Alerta(string codSalas) {
            return logDAL.GetTop10Alerta(codSalas);
        }
        public List<LogAlertaBilleterosEntidad> GetLogsxCod_Even_OL(string inQuery) {
            return logDAL.GetLogsxCod_Even_OL(inQuery);
        }
        public List<LogAlertaBilleterosEntidad> GetLogAlertaBilleterosxFiltros(DateTime fechaini, DateTime fechafin, string whereQuery = "") {
            return logDAL.GetLogAlertaBilleterosxFiltros(fechaini, fechafin, whereQuery);
        }

        //Revision de estado  sobre eventos, alertas y servicio
        public List<LogAlertaBilleterosEntidad> ConsultaRegistrosAlertaBilletero() {
            return logDAL.ConsultaRegistrosAlertaBilletero();
        }
        //Revision de estado  sobre eventos, alertas y servicio por usuario
        public List<LogAlertaBilleterosEntidad> ConsultaRegistrosAlertaBilleteroxUsuario(string salas) {
            return logDAL.ConsultaRegistrosAlertaBilleteroxUsuario(salas);
        }

        public List<int> ListarSalasActivas() {
            return logDAL.ConsultaSalasActivas();
        }
        public List<int> ConsultaSalasActivasxUsuario(int userId) {
            return logDAL.ConsultaSalasActivasxUsuario(userId);
        }

        public LogAlertaBilleterosEntidad GetAlertaBilleteroxId(Int64 id) {
            return logDAL.GetAlertaBilleteroxId(id);
        }
        public LogAlertaBilleterosEntidad GetLogAlertaBilleteroPorCodEvenOL(Int64 Cod_Even_OL, int CodSala, int Tipo) {
            return logDAL.GetLogAlertaBilleteroPorCodEvenOL(Cod_Even_OL, CodSala, Tipo);
        }
        public bool EditarLogAlertaBilletero(LogAlertaBilleterosEntidad alertaBilletero) {
            return logDAL.EditarLogAlertaBilletero(alertaBilletero);
        }

        public List<LogAlertaBilleterosEntidad> ObtenerAlertasEventos() {
            return logDAL.ObtenerAlertasEventos();

        }
        public List<LogAlertaBilleterosEntidad> ObtenerAlertasBilleteros() {
            return logDAL.ObtenerAlertasBilleteros();
        }

        #region Logs Reporte Nominal

        public List<ALEV_ReporteNominalEntidad> ObtenerReporteNominal(List<int> rooms, DateTime fromDate, DateTime toDate, int type)
        {
            List<ALEV_ReporteNominalEntidad> list = new List<ALEV_ReporteNominalEntidad>();
            List<ALEV_SalaNominalEntidad> salas = logDAL.ObtenerSalaNominal(rooms);
            List<ALEV_LogNominalEntidad> logsNominal = logDAL.ObtenerReporteNominal(rooms, fromDate, toDate, type);

            foreach (ALEV_SalaNominalEntidad sala in salas)
            {
                ALEV_ReporteNominalEntidad reporteNominal = new ALEV_ReporteNominalEntidad
                {
                    SalaId = sala.Codigo,
                    Sala = sala.Nombre,
                    Tipo = type,
                    Logs = logsNominal.Where(log => log.SalaId == sala.Codigo).ToList()
                };

                list.Add(reporteNominal);
            }

            return list;
        }

        public List<string> ObtenerRangoFechasNominal(DateTime fromDate, DateTime toDate)
        {
            return logDAL.ObtenerRangoFechasNominal(fromDate, toDate);
        }

        #endregion

        #region Logs Alerta Billeteros

        public List<LogAlertaBilleterosEntidad> ObtenerSalaLogsFecha(int room, int type, DateTime date)
        {
            return logDAL.ObtenerSalaLogsFecha(room, type, date);
        }

        
        public List<EVT_EventosOnlineEntidad> ObtenerEventosDelDiaPorCodSalaMovil(int codSala)
        {
            return logDAL.ObtenerEventosDelDiaPorCodSalaMovil(codSala);
        }
        public List<EVT_EventosOnlineEntidad> ObtenerEventosPorRangoFechaCodSala(int codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return logDAL.ObtenerEventosPorRangoFechaCodSala( codSala,  fechaIni,  fechaFin);
        }
        #endregion
    }
}

using CapaDatos.Progresivo;
using CapaEntidad.Progresivo;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Progresivo
{
    public class AlertaProgresivoBL
    {
        private readonly AlertaProgresivoDAL _alertaProgresivoDAL = new AlertaProgresivoDAL();

        public bool GuardarAlertaProgresivoDetallePozo(AlertaProgresivoEntidad alertaProgresivo)
        {
            bool response = false;

            alertaProgresivo.FechaRegistro = DateTime.Now;

            long alertaId = _alertaProgresivoDAL.GuardarAlertaProgresivo(alertaProgresivo);

            if(alertaId > 0)
            {
                foreach(AlertaProgresivoDetalleEntidad progresivoDetalle in alertaProgresivo.Detalle)
                {
                    progresivoDetalle.AlertaId = alertaId;
                    progresivoDetalle.FechaRegistro = DateTime.Now;

                    long detalleId = _alertaProgresivoDAL.GuardarAlertaProgresivoDetalle(progresivoDetalle);

                    if(detalleId > 0)
                    {
                        foreach(AlertaProgresivoPozoEntidad progresivoPozo in progresivoDetalle.Pozos)
                        {
                            progresivoPozo.AlertaId = alertaId;
                            progresivoPozo.DetalleId = detalleId;

                            _alertaProgresivoDAL.GuardarAlertaProgresivoPozo(progresivoPozo);
                        }
                    }
                }

                response = true;
            }

            return response;
        }

        public List<AlertaProgresivoEntidad> ListarAlertasProgresivoSala(int salaId, DateTime fromDate, DateTime toDate)
        {
            return _alertaProgresivoDAL.ListarAlertasProgresivoSala(salaId, fromDate, toDate);
        }

        public List<AlertaProgresivoEntidad> ListarAlertasProgresivoSalaDetalles(int salaId, DateTime fromDate, DateTime toDate)
        {
            List<AlertaProgresivoEntidad> listAlertaProgresivos = _alertaProgresivoDAL.ListarAlertasProgresivoSala(salaId, fromDate, toDate);

            foreach(AlertaProgresivoEntidad alertaProgresivo in listAlertaProgresivos)
            {
                alertaProgresivo.Detalle = ListarAlertaProgresivoDetalles(alertaProgresivo.Id);
            }

            return listAlertaProgresivos;
        }

        public AlertaProgresivoEntidad ObtenerAlertaProgresivo(long alertaId)
        {
            AlertaProgresivoEntidad alerta = _alertaProgresivoDAL.ObtenerAlertaProgresivo(alertaId);

            if(alerta.Id > 0)
            {
                List<AlertaProgresivoDetalleEntidad> progresivoDetalle = _alertaProgresivoDAL.ListarAlertaProgresivoDetalles(alertaId);

                foreach(AlertaProgresivoDetalleEntidad detalle in progresivoDetalle)
                {
                    detalle.Pozos = _alertaProgresivoDAL.ListarAlertaProgresivoPozos(alerta.Id, detalle.Id);
                }

                alerta.Detalle = progresivoDetalle;
            }

            return alerta;
        }

        public List<AlertaProgresivoDetalleEntidad> ListarAlertaProgresivoDetalles(long alertaId)
        {
            List<AlertaProgresivoDetalleEntidad> progresivoDetalle = _alertaProgresivoDAL.ListarAlertaProgresivoDetalles(alertaId);

            foreach (AlertaProgresivoDetalleEntidad detalle in progresivoDetalle)
            {
                detalle.Pozos = _alertaProgresivoDAL.ListarAlertaProgresivoPozos(alertaId, detalle.Id);
            }

            return progresivoDetalle;
        }


        public long GuardarAlertaProgresivo(AlertaProgresivoEntidad alertaProgresivo) {
            return _alertaProgresivoDAL.GuardarAlertaProgresivo(alertaProgresivo);
        }
        public List<AlertaProgresivoEntidad> ListarAlertasProgresivoSalaYTipo(int[] salas, DateTime fromDate, DateTime toDate, int type) => _alertaProgresivoDAL.ListarAlertasProgresivoSalaYTipo(salas, fromDate, toDate, type);

	}
}

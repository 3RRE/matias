using CapaDatos.Alertas;
using CapaEntidad;
using CapaEntidad.Alertas;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Alertas
{
    public class ALT_AlertaSalaBL
    {
        private ALT_AlertaSalaDAL alertaSalaDal = new ALT_AlertaSalaDAL();

        public List<ALT_AlertaSalaEntidad> ALT_AlertaSala_Listado()
        {
            return alertaSalaDal.ALT_AlertaSala_Listado();
        }
        public List<ALT_AlertaSalaEntidad> ALT_AlertaSala_xsala_idListado(string codsala, DateTime fechaini, DateTime fechafin)
        {
            return alertaSalaDal.ALT_AlertaSala_xsala_idListado(codsala, fechaini, fechafin);
        }
        public List<ALT_AlertaSalaEntidad> ALT_AlertaSala_xsala_idFechaListado(int codsala, DateTime fechaini, DateTime fechafin)
        {
            return alertaSalaDal.ALT_AlertaSala_xsala_idFechaListado(codsala, fechaini,fechafin);
        }

        public List<ALT_AlertaDeviceEntidad> ALT_AlertaSala_xdevicesListado(int codsala)
        {
            return alertaSalaDal.ALT_AlertaSala_xdevicesListado(codsala);
        }

        public ALT_AlertaSalaEntidad ALT_AlertasalaIdObtenerJson(Int64 id)
        {
            return alertaSalaDal.ALT_AlertasalaIdObtenerJson(id);
        }

        public ALT_AlertaSalaEntidad ALT_AlertasalaAlertaIdObtenerJson(Int64 id,int CodSala)
        {
            return alertaSalaDal.ALT_AlertasalaAlertaIdObtenerJson(id, CodSala);
        }
        public int ALT_AlertasalaInsertarJson(ALT_AlertaSalaEntidad alerta)
        {
            return alertaSalaDal.ALT_AlertasalaInsertarJson(alerta);
        }

        public bool ALT_AlertasalaEditarJson(ALT_AlertaSalaEntidad alerta)
        {
            return alertaSalaDal.ALT_AlertasalaEditarJson(alerta);
        }

        public bool ALT_AlertasalaEliminarJson(Int64 id)
        {
            return alertaSalaDal.ALT_AlertasalaEliminarJson(id);
        }


        public int ConsultarAlertasCargo(int codSala)
        {
            return alertaSalaDal.ConsultarAlertasCargo(codSala);

        }
        public bool CambiarTipoAlerta(int codSala, int tipo)
        {
            return alertaSalaDal.CambiarTipoAlerta(codSala, tipo);

        }

        public List<string> AlertBilleterosCorreos(int codSala) {
            return alertaSalaDal.AlertBilleterosCorreos(codSala);

        }
        public List<string> AlertaEventosCorreos(int codSala) {
            return alertaSalaDal.AlertaEventosCorreos(codSala);

        }

        public ContadorCorreoAlertaEntidad ObtenerValorContador() {
            return alertaSalaDal.ObtenerValorContador();
        }

        public void ResetearContador(DateTime fecha ) {
             alertaSalaDal.ResetearContador(fecha);
        }
        public void AgregarContador() {
            alertaSalaDal.AgregarContador();
        }

        public List<AlertBillNotificationReqEntidad> ListarAlertBillNotificationSala(string roomCode, int status, int days)
        {
            return alertaSalaDal.ListarAlertBillNotificationSala(roomCode, status, days);
        }

        #region Destinatarios Online

        public List<WEB_DestinatarioEntidad> ObtenerDestinatariosOnline(int salaId)
        {
            return alertaSalaDal.ObtenerDestinatariosOnline(salaId);
        }

        #endregion
    }
}

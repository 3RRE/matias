using CapaDatos.Progresivo;
using CapaEntidad.Alertas;
using CapaEntidad.Progresivo;
using System.Collections.Generic;

namespace CapaNegocio.Progresivo
{
    public class AlertaProgresivoCargoBL
    {
        private readonly AlertaProgresivoCargoDAL _alertaProgresivoCargoDAL = new AlertaProgresivoCargoDAL();

        public List<AlertaProgresivoCargoEntidad> ListarAlertaProgresivoCargo()
        {
            return _alertaProgresivoCargoDAL.ListarAlertaProgresivoCargo();
        }

        public int GuardarAlertaProgresivoCargo(AlertaProgresivoCargoEntidad alertaCargo)
        {
            return _alertaProgresivoCargoDAL.GuardarAlertaProgresivoCargo(alertaCargo);
        }

        public bool EliminarAlertaProgresivoCargo(int alertaId)
        {
            return _alertaProgresivoCargoDAL.EliminarAlertaProgresivoCargo(alertaId);
        }

        public List<ALT_AlertaDeviceEntidad> ListarAlertaDeviceSala(int salaId)
        {
            return _alertaProgresivoCargoDAL.ListarAlertaDeviceSala(salaId);
        }

        public List<string> ListarAlertaCorreosSala(int salaId)
        {
            return _alertaProgresivoCargoDAL.ListarAlertaCorreosSala(salaId);
        }
    }
}

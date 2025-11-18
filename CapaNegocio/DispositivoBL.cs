using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class DispositivoBL
    {
        private DispositivoDAL dispositivoDal = new DispositivoDAL();
        public List<DispositivoEntidad> DispositivoListadoJson()
        {
            return dispositivoDal.DispositivoListadoJson();
        }
        public DispositivoEntidad DispositivoObtenerJson(int dispositivoId)
        {
            return dispositivoDal.DispositivoObtenerJson(dispositivoId);
        }
        public bool DispositivoInsertarJson(DispositivoEntidad dispositivo)
        {
            return dispositivoDal.DispositivoInsertarJson(dispositivo);
        }
        public bool DispositivoEditarJson(DispositivoEntidad dispositivo)
        {
            return dispositivoDal.DispositivoEditarJson(dispositivo);
        }
        public bool ComprobarDispositivoJson(string mac)
        {
            return dispositivoDal.ComprobarDispositivoJson(mac);
        }
    }
}

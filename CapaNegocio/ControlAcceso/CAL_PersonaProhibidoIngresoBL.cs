using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using Newtonsoft.Json;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_PersonaProhibidoIngresoBL
    {
        CAL_PersonaProhibidoIngresoDAL capaDatos = new CAL_PersonaProhibidoIngresoDAL();

        public List<CAL_PersonaProhibidoIngresoEntidad> TimadorListadoCompletoJson()
        {
            return capaDatos.GetAllTimador();
        }
        public CAL_PersonaProhibidoIngresoEntidad TimadorIdObtenerJson(int id)
        {
            return capaDatos.GetIDTimador(id);
        }
        public int TimadorInsertarJson(CAL_PersonaProhibidoIngresoEntidad Entidad)
        {
            var id = capaDatos.InsertarTimador(Entidad);

            return id;
        }
        public bool TimadorEditarJson(CAL_PersonaProhibidoIngresoEntidad Entidad)
        {
            var status = capaDatos.EditarTimador(Entidad);

            return status;
        }
        public bool TimadorEliminarJson(int id)
        {
            var status = capaDatos.EliminarTimador(id);

            return status;
        }
        public CAL_PersonaProhibidoIngresoEntidad GetTimadorPorDNI(string dni)
        {
            return capaDatos.GetTimadorPorDNI(dni);
        }
    }
}

using CapaDatos.Ocurrencias;
using CapaEntidad.Ocurrencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ocurrencias
{
    public class OCU_CorreoBL
    {
        private OCU_CorreoDAL correoDal = new OCU_CorreoDAL();
        public List<OCU_CorreoEntidad> GetListadoCorreos()
        {
            return correoDal.GetListadoCorreos();
        }
        public int GuardarCorreos(string values)
        {
            return correoDal.GuardarCorreos(values);
        }
        public int GuardarCorreo(OCU_CorreoEntidad correo)
        {
            return correoDal.GuardarCorreo(correo);
        }
        public bool EditarCorreo(OCU_CorreoEntidad correo)
        {
            return correoDal.EditarCorreo(correo);
        }
        public bool EditarEstadoCorreo(OCU_CorreoEntidad correo)
        {
            return correoDal.EditarEstadoCorreo(correo);
        }
        public OCU_CorreoEntidad GetCorreoID(int CorreoId) {
            return correoDal.GetCorreoID(CorreoId);
        }
    }
}

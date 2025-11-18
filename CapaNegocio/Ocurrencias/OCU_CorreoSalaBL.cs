using CapaDatos.Ocurrencias;
using CapaEntidad.Ocurrencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ocurrencias
{
    public class OCU_CorreoSalaBL
    {
        private OCU_CorreoSalaDAL correoSalaDal = new OCU_CorreoSalaDAL();
        public List<OCU_CorreoSalaEntidad> GetListadoCorreoSala()
        {
            return correoSalaDal.GetListadoCorreoSala();
        }
        public List<OCU_CorreoSalaEntidad> GetListadoCorreoSalaxSala(int SalaId)
        {
            return correoSalaDal.GetListadoCorreoSalaxSala(SalaId);
        }
        public int GuardarCorreoSalaVarios(string values)
        {
            return correoSalaDal.GuardarCorreoSalaVarios(values);
        }
        public bool EliminarCorreoSalaxCorreoId(int CorreoId)
        {
            return correoSalaDal.EliminarCorreoSalaxCorreoId(CorreoId);
        }
    }
}

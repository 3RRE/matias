using CapaDatos.ControlAcceso;
using CapaEntidad.ControlAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ControlAcceso
{
    public class CAL_MenorDeEdadBL
    {
        CAL_MenorEdadDAL capaDatos = new CAL_MenorEdadDAL();
        public CAL_MenorDeEdadEntidad GetMenorEdadPorDNI(string dni)
        {
            return capaDatos.GetMenorEdadPorDNI(dni);
        }
        public int InsertarMenorEdad(CAL_MenorDeEdadEntidad entidadM)
        {
            return capaDatos.InsertarMenorEdad(entidadM);
        }
        public List<CAL_MenorDeEdadEntidad> ListarMenorEdad()
        {
            return capaDatos.ListarMenorEdad();
        }
        public CAL_MenorDeEdadEntidad GetMenorEdadPorId(int id)
        {
            return capaDatos.GetMenorEdadPorId(id);
        }

        public bool EditarMenorEdad(CAL_MenorDeEdadEntidad entidad)
        {
            return capaDatos.EditarMenorEdad(entidad);
        }
    }
}

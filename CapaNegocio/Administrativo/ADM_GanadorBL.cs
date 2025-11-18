using CapaDatos.Administrativo;
using CapaEntidad.Administrativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administrativo
{
    public class ADM_GanadorBL
    {
        private readonly ADM_GanadorDAL _ganadorDAL = new ADM_GanadorDAL();
        public int GuardarADM_Ganador(ADM_GanadorEntidad ganador)
        {
            return _ganadorDAL.GuardarADM_Ganador(ganador);
        }
        public bool EliminarPremiosSalaFecha(int CodSalaProgresivo, DateTime FechaOperacion)
        {
            return _ganadorDAL.EliminarPremiosSalaFecha(CodSalaProgresivo, FechaOperacion);
        }
    }
}

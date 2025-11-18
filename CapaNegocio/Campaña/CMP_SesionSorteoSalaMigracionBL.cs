using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_SesionSorteoSalaMigracionBL
    {
        private readonly CMP_SesionSorteoSalaMigracionDAL _sesionSorteoSalaMigracionDAL;
        public CMP_SesionSorteoSalaMigracionBL()
        {
            _sesionSorteoSalaMigracionDAL = new CMP_SesionSorteoSalaMigracionDAL();
        }
        public int GuardarSesionSorteoSalaMigracion(CMP_SesionSorteoSalaMigracion item)
        {
            return _sesionSorteoSalaMigracionDAL.GuardarSesionSorteoSalaMigracion(item);
        }
        public List<CMP_SesionSorteoSalaMigracion> ListarSesionSorteoSalaMigracion(string queryConsulta)
        {
            return _sesionSorteoSalaMigracionDAL.ListarSesionSorteoSalaMigracion(queryConsulta);
        }
        public List<CMP_JugadasCliente> ListarJugadasTableau(string queryConsulta)
        {
            return _sesionSorteoSalaMigracionDAL.ListarJugadasTableau(queryConsulta);
        }
    }
}

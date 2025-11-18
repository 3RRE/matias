using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Campaña
{
    public class CMP_SesionMigracionBL
    {
        private readonly CMP_SesionMigracionDAL _sesionMigracionDAL;
        public CMP_SesionMigracionBL()
        {
            _sesionMigracionDAL = new CMP_SesionMigracionDAL();
        }
        public int GuardarSesion(CMP_SesionMigracion sesion)
        {
            return _sesionMigracionDAL.GuardarSesion(sesion);
        }
        public List<CMP_SesionMigracion> ListarSesionPorQuery(string Query)
        {
            return _sesionMigracionDAL.ListarSesionPorQuery(Query);
        }
    }
}

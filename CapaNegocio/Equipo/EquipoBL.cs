using CapaDatos.Disco;
using CapaDatos.Equipo;
using CapaEntidad.Discos;
using CapaEntidad.Equipo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Equipo
{
    public class EquipoBL
    {
        private EquipoDAL _equipoDal = new EquipoDAL();

        public List<EquipoEntidad> ListadoEquiposInfo(int codSala, DateTime fechaIni, DateTime fechaFin)
        {
            return _equipoDal.ListadoEquipoInfo(codSala, fechaIni, fechaFin);
        }

        public int AgregarEquipoInfo(EquipoEntidad equipo)
        {
            return _equipoDal.GuardarEquipoInfo(equipo);
        }

        public EquipoEntidad ObtenerUltimoRegistro(int id)
        {
            return _equipoDal.UltimoRegistroEquipo(id);
        }
    }
}

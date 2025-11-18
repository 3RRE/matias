using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ModuloBL
    {
        private ModuloDAL moduloDAL = new ModuloDAL();

        public List<ModuloEntidad> ModuloListarJson()
        {
            return moduloDAL.ModuloListarJson();
        }

        public bool ModuloGuardarJson(ModuloEntidad moduloEntidad)
        {
            return moduloDAL.ModuloGuardarJson(moduloEntidad);
        }

        public ModuloEntidad ModuloIdObtenerJson(int sub)
        {
            return moduloDAL.ModuloIdObtenerJson(sub);
        }

        public bool ModuloActualizarJson(ModuloEntidad moduloEntidad)
        {
            return moduloDAL.ModuloActualizarJson(moduloEntidad);
        }

        public List<ModuloEntidad> BuscarModuloSistemaJson(int id)
        {
            return moduloDAL.BuscarModuloSistema(id);
        }
    }
}

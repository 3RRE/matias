using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ConfiguracionSeguridadBL
    {
        private ConfiguracionSeguridadDAL configuracionseguridaddal = new ConfiguracionSeguridadDAL();

        public bool GuardarConfiguracionSeguridadJson(ConfiguracionSeguridadEntidad cs)
        {
            return configuracionseguridaddal.GuardarConfiguracionSeguridad(cs);
        }
        public bool ActualizarConfiguracionSeguridadJson(ConfiguracionSeguridadEntidad cs)
        {
            return configuracionseguridaddal.ActualizarConfiguracionSeguridad(cs);
        }

        public ConfiguracionSeguridadEntidad ObtenerConfiguracionSeguridadJson()
        {
            return configuracionseguridaddal.ObtenerConfiguracionSeguridad();
        }
    }
}

using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class SEG_Configuracion_SeguridadBL
    {
        private SEG_Configuracion_SeguridadDAL csDal = new SEG_Configuracion_SeguridadDAL();
        public bool GuardarConfiguracionSeguridad(SEG_Configuracion_SeguridadEntidad cs)
        {
            return csDal.GuardarConfiguracionSeguridad(cs);
        }
        public bool ActualizarConfiguracionSeguridad(SEG_Configuracion_SeguridadEntidad cs)
        {
            return csDal.ActualizarConfiguracionSeguridad(cs);
        }
        public SEG_Configuracion_SeguridadEntidad ConfiguracionSeguridadObtenerJson()
        {
            return csDal.ConfiguracionSeguridadObtenerJson();
        }
    }
}

using CapaDatos.SatisfaccionCliente.Configuracion;
using CapaEntidad.SatisfaccionCliente.DTO.Configuracion;
using CapaEntidad.SatisfaccionCliente.Entity.Configuracion;
using System.Collections.Generic;

namespace CapaNegocio.SatisfaccionCliente.Configuracion {
    public class ESC_ConfiguracionBL {
        private readonly ESC_ConfiguracionDAL configuracionDAL;

        public ESC_ConfiguracionBL() {
            configuracionDAL = new ESC_ConfiguracionDAL();
        }

        public List<ESC_ConfiguracionDto> ObtenerConfiguraciones() {
            return configuracionDAL.ObtenerConfiguraciones();
        }

        public ESC_ConfiguracionDto ObtenerConfiguracionPorCodSala(int codSala) {
            return configuracionDAL.ObtenerConfiguracionPorCodSala(codSala);
        }

        public ESC_ConfiguracionDto ObtenerConfiguracionPorId(int id) {
            return configuracionDAL.ObtenerConfiguracionPorId(id);
        }

        public int InsertarConfiguracion(ESC_Configuracion configuracion) {
            return configuracionDAL.InsertarConfiguracion(configuracion);
        }

        public int ActualizarConfiguracion(ESC_Configuracion configuracion) {
            return configuracionDAL.ActualizarConfiguracion(configuracion);
        }
    }
}

using CapaDatos.ProgresivoRuleta;
using CapaEntidad.ProgresivoRuleta.Config;
using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaEntidad.ProgresivoRuleta.Filtro;
using System.Collections.Generic;

namespace CapaNegocio.ProgresivoRuleta {
    public class PRU_ConfiguracionBL {
        private readonly PRU_ConfiguracionDAL configuracionDAL;

        public PRU_ConfiguracionBL() {
            configuracionDAL = new PRU_ConfiguracionDAL();
        }

        public List<PRU_ConfiguracionDto> ObtenerConfiguraciones() {
            return configuracionDAL.ObtenerConfiguraciones();
        }

        public List<PRU_ConfiguracionDto> ObtenerConfiguracionesPorCodSala(int codSala) {
            return configuracionDAL.ObtenerConfiguracionesPorCodSala(codSala);
        }

        public PRU_ConfiguracionDto ObtenerConfiguracionPorId(int id) {
            return configuracionDAL.ObtenerConfiguracionPorId(id);
        }

        public bool InsertarConfiguracion(PRU_Configuracion categoria) {
            //categoria.CodMaquinas = categoria.SlotHexValues.ToDelimitedString();
            //categoria.Posiciones = categoria.SlotHexPositions.ToDelimitedString();
            return configuracionDAL.InsertarConfiguracion(categoria) > 0;
        }

        public bool ActualizarConfiguracion(PRU_Configuracion categoria) {
            //categoria.CodMaquinas = categoria.SlotHexValues.ToDelimitedString();
            //categoria.Posiciones = categoria.SlotHexPositions.ToDelimitedString();
            return configuracionDAL.ActualizarConfiguracion(categoria) > 0;
        }

        public Pru_MisteryConfig ObtenerConfiguracionPorFiltro(PRU_Filtro filtro) {
            return configuracionDAL.ObtenerConfiguracionPorFiltro(filtro);
        }
    }
}

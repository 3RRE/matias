using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_NivelAtencionBL {
        private readonly GLPI_NivelAtencionDAL nivelAtencionDAL;

        public GLPI_NivelAtencionBL() {
            nivelAtencionDAL = new GLPI_NivelAtencionDAL();
        }

        public List<GLPI_NivelAtencion> ObtenerNivelesAtencion() {
            return nivelAtencionDAL.ObtenerNivelesAtencion();
        }

        public List<GLPI_SelectHelper> ObtenerNivelesAtencionSelect() {
            List<GLPI_SelectHelper> options = ObtenerNivelesAtencion()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect(),
                    Color = x.ObtenerColorSelect(),
                }).ToList();
            return options;
        }

        public GLPI_NivelAtencion ObtenerNivelAtencionPorId(int id) {
            return nivelAtencionDAL.ObtenerNivelAtencionPorId(id);
        }

        public bool InsertarNivelAtencion(GLPI_NivelAtencion nivelAtencion) {
            return nivelAtencionDAL.InsertarNivelAtencion(nivelAtencion) > 0;
        }

        public bool ActualizarNivelAtencion(GLPI_NivelAtencion nivelAtencion) {
            return nivelAtencionDAL.ActualizarNivelAtencion(nivelAtencion) > 0;
        }

        public bool EliminarNivelAtencion(int id) {
            return nivelAtencionDAL.EliminarNivelAtencion(id) > 0;
        }
    }
}

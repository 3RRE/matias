using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_IdentificadorBL {
        private readonly GLPI_IdentificadorDAL identificadorDAL;

        public GLPI_IdentificadorBL() {
            identificadorDAL = new GLPI_IdentificadorDAL();
        }

        public List<GLPI_Identificador> ObtenerIdentificadores() {
            return identificadorDAL.ObtenerIdentificadores();
        }

        public List<GLPI_SelectHelper> ObtenerIdentificadoresSelect() {
            List<GLPI_SelectHelper> options = ObtenerIdentificadores()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_Identificador ObtenerIdentificadorPorId(int id) {
            return identificadorDAL.ObtenerIdentificadorPorId(id);
        }

        public bool InsertarIdentificador(GLPI_Identificador identificador) {
            return identificadorDAL.InsertarIdentificador(identificador) > 0;
        }

        public bool ActualizarIdentificador(GLPI_Identificador identificador) {
            return identificadorDAL.ActualizarIdentificador(identificador) > 0;
        }

        public bool EliminarIdentificador(int id) {
            return identificadorDAL.EliminarIdentificador(id) > 0;
        }
    }
}

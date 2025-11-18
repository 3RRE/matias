using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_CorreoBL {
        private readonly GLPI_CorreoDAL correoDAL;

        public GLPI_CorreoBL() {
            correoDAL = new GLPI_CorreoDAL();
        }

        public List<GLPI_Correo> ObtenerCorreos() {
            return correoDAL.ObtenerCorreos();
        }

        public List<GLPI_Correo> ObtenerCorreosActivos() {
            return correoDAL.ObtenerCorreosActivos();
        }

        public List<GLPI_SelectHelper> ObtenerCorreosSelect() {
            List<GLPI_SelectHelper> options = ObtenerCorreosActivos()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_Correo ObtenerCorreoPorCorreo(string correo) {
            return correoDAL.ObtenerCorreoPorCorreo(correo);
        }

        public bool InsertarCorreo(GLPI_Correo correo) {
            bool correoExiste = ObtenerCorreoPorCorreo(correo.Correo).Existe();
            if(correoExiste) {
                return false;
            }
            return correoDAL.InsertarCorreo(correo) > 0;
        }

        public int InsertarCorreos(List<string> correos, int idUsuario) {
            int cantidadInsertados = 0;
            correos.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            foreach(string correo in correos) {
                bool insertado = InsertarCorreo(new GLPI_Correo {
                    Correo = correo,
                    IdUsuaroRegistra = idUsuario,
                    Estado = true
                });
                if(insertado) {
                    cantidadInsertados++;
                }
            }
            return cantidadInsertados;
        }
    }
}

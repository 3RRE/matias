using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_PartidaBL {
        private readonly GLPI_PartidaDAL partidaDAL;

        public GLPI_PartidaBL() {
            partidaDAL = new GLPI_PartidaDAL();
        }

        public List<GLPI_Partida> ObtenerPartidas() {
            return partidaDAL.ObtenerPartidas();
        }

        public List<GLPI_SelectHelper> ObtenerPartidasSelect() {
            List<GLPI_SelectHelper> options = ObtenerPartidas()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_Partida ObtenerPartidaPorId(int id) {
            return partidaDAL.ObtenerPartidaPorId(id);
        }

        public bool InsertarPartida(GLPI_Partida partida) {
            return partidaDAL.InsertarPartida(partida) > 0;
        }

        public bool ActualizarPartida(GLPI_Partida partida) {
            return partidaDAL.ActualizarPartida(partida) > 0;
        }

        public bool EliminarPartida(int id) {
            return partidaDAL.EliminarPartida(id) > 0;
        }
    }
}

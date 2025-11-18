using CapaEntidad.GLPI.Enum;

namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_Partida : GLPI_BaseClassMantenedor {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public GLPI_TipoGasto TipoGasto { get; set; }

        public override string ObtenerIdPadreSelect() {
            return string.Empty;
        }

        public override string ObtenerTextoSelect() {
            return string.IsNullOrWhiteSpace(Codigo) ? Nombre : $"{Codigo} - {Nombre}";
        }
        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

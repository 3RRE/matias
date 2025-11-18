namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_Correo : GLPI_BaseClassMantenedor {
        public string Correo { get; set; }
        public int IdUsuaroRegistra { get; set; }

        public override string ObtenerIdPadreSelect() {
            return string.Empty;
        }

        public override string ObtenerTextoSelect() {
            return Correo;
        }
        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

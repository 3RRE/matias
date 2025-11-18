namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_NivelAtencion : GLPI_BaseClassMantenedor {
        public string Nombre { get; set; }
        public string Color { get; set; }

        public override string ObtenerIdPadreSelect() {
            return string.Empty;
        }

        public override string ObtenerTextoSelect() {
            return Nombre;
        }    
        public string ObtenerColorSelect() {
            return Color;
        }

        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

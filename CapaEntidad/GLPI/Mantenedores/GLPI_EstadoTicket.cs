namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_EstadoTicket : GLPI_BaseClassMantenedor {
        public object estado;

        public string Nombre { get; set; }

        public override string ObtenerIdPadreSelect() {
            return string.Empty;
        }

        public override string ObtenerTextoSelect() {
            return Nombre;
        }       
        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

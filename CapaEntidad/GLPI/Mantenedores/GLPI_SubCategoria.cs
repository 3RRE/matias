namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_SubCategoria : GLPI_BaseClassMantenedor {
        public int IdPartida { get; set; }
        public string NombrePartida { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string Nombre { get; set; }

        public override string ObtenerIdPadreSelect() {
            return IdCategoria.ToString();
        }

        public override string ObtenerTextoSelect() {
            return Nombre;
        }
        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

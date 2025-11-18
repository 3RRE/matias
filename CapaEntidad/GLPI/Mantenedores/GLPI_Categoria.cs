namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_Categoria : GLPI_BaseClassMantenedor {
        public int IdPartida { get; set; }
        public string NombrePartida { get; set; }
        public string Nombre { get; set; }

        public override string ObtenerIdPadreSelect() {
            return IdPartida.ToString();
        }

        public override string ObtenerTextoSelect() {
            return Nombre;
        }

        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

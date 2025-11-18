namespace CapaEntidad.GLPI.Mantenedores {
    public class GLPI_Usuario : GLPI_BaseClassMantenedor {
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string NombreCargo { get; set; } = string.Empty;

        public override string ObtenerIdPadreSelect() {
            return string.Empty;
        }

        public string ObtenerNombreCompleto() {
            return $"{Nombres} {ApellidoPaterno} {ApellidoMaterno}".Trim();
        }

        public override string ObtenerTextoSelect() {
            return $"{NombreCargo} - {ObtenerNombreCompleto()}";
        }
        public override bool ObtenerEstadoSelect() {
            return Estado;
        }
    }
}

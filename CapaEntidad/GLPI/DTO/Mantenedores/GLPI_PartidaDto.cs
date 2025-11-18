using CapaEntidad.GLPI.Enum;

namespace CapaEntidad.GLPI.DTO.Mantenedores {
    public class GLPI_PartidaDto {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public GLPI_TipoGasto TipoGasto { get; set; }
    }
}

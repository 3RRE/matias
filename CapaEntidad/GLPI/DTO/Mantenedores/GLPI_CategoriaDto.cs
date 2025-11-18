namespace CapaEntidad.GLPI.DTO.Mantenedores {
    public class GLPI_CategoriaDto {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public GLPI_PartidaDto Partida { get; set; } = new GLPI_PartidaDto();
    }
}

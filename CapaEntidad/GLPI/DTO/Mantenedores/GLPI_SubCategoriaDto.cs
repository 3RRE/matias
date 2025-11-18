namespace CapaEntidad.GLPI.DTO.Mantenedores {
    public class GLPI_SubCategoriaDto {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public GLPI_CategoriaDto Categoria { get; set; } = new GLPI_CategoriaDto();
    }
}

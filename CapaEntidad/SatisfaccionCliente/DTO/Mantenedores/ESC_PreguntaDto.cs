namespace CapaEntidad.SatisfaccionCliente.DTO.Mantenedores {
    public class ESC_PreguntaDto {
        public int Id { get; set; }
        public ESC_SalaDto Sala { get; set; } = new ESC_SalaDto();
        public string Texto { get; set; } = string.Empty;
        public bool EsObligatoria { get; set; }
        public string EsObligatoriaStr { get => EsObligatoria ? "Sí" : "No"; }
        public bool Estado { get; set; }
        public string EstadoStr { get => Estado ? "Activo" : "Inactivo"; }

        public bool Existe() {
            return Id > 0;
        }
    }
}

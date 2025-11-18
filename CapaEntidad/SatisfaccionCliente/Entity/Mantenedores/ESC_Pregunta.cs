namespace CapaEntidad.SatisfaccionCliente.Entity.Mantenedores {
    public class ESC_Pregunta : ESC_BaseClassMantenedor {
        public int CodSala { get; set; }
        public string Texto { get; set; }
        public bool EsObligatoria { get; set; }
    }
}

namespace CapaEntidad.BUK {
    public class BUK_EquivalenciaEmpresaEntidad {
        public int IdEquivalenciaEmpresa { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodEmpresaOfisis { get; set; } = string.Empty;
        public int IdEmpresaBuk { get; set; }
        public int Estado { get; set; }

        public bool Existe() {
            return IdEquivalenciaEmpresa > 0;
        }
    }
}

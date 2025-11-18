namespace CapaEntidad.BUK {
    public class BUK_EquivalenciaSedeEntidad {
		public int IdEquivalenciaSede { get; set; }
		public int IdEquivalenciaEmpresa { get; set; }
		public int IdEmpresaBuk { get; set; }
		public string CodEmpresaOfisis { get; set; } = string.Empty;
		public string CodSedeOfisis { get; set; } = string.Empty;
		public string NombreSede { get; set; } = string.Empty;
        public string NombreEmpresa { get; set; } = string.Empty;

		public bool Existe() {
			return IdEquivalenciaSede > 0;
		}
    }
}

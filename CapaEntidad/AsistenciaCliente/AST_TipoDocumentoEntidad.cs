namespace CapaEntidad.AsistenciaCliente {
    public class AST_TipoDocumentoEntidad {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public bool Existe() {
            return Id > 0;
        }

    }
}

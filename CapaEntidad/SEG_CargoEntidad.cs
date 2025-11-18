namespace CapaEntidad {
    public class SEG_CargoEntidad {
        public int CargoID { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }

        //agregado para alertas agosto/2021
        public int alt_id { get; set; }

        public bool Existe() {
            return CargoID > 0;
        }
    }
}

using System;

namespace CapaEntidad.Reclamaciones
{
    public class REC_reclamacionEntidad
    {
		public Int64 id { get; set; }
		public string codigo { get; set; }
		public DateTime fecha { get; set; }
		public string nombre { get; set; }
		public string direccion { get; set; }
		public string documento { get; set; }
		public string telefono { get; set; }
		public string correo { get; set; }
		public string tipo { get; set; }
		public double monto { get; set; }
		public string descripcion { get; set; }
		public string tipo_reclamo { get; set; }
		public string tipo_local { get; set; }
		public Int32 sala_id { get; set; }
		public string local_nombre { get; set; }
		public string razon_social { get; set; }
		public string local_direccion { get; set; }
		public string correo_jop { get; set; }
		public string correo_sop { get; set; }
		public string referencia { get; set; }
		public string detalle { get; set; }
		public string pedido { get; set; }
		public string hash { get; set; }
		public int estado { get; set; }
		public string atencionsala { get; set; }
		public string atencionlegal { get; set; }
		public int atencion { get; set; }
        public string adjunto { get; set; }
        public string nombrepadre_madre { get; set; }
        public string nombre_sala { get; set; }
        public string imagen_empresa { get; set; }
        public bool mostrar_todo { get; set; }
        public int tipo_sala { get; set; }
        public DateTime fecha_enviolegal { get; set; }
        public string ruc_empresa { get; set; }
        public string usuario_sala { get; set; }
        public string usuario_legal { get; set; }
        public string direcciones_adjuntas { get; set; }
        public string imagen_sala { get; set; }
		public string ruta_firma_desistimiento { get; set; }
		public int desistimiento { get; set; }
		public string firma_desistimientobase64 { get; set; }
		public int enviar_adjunto { get; set; }
    }
}
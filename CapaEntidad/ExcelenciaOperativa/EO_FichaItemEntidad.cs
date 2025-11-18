using System;

namespace CapaEntidad.ExcelenciaOperativa
{
    public class EO_FichaItemEntidad
    {
        public long ItemId { get; set; }
        public long FichaId { get; set; }
        public long CategoriaId { get; set; }
        public string Nombre { get; set; }
        public float PuntuacionObtenida { get; set; }
        public float PuntuacionBase { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public int? FechaExpiracionActivo { get; set; }
        public string Respuesta { get; set; }
        public string Observacion { get; set; }
        public int? TipoRespuesta { get; set; }
        public string Codigo { get; set; }
    }
}

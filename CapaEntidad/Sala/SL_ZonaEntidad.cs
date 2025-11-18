using System;

namespace CapaEntidad.Sala
{
    public class SL_ZonaEntidad
    {
        public int Id { get; set; }
        public int SalaId { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public byte Estado { get; set; }
        public string SalaNombre { get; set; }
    }
}

using System;

namespace CapaEntidad.Progresivo
{
    public class AlertaProgresivoCargoEntidad
    {
        public int Id { get; set; }
        public int CargoId { get; set; }
        public string CargoNombre { get; set; }
        public int SalaId { get; set; }
        public string SalaNombre { get; set; }
        public string EmpresaNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CapaEntidad.Progresivo
{
    public class AlertaProgresivoEntidad
    {
        public long Id { get; set; }
        public int SalaId { get; set; }
        public string SalaNombre { get; set; }
        public string ProgresivoNombre { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public List<AlertaProgresivoDetalleEntidad> Detalle { get; set; }
    }
}

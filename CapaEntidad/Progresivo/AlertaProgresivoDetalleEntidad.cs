using System;
using System.Collections.Generic;

namespace CapaEntidad.Progresivo
{
    public class AlertaProgresivoDetalleEntidad
    {
        public long Id { get; set; }
        public int ProgresivoID { get; set; }
        public int NroPozos { get; set; }
        public bool PorCredito { get; set; }
        public bool BaseOculto { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public int NroJugadores { get; set; }
        public int ProgresivoImagenID { get; set; }
        public bool PagoCaja { get; set; }
        public int DuracionPantalla { get; set; }
        public string Simbolo { get; set; }
        public int Estado { get; set; }
        public string FechaIni_desc { get; set; }
        public string FechaFin_desc { get; set; }
        public int indice { get; set; }
        public string Estado_desc { get; set; }
        public string ProgresivoImagen_desc { get; set; }
        public bool RegHistorico { get; set; }
        public string ProgresivoImagenNombre { get; set; }
        public int ProgresivoIDOnline { get; set; }
        public string ProgresivoNombreOnline { get; set; }
        public int SalaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long AlertaId { get; set; }
        public bool ProActual { get; set; }
        public List<AlertaProgresivoPozoEntidad> Pozos { get; set; }
        
    }
}

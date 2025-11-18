using System;

namespace CapaEntidad.Progresivo
{
    public class AlertaProgresivoPozoEntidad
    {
        public int ProgresivoID { get; set; }
        public int DetalleProgresivoID { get; set; }
        public int PozoID { get; set; }
        public double Actual { get; set; }
        public double Anterior { get; set; }
        public double ActualOculto { get; set; }
        public double AnteriorOculto { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoPozo { get; set; }
        public int Estado { get; set; }
        public double MontoMin { get; set; }
        public double MontoBase { get; set; }
        public double MontoMax { get; set; }
        public double IncPozo1 { get; set; }
        public double IncPozo2 { get; set; }
        public double MontoOcMin { get; set; }
        public double MontoOcMax { get; set; }
        public double IncOcPozo1 { get; set; }
        public double IncOcPozo2 { get; set; }
        public bool Parametro { get; set; }
        public double Punto { get; set; }
        public double Prob1 { get; set; }
        public double Prob2 { get; set; }
        public int Indice { get; set; }
        public int EstadoInicial { get; set; }
        public int Dificultad { get; set; }
        public int RsJugadores { get; set; }
        public int RsApuesta { get; set; }
        public string Dificultad_desc { get; set; }
        public string Estado_desc { get; set; }
        public double TrigMin { get; set; }
        public double TrigMax { get; set; }
        public double Top { get; set; }
        public double TopAnt { get; set; }
        public string TMin { get; set; }
        public string TMax { get; set; }
        public long DetalleId { get; set; }
        public long AlertaId { get; set; }
        public bool ProActual { get; set; }
    }
}



using System;

namespace CapaEntidad.AnalisisDataSala {
    public class AnalisisSalaDTO {
    }
    public class IdleTimeKpiDto {
        public DateTime FechaOperativa { get; set; }
        public string CodSala { get; set; }
        public int TotalSegundosEnJuego { get; set; }
        public int TotalSegundosInactivo { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
    }

    // Para el Endpoint 2 (Gráfico de Horas)
    public class IdleTimePorHoraDto {
        public int HoraDelDia { get; set; }
        public int SegundosEnJuego { get; set; }
        public int SegundosInactivo { get; set; }
    }

    // Para el Endpoint 3 (Ranking)
    public class IdleTimeRankingDto {
        public DateTime FechaOperativa { get; set; }
        public string CodSala { get; set; }
        public string CodMaq { get; set; }
        public int TotalSegundosEnJuego { get; set; }
        public int TotalSegundosInactivo { get; set; }
    }

    // Para el Endpoint 4 (Gantt)
    public class IdleTimeTimelineDto {
        public string CodSala { get; set; }
        public string CodMaq { get; set; }
        public bool EstadoEnJuego { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public int DuracionSegundos { get; set; }
    }

    /// <summary>
    /// DTO para el Endpoint 1: GetHitFrecGeneral
    /// (Recibe los datos de la tabla HitFrec_ResumenGeneral)
    /// </summary>
    public class HitFrecKpiDto {
        public DateTime FechaOperativa { get; set; }
        public string CodSala { get; set; }
        public int TotalJuegos { get; set; }
        public int TotalHits { get; set; }
        public double HitFrequencyPorcentaje { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
    }

    /// <summary>
    /// DTO para el Endpoint 2: GetHitFrecPorMaquina
    /// (Recibe los datos de la tabla HitFrec_ResumenPorMaquina)
    /// </summary>
    public class HitFrecRankingDto {
        public DateTime FechaOperativa { get; set; }
        public string CodSala { get; set; }
        public string CodMaq { get; set; }
        public int TotalJuegos { get; set; }
        public int TotalHits { get; set; }
        public double HitFrequencyPorcentaje { get; set; }
    }

    /// <summary>
    /// DTO para el Endpoint 3: GetHitFrecLogDetallado (Sustento)
    /// (Recibe los datos de la tabla HitFrec_LogDetallado)
    /// </summary>
    public class HitFrecLogDto {
        public DateTime FechaHora { get; set; }
        public string CodMaq { get; set; }
        public long GamesPlayed { get; set; }
        public long CoinOut { get; set; }
        public long HandPay { get; set; }
        public long Jackpot { get; set; }
        public long CancelCredits { get; set; }
        // (Estos son los contadores incrementales que guardamos)
        public int ConteoGamesPlayed { get; set; }
        public int ConteoHitFrecuency { get; set; }
    }
}

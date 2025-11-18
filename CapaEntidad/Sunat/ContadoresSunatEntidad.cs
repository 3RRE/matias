using System;

namespace CapaEntidad.Sunat {
    public class ContadoresSunatEntidad {
        //public int IdContadorSunat { get; set; }
        public int CodSala { get; set; }
        public string Sala { get; set; }
        public DateTime FechaMigracion { get; set; } = DateTime.Now;

        public DateTime Fecha { get; set; }
        public dynamic Trama { get; set; }
        public bool Cereo { get; set; }
        public int IdConSunat { get; set; }
        public int Envio { get; set; }
        public string IdCereo { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaProceso { get; set; }
        public string Motivo { get; set; }
        public int IdConfSunat { get; set; }
        public int BandBusq { get; set; }
        //Datos de Trama
        public string Cabecera { get; set; }
        public string DGJM { get; set; }
        public string CodMaq { get; set; }
        public string FechaTrama { get; set; }
        public string Reserva1 { get; set; }
        public string Moneda { get; set; }
        public string Denominacion { get; set; }
        public string CoinInFinal { get; set; }
        public string CoinOutFinal { get; set; }
        public string PagoManualFinal { get; set; }
        public string OtroFinal { get; set; }
        public string TipoCambio { get; set; }
    }
}

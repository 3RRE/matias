using System;

namespace CapaEntidad.Sunat {
    public class EventosSunatEntidad {
        //public int IdEventoSunat { get; set; }
        public int CodSala { get; set; }
        public string Sala { get; set; }
        public DateTime FechaMigracion { get; set; } = DateTime.Now;

        public DateTime Fecha { get; set; }
        public dynamic Trama { get; set; }
        public bool TipoTrama { get; set; }
        public int IdEvSunat { get; set; }
        public int Envio { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaProceso { get; set; }
        public string Motivo { get; set; }
        public int IdConfSunat { get; set; }
        public int BandBusq { get; set; }
        //Datos de Trama
        public string Cabecera { get; set; }
        public string DGJM { get; set; }
        public string CodMaq { get; set; }
        public string IdColector { get; set; }
        public string FechaTrama { get; set; }
        public string Pccm { get; set; }
        public string Pccsuctr { get; set; }
        public string Rce { get; set; }
        public string Embbram { get; set; }
        public string Apl { get; set; }
        public string Fmc { get; set; }
        public string Frammr { get; set; }
        public string CereoTrama { get; set; }
        public string Reserva1 { get; set; }
        public string Reserva2 { get; set; }
    }
}

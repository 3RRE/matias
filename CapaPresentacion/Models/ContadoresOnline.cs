using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public class ContadoresOnline
    {
        public int Cod_Cont_OL { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodMaq { get; set; }
        public double CoinIn { get; set; }
        public double CoinOut { get; set; }
        public double HandPay { get; set; }
        public double CurrentCredits { get; set; }
        public double Monto { get; set; }
        public double EftIn { get; set; } 
        public double CancelCredits { get; set; }
        public double Jackpot { get; set; }
        public double GamesPlayed { get; set; }
        public double TrueIn { get; set; }
        public double TrueOut { get; set; }
        public double TotalDrop { get; set; } 
        public double Bill { get; set; }
        public string NroTiket { get; set; }
        public double TicketIn { get; set; }
        public double TicketOut { get; set; }
        public double TicketBonusIn { get; set; }
        public double TicketBonusOut { get; set; }
        public double MontoTiket { get; set; }
        public int codevento { get; set; }
        public double Token { get; set; }
        public string crc { get; set; }
        public double tmpebw { get; set; }
        public double tapebw { get; set; }
        public double tappw { get; set; }
        public double tmppw { get; set; }
        public int codcli { get; set; } 
        public int CONT_OLN_REMOTO { get; set; }  
    }
}
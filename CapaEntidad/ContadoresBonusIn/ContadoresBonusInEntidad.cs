using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ContadoresBonusIn {
    public class ContadoresBonusInCompleto {
        public int Cod_ContadoresBonusIn { get; set; }
        public Int64 Cod_Cont_OL { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodMaq { get; set; }
        public string CodMaqMin { get; set; }
        public string CodTarjeta { get; set; }
        public float CoinIn { get; set; }
        public float CoinOut { get; set; }
        public float HandPay { get; set; }
        public float CurrentCredits { get; set; }
        public float Monto { get; set; }
        public float EftIn { get; set; }
        public float EftOut { get; set; }
        public float CancelCredits { get; set; }
        public float Jackpot { get; set; }
        public float GamesPlayed { get; set; }
        public float TrueIn { get; set; }
        public float TrueOut { get; set; }
        public float TotalDrop { get; set; }
        public float Bill { get; set; }
        public Int64 Bill1 { get; set; }
        public Int64 Bill2 { get; set; }
        public Int64 Bill5 { get; set; }
        public Int64 Bill10 { get; set; }
        public Int64 Bill20 { get; set; }
        public Int64 Bill50 { get; set; }
        public Int64 Bill100 { get; set; }
        public string NroTiket { get; set; }
        public float TicketIn { get; set; }
        public float TicketOut { get; set; }
        public float TicketBonusIn { get; set; }
        public float TicketBonusOut { get; set; }
        public Int64 MontoTiket { get; set; }
        public float Progresivo { get; set; }
        public string Enviado { get; set; }
        public int codevento { get; set; }
        public int codcli { get; set; }
        public int codsuper { get; set; }
        public int CodPer { get; set; }
        public int CodAtencion { get; set; }
        public int CodCuadre { get; set; }
        public float PreCredito { get; set; }
        public float Token { get; set; }
        public string crc { get; set; }
        public string PorD { get; set; }
        public string Tficha { get; set; }
        public float tmpebw { get; set; }
        public float tapebw { get; set; }
        public float tappw { get; set; }
        public float tmppw { get; set; }
        public string Empresa { get; set; }
        public string Sala { get; set; }
        public int CodSala { get; set; }
        public DateTime fOperacion {  get; set; }
    }

    public class ContadoresOnlineBoton {
        public Int64 Cod_Cont_OL { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodMaq { get; set; }
        public int CodSala { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_SesionSorteoSalaMigracion
    {
        public long SesionId { get; set; }

        public long SorteoId { get; set; }

        public long JugadaId { get; set; }

        public int CantidadCupones { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string SerieIni { get; set; }

        public string SerieFin { get; set; }

        public string NombreSorteo { get; set; }

        public decimal CondicionWin { get; set; }

        public decimal WinCalculado { get; set; }

        public decimal CondicionBet { get; set; }

        public decimal BetCalculado { get; set; }

        public int TopeCuponesxJugada { get; set; }

        public string ParametrosImpresion { get; set; }

        public long Cod_Cont_OL { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime Hora { get; set; }

        public string CodMaq { get; set; }

        public double CoinOut { get; set; }

        public double CurrentCredits { get; set; }

        public decimal Monto { get; set; }

        public decimal Token { get; set; }

        public double CoinOutAnterior { get; set; }

        public int Estado_Oln { get; set; }

        public double HandPay { get; set; }

        public double JackPot { get; set; }

        public double HandPayAnterior { get; set; }

        public double JackPotAnterior { get; set; }

        public decimal CoinIn { get; set; }

        public decimal CoinInAnterior { get; set; }
        public long SesionMigracionId { get; set; }
        public long Id { get; set; }

    }
}

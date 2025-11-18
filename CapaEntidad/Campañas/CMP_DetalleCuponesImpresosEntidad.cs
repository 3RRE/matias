using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_DetalleCuponesImpresosEntidad
    {
        public Int64 DetImpId { get; set; }
        public Int64 CgId { get; set; }
        public int CodSala { get; set; }
        public string SerieIni { get; set; }
        public string SerieFin { get; set; }
        public int CantidadCuponesImpresos { get; set; }
        public string UltimoCuponImpreso { get; set; }


        public string CodMaq { get; set; }
        public double CoinOutAnterior { get; set; }
        public double CoinOut { get; set; }
        public double CoinOutIas { get; set; }
        public double CurrentCredits { get; set; }
        
        public decimal Monto { get; set; }
        public decimal Token { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Int64 id { get; set; }
        //Nuevos Campos
        public double HandPay { get; set; }
        public double JackPot { get; set; }
        public double HandPayAnterior { get; set; }
        public double JackPotAnterior { get; set; }
        public long Cod_Cont { get; set; }
        public long SesionId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_ContadoresOnlineWebCuponesEntidad
    {
        public Int64 id { get; set; }
        public Int64 Cod_Cont { get; set; }
        public Int64 Cod_Cont_OL { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodMaq { get; set; }
        public string CodMaqMin { get; set; }
        public double CoinOut { get; set; }
        public double CurrentCredits { get; set; }
        public double Monto { get; set; }
        public double Token { get; set; }
        public double CoinOutAnterior { get; set; }
        public int Estado_Oln { get; set; }
        public double Win { get; set; }
        public int CantidadCupones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodCliente { get; set; }
        public double CoinOutIas { get; set; }
        public int EstadoEnvio { get; set; }
        public string CodSala { get; set; }
        public DateTime FechaLlegada { get; set; }
        public Int64 DetalleCuponesImpresos_id { get; set; }
        public List<CMP_DetalleCuponesGeneradosEntidad> ListaDetalleIASCupones { get; set; }
        //Nuevos Campos
        public double HandPay { get; set; }
        public double JackPot { get; set; }
        public double HandPayAnterior { get; set; }
        public double JackPotAnterior { get; set; }
        public string SerieIni { get; set; }
        public string SerieFin { get; set; }
        public CMP_ContadoresOnlineWebCuponesEntidad()
        {
            ListaDetalleIASCupones = new List<CMP_DetalleCuponesGeneradosEntidad>();
        }
    }
}

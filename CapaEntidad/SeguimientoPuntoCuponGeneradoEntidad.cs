using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SeguimientoPuntoCuponGeneradoEntidad
    {
        public int CodU { get; set; }
        public string NombrePromoSorteo { get; set; }
        public string DescripcionPromoSorteo { get; set; }
        public string Cliente { get; set; }
        public int puntocupontotalxregjuego { get; set; }
        public string SlotID { get; set; }
        public double MontoCoinIn { get; set; }
        public double MontoCoinOut { get; set; }
        public double MontoHandPay { get; set; }
        public string TarjetaRFID { get; set; }
        public string doi { get; set; } 
    }
}

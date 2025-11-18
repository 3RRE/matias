using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ResumenSalaEntidad
    {
        public int codResumen { get; set; }
        public DateTime fechaOperacion { get; set; }
        public int estado { get; set; }
        public string usuario { get; set; }
        public double saldoinicialsoles { get; set; }
        public double fondocaja { get; set; }
        public double salidaotros { get; set; }
        public double ingresootros { get; set; }
        public double prestamoaotrasala { get; set; }
        public double prestamodeotrasala { get; set; }
        public double devolucionaotrasala { get; set; }
        public double devoluciondeotrasala { get; set; }
        public double prestamoaoficina { get; set; }
        public double refuerzodefondo { get; set; }
        public DateTime fecharegistro { get; set; }
        public double sobrantes { get; set; }
        public double faltantes { get; set; }
        public double sorteos { get; set; }
        public double monedasfallas { get; set; }
        public double visa { get; set; }
        public double efectivofinal { get; set; }
        public double efectivoSoles { get; set; }
        public double efectivoDolares { get; set; }
        public double mastercard { get; set; }
        public double otrastarjetas { get; set; }
        public int codResumenSala { get; set; }
    }
}

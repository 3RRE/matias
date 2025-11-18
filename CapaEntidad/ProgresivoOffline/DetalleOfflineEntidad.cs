using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ProgresivoOffline
{
    public class DetalleOfflineEntidad
    {

        public int IdDetalleProgresivo { get; set; }
        public int IdCabeceraProgresivo { get; set; }
        public string CodMaq { get; set; }
        public double codevento { get; set; }
        public double Bonus1 { get; set; }
        public double Bonus2 { get; set; }
        public double Dif_Bonus1 { get; set; }
        public double Dif_Bonus2 { get; set; }
        public double CurrentCredits { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaStr { get; set; }
        public DateTime Hora { get; set; }
        public string HoraStr { get; set; }
        public DateTime FechaCompleta { get; set; }
    }
}

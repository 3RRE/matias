using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso
{
    public class CAL_PoliticoEntidad
    {
        public int PoliticoID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public bool Estado { get; set; }
        public int CargoPoliticoID { get; set; }
        public string Dni { get; set; }
        public string EntidadEstatal { get; set; }
        public decimal Meses { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string cargoPoliticoNombre { get; set; }
        public string descripcionPoliticoNombre { get; set; }

        public int TipoDOI { get; set; }
    }
}

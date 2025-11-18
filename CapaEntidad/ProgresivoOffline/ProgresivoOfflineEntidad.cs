using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ProgresivoOffline
{
    public class ProgresivoOfflineEntidad
    {
        public int IdProgresivo { get; set; }
        public int CodSala { get; set; }
        public int WEB_PrgID { get; set; }
        public string WEB_Nombre { get; set; }
        public int WEB_NroPozos { get; set; }
        public string WEB_Url { get; set; }
        public string WEB_Estado { get; set; }
        public DateTime WEB_FechaRegistro { get; set; }
    }
}

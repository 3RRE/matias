using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_CampañaEntidad
    {
        public Int64 id { get; set; }
        public int sala_id { get; set; }
        public string nombre { get; set;}
        public string nombresala { get; set; }
        public string descripcion { get; set; }
        public DateTime fechareg { get; set; } 
        public DateTime fechaini { get; set; }
        public DateTime fechafin { get; set; }
        public int usuario_id { get; set; }
        public string usuarionombre { get; set; }
        public int estado { get; set; }
        public int tipo { get; set; }
        public string UrlProgresivo { get; set; }
        //Para campañas de tipo WhatsApp
        public string mensajeWhatsApp { get; set; }
        public int duracionCodigoDias { get; set; }
        public int duracionCodigoHoras { get; set; }
        public bool codigoSeReactiva { get; set; }
        public int duracionReactivacionCodigoDias { get; set; }
        public int duracionReactivacionCodigoHoras { get; set; }
        public string mensajeWhatsAppReactivacion { get; set; }

        public bool Existe() {
            return id > 0;
        }
    }
}

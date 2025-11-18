using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class BonificacionesEntidad
    {
        public Int64 bon_id { get; set; }
        public int CodSala { get; set; }
        public string nombresala { get; set; }
        public DateTime bon_fecha { get; set; }
        public string bon_documento { get; set; }
        public string bon_nombre { get; set; }
        public string bon_apepaterno { get; set; }
        public string bon_apematerno { get; set; }

        public float bon_monto { get; set; }
        public string bon_ticket { get; set; }
        public DateTime bon_fecharegistro { get; set; }
        public int UsuarioID { get; set; }
        public string nombreusuario { get; set; }
        public int bon_estado { get; set; }
    }
}

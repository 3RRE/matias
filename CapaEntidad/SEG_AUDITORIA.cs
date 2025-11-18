using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class SEG_AUDITORIA
    {
        public int codAuditoria { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string usuario { get; set; }
        public string proceso { get; set; }
        public string descripcion { get; set; }
        public string subsistema { get; set; }
        public string ip { get; set; }
        public string usuariodata { get; set; }//session usuario 
        public string datainicial { get; set; }
        public string datafinal{ get; set; }
        public int codSala{ get; set; }
        public string sala { get; set; }
        public int formularioID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ClienteSatisfaccion.Entidad {
    public class TabletEntidad {
        public int IdTablet { get; set; }
        public string Guid { get; set; }
        public string Nombre { get; set; }
        public int SalaId { get; set; }
        public bool Activa { get; set; }
    }
}

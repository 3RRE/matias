using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso
{
    public class CAL_CodigoPersonaEntidad
    {

        public int Id { get; set; }
        public string TipoPersona { get; set; }
        public int CodigoID { get; set; }
        public string CodigoNombre { get; set; }
        public int Editable { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class MarcaMaquinaEntidad
    {
        public int CodMarcaMaquina { set; get; }
        public string Nombre { set; get; }
        public string ColorHexa { set; get; }
        public string Sigla { set; get; }
        public DateTime FechaRegistro { set; get; }
        public DateTime FechaModificacion { set; get; }
        public int Activo { set; get; }
        public int Estado { set; get; }
        public int CodRD { set; get; }
        public string CodUsuario { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_CorreoEntidad {
        public int CodCorreo { get; set; }
        public int CodMaquinaInoperativa { get; set; }
        public int CodUsuario { get; set; }
        public int CodEstadoProceso { get; set; }
        public int CantEnvios { get; set; }
        public string UsuarioMail { get; set; }
        public string UsuarioNombre { get; set; }
    }
}

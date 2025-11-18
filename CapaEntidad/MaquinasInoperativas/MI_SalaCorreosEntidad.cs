using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas
{
    public class MI_SalaCorreosEntidad
    {
        public int CodSalaCorreos { get; set; }
        public int CodSala { get; set; }
        public int CodUsuario { get; set; }
        public int CodTipo { get; set; }
        public string UsuarioNombre { get; set; }
        public string Mail { get; set; }
        public string NombreMail { get; set; }
    }
}

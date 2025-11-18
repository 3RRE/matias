using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ProgresivoSeguridad
{
    public class Signalr_usuarioEntidad
    {
        public Int64 sgn_id { get; set; }
        public Int32 usuario_id { get; set; }
        public string UsuarioNombre { get; set; }
        public string RolNombre { get; set; }
        public string sgn_conection_id { get; set; }
        public DateTime sgn_fechaUpdate { get; set; }
        public string sgn_token { get; set; }
        public int sgn_estado { get; set; }
        public int sala_id { get; set; }
        public string SalaNombre { get; set; }
    }
}

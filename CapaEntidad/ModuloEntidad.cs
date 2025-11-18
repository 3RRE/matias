using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ModuloEntidad
    {
        public int ModuloId { get; set; }
        public int SistemaID { get; set; }
        public string SistemaDescripcion { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Estado { get; set; }


    }
}

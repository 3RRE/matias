using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EstadoServiciosEntidad
    {
        public int Id { get; set; }
        public int CodSala { get; set; }
        public bool EstadoWebOnline { get; set; }
        public bool EstadoGladconServices { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string NombreSala { get; set; }

    }
}

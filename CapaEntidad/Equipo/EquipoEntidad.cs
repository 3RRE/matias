using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Equipo
{
    public class EquipoEntidad
    {
        public int IdEquipoInfo  { get;set;}
        public string MemoriaTotal { get;set;}
        public string MemoriaDisponible { get;set;}
        public string MemoriaUsada { get;set; }
        public string PorcentajeUsoRam { get; set; }
        public string PorcentajeCpu { get; set; }
        public string VelocidadCpu { get; set; }
        public int ProcesosCpu { get; set; }
        public int CodSala { get; set; }
        public string IpEquipo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administrativo
{
    public class ADM_MaquinaSalaProgresivoEntidad
    {
        public int CodMaquinaSalaProgresivo { get; set; }

        public int CodMaquina { get; set; }

        public int CodSalaProgresivo { get; set; }

        public DateTime FechaEnlace { get; set; }

        public DateTime FechaDesactivacion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public int Estado { get; set; }

        public string CodUsuario { get; set; }
        public int CodProgresivoWO { get; set; }
        public string CodMaquinaLey { get; set; }
        public string CodAlterno { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administrativo
{
    public class ADM_PozoHistoricoEntidad
    {
        public int CodPozoHistorico { get; set; }

        public int CodDetalleSalaProgresivo { get; set; }

        public decimal MontoActualAutomatico { get; set; }

        public decimal MontoActualSala { get; set; }

        public decimal MontoOcultoActualAutomatico { get; set; }

        public decimal MontoOcultoActualSala { get; set; }

        public DateTime FechaOperacion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int Estado { get; set; }

        public bool Activo { get; set; }

        public string CodUsuario { get; set; }
    }
}

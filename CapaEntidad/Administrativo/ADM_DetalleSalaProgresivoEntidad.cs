using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administrativo
{
    public class ADM_DetalleSalaProgresivoEntidad
    {
        public int CodDetalleSalaProgresivo { get; set; }

        public int CodSalaProgresivo { get; set; }

        public int NroPozo { get; set; }

        public string NombrePozo { get; set; }

        public int Dificultad { get; set; }

        public decimal MontoBase { get; set; }

        public decimal MontoIni { get; set; }

        public decimal MontoFin { get; set; }

        public int Modalidad { get; set; }

        public decimal MontoOcultoBase { get; set; }

        public decimal MontoOcultoIni { get; set; }

        public decimal MontoOcultoFin { get; set; }

        public decimal Incremento { get; set; }

        public decimal IncrementoPozoOculto { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public int Estado { get; set; }

        public int CodProgresivoExterno { get; set; }

        public string CodUsuario { get; set; }

        public DateTime fechaIni { get; set; }

        public DateTime fechaFin { get; set; }
        public List<ADM_PozoHistoricoEntidad> PozoHistorico { get; set; }
        public ADM_DetalleSalaProgresivoEntidad()
        {
            PozoHistorico = new List<ADM_PozoHistoricoEntidad>();
        }
    }
}

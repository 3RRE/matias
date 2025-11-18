using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administrativo
{
    public class ADM_SalaProgresivoEntidad
    {
        public int CodSalaProgresivo { get; set; }

        public int CodSala { get; set; }

        public int CodProgresivo { get; set; }

        public int NroPozos { get; set; }

        public int NroJugadores { get; set; }

        public int SubidaCreditos { get; set; }

        public DateTime FechaInstalacion { get; set; }

        public DateTime FechaDesinstalacion { get; set; }

        public string ColorHexa { get; set; }

        public string Sigla { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public int Estado { get; set; }

        public string Url { get; set; }

        public string CodUsuario { get; set; }

        public int CodProgresivoWO { get; set; }

        public int CodTipoConfiguracionProgresivo { get; set; }
        public string Nombre { get; set; }
        public string TipoProgresivo { get; set; }
        public int ProgresivoID { get; set; }
        public string NombreSala { get; set; }
        public string RazonSocial { get; set; }
        public string ClaseProgresivo { get; set; }
        public List<ADM_DetalleSalaProgresivoEntidad> DetalleSalaProgresivo { get; set; }
        public ADM_SalaProgresivoEntidad()
        {
            DetalleSalaProgresivo = new List<ADM_DetalleSalaProgresivoEntidad>();
        }
    }
}

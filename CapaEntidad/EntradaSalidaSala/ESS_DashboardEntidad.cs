using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala
{
    namespace CapaEntidad.EntradaSalidaSala
    {
        public class ESS_DashboardEntidad
        {
            public List<ESS_DashboardLudopatasEntidad> Ludopatas { get; set; }

        }

        public class ESS_DashboardLudopatasEntidad
        {
            public string NombreSala { get; set; }
            public string DNI { get; set; }
            public DateTime FechaIngreso { get; set; }
        }


        public class ESS_DashboardReacudacionEntidad
        {
            public int IdRecaudacion { get; set; }
            public int IdPersonalRecaudacion { get; set; }
            public string NombreSala { get; set; }
            public int Estado { get; set; }
            public string NombreFuncion { get; set; }
        }

        public class ESS_DashboardCajasTemporizadasEntidad
        {
            public string NombreSala { get; set; }
            public string NombreDeficiencia { get; set; }
            public string NombreEstado { get; set; }
        }

        public class ESS_DashboardEnteReguladoraEntidad
        {
            public string NombreSala { get; set; }
            public string EnteReguladora { get; set; }
            public string NombreMotivo { get; set; }
        }

        public class ESS_DashboardOcurrenciasLogEntidad
        {
            public string NombreSala { get; set; }
            public string NombreTipologia { get; set; }
            public string NombreEstado { get; set; }
        }
    }
}
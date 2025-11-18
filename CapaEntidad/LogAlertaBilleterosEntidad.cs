using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class LogAlertaBilleterosEntidad
    {
        public Int64 Id { get; set; }
        public int CodSala { get; set; }
        public string Descripcion { get; set; }
        public int Tipo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Int64 Cod_Even_OL { get; set; }
        public string Preview { get; set; }
        public string NombreSala { get; set; }
    }
    public enum TipoLog { EventoServicioOnline=1,EventoOnlineTecnologias=2, AlertaBilletero = 3, }

}

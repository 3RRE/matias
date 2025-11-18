using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EventosSignificativos {
    public class EventosSignificativosEntidad {
        public int IdEventoSignificativo { get; set; }
        public int Cod_Even_OL { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public string CodTarjeta { get; set; }
        public string CodMaquina { get; set; }
        public int Cod_Evento { get; set; }
        public string COD_EMPRESA { get; set; }
        public string COD_SALA { get; set; }
        public string NombreEvento { get; set; }
    }
}

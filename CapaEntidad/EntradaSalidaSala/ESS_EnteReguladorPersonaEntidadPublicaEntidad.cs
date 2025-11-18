using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_EnteReguladorPersonaEntidadPublicaEntidad {
        public int PersonaEntidadPublicaID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int? Estado { get; set; }
        public int? EntidadPublicaID { get; set; }
        public string Dni { get; set; }
        public int? CargoEntidadID { get; set; }
        public decimal? Meses { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? TipoDOI { get; set; }
        public string CargoEntidadNombre { get; set; }
        public string EntidadPublicaNombre { get; set; }
    }
}

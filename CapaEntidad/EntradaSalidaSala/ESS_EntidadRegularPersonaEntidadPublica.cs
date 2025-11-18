using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.EntradaSalidaSala {

    public class ESS_EntidadRegularPersonaEntidadPublica {
        public int IdEntidadRegularPersonaEntidadPublica { get; set; } 
        public int PersonaEntidadPublicaID { get; set; } 
        public int IdEnteRegulador { get; set; } 
        public string Nombres { get; set; }
        public string Apellidos { get; set; } 
        public int Estado { get; set; } 
        public int IdEntidadPublica { get; set; } 
        public string EntidadPublicaNombre { get; set; } 
        public string Dni { get; set; } 
        public int IdCargoEntidad { get; set; } 
        public string CargoEntidadNombre { get; set; } 
        public DateTime FechaRegistro { get; set; } 
        public string TipoDOI { get; set; } 
    }


}

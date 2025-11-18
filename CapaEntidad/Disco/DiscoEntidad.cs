
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Discos {
    /// <summary>
    /// idDisco:Nombre de la particion (D:, C:)
    /// </summary>

    public class DiscoEntidad {
        
        public string nombreSala { get; set; }
        public int idDisco { get; set; }
        public int condicionDisco { get; set; }
        public int codSala { get; set; }
        public string nombreDisco { get; set; }
        public string ipServidor { get; set; }
        public string seudonimoDisco { get; set; }
        public string tipoDisco { get; set; }
        public string sistemaDisco { get; set; }
        public string capacidadTotal { get; set; }
        public string capacidadEnUso { get; set; }
        public string capacidadLibre { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
    public class EspacioDiscoBD {

        public int Id { get; set; }
        public string NombreBD { get; set; }
        public string EspacioBD { get; set; }
        public string NombreLog { get; set; }
        public string EspacioLog { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

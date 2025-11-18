using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Marca
    {
        public long MarcaID { get; set; }
        public String Nombre { get; set; }
        public long Estado { get; set; }
        public String Estado_desc { get; set; }
        public long indice { get; set; }
    }

    public class MaquinaJuego {

        public String Juego { get; set; }
    }

}

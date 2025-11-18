using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ContadoresNegativos
{
    public class ContadorNegativoConfigEntidad
    {
       
            public int idContadorConfigCargo { get; set; }
            public int cargo_id { get; set; }
            public string cargo_nombre { get; set; }
            public int sala_id { get; set; }
            public string sala_nombre { get; set; }
            public string empresa_nombre { get; set; }
            public DateTime fechaRegistro { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_SalalibreEntidad
    {
        public Int64 CodSala { get; set; }
        public Int64 CodEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string nombresala { get; set; }
        public Int64 id { get; set; }
        public Int64 Salasesion_id { get; set; }
        
        public DateTime fechareg { get; set; }
        public int estado { get; set; }
    }
}

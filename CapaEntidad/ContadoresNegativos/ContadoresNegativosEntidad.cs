using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ContadoresNegativos
{
    public class ContadoresNegativosEntidad
    {

        public int IdContadorNegativo { get; set; }
        public int CodEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public string CodMaquina { get; set; }
        public DateTime FechaRegistroSala { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Int64 CodigoId { get; set; }

    }
}

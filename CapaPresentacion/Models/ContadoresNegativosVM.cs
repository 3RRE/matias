using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public class ContadoresNegativosVM
    {
        public int IdContadorNegativo { get; set; }
        public int CodEmpresa { get; set; }
        //public string NombreEmpresa { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public string CodMaquina { get; set; }
        public string FechaRegistroSala { get; set; }
        public string Descripcion { get; set; }
        public string FechaRegistro { get; set; }
        public Int64 CodigoId { get; set; }

    }
}
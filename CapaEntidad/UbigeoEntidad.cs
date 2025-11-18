using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class UbigeoEntidad
    {
        public int CodUbigeo { get; set; }
        public string PaisId { get; set; }
        public int DepartamentoId { get; set; }
        public int ProvinciaId { get; set; }
        public int DistritoId { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Activo { get; set; }
        public int Estado { get; set; }
        public string Bandera { get; set; }
        public string CodigoTelefonico { get; set; }
    }
}

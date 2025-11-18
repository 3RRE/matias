using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_DetalleCuponesGeneradosEntidad
    {
        public Int64 DetGenId { get; set; }
        public Int64 DetImId { get; set; }
        public int CodSala { get; set; }
        public string Serie { get; set; }
        public int CantidadImpresiones { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        //Data Extra
        public string NombreSala { get; set; }
        public string RucEmpresa { get; set; }
        public string RazonSocialEmpresa { get; set; }
        public string DniCliente { get; set; }
        public string NombreCliente { get; set; }
        public string FechaString { get; set; }
    }
}

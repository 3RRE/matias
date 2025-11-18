using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.MaquinasInoperativas
{
    public class MI_TraspasoRepuestoAlmacenEntidad
    {
        public int CodTraspasoRepuestoAlmacen { get; set; }
        public int CodMaquinaInoperativa { get; set; }
        public int CodAlmacenOrigen { get; set; }
        public int CodAlmacenDestino { get; set; }
        public int CodRepuesto { get; set; }
        public int CodPiezaRepuestoAlmacen { get; set; }
        public int Cantidad { get; set; }
        public int Estado { get; set; }
        public int CodUsuarioRemitente { get; set; }
        public int CodUsuarioDestinatario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public string NombreRepuesto { get; set; }
        public string NombreAlmacenOrigen { get; set; }
        public string NombreAlmacenDestino { get; set; }
        public string NombreSala { get; set; }
    }
}

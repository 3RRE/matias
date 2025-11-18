using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ControlAcceso
{
    public class CAL_BandaEntidad
    {

        public int BandaID { get; set; }
        public string Descripcion { get; set; }
        public int CodUbigeo { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string codPais { get; set; }
        public string codDepartamento { get; set; }
        public string codProvincia { get; set; }
        public string codDistrito { get; set; }
        public string NombreUbigeo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Estado { get; set; }
    }
}

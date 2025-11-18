using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class AumentoCreditoMaquinaEntidad
    {
        public int Id { get; set; }
        public string CodMaq { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaUltimoAumento { get; set; }
        public int Cantidad { get; set; }
        public int UsuarioRegistro { get; set; }
        public int CodSala { get; set; }
        public int PuertoSignalr{ get; set; }
        public int Hora{ get; set; }
        public int Minuto{ get; set; }
    }
}

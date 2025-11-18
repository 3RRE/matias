using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Campañas
{
    public class CMP_MaquinaRestringidaEntidad
    {
        public string CodMaquina { get; set; }
        public int CodSala{ get; set; }
        public string Juego { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Restringido { get; set; }
        public int UsuarioId { get; set; }
        public int EstadoOnline { get; set; }
    }
}

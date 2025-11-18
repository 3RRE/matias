using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ocurrencias
{
    public class OCU_CorreoSalaEntidad
    {
        public int CorreoId { get; set; }
        public int SalaId { get; set; }
        public OCU_CorreoEntidad Correo { get; set; }
        public SalaEntidad Sala{ get; set; }
        public OCU_CorreoSalaEntidad()
        {
            this.Correo = new OCU_CorreoEntidad();
            this.Sala = new SalaEntidad();
        }
    }
}

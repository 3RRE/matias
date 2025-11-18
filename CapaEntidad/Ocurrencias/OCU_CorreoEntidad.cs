using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ocurrencias
{
    public class OCU_CorreoEntidad
    {
        public int Id { get; set; }
        public int CodTipoCorreo { get; set; }//1.- Remitente , 2.- Destinatario
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int SSL { get; set; }
        public string Smtp { get; set; }
        public int Puerto { get; set; }
        public int Estado { get; set; }
        public List<int> arraySalas { get; set; }
        public List<OCU_CorreoSalaEntidad> ListaCorreoSala { get; set; }
        public string PasswordEncriptado{ get; set; }
        public OCU_CorreoEntidad()
        {
            this.ListaCorreoSala = new List<OCU_CorreoSalaEntidad>();
        }
    }
}

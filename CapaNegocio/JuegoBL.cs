using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class JuegoBL
    {
        private JuegoDAL juegodal = new JuegoDAL();

        public List<JuegoEntidad> ListarJuegoMaquinaJson(int id)
        {
            return juegodal.ListaJuegoMaquinaJson(id);
        }
    }
}

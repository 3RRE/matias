using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TipoCambioBL
    {
        private TipoCambioDAL tipocambiodal = new TipoCambioDAL();

        public List<TipoCambioEntidad> ListaTipoCambioJson()
        {
            return tipocambiodal.ListaTipoCambioJson();
        }
    }
}

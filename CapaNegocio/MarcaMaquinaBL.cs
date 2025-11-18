using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class MarcaMaquinaBL
    {
        private MarcaMaquinaDAL marcamaquinadal = new MarcaMaquinaDAL();

        public List<MarcaMaquinaEntidad> MarcaMaquinaListaJson()
        {
            return marcamaquinadal.MarcaMaquinaListaJson();
        }
    }
}

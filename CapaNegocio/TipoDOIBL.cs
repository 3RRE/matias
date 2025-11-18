using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TipoDOIBL
    {
        private TipoDOIDAL tipoDoidal = new TipoDOIDAL();
        public List<TipoDOIEntidad> TipoDocumentoListarJson()
        {
            return tipoDoidal.TipoDocumentoListarJson();
        }
    }
}

using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using CapaNegocio.EntradaSalidaSala; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_CatalogoBL {
        private ESS_CatalogoDAL _catalogoDal = new ESS_CatalogoDAL();
         

        public List<ESS_cboCatalogoEntidad> ListadocboCatalogo(int IdTipoCatalogo) {
            return _catalogoDal.ListadoCatalogo(IdTipoCatalogo);
        }
    }
    
}

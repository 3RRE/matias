using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio {
    public class API_ReniecKeys {
        API_ReniecKeysDAL _reniec;
        public API_ReniecKeys() {
            _reniec = new API_ReniecKeysDAL();
        }
        public List<API_ReniecKeysEntidad> ListaKeys() {
            return _reniec.ListaKeys();
        }
    }
}

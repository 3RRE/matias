using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class BancoCuentaBL
    {
        private BancoCuentaDAL bancocuentaDal = new BancoCuentaDAL();
        public List<BancoCuentaEntidad> BancoCuentaListadoJson()
        {
            return bancocuentaDal.BancoCuentaListadoJson();
        }

        public List<BancoCuentaEntidad> BancoCuentaclienteidListadoJson(int id)
        {
            return bancocuentaDal.BancoCuentaclienteidListadoJson(id);
        }
        public BancoCuentaEntidad BancoCuentaObtenerJson(int id)
        {
            return bancocuentaDal.BancoCuentaObtenerJson(id);
        }
        public bool BancoCuentaInsertarJson(BancoCuentaEntidad bancoCuenta)
        {
            return bancocuentaDal.BancoCuentaInsertarJson(bancoCuenta);
        }
        public bool BancoCuentaEditarJson(BancoCuentaEntidad bancoCuenta)
        {
            return bancocuentaDal.BancoCuentaEditarJson(bancoCuenta);
        }
    }
}

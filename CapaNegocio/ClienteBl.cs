using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ClienteBl
    {
        private ClienteDAL clienteDal = new ClienteDAL();
        public List<Cliente> ClienteListadoJson()
        {
            return clienteDal.ClienteListadoJson();
        }
        public List<Cliente> ClienteBuscarJson(string nrodoc)
        {
            return clienteDal.ClienteBuscarJson(nrodoc);
        }
        public bool clienteInsertarJson(Cliente cliente)
        {
            return clienteDal.ClienteInsertarJson(cliente);
        }

        public bool ClienteEditarJson(Cliente cliente)
        {
            return clienteDal.ClienteEditarJson(cliente);
        }
    }
}

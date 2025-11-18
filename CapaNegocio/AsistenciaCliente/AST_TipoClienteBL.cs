using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaCliente
{
    public class AST_TipoClienteBL
    {
        private AST_TipoClienteDAL tipoDal = new AST_TipoClienteDAL();
        public List<AST_TipoClienteEntidad> GetListadoTipoCliente()
        {
            return tipoDal.GetListadoTipoCliente();
        }
        public AST_TipoClienteEntidad GetTipoClienteID(int TipoDocId)
        {
            return tipoDal.GetTipoClienteId(TipoDocId);
        }
        public int GuardarTipoCliente(AST_TipoClienteEntidad TipoCliente)
        {
            return tipoDal.GuardarTipoCliente(TipoCliente);
        }
        public bool EditarTipoCliente(AST_TipoClienteEntidad TipoCliente)
        {
            return tipoDal.EditarTipoCliente(TipoCliente);
        }
    }
}

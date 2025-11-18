using CapaDatos;
using CapaDatos.Campaña;
using CapaEntidad.Campañas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaNegocio.Campaña
{
    public class CMP_impresora_usuarioBL
    {
        private CMP_impresora_usuarioDAL impresora_usuario_dal = new CMP_impresora_usuarioDAL();
        public List<CMP_impresora_usuarioEntidad> ImpresoraUsuarioListadoCompletoJson()
        {
            return impresora_usuario_dal.GetListadoImpresoraUsuario();
        }

        public CMP_impresora_usuarioEntidad ImpresoraUsuarioIdObtenerJson(Int64 id)
        {
            return impresora_usuario_dal.GetImpresoraUsuarioID(id);
        }
        public Int64 ImpresoraUsuarioInsertarJson(CMP_impresora_usuarioEntidad impresora)
        {
            return impresora_usuario_dal.GuardarImpresoraUsuario(impresora);
        }
        public bool ImpresoraUsuarioEditarJson(CMP_impresora_usuarioEntidad impresora)
        {
            return impresora_usuario_dal.EditarImpresoraUsuario(impresora);
        }
        public bool ImpresoraUsuarioEliminarJson(Int64 id)
        {
            return impresora_usuario_dal.EliminarImpresoraUsuario(id);
        }
    }
}

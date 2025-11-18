using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaCliente
{
    public class AST_TipoJuegoBL
    {
        private AST_TipoJuegoDAL tipoDal = new AST_TipoJuegoDAL();
        public List<AST_TipoJuegoEntidad> GetListadoTipoJuego()
        {
            return tipoDal.GetListadoTipoJuego();
        }
        public AST_TipoJuegoEntidad GetTipoJuegoID(int TipoJuegoId)
        {
            return tipoDal.GetTipoJuegoId(TipoJuegoId);
        }
        public int GuardarTipoJuego(AST_TipoJuegoEntidad TipoJuego)
        {
            return tipoDal.GuardarTipoJuego(TipoJuego);
        }
        public bool EditarTipoJuego(AST_TipoJuegoEntidad TipoJuego)
        {
            return tipoDal.EditarTipoJuego(TipoJuego);
        }
    }
}

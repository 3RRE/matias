using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaCliente
{
    public class AST_TipoFrecuenciaBL
    {
        private AST_TipoFrecuenciaDAL tipoDal = new AST_TipoFrecuenciaDAL();
        public List<AST_TipoFrecuenciaEntidad> GetListadoTipoFrecuencia()
        {
            return tipoDal.GetListadoTipoFrecuencia();
        }
        public AST_TipoFrecuenciaEntidad GetTipoFrecuenciaID(int TipoDocId)
        {
            return tipoDal.GetTipoFrecuenciaId(TipoDocId);
        }
        public int GuardarTipoFrecuencia(AST_TipoFrecuenciaEntidad TipoFrecuencia)
        {
            return tipoDal.GuardarTipoFrecuencia(TipoFrecuencia);
        }
        public bool EditarTipoFrecuencia(AST_TipoFrecuenciaEntidad TipoFrecuencia)
        {
            return tipoDal.EditarTipoFrecuencia(TipoFrecuencia);
        }
    }
}

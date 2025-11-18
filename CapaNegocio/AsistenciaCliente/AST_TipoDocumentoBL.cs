using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.AsistenciaCliente
{
    public class AST_TipoDocumentoBL
    {
        private AST_TipoDocumentoDAL tipoDal = new AST_TipoDocumentoDAL();
        public List<AST_TipoDocumentoEntidad> GetListadoTipoDocumento()
        {
            return tipoDal.GetListadoTipoDocumento();
        }
        public AST_TipoDocumentoEntidad GetTipoDocumentoID(int TipoDocId)
        {
            return tipoDal.GetTipoDocumentoID(TipoDocId);
        }
        public int GuardarTipoDocumento(AST_TipoDocumentoEntidad tipoDocumento)
        {
            return tipoDal.GuardarTipoDocumento(tipoDocumento);
        }
    }
}

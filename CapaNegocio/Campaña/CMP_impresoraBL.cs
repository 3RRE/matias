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
    public class CMP_impresoraBL
    {
        private CMP_impresoraDAL impresora_dal = new CMP_impresoraDAL();
        public List<CMP_impresoraEntidad> ImpresoraListadoCompletoJson()
        {
            return impresora_dal.GetListado();
        }

        public List<CMP_impresoraEntidad> ImpresoraListadoxSala_idJson(Int64 sala_id)
        {
            return impresora_dal.GetListadoxSala_id(sala_id);
        }

        public List<CMP_impresoraEntidad> GetListadoxSala_idxUsuarioid(Int64 sala_id,Int64 usuario_id)
        {
            return impresora_dal.GetListadoxSala_idxUsuarioid(sala_id,usuario_id);
        }

        public CMP_impresoraEntidad ImpresoraIdObtenerJson(Int64 id)
        {
            return impresora_dal.GetImpresoraID(id);
        }
        public Int64 ImpresoraInsertarJson(CMP_impresoraEntidad impresora)
        {
            return impresora_dal.GuardarImpresora(impresora);
        }
        public bool ImpresoraEditarJson(CMP_impresoraEntidad impresora)
        {
            return impresora_dal.EditarImpresora(impresora);
        }
        public bool ImpresoraEliminarJson(Int64 id)
        {
            return impresora_dal.EliminarImpresora(id);
        }
    }
}

using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
   public class DestinatarioBL
    {
        private DestinatarioDAL destinatarioDal = new DestinatarioDAL();
        public List<DestinatarioEntidad> DestinatarioListadoJson()
        {
            return destinatarioDal.DestinatarioListadoJson();
        }
        public List<DestinatarioEntidad> DestinatarioListadoTipoEmailJson(int TipoEmail)
        {
            return destinatarioDal.DestinatarioListadoTipoEmailJson(TipoEmail);
        }        
        public DestinatarioEntidad DestinatarioObtenerJson(int id)
        {
            return destinatarioDal.DestinatarioObtenerJson(id);
        }        
        public bool DestinatarioInsertarJson(DestinatarioEntidad destinatario)
        {
            return destinatarioDal.DestinatarioInsertarJson(destinatario);
        }
        public bool DestinatarioEditarJson(DestinatarioEntidad destinatario)
        {
            return destinatarioDal.DestinatarioEditarJson(destinatario);
        }

        public List<DestinatarioEntidad> ListarDestinatariosAsinadosTipo(int tipo)
        {
            return destinatarioDal.ListarDestinatariosAsinadosTipo(tipo);
        }
    }
}

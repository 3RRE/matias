using CapaDatos.ProgresivoSeguridad;
using CapaEntidad.ProgresivoSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.SeguridadProgresivo
{
    public class SeguridadProgresivoBL
    {
        private Signalr_usuarioDAL seguridadProgresivonDAL = new Signalr_usuarioDAL();
        public List<Signalr_usuarioEntidad> GetListaSignalr_usuario()
        {
            return seguridadProgresivonDAL.GetListaSignalr_usuario();
        }
       
        public Signalr_usuarioEntidad GetSignalr_usuarioId(Int64 sgn_id)
        {
            return seguridadProgresivonDAL.GetSignalr_usuarioId(sgn_id);
        }
        public List<Signalr_usuarioEntidad> GetSignalr_usuarioIdxUsuarioID(Int64 usuario_id)
        {
            return seguridadProgresivonDAL.GetSignalr_usuarioIdxUsuarioID(usuario_id);
        }

        public bool GuardarSignalr(Signalr_usuarioEntidad sginalr)
        {
            return seguridadProgresivonDAL.GuardarSignalr(sginalr);
        }

        public Int64 GuardarSignalr_returnID(Signalr_usuarioEntidad sginalr)
        {
            return seguridadProgresivonDAL.GuardarSignalr_returnID(sginalr);
        }

        public bool ActualizarConection_id(Signalr_usuarioEntidad conectid)
        {
            return seguridadProgresivonDAL.ActualizarSignalruser(conectid);
        }
        public bool ActualizarConection_id(string conectid, Int64 usu_id)
        {
            return seguridadProgresivonDAL.ActualizarConection_id(conectid, usu_id,DateTime.Now);
        }
        public bool ActualizarToken(string token, Int64 sgn_id, int estado)
        {
            return seguridadProgresivonDAL.ActualizarToken(token, sgn_id, DateTime.Now,estado);
        }

        public bool EliminarSignalrid(Int64 sgn_id)
        {
            return seguridadProgresivonDAL.EliminarSignalrid(sgn_id);
        }

        public bool ActualizarSalaId(int sala_id, long sgn_id)
        {
            return seguridadProgresivonDAL.ActualizarSalaId(sala_id, sgn_id);
        }
    }
}

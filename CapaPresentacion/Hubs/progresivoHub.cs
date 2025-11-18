using CapaEntidad;
using CapaEntidad.ProgresivoSeguridad;
using CapaNegocio;
using CapaNegocio.SeguridadProgresivo;
using CapaPresentacion.Models;
using CapaPresentacion.Utilitarios;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CapaPresentacion.Hubs
{
    public class progresivoHub : Hub
    {
        private LogAlertaBilleterosBL logBl = new LogAlertaBilleterosBL();
        static List<signalrUser> SignalrUsers = new List<signalrUser>();
        private SeguridadProgresivoBL segprogresivoBl = new SeguridadProgresivoBL();
        private static int _userCount = 0;
        public override Task OnConnected()
        {
            //_userCount++;
            //Clients.All.online(_userCount);
            validarConexion(Context.ConnectionId);
            return base.OnConnected();
        }
       
        public override Task OnDisconnected(bool stopCalled)
        {
            //if (SignalrUsers.Where(x => x.conection_id == Context.ConnectionId).Any())
            //{
            //    SignalrUsers = SignalrUsers.Where(x => x.conection_id != Context.ConnectionId).ToList();
            //}
            ////_userCount--;
            //Clients.All.online(SignalrUsers.Count());
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //_userCount++;

            //Clients.All.online(_userCount);
            //validarConexion(Context.ConnectionId);

            return base.OnReconnected();
        }

        public void conectar(string conection_id)
        {
            string sgn_id = Context.Request.Cookies["c_id_sgn"].Value;
            Signalr_usuarioEntidad sgn_registro = new Signalr_usuarioEntidad();
            if (!SignalrUsers.Where(x => x.conection_id == conection_id).Any())
            {
                SignalrUsers.Add(new signalrUser { conection_id = conection_id,usuario_id= sgn_registro.usuario_id });
            }
            Clients.All.online(SignalrUsers.Count());
        }

        public void desconectar(string conection_id)
        {
            if (SignalrUsers.Where(x => x.conection_id == conection_id).Any())
            {
                SignalrUsers = SignalrUsers.Where(x => x.conection_id != conection_id).ToList();
            }
        }

        public void RecibirUltimas10Alertas(List<LogAlertaBilleterosEntidad> ultimas10Alertas) {
            Clients.All.SendAsync("RecibirUltimas10Alertas", ultimas10Alertas);
        }
        //public List<LogAlertaBilleterosEntidad> ra() {
        //    //return logBl.GetTop10Alerta();
        //}
        public int EnviarMensaje() {
            return 1;
        }
        public void validarConexion(string idconection)
        {
            string sgn_id = Context.Request.Cookies["c_id_sgn"].Value;
            Signalr_usuarioEntidad sgn_registro = new Signalr_usuarioEntidad();
            sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            if (sgn_registro == null)
            {
                //cerrarUsuario(idconection);
            }
            else
            {
                sgn_registro.sgn_conection_id = idconection;
                sgn_registro.sgn_fechaUpdate = DateTime.Now;
                segprogresivoBl.ActualizarConection_id(sgn_registro);
            }

        }
        public void cerrarConexion()
        {
            string sgn_id = Context.Request.Cookies["c_id_sgn"].Value;
            segprogresivoBl.EliminarSignalrid(Convert.ToInt64(sgn_id));
            

        }
        public void refrescarListaConecciones()
        {
            Clients.All.refrescarListaConecciones();
        }
        public void refrescarf5()
        {
            Clients.All.refrescarf5();
        }
        public void cerrarLogin()
        {
            Clients.All.cerrarLogin();
        }
        public void cerrarUsuario(string conectionid)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.Client(conectionid).cerrarUsuario();
        }
        public void mensajeAtodos(string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.mensajeAtodos(message);
        }
        public void mensajeAusuario(string conectionid,string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.Client(conectionid).mensajeAusuario(message);
        }
        public void verToken()
        {
            string sgn_id = Context.Request.Cookies["c_id_sgn"].Value;
            Signalr_usuarioEntidad sgn_registro = new Signalr_usuarioEntidad();
            sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            var mensaje = "";
            if (sgn_registro == null)
            {
                mensajeAusuario(Context.ConnectionId, "No se encontró Registro");
            }
            else {
                if (sgn_registro.sgn_token == "")
                {
                    mensajeAusuario(Context.ConnectionId, "No se generó su token");
                }
                else
                {
                    if (sgn_registro.sgn_estado == 1)
                    {
                        mensaje = $"No se generó su token";
                        mensajeAusuario(Context.ConnectionId, mensaje);
                    }
                    else
                    {
                        if (sgn_registro.sgn_conection_id == Context.ConnectionId)
                        {
                            mensaje = $"Su token es : {sgn_registro.sgn_token}";
                            mensajeAusuario(Context.ConnectionId, mensaje);
                        }
                        else
                        {
                            mensajeAusuario(Context.ConnectionId, mensaje);
                        }
                    }
                    
                }
               
            }
            
            
        }
        public void pedirToken(int room)
        {
            string sgn_id = Context.Request.Cookies["c_id_sgn"].Value;
            Signalr_usuarioEntidad sgn_registro = new Signalr_usuarioEntidad();
            sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            if (sgn_registro == null)
            {
                //cerrarUsuario(idconection);
            }
            else
            {
                funcionesExtra fun = new funcionesExtra();
                var number = fun.RandomNumbers(4);
                
                if(segprogresivoBl.ActualizarSalaId(room, sgn_registro.sgn_id))
                {
                    segprogresivoBl.ActualizarToken("", sgn_registro.sgn_id, 1);

                    refrescarListaConecciones();
                }
            }
            // Call the addNewMessageToPage method to update clients.

        }
        public void enviarToken(Int64 sgn_id)
        {           
            Signalr_usuarioEntidad sgn_registro = new Signalr_usuarioEntidad();
            sgn_registro = segprogresivoBl.GetSignalr_usuarioId(Convert.ToInt64(sgn_id));
            if (sgn_registro == null)
            {
                //cerrarUsuario(idconection);
            }
            else
            {
                var mensaje = $"Su token es : {sgn_registro.sgn_token}";
                mensajeAusuario(sgn_registro.sgn_conection_id, mensaje);

            }
            refrescarListaConecciones();
            // Call the addNewMessageToPage method to update clients.

        }
    }
}
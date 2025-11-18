using CapaDatos.WhatsApp;
using CapaEntidad.WhatsApp;
using CapaEntidad.WhatsApp.Response;
using System;
using System.Collections.Generic;

namespace CapaNegocio.WhatsApp {
    public class WSP_MensajeEnviadoBL {
        private WSP_MensajeEnviadoDAL _mensajeEnviadoDAL = new WSP_MensajeEnviadoDAL();

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviados() {
            return _mensajeEnviadoDAL.ObtenerTodosLosMensajesEnviados();
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamente() {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosCorrectamente();
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamente() {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosIncorrectamente();
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosPorCodigoDePais(string codigoPais) {
            return _mensajeEnviadoDAL.ObtenerTodosLosMensajesEnviadosPorCodigoDePais(codigoPais);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamentePorCodigoDePais(string codigoPais) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosCorrectamentePorCodigoDePais(codigoPais);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamentePorCodigoDePais(string codigoPais) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosIncorrectamentePorCodigoDePais(codigoPais);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosPorIdContacto(int idContacto) {
            return _mensajeEnviadoDAL.ObtenerTodosLosMensajesEnviadosPorIdContacto(idContacto);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamentePorIdContacto(int idContacto) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosCorrectamentePorIdContacto(idContacto);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamentePorIdContacto(int idContacto) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosIncorrectamentePorIdContacto(idContacto);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosDesdeUnNumero(string phoneNumber) {
            return _mensajeEnviadoDAL.ObtenerTodosLosMensajesEnviadosDesdeUnNumero(phoneNumber);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamenteDesdeUnNumero(string phoneNumber) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosCorrectamenteDesdeUnNumero(phoneNumber);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamenteDesdeUnNumero(string phoneNumber) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosIncorrectamenteDesdeUnNumero(phoneNumber);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerTodosLosMensajesEnviadosHaciaUnNumero(string phoneNumber) {
            return _mensajeEnviadoDAL.ObtenerTodosLosMensajesEnviadosHaciaUnNumero(phoneNumber);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosCorrectamenteHaciaUnNumero(string phoneNumber) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosCorrectamenteHaciaUnNumero(phoneNumber);
        }

        public List<WSP_MensajeEnviadoEntidad> ObtenerMensajesEnviadosIncorrectamenteHaciaUnNumero(string phoneNumber) {
            return _mensajeEnviadoDAL.ObtenerMensajesEnviadosIncorrectamenteHaciaUnNumero(phoneNumber);
        }

        public WSP_MensajeEnviadoEntidad ObtenerMensajeEnviadoPorIdMensajeEnviado(int idMensajeEnviado) {
            return _mensajeEnviadoDAL.ObtenerMensajeEnviadoPorIdMensajeEnviado(idMensajeEnviado);
        }

        public bool InsertarMensaje(List<WSP_Message> mensajes) {
            bool exito = true;
            foreach(var mensaje in mensajes) {
                var me = new WSP_MensajeEnviadoEntidad();
                me.IdContacto = mensaje.idContact;
                me.CodigoMensaje = mensaje.id;
                me.Desde = mensaje.from;
                me.Hacia = mensaje.to;
                me.Mensaje = mensaje.message;
                me.Estado = mensaje.state;
                DateTime init = new DateTime(1965, 1, 1, 0, 0, 0, 0);
                me.FechaEnvio = init.AddSeconds(mensaje.timestamp).AddHours(-5);
                exito = exito && _mensajeEnviadoDAL.InsertarMensaje(me);
            }
            return exito;
        }
    }
}

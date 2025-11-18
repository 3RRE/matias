using CapaDatos.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente;
using System;
using System.Collections.Generic;

namespace CapaNegocio.AsistenciaCliente {
    public class AST_ClienteSalaBL {
        private AST_ClienteSalaDAL clienteSalaDal = new AST_ClienteSalaDAL();
        public bool GuardarClienteSala(AST_ClienteSalaEntidad clienteSala) {
            return clienteSalaDal.GuardarClienteSala(clienteSala);
        }
        public bool EditarClienteSala(AST_ClienteSalaEntidad clienteSala) {
            return clienteSalaDal.EditarClienteSala(clienteSala);
        }
        public AST_ClienteSalaEntidad GetClienteSalaID(int ClienteId, int SalaId) {
            return clienteSalaDal.GetClienteSalaID(ClienteId, SalaId);
        }
        public bool EditarClienteSalaCompleto(AST_ClienteSalaEntidad clienteSala) {
            return clienteSalaDal.EditarClienteSalaCompleto(clienteSala);
        }
        public int ObtenerTotalRegistrosFiltrados(string WhereQuery, string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            return clienteSalaDal.ObtenerTotalRegistrosFiltrados(WhereQuery, ArraySalaId, fechaIni, fechaFin);
        }
        public int ObtenerTotalRegistros() {
            return clienteSalaDal.ObtenerTotalRegistros();
        }
        public List<AST_ClienteSala> GetAllClientesFiltrados(string WhereQuery, string ArraySalaId, DateTime fechaIni, DateTime fechaFin) {
            return clienteSalaDal.GetAllClientesFiltrados(WhereQuery, ArraySalaId, fechaIni, fechaFin);
        }
        public List<AST_ClienteSalaEntidad> GetListadoClienteSala(int ClienteId) => clienteSalaDal.GetListadoClienteSala(ClienteId);
        public bool GuardarClienteSalaConFecha(AST_ClienteSalaEntidad clienteSala) => clienteSalaDal.GuardarClienteSalaConFecha(clienteSala);

        public bool ActualizarEnvioNotificacion(int idCliente, int codSala, AST_TipoNotificacionCliente tipoNotificacion, bool enviaNotificacion) {
            int idActualizado = 0;
            switch(tipoNotificacion) {
                case AST_TipoNotificacionCliente.Whatsapp:
                    idActualizado = clienteSalaDal.ActualizarEnvioNotificacionWhatsapp(idCliente, codSala, enviaNotificacion);
                    break;
                case AST_TipoNotificacionCliente.Sms:
                    idActualizado = clienteSalaDal.ActualizarEnvioNotificacionSms(idCliente, codSala, enviaNotificacion);
                    break;
                case AST_TipoNotificacionCliente.Email:
                    idActualizado = clienteSalaDal.ActualizarEnvioNotificacionEmail(idCliente, codSala, enviaNotificacion);
                    break;
                case AST_TipoNotificacionCliente.Llamada:
                    idActualizado = clienteSalaDal.ActualizarLlamadaCelular(idCliente, codSala, enviaNotificacion);
                    break;
                default:
                    idActualizado = 0;
                    break;
            }
            return idActualizado > 0;
        }
    
        public List<AST_ClienteSala> ObtenerClientesParaEnvioNotificacion(AST_FiltroCliente filtros, List<int> codsSalas) {
            string whereQuery = string.Empty;
            if(!string.IsNullOrWhiteSpace(filtros.NumeroDocumento)) {
                whereQuery = $"astc.NroDoc = '{filtros.NumeroDocumento}'";
            } else {
                whereQuery = $"astc.Nombre LIKE '%{filtros.Nombres}%'";
                if(!string.IsNullOrWhiteSpace(filtros.ApellidoPaterno)) {
                    whereQuery += $" AND astc.ApelPat LIKE '%{filtros.ApellidoPaterno}%'";
                }
                if(!string.IsNullOrWhiteSpace(filtros.ApellidoMaterno)) {
                    whereQuery += $" AND astc.ApelMat LIKE '%{filtros.ApellidoMaterno}%'";
                }
            }
            string codsSalasStr = string.Join(",", codsSalas);
            return clienteSalaDal.ObtenerClientesParaEnvioNotificacion(whereQuery, codsSalasStr);
        }
    }
}

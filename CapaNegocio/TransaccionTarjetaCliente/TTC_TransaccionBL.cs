using CapaDatos.TransaccionTarjetaCliente;
using CapaEntidad.TransaccionTarjetaCliente.Dto;
using CapaEntidad.TransaccionTarjetaCliente.Entidad;
using CapaEntidad.TransaccionTarjetaCliente.Filtro;
using CapaNegocio.AsistenciaCliente;
using System;
using System.Collections.Generic;

namespace CapaNegocio.TransaccionTarjetaCliente {
    public class TTC_TransaccionBL {
        private readonly TTC_TransaccionDAL transaccionDAL;
        private readonly AST_ClienteBL clienteBl;

        public TTC_TransaccionBL() {
            transaccionDAL = new TTC_TransaccionDAL();
            clienteBl = new AST_ClienteBL();
        }

        public List<TTC_TransaccionDto> ObtenerTransaccionesPorCodSala(TTC_TransaccionFiltro filtro) {
            return transaccionDAL.ObtenerTransaccionesPorCodSala(filtro);
        }

        public bool GuardarTransaccionesMasivo(List<TTC_TransaccionEntidad> transacciones) {
            transacciones.ForEach(t => {
                int idCliente = clienteBl.GetClientexNroDoc(t.NumeroDocumentoCliente).Id;
                if(idCliente > 0) {
                    t.IdCliente = idCliente;
                }
            });
            return transaccionDAL.GuardarTransaccionesMasivo(transacciones);
        }

        public List<TTC_TransaccionEntidad> ObtenerTransaccionesParaMoldat(TTC_TransaccionMoldatFiltro filtro) {
            return transaccionDAL.ObtenerTransaccionesParaMoldat(filtro);
        }

        public void ActualizarEstadoMigracionesMoldat(List<int> ids, DateTime? fechaMigracionMoldat) {
            foreach(int id in ids) {
                transaccionDAL.ActualizarEstadoMigracionMoldatPorId(id, fechaMigracionMoldat);
            }
        }

        public int ObtenerUltimoItemVoucherPorCodSala(int codSala) {
            return transaccionDAL.ObtenerUltimoItemVoucherPorCodSala(codSala);
        }
    }
}

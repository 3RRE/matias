using CapaDatos.WhatsApp;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.WhatsApp;
using System;
using System.Collections.Generic;

namespace CapaNegocio.WhatsApp {
    public class WSP_MensajeriaClienteBL {
        private WSP_MensajeriaClienteDAL _mensajeriaCLienteDAL = new WSP_MensajeriaClienteDAL();

        public List<WSP_MensajeriaClienteEntidad> ObtenerClientes() {
            return _mensajeriaCLienteDAL.ObtenerClientes();
        }

        public List<WSP_MensajeriaClienteEntidad> ObtenerClientesPorFiltro(string[] filtroSala, string[] filtroTipoCliente, string[] filtroTipoFrecuencia, string[] filtroTipoJuego) {
            List<string> filtros = new List<string>();

            if(filtroSala != null && filtroSala.Length >= 1) {
                filtros.Add($"s.CodSala IN ({String.Join(",", filtroSala)})");
            }

            if(filtroTipoCliente != null && filtroTipoCliente.Length >= 1) {
                filtros.Add($"cs.TipoClienteId IN ({String.Join(",", filtroTipoCliente)})");
            }

            if(filtroTipoFrecuencia != null && filtroTipoFrecuencia.Length >= 1) {
                filtros.Add($"cs.TipoFrecuenciaId IN ({String.Join(",", filtroTipoFrecuencia)})");
            }

            if(filtroTipoJuego != null && filtroTipoJuego.Length >= 1) {
                filtros.Add($"cs.TipoJuegoId IN ({String.Join(",", filtroTipoJuego)})");
            }

            if(filtros.Count == 0) {
                return _mensajeriaCLienteDAL.ObtenerClientes();
            }

            string filtro = String.Join(" AND ", filtros);

            return _mensajeriaCLienteDAL.obtenerClientesPorFiltro(filtro);
        }

        public List<string> ObtenerNumerosClientes(List<AST_ClienteEntidad> clientes) {
            List<string> phones = new List<string>();
            foreach(var cliente in clientes) {
                bool hasCountryCode = !String.IsNullOrEmpty(cliente.CodigoPais.Trim());
                bool hasPhone1 = !String.IsNullOrEmpty(cliente.Celular1.Trim());
                bool hasPhone2 = !String.IsNullOrEmpty(cliente.Celular2.Trim());

                cliente.CodigoPais = hasCountryCode ? cliente.CodigoPais : "51";

                string phone = string.Empty;

                if(hasCountryCode) {
                    phone = hasPhone1 ? $"{cliente.CodigoPais.Trim()}{cliente.Celular1.Trim()}" : hasPhone2 ? $"{cliente.CodigoPais.Trim()}{cliente.Celular2.Trim()}" : string.Empty;
                } else {
                    phone = hasPhone1 && cliente.Celular1.Trim().Length == 9 && cliente.Celular1.StartsWith("9") ? $"{cliente.CodigoPais.Trim()}{cliente.Celular1.Trim()}" : hasPhone2 && cliente.Celular2.Trim().Length == 9 && cliente.Celular2.StartsWith("9") ? $"{cliente.CodigoPais.Trim()}{cliente.Celular2.Trim()}" : string.Empty;
                }

                if(!String.IsNullOrEmpty(phone)) {
                    phones.Add(phone);
                }
            }

            return phones;
        }
    }
}

using CapaDatos.AsistenciaCliente;
using CapaEntidad;
using CapaEntidad.AsistenciaCliente;
using CapaEntidad.AsistenciaCliente.DataWarehouse;
using CapaEntidad.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.AsistenciaCliente {
    public class AST_ClienteBL {
        private AST_ClienteDAL clienteDal = new AST_ClienteDAL();

        public List<AST_ClienteEntidad> GetListadoCliente() {
            return clienteDal.GetListadoCliente();
        }

        public List<AST_ClienteEntidad> GetListadoClienteFiltrados(string whereQuery) {
            return clienteDal.GetListadoClienteFiltrados(whereQuery);
        }

        public int GetListadoClienteFiltradosTotal(string whereQuery) {
            return clienteDal.GetListadoClienteFiltradosTotal(whereQuery);
        }

        public List<AST_ClienteEntidad> GetListadoClienteCoincidencia(string coincidencia, string campo) {
            return clienteDal.GetListadoClienteCoincidencia(coincidencia, campo);
        }
        public List<AST_ClienteEntidad> GetListadoClientePorNombresyApellidos(string coincidencia) {
            return clienteDal.GetListadoClientePorNombresyApellidos(coincidencia);
        }
        public AST_ClienteEntidad GetClienteID(int ClienteId) {
            return clienteDal.GetClienteID(ClienteId);
        }
        public AST_ClienteEntidad GetClientexNroyTipoDoc(int tipoDocumentoId, string NroDocumento) {
            return clienteDal.GetClientexNroyTipoDoc(tipoDocumentoId, NroDocumento);
        }

        public AST_ClienteEntidad GetClientexNroDoc(string NroDocumento) {
            return clienteDal.GetClientexNroDoc(NroDocumento);
        }
        public List<AST_ClienteEntidad> GetListaClientesxNroDoc(string NroDocumento) {
            return clienteDal.GetListaClientesxNroDoc(NroDocumento);
        }
        public List<AST_ClienteEntidad> GetListaClientesxNroDocMetodoBusqueda(string NroDocumento) {
            return clienteDal.GetListaClientesxNroDocMetodoBusqueda(NroDocumento);
        }
        public List<AST_ClienteEntidad> GetListaClientesxNombreMetodoBusqueda(string NroDocumento) {
            return clienteDal.GetListaClientesxNombreMetodoBusqueda(NroDocumento);
        }
        public int GuardarCliente(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarCliente(cliente);
        }
        public int GuardarClienteSinFechas(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteSinFechas(cliente);
        }

        public int GuardarClienteCampania(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteCampania(cliente);
        }

        public int GuardarClienteEmpadronamiento(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteEmpadronamiento(cliente);
        }

        public bool EditarCliente(AST_ClienteEntidad cliente) {
            return clienteDal.EditarCliente(cliente);
        }

        public bool ActualizarContactoCliente(AST_ClienteEntidad cliente) {
            return clienteDal.ActualizarContactoCliente(cliente);
        }

        public bool EditarEstadoCliente(AST_ClienteEntidad cliente) {
            return clienteDal.EditarEstadoCliente(cliente);
        }
        public bool EditarAsistenciaDespuesCuarentena(AST_ClienteEntidad cliente) {
            return clienteDal.EditarAsistenciaDespuesCuarentena(cliente);
        }
        public int GuardarClienteLudopatas(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteLudopatas(cliente);
        }
        public int GetTotalClientes() {
            return clienteDal.GetTotalClientes();
        }
        public List<AST_ClienteEntidad> GetListaCumpleanios(int CodSala) {
            return clienteDal.GetListaCumpleanios(CodSala);
        }

        public List<AST_ClienteEntidad> GetListaMasivoClientesxNroDoc(List<string> numberDocuments) {
            return clienteDal.GetListaMasivoClientesxNroDoc(numberDocuments);
        }
        public List<dynamic> GetTotalClientesPorAnio(int anio) {
            return clienteDal.GetTotalClientesPorAnio(anio);
        }

        public int GuardarClienteCampaniaATP(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteCampaniaATP(cliente);
        }

        public AST_ClienteEntidad ObtenerClienteById(int clienteId) {
            return clienteDal.ObtenerClienteById(clienteId);
        }

        public int GuardarClienteEmpadronamientoV2(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteEmpadronamientoV2(cliente);
        }
        public List<AST_ClienteMigracion> ListarClienteMigracion(int Id) {
            return clienteDal.ListarClienteMigracion(Id);
        }
        public int GuardarClienteSorteoExterno(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteSorteoExterno(cliente);
        }

        public int GuardarClienteCampaniaWhatsApp(AST_ClienteEntidad cliente) {
            return clienteDal.GuardarClienteCampaniaWhatsApp(cliente);
        }
        public bool VerificarSiExisteCampania(int codSala) {
            return clienteDal.VerificarSiExisteCampania(codSala);
        }
        public ResponseEntidad<ClienteVerificacionResponse> GetExistenciaDeClienteParaCampaniaWhatsApp(string documentNumber, int idDocumentType, string phoneNumber, int codSala) {
            ResponseEntidad<ClienteVerificacionResponse> response = new ResponseEntidad<ClienteVerificacionResponse>();

            try {
                var clientes = clienteDal.GetExistenciaDeClienteParaCampaniaWhatsApp(documentNumber, idDocumentType, phoneNumber, codSala);

                DateTime fechaActual = DateTime.Now;
                TimeSpan rangoDeTresHoras = TimeSpan.FromHours(3);

                //SOLO HABRA UN REGISTRO SI EL CLIENTE TIENE UNICAMENTE UN REGISTRO EN LA TABLA AST_CLIENTE, Y EN ESE CASO NO SE TOMA EN CUENTA EL REGISTRO SI ES POR SYSLUDOPATAS Y ES CON UNA ANTIGUEDAD DE 3 HORAS
                if(clientes.Count == 1) {
                    clientes.RemoveAll(x =>
                        x.TipoRegistro.Equals("SYSLUDOPATAS") &&
                        x.FechaRegistro >= fechaActual.Subtract(rangoDeTresHoras) &&
                        x.FechaRegistro <= fechaActual);
                }

                response.success = true;
                response.data.clientExist = clientes.Count > 0;
                bool existsDocumentNumber = clientes.Any(x => x.NroDoc.Equals(documentNumber));
                bool existsPhoneNumber = clientes.Any(x => x.Celular1.Equals(phoneNumber) || x.Celular2.Equals(phoneNumber));
                response.displayMessage = existsDocumentNumber && existsPhoneNumber ? $"Ya se encuentra registrado un cliente con número de documento {documentNumber} y número de celular {phoneNumber}." : existsDocumentNumber ? $"Ya se encuentra registrado un cliente con número de documento {documentNumber}." : existsPhoneNumber ? $"Ya se encuentra registrado un cliente con número de celular {phoneNumber}." : "Cliente disponible para registro.";
            } catch(Exception ex) {
                response.success = false;
                response.data.clientExist = true;
                response.displayMessage = ex.ToString();
            }

            return response;
        }
        public int GuardarClienteSorteosSala(AST_ClienteEntidad cliente) => clienteDal.GuardarClienteSorteosSala(cliente);
        public List<AST_ClienteCortesia> GetClientesCortesia() {
            return clienteDal.GetClientesCortesia();
        }

        public AST_ClienteEntidad ObtenerPermisosDeContactoDeCliente(string numeroDocumento, int idTipoDocumento, int codSala) {
            return clienteDal.ObtenerPermisosDeContactoDeCliente(numeroDocumento, idTipoDocumento, codSala);
        }

        #region Migracion DWH
        public void ActualizarEstadoMigracionesDwh(List<AST_ClienteEstadoMigracion> ids, DateTime? fechaMigracionDwh) {
            foreach(AST_ClienteEstadoMigracion id in ids) {
                clienteDal.ActualizarEstadoMigracionDwh(id, fechaMigracionDwh);
            }
        }

        public List<AST_ClienteMigracionDwhEntidad> ObtenerClientesControlAccesoParaDwh(AST_ClienteMigracionDwhFiltro filtro) {
            return clienteDal.ObtenerClientesControlAccesoParaDwh(filtro);
        }
        #endregion
    }
}

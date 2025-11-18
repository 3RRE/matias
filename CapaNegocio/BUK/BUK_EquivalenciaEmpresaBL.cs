using CapaDatos.BUK;
using CapaEntidad.BUK;
using CapaEntidad.BUK.Response;
using CapaEntidad.Response;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CapaNegocio.BUK {
    public class BUK_EquivalenciaEmpresaBL {
        private readonly BUK_EquivalenciaEmpresaDAL _equivalenciaEmpresaDAL;

        public BUK_EquivalenciaEmpresaBL() {
            _equivalenciaEmpresaDAL = new BUK_EquivalenciaEmpresaDAL();
        }

        public List<BUK_EquivalenciaEmpresaEntidad> ObtenerTodasLasEquivalenciasEmpresa() {
            return _equivalenciaEmpresaDAL.ObtenerTodasLasEquivalenciasEmpresa();
        }

        public List<BUK_EquivalenciaEmpresaEntidad> ObtenerTodasLasEquivalenciasEmpresaActivas() {
            return _equivalenciaEmpresaDAL.ObtenerTodasLasEquivalenciasEmpresaActivas();
        }

        public List<BUK_EquivalenciaEmpresaEntidad> ObtenerTodasLasEquivalenciasEmpresaCorrectas() {
            return _equivalenciaEmpresaDAL.ObtenerTodasLasEquivalenciasEmpresaCorrectas();
        }

        public BUK_EquivalenciaEmpresaEntidad ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(int idEquivalenciaEmpresa) {
            return _equivalenciaEmpresaDAL.ObtenerEquivalenciaEmpresaPorIdEquivalenciaEmpresa(idEquivalenciaEmpresa);
        }
        
        public BUK_EquivalenciaEmpresaEntidad ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(int idEmpresaOfisis) {
            return _equivalenciaEmpresaDAL.ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(idEmpresaOfisis);
        }
        
        public BUK_EquivalenciaEmpresaEntidad ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(string codEmpresaOfisis) {
            return _equivalenciaEmpresaDAL.ObtenerEquivalenciaEmpresaPorCodEmpresaOfisis(codEmpresaOfisis);
        }

        public bool InsertarEquivalenciaEmpresa(BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa) {
            return _equivalenciaEmpresaDAL.InsertarEquivalenciaEmpresa(equivalenciaEmpresa) != 0;
        }

        public bool ActualizarEquivalenciaEmpresa(BUK_EquivalenciaEmpresaEntidad equivalenciaEmpresa) {
            return _equivalenciaEmpresaDAL.ActualizarEquivalenciaEmpresa(equivalenciaEmpresa) != 0;
        }

        public bool EliminarEquivalenciaEmpresa(int idEquivalenciaEmpresa) {
            return _equivalenciaEmpresaDAL.EliminarEquivalenciaEmpresa(idEquivalenciaEmpresa) != 0;
        }

        public ResponseEntidad<BUK_EquivalenciaEmpresaResponse> SincronizarEmpresas(List<BUK_EquivalenciaEmpresaEntidad> empresas) {
            ResponseEntidad<BUK_EquivalenciaEmpresaResponse> response = new ResponseEntidad<BUK_EquivalenciaEmpresaResponse>();

            response.success = empresas.Count > 0;

            foreach(var empresa in empresas) {
                var empresaRevision = _equivalenciaEmpresaDAL.ObtenerEquivalenciaEmpresaPorIdEmpresaBuk(empresa.IdEmpresaBuk);
                if(empresaRevision.Existe()) {
                    if(empresaRevision.Nombre.Equals(empresa.Nombre)) {
                        response.data.RegistrosExistentes++;
                    } else {
                        empresaRevision.Nombre = empresa.Nombre;
                        empresaRevision.IdEmpresaBuk = empresa.IdEmpresaBuk;
                        if(ActualizarEquivalenciaEmpresa(empresa)) {
                            response.data.RegistrosActualizadosCorrectamente++;
                        } else {
                            response.data.RegistrosActualizadosIncorrectamente++;
                        }
                    }
                    continue;
                }
                if(InsertarEquivalenciaEmpresa(empresa)) {
                    response.data.RegistrosInsertadosCorrectamente++;
                } else {
                    response.data.RegistrosInsertadosIncorrectamente++;
                }
            }

            response.displayMessage = response.success ? "Empresas sincronizadas correctamente" : "No hay empresas para sincronizar";

            return response;
        }
    }
}

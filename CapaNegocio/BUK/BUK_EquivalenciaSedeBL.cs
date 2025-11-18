using CapaDatos.BUK;
using CapaEntidad.BUK;
using CapaEntidad.BUK.Response;
using CapaEntidad.Response;
using System.Collections.Generic;

namespace CapaNegocio.BUK {
    public class BUK_EquivalenciaSedeBL {
        private readonly BUK_EquivalenciaSedeDAL _equivalenciaSedeDAL;
        private readonly BUK_EquivalenciaEmpresaDAL _equivalenciaEmpresaDAL;

        public BUK_EquivalenciaSedeBL() {
            _equivalenciaSedeDAL = new BUK_EquivalenciaSedeDAL();
            _equivalenciaEmpresaDAL = new BUK_EquivalenciaEmpresaDAL();
        }

        public List<BUK_EquivalenciaSedeEntidad> ObtenerTodasLasEquivalenciasSede() {
            return _equivalenciaSedeDAL.ObtenerTodasLasEquivalenciasSede();
        }

        public List<BUK_EquivalenciaSedeEntidad> ObtenerTodasLasEquivalenciasSedeCorrectas() {
            return _equivalenciaSedeDAL.ObtenerTodasLasEquivalenciasSedeCorrectas();
        }

        public List<BUK_EquivalenciaSedeEntidad> ObtenerEquivalenciaSedePorIdEquivalenciaEmpresa(int idEquivalenciaEmpresa) {
            return _equivalenciaSedeDAL.ObtenerEquivalenciaSedePorIdEquivalenciaEmpresa(idEquivalenciaEmpresa);
        }

        public List<BUK_EquivalenciaSedeEntidad> ObtenerEquivalenciaSedePorCodEmpresaOfisis(string codEmpresaOfisis) {
            return _equivalenciaSedeDAL.ObtenerEquivalenciaSedePorCodEmpresaOfisis(codEmpresaOfisis);
        }

        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorIdEquivalenciaSede(int idEquivalenciaSede) {
            return _equivalenciaSedeDAL.ObtenerEquivalenciaSedePorIdEquivalenciaSede(idEquivalenciaSede);
        }

        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(string codEmpresaOfisis, string nombreEquivalenciaSede) {
            return _equivalenciaSedeDAL.ObtenerEquivalenciaSedePorCodEmpresaOfisisYNombreEquivalenciaSede(codEmpresaOfisis, nombreEquivalenciaSede);
        }

        public BUK_EquivalenciaSedeEntidad ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisis(string codEmpresaOfisis, string codSedeOfisis) {
            return _equivalenciaSedeDAL.ObtenerEquivalenciaSedePorCodEmpresaYSedeOfisis(codEmpresaOfisis, codSedeOfisis);
        }

        public bool InsertarEquivalenciaSede(BUK_EquivalenciaSedeEntidad equivalenciaSede) {
            return _equivalenciaSedeDAL.InsertarEquivalenciaSede(equivalenciaSede) != 0;
        }

        public bool ActualizarEquivalenciaSede(BUK_EquivalenciaSedeEntidad equivalenciaSede) {
            return _equivalenciaSedeDAL.ActualizarEquivalenciaSede(equivalenciaSede) != 0;
        }

        public bool EliminarEquivalenciaSede(int idEquivalenciaSede) {
            return _equivalenciaSedeDAL.EliminarEquivalenciaSede(idEquivalenciaSede) != 0;
        }

        public ResponseEntidad<BUK_EquivalenciaSedeResponse> SincornizarSedes(List<BUK_EquivalenciaSedeEntidad> sedes) {
            ResponseEntidad<BUK_EquivalenciaSedeResponse> response = new ResponseEntidad<BUK_EquivalenciaSedeResponse>();

            response.success = sedes.Count > 0;

            foreach(var sede in sedes) {
                var sedeRevision = _equivalenciaSedeDAL.ObtenerEquivalenciaSedePorNombreEquivalenciaSede(sede.NombreSede);
                if(sedeRevision.Existe()) {
                    response.data.RegistrosExistentes++;
                    continue;
                }

                if(InsertarEquivalenciaSede(sede)) {
                    response.data.RegistrosInsertadosCorrectamente++;
                } else {
                    response.data.RegistrosInsertadosIncorrectamente++;
                }                
            }

            response.displayMessage = response.success ? "Sedes sincronizadas correctamente" : "No hay sedes para sincronizar";

            return response;
        }
    }
}

using CapaEntidad.EntradaSalidaSala;
using CapaDatos.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {

    public class ESS_LogOcurrenciaBL {
        private ESS_LogOcurrenciaDAL _logocurrenciaDal = new ESS_LogOcurrenciaDAL();

        public List<ESS_LogOcurrenciaEntidad> ListadoLogsOcurrencia(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return _logocurrenciaDal.ListadoLogsOcurrencia(codSala, fechaIni, fechaFin);
        }
        public int GuardarLogOcurrencia(ESS_LogOcurrenciaEntidad registro) {
            return _logocurrenciaDal.GuardarLogOcurrencia(registro);
        } 
        public bool ActualizarLogOcurrencia(ESS_LogOcurrenciaEntidad registro) {
            var status = _logocurrenciaDal.ActualizarLogOcurrencia(registro);
            return status;
        } 
        public bool EliminarLogOcurrencia(int idregistro) {
            return _logocurrenciaDal.EliminarLogOcurrencia(idregistro);
        }
        public List<ESS_AreaEntidad> ListarAreaPorEstado(int estado) => _logocurrenciaDal.ListarAreaPorEstado(estado);
        public List<ESS_TipologiaEntidad> ListarTipologiaPorEstado(int estado) => _logocurrenciaDal.ListarTipologiaPorEstado(estado);
        public List<ESS_ActuanteEntidad> ListarActuantePorEstado(int estado) => _logocurrenciaDal.ListarActuantePorEstado(estado);
        public List<ESS_ComunicacionEntidad> ListarComunicacionPorEstado(int estado) => _logocurrenciaDal.ListarComunicacionPorEstado(estado);
        public List<ESS_EstadoOcurrenciaEntidad> ListarEstadoOcurrenciaPorEstado(int estado) => _logocurrenciaDal.ListarEstadoOcurrenciaPorEstado(estado);


        public List<ESS_AreaEntidad> ListarArea() => _logocurrenciaDal.ListarArea();

        public int InsertarArea(ESS_AreaEntidad model) => _logocurrenciaDal.InsertarArea(model);

        public bool EditarArea(ESS_AreaEntidad model) => _logocurrenciaDal.EditarArea(model);

        public List<ESS_TipologiaEntidad> ListarTipologia() => _logocurrenciaDal.ListarTipologia();

        public int InsertarTipologia(ESS_TipologiaEntidad model) => _logocurrenciaDal.InsertarTipologia(model);

        public bool EditarTipologia(ESS_TipologiaEntidad model) => _logocurrenciaDal.EditarTipologia(model);
        public List<ESS_ActuanteEntidad> ListarActuante() => _logocurrenciaDal.ListarActuante();

        public int InsertarActuante(ESS_ActuanteEntidad model) => _logocurrenciaDal.InsertarActuante(model);

        public bool EditarActuante(ESS_ActuanteEntidad model) => _logocurrenciaDal.EditarActuante(model);
        public List<ESS_ComunicacionEntidad> ListarComunicacion() => _logocurrenciaDal.ListarComunicacion();

        public int InsertarComunicacion(ESS_ComunicacionEntidad model) => _logocurrenciaDal.InsertarComunicacion(model);

        public bool EditarComunicacion(ESS_ComunicacionEntidad model) => _logocurrenciaDal.EditarComunicacion(model);


    }

}

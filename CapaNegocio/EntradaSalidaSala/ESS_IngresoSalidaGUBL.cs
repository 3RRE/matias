using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_IngresoSalidaGUBL {
        private ESS_IngresoSalidaGUDAL _ingresosalidaguDal = new ESS_IngresoSalidaGUDAL();

        public List<ESS_IngresoSalidaGUEntidad> ListadoIngresoSalida(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return _ingresosalidaguDal.ListadoIngresoSalida(codSala, fechaIni, fechaFin);
        }
        public int GuardarIngresoSalidaGU(ESS_IngresoSalidaGUEntidad registro) {
            return _ingresosalidaguDal.GuardarIngresoSalidaGU(registro);
        }

        public bool ActualizarIngresoSalidaGU(ESS_IngresoSalidaGUEntidad registro) {
            var status = _ingresosalidaguDal.ActualizarIngresoSalidaGU(registro);
            return status;
        }
        public bool EliminarIngresoSalidaGU(int idregistro) {
            return _ingresosalidaguDal.EliminarIngresoSalidaGU(idregistro);
        }
        public bool FinalizarHoraRegistroIngresoSalidaGU(int idregistro) {
            var status = _ingresosalidaguDal.FinalizarHoraRegistroIngresoSalidaGU(idregistro);
            return status;
        }
        public List<ESS_MotivoGUEntidad> ListarMotivoGUPorEstado(int estado) => _ingresosalidaguDal.ListarMotivoGUPorEstado(estado);


        public List<ESS_MotivoGUEntidad> ListarMotivo() => _ingresosalidaguDal.ListarMotivo();

        public int InsertarMotivo(ESS_MotivoGUEntidad model) => _ingresosalidaguDal.InsertarMotivo(model);

        public bool EditarMotivo(ESS_MotivoGUEntidad model) => _ingresosalidaguDal.EditarMotivo(model);
    }
}

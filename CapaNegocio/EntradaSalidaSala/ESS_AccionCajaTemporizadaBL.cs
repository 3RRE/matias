using CapaDatos.EntradaSalidaSala;
using CapaEntidad.BUK;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_AccionCajaTemporizadaBL {
        private ESS_AccionCajaTemporizadaDAL _accioncajatemporizadaDal = new ESS_AccionCajaTemporizadaDAL();

        public List<ESS_AccionCajaTemporizadaEntidad> ListadoAccionCajaTemporizada(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return _accioncajatemporizadaDal.ListadoAccionCajaTemporizada(codSala, fechaIni, fechaFin);
        }
        public int GuardarAccionCajaTemporizada(ESS_AccionCajaTemporizadaEntidad registro) {
            return _accioncajatemporizadaDal.GuardarAccionCajaTemporizada(registro);
        }
        public int GuardarAccionCajaTemporizadafromImportar(ESS_AccionCajaTemporizadaEntidad registro)
        {
            return _accioncajatemporizadaDal.GuardarAccionCajaTemporizadafromImportar(registro);
        }

        public bool ActualizarAccionCajaTemporizada(ESS_AccionCajaTemporizadaEntidad registro) {
            var status = _accioncajatemporizadaDal.ActualizarAccionCajaTemporizada(registro);
            return status;
        }
        public bool EliminarAccionCajaTemporizada(int idregistro) {
            return _accioncajatemporizadaDal.EliminarAccionCajaTemporizada(idregistro);
        }
        public bool FinalizarHoraRegistroAccionCajaTemporizada(int idregistro, DateTime horaSalida) {
            var status = _accioncajatemporizadaDal.FinalizarHoraRegistroAccionCajaTemporizada(idregistro, horaSalida);
            return status;
        }

        public List<ESS_AccionCajaTemporizadaCargoEntidad> ListarEmpleadosGerentesYJefes() => _accioncajatemporizadaDal.ListarEmpleadosGerentesYJefes();
         
        public BUK_EmpleadoEntidad ObtenerEmpleadoPorDocumentoBUK(string NumeroDocumento)
        {
            return _accioncajatemporizadaDal.ObtenerEmpleadoPorDocumentoBUK(NumeroDocumento);
        }

    }
}

using CapaDatos.EntradaSalidaSala;
using CapaEntidad.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {
    public class ESS_VisitasSupervisorBL {
        private ESS_VisitasSupervisorDAL _visitasSupervisorDal = new ESS_VisitasSupervisorDAL();
        public List<ESS_VisitasSupervisorEntidad> ListadoVisitasSupervisor(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return _visitasSupervisorDal.ListadoVisitasSupervisor(codSala, fechaIni, fechaFin);
        }
        public bool EliminarVisitasSupervisor(int idregistro) {
            return _visitasSupervisorDal.EliminarVisitasSupervisor(idregistro);
        }
        public bool EditarVisitasSupervisor(ESS_VisitasSupervisorEntidad registro) {
            var status = _visitasSupervisorDal.EditarVisitasSupervisor(registro);
            return status;
        }
        public int GuardarVisitasSupervisor(ESS_VisitasSupervisorEntidad visitasSupervisor) {
            return _visitasSupervisorDal.GuardarVisitasSupervisor(visitasSupervisor);
        }
        public List<ESS_VisitaSupervisorMotivoEntidad> ListarMotivoPorEstado(int estado) => _visitasSupervisorDal.ListarMotivoPorEstado(estado);

    }
}

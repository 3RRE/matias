using CapaEntidad.EntradaSalidaSala;
using CapaDatos.EntradaSalidaSala;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.EntradaSalidaSala {

    public class ESS_AperturaCierreSalaBL {
        private ESS_AperturaCierreSalaDAL essAperturaCierreSalaDAL = new ESS_AperturaCierreSalaDAL();


        public List<ESS_AperturaCierreSalaEntidad> ListarRegistroAperturaCierreSala(int[] codSala, DateTime fechaIni, DateTime fechaFin) {
            return essAperturaCierreSalaDAL.ListarRegistroAperturaCierreSala(codSala, fechaIni, fechaFin);
        }
         public DateTime? ObtenerFechaHoraCierrePorId(int IdAperturaCierreSala) {
            return essAperturaCierreSalaDAL.ObtenerFechaHoraCierrePorId(IdAperturaCierreSala);
        }
         
        public int GuardarRegistroAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            return essAperturaCierreSalaDAL.GuardarRegistroAperturaCierreSala(registro);
        }
        public int GuardarRegistroAperturaCierreSala_Importar(ESS_AperturaCierreSalaEntidad registro) {
            return essAperturaCierreSalaDAL.GuardarRegistroAperturaCierreSala_Importar(registro);
        }

        public bool ActualizarAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            var status = essAperturaCierreSalaDAL.ActualizarAperturaCierreSala(registro);
            return status;
        }

        public bool FinalizarRegistroAperturaCierreSala(ESS_AperturaCierreSalaEntidad registro) {
            var status = essAperturaCierreSalaDAL.FinalizarRegistroAperturaCierreSala(registro);
            return status;
        }


        public bool EliminarRegistroAperturaCierreSala(ESS_AperturaCierreSalaEntidad entidad) {
            return essAperturaCierreSalaDAL.EliminarRegistroAperturaCierreSala(entidad.IdAperturaCierreSala);
        }

        public List<ESS_AperturaCierreSalaPersonaEntidad> ListarEmpleadoBUKcargo() => essAperturaCierreSalaDAL.ListarEmpleadoBUKcargo();


    }
}

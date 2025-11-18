using CapaDatos.GLPI.Mantenedores;
using CapaEntidad.GLPI.Helper;
using CapaEntidad.GLPI.Mantenedores;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.GLPI.Mantenedores {
    public class GLPI_ProcesoBL {
        private readonly GLPI_ProcesoDAL procesoDAL;

        public GLPI_ProcesoBL() {
            procesoDAL = new GLPI_ProcesoDAL();
        }

        public List<GLPI_Proceso> ObtenerProcesos() {
            return procesoDAL.ObtenerProcesos();
        }

        public List<GLPI_SelectHelper> ObtenerProcesosSelect() {
            List<GLPI_SelectHelper> options = ObtenerProcesos()
                .Select(x => new GLPI_SelectHelper {
                    IdPadre = x.ObtenerIdPadreSelect(),
                    Valor = x.ObtenerValorSelect(),
                    Texto = x.ObtenerTextoSelect()
                }).ToList();
            return options;
        }

        public GLPI_Proceso ObtenerProcesoPorId(int id) {
            return procesoDAL.ObtenerProcesoPorId(id);
        }

        public bool InsertarProceso(GLPI_Proceso proceso) {
            return procesoDAL.InsertarProceso(proceso) > 0;
        }

        public bool ActualizarProceso(GLPI_Proceso proceso) {
            return procesoDAL.ActualizarProceso(proceso) > 0;
        }

        public bool EliminarProceso(int id) {
            return procesoDAL.EliminarProceso(id) > 0;
        }
    }
}

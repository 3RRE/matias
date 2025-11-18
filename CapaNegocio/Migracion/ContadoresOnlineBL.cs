using CapaDatos.Migracion;
using CapaEntidad.Migracion;
using CapaEntidad.Reportes._9050;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Migracion {
    public class ContadoresOnlineBL {
        private ContadoresOnlineDAL contadoresOnlineDAL;

        public ContadoresOnlineBL() {
            contadoresOnlineDAL = new ContadoresOnlineDAL();
        }

        public bool GuardarContadoresOnline(List<ContadoresOnline> contadoresOnline) {
            return contadoresOnlineDAL.GuarGuardarContadoresOnline(contadoresOnline);
        }

        public List<ContadoresOnlineDto> ObtenerContadoresOnlineParaReporte9050(string fecha_Inicio, string fecha_fin, int codSala) {
            return contadoresOnlineDAL.ObtenerContadoresOnlineParaReporte9050(fecha_Inicio, fecha_fin, codSala);
        }

        public DateTime ObtenerFechaDeUltimoContadorPorCodSala(int codSala) {
            return contadoresOnlineDAL.ObtenerFechaDeUltimoContadorPorCodSala(codSala);
        }
    }
}

using CapaDatos.Alertas;
using CapaDatos.Disco;
using CapaEntidad.Alertas;
using CapaEntidad.Disco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Disco {
    public class AlertaDiscoBL {
        private AlertaDiscoDAL _alertaDiscoDal = new AlertaDiscoDAL();

        public List<AlertaDiscoEntidad> AlertaDisco_xdevicesListado(int codsala) {
            return _alertaDiscoDal.AlertaDisco_xdevicesListado(codsala);
        }
        public List<string> AlertaDiscosCorreosListado(int codsala)
        {
            return _alertaDiscoDal.AlertaDiscosCorreosListado(codsala);

        }


        //public AlertaDiscoEntidad AlertaDiscoAlertaIdObtenerJson(Int64 id, int CodSala) {
        //    return _alertaDiscoDal.AlertaDiscoAlertaIdObtenerJson(id, CodSala);
        //}
    }
}

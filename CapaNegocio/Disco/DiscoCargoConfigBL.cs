using CapaDatos.Disco;
using CapaEntidad.Alertas;
using CapaEntidad.Disco;
using CapaEntidad.Discos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Disco {
    public class DiscoCargoConfigBL {
        private DiscoCargoConfigDAL _discoConfigDal = new DiscoCargoConfigDAL();


        public List<DiscoCargoConfigEntidad> ListadoDiscoCargoConfig() {
            return _discoConfigDal.DiscoCargoConf_Listado();
        }

        public int DiscoCargoConfInsertarJson(DiscoCargoConfigEntidad discoConfig) {
            return _discoConfigDal.DiscoCargoConfInsertarJson(discoConfig);
        }

        public bool DiscoCargoConfEliminarJson(int id) {
            return _discoConfigDal.DiscoCargoConfEliminarJson(id);
        }
    }
}

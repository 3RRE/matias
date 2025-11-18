using CapaDatos.ContadoresNegativos;
using CapaDatos.Disco;
using CapaEntidad.ContadoresNegativos;
using CapaEntidad.Disco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ContadoresNegativos
{
    public class ContadorNegativoConfigBL
    {
        private ContadoresNegativosConfigDAL _alertaContador = new ContadoresNegativosConfigDAL();

        public List<ContadorNegativoConfigEntidad> ListadoContadorNegativoCargoConfig()
        {
            return _alertaContador.ContadoresNegativosConfig_Listado();
        }

        public int ContadorNegativoConfigInsertarJson(ContadorNegativoConfigEntidad discoConfig)
        {
            return _alertaContador.ContadorNegativoConfigInsertarJson(discoConfig);
        }

        public bool ContadorNegativoConfigEliminarJson(int id)
        {
            return _alertaContador.ContadorNegativoConfigEliminarJson(id);
        }
    }
}

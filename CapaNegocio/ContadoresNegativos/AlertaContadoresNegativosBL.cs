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
    public class AlertaContadoresNegativosBL
    {
        private AlertaContadoresNegativosDAL _contadorNegativo = new AlertaContadoresNegativosDAL();

        public List<AlertaContadorNegativoEntidad> AlertaContador_xdevicesListado(int codsala)
        {
            return _contadorNegativo.AlertaContadoresNegativos(codsala);
        }
        public List<string> AlertaContadorCorreosListado(int codsala)
        {
            return _contadorNegativo.AlertaContadorCorreosListado(codsala);

        }
    }
}

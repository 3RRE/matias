using CapaDatos.ContadoresNegativos;
using CapaEntidad.Alertas;
using CapaEntidad.ContadoresNegativos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.ContadoresNegativos
{
    public class ContadoresNegativosBL
    {
        private ContadoresNegativosDAL _contadorNegativo = new ContadoresNegativosDAL();
        
        public int AgregarContadorNegativo(ContadoresNegativosEntidad contador)
        {
            return _contadorNegativo.GuardarContadorNegativo(contador);
        }

        public List<ContadoresNegativosEntidad> ListadoContadoresNegativos(string codsala, DateTime fechaini, DateTime fechafin)
        {
            return _contadorNegativo.ListadoContadoresNegativosSala(codsala, fechaini, fechafin);
        }
    }
}

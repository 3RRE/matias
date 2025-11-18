using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class DepositoBL
    {
        public DepositoDAL depositodal = new DepositoDAL();
        public int DepositoInsertarJson(DepositoEntidad deposito)
        {
            return depositodal.DepositoInsertarJson(deposito);
        }
        public List<DepositoEntidad> DepositoListarDepositoSalaJson(int DepositoSala)
        {
            return depositodal.DepositoListarDepositoSalaJson(DepositoSala);
        }
        public bool DepositoEditarJson(DepositoEntidad deposito)
        {
            return depositodal.DepositoEditarJson(deposito);
        }
    }
}

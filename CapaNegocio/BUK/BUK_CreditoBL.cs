using CapaDatos.BUK;
using CapaEntidad.BUK;
using System.Collections.Generic;

namespace CapaNegocio.BUK {
    public class BUK_CreditoBL {
        private readonly BUK_CreditoDAL _creditoDAL;

        public BUK_CreditoBL() {
            _creditoDAL = new BUK_CreditoDAL();
        }

        public List<BUK_CreditoEntidad> ObtenerCreditos() {
            return _creditoDAL.ObtenerCreditos();
        }

        public List<BUK_CreditoEntidad> ObtenerCreditosDeEmpleadoByEmpresa(BUK_CreditoEntidad credito) {
            return _creditoDAL.ObtenerCreditosDeEmpleadoByEmpresa(credito);
        }

        public BUK_CreditoEntidad ObtenerCreditoPorId(int idCredito) {
            return _creditoDAL.ObtenerCreditoPorId(idCredito);
        }

        public int InsertarCredito(BUK_CreditoEntidad credito) {
            return _creditoDAL.InsertarCredito(credito);
        }

        public int AnularCredito(BUK_CreditoEntidad credito) {
            return _creditoDAL.AnularCredito(credito);
        }
    }
}

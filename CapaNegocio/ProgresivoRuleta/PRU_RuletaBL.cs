using CapaDatos.ProgresivoRuleta;
using CapaEntidad.ProgresivoRuleta.Dto;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.ProgresivoRuleta {
    public class PRU_RuletaBL {
        private readonly PRU_RuletaDAL _pruAlertaDAL = new PRU_RuletaDAL();

        public List<PRU_RuletaSelectDto> SeleccionarRuletasBySalaId(int salaId) {
            IEnumerable<PRU_RuletaSelectDto> items = _pruAlertaDAL.SeleccionarRuletasBySalaId(salaId);

            return items.OrderBy(x => x.Nombre).ToList();
        }
    }
}

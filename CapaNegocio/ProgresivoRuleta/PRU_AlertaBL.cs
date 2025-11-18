using CapaDatos.ProgresivoRuleta;
using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.ProgresivoRuleta {
    public class PRU_AlertaBL {
        private readonly PRU_AlertaDAL _pruAlertaDAL = new PRU_AlertaDAL();

        public List<PRU_AlertaDto> ObtenerAlertasByFechas(int salaId, int? ruletaId, DateTime fechaInicial, DateTime fechaFinal) {
            IEnumerable<PRU_AlertaDto> items = _pruAlertaDAL.ObtenerAlertasByFechas(salaId, fechaInicial, fechaFinal);

            if(ruletaId > 0) {
                items = items.Where(x => x.Ruleta.Id == ruletaId);
            }

            return items.OrderBy(x => x.FechaAlerta).ToList();
        }

        public bool InsertarAlerta(PRU_Alerta alerta) {
            return _pruAlertaDAL.InsertarAlerta(alerta) > 0;
        }
    }
}

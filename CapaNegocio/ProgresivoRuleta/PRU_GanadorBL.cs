using CapaDatos.ProgresivoRuleta;
using CapaEntidad.ProgresivoRuleta.Config;
using CapaEntidad.ProgresivoRuleta.Dto;
using CapaEntidad.ProgresivoRuleta.Entidades;
using CapaEntidad.ProgresivoRuleta.Filtro;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.ProgresivoRuleta {
    public class PRU_GanadorBL {
        private readonly PRU_GanadorDAL _pruGanadorDAL;
        private readonly PRU_AlertaDAL _pruAlertaDAL;

        public PRU_GanadorBL() {
            _pruGanadorDAL = new PRU_GanadorDAL();
            _pruAlertaDAL = new PRU_AlertaDAL();
        }

        public List<PRU_GanadorDto> ObtenerGanadoresByFechas(int salaId, int? ruletaId, DateTime fechaInicial, DateTime fechaFinal) {
            IEnumerable<PRU_GanadorDto> items = _pruGanadorDAL.ObtenerGanadoresByFechas(salaId, fechaInicial, fechaFinal);

            if(ruletaId > 0) {
                items = items.Where(x => x.Ruleta.Id == ruletaId);
            }

            return items.OrderBy(x => x.FechaGanador).ToList();
        }

        public bool InsertarGanador(PRU_Ganador ganador) {
            return _pruGanadorDAL.InsertarGanador(ganador) > 0;
        }

        public bool CambiarAcreditacionGanador(PRU_AcreditacionGanadorConfig acreditacion) {
            //obtengo el ganador ultimo ganado por codsala y idruleta
            PRU_Filtro filtro = new PRU_Filtro() {
                CodSala = acreditacion.CodSala,
                IdRuleta = acreditacion.IdRuleta
            };
            PRU_GanadorDto ultimoGanador = _pruGanadorDAL.ObtenerUltimoGanadorPorFiltro(filtro);
            if(!ultimoGanador.Existe()) {
                return false;
            }

            //actualizo el estado del ganador
            int idActualizado = _pruGanadorDAL.ActualizarAcreditacionGanador(ultimoGanador.Id, acreditacion.EsAcreditado);
            if(idActualizado <= 0) {
                return false;
            }

            //inserto en alerta
            PRU_Alerta alerta = new PRU_Alerta() {
                CodSala = acreditacion.CodSala,
                IdRuleta = acreditacion.IdRuleta,
                CodMaquina = ultimoGanador.CodMaquina,
                Monto = ultimoGanador.Monto,
                Detalle = acreditacion.Detalle,
                FechaAlerta = acreditacion.FechaAlerta,
            };
            return _pruAlertaDAL.InsertarAlerta(alerta) > 0;
        }

        public PRU_GanadorDto ObtenerUltimoGanadorPorFiltro(PRU_AcreditacionGanadorConfig acreditacion) {
            //obtengo el ganador ultimo ganado por codsala y idruleta
            PRU_Filtro filtro = new PRU_Filtro() {
                CodSala = acreditacion.CodSala,
                IdRuleta = acreditacion.IdRuleta
            };
            PRU_GanadorDto ultimoGanador = _pruGanadorDAL.ObtenerUltimoGanadorPorFiltro(filtro);
            if(!ultimoGanador.Existe()) {
                return null;
            }

            return ultimoGanador;
        }

    }
}


using System.Collections.Generic;
using System;
using CapaDatos.AnalisisDataSala;
using CapaEntidad.AnalisisDataSala;

namespace CapaNegocio.AnalisisDataSala {
    public class AnalisisDataSalaBL {
        private readonly AnalisisDataSalaDAL _analisisDal = new AnalisisDataSalaDAL();

        // NOTA: Debes crear las clases DTO (ej. IdleTimeKpiDto, IdleTimePorHoraDto)
        // en tu proyecto de Entidades para que el DAL pueda devolverlas.

        public IdleTimeKpiDto GetKpiGeneral(string codSala, DateTime fecha) {
            // (Aquí podrías añadir lógica de negocio, ej. calcular el % de utilización)
            return _analisisDal.GetKpiGeneral(codSala, fecha);
        }

        public List<IdleTimePorHoraDto> GetUtilizacionPorHora(string codSala, DateTime fecha) {
            return _analisisDal.GetUtilizacionPorHora(codSala, fecha);
        }

        public List<IdleTimeRankingDto> GetRankingMaquinas(string codSala, DateTime fecha) {
            return _analisisDal.GetRankingMaquinas(codSala, fecha);
        }

        public List<IdleTimeTimelineDto> GetTimelineMaquina(string codSala, DateTime fecha, string codMaq) {
            return _analisisDal.GetTimelineMaquina(codSala, fecha, codMaq);
        }

        // -----------------------------------------------------------------
        // --- NUEVOS MÉTODOS PARA HIT FREQUENCY ---
        // -----------------------------------------------------------------

        public HitFrecKpiDto GetHitFrecGeneral(string codSala, DateTime fecha) {
            return _analisisDal.GetHitFrecGeneral(codSala, fecha);
        }

        public List<HitFrecRankingDto> GetHitFrecPorMaquina(string codSala, DateTime fecha) {
            return _analisisDal.GetHitFrecPorMaquina(codSala, fecha);
        }

        public List<HitFrecLogDto> GetHitFrecLogDetallado(string codSala, DateTime fecha, string codMaq) {
            return _analisisDal.GetHitFrecLogDetallado(codSala, fecha, codMaq);
        }
    }
}

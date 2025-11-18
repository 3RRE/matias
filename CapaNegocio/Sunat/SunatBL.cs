using CapaDatos.Sunat;
using CapaEntidad.Sunat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.Sunat {
    public class SunatBL {
        private readonly SunatDAL sunatDAL;

        public SunatBL() {
            sunatDAL = new SunatDAL();
        }

        #region Contadores
        public bool GuardarContadoresSunat(List<ContadoresSunatEntidad> contadoresSunat) {
            int codSala = contadoresSunat.FirstOrDefault()?.CodSala ?? 0;
            List<int> idsInsertar = contadoresSunat.Select(c => c.IdConSunat).ToList();
            List<int> idsExistentes = sunatDAL.ObtenerIdsContadoresExistentes(idsInsertar, codSala);
            List<ContadoresSunatEntidad> contadoresSunatInsertar = contadoresSunat.Where(x => !idsExistentes.Contains(x.IdConSunat)).ToList();
            return sunatDAL.GuardarContadoresSunat(contadoresSunatInsertar);
        }
        public List<ContadoresSunatEntidad> ObtenerUltimosContadoresSunat(int cantidadDias, List<int> codigosSalas) {
            string codSalas = string.Join(", ", codigosSalas);
            return sunatDAL.ObtenerUltimosContadoresSunat(cantidadDias, codSalas);
        }
        public int ObtenerUltimoIdContadorSunatPorCodSala(int codSala) {
            return sunatDAL.ObtenerUltimoIdContadorSunatPorCodSala(codSala);
        }
        public List<ContadoresSunatEntidad> ObtenerContadoresSunatxFecha(int codSala, DateTime fechaIni, DateTime fechaFin) {
            return sunatDAL.ObtenerContadoresSunatxFecha(codSala, fechaIni, fechaFin);
        }
        public bool EditarContadorSunat(ContadoresSunatEntidad cliente, int codSala) {
            return sunatDAL.EditarContadoresSunat(cliente, codSala);
        }
        #endregion

        #region Eventos
        public bool GuardarEventosSunat(List<EventosSunatEntidad> eventosSunat) {
            int codSala = eventosSunat.FirstOrDefault()?.CodSala ?? 0;
            List<int> idsInsertar = eventosSunat.Select(c => c.IdEvSunat).ToList();
            List<int> idsExistentes = sunatDAL.ObtenerIdsEventosExistentes(idsInsertar, codSala);
            List<EventosSunatEntidad> contadoresSunatInsertar = eventosSunat.Where(x => !idsExistentes.Contains(x.IdEvSunat)).ToList();
            return sunatDAL.GuardarEventosSunat(contadoresSunatInsertar);
        }
        public List<EventosSunatEntidad> ObtenerUltimosEventosSunat(int cantidadDias, List<int> codigosSalas) {
            string codSalas = string.Join(", ", codigosSalas);
            return sunatDAL.ObtenerUltimosEventosSunat(cantidadDias, codSalas);
        }
        public List<EventosSunatEntidad> ObtenerEventosSunatxSala(int codSala, DateTime fechaIni, DateTime fechaFin) {
            return sunatDAL.ObtenerEventosSunatxSala(codSala, fechaIni, fechaFin);
        }
        public bool EditarEventoSunat(EventosSunatEntidad cliente, int codSala) {
            return sunatDAL.EditarEventoSunat(cliente, codSala);
        }
        public int ObtenerUltimoIdEventoSunatPorCodSala(int codSala) {
            return sunatDAL.ObtenerUltimoIdEventoSunatPorCodSala(codSala);
        }
        #endregion
    }
}

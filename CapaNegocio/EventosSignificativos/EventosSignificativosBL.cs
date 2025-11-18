using CapaDatos.EventosSignificativos;
using CapaDatos.Sunat;
using CapaEntidad.EventosSignificativos;
using CapaEntidad.Sunat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio.EventosSignificativos {
    public class EventosSignificativosBL {
        private EventosSignificativosDAL _eventoSignificativo = new EventosSignificativosDAL();

        public List<EventosSignificativosEntidad> ListarEventosSignificativos(int codSala, DateTime fechaIni, DateTime fechaFin) {
            return _eventoSignificativo.ListadoEventosSignificativos(codSala, fechaIni, fechaFin);
        }
        public bool GuardarEventosSignificativos(List<EventosSignificativosEntidad> eventosLista) {
            string codSala = eventosLista.FirstOrDefault()?.COD_SALA ?? "";
            List<int> idsInsertar = eventosLista.Select(c => c.Cod_Even_OL).ToList();
            List<int> idsExistentes = _eventoSignificativo.ObtenerIdsEventosSignificativosExistentes(idsInsertar, codSala);
            List<EventosSignificativosEntidad> eventosSignificativosInsertar = eventosLista.Where(x => !idsExistentes.Contains(x.Cod_Even_OL)).ToList();
            return _eventoSignificativo.GuardarEventosSignificativos(eventosSignificativosInsertar);
        }
    }
}

using System;
using System.Collections.Generic;

namespace CapaEntidad.Cortesias.Reporte {
    public class CRT_ReporteFiltro {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public List<int> CodsSala { get; set; } = new List<int>();
        public List<int> IdsTipo { get; set; } = new List<int>();
        public List<int> IdsSubTipo { get; set; } = new List<int>();
        public List<int> IdsProducto { get; set; } = new List<int>();
        public string CodMaquina { get; set; }
        public string IdSalaMaquina { get; set; }

        public bool TieneSalas() {
            return CodsSala.Count > 0;
        }

        public bool TieneTipos() {
            return IdsTipo.Count > 0;
        }

        public bool TieneSubTipos() {
            return IdsSubTipo.Count > 0;
        }

        public bool TieneProductos() {
            return IdsProducto.Count > 0;
        }

        public bool TieneMaquina() {
            return !string.IsNullOrEmpty(CodMaquina);
        }

        public bool TieneSalaMaquina() {
            return !string.IsNullOrEmpty(IdSalaMaquina);
        }
    }
}

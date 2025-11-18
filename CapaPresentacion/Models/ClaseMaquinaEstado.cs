using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Models {
    public class MaquinaPorSala {
        public int IdSala { get; set; }
        public string Sala { get; set; }
        public int MaquinasConectadas { get; set; }
        public int MaquinasDesconectadas { get; set; }
        public int TotalMaquinas { get; set; }
    }
}

namespace CapaPresentacion.Models {
    public class ConsolidadoMaquinas {
        public string Sala { get; set; }
        public int MaquinasConectadas { get; set; }
        public int MaquinasDesconectadas { get; set; }
        public int TotalMaquinas { get; set; }
    }
}
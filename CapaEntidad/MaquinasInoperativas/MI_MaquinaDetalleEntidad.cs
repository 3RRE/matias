using System.Collections.Generic;

namespace CapaEntidad.MaquinasInoperativas {
    public class MI_MaquinaDetalleEntidad {
        public int CodMaquina { get; set; }
        public int CodLinea { get; set; }
        public int CodJuego { get; set; }
        public int CodSala { get; set; }
        public int CodModeloMaquina { get; set; }
        public int CodMarcaMaquina { get; set; }
        public int CodContrato { get; set; }
        public int CodFicha { get; set; }
        public string CodMaquinaLey { get; set; }
        public string NombreLinea { get; set; }
        public string NroSerie { get; set; }
        public string NombreJuego { get; set; }
        public string NombreSala { get; set; }
        public string NombreModeloMaquina { get; set; }
        public string DescripcionContrato { get; set; }
        public string NombreFicha { get; set; }
        public string NombreMarcaMaquina { get; set; }
        public double Token { get; set; }
        public MI_MaquinaZonaEntidad Zona { get; set; } = new MI_MaquinaZonaEntidad();
        public MI_MaquinaIslaEntidad Isla { get; set; } = new MI_MaquinaIslaEntidad();
        public int Posicion { get; set; }
    }

    public class MI_MaquinaZonaEntidad {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class MI_MaquinaIslaEntidad {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class MI_MaquinaDetalleResponse {
        public bool respuesta { get; set; }
        public MI_MaquinaDetalleEntidad data { get; set; }
    }

    public class MI_MaquinaDetalleListResponse {
        public bool respuesta { get; set; }
        public List<MI_MaquinaDetalleEntidad> data { get; set; }
    }
}

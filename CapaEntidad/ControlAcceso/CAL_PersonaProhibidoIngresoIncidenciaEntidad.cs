using System;

namespace CapaEntidad.ControlAcceso {
    public class CAL_PersonaProhibidoIngresoIncidenciaEntidad {
        public int TimadorIncidenciaID { get; set; }
        public int TimadorID { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int EmpleadoID { get; set; }
        public int CodSala { get; set; }
        public string Observacion { get; set; }
        public int SustentoLegal { get; set; }


        public string EmpleadoNombres { get; set; }
        public string EmpleadoApellidoPaterno { get; set; }
        public string EmpleadoNombreCompleto { get; set; }
        public string SalaNombre { get; set; }

    }
}

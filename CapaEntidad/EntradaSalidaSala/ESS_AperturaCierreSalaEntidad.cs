using System;
using System.Collections.Generic;

namespace CapaEntidad.EntradaSalidaSala {
    public class ESS_AperturaCierreSalaEntidad {

        public int IdAperturaCierreSala { get; set; }
        public int IdEmpleadoSEG { get; set; }
        public int CodSala { get; set; }
        public string Sala { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraApertura { get; set; }
        public string PrevencionistaApertura { get; set; }
        public string JefeSalaApertura { get; set; }
        public string ObservacionesApertura { get; set; }
        public TimeSpan HoraCierre { get; set; }
        public string PrevencionistaCierre { get; set; }
        public string JefeSalaCierre { get; set; }
        public int? IdPrevencionistaApertura { get; set; }
        public int? IdJefeSalaApertura { get; set; } 
        public int? IdPrevencionistaCierre { get; set; }
        public int? IdJefeSalaCierre { get; set; }
        public string ObservacionesCierre { get; set; }

        public int? Estado { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

        public int IdBuk { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public DateTime? FechaLimiteEditarAperturaCierreSala { get; set; }
    }
    public class ESS_AperturaCierreSalaPersonaEntidad {

        public int? IdBuk { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public int? IdCargo { get; set; }
        public string NumeroDocumento { get; set; }
    }
} 
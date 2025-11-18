using System;
using System.Collections.Generic;

namespace CapaEntidad.MaquinaEstado {
    public class TEC_EstadoMaquinaEntidad {
        public Int64 id { get; set; } 
        public int sala_id { get; set; } 
        public string sala { get; set; }
        public int CantMaquinaConectada { get; set; } 
        public int CantMaquinaNoConectada { get; set; }
        public int CantMaquinaPLay { get; set; } 
        public int CantMaquinaRetiroTemporal { get; set; }
        public int TotalMaquina { get; set; }
        public DateTime FechaOperacion { get; set; }  
        public DateTime FechaCierre{ get; set; }  

        public List<TEC_EstadoMaquinaDetalleEntidad> Maquinas { get; set; }
        public List<TEC_RegistroMaquinaEntidad> RegistroMaquina { get; set; }
        public List<TEC_HistorialMaquinaEntidad> ListadoMaquinaEstado { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int Accion { get; set; }


    } 

    public class TEC_ConsolidadoMaquina {

        public int TotalConectadas { get; set; }
        public int TotalDesconectadas { get; set; }
        public int TotalMaquinaPLay { get; set; } 
        public int TotalRetiroTemporal { get; set; }
        public int TotalMaquinas { get; set; } 
    }

    public class TEC_estadomaquinaLista {

        public List<TEC_EstadoMaquinaEntidad> lista { get; set; }
        public TEC_ConsolidadoMaquina consolidado { get; set; }

    }
    public class TEC_EstadoMaquinaDetalleEntidad{
        public int IdEstadoMaquinaDetalle { get; set; }
        public int IdEstadoMaquina { get; set; }
        public string CodMaquina { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class TEC_RegistroMaquinaEntidad {

        public int IdRegistroMaquina { get; set; }
        public int CodSala { get; set; }
        public string NombreSala { get; set; }
        public string CodMaquinaINDECI { get; set; }
        public string CodMaquinaRD { get; set; }
        public string TipoRegistroMaquina { get; set; }
        public int TotalMaquina { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class TEC_HistorialMaquinaEntidad {
        public Int64 IdHistorialMaquina { get; set; }
        public int CodSala { get; set; }
        public string Sala { get; set; }
        public string CodMaquina { get; set; }
        public string EstadoMaquina { get; set; }
        public DateTime FechaOperacion { get; set; }
    }
}
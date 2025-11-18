using System;

namespace CapaEntidad.AsistenciaCliente {
    public class AST_ClienteSalaEntidad {
        public AST_ClienteSalaEntidad() {
            this.TipoJuego = new AST_TipoJuegoEntidad();
            this.TipoFrecuencia = new AST_TipoFrecuenciaEntidad();
            this.TipoCliente = new AST_TipoClienteEntidad();
        }

        public int ClienteId { get; set; }
        public int SalaId { get; set; }
        public int TipoFrecuenciaId { get; set; }
        public int TipoJuegoId { get; set; }
        public double ApuestaImportante { get; set; }
        public int TipoJId { get; set; }
        public int TipoClienteId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string TipoRegistro { get; set; }
        public bool EnviaNotificacionWhatsapp { get; set; }
        public bool EnviaNotificacionSms { get; set; }
        public bool EnviaNotificacionEmail { get; set; }
        public bool LlamadaCelular { get; set; }
        public bool EsLudopata { get; set; }
        public AST_TipoJuegoEntidad TipoJuego { get; set; }
        public AST_TipoFrecuenciaEntidad TipoFrecuencia { get; set; }
        public AST_TipoClienteEntidad TipoCliente { get; set; }
        public SalaEntidad Sala { get; set; }

        public bool Existe() {
            return ClienteId > 0 && SalaId > 0;
        }
    }
}

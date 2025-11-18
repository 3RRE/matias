using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.AsistenciaCliente
{
    public class AST_AsistenciaClienteSalaEntidad
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int SalaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int TipoFrecuenciaId { get; set; }
        public int TipoJuegoId { get; set; }
        public int TipoClienteId { get; set; }
        public double ApuestaImportante { get; set; }
        public string CodMaquina { get; set; }
        public string JuegoMaquina { get; set; }
        public string MarcaMaquina { get; set; }
        public AST_ClienteEntidad Cliente { get; set; }
        public SalaEntidad Sala{ get; set; }
        public AST_TipoFrecuenciaEntidad TipoFrecuencia { get; set; }
        public AST_TipoJuegoEntidad TipoJuego { get; set; }
        public AST_TipoClienteEntidad TipoCliente { get; set; }
        public AST_AsistenciaClienteSalaEntidad()
        {
            this.Cliente = new AST_ClienteEntidad();
            this.Sala = new SalaEntidad();
            this.TipoJuego = new AST_TipoJuegoEntidad();
            this.TipoFrecuencia = new AST_TipoFrecuenciaEntidad();
            this.TipoCliente = new AST_TipoClienteEntidad();
        }
    }
}

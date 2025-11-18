using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ocurrencias
{
    public class OCU_OcurrenciaEntidad
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombres { get; set; }
        public string ApelPat { get; set; }
        public string ApelMat { get; set; }
        public int TipoDocId { get; set; }
        public string NroDoc { get; set; }
        public int TipoOcurrenciaId { get; set; }
        public string Descripcion { get; set; }
        public string JefeSala { get; set; }
        public string SeInformoA { get; set; }
        public int CodSala { get; set; }
        public int UsuarioReg { get; set; }
        public int Enviado { get; set; }
        public OCU_TipoOcurrenciaEntidad TipoOcurrencia { get; set; }
        public TipoDOIEntidad TipoDocumento { get; set; }
        public SalaEntidad Sala { get; set; }
        public SEG_UsuarioEntidad SEG_Usuario {get;set;}
        public string Hash { get; set; }
        public OCU_OcurrenciaEntidad()
        {
            this.TipoOcurrencia = new OCU_TipoOcurrenciaEntidad();
            this.TipoDocumento = new TipoDOIEntidad();
            this.Sala = new SalaEntidad();
            this.SEG_Usuario = new SEG_UsuarioEntidad();
        }
    }
}

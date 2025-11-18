using System;
namespace CapaEntidad
{
    public class HistorialProgresivoEntidad
    {
        public int CodHistorialProgresivo { get; set; }
        public int CodSala { get; set; }
        public int CodProgresivo { get; set; }
        public string Parametros { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int UsuarioID { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CapaEntidad.ExcelenciaOperativa
{
    public class EO_FichaExcelenciaOperativaEntidad
    {
        public long FichaId { get; set; }
        public int UsuarioId { get; set; }
        public int SalaId { get; set; }
        public string UsuarioNombre { get; set; }
        public string SalaNombre { get; set; }
        public int Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public float PuntuacionObtenida { get; set; }
        public float PuntuacionBase { get; set; }
        public float Porcentaje { get; set; }
        public DateTime? FechaCreado { get; set; }
        public DateTime? FechaActualizado { get; set; }
        public List<EO_FichaCategoriaEntidad> Categorias { get; set; }
        public string Codigo { get; set; }
        public int FichaVersion { get; set; }
    }
}

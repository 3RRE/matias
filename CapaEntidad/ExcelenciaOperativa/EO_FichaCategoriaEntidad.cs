using System.Collections.Generic;

namespace CapaEntidad.ExcelenciaOperativa
{
    public class EO_FichaCategoriaEntidad
    {
        public long CategoriaId { get; set; }
        public long FichaId { get; set; }
        public string Nombre { get; set; }
        public float PuntuacionObtenida { get; set; }
        public float PuntuacionBase { get; set; }
        public float Porcentaje { get; set; }
        public List<EO_FichaItemEntidad> Items { get; set; }
        public string Codigo { get; set; }
    }
}

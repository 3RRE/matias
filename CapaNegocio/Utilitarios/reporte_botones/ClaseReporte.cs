using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Utilitarios.reporte_botones
{
    public class ClaseReporte
    {
        public string nombrearchivo { get; set; }
        public string tituloreporte { get; set; }
        public string titulo_subtitulo { get; set; }
        public string nombrehoja { get; set; }
        public string porcentajes { get; set; }
        public JArray Tablas { get; set; }
        public JToken Cabecera { get; set; }
        public JToken Cabecera_reporte { get; set; }
        public JToken tabladefinicioncolumnas { get; set; }
        public string tablaaoHeader { get; set; }
        public bool usardatatable { get; set; }
        public bool multiplestablas { get; set; }
        public bool mostrar_headers_tabla { get; set; }
        public EstilosReporte estilos_reporte { get; set; } = new EstilosReporte();

    }
    public class REPORTES_OBJ
    {
        public List<ClaseReporte> TABLAS_DATOS { get; set; } = new List<ClaseReporte>();
        public string nombrearchivo { get; set; }


    }
}

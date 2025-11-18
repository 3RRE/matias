using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Utilitarios.reporte_botones
{
    /// <summary>
    /// Clase para estilos de reportes pdf y excel
    /// </summary>
    public class EstilosReporte
    {
        /*HEADERS PAGINA*/    //empresa sala 
                              /*TITULO REPORTE*/
                              /*CABECERAS TABLA*/   //fecha filtros
                                                    /*TABLA DATOS*/
                                                    /*TABLA FOOTER*/





        public EstilosReporte(string json = "")
        {
        }

        public string orientacion_pdf { get; set; } = "vertical";///horizontaLS
        public string font_name { get; set; } = "Calibri";

        public double fontsize { get; set; } = 13;
        public String color { get; set; } = "#000000";

        public int margen_cm_left { get; set; } = 1;
        public int margen_cm_right { get; set; } = 1;
        public int margen_cm_top { get; set; } = 1;
        public int margen_cm_bottom { get; set; } = 1;


        public int headers_pagina_font_size { get; set; } = 10;
        public int titulo_reporte_font_size { get; set; } = 16;
        public int titulo_subtitulo_reporte_font_size { get; set; } = 13;
        public int cabeceras_tabla_font_size { get; set; } = 11;
        public int tabla_datos_header_font_size { get; set; } = 10;

        public int tabla_datos_font_size { get; set; } = 10;
        public int tabla_footer_font_size { get; set; } = 10;

        /// <summary>
        /// Porcentages de los headers de página pdf
        /// </summary>
        public string headers_pagina_porcentajes { get; set; } = "10, 45, 20, 25";

        public string cabeceras_reporte_porcentajes { get; set; } = "25,35,20,20";


        /// <summary>
        /// Porcentages headers DE TABLA 
        /// </summary>
        //public string headers_reporte_porcentajes{ get; set; } = "25,75";
        public string cabeceras_tabla_porcentajes { get; set; } = "25,75";
        //FIN CABECERAS  DE CADA TABLA


        public string cabeceras_tabla_nombre_color { get; set; } = "#000000";
        public bool cabeceras_tabla_nombre_bold { get; set; } = true;
        public bool cabeceras_tabla_valor_bold { get; set; } = false;
        public string cabeceras_tabla_valor_color { get; set; } = "#0000FF";
        public string cabeceras_tabla_col_nombre_alinear { get; set; } = "center";
        public int cabeceras_tabla_col_nombre_rightindent { get; set; } = 0;
        public int cabeceras_tabla_col_nombre_lefttindent { get; set; } = 0;
        public string cabeceras_tabla_col_valor_alinear { get; set; } = "left";
        public int cabeceras_tabla_col_valor_rightindent { get; set; } = 0;
        public int cabeceras_tabla_col_valor_lefttindent { get; set; } = 0;


        public int tabla_datos_margin_top { get; set; } = 3;
        public int cabeceras_reporte_margin_top { get; set; } = 3;


        public double tabla_datos_porcentaje { get; set; } = 100;
        public string tabla_alinear { get; set; } = "center";

        public string tabla_datos_header_font_color { get; set; } = "#000000";
        public bool tabla_datos_header_font_bold { get; set; } = true;
        public string tabla_datos_header_alinear { get; set; } = "center";
        public string tabla_datos_body_alinear { get; set; } = "left";
        public string tabla_datos_body_tipo { get; set; } = "string";
        public string tabla_datos_body_formato { get; set; } = "";

        /// <summary>
        /// Colspan para celdas excel
        /// </summary>
        public int tabla_datos_cell_colspan { get; set; } = 1;
        public int tabla_datos_cell_rowspan { get; set; } = 1;
        public int tabla_footer_cell_colspan { get; set; } = 1;
        public int tabla_footer_cell_rowspan { get; set; } = 1;



        public String tabla_footer_color { get; set; } = "#0000FF";
        public string tabla_footer_font_color { get; set; } = "#000000";
        public bool tabla_footer_font_bold { get; set; } = true;


        public int tabla_footer_bordes_width { get; set; } = 0;
        public int tabla_datos_header_bordes_width { get; set; } = 0;
        public bool tabla_datos_header_subrayado { get; set; } = false;
        public int tabla_datos_bordes_width { get; set; } = 0;

        public int tabla_datos_header_border_bottom_width { get; set; } = 0;
        public int tabla_datos_header_border_top_width { get; set; } = 0;

        public int tabla_footer_border_top_width { get; set; } = 0;





        public int cabeceras_tabla_columna_inicio { get; set; } = 1;
        public int tabla_datos_columna_inicio { get; set; } = 1;
        public int tabla_footer_columna_inicio { get; set; } = 1;


        public string tabla_datos_header_background_color { get; set; } = "#FFFFFF";
        public string tabla_datos_body_background_color_row { get; set; } = "#FFFFFF";
        public string tabla_datos_body_background_color_even { get; set; } = "#FFFFFF";

        public int tabla_datos_rows_bottom_padding { get; set; } = 2;
        public int tabla_datos_rows_top_padding { get; set; } = 2;

    }


    public class definicioncolumnas
    {
        public definicioncolumnas()
        {
        }
        public string nombre { get; set; }
        public string tipo { get; set; } = "STRING";
        public string alinear { get; set; } = "LEFT";
        public bool sumar { get; set; } = false;
        public string formato { get; set; } = "";/// formato  decimal " #,##0.00",  moneda "\"S/\" #,##0.00"

        public List<string> footer { get; set; } = new List<string>();

        public decimal valor_suma { get; set; }
        public string valor_suma2 { get; set; }
        public string footer_font_color { get; set; } = "#000000";
        public bool footer_font_bold { get; set; } = false;
        public string footer_alinear { get; set; } = "center";
        public string header_alinear { get; set; } = "center";

        public int border_bottom_width { get; set; } = 0;
        public int border_top_width { get; set; } = 0;
        public int colspan { get; set; } = 1;
        public int rowspan { get; set; } = 1;




    }


}

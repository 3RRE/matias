class EstilosReporte {
    constructor(options = {}) {
        Object.assign(this, {
            /*HEADERS PAGINA*/    //empresa sala 
            /*TITULO REPORTE*/
            /*CABECERAS TABLA*/   //fecha filtros
            /*TABLA DATOS*/
            /*TABLA FOOTER*/
            font_name: "Calibri",
            fontsize: 13,
            color: "#000000",
            orientacion_pdf: "vertical",///horizontaLS

            margen_cm_left: 1,
            margen_cm_right: 1,
            margen_cm_top: 1,
            margen_cm_bottom: 1,

            headers_pagina_font_size: 10,
            titulo_reporte_font_size: 15,
            titulo_subtitulo_reporte_font_size: 13,
            cabeceras_tabla_font_size: 11,
            tabla_datos_font_size: 10,
            tabla_datos_header_font_size: 10,
            tabla_footer_font_size: 10,


            headers_pagina_porcentajes: "10, 45, 20, 25",
            /////%  cabeceras de reporte 
            cabeceras_reporte_porcentajes: "25,35,20,20",

            //  headers_reporte_porcentajes   : "25,75",


            //// %  cabeceras de subtablas
            cabeceras_tabla_porcentajes: "25,75",
            //FIN CABECERAS  DE CADA TABLA


            cabeceras_tabla_nombre_color: "#000000",
            cabeceras_tabla_nombre_bold: true,
            cabeceras_tabla_valor_bold: false,
            cabeceras_tabla_valor_color: "#0000FF",
            cabeceras_tabla_col_nombre_alinear: "center",
            cabeceras_tabla_col_nombre_rightindent: 0,
            cabeceras_tabla_col_nombre_lefttindent: 0,
            cabeceras_tabla_col_valor_alinear: "left",
            cabeceras_tabla_col_valor_rightindent: 0,
            cabeceras_tabla_col_valor_lefttindent: 0,

            tabla_datos_cell_colspan: 1,
            tabla_datos_cell_rowspan: 1,
            tabla_footer_cell_colspan: 1,
            tabla_footer_cell_rowspan: 1,


            tabla_datos_porcentaje: 100,
            tabla_alinear: "center",

            tabla_datos_header_font_color: "#000000",
            tabla_datos_header_font_bold: true,
            tabla_datos_header_alinear: "center",
            tabla_datos_body_alinear: "center",
            tabla_datos_body_tipo: "string",

            tabla_footer_color: "#0000FF",
            tabla_footer_font_color: "#000000",
            tabla_footer_font_bold: true,


            tabla_footer_bordes_width: 0,
            tabla_datos_header_bordes_width: 0,
            tabla_datos_header_subrayado: false,
            tabla_datos_bordes_width: 0,

            tabla_datos_header_border_bottom_width: 0,
            tabla_datos_header_border_top_width: 0,
            tabla_footer_border_top_width: 0,

            cabeceras_tabla_columna_inicio: 1,
            tabla_datos_columna_inicio: 1,
            tabla_footer_columna_inicio: 1,


            tabla_datos_header_background_color: "#FFD3D3D3",
            tabla_datos_body_background_color_row: "#FFFFFF",
            tabla_datos_body_background_color_even: "#FFFFFF",

            tabla_datos_rows_bottom_padding: 2,
            tabla_datos_rows_top_padding: 2

        }, options);
    }
}






class definicioncolumnas {
    constructor(options = {}) {
        Object.assign(this, {
            nombre: "",
            tipo: "STRING",
            alinear: "LEFT",
            sumar: false,
            formato: "",
            footer: [],
            valor_suma: 0,
            valor_suma2: "",
            footer_font_color: "#000000",
            footer_font_bold: false,
            footer_alinear: "center",
            header_alinear: "center",

            border_bottom_width: 0,
            border_top_width: 0,
            colspan: 1,
            rowspan: 1,
        }, options);
    }
}

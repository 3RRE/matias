using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CapaNegocio.Utilitarios.reporte_botones
{
    public class Excel_botones
    {
        public static byte[] ExportarExcel_funcion_botones(JObject DATA)
        {
            byte[] result = null;
            var formato_decimal = System.Configuration.ConfigurationManager.AppSettings["formatoNumero"] == null ? "#,##0.00" : System.Configuration.ConfigurationManager.AppSettings["formatoNumero"];
            var OBJETO_DATA = DATA;
            var TABLAS_ARRAY = OBJETO_DATA["TABLAS_DATOS"];
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                foreach (var TABLA in TABLAS_ARRAY)
                {
                    var data = TABLA;

                    var tituloreporte = data["tituloreporte"];
                    var usardatatable = data["usardatatable"];
                    var fecha = data["fecha"];
                    var tablas = data["Tablas"];
                    var tabladefinicioncolumnas = data["tabladefinicioncolumnas"];
                    var aoHeader = data["tablaaoHeader"];

                    var datos = data["Tablas"][0];

                    var invisibles = data["invisibles"];


                    var cabeceras = data["Cabecera"];
                    var cabeceras_reporte = data["Cabecera_reporte"];

                    var multiplestablas = data["multiplestablas"];
                    var mostrartablaheaders = data["mostrar_headers_tabla"];

                    var titulo_subtitulo = data["titulo_subtitulo"];

                    var data_estilos_reporte = data["estilos_reporte"];
                    EstilosReporte ESTILOS_REPORTE = new EstilosReporte();
                    if (data_estilos_reporte != null)
                    {
                        ESTILOS_REPORTE = JsonConvert.DeserializeObject<EstilosReporte>(data_estilos_reporte.ToString());
                    }

                    if (usardatatable == null)
                    {
                        usardatatable = "false";
                    }
                    bool multiples_tablas = false;
                    if (multiplestablas != null)
                    {
                        multiples_tablas = (bool)multiplestablas;
                    }
                    bool mostrar_headers_tabla = true;
                    if (mostrartablaheaders != null)
                    {
                        mostrar_headers_tabla = (bool)mostrartablaheaders;
                    }

                    var nombrehoja = data["nombrehoja"];
                    var string_nombrehoja = nombrehoja != null ? (string)nombrehoja : "Hoja 1";
                    //var colorthcabecera = data["colorthcabecera"];

                    var tablaprimera = data["Tablas"][0];
                    ExcelWorksheet worksheet = CREARHOJA(package, string_nombrehoja);
                    worksheet.Cells.Style.Font.Name = ESTILOS_REPORTE.font_name;
                    ExcelRange celda;
                    int comienzoy = 2;
                    int comienzox = 1;
                    //worksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
                    comienzoy = GENERAR_HEADERS_PAGINA(ESTILOS_REPORTE, worksheet, cabeceras, comienzox, comienzoy);
                    //comienzoy++;


                    comienzoy = GENERAR_TITULO_REPORTE(ESTILOS_REPORTE, worksheet, tituloreporte.ToString(), comienzox, comienzoy);

                    ///subtitulo
                    if (titulo_subtitulo != null)
                    {
                        if (titulo_subtitulo.ToString() != "")
                        {
                            comienzoy = GENERAR_SUBTITULO_REPORTE(ESTILOS_REPORTE, worksheet, titulo_subtitulo.ToString(), comienzox, comienzoy);

                        }
                    }

                    comienzoy++;
                    ////////////CABECERAS REPORTE
                    comienzox = 1;
                    comienzoy = GENERAR_CABECERA_REPORTE(ESTILOS_REPORTE, worksheet, cabeceras_reporte, comienzox, comienzoy);

                    comienzoy++;
                    comienzoy++;
                    ///NUEVOOOOOOOO
                    //foreach (var TABLA in tablas)
                    //{
                    var tabladatos = datos;//tablaactual["datos"];
                    var primerelemento = (JObject)tabladatos.First();
                    var propiedades = primerelemento.Properties();
                    comienzox = 1;
                    //var color_thcabecera = Color.FromName((string)colorthcabecera);

                    var invi = new List<string>();

                    if (invisibles != null && invisibles.ToString() != "")
                    {
                        var invisiblesobj = JArray.Parse(invisibles.ToString());
                        var pror = propiedades.ToList();
                        foreach (var invisislb in invisiblesobj)
                        {
                            invi.Add(pror[Convert.ToInt32(invisislb)].Name.ToString());
                        }
                    }


                    var Columnas = get_columnas_propiedades(primerelemento, (string)tabladefinicioncolumnas, invi, ESTILOS_REPORTE);
                    ///nuevo headers    TABLA /////////////////////////////////////////// TABLA
                    bool usar_datatable = (bool)usardatatable;
                    //if (aoHeader != null && (string)aoHeader != "")
                    string color_header = ESTILOS_REPORTE.tabla_datos_header_background_color;
                    var color_head = get_color(color_header);
                    //color_head = Color.LightGray;
                    if (mostrar_headers_tabla)
                    {
                        if (usar_datatable)
                        {
                            foreach (var trfila in JArray.Parse(aoHeader.ToString()))/////3  trs filas de header
                            {


                                var nrofila = (string)trfila["nrofila"];
                                var ths = trfila["ths"];
                                int contadorcolumnas = ESTILOS_REPORTE.tabla_datos_columna_inicio;
                                foreach (var th in JArray.Parse(ths.ToString()))/////27  ths de tr  
                                {
                                    var th_text = (string)th["text"];
                                    var colspan = (int)th["colspan"]; ////columnas para derecha
                                    var alinear = (string)th["alinear"];

                                    var columna_obj = Columnas.Where(x => x.nombre == th_text).FirstOrDefault();
                                    colspan = columna_obj.colspan;

                                    var rowspan = (int)th["rowspan"];///filas par abajo
                                    celda = worksheet.Cells[comienzoy, contadorcolumnas, comienzoy + rowspan - 1, contadorcolumnas + colspan - 1];
                                    contadorcolumnas = contadorcolumnas + colspan;
                                    celda.Value = th_text;
                                    if (colspan > 1)
                                    {
                                        celda.Merge = true;
                                    }
                                    GenerarBordesCelda(celda);

                                    //CeldaAlinear(celda, alinear);

                                    celda.Style.Font.Bold = ESTILOS_REPORTE.tabla_datos_header_font_bold;
                                    celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    celda.Style.Fill.BackgroundColor.SetColor(color_head);
                                    celda.Style.Font.UnderLine = ESTILOS_REPORTE.tabla_datos_header_subrayado;
                                    //celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    celda.Style.ShrinkToFit = false;
                                }
                                comienzoy++;
                            }
                        }
                        else
                        {
                            if (multiples_tablas)
                            {
                                int col = ESTILOS_REPORTE.tabla_datos_columna_inicio;
                                var definicioncolumnas_lista_PR = JsonConvert.DeserializeObject<List<definicioncolumnas>>(tabladefinicioncolumnas.ToString());
                                int colspan = ESTILOS_REPORTE.tabla_datos_cell_colspan;
                                foreach (var def_col in definicioncolumnas_lista_PR)
                                {
                                    colspan = def_col.colspan;

                                    celda = worksheet.Cells[comienzoy, col, comienzoy, col + (colspan - 1)];
                                    celda.Value = def_col.nombre;
                                    if (colspan > 1)
                                    {
                                        celda.Merge = true;
                                    }
                                    GenerarBordesCelda(celda);
                                    celda.Style.Font.Bold = ESTILOS_REPORTE.tabla_datos_header_font_bold;
                                    celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    celda.Style.Fill.BackgroundColor.SetColor(color_head);
                                    celda.Style.Font.UnderLine = ESTILOS_REPORTE.tabla_datos_header_subrayado;
                                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    celda.Style.ShrinkToFit = false;
                                    //col++;
                                    col = col + colspan;
                                }
                            }
                            else
                            {
                                var primera_fila = (JObject)tabladatos.First();
                                var columnas = primera_fila.Properties();
                                comienzoy = GENERAR_HEADERS_TABLA_DATOS(ESTILOS_REPORTE, worksheet, tabladatos, tabladefinicioncolumnas, comienzoy, invisibles);
                                comienzox = 1;

                            }

                        }
                    }
                    ////fin nuevo header
                    int COMIENZOTABLADATOSY = comienzoy;

                    int finfilasdatos = tabladatos.Count() + COMIENZOTABLADATOSY;
                    var primerafila = tabladatos[0];

                    int iii = 0;
                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();
                    String nombrecolumna = string.Empty;
                    definicioncolumnas columna_objeto;

                    var TipoCelda = new Dictionary<String, Func<JToken, object>>() {
                                    { "DECIMAL", x =>(decimal)x },
                                    { "FLOAT",   x =>(decimal)x },
                                    { "STRING",  x =>  (string)x },
                                    { "DATE",    x =>  (DateTime)x },
                                    { "INTEGER", x =>(Int64)x},
                                    { "BOOLEAN", x =>(bool)x}
                                };
                    string color_odd = "#D9E1F2";
                    string color_even = "#FFFFFF";
                    var color1 = get_color(color_odd);
                    var color2 = get_color(color_even);

                    if (multiples_tablas)
                    {
                        int COMIENZOTABLADATOSFILA_Y = comienzoy;
                        var DATOS = (JArray)tablaprimera;
                        var tabla_fila = DATOS;
                        ///fin headers tabla

                        foreach (var TABLA_objeto in tabla_fila)
                        {
                            var nomostrar = TABLA_objeto["nomostrar"];
                            var definicioncolumnas = TABLA_objeto["definicioncolumnas"];

                            var definicioncolumnas_lista = JsonConvert.DeserializeObject<List<definicioncolumnas>>(definicioncolumnas.ToString());

                            var cabezas = TABLA_objeto["cabeceras"];
                            var cabecerasnuevas = TABLA_objeto["cabecerasnuevas"];
                            var listado = TABLA_objeto["datos"];
                            var porcentajes_PDF = TABLA_objeto["porcentajes"];
                            var estilos = TABLA_objeto["estilos_reporte"];
                            EstilosReporte estilos_excel = new EstilosReporte();
                            if (estilos != null)
                            {
                                estilos_excel = JsonConvert.DeserializeObject<EstilosReporte>(estilos.ToString());
                            }
                            var multiplestablas2 = TABLA_objeto["multiplestablas"];
                            var mostrartablaheaders2 = TABLA_objeto["mostrar_headers_tabla"];
                            bool mostrar_headers_tabla22 = true;
                            if (mostrartablaheaders2 != null)
                            {
                                mostrar_headers_tabla22 = (bool)mostrartablaheaders2;
                            }
                            bool multiples_tablas_2 = false;
                            if (multiplestablas2 != null)
                            {
                                multiples_tablas_2 = (bool)multiplestablas2;
                            }

                            ///cabezas tabla fila
                            comienzoy++;

                            int columna_inicio = estilos_excel.cabeceras_tabla_columna_inicio;
                            if (cabezas == null)
                            {
                                comienzoy = GENERAR_CABECERA_TABLA(estilos_excel, worksheet, cabecerasnuevas, columna_inicio, comienzoy);
                            }
                            else
                            {
                                comienzoy = GENERAR_CABECERA_TABLA(estilos_excel, worksheet, cabezas, columna_inicio, comienzoy);
                            }
                            comienzoy++;
                            comienzox = 1;
                            /////fni cabezas ta


                            if (multiples_tablas_2)
                            {///nuevo nivel tablas
                                int cantidad_tablas = ((JArray)listado).Count;
                                int contador = 0;
                                int COMIENZOY_TABLADATOS2 = comienzoy;
                                string formula_suma_monto = "";
                                foreach (var fila_nivel2 in listado)///llenar datos tabla
                                {
                                    var nomostrar2 = fila_nivel2["nomostrar"];
                                    var definicioncolumnas2 = fila_nivel2["definicioncolumnas"];
                                    var cabezas2 = fila_nivel2["cabeceras"];
                                    var listado2 = fila_nivel2["datos"];
                                    var porcentajes_PDF2 = fila_nivel2["porcentajes"];
                                    var estilos2 = fila_nivel2["estilos_reporte"];
                                    var mostrartablaheaders_nivel2 = fila_nivel2["mostrar_headers_tabla"];

                                    EstilosReporte estilos_excel2 = new EstilosReporte();
                                    if (estilos2 != null)
                                    {
                                        estilos_excel2 = JsonConvert.DeserializeObject<EstilosReporte>(estilos2.ToString());
                                    }
                                    bool mostrar_headers_tabla2 = true;
                                    if (mostrartablaheaders_nivel2 != null)
                                    {
                                        mostrar_headers_tabla2 = (bool)mostrartablaheaders_nivel2;
                                    }
                                    ///cabezas tabla fila2
                                    //comienzoy++;
                                    int columna_inicio2 = estilos_excel2.cabeceras_tabla_columna_inicio;////cabeceras tabla

                                    var primer_fila2 = (JObject)listado2.First();
                                    var propiedades_fila2 = primer_fila2.Properties();
                                    var Columnas_tabla2 = get_columnas_propiedades(primer_fila2, definicioncolumnas2.ToString());

                                    comienzoy = GENERAR_CABECERA_TABLA(estilos_excel2, worksheet, cabezas2, columna_inicio2, comienzoy);
                                    comienzox = 1;
                                    comienzoy++;
                                    /////fni cabezas tabla fila2

                                    if (mostrar_headers_tabla2)
                                    {
                                        comienzoy = GENERAR_HEADERS_TABLA_DATOS(estilos_excel2, worksheet, listado2, definicioncolumnas2, comienzoy);///headers tabla datos
                                    }

                                    COMIENZOY_TABLADATOS2 = comienzoy;
                                    comienzoy = GENERAR_TABLA_DATOS(estilos_excel2, worksheet, listado2, definicioncolumnas2, comienzoy);
                                    FORMATEAR_COLUMNAS(estilos_excel2, worksheet, Columnas_tabla2, COMIENZOY_TABLADATOS2, comienzoy);

                                    comienzoy = GENERAR_FOOTER(estilos_excel2, worksheet, Columnas_tabla2, COMIENZOY_TABLADATOS2, comienzoy, definicioncolumnas_lista);
                                    formula_suma_monto = (comienzoy - 1).ToString() + "+";

                                    contador++;

                                }
                                int collll = estilos_excel.tabla_footer_columna_inicio;
                                foreach (var def_col in definicioncolumnas_lista)
                                {
                                    celda = worksheet.Cells[comienzoy, collll];
                                    celda.Style.Font.Bold = def_col.footer_font_bold;
                                    CeldaAlinear(celda, def_col.footer_alinear);
                                    celda.Style.Font.Color.SetColor(get_color(def_col.footer_font_color));
                                    if ((def_col.tipo).ToUpper() == "DECIMAL")
                                    {
                                        def_col.formato = formato_decimal;
                                    }
                                    celda.Style.Numberformat.Format = def_col.formato;
                                    if (def_col.footer.Count > 0)
                                    {
                                        if (def_col.footer[0] == "sumar")
                                        {
                                            var formula = def_col.valor_suma2;
                                            celda.Formula = "SUM(" + formula + ")";
                                        }
                                        else
                                        {
                                            var valor = def_col.footer[0];
                                            celda.Value = valor;
                                        }
                                    }
                                    else
                                    {
                                        var valor = "";
                                        celda.Value = valor;
                                    }
                                    collll++;
                                }
                                comienzoy++;
                                //comienzoy = generar_footer(estilos_excel2, worksheet, Columnas_tabla2, COMIENZOY_TABLADATOS2, comienzoy, definicioncolumnas_lista);

                            }
                            else
                            {
                                //////tabla datos fila
                                var primer_fila = (JObject)listado.First();
                                var propiedades_fila = primer_fila.Properties();
                                var Columnas_tabla = get_columnas_propiedades(primer_fila, definicioncolumnas.ToString(), null, estilos_excel);
                                if (mostrar_headers_tabla22)
                                {
                                    comienzoy = GENERAR_HEADERS_TABLA_DATOS(estilos_excel, worksheet, listado, definicioncolumnas, comienzoy);///headers tabla datos
                                }

                                int COMIENZOY_TABLADATOS = comienzoy;
                                comienzoy = GENERAR_TABLA_DATOS(estilos_excel, worksheet, listado, definicioncolumnas, comienzoy);
                                FORMATEAR_COLUMNAS(estilos_excel, worksheet, Columnas_tabla, COMIENZOY_TABLADATOS, comienzoy);

                                comienzoy = GENERAR_FOOTER(estilos_excel, worksheet, Columnas_tabla, COMIENZOY_TABLADATOS, comienzoy);

                                //var color_fila_tabla1 = get_color(estilos_excel.tabla_datos_body_background_color_even);
                                //var color_fila_tabla2 = get_color(estilos_excel.tabla_datos_body_background_color_row);

                            }

                        }//fin foreach tabla_fila
                    }//FIN IF MULTIPLESTABLAS
                    else
                    {  ////EXCEL SIN SUBTABLAS
                        int colspan = 1;
                        int COMIENZOY_TABLADATOS = comienzoy;
                        foreach (var fila in tabladatos)///llenar datos tabla
                        {
                            comienzox = ESTILOS_REPORTE.tabla_datos_columna_inicio;
                            colspan = ESTILOS_REPORTE.tabla_datos_cell_colspan;
                            foreach (var prop in propiedades)
                            {
                                nombrecolumna = prop.Name;
                                if (invisibles != null)
                                {
                                    bool ocultar = ocultar_columna(invi, nombrecolumna);
                                    if (ocultar) { continue; }

                                }

                                columna_objeto = Columnas.Where(x => (x.nombre == nombrecolumna.Replace("_", " ") || x.nombre == nombrecolumna)).FirstOrDefault();
                                var tipo_col = columna_objeto.tipo.ToUpper();
                                colspan = columna_objeto.colspan;
                                celda = worksheet.Cells[comienzoy, comienzox, comienzoy, comienzox + (colspan - 1)];
                                if (colspan > 1)
                                {
                                    celda.Merge = true;
                                }
                                var valor_celda = TipoCelda[tipo_col](fila[nombrecolumna]);
                                celda.Value = valor_celda;

                                //celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                //celda.Style.Fill.BackgroundColor.SetColor(comienzoy % 2 == 0
                                //   ? color1
                                //   : color2);
                                comienzox = comienzox + colspan;
                                //comienzox++;
                            }///FIN FOR EACH PROPIEDADES =>  columnas 
                            comienzoy++;
                            iii++;
                        }////fin datos tabla   FILAS
                        string tipo, formato; bool sumar;
                        string alinear = string.Empty;
                        int columna_actual = 1;
                        colspan = 1;
                        foreach (var col in Columnas)
                        {
                            tipo = (col.tipo).ToUpper();
                            formato = col.formato;
                            sumar = col.sumar;
                            alinear = col.alinear;
                            colspan = col.colspan;
                            //celda = worksheet.Cells[finfilasdatos, comienzox];
                            celda = worksheet.Cells[COMIENZOTABLADATOSY, columna_actual, finfilasdatos - 1, columna_actual + colspan - 1];
                            CeldaAlinear(celda, alinear);
                            GenerarBordesCelda(celda);

                            if (tipo == "DECIMAL" || tipo == "FLOAT")
                            {
                                celda.Style.Numberformat.Format = formato;
                                var cf = worksheet.ConditionalFormatting.AddLessThan(celda);
                                cf.Formula = "0";
                                cf.Style.Font.Color.Color = Color.Red;

                            }
                            else if (tipo == "INTEGER")
                            {
                                worksheet.ConditionalFormatting.AddLessThan(celda).Formula = "0";
                                celda.Style.Numberformat.Format = formato;
                            }
                            else if (tipo == "DATE")
                            {
                                celda.Style.Numberformat.Format = formato;

                            }

                            if (Convert.ToBoolean(sumar) == true)
                            {
                                celda = worksheet.Cells[finfilasdatos, columna_actual];
                                celda.Formula = "SUM(" + worksheet.Cells[COMIENZOTABLADATOSY, columna_actual].Address + ":" + worksheet.Cells[finfilasdatos - 1, columna_actual].Address + ")";
                                celda.Style.Font.Bold = true;
                                celda.Style.Numberformat.Format = formato;

                            }
                            columna_actual++;
                            columna_actual = columna_actual + colspan - 1;
                        }
                        //FORMATEAR_COLUMNAS(ESTILOS_REPORTE, worksheet, Columnas, COMIENZOY_TABLADATOS, comienzoy);
                        comienzoy = GENERAR_FOOTER(ESTILOS_REPORTE, worksheet, Columnas, COMIENZOY_TABLADATOS, comienzoy);


                    }

                    //}
                    /// FIN NUEVOOOOOOOOOOOO

                    int columna = 1;
                    foreach (var prop in propiedades)
                    {
                        worksheet.Column(columna).AutoFit();
                        columna++;
                    }
                    //worksheet.InsertColumn(1, 1);
                    worksheet.View.FreezePanes(COMIENZOTABLADATOSY, 1);
                }///fin foeach TABLAS_ARRAY
                package.Workbook.Calculate();
                result = package.GetAsByteArray();
            }///FIN EXCELPACKAGE

            return result;
        }




        private static ExcelWorksheet CREARHOJA(ExcelPackage p, string sheetName)
        {
            //p.Workbook.Worksheets.Add(sheetName);
            //ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ExcelWorksheet ws = p.Workbook.Worksheets.Add(sheetName);
            ws.Name = sheetName; //Setting Sheet's name
            ws.Cells.Style.Font.Size = 9; //Default font size for whole sheet
            //ws.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
            //ws.Cells.Style.Font.Name = "Courier New";
            ws.Cells.Style.Font.Name = "Calibri";
            return ws;
        }
        public static void GenerarBordesCelda(ExcelRange celda)
        {
            celda.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            celda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            celda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
        public static bool ocultar_columna(List<string> invisibles, string columna_name)
        {
            bool invisiblee = false;
            foreach (var pr in invisibles)
            {
                if (columna_name == pr.Replace("_", " "))
                {
                    invisiblee = true;
                }
            }
            return invisiblee;
        }
        public static List<definicioncolumnas> get_columnas_propiedades(JObject objetofila, string definicioncolumnas, List<string> invisibles = null, EstilosReporte estilos = null)
        {
            var propiedades = objetofila.Properties();
            List<definicioncolumnas> columnas = new List<definicioncolumnas>();
            foreach (var columna in propiedades)
            {
                var columna_name = columna.Name.Replace("_", " ");

                if (invisibles != null)
                {
                    bool ocultar = ocultar_columna(invisibles, columna_name);
                    if (ocultar) { continue; }
                }

                string tipo = (columna.Value.Type).ToString();
                var COL = new definicioncolumnas();
                COL.nombre = columna_name;
                COL.tipo = tipo;

                if (estilos != null)
                {
                    COL.alinear = estilos.tabla_datos_body_alinear;
                    COL.header_alinear = estilos.tabla_datos_header_alinear;
                    COL.tipo = estilos.tabla_datos_body_tipo;
                    COL.formato = estilos.tabla_datos_body_formato;
                    if (COL.formato == null || COL.formato == "")
                    {
                        tipo = COL.tipo;
                        if (tipo.ToUpper() == "DECIMAL")
                        {
                            COL.formato = "#,##0.00";
                        }
                        else if (tipo.ToUpper() == "FLOAT") { COL.formato = "#,##0.00"; }
                        else if (tipo.ToUpper() == "INTEGER") { COL.formato = "###0"; }
                        else { COL.formato = ""; }
                    }
                }

                if (definicioncolumnas != null)
                {
                    foreach (var tipocol in JArray.Parse(definicioncolumnas.ToString()))
                    {
                        if (((string)tipocol["nombre"]).ToUpper() == columna_name.ToUpper())
                        {
                            COL = JsonConvert.DeserializeObject<definicioncolumnas>(tipocol.ToString());
                            String formato = COL.formato;
                            if (formato == null || formato == "")
                            {
                                tipo = COL.tipo;
                                if (tipo.ToUpper() == "DECIMAL")
                                {
                                    formato = "#,##0.00";
                                }
                                else if (tipo.ToUpper() == "FLOAT") { formato = "#,##0.00"; }
                                else if (tipo.ToUpper() == "INTEGER") { formato = "###0"; }
                                else { formato = ""; }
                                COL.formato = formato;
                            }
                            break;
                        }

                    }
                }
                columnas.Add(COL);
            }
            //var columnas_2 = JsonConvert.DeserializeObject<List<definicioncolumnas>>(definicioncolumnas.ToString());
            //foreach (var item in columnas_2)
            //{
            //    string formato;
            //    var tipo = item.tipo;
            //    if (tipo.ToUpper() == "DECIMAL")
            //    {
            //        formato = "#,##0.00";
            //    }
            //    else if (tipo.ToUpper() == "FLOAT") { formato = "#,##0.00"; }
            //    else if (tipo.ToUpper() == "INTEGER") { formato = "###0"; }
            //    else { formato = ""; }
            //    item.formato = formato;
            //}
            return columnas;
        }
        public static int GENERAR_HEADERS_PAGINA(EstilosReporte estilos, ExcelWorksheet worksheet, JToken cabezas, int comienzox, int comienzoy)
        {
            int columna_cabecera = 1;
            ExcelRange celda;
            int cabcont = 0;
            foreach (var cab in cabezas)
            {
                var nombre_var = (string)cab["nombre"];
                var valor_var = (string)cab["valor"];
                if (nombre_var != "Página")
                {
                    if ((cabcont + 1) % 2 != 0)
                    {//izq
                        columna_cabecera = 1;
                        celda = worksheet.Cells[comienzoy, columna_cabecera];
                        celda.Value = nombre_var;
                        celda.Style.Font.Bold = true;
                        columna_cabecera++;
                        celda = worksheet.Cells[comienzoy, columna_cabecera, comienzoy, columna_cabecera + 3];
                        celda.Value = valor_var;
                        celda.Merge = true;
                        celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment Left
                        celda.Style.Font.Size = 11;
                        columna_cabecera = columna_cabecera + 3;
                        columna_cabecera++;
                    }
                    else
                    {///der
                        celda = worksheet.Cells[comienzoy, columna_cabecera];
                        celda.Value = nombre_var;
                        celda.Style.Font.Bold = true;
                        columna_cabecera++;
                        celda = worksheet.Cells[comienzoy, columna_cabecera, comienzoy, columna_cabecera + 2];
                        celda.Value = valor_var;
                        celda.Merge = true;
                        celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Alignment Left
                        celda.Style.Font.Size = 11;
                        comienzoy++;
                    }
                }
                cabcont++;
            }
            comienzoy++;
            return comienzoy;
        }
        public static int GENERAR_TITULO_REPORTE(EstilosReporte estilos, ExcelWorksheet worksheet, string tituloreporte, int comienzox, int comienzoy)
        {
            ExcelRange celda;
            celda = worksheet.Cells[comienzoy, 1, comienzoy, 8];
            celda.Value = (string)tituloreporte;
            celda.Merge = true;
            celda.Style.Font.Bold = true;
            celda.Style.Font.Size = 16;
            celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            comienzoy++;
            return comienzoy;
        }
        public static int GENERAR_SUBTITULO_REPORTE(EstilosReporte estilos, ExcelWorksheet worksheet, string subtituloreporte, int comienzox, int comienzoy)
        {
            ExcelRange celda;
            celda = worksheet.Cells[comienzoy, 1, comienzoy, 8];
            celda.Value = (string)subtituloreporte;
            celda.Merge = true;
            celda.Style.Font.Bold = true;
            celda.Style.Font.Size = 15;
            celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            comienzoy++;
            return comienzoy;
        }
        public static int GENERAR_CABECERA_REPORTE(EstilosReporte estilos, ExcelWorksheet worksheet, JToken cabeceras_reporte, int comienzox, int comienzoy)
        {
            int cabcont = 0;
            int columna_cabecera = 1;
            ExcelRange celda;
            foreach (var cab in cabeceras_reporte)
            {
                if ((cabcont + 1) % 2 != 0)
                {//izq
                    columna_cabecera = 1;
                    celda = worksheet.Cells[comienzoy, columna_cabecera];
                    celda.Value = (string)cab["nombre"];
                    celda.Style.Font.Bold = estilos.cabeceras_tabla_nombre_bold;
                    celda.Style.Font.Color.SetColor(get_color(estilos.cabeceras_tabla_nombre_color));
                    CeldaAlinear(celda, estilos.cabeceras_tabla_col_nombre_alinear);

                    columna_cabecera++;
                    celda = worksheet.Cells[comienzoy, columna_cabecera, comienzoy, columna_cabecera + 3];
                    celda.Value = (string)cab["valor"];
                    celda.Merge = true;
                    celda.Style.Font.Color.SetColor(get_color(estilos.cabeceras_tabla_valor_color));
                    celda.Style.Font.Bold = estilos.cabeceras_tabla_valor_bold;
                    CeldaAlinear(celda, estilos.cabeceras_tabla_col_valor_alinear);
                    celda.Style.Font.Size = 11;
                    columna_cabecera = columna_cabecera + 3;
                    columna_cabecera++;
                }
                else
                {///der
                    celda = worksheet.Cells[comienzoy, columna_cabecera];
                    celda.Value = (string)cab["nombre"];
                    celda.Style.Font.Bold = estilos.cabeceras_tabla_nombre_bold;
                    celda.Style.Font.Color.SetColor(get_color(estilos.cabeceras_tabla_nombre_color));
                    CeldaAlinear(celda, estilos.cabeceras_tabla_col_nombre_alinear);

                    columna_cabecera++;
                    celda = worksheet.Cells[comienzoy, columna_cabecera, comienzoy, columna_cabecera + 2];
                    celda.Value = (string)cab["valor"];
                    CeldaAlinear(celda, estilos.cabeceras_tabla_col_valor_alinear);
                    celda.Merge = true;
                    celda.Style.Font.Color.SetColor(get_color(estilos.cabeceras_tabla_valor_color));
                    celda.Style.Font.Bold = estilos.cabeceras_tabla_valor_bold;
                    celda.Style.Font.Size = 11;
                    comienzoy++;
                }
                cabcont++;
                //comienzoy++;
            }
            return comienzoy;

        }
        public static int GENERAR_CABECERA_TABLA(EstilosReporte estilos, ExcelWorksheet worksheet, JToken cabezas, int comienzox, int comienzoy)
        {
            int columna_inicio2 = estilos.cabeceras_tabla_columna_inicio;
            foreach (var cab in cabezas)
            {
                ExcelRange celda;
                celda = worksheet.Cells[comienzoy, columna_inicio2, comienzoy, columna_inicio2];
                celda.Value = (string)cab["nombre"];
                CeldaAlinear(celda, estilos.cabeceras_tabla_col_nombre_alinear);
                celda.Style.Font.Color.SetColor(get_color(estilos.cabeceras_tabla_nombre_color));
                celda.Style.Font.Bold = estilos.cabeceras_tabla_nombre_bold;
                columna_inicio2++;
                celda = worksheet.Cells[comienzoy, columna_inicio2, comienzoy, columna_inicio2 + 4];
                celda.Value = (string)cab["valor"];
                celda.Style.Font.Color.SetColor(get_color(estilos.cabeceras_tabla_valor_color));
                celda.Merge = true;
                CeldaAlinear(celda, estilos.cabeceras_tabla_col_valor_alinear);
                celda.Style.Font.Bold = estilos.cabeceras_tabla_valor_bold;
                celda.Style.Font.Size = 11;// estilos.cabeceras_tabla_font_size;
                comienzoy++;
            }
            return comienzoy;

        }
        public static int GENERAR_HEADERS_TABLA_DATOS(EstilosReporte estilos, ExcelWorksheet worksheet, JToken listado, JToken definicioncolumnas, int comienzoy, JToken invisibles = null)
        {


            var primer_fila = (JObject)listado.First();
            var propiedades_fila = primer_fila.Properties();
            string def_col_string = definicioncolumnas != null ? definicioncolumnas.ToString() : null;

            var Columnas_tabla = get_columnas_propiedades(primer_fila, def_col_string);

            var columnas = primer_fila.Properties();
            int comienzox = estilos.tabla_datos_columna_inicio;
            ExcelRange celda;
            ///headers tabla
            //int colspan = Columnas_tabla.colsp;

            var invi = new List<string>();
            if (invisibles != null && invisibles.ToString() != "")
            {
                var invisiblesobj = JArray.Parse(invisibles.ToString());
                var pror = columnas.ToList();
                foreach (var invisislb in invisiblesobj)
                {
                    invi.Add(pror[Convert.ToInt32(invisislb)].Name.ToString());
                }
            }


            foreach (var col in columnas)
            {
                var a = col.Name;
                string nombre_col = a.ToString();

                if (invisibles != null)
                {
                    bool ocultar = ocultar_columna(invi, nombre_col);
                    if (ocultar) { continue; }

                }

                var columna_objeto = Columnas_tabla.Where(x => (x.nombre == nombre_col.Replace("_", " ") || x.nombre == nombre_col)).FirstOrDefault();
                int colspan = columna_objeto.colspan;

                celda = worksheet.Cells[comienzoy, comienzox, comienzoy, comienzox + (colspan - 1)];
                if (colspan > 1)
                {
                    celda.Merge = true;
                }
                celda.Value = nombre_col;
                GenerarBordesCelda(celda);
                celda.Style.Font.Bold = estilos.tabla_datos_header_font_bold;
                celda.Style.Font.Color.SetColor(get_color(estilos.tabla_datos_header_font_color));
                celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                celda.Style.Font.UnderLine = estilos.tabla_datos_header_subrayado;
                celda.Style.Fill.BackgroundColor.SetColor(get_color(estilos.tabla_datos_header_background_color));
                celda.Style.ShrinkToFit = false;
                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                comienzox = comienzox + colspan;
            }
            comienzoy++;
            return comienzoy;

        }

        public static int GENERAR_TABLA_DATOS(EstilosReporte estilos, ExcelWorksheet worksheet, JToken listado, JToken definicioncolumnas, int comienzoy)
        {
            var primer_fila = (JObject)listado.First();
            var propiedades_fila = primer_fila.Properties();
            var Columnas_tabla = get_columnas_propiedades(primer_fila, definicioncolumnas.ToString(), null, estilos);
            definicioncolumnas columna_objeto;
            ExcelRange celda;
            var color_fila_tabla1 = get_color(estilos.tabla_datos_body_background_color_even);
            var color_fila_tabla2 = get_color(estilos.tabla_datos_body_background_color_row);
            var TipoCelda = new Dictionary<String, Func<JToken, object>>() {
                                    { "DECIMAL", x =>(decimal)x },
                                    { "FLOAT",   x =>(decimal)x },
                                    { "STRING",  x =>  (string)x },
                                    { "DATE",    x =>  (string)x },
                                    { "INTEGER", x =>(Int64)x},
                                    { "BOOLEAN", x =>(bool)x}
                                };

            foreach (var fila in listado)///llenar datos tabla
            {
                int comienzox = estilos.tabla_datos_columna_inicio;
                int colspan = estilos.tabla_datos_cell_colspan;
                foreach (var prop in propiedades_fila)
                {
                    string nombrecolumna = prop.Name;
                    columna_objeto = Columnas_tabla.Where(x => x.nombre == nombrecolumna.Replace("_", " ")).FirstOrDefault();
                    var tipo_col = columna_objeto.tipo.ToUpper();
                    colspan = columna_objeto.colspan;
                    celda = worksheet.Cells[comienzoy, comienzox, comienzoy, comienzox + (colspan - 1)];
                    if (colspan > 1)
                    {
                        celda.Merge = true;
                    }
                    var valor_celda = TipoCelda[tipo_col](fila[nombrecolumna]);
                    celda.Value = valor_celda;
                    GenerarBordesCelda(celda);

                    celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    celda.Style.Fill.BackgroundColor.SetColor(comienzoy % 2 == 0
                       ? color_fila_tabla1
                       : color_fila_tabla2);
                    //comienzox++;
                    comienzox = comienzox + colspan;
                }///FIN FOR EACH PROPIEDADES =>  columnas 
                comienzoy++;
            }////fin datos tabla   FILAS
            return comienzoy;

        }

        public static int FORMATEAR_COLUMNAS(EstilosReporte estilos, ExcelWorksheet worksheet, List<definicioncolumnas> lista_definicioncolumnas, int COMIENZOY_TABLADATOS, int comienzoy)
        {
            ExcelRange celda;
            string tipo, formato;
            bool sumar;
            string alinear = string.Empty;
            int columna_actual = estilos.tabla_datos_columna_inicio;
            foreach (var col in lista_definicioncolumnas)
            {
                int colspan = estilos.tabla_datos_cell_colspan;
                colspan = col.colspan;

                tipo = col.tipo;
                tipo = tipo.ToUpper();
                formato = col.formato;
                sumar = col.sumar;
                alinear = col.alinear;
                celda = worksheet.Cells[COMIENZOY_TABLADATOS, columna_actual, comienzoy - 1, columna_actual + (colspan - 1)];
                if (colspan > 1)
                {
                    //celda.Merge = true;
                }
                CeldaAlinear(celda, alinear);
                //GenerarBordesCelda(celda);
                if (tipo == "DECIMAL" || tipo == "FLOAT")
                {
                    celda.Style.Numberformat.Format = formato;
                    worksheet.ConditionalFormatting.AddLessThan(celda).Formula = "0";
                }
                else if (tipo == "INTEGER")
                {
                    worksheet.ConditionalFormatting.AddLessThan(celda).Formula = "0";
                    celda.Style.Numberformat.Format = formato;
                }
                columna_actual = columna_actual + colspan;
            }
            return comienzoy;
        }
        public static int GENERAR_FOOTER(EstilosReporte estilos, ExcelWorksheet worksheet, List<definicioncolumnas> lista_definicioncolumnas, int COMIENZOY_TABLADATOS, int comienzoy, List<definicioncolumnas> lista_definicioncolumnas_padre = null)
        {
            ExcelRange celda;
            ////footer
            int max_cant_filas_footer = get_footer_maxfilas(lista_definicioncolumnas);
            int COMIENZOY_TABLAFOOTER = comienzoy;
            for (int i = 0; i < max_cant_filas_footer; i++)
            {
                int co_i = estilos.tabla_datos_columna_inicio; ;
                foreach (var col in lista_definicioncolumnas)
                {
                    string nombre_columna2 = col.nombre;
                    var columna_objeto2 = lista_definicioncolumnas.Where(x => x.nombre == nombre_columna2.Replace("_", " ")).FirstOrDefault();
                    int colspan = columna_objeto2.colspan;
                    string footer = "";
                    if (col.footer.Count > 0)
                    {
                        celda = worksheet.Cells[comienzoy, co_i, comienzoy, co_i + (colspan - 1)];
                        if (colspan > 1)
                        {
                            celda.Merge = true;
                        }
                        footer = col.footer[i];
                        if (footer == "sumar")
                        {
                            //footer = col.valor_suma.ToString("#0.00", CultureInfo.InvariantCulture);
                            celda.Formula = "SUM(" + worksheet.Cells[COMIENZOY_TABLADATOS, co_i].Address + ":" + worksheet.Cells[comienzoy - 1, co_i].Address + ")";
                            celda.Style.Font.Bold = columna_objeto2.footer_font_bold;
                            celda.Style.Numberformat.Format = columna_objeto2.formato;
                            celda.Style.Font.Color.SetColor(get_color(columna_objeto2.footer_font_color));
                            if (lista_definicioncolumnas_padre != null)
                            {
                                var fila = lista_definicioncolumnas_padre.Where(x => x.nombre == nombre_columna2).FirstOrDefault();
                                fila.valor_suma2 = fila.valor_suma2 + "+" + celda.Address;
                            }
                            CeldaAlinear(celda, columna_objeto2.footer_alinear);
                            worksheet.ConditionalFormatting.AddLessThan(celda).Formula = "0";
                        }
                        else
                        {
                            celda.Value = footer;
                            celda.Style.Font.Bold = columna_objeto2.footer_font_bold;
                            celda.Style.Font.Color.SetColor(get_color(columna_objeto2.footer_font_color));
                            CeldaAlinear(celda, columna_objeto2.footer_alinear);

                        }
                    }
                    co_i = co_i + colspan;
                }
                comienzoy++;
            }/////end footer
            return comienzoy;

        }
        public static int get_footer_maxfilas(List<definicioncolumnas> Columnas_tabla)
        {
            int max_cant_filas_footer = 0;
            foreach (var col in Columnas_tabla)
            {
                var footer = col.footer;
                int cant_filas = footer.Count;
                if (cant_filas > max_cant_filas_footer)
                {
                    max_cant_filas_footer = cant_filas;
                }
            }
            return max_cant_filas_footer;
        }
        public static void CeldaAlinear(ExcelRange celda, string align)
        {
            switch (align.ToUpper())
            {
                case "LEFT":
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    break;
                case "CENTER":
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    break;
                case "RIGHT":
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    break;
                default:
                    celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    break;
            }
        }
        public static Color get_color(string color_string)
        {
            return System.Drawing.ColorTranslator.FromHtml(color_string);
        }

    }
}

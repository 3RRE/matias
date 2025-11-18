
///FUNCIONES PARA GENERAR EXCEL PDF IMPRIMIR DESDE  DATATABLES

///funcionbotonesfinal de S3K.CorpPj.Seguridad


$.expr[':'].icontains = $.expr.createPseudo(function (text) {
    text = text.toLowerCase();
    return function (el) {
        return ~$.text(el).toLowerCase().indexOf(text);
    };
});

///sacarcolumnasporcentajes_pdf() =>  devuelve porcentajes de columna para PDF y  columnas invisibles que no deben mostrarse en excel o pdf ,  todo lo calcula utilizando el datatable en la vista
function sacarcolumnasporcentajes_pdf(tablaserverobjeto, ocultar) {
    var valores = {};
    var porcentajes = [];
    var visibles = []; var invisibles = []; var tablaobj = tablaserverobjeto;
    var tabla = tablaserverobjeto.context[0].nTable.id;
    $(tablaserverobjeto.columns().context[0].aoColumns).each(function (i, e) {
        tablaserverobjeto.columns().context[0].aoColumns[i].bVisible ? visibles.push(i) : invisibles.push(i);
        if (tablaserverobjeto.columns().context[0].aoColumns[i].sTitle === ocultar) {
            a = 0;
            $.each(tablaserverobjeto.rows(0).data()[0]
                , function (nombre, valor) {
                    if (nombre === tablaserverobjeto.columns().context[0].aoColumns[i].data) {
                        invisibles.push(a);
                    }
                    a++;
                }
            );
        }

    });
    widthaquitar = 0;
    cantidadcolumnas = $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').length;
    widthtotal = $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0)').outerWidth();
    sumawidthocultar = 0;
    selectparaocultar = "";

    $(ocultar).each(function (i, e) {
        sumawidthocultar = sumawidthocultar + $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th:icontains("' + e + '")').outerWidth();
        selectparaocultar = selectparaocultar + ":not(:icontains(" + e + "))";
    });

    widthfinal = widthtotal - sumawidthocultar;
    $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th' + selectparaocultar)
        .each(function (i, e) {
            porcentajes.push({
                wid: $(this).outerWidth(),
                por: $(this).outerWidth() * 100 / widthfinal,
                nombre: $(this).text()
            });

        });
    valores.porcentajes = porcentajes;
    valores.visibles = visibles;
    valores.invisibles = invisibles;
    return valores;
}

function generar_tabla_headers(aoHeaders) {
    var FILASHEADER_array = [];
    headersarray = aoHeaders;//objeto_datatabla.context[0].aoHeader;
    i = 1;
    $(headersarray).each(function (headers_i, headers_e) {
        TRHEADER_objeto = {};
        TRHEADER_objeto.nrofila = i;///primer tr
        ths_array = [];
        var filaheaders_array = headers_e;
        $(filaheaders_array).each(function (fh_i, fh_e) {
            if (fh_e.cell.textContent !== "") {////si texto de th de columna esta vacio
                th_objeto = {};
                var th = $(fh_e.cell.outerHTML);
                var alinear_header = th.css("text-align");
                var celda = fh_e.cell.attributes;
                objetocelda = {};
                objetocelda.text = fh_e.cell.textContent;
                //if (typeof celda.colspan == "undefined") { colspan_celda = 1; } else { colspan_celda = celda.colspan.value; }
                rowspan_celda = celda.rowspan.value;
                colspan_celda = celda.colspan.value;
                objetocelda.rowspan = rowspan_celda;//celda.rowspan.value;
                objetocelda.colspan = colspan_celda;//celda.colspan.value;
                objetocelda.alinear = alinear_header;//celda.colspan.value;
                th_objeto = objetocelda;
                if (JSON.stringify(ths_array[ths_array.length - 1]) !== JSON.stringify(th_objeto)) {
                    ////solo enviar unicos
                    if ($.inArray((th_objeto.text).toUpperCase(), nomostrar_array) === -1) {

                        ths_array.push(th_objeto);
                    }
                }
            }
        });
        TRHEADER_objeto.ths = ths_array;
        FILASHEADER_array.push(TRHEADER_objeto);
        i++;
    });
    return FILASHEADER_array;

}

////obtiene objeto del array de columnas para cambiar

function get_columnacambios(nombrecolumna, columnac_array) {
    var columnacambios = "";
    $(columnac_array).each(function (i, e) {
        var nombre_c = e.nombre;
        if (nombrecolumna.toUpperCase() === nombre_c.toUpperCase()) {
            columnacambios = e;
        }
    });
    return columnacambios;
}
//////funcion para callback al terminar de generar excel y al aparecer boton de descarga
function xhr_generar_botonexcel(urlaccion, parametros_array, accion = "excel") {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', urlaccion, true);
    xhr.responseType = 'arraybuffer';
    xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    xhr.onprogress = function (e) {
        console.log(e.loaded / e.total * 100);//shows downloaded percentage
    };

    xhr.onload = function () {
        aver = this;
        status = this.status;
        tipo = xhr.getResponseHeader("Content-Type");
        if (status === "200") {
            switch (tipo) {
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/pdf":
                    var filename = "";
                    disposition = xhr.getResponseHeader('Content-Disposition');

                    if (disposition && disposition.indexOf('attachment') !== -1) {
                        var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                        var matches = filenameRegex.exec(disposition);
                        if (matches !== null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                    }
                    var type = xhr.getResponseHeader('Content-Type');

                    blob = new Blob([this.response], { type: type });
                    if (typeof window.navigator.msSaveBlob !== 'undefined') {
                        // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                        window.navigator.msSaveBlob(blob, filename);
                    }
                    else {
                        var URL = window.URL || window.webkitURL;
                        downloadUrl = URL.createObjectURL(blob);

                        if (filename) {
                            // use HTML5 a[download] attribute to specify filename
                            var a = document.createElement("a");
                            // safari doesn't support this yet
                            //if (typeof a.download === 'undefined') {
                            if (accion === 'imprimir') {
                                $.LoadingOverlay("hide");
                                //window.location = downloadUrl;
                                window.open(downloadUrl, '_blank');
                                //    ventana=window.open(downloadUrl, "", "width=800,height=600"); 
                                //    ventana.close();
                                //    ventana.print();
                            }
                            else {
                                $.LoadingOverlay("hide");
                                a.href = downloadUrl;
                                a.download = filename;
                                document.body.appendChild(a);
                                a.click();
                            }
                        }
                        else {
                            window.location = downloadUrl;
                        }
                        setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
                    }
                    break;
                case "text/html; charset=utf-8":
                    decodedString = String.fromCharCode.apply(null, new Uint8Array(xhr.response));
                    console.error(decodedString);
            }
        }
        else if (status === "401") {
            $.LoadingOverlay("hide");
            $.confirm({
                title: 'El Tiempo de sesion a Terminado',
                content: 'Automaticamente se Cerrara el Sistema en 10 Seg. .',
                autoClose: 'confirm|10000',
                confirmButton: 'Salir',
                cancelButton: 'Loguearse',
                confirm: function () {
                    window.location = basePath;
                },
                cancel: function () {


                },
                onClose: function () {
                    $("#small-modal").modal({
                        backdrop: 'static'
                    });
                }
            });


        }
        else if (status === "403") {
            $.LoadingOverlay("hide");
            window.toastr.error("No tiene permisos", "Mensaje Servidor");
        }
        else if (status === "500") {
            toastr.error("Error", "Mensaje Servidor");
            $.LoadingOverlay("hide");
            switch (tipo) {
                case "text/html; charset=utf-8":
                    decodedString = String.fromCharCode.apply(null, new Uint8Array(xhr.response));
                    console.error(decodedString);
                    //console.info(xhr.response);
                    break;
            }
        }
        else {
            aaaa = this;
            $.LoadingOverlay("hide");
            toastr.error("Error", "Mensaje Servidor");
            // Handle Error Here
        }
    };
    xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
    try {
        xhr.send($.param(parametros_array));
    }
    catch (e) {
        $.LoadingOverlay("hide");
        toastr.error('Error Servidor');
    }

}
function render_columnas(objeto_datatable, array_columnas_ocultar, array_columnas_cambiar) {
    arraydatosFinal = [];
    datosdt = objeto_datatable.data();///data de objetodatatable
    datos_array = datosdt.toArray();

    columnasvisibles_array = objeto_datatable.context[0].aoColumns;

    $(datos_array).each(function (datosdt_i, datosdt_valor) {//recorrer filas para  render si es necesario
        var dato_fila = datosdt_valor;
        objetofila = {};
        $(columnasvisibles_array).each(function (col_i, col_valor) {  ////columns en datatable
            if (col_valor.data === null) {
                return;
            }
            var col_datanombre = col_valor["data"];
            ///Columnas no mostrar
            if ($.inArray(col_valor["sTitle"].toUpperCase(), array_columnas_ocultar) !== -1) {//no mostrar columnas enviadas en opciones
                return;
            }
            // fin columnas no mostrar
            var cambioscolumna = get_columnacambios((col_valor["sTitle"]).toUpperCase(), array_columnas_cambiar);//SI SE ENVIA CAMBIO DE RENDER DE COLUMNA
            if (cambioscolumna != "") {
                objetofila[col_valor["sTitle"]] = cambioscolumna.render(dato_fila[col_datanombre]);
            }
            else {
                if (col_valor.mRender != null) {
                    var valorrender = objeto_datatable.cells(datosdt_i, col_i).render("display")[0]; ///datosdt_i => contador datos_array; col_i=> contador columnasviibles_array
                    objetofila[col_valor["sTitle"]] = valorrender;// dato_fila[col_datanombre];
                }
                else {
                    objetofila[col_valor["sTitle"]] = dato_fila[col_datanombre];
                }
            }
        });
        arraydatosFinal.push(objetofila);
    });
    return arraydatosFinal;
}
///funcionbotonesfinal funcionbotonesfinal funcionbotonesfinal
function funcion_botones(obj) { ////obj  => objeto

    ARRAY_TABLAS = [];
    TABLAS = obj;
    $.LoadingOverlay("show");

    cabecerasdefecto = [];
    cabecerasdefecto.push({ nombre: "Empresa", valor: getCookie("Empresa_erp") });
    cabecerasdefecto.push({ nombre: "Sala", valor: getCookie("Sala_erp") });
    cabecerasdefecto.push({ nombre: "Fecha", valor: new moment().format("DD/MM/YYYY HH:mm a") });

    defaults = {
        usardatatable: true,
        cabecerasdefecto: true,
        estilos_tabla: null,
        multiplestablas: false,

        cabeceras: cabecerasdefecto,
        cabecerasnuevas: [],
        datos: "",
        definicioncolumnas: [],
        tituloarchivo: $('.main-text:visible').text().trim().replace(/ /g, "_").replace(" ", "_"),
        titulo_subtitulo: "",
        tituloreporte: $('.main-text:visible').text().trim(),
        nombrehoja: "Hoja",
        aoColumns: [],   ////def columnas objeto datatable
        FILASHEADER_array: [],
        url: basePath + "Funciones/BOTON_REPORTES",     ////////////////*/URL DE METODO PARA BOTONES*/
        accionboton: "excel",  /////*/excel, pdf, imprimir*/
        porcentajespdf: [],
        mostrar_headers_tabla: true
    };
    ////****************************************
    $(TABLAS).each(function (ii, ee) {

        var obj = ee;
        nombrehoja = "Hoja " + (ii + 1);
        if (typeof obj.nombrehoja === "undefined" || obj.nombrehoja === "") {
            obj.nombrehoja = nombrehoja;
        }
        opciones = $.extend({}, defaults, obj);

        if (opciones.cabecerasdefecto === false) {
            if (typeof obj.cabeceras === "undefined") {
                opciones.cabeceras = [];
            }
        }
        if (opciones.usardatatable) {
            var objeto_datatabla = obj.tablaobj;
            if (typeof obj.tablaobj === "undefined") { $.LoadingOverlay("hide"); console.warn("No enviaste objeto datatable => tablaobj ;  enviar tablaobj o  usardatable:false"); return; }
            if (objeto_datatabla.data().length === 0) {
                toastr.error("No hay datos");
                $.LoadingOverlay("hide");
                return;
            }
            columnacambio_array = obj.columna_cambio;

            nomostrar = obj.ocultar;///objeto o string;
            nomostrar_array = [];
            if (typeof nomostrar !== "undefined") {
                if (typeof nomostrar === "string") {
                    nomostrar_array.push(nomostrar.toUpperCase());
                } else {
                    nomostrar_array = nomostrar.map(a => a.toUpperCase());
                }
            }
            arraydatosFinal = [];
            arraydatosFinal = render_columnas(objeto_datatabla, nomostrar_array, columnacambio_array); ///array final de objetos 

            objeto_datatabla.cabecerasnuevas = opciones.cabecerasnuevas;
            opciones.aoColumns = objeto_datatabla.context[0].aoColumns;

            if (opciones.porcentajespdf === null || opciones.porcentajespdf.length === 0 || typeof opciones.porcentajespdf === "undefined") {
                valorestabla = sacarcolumnasporcentajes_pdf(objeto_datatabla, nomostrar_array);//false  no incluir ultima columna accion  , por defecto false
                opciones.porcentajespdf = valorestabla.porcentajes;
            }
            ///FIN CABECERAS REPORTE
            opciones.datos = JSON.stringify(arraydatosFinal);
            ////sacar headers  tr ths  con rowspan colspan para excel  => para generar excel con multiples filas en el header
            opciones.FILASHEADER_array = generar_tabla_headers(objeto_datatabla.context[0].aoHeader);
            objeto_datatabla.definicioncolumnas = opciones.definicioncolumnas;

        }
        else {
            if (typeof opciones.datos !== "object") { console.warn("datos no es objeto"); $.LoadingOverlay("hide"); return; }
            if (opciones.datos.length === 0) { console.warn("no hay datos "); $.LoadingOverlay("hide"); return; }
            console.warn("reporte_botones.js FUNCION_BOTONES : Enviar variable tablaobj => objetodatatable");
            //$.LoadingOverlay("hide");
            if (opciones.porcentajespdf.length === 0) {
                var porcentajes = [];
                var nro_col = Object.keys(opciones.datos[0]).length;
                var porcentaje_div = 100 / nro_col;
                $.each(opciones.datos[0], function (col, val) {
                    porcentajes.push({
                        por: porcentaje_div,
                        nombre: val
                    });
                });
                opciones.porcentajespdf = porcentajes;
            }
            opciones.datos = JSON.stringify(opciones.datos);
            opciones.aoHeader = "";
            opciones.FILASHEADER_array = "";
        }
        ////enviar con mayusculas
        $(opciones.definicioncolumnas).each(function (i, e) {
            var fila = e;
            $.each(fila, function (colnom, colvalor) {
                if (colnom === "alinear" || colnom === "tipo") {
                    fila[colnom] = fila[colnom].toUpperCase();
                }
            });
        });
        ////
        tabla = {
            "usardatatable": opciones.usardatatable
            , "estilos_tabla": JSON.stringify(opciones.estilos_tabla)
            , "multiplestablas": opciones.multiplestablas

            , "accion": opciones.accionboton
            , "cabeceras": JSON.stringify(opciones.cabeceras)
            , "cabecerasnuevas": JSON.stringify(opciones.cabecerasnuevas)
            , "datos": opciones.datos
            , "definicioncolumnas": JSON.stringify(opciones.definicioncolumnas)
            , "aoColumns": JSON.stringify(opciones.aoColumns)
            , "aoHeader": JSON.stringify(opciones.FILASHEADER_array)
            , "hojas": JSON.stringify(opciones.hojas)
            , "nombrearchivo": opciones.tituloarchivo
            , "tituloreporte": opciones.tituloreporte
            , "titulo_subtitulo": opciones.titulo_subtitulo
            , "nombrehoja": opciones.nombrehoja
            , "visibles": JSON.stringify(opciones.visibles)
            , "porcentajes": JSON.stringify(opciones.porcentajespdf)
            , "mostrar_headers_tabla": opciones.mostrar_headers_tabla
        };
        ARRAY_TABLAS.push({ name: "tabla_" + ii, value: JSON.stringify(tabla) });
    });///fin foreach TABLAS opciones
    accion_ = tabla.accion;
    xhr_generar_botonexcel(opciones.url, ARRAY_TABLAS, accion_);


    //var form = $('<form></form>');

    //form.attr("method", "post");
    //form.attr("target", "_blank");
    //form.attr("action", (opciones.url));

    //$.each(ARRAY_TABLAS, function (i,e) {
    //    var field = $('<input></input>');

    //    field.attr("type", "hidden");
    //    field.attr("name", e.name);
    //    field.attr("value", e.value);

    //    form.append(field);
    //});
    //$(document.body).append(form);
    //form.submit();
    //$.LoadingOverlay("hide");



}
        ///FINNNNNNNNN funcionbotonesfinal de S3K.CorpPj.Seguridad


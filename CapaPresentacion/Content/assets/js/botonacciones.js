

//////funcion para callback al terminar de generar excel y al aparecer boton de descarga
function generar_botonexcel(urlaccion,parametros_array) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', urlaccion, true);
    xhr.responseType = 'arraybuffer';
    xhr.onload = function () {
        if (this.status === 200) {
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }
            var type = xhr.getResponseHeader('Content-Type');

            var blob = new Blob([this.response], { type: type });
            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                window.navigator.msSaveBlob(blob, filename);
            }
            else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    // use HTML5 a[download] attribute to specify filename
                    var a = document.createElement("a");
                    // safari doesn't support this yet
                    if (typeof a.download === 'undefined') {
                        window.location = downloadUrl;
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
        }
        else {
            $.LoadingOverlay("hide");
            toastr.error("Error Excel","Mensaje Servidor")
            // Handle Error Here
        }
    };
    xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
    xhr.send($.param(parametros_array));
}

///obtiene nombre de columna del objeto DataTable,  usando sTitle  , title
function sacarColumnaTitle(objetodt, datanombre) {
    var columnas = objetodt.context[0].aoColumns
    titulodecolumna = "";
    $(columnas).each(function (col_i, col_valor) {
        if (col_valor["data"] == datanombre) {
            titulodecolumna = col_valor["title"]

            //return titulodecolumna;
        }
    })
    return titulodecolumna;
}
////obtiene objeto del array de columnas para cambiar
function get_columnacambios(nombrecolumna, columnac_array) {
    var columnacambios = "";
    $(columnac_array).each(function (i, e) {
        if (nombrecolumna.toUpperCase() == (e.nombre).toUpperCase()) {
            columnacambios = e;
        }
    })
    return columnacambios;
}

////obtiene array de headers para excel  , usa aoHeader del objeto datatable
function get_headerexcel(objetodt) {
    nuevo_aoheader = objetodt.context[0].aoHeader;
    aoheader_array = $.extend(true, [], nuevo_aoheader);
    $(aoheader_array).each(function (i, e) {
        celdas_array = e;
        col = 0;
        $(celdas_array).each(function (cel_i, cel_e) {
            if (cel_e.USADO == true) { return; }
            celdajquery = cel_e.cell;
            rowspan = parseInt($(celdajquery).attr("rowspan"))
            colspan = parseInt($(celdajquery).attr("colspan"))
            cel_e.anro = cel_i;
            cel_e.rowspan = rowspan
            cel_e.colspan = colspan
            cel_e.x1 = cel_i + 1;
            cel_e.x2 = cel_i + colspan;
            cel_e.y1 = i + 1;
            cel_e.y2 = i + rowspan;
            cel_e.text = $(celdajquery).text()
            if (colspan > 1) {
                sig = cel_i + 1;
                for (iii = 1; iii < colspan; iii++) {
                    aoheader_array[i][sig].USADO = true;
                    aoheader_array[i][sig].USADOPORRRR = aoheader_array[i][cel_i];
                    sig++;
                }
            }
            if (rowspan > 1) {
                if (typeof aoheader_array[i - 1] == "undefined") { return; }
                rowspan_cellsuperiori = parseInt($(aoheader_array[i - 1][cel_i].cell).attr("rowspan"));
                if (rowspan_cellsuperiori > 1) {
                    cel_e.USADO = true;
                    cel_e.unique = "FALSEEEEEEEEEEEEEEE";//"FALSEEEE";
                }
            }
        })
    })

    nuevo_array = []
    $(aoheader_array).each(function (i, e) {
        fila_tr = {}
        td_array = []
        $(e).each(function (ii, ee) {
            if (typeof ee.USADO == "undefined") {
                td_array.push(ee)
            }
        })
        fila_tr.tds = td_array;
        fila_tr.nro = i;
        nuevo_array.push(fila_tr)
    })

    return nuevo_array;


}

//ocultar = ["Accion"];//array de columnas para ocultar , usar titulo de columna
//reemplazar = {
// cabecerasnuevas = [];
//  cabecerasnuevas.push({ nombre: "aver", valor: "avervalor" });
//  porcentajesparapdf = [{  "por": 32.613508442776734 },{  "por": 32.98686679174484, "nombre": "Correo" } ]
// Tiene q ser mismo numero de columnas 
//tituloreporte = "Reporte Empleados";
//funcionbotones({
//    botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar, tituloreporte: tituloreporte
//});



//nomostrar = "Accion"
//columna_cambio = [{
//    nombre: "Estado",
//    render: function (o) {
//        valor = "";
//        if (o == 1) {
//            valor = "Habilitado";
//        }
//        else { valor = "Deshabilitado"; }
//        return valor;
//    }
//}]

//definicioncolumnas = [];
//definicioncolumnas.push({ nombre: "tipo_cambio", tipo: "decimal", alinear: "right", formato: "S/#,##0.00" });
//                        ///tipo=>   STRING, DATE, integer,  decimal,FLOAT       ;  string por defecto
//                        ///alinear  =>   LEFT  , center , right    ;   left por defecto
//                        /// sumar  =>  por defecto false  , para integers, decimals 
//                        ////formato  => formato de columna  para EXCEL   ejem:   "#,##0.00"     "###0" 

//columna_cambio = [{
//    nombre: "estadoselect",
//    "render": function (o) {
//        valor = "";
//        if (o == 0) {
//            valor = "Anulado";
//        }
//        if (o == 1) {
//            valor = "Abierto";
//        }
//        if (o == 2) {
//            valor = "Cerrado";
//        }
//        return valor;
//    }
//}]
function funcionbotonesnuevo(obj) { ////obj  => objeto
    $.LoadingOverlay("show");

    var objeto_datatabla = obj.tablaobj;
    columnacambio_array = obj.columna_cambio;
    nomostrar = obj.ocultar;///objeto o string;
    nomostrar_array = [];
    if (typeof nomostrar == "string") {
        nomostrar_array.push(nomostrar.toUpperCase())
    } else {
        //nomostrar_array = nomostrar;
        nomostrar_array = nomostrar.map(a => a.toUpperCase())
    }
    columnasvisibles_array = objeto_datatabla.context[0].aoColumns;
    arraydatosFinal = [];    ///array final de objetos 
    datosdt = objeto_datatabla.data();
    datos_array = datosdt.toArray();
    $(datos_array).each(function (datosdt_i, datosdt_valor) {

        var dato_fila = datosdt_valor;
        objetofila = {};
        $(columnasvisibles_array).each(function (col_i, col_valor) {  ////columns en datatable
            if (col_valor.data == null) {
                return;
            }
            var col_datanombre = col_valor["data"];
            ///Columnas no mostrar
            if ($.inArray((col_valor["sTitle"]).toUpperCase(), nomostrar_array) != -1) {
                return;
            }
            // fin columnas no mostrar

            var cambioscolumna = get_columnacambios((col_valor["sTitle"]).toUpperCase(), columnacambio_array)  //SI SE ENVIA CAMBIO DE RENDER DE COLUMNA
            if (cambioscolumna != "") {
                objetofila[col_valor["sTitle"]] =cambioscolumna.render(dato_fila[col_datanombre])
                // objetofila[col_valor["sTitle"]] = cambioscolumna.render(dato_fila[col_datanombre])
            }
            else {
                if (col_valor.mRender != null) {
                    var valorrender = objeto_datatabla.cells(datosdt_i, col_i).render("display")[0]; ///datosdt_i => contador datos_array; col_i=> contador columnasviibles_array
                    objetofila[col_valor["sTitle"]] = valorrender;// dato_fila[col_datanombre];
                }
                else {
                    objetofila[col_valor["sTitle"]] = dato_fila[col_datanombre];
                }
            }
        })
        arraydatosFinal.push(objetofila)
    })
    
    $this = obj.botonobjeto;  /// se agregara html de form despues de este elemento
    ///*********************  OPCIONES AL INICIALIZAR
    cabecerasdefecto = []
    //cabecerasdefecto.push({ nombre: "Empresa", valor: getCookie("Empresa") })
    //cabecerasdefecto.push({ nombre: "Sala", valor: getCookie("Sala") })
    cabecerasdefecto.push({ nombre: "Fecha", valor: new moment().format("DD/MM/YYYY HH:mm a") })
    ////
    defaults = {
        cabeceras: cabecerasdefecto,
        cabecerasnuevas: [],
        datos: "",
        definicioncolumnas: [],
        tituloarchivo: $('.main-text:visible').text().trim().replace(/ /g, "_").replace(" ", "_"),
        tituloreporte: $('.main-text:visible').text().trim(),
        aoColumns: objeto_datatabla.context[0].aoColumns,   ////def columnas objeto datatable
        url: "Funciones/botonacciones",

    }
    ////****************************************
    opciones = $.extend({}, defaults, obj);
    var nombreform = 'formuenviar';
    /////OBJETO  CABECERAS REPORTE
    objeto_datatabla.cabecerasnuevas = opciones.cabecerasnuevas;


    ////enviar con mayusculas
    $(opciones.definicioncolumnas).each(function (i, e) {
        var fila = e;
        $.each(fila, function (colnom, colvalor) {
            if (colnom == "alinear" || colnom == "tipo") {
                fila[colnom] = (fila[colnom]).toUpperCase();
            }
        })
    }
    )
    ////
    objeto_datatabla.definicioncolumnas = opciones.definicioncolumnas;
    ///FIN CABECERAS REPORTE

    var accion = basePath + opciones.url;
    var accionboton = $(obj.botonobjeto).attr("id");
    $('#' + nombreform).remove();

    opciones.datos = JSON.stringify(arraydatosFinal);



    ////sacar headers  tr ths  con rowspan colspan para excel  => para generar excel con multiples filas en el header
    var FILASHEADER_array = [];
    var headersarray = objeto_datatabla.context[0].aoHeader;
    i = 1;
    $(headersarray).each(function (headers_i, headers_e) {
        TRHEADER_objeto = {}
        TRHEADER_objeto.nrofila = i;///primer tr
        ths_array = [];
        var filaheaders_array = headers_e;
        $(filaheaders_array).each(function (fh_i, fh_e) {
            th_objeto = {};
            var celda = fh_e.cell.attributes;
            objetocelda = {}
            objetocelda.text = fh_e.cell.textContent;
            //if (typeof celda.rowspan == "undefined") { rowspan_celda = 1; } else { rowspan_celda = celda.rowspan.value;}
            //if (typeof celda.colspan == "undefined") { colspan_celda = 1; } else { colspan_celda = celda.colspan.value; }

            rowspan_celda = celda.rowspan.value;
            colspan_celda = celda.colspan.value;


            objetocelda.rowspan = rowspan_celda;//celda.rowspan.value;
            objetocelda.colspan = colspan_celda;//celda.colspan.value;
            th_objeto = objetocelda;
            if (JSON.stringify(ths_array[ths_array.length - 1]) != JSON.stringify(th_objeto)) {
                ////solo enviar unicos
                if ($.inArray((th_objeto.text).toUpperCase(), nomostrar_array) == -1) {

                    ths_array.push(th_objeto)
                }
            }
        })
        TRHEADER_objeto.ths = ths_array;
        FILASHEADER_array.push(TRHEADER_objeto)
        i++;
    })
    ///



    $($this).after($("<form style='display:none' id='" + nombreform + "'></form>")
        .append($('<input type="text">').attr("name", "accion").val(accionboton))
        .append($('<input type="text">').attr("name", "cabeceras").val(JSON.stringify(opciones.cabeceras)))
        .append($('<input type="text">').attr("name", "cabecerasnuevas").val(JSON.stringify(opciones.cabecerasnuevas)))
        .append($('<input type="text">').attr("name", "datos").val(opciones.datos))
        .append($('<input type="text">').attr("name", "definicioncolumnas").val(JSON.stringify(opciones.definicioncolumnas)))

        .append($('<input type="text">').attr("name", "aoColumns").val(JSON.stringify(opciones.aoColumns)))
        .append($('<input type="text">').attr("name", "aoHeader").val(JSON.stringify(FILASHEADER_array)))


        .append($('<input type="text">').attr("name", "hojas").val(JSON.stringify(opciones.hojas)))
        .append($('<input type="text">').attr("name", "nombrearchivo").val(opciones.tituloarchivo))
        .append($('<input type="text">').attr("name", "tituloreporte").val(opciones.tituloreporte))
        .append($('<input type="text">').attr("name", "visibles").val(JSON.stringify(opciones.visibles)))
        .attr("action", accion).attr("method", "POST")
    );

    generar_botonexcel(accion, $("#" + nombreform).serializeArray());

  
    //$('#' + nombreform).submit();
    //$('#' + nombreform).submit();

}


function funcionbotones(obj) {
    ocultarcolumnas = obj.ocultar;
    $this = obj.botonobjeto;
    var objetodatatable = obj.tablaobj;

    nuevoarray = [];

    //edt=objetodatatable.rows({ filter: 'applied' }).data()
    edt = objetodatatable.data();

    $(edt).each(function (i, e) {
        nuevoarray.push(e);
    });
    columnasvisibles2 = [];

    $(objetodatatable.context[0].aoColumns).each(
        function (i, e) {
            // if (e.data != opciones.nomostrar) {
            columnasvisibles2.push(e.data);
            // }
        });

    datosnuevos = nuevoarray;
    $(datosnuevos).each(function (i, e) {
        $.each(e,
            function (key, element) {
                var nombreprop = key;
                if ($.inArray(nombreprop, columnasvisibles2) != -1) {//existe en array

                } else {
                    delete e[key];
                }
            });
    });
    ///////Cambiar nombre de columna de json por  titulo de cada columna en datatable
    arraydatosfinal = [];
    $(datosnuevos).each(function (i, e) {
        filanueva = {}
        $.each(e,
            function (key, element) {

                $(edt.context[0].aoColumns).each(function (cont, valor) {
                    if (valor.data == key) {

                        if (valor.data == key) {
                            if (valor.sTitle == "Estado") {
                                var valorestado = "";
                                switch (parseInt(element)) {
                                    case 0: valorestado = "DesHabilitado"; break;
                                    case 1: valorestado = "Habilitado"; break;
                                }
                                filanueva[valor.sTitle] = valorestado;

                            } else {
                                filanueva[valor.sTitle] = element;
                            }
                        }
                        //filanueva[valor.sTitle] = element;
                    }
                })
            });
        arraydatosfinal.push(filanueva)
    })
    ////fin cambio

    valorestabla = sacarcolumnasvisibilidadnuevo(objetodatatable, { ocultar: ocultarcolumnas });//false  no incluir ultima columna accion  , por defecto false
    ///*********************  OPCIONES AL INICIALIZAR
    defaults = {
        tituloarchivo: $('.main-text:visible').text().trim().replace(/ /g, "_").replace(" ", "_"),
        visibles: valorestabla.visibles,
        invisibles: valorestabla.invisibles,
        cabecerasnuevas: [],
        definicioncolumnas: [],
        //nomostrar: "",
        tituloreporte: $('.main-text:visible').text().trim().replace(/ /g, "_").replace(" ", "_"),
        porcentajespdf: valorestabla.porcentajes,
        datos: ""
    }
    ////****************************************
    opciones = $.extend({}, defaults, obj);


    datosnuevos = JSON.stringify(arraydatosfinal);////datosnuevos

    if (opciones.datos != "") {
        datosnuevos = JSON.stringify(opciones.datos);
    }
    objetodatatable.visibles = opciones.visibles;
    objetodatatable.invisibles = opciones.invisibles;
    objetodatatable.porcentajes = opciones.porcentajespdf;

    /////OBJETO  CABECERAS REPORTE
    objetodatatable.cabecerasnuevas = opciones.cabecerasnuevas;
    objetodatatable.definicioncolumnas = opciones.definicioncolumnas;
    ///FIN CABECERAS REPORTE

    var nombreform = 'formuenviar';
    var accion = basePath + "Funciones/botonacciones"; var accionboton = $(obj.botonobjeto).attr("id");
    $('#' + nombreform).remove();
    $($this).after($("<form style='display:none' id='" + nombreform + "'></form>")
        .append($('<input type="text">').attr("name", "datos").val(datosnuevos))
        //.append($('<input type="text">').attr("name", "datosurl").val("CajaMovimientoController/BuscarMonedasEncontradas"))
        //.append($('<input type="text">').attr("name", "datosurlparametros").val(JSON.stringify({ fecha: "2017-10-10", signo: "", txtMaq: {}, monto: "", posicion: 1 })))
        .append($('<input type="text">').attr("name", "cabecerasnuevas").val(JSON.stringify(opciones.cabecerasnuevas)))
        .append($('<input type="text">').attr("name", "definicioncolumnas").val(JSON.stringify(opciones.definicioncolumnas)))
        .append($('<input type="text">').attr("name", "tituloreporte").val(opciones.tituloreporte))
        .append($('<input type="text">').attr("name", "visibles").val(JSON.stringify(opciones.visibles)))
        .append($('<input type="text">').attr("name", "accion").val(accionboton))
        .append($('<input type="text">').attr("name", "nombrearchivo").val(opciones.tituloarchivo))
        .append($('<input type="text">').attr("name", "invisibles").val(JSON.stringify(opciones.invisibles)))
        .append($('<input type="text">').attr("name", "porcentajes").val(JSON.stringify(opciones.porcentajespdf)))
        .attr("action", accion).attr("method", "POST")
    );

    if (accionboton == "imprimir") {
        $('#' + nombreform).attr('target', '_blank');
    }
    $('#' + nombreform).submit();

}

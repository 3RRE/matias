
////////////////////////FUNCIONES PARA DATATABLE SERVERSIDE
///EJEM
        //opcionesdatatable2 = {
        //    paging: true,
        //    buscadorexterno: true, recargarintervalo: true, tiempointervalo: 5000,
        //    buscador: "buscadorexterno2",
        //    url: basePath + "Mantenimiento/GetListadoMaquinadatatables2", deferLoading: null,
        //    fixedColumns: {
        //        leftColumns: 3
        //    },
        //    divparabuscador: "filtrodiv",
        //    cambiarcolumna: [{
        //        data: "Conexion", name: "Conexion",
        //        "bSortable": false,
        //        "width": "40px",
        //        "render": function (o) {

        //            return o == "On" ? '<span type="button" class="btn btn-xs btn-success">' + o + ' </span>' : o == "Off" ? '<span type="button" class="btn btn-xs btn-danger">' + o + ' </span> ' : '<span type="button" class="btn btn-xs btn-info">' + o + ' </span>';
        //        }
        //    },
        //    {
        //        className: "tdright",
        //        data: "Cout", name: "Cout",
        //        "width": "67px",
        //    },
        //    {
        //        className: "tdright",
        //        data: "HandPay", name: "HandPay",
        //    }
        //    ]
        //    , order: [0, "asc"]
        //    ,
        //    complete: function () {
        //    }
        //}
        //tabla2objeto = $("#IDTABLA").datatableservidor(opcionesdatatable2);
///FIN EJEM

tablaserver = 1;
primeravezajax = 1;

ajaxrepitiendo = null;




///sacarcolumnasvisibilidad() =>  devuelve porcentajes de columna para PDF y  columnas invisibles que no deben mostrarse en excel o pdf ,  todo lo calcula utilizando el datatable en la vista
function sacarcolumnasvisibilidadnuevo(tablaserverobjeto, objopciones) {
    defaults = {
        ocultar: ["Acciones"]
    }
    ////****************************************
    opciones = $.extend({}, defaults, objopciones);

    var valores = {};
    var porcentajes = []
    var visibles = []; var invisibles = []; var tablaobj = tablaserverobjeto;
    var tabla = tablaserverobjeto.context[0].nTable.id;
    $(tablaserverobjeto.columns().context[0].aoColumns).each(function (i, e) {
        tablaserverobjeto.columns().context[0].aoColumns[i].bVisible ? visibles.push(i) : invisibles.push(i);
        if (opciones.ocultar.constructor === Array) {
            if ($.inArray(tablaserverobjeto.columns().context[0].aoColumns[i].sTitle, opciones.ocultar) != -1) {
                // invisibles.push(i)
                a = 0;
                $.each(tablaserverobjeto.rows(0).data()[0]
                    , function (nombre, valor) {
                        if (nombre == tablaserverobjeto.columns().context[0].aoColumns[i].data) {
                            invisibles.push(a)
                        }
                        a++
                    }
                )
            }
        } else {
            if (tablaserverobjeto.columns().context[0].aoColumns[i].sTitle == opciones.ocultar) {
                a = 0;
                $.each(tablaserverobjeto.rows(0).data()[0]
                    , function (nombre, valor) {
                        if (nombre == tablaserverobjeto.columns().context[0].aoColumns[i].data) {
                            invisibles.push(a)
                        }
                        a++
                    }
                )
            }

        }
    });
    widthaquitar = 0;

    cantidadcolumnas = $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').length;
    widthtotal = $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0)').outerWidth();
    //widthaocultar =$("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').eq(cantidadcolumnas - 1).outerWidth();

    sumawidthocultar = 0;
    selectparaocultar = "";

    if (opciones.ocultar.constructor === Array) {
        $(opciones.ocultar).each(function (i, e) {
            sumawidthocultar = sumawidthocultar + $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th:contains("' + e + '")').outerWidth()
            selectparaocultar = selectparaocultar + ":not(:contains(" + e + "))";
        })
    } else {
        sumawidthocultar = sumawidthocultar + $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th:contains("' + opciones.ocultar + '")').outerWidth()
        selectparaocultar = ":not(:contains(" + opciones.ocultar + "))";

    }
    widthfinal = widthtotal - sumawidthocultar;
    //widthsinacciones = widthtotal - widthacciones;
    $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th' + selectparaocultar)
        //   $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th:not(:eq(' + ($("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').length - 1) + '))')
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



///sacarcolumnasvisibilidad() =>  devuelve porcentajes de columna para PDF y  columnas invisibles que no deben mostrarse en excel o pdf ,  todo lo calcula utilizando el datatable en la vista
function sacarcolumnasvisibilidad(tablaserverobjeto, concolumnaacciones) {

    //colaccion = concolumnaacciones || true;
    if (typeof concolumnaacciones == 'undefined') { colaccion = true; } else {

        if (concolumnaacciones == true) {
            colaccion = false;
        } else {
            colaccion = true;

        }
    }
    var valores = {};
    var porcentajes = []
    var visibles = []; var invisibles = []; tablaobj = tablaserverobjeto;
    var tabla = tablaserverobjeto.context[0].nTable.id;
    // if (typeof tablaserver != "undefined" && tablaserver != 1) {
    $(tablaserverobjeto.columns().context[0].aoColumns).each(function (i, e) {
        tablaserverobjeto.columns().context[0].aoColumns[i].bVisible ? visibles.push(i) : invisibles.push(i);

    });
    widthaquitar = 0;
    /* if (typeof tablaserverobjeto.noincluircolumnapdfexcel != "undefined") {
        
         if (tablaserverobjeto.noincluircolumnapdfexcel.length > 0) {
 
             $(tablaserverobjeto.noincluircolumnapdfexcel).each(function (i, e) {
                 visibles = visibles.filter(function (el) {
                     return el !== e;
                 });
 
                 invisibles.push(e);
 
                 widthaquitar = widthaquitar+$("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').eq(i).outerWidth();
                 
 
             })
         }
     }*/

    cantidadcolumnas = $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').length;
    widthtotal = $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0)').outerWidth();
    widthacciones =
        $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').eq(cantidadcolumnas - 1).outerWidth();

    if (colaccion) {///colaccion true =>  no incluir ultima columna acciones
        widthsinacciones = widthtotal - widthacciones;
        $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th:not(:eq(' + ($("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th').length - 1) + '))')
            .each(function (i, e) {
                porcentajes.push({
                    wid: $(this).outerWidth(),
                    por: $(this).outerWidth() * 100 / widthsinacciones,
                    nombre: $(this).text()
                });

            });

    } else {/// colaccion false => incluir ultima columna
        widthsinacciones = widthtotal;
        $("#" + tabla + '_wrapper .dataTables_scrollHeadInner table thead tr:eq(0) th')
            .each(function (i, e) {
                porcentajes.push({
                    wid: $(this).outerWidth(),
                    por: $(this).outerWidth() * 100 / widthsinacciones,
                    nombre: $(this).text()
                });

            });

    }
    // widthsinacciones = widthsinacciones - widthaquitar;



    valores.porcentajes = porcentajes;
    valores.visibles = visibles;
    valores.invisibles = invisibles;



    //  }
    return valores;
}
function getcolumna(colarray, nombrecol) {
    var no;
    $(colarray).each(function (i, e) {
        if (e.name == nombrecol) {
            no = i;
        }
    });
    return no;
}
(function ($) {
    $.fn.extend({
        acciones: function (opcionesacciones) {
            var opcionesa = opcionesacciones;
            defaults = {

                html: '<button type="button" class="btn btn-xs btn-warning btnEditar"><i class="glyphicon glyphicon-pencil"></i></button>',
                botones: [],
            };
            html = '';
            if (typeof opcionesa.botones != 'undefined') {
                $(opcionesa.botones).each(function (i, e) {
                    html = html + '<button type="button" class="btn btn-xs' + e.claseboton + '" data-id="' + e.id + '"><i class="glyphicon ' + e.claseicono + '"></i></button>'

                })
                opcionesa.html = hmtl;
            }
            opciones = $.extend({}, defaults, opcionesa);

            $(this).html(opciones.html);
            $(this).on('click', function (e) { e.preventDefault() });
        }

    });
})(jQuery);

(function ($) {
    $.fn.extend({
        ///funcion datatablesservidor   $(el).datatableservidor(opcionesobjeto)
        datatableservidor: function (opcionesa) {
            var objetosdtservidor = [];
            var opciones = opcionesa;
            defaults = {
                buscadorexterno: true, data: "",
                deferLoading: null,
                "bDestroy": true,
                "bSort": true,
                "paging": true,
                // "scrollY": "450px",
                "scrollX": false,
                "sScrollX": "100%",
                "scrollCollapse": true,
                "bProcessing": true,
                "bDeferRender": false,
                "autoWidth": false,
                "bAutoWidth": false,
                "pageLength": 10,
                recargarintervalo: false,
                tiempointervalo: 3000,
                fixedColumns: {
                    leftColumns: 0
                }

            };


            opciones = $.extend({}, defaults, opciones);
            //var intervalotabla;
            if (opciones.recargarintervalo == true) { var intervalotabla; }
            if (opciones.columnas == "") { console.log("No se envio cabeceras"); alert("No se envio cabeceras") }
            $(this).each(function (elno, el) {
                $(el).empty(); var esteelemento = el;

                $('#' + $(el).attr("id") + "_controlescontenedor").remove();// alert("aaa");//
                if (opciones.divparabuscador) {
                    $('#' + opciones.divparabuscador)
                        .append($("<div>").addClass("row")
                            .attr("id", $(el).attr("id") + "_divfiltro")
                            .append(
                                $("<div>").addClass("col-md-10 lineDerecha").css("min-height", "150px")
                                    .append($("<h5>").addClass("text-warning").text("Filtros de Búsqueda")).append($("<div>").append($("<div>")
                                        .addClass("row").attr("id", $(el).attr("id") + "_buscadorexterno")))
                            )
                            .append(
                                $("<div>").addClass("col-md-2")
                                    .append(
                                        $("<div>")
                                            .append($("<h5>").addClass("text-warning").text("Ocultar/Ver Columnas")))
                                    .append($("<div>")
                                        .append($("<div>")
                                            .attr("id", $(el).attr("id") + "_" + "pruebaScroll")
                                            .addClass("other-stats")
                                            .css("height", "120px")
                                            .append($("<ul>").attr("id", $(el).attr("id") + "_" + "divtogglecolumnas")
                                                .addClass("task-list")))))
                        ); //fin append


                } else {

                    $(el).closest(".row").parent().closest(".row")
                        .before(
                            $('<div>').addClass("row divfiltro")
                                .attr("id", $(el).attr("id") + "_divfiltro")
                                .append(
                                    $('<div>').attr("id", $(el).attr("id") + "_controlescontenedor").addClass("col-md-12 notlabel")
                                        .append($('<div>').addClass("block")
                                            .append($('<div>').addClass("block-content-outer")
                                                .append($("<div>")
                                                    .addClass("block-content-inner")
                                                    .append($("<div>").addClass("row")
                                                        .append($("<div>").addClass("col-md-10 lineDerecha").css("min-height", "150px")
                                                            .append($("<h5>").addClass("text-warning").text("Filtros de Búsqueda"))
                                                            .append($("<br>")).append($("<div>")
                                                                .append($("<div>").addClass("row").attr("id", $(el).attr("id") + "_buscadorexterno"))))
                                                        .append($("<div>").addClass("col-md-2")
                                                            .append($("<div>")
                                                                .append($("<h5>").addClass("text-warning").text("Ocultar/Ver Columnas")))
                                                            .append($("<div>")
                                                                .append($("<div>").attr("id", $(el).attr("id") + "_" + "pruebaScroll")
                                                                    .addClass("other-stats").css("height", "120px")
                                                                    .append($("<ul>").attr("id", $(el).attr("id") + "_" + "divtogglecolumnas").addClass("task-list"))))))
                                                ) /*fin append block-content-outer*/
                                            ) /*fin append block*/
                                        ) /*fin notlabel*/
                                ));

                }//fin else divparabuscador
                /*   $(el).before(
                                   $("<div>")
                                           .attr("id", $(el).attr("id") + "_buscadorexterno")
                                 )*/
                var buscadorexternoel = $(el).attr("id");

                data = opciones.data != "undefined" && opciones.data != "" ? JSON.stringify(opciones.data) : " ";
                data = "&id=1"; visibles = [];
                invisibles = []; porcentajes = [];
                //tablaserver = null;
                var urlSERVER = opciones.url;
                $.ajax({
                    "url": urlSERVER, "dataType": "json", method: "POST", data: "columnas=1",//1er AJAX devuelve HEADERS
                    "success": function (jsonRES) {
                        if (opciones.accioncolumna) {
                        }
                        jsonres = jsonRES;
                        // agregartablehtml(esteelemento, jsonRES)
                        var objetodt = (agregartablehtml(esteelemento, jsonRES));
                        columnasdt = objetodt.columnasdt;
                        // colacciones=getcolumna(columnasdt,"Acciones");//alert(colacciones)
                        //columnasdt[colacciones]=(opciones.accioncolumna);
                        if (opciones.cambiarcolumna) {
                            if ($.isArray(opciones.cambiarcolumna)) {
                                $(opciones.cambiarcolumna).each(function (i, e) {
                                    col = getcolumna(columnasdt, e.name)
                                    columnasdt[col] = (e);
                                });
                            } else {
                                //                                alert(opciones.cambiarcolumna.name)
                                col = getcolumna(columnasdt, opciones.cambiarcolumna.name)
                                columnasdt[col] = (opciones.cambiarcolumna);
                            }

                            //                            col = getcolumna(columnasdt, opciones.cambiarcolumna.name)
                            //                            columnasdt[col] = (opciones.cambiarcolumna);
                        }

                        $.fn.dataTable.ext.legacy.ajax = true;
                        var tablaserver = $(esteelemento).on('order.dt', function () {
                            $('table').css('width', '100%');
                            $('.dataTables_scrollHeadInner').css('width', '100%');
                            $('.dataTables_scrollFootInner').css('width', '100%');
                        })
                            .on('search.dt', function () {
                                $('table').css('width', '100%');
                                $('.dataTables_scrollHeadInner').css('width', '100%');
                                $('.dataTables_scrollFootInner').css('width', '100%');
                            })
                            .on('page.dt', function () {
                                $('table').css('width', '100%');
                                $('.dataTables_scrollHeadInner').css('width', '100%');
                                $('.dataTables_scrollFootInner').css('width', '100%');
                            }).DataTable({
                                "bDestroy": true,
                                "bSort": opciones.bSort,
                                "paging": opciones.paging,
                                //  "scrollY": "450px",
                                "scrollX": false,
                                "sScrollX": "100%",
                                "scrollCollapse": true,
                                "bProcessing": true,
                                "bDeferRender": false,
                                "autoWidth": false,
                                "bAutoWidth": false,
                                "pageLength": opciones.pageLength,
                                fixedColumns: opciones.fixedColumns,
                                serverSide: true,
                                "bDestroy": true,
                                //  dom: 'lBfrtip',
                                dom: 'lfrtip',
                                /* buttons: [
                                     {
                                         extend: 'pdfHtml5',
                                         message: 'PDF created by PDFMake with Buttons for DataTables.'
                                     }
                                 ],*/
                                colReorder: true,
                                // paging: false,
                                "lengthMenu": [[10, 50, 200, -1], [10, 50, 200, "All"]],
                                // "order": [[ 3, "desc" ]]
                                processing: true,

                                deferLoading: opciones.deferLoading,
                                "order": opciones.order,
                                "columnDefs": opciones.columnDefs,

                                ajax: function (datat, callback, settings) {

                                    datat.primeravezajax = primeravezajax;
                                    primeravezajax = 0;
                                    data = datat;

                                    if (typeof opciones.sumatotalcolumnas != "undefined") {
                                        datat.sumatotalcolumnas = (opciones.sumatotalcolumnas).toString()
                                    }

                                    ajaxrepitiendo = $.ajax({
                                        url: urlSERVER,
                                        type: 'POST',
                                        data: datat,//+data,


                                        beforeSend: function () {
                                            if (typeof opciones.beforeSend != "undefined") {
                                                opciones.beforeSend();
                                            }
                                        },
                                        complete: function () {
                                            if (typeof opciones.complete != "undefined") {

                                                opciones.complete();
                                            }
                                        },
                                        success: function (datos) {//  alert(datat)
                                            //  agregartablaheaderfooter(elemento);
                                            aaaa = datos;
                                            var respuesta = JSON.parse(datos);
                                            var valores = sacarcolumnasvisibilidad(tablaserver);


                                            // agregartablehtml(this, respuesta.cabeceras)

                                            callback(respuesta);
                                            consulta = respuesta.consultasin;
                                            consultapdf = respuesta.consulta
                                            //$('#formpend #consulta').val(consulta);
                                            tablaserver.consulta = consulta
                                            tablaserver.consultapdf = consultapdf
                                            tablaserver.visibles = valores.visibles
                                            tablaserver.invisibles = valores.invisibles
                                            tablaserver.porcentajes = valores.porcentajes
                                            tablaserver.Cabezas = objetodt.Cabezas
                                            tablaserver.sumascolumnas = respuesta.sumascolumnas
                                            tablaserver.sumascolumnassinfiltro = respuesta.sumascolumnassinfiltro
                                            tablaserver.sumascolumnasult = respuesta.sumascolumnasult
                                            tablaserver.consultasumascolumnasult = respuesta.consultasumascolumnasult
                                            tablaserver.consultasumascolumnassinfiltrosult = respuesta.consultasumascolumnassinfiltrosult
                                            tablaserver.sumascolumnasultTOTAL = typeof respuesta.sumascolumnasultTOTAL != "undefined" ? respuesta.sumascolumnasultTOTAL : ""

                                            tablaserver.totalProduccion = typeof respuesta.totalProduccion != "undefined" ? respuesta.totalProduccion : 0
                                            tablaserver.totalRecaudacion = typeof respuesta.totalRecaudacion != "undefined" ? respuesta.totalRecaudacion : 0
                                            //  alert(tablaserver.sumascolumnas)
                                            /* if (typeof opciones.noincluircolumnapdfexcel != "undefined") {
                                                 tablaserver.noincluircolumnapdfexcel = opciones.noincluircolumnapdfexcel
                                                 $(opciones.noincluircolumnapdfexcel).each(function (i,e) {
                                                     tablaserver.visibles = tablaserver.visibles.filter(function (el) {
                                                         return el !== e;
                                                     });
     
                                                     tablaserver.invisibles.push(e);
                                                 })
                                             }*/
                                            $('#formpend #consulta').val(consultapdf);
                                            $('#formpend #cabezas').val(JSON.stringify(objetodt.Cabezas));

                                            if (opciones.ajaxCallback) {
                                                //  alert(tablaserver.sumascolumnas)

                                                opciones.ajaxCallback(tablaserver.sumascolumnasult, tablaserver.sumascolumnasultTOTAL != "" ? tablaserver.sumascolumnasultTOTAL : "")

                                            }
                                        }
                                    });
                                },

                                columns: columnasdt, //[{},{}]

                                "fnRowCallback": function (nRow, aaData, iDisplayIndex) {
                                    ababab = nRow;
                                    ababab2 = aaData;
                                    $(nRow).attr('data-pk', aaData[0]);

                                    $(jsonres).each(function (i, e) {
                                        if (e.atributodata) {
                                            $(nRow).attr('data-' + e.columnanombre, aaData[Object.keys(aaData)[i]]);
                                        }
                                    })

                                        ; return nRow;
                                },

                                "createdRow": function (row, data, index) {
                                    if (opciones.createdRow) {
                                        opciones.createdRow(row, data, index);
                                    }

                                },

                                "initComplete": function (settings, json) {
                                    // if (opciones.buscadorexterno) { agregarbuscadorexterno(opciones.buscador,this, Cabezas) }
                                    if (opciones.recargarintervalo == true) {
                                        if (typeof intervalotabla != "undefined") { clearInterval(intervalotabla) };
                                        intervalotabla = setInterval(function () {
                                            //tablaserver.draw(false)
                                            //tablaserver = tablaserver.ajax.reload();
                                            tablaserver = tablaserver.ajax.reload(null, false);///  1er parametro=> callback al terminar carga  ;2nd parametro=> mantiene paginación

                                        }, opciones.tiempointervalo);
                                        tablaserver.intervalo = intervalotabla;
                                        tablaserver.tiempointervalo = opciones.tiempointervalo;

                                    }
                                    if (opciones.initComplete) {
                                        opciones.initComplete();
                                    }

                                },
                                "drawCallback": function (osettings) {
                                    if ($.isFunction(opciones.drawCallback)) {
                                        opciones.drawCallback();
                                    }
                                    $('.dataTables_filter').hide();// sacarcolumnasvisibilidad(this);
                                },


                                "footerCallback": function (row, data, start, end, display) {
                                    var api = this.api(), data;
                                    // Remove the formatting to get integer data for summation
                                    var intVal = function (i) {
                                        return typeof i === 'string' ?
                                            i.replace(/[\$,]/g, '') * 1 :
                                            typeof i === 'number' ? i : 0;
                                    };
                                    // Total over all pages
                                    if (api.column(".sum").length) {
                                        if (api.column('.sum', { search: 'applied' }).data().length) {
                                            aba = api
                                            //  .column('.sum', { search: 'applied' })
                                            total_revenue = api
                                                .column('.sum', { search: 'applied' })
                                                .data()
                                                .reduce(function (a, b) {
                                                    return intVal(a) + intVal(b);
                                                });
                                        } else {
                                            total_revenue = 0;
                                        }
                                        $(api.columns('.sum').footer()).html(

                                            parseFloat(total_revenue) > 0 ? parseFloat(total_revenue).toFixed(2) : 0.00
                                        );
                                    }
                                },

                            });//fin DATATABLES INICIALIZACION

                        if (opciones.buscadorexterno) {
                            // agregarbuscadorexterno("buscadorexterno", tablaserver, objetodt.Cabezas)
                            agregarbuscadorexterno(buscadorexternoel, tablaserver, objetodt.Cabezas)
                        }
                        if (opciones.recargarintervalo == true) { tablaserver.intervalo = intervalotabla; }
                        objetosdtservidor.push(tablaserver);

                    }//FIN SUCCESS

                });//FIN AJAX

            });//FIN EACH
            //$(this).html(html);
            return objetosdtservidor;

        },// fin datatableservidor plugin 

    });
})(jQuery);

function filtrar(datatabledtfil, t, exacto) {
    if (exacto) {
        //$('#' + datatablenombre).DataTable().
        //    datatabledt.column($(t).attr('data-columna')).search($(t).val(), true).draw();
        if ($(t).val() != "") {
            datatabledtfil.column($(t).attr('data-columna')).search("^" + $(t).val() + "$", true, false).draw();
        } else {
            datatabledtfil.column($(t).attr('data-columna')).search($(t).val()).draw();
        }
        //'^My exact match$', true, false );
    }
    else {
        //$('#' + datatablenombre).DataTable().column($(t).attr('data-columna')).search($(t).val()).draw();
        datatabledtfil.column($(t).attr('data-columna')).search($(t).val()).draw();
    }
}
function filtrofecharango(datatabledt, col) {
    desde = $('#desde' + col).val() ? $('#desde' + col).val() : '0';
    hasta = $('#hasta' + col).val() ? $('#hasta' + col).val() : '0';
    rango = desde + '~' + hasta;
    //$('#' + datatablenombre).DataTable().column(col).search(rango).draw();
    datatabledt.column(col).search(rango).draw();
}

function filtrofecha(datatabledt, col) {
    desde = $('#desde' + col).val() ? $('#desde' + col).val() : '0';
    hasta = $('#hasta' + col).val() ? $('#hasta' + col).val() : '0';
    rango = desde + '~' + hasta;
    //$('#' + datatablenombre).DataTable().column(col).search(rango).draw();

    if ($('#hasta' + col).length > 0) {
        datatabledt.column(col).search(rango).draw();
    } else {
        datatabledt.column(col).search(desde).draw();
    }
}

function agregartablehtml(tabla, opcionesta) {
    ak = opcionesta
    var Cabezas = opcionesta;
    var columnasdt = []; columnasexterno = [];

    elemento = $(tabla);
    elemento.append($("<thead>").append("<tr>"));
    elemento.append($("<tbody>"));
    elemento.append($("<tfoot>").append("<tr>"));
    haysuma = false;


    $(Cabezas).each(function (i, e) {
        var colnombr = e.columnanombre;
        colnombr = colnombr.substr(colnombr.indexOf(".") + 1);
        colnombr = colnombr.replace("[", "");
        colnombr = colnombr.replace("]", "");
        columnasdt.push({ name: colnombr, data: colnombr, title: colnombr, defaultContent: " -- ", className: e.classname });

        $("#" + elemento.attr("id") + " thead tr").append(
            $("<th>").text(e.columnanombre).addClass(function () {
                if (e.suma != "sinsuma") {
                    haysuma = true;
                    return "sum";
                };
            })
        );
        $("#" + elemento.attr("id") + " tfoot tr").append($("<th>"));
        if (!haysuma) {
            $("tfoot", elemento).remove();
        }

    });
    var columnasexterno = new Array();
    columnas = new Array();
    $(Cabezas).each(function (i, e) {
        if (e.tipobuscador != "null") {
            columnas.push({ type: e.tipobuscador });
            columnasexterno.push({ sSelector: "#col" + (i), type: e.tipobuscador, values: (e.valorescombo).split(',') });
        } else {
            columnas.push(null);
            columnasexterno.push(null);
        }
    });
    var objetodt = {}
    objetodt.columnasdt = columnasdt;
    objetodt.Cabezas = Cabezas;
    objetodt.columnasexterno = columnasexterno;
    return objetodt;
}
//FUNCIÓN AGREGAR INPUTS PARA FILTROS
function agregartablaheaderfooter(el) {
    $(el).append($("<thead>").append("<tr>"));
    $(el).append($("<tbody>"));
    $(el).append($("<tfoot>").append("<tr>"));
}

function limpiartitulo(valor) {
    valor = valor.replace("[", "").replace("]", "").replace("_", " ");
    valor = valor.charAt(0).toUpperCase() + valor.slice(1);
    //  aa = aa.charAt(0).toUpperCase() + aa.slice(1)
    return valor;
}
function agregarbuscadorexterno(tabbuscador, datatabledt, cabece) {
    //$('#buscadorexterno').empty();
    $('#' + tabbuscador + "_buscadorexterno").empty();
    //  alert(tabbuscador)
    var buscador = tabbuscador + "_buscadorexterno";
    var Cabezas = cabece;
    ajajaj = Cabezas;
    $(Cabezas).each(function (i, e) {
        tipobuscador = e.tipobuscador;
        palabraexacta = e.busquedaexacta;
        // alert(e.visible+" "+e.columnanombre)
        if (e.columnanombre != "Acciones" || e.columnanombre != "EmpleadoID") {   ///ocultar/ver columnas

            if (e.visible == true) {
                $('#' + tabbuscador + '_divtogglecolumnas').append($("<li class='task-list-item " + tabbuscador + "_" + "divtogglecolumnas'>" +
                    "<div class='checkbox'>" +
                    "<label>" +
                    "<input type='checkbox' checked class='task-list-item-checkbox'> " + e.columnanombre +
                    "</label>" +
                    "</div>" +
                    "</li>").attr("data-column", i));
                //                append($("<span>").addClass("").text(e.columnanombre).attr("data-column", i)));
            }
        }
        // $(a).append()
        if (tipobuscador == "text") {
            $("#" + buscador)
                .append($("<div>").addClass("col-sm-4")
                    .append($("<div>").addClass("input-group ")
                        .append($("<span>").addClass("input-group-addon").text(limpiartitulo(e.columnanombre)))
                        .append($('<input type="text">')
                            .addClass("text")
                            .addClass("form-control input-sm")
                            .attr("data-columna", i)
                            .attr("data-palabraexacta", palabraexacta)
                            .attr("id", buscador + i)))
                );

        }

        if (tipobuscador == "select") {
            valoresselect = JSON.parse(e.valorescombo);
            $("#" + buscador).append(
                $("<div>").addClass("col-sm-4")
                    .append($("<div>").addClass("input-group ")
                        .append($("<span>").addClass("input-group-addon").text(limpiartitulo(e.columnanombre)))
                        .append($('<select  type="select">')
                            .addClass("form-control input-sm")
                            .attr("data-columna", i)
                            .attr("data-palabraexacta", palabraexacta)
                            .attr("id", buscador + i))));

            $('#' + buscador + i).append($('<option>', { value: "", text: "Listar Todo" })); //alert('#' + buscador + i)
            $(valoresselect).each(function (a, v) {
                $('#' + buscador + i).append($('<option>', { value: v.valor, text: v.texto }));

            });

        }

        if (tipobuscador == "date-range") {
            $('#' + buscador).append(
                $("<div>").addClass("form-inline")
                    .append($("<div>").addClass("form-group")
                        .append($("<label>").text(limpiartitulo(e.columnanombre)).attr("for", e.columnanombre))
                        .append($('<div>').addClass("input-group dateOnly")
                            .append($('<input type="text"  class="form-control" >').attr("id", "desde" + i).attr("data-columna", i))
                            .append($("<span>").addClass("input-group-addon input-group-icon")
                                .append($('<span>').addClass("glyphicon glyphicon-calendar")))))
                    .append($("<div>").addClass("form-group")
                        .append($("<label>").text("Hasta"))
                        .append($('<div>').addClass("input-group dateOnly")
                            .append($('<input type="text" class="form-control">').attr("id", "hasta" + i).attr("data-columna", i))
                            .append($("<span>").addClass("input-group-addon input-group-icon")
                                .append($('<span>').addClass("glyphicon glyphicon-calendar"))))));
        }


        if (tipobuscador == "date") {
            $('#' + buscador)
                .append($("<div>").addClass("form-inline")
                    .append($("<div>").addClass("form-group")
                        .append($("<label>").text(limpiartitulo(e.columnanombre)).attr("for", e.columnanombre))
                        .append($('<div>').addClass("input-group dateOnly")
                            .append($('<input type="text"  class="form-control" >').attr("id", "desde" + i).attr("data-columna", i))
                            .append($("<span>").addClass("input-group-addon input-group-icon")
                                .append($('<span>').addClass("glyphicon glyphicon-calendar"))))));
        }

    });

    $("#" + tabbuscador + "_" + "pruebaScroll").mCustomScrollbar({
        autoHideScrollbar: true,
        scrollbarPosition: "outside",
        theme: "dark"
    });


    $("#" + tabbuscador + "_" + "pruebaScroll").iCheck({
        checkboxClass: 'icheckbox_square-orange',
        radioClass: 'iradio_square-red',
        increaseArea: '10%' // optional
    });

    /////////////////funcion delayup  $(el).delayKeyup(callback, tiempo)
    $.fn.delayKeyup = function (callback, ms) {
        var timer = 0;
        var el = $(this);

        $(el).each(function (i, e) {
            $(e).on('keyup', function () {
                clearTimeout(timer);
                timer = setTimeout(function () {
                    console.log("espero " + ms + " ."); //console.log(e);

                    callback(e)
                }, ms);
            });
        })
        return $(this);
    };
    /////////////////fin funcion delayup

    var tiempoesperakeyup = 1000; //tiempo a esperar despues de tipear filtro
    $('#' + buscador + ' .text').delayKeyup(function (e) {
        if ($(e).attr("data-palabraexacta") == "palabraexacta") {
            filtrar(datatabledt, e, true);
        } else {
            filtrar(datatabledt, e, false);
        }
        //alert(el.val());
        // Here I need the input element (value for ajax call) for further process
    }, tiempoesperakeyup);

    $('#' + buscador + ' select').on('change', function (i, e) {
        if ($(this).attr("data-palabraexacta") == "palabraexacta") {
            filtrar(datatabledt, this, true);
        } else {
            filtrar(datatabledt, this, false);
        }
    });
    ///OCULTAR/VER COLUMNAS
    //

    $("#" + tabbuscador + "_" + 'divtogglecolumnas input').on("ifChecked", function (ev) {

        //  ev.preventDefault();
        var column = datatabledt.column(parseInt($(this).closest("li").attr('data-column')));
        column.visible(!column.visible());
        if (!column.visible()) {
            $(this).css("color", "red");
        }
        if (column.visible()) {
            $(this).css("color", "#157ced");
        }
        var valores = sacarcolumnasvisibilidad(datatabledt);
        datatabledt.visibles = valores.visibles;
        datatabledt.invisibles = valores.invisibles;
        datatabledt.porcentajes = valores.porcentajes;
    });

    $("#" + tabbuscador + "_" + 'divtogglecolumnas input').on("ifUnchecked", function (ev) {
        //   ev.preventDefault();
        var column = datatabledt.column(parseInt($(this).closest("li").attr('data-column')));
        column.visible(!column.visible());
        if (!column.visible()) {
            $(this).css("color", "red");
        }
        if (column.visible()) {
            $(this).css("color", "#157ced");
        }
        var valores = sacarcolumnasvisibilidad(datatabledt);
        datatabledt.visibles = valores.visibles;
        datatabledt.invisibles = valores.invisibles;
        datatabledt.porcentajes = valores.porcentajes;
    });

    var dateNow = new Date();
    dateNow.setDate(dateNow.getDate() - 1);
    $("#" + buscador + ".dateOnly").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        // defaultDate: dateNow
    }).on('change', function () {
        filtrofecha(datatabledt,
            $('input', $(this)).attr("data-columna")
        );
    });
}
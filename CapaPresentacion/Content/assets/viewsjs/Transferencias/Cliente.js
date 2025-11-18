$(document).ready(function () {
   
    setCookie("datainicial", "");
    ListaClientes();
    $("#btnBuscar").on("click", function () {
        
        sincronizar();
    });


    $(document).on("click", ".cuentas", function () {
        var tipodoc = $(this).data("tipo");
        var nrodoc = $(this).data("nro");
        var nombre = $(this).data("nombre");
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/CuentasClienteJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ tipodoc, nrodoc ,id}),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "Transferencias/CuentasClienteJson", "BOTON CUENTAS");
                var registros = response.data;
                if (registros.length > 0) {
                    var datas = "<br><table style=' border:1px solid #000'>";
                    $.each(registros, function (index, value) {
                        datas += "<tr style=' border: 1px solid #000'><td style=' border: 1px solid #000'>" + value.Banco + "</td><td style=' border: 1px solid #000'> " + value.NroCuenta + "</td></tr>";
                    });
                    datas += "</table>";
                    $("#nom_cliente").html(nombre);
                    $("#cuenta_banco").html(datas);
                    $("#full-modal").modal("show");
                }

            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    VistaAuditoria("Transferencias/Clientes", "VISTA", 0, "", 3);

    $(document).on("click", "#btnExcel", function () {
        
        
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/ClienteExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "Transferencias/ClienteExcelJson", "BOTON EXCEL");
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }

            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });
});

function sincronizar() {

 
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Transferencias/Sincronizar",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            response = response.data;
            if (response) {
                ListaClientes();
            }
            else {
                toastr.error("Error, No se pudo Sincronizar.");
            }
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function ListaClientes() {
   
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Transferencias/ListaClientesJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#ResumenSala").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,
                columns: [

                    { data: "ClienteID", title: "ID" },
                    
                    { data: "ClienteTipoDoc", title: "Tipo Doc." },
                    { data: "ClienteNroDoc", title: "Nro. Doc" },
                    {
                        data: null, title: "Cliente", "render": function (value) {

                            return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                        }
                    },
                   
                    {
                        data: null, title: "Cuentas",
                        "render": function (value) {
                            var estado = value.ClienteID;
                            var butom = "";
                            var empleado = value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                            butom = '<button type="button" class="btn btn-xs btn-success cuentas" data-nombre="' + empleado+'" data-id="' + value.ClienteID + '" data-tipo="' + value.ClienteTipoDoc + '" data-nro="' + value.ClienteNroDoc+'"><i class="glyphicon glyphicon-pencil"></i> Cuentas</button> '
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
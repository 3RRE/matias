$(document).ready(function () {

    getSalas();

    $("#cboSala").change(function () {
        getDataTables();
        resetCorreos();
    });

    $('.btnAgregar').on('click', function (e) {

        var name = $(this).attr('id');
        var id = name.substr(name.length - 1);

        addCorreo(id);

    });

    $(document).on("click", ".btnEliminar", function () {

        var idSalaCorreo = $(this).data("id");

        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar este correo de este proceso?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {


                removeCorreo(idSalaCorreo);

            },

            cancel: function () {
                //close
            },

        });

    });


});

function getSalas() {
    var element = $('#cboSala')

    $.ajax({
        url: `${basePath}Sala/ListadoSalaActivasSinSeguridad`,
        type: "POST",
        contentType: "application/json",
        cache: false,
        beforeSend: function () {
            llenarSelect(basePath + "MIMaquinaInoperativa/ListarUsuarioCorreos", {}, "cboCorreos1", "CodUsuario", "NombreMail");
            llenarSelect(basePath + "MIMaquinaInoperativa/ListarUsuarioCorreos", {}, "cboCorreos2", "CodUsuario", "NombreMail");
            llenarSelect(basePath + "MIMaquinaInoperativa/ListarUsuarioCorreos", {}, "cboCorreos3", "CodUsuario", "NombreMail");
            llenarSelect(basePath + "MIMaquinaInoperativa/ListarUsuarioCorreos", {}, "cboCorreos4", "CodUsuario", "NombreMail");
            element.html(`<option value="0" data-name="">--Todas--</option>`)
            $.LoadingOverlay("show")
        },
        success: function (response) {
            var items = response.data
            items.map(item => {
                element.append(`<option value="${item.CodSala}" data-name="${item.Nombre}">${item.Nombre}</option>`)
            })
            getDataTables();
        },
        complete: function () {
            element.select2()

            $.LoadingOverlay("hide")
        }
    })
}


function getDataTables() {

    var codSala = $('#cboSala').val();

    $.ajax({
        url: `${basePath}MIMaquinaInoperativa/ListarCorreosxSala`,
        type: "POST",
        contentType: "application/json",
        cache: false,
        data: JSON.stringify({ codSala }),
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {

            if (response.respuesta) {
                DataTableUpdate(response.data1, "tableCorreos1");
                DataTableUpdate(response.data2, "tableCorreos2");
                DataTableUpdate(response.data3, "tableCorreos3");
                DataTableUpdate(response.data4, "tableCorreos4");
            } else {

                toastr.error(response.mensaje, "Mensaje Servidor");
            }

        },
        complete: function () {

            $.LoadingOverlay("hide")
        }
    })
}

function DataTableUpdate(array, dataTable) {

    dataTable = "#" + dataTable;

    objetodatatable = $(dataTable).DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: array
        ,
        columns: [
            { data: "UsuarioNombre", title: "Usuario" },
            { data: "Mail", title: "Correo" },
            {
                data: "CodSalaCorreos", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEliminar').tooltip({
                title: "Eliminar"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}

function addCorreo(codTipo) {

    cboCorreos = "#cboCorreos" + codTipo;
    codUsuario = $(cboCorreos).val();
    codSala = $("#cboSala").val();

    if (codUsuario != 0) {

        $.ajax({
            url: `${basePath}MIMaquinaInoperativa/AgregarCorreoSala`,
            type: "POST",
            contentType: "application/json",
            cache: false,
            data: JSON.stringify({ codSala, codUsuario, codTipo }),
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            success: function (response) {

                if (response.respuesta) {
                    toastr.success(response.mensaje, "Mensaje Servidor");
                    getDataTables();
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            },
            complete: function () {

                $.LoadingOverlay("hide")
            }
        })


    } else {
        toastr.error("Seleccione un correo");
    }

}


function removeCorreo(codSalaCorreos) {

    if (codSalaCorreos != 0) {

        $.ajax({
            url: `${basePath}MIMaquinaInoperativa/EliminarCorreoSala`,
            type: "POST",
            contentType: "application/json",
            cache: false,
            data: JSON.stringify({ codSalaCorreos }),
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            success: function (response) {

                if (response.respuesta) {
                    toastr.success(response.mensaje, "Mensaje Servidor");
                    getDataTables();
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            },
            complete: function () {

                $.LoadingOverlay("hide")
            }
        })


    } else {
        toastr.error("Seleccione un correo");
    }

}

function resetCorreos() {

    var correo = "#cboCorreos";

    for (var i = 1; i <= 4; i++) {
        $(correo + i).val(null);
    }

}
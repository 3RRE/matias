$(document).ready(function () {

    getSalas();

    $("#cboSala").change(function () {

        resetCorreos();

        if ($(this).val()==0) {
            DataTableUpdate([], "tableCorreos");
            return false;
        }

        getDataTables();
    });

    $('.btnAgregar').on('click', function (e) {

        if ($("#cboSala").val() == 0) {
            toastr.error("Seleccione una sala", "Mensaje Servidor");
            return false;
        } else {

            var correo = $("#txtCorreo").val();

            editCorreo(correo, 0)
        }


    });

    $(document).on("click", ".btnEliminar", function () {

        var idSalaCorreo = $(this).data("id");

        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar este correo?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                editCorreo("", idSalaCorreo)

            },

            cancel: function () {
                //close
            },

        });

    });

    DataTableUpdate([], "tableCorreos");

});

function getSalas() {
    var element = $('#cboSala')

    $.ajax({
        url: `${basePath}Sala/ListadoSalaActivasSinSeguridad`,
        type: "POST",
        contentType: "application/json",
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            var items = response.data
            items.map(item => {
                element.append(`<option value="${item.CodSala}" data-name="${item.Nombre}">${item.Nombre}</option>`)
            })
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
        url: `${basePath}Reclamacion/ListarCorreosxSala`,
        type: "POST",
        contentType: "application/json",
        cache: false,
        data: JSON.stringify({ codSala }),
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {

            if (response.respuesta) {
                DataTableUpdate(response.data, "tableCorreos");
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
        data: array,
        columns: [
            { data: "correo", title: "Correo" },
            {
                data: "id", title: "Accion",
                "render": function (o, type, oData) {

                    if (o != 0) {
                        return `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                    } else {
                        return ``;
                    }
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

function editCorreo(correo,codSplit) {

    codSala = $("#cboSala").val();

    if (correo != "" || codSplit!=0) {

        $.ajax({
            url: `${basePath}Reclamacion/EditarCorreoSala`,
            type: "POST",
            contentType: "application/json",
            cache: false,
            data: JSON.stringify({ codSala, correo, codSplit }),
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            success: function (response) {

                if (response.respuesta) {
                    toastr.success(response.mensaje, "Mensaje Servidor");
                    getDataTables();
                    resetCorreos();

                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            },
            complete: function () {

                $.LoadingOverlay("hide")
            }
        })


    } else {
        toastr.error("Ingrese un correo", "Mensaje Servidor");
    }

}



function resetCorreos() {

    var correo = "#txtCorreo";
    $(correo).val("");

}
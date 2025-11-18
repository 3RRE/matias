
$(document).ready(function () {



    var url = basePath + "Super/ActionNames";
    var data = { controllerName: "Empleado" };

    DataPostSend(url, data, true).done(function (response) {
        console.log(response);
        if (response) {
            if (response.respuesta) {
                var cantidad = response.respuesta;
                var cabeceras = response.cabeceras;
                $.each(cabeceras, function (key, value) {
                    // console.log(key)
                    if (key == 0) {
                        clase = "active";
                    } else {
                        clase = "";
                    }
                    $("#default-tabs-alt-1").append('<li class="' + clase + '"><a href="#' + value.WEB_PermControlador + '" data-toggle="tab">' + value.WEB_PermControlador.replace("Controller", "") + '</a></li>');
                    $("#contenidoTabs").append(' <div id="' + value.WEB_PermControlador + '" class="tab-pane ' + clase + '">' +
                        ' <table id="table' + value.WEB_PermControlador + '" class="table table-striped table-bordered table-hover table-condensed">' +
                        '<thead>' +
                        '<tr>' +
                        '<th width="15%">Funcion</th>' +
                        '<th width="15%">Nombre</th>' +
                        '<th>Descripcion</th>' +
                        '<th width="129px">Estado</th>' +
                        '</tr>' +
                        '</thead>' +
                        '<tbody id="body' + value.WEB_PermControlador + '"></tbody>' +
                        '</table>' +
                        '</div>');

                    $.each(cantidad, function (key2, value2) {
                        if (value2.WEB_PermControlador == value.WEB_PermControlador) {
                            var selectedOptions = "";
                            if (value2.WEB_PermEstado == 1) {
                                selectedOptions = '<option value="1" selected>Habilitado</option>' +
                                    '<option value="0">Deshabilitado</option>';
                            } else {
                                selectedOptions = '<option value="1">Habilitado</option>' +
                                    '<option value="0" selected>Deshabilitado</option>';
                            }
                            $("#body" + value.WEB_PermControlador).append('<tr>' +
                                '<td>' + value2.WEB_PermNombre + '</td>' +
                                '<td><input type="text" class="form-control input-sm inpNombre" data-id="' + value2.WEB_PermID + '" name="nombre" value="' + value2.WEB_PermNombreR + '" placeholder="Nombre"></td>' +
                                '<td><input type="text" class="form-control input-sm inpDescri" data-id="' + value2.WEB_PermID + '" name="descripcion" value="' + value2.WEB_PermDescripcion + '" placeholder="Descripcion"></td>' +
                                '<td><select class="form-control  input-sm selectPerm" data-id="' + value2.WEB_PermID + '">' + selectedOptions + '</select></td>' +
                                '</tr>');
                        }
                    });

                    $("#table" + value.WEB_PermControlador).on('page.dt', function () {
                        console.log('Page');
                        //$('table').css('height', '250px');
                        $('.dataTables_scrollBody').css('height', '250px');
                    }).DataTable({
                        "bDestroy": true,
                        "bSort": true,
                        "paging": true,
                        "scrollX": false,
                        "sScrollX": "100%",
                        "scrollCollapse": false,
                        "bProcessing": true,
                        "bDeferRender": true,
                        "autoWidth": true,
                        "bAutoWidth": false,
                        "lengthMenu": [[10, 50, 200, -1], [10, 50, 200, "All"]],
                        "pageLength": 10
                    });
                });




            } else {
                toastr.error("No se Conecto al Servidor", "Mensaje Servidor");
            }

        } else {
            toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
        }



    }).fail(function (x) {
        toastr.error("Error Fallo Servidor", "Mensaje Servidor");
    });

    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
        console.log("tabbbbbb")
        $('table').css('width', '100%');
        $('.dataTables_scrollHeadInner').css('width', '100%');
        $('.dataTables_scrollFootInner').css('width', '100%');

        //$($.fn.dataTable.tables(true)).DataTable()
        //   .columns.adjust();
    });

    //$(document).enterKey(".inpPermi",function () {
    //    var descripcion = $(this).val();
    //    var id = jQuery(this).data("id");
    //    var data = { permisoId: id, descripcion: descripcion }
    //    var url = basePath + "Permiso/ActualizarDescripcionPermiso";
    //    DataPostWithoutChange(url, data, false);

    //});

    $(document).on('keypress', "input.inpDescri", function (e) {
        if (e.which == 13) {
            var descripcion = $(this).val();
            var id = jQuery(this).data("id");
            var data = { permisoId: id, descripcion: descripcion }
            var url = basePath + "Super/ActualizarDescripcionPermiso";
            DataPostWithoutChange(url, data, false);
        }
    });

    $(document).on('keypress', "input.inpNombre", function (e) {
        if (e.which == 13) {
            var nombrer = $(this).val();
            var id = jQuery(this).data("id");
            var data = { permisoId: id, nombre: nombrer }
            var url = basePath + "Super/ActualizarNombrePermiso";
            DataPostWithoutChange(url, data, false);
        }
    });

    $(document).on('change', '.selectPerm', function (e) {
        var estado = $(this).val();
        var id = jQuery(this).data("id");
        var data = { permisoId: id, estado: estado }
        var url = basePath + "Super/ActualizarEstadoPermiso";
        DataPostWithoutChange(url, data, false);
    });


});
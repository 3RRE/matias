$(document).ready(function () {
    let canSearch = true;
    ListarCliente();
    $('#btnNuevoCliente').on('click', function (e) {
        window.location.replace(basePath + "AsistenciaCliente/RegistroCliente");
    });
    $(document).on('click', '.btnEditar', function (e) {
        let id = $(this).data("id");
        let url = basePath + "AsistenciaCliente/EditarCliente/" + id;
        window.location.replace(url);
    });
    $(document).on("click", "#btnExcel", function () {
        let dataPermiso={
            buscar:buscar,
            nroDoc: $("#txt_nrodoc").val()
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AsistenciaCliente/GetListadoClienteExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataPermiso),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;
                    let file = response.excelName;
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

    $(document).on("change", ".selectEstado", function () {
        let Id = $(this).data("id");
        let Estado = $(this).val();
        let dataForm={
            Estado:Estado,
            ID:Id
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea Cambiar el estado del Cliente?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'SI',
            cancelButton: 'NO',
            content: false,
            confirm: function () {
                $.ajax({
                    url: basePath + "AsistenciaCliente/CambiarEstadoCliente",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function () {
                    },
                    complete: function () {
                    },
                    success: function (response) {
                        if(response.respuesta){
                            toastr.success(response.mensaje,"Mensaje");
                        }else{
                            if(Estado=='A'){
                                $('.selectId'+Id).val('P');
                            }
                            else{
                                $('.selectId'+Id).val('A');
                            }
                            toastr.error(response.mensaje,"Mensaje");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        if(Estado=='A'){
                            $('.selectId'+Id).val('P');
                        }
                        else{
                            $('.selectId'+Id).val('A');
                        }
                    }
                });
            },
            cancel: function () {
                if(Estado=='A'){
                    $('.selectId'+Id).val('P');
                }
                else{
                    $('.selectId'+Id).val('A');
                }
                // $('.selectId'+Id).trigger('change');
            }
        });
    });
    $(document).on("change", ".selectADC", function () {
        let Id = $(this).data("id");
        let Asistencia = $(this).val();
        let dataForm = {
            Asistencia: Asistencia,
            ID: Id
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea Cambiar el estado de asistencia del Cliente?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'SI',
            cancelButton: 'NO',
            content: false,
            confirm: function () {
                $.ajax({
                    url: basePath + "AsistenciaCliente/CambiarAsistenciaDespuesCuarentenaCliente",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function () {
                    },
                    complete: function () {
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            toastr.success(response.mensaje, "Mensaje");
                        } else {
                            if (Asistencia == 1) {
                                $('.selectIdADC' + Id).val(0);
                            }
                            else {
                                $('.selectIdADC' + Id).val(1);
                            }
                            toastr.error(response.mensaje, "Mensaje");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        if (Asistencia == 1) {
                            $('.selectIdADC' + Id).val(0);
                        }
                        else {
                            $('.selectId' + Id).val(1);
                        }
                    }
                });
            },
            cancel: function () {
                if (Asistencia == 1) {
                    $('.selectIdADC' + Id).val(0);
                }
                else {
                    $('.selectIdADC' + Id).val(1);
                }
                // $('.selectId'+Id).trigger('change');
            }
        });
    });

    $(document).on('click', '.btnBuscar', function (e) {
        var busqueda = $("#txt_nrodoc").val();

        if (busqueda == "") {
            toastr.error("Ingrese numero de documento", "Mensaje Servidor");

        }
        if (busqueda.length >= 8) {
            ListarCliente();
        }
    });
})
function ListarCliente() {
    var buscar_ = buscar;
    console.log(buscar_);
    let url = basePath + "AsistenciaCliente/GetListadoCliente";
    let data = { nrodoc: $("#txt_nrodoc").val() };
    if(buscar_) {
        url = basePath + "AsistenciaCliente/GetListadoClienteNroDocumento";
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                let dataListado;
                let permiso;
                console.log(response);
                if (buscar_) {
                    dataListado = response.data;
                }
                else {
                    dataListado = response.data.dataListado;
                    permiso = response.data.dataPermiso;
                }

                objetodatatable = $("#tableCliente").DataTable({
                    "bDestroy": true,
                    "bSort": !buscar_,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "sScrollX": "100%",
                    "paging": !buscar_,
                    "autoWidth": false,
                    searching: !buscar_,

                    "bAutoWidth": true,
                    "bProcessing": true,
                    "bDeferRender": true,
                    order: [],
                    data: dataListado,
                    columns: [
                        // {
                        //     data:"SalaRegistro.Nombre", title: "Sala"
                        // },
                        // { 
                        //     data: null, title: "Ubi. Registro","render":function(value,type,oData,metadata){
                        //         return `${oData.UbigeoRegDepartamento}, ${oData.UbigeoRegProvincia},${oData.UbigeoRegDistrito}`
                        //     } 
                        // },
                        {
                            data: "Id", title: "Id"
                        },
                        {
                            data: null, title: "Nombres y Apellidos", "render": function (value, type, oData, metadata) {
                                if (oData.ApelPat != '') {
                                    return `${oData.Nombre} ${oData.ApelPat} ${oData.ApelMat}`
                                }
                                else {
                                    return `${oData.NombreCompleto}`
                                }
                            }
                        },
                        {
                            data: "TipoDocumento.Nombre", title: "Tipo Doc."
                        },
                        {
                            data: "NroDoc", title: "Nro. Doc."
                        },
                        // { 
                        //     data: null, title: "Ubi. Procedencia","render":function(value,type,oData,metadata){
                        //         return `${oData.UbigeoProcDepartamento}, ${oData.UbigeoProcProvincia},${oData.UbigeoProcDistrito}`
                        //     } 
                        // },
                        {
                            data: "Edad", title: "Edad"
                        },
                        {
                            data: null, title: "Género", "render": function (value, type, oData, metadata) {
                                let genero = "";
                                if (oData.Genero == "M") {
                                    genero = "MASCULINO"
                                } else {
                                    genero = "FEMENINO"
                                }
                                return genero
                            }
                        },
                        {
                            data: "Celular1", title: "Celular 1"
                        },
                        //{   
                        //    data: "Celular2", title: "Celular 2"
                        //},
                        {
                            data: "Mail", title: "Mail"
                        },
                        {
                            data: null, title: "Cumpleaños", "render": function (value, type, oData, metadata) {
                                return moment(oData.FechaNacimiento).format('DD/MM/YYYY')
                            }
                        },
                        {
                            data: null, title: "Asist. Desp. de Cuarent.", "render": function (value, type, oData, metadata) {
                                let select = `<select class="selectADC selectIdADC${oData.Id}" data-id=${oData.Id} style="width:100%">`;
                                if (oData.AsistioDespuesCuarentena == 1) {
                                    select += `<option value=1 selected>SI</option><option value=0 >NO</option>`;
                                }
                                else {
                                    select += `<option value=1>SI</option><option value=0 selected>NO</option>`;
                                }
                                select += `</select>`;
                                return select;
                                // let span="";
                                // if(oData.AsistioDespuesCuarentena==1){
                                //     span = "SI"
                                // }
                                // else{
                                //     span="NO"
                                // }
                                // return span
                            }
                        },
                        // {   
                        //     data: "TipoJuego.Nombre", title: "TipoJuego",
                        // },
                        // {   
                        //     data: "TipoFrecuencia.Nombre", title: "TipoFrecuencia",
                        // },
                        {
                            data: null,
                            title: "Estado",
                            "render": function (value, type, oData, meta) {
                                let span = "";
                                if (permiso) {
                                    let select = `<select class="selectEstado selectId${oData.Id}" data-id=${oData.Id} style="width:100%">`;
                                    if (oData.Estado == 'A') {
                                        select += `<option value="A" selected>Activo</option><option value="P">Pendiente</option>`;
                                    }
                                    else {
                                        select += `<option value="A">Activo</option><option value="P" selected>Pendiente</option>`;
                                    }
                                    select += `</select>`;
                                    span = select;
                                }
                                else {
                                    if (oData.Estado == "A") {
                                        span = "ACTIVO"
                                    }
                                    else {
                                        span = "PENDIENTE"
                                    }
                                }
                                return span;
                            }
                        }
                        ,
                        {
                            data: null,
                            title: "ACCIONES",
                            "bSortable": false,
                            "render": function (value, type, oData, meta) {
                                let botones = '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + oData.Id + '"><i class="glyphicon glyphicon-pencil"></i> Editar</button> ';
                                return botones;
                            }
                        }
                    ],
                    "initComplete": function (settings, json) {
                        // $(".selectEstado").select2();
                    },
                    "drawCallback": function (settings) {
                    }
                });

                $('#tableCliente tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });

            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                console.log("errorrrrrrrr");
            }
        });
    }
    else {
        objetodatatable = $("#tableCliente").DataTable({
            language: {
                searchPlaceholder: "Presione Enter para buscar"
            },
            "bDestroy": true,
            "bSort": !buscar_,
            "ordering": true,
            "scrollCollapse": true,
            "scrollX": true,
            "paging": !buscar_,
            "aaSorting": [],
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            //data: response.data,
            "serverSide": true,
            "searching": { regex: !buscar_ },
            "processing": true,
            "ajax": {
                //url: basePath + "AsistenciaCliente/GetListadoCliente",
                url: `${basePath}/AsistenciaCliente/GetListadoClienteJson`,
                type: "POST",
                data: function (data) {
                    data.calle = "werty";
                    return JSON.stringify(data);
                },
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                    canSearch = true;
                },
                dataFilter: function (response) {
                    // this to see what exactly is being sent back
                    var datos = jQuery.parseJSON(response);
                    permiso = datos.dataPermiso;
                    console.log("aq", datos.dataPermiso);
                    return response;
                },
                dataType: "json",
                processData: false,
                contentType: "application/json;charset=UTF-8"
            },
            columns: [

                {
                    data: "Id", title: "Id"
                },
                {
                    data: "cliente.Nombre", title: "Nombres y Apellidos", "render": function (value, type, oData, metadata) {
                        if (oData.ApelPat != '') {
                            return `${oData.Nombre} ${oData.ApelPat} ${oData.ApelMat}`
                        }
                        else {
                            return `${oData.NombreCompleto}`
                        }
                    }
                },
                {
                    data: "TipoDocumento.Nombre", title: "Tipo Doc."
                },
                {
                    data: "cliente.NroDoc", title: "Nro. Doc.", "render": function (value, type, oData, metadata) {
                        return oData.NroDoc
                    }
                },
                {
                    data: "Edad", title: "Edad"
                },
                {
                    data: "cliente.Genero", title: "Género", "render": function (value, type, oData, metadata) {
                        let genero = "";
                        if (oData.Genero == "M") {
                            genero = "MASCULINO"
                        } else {
                            genero = "FEMENINO"
                        }
                        return genero
                    }
                },
                {
                    data: "cliente.Celular1", title: "Celular 1", "render": function (value, type, oData, metadata) {
                        return oData.Celular1
                    }
                },
                {
                    data: "cliente.Mail", title: "Mail", "render": function (value, type, oData, metadata) {
                        return oData.Mail
                    }
                },
                {
                    data: "cliente.FechaNacimiento", title: "Cumpleaños", "render": function (value, type, oData, metadata) {
                        return moment(oData.FechaNacimiento).format('DD/MM/YYYY')
                    }
                },
                {
                    data: "cliente.AsistioDespuesCuarentena", title: "Asist. Desp. de Cuarent.", "render": function (value, type, oData, metadata) {
                        let select = `<select class="selectADC selectIdADC${oData.Id}" data-id=${oData.Id} style="width:100%">`;
                        if (oData.AsistioDespuesCuarentena == 1) {
                            select += `<option value=1 selected>SI</option><option value=0 >NO</option>`;
                        }
                        else {
                            select += `<option value=1>SI</option><option value=0 selected>NO</option>`;
                        }
                        select += `</select>`;
                        return select;

                    }
                },

                {
                    data: "cliente.Estado",
                    title: "Estado",
                    "render": function (value, type, oData, meta) {
                        let span = "";
                        if (permiso) {
                            let select = `<select class="selectEstado selectId${oData.Id}" data-id=${oData.Id} style="width:100%">`;
                            if (oData.Estado == 'A') {
                                select += `<option value="A" selected>Activo</option><option value="P">Pendiente</option>`;
                            }
                            else if (oData.Estado == 'P') {
                                select += `<option value="A">Activo</option><option value="P" selected>Pendiente</option>`;
                            } else {
                                select += `<option value="">------</option><option value="A">Activo</option><option value="P">Pendiente</option>`;
                            }
                            select += `</select>`;
                            span = select;
                        }
                        else {
                            if (oData.Estado == "A") {
                                span = "ACTIVO"
                            }
                            else if (oData.Estado == "P") {
                                span = "PENDIENTE"
                            } else {
                                span = "SIN ESTADO"
                            }
                        }
                        return span;
                    }
                },
                {
                    data: null,
                    title: "ACCIONES",
                    "bSortable": false,
                    "render": function (value, type, oData, meta) {
                        let botones = '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + oData.Id + '"><i class="glyphicon glyphicon-pencil"></i> Editar</button> ';
                        return botones;
                    }
                }
            ],
            "drawCallback": function (settings) {
                $('.btnEditar').tooltip({
                    title: "Editar"
                });
            },
            "fnDrawCallback": function (oSettings) {

            },
            "initComplete": function (settings, json) {

            },
        });
        $('#tableCliente_filter input').css('width', '175px');
        $('#tableCliente_filter input').unbind().bind('keyup', function (e) {
            let search = this.value;
            if (e.keyCode === 13 && canSearch) {
                canSearch = false;
                objetodatatable.search(search).draw();
            }
        });
        $('#tableCliente tbody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
        });
    }
}

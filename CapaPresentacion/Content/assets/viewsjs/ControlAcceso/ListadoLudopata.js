
$(document).ready(function () {
    let canSearch = true;
    $("#FechaInscripcion").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    ListarLudopata();
    ObtenerListaTipoExcluison();
    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textLudopata").text("Editar");
        $("#divContacto").hide()
        $(".mySelectLudopata").html('')
        $(".mySelectLudopata").append('<option value="">---Seleccione---</option>');
        $(".mySelectLudopata").select2({
            multiple: false, placeholder: "--Seleccione--",dropdownParent:$("#full-modal_ludopata")
        })
        $(".mySelectLudopata2").select2({
            multiple: false, placeholder: "--Seleccione--",dropdownParent:$("#full-modal_ludopata")
        })
        ObtenerRegistro(id);
    });

    VistaAuditoria("CALLudopata/ListadoLudopata", "VISTA", 0, "", 3);

    $(document).on('click','#btnNuevo', function (e) {
        LimpiarFormValidator();
        $("#textLudopata").text("Nuevo");
        $("#LudopataID").val(0)
        $("#Nombre").val("")
        $("#ApellidoPaterno").val("")
        $("#ApellidoMaterno").val("")
        $("#DNI").val("")
        $("#Telefono").val("")
        $("#CodRegistro").val("")
        $("#FechaInscripcion").val(moment($.now()).format('DD/MM/YYYY'))
        $(".mySelectLudopata").html('')
        $(".mySelectLudopata").append('<option value="">---Seleccione---</option>');
        $(".mySelectLudopata").select2({
            multiple: false, placeholder: "--Seleccione--",dropdownParent:$("#full-modal_ludopata")
        })
        //$(".mySelectLudopata2").html('')
        //$(".mySelectLudopata2").append('<option value="">---Seleccione---</option>');
        $(".mySelectLudopata2").select2({
            multiple: false, placeholder: "--Seleccione--",dropdownParent:$("#full-modal_ludopata")
        })
        $(".mySelectLudopata2").val(null).trigger("change");
        ObtenerDataSelects()
    });

    $(document).on("click", ".btnEliminar", function () {
        let elemento = $(this);
        let id = $(this).data("id");
        let ContactoID = $(this).data('contactoid')
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Ludopata?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALLudopata/LudopataEliminarJson",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id, ContactoID }),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            ListarLudopata();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                });

            },

            cancel: function () {
                //close
            },

        });



    });

    $("#form_registro_ludopata")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Nombre: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ApellidoPaterno: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DNI: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                TipoExclusion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                TipoDoiID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Estado: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },


            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });
    $("#form_registro_contacto")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                NombreContacto: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ApellidoPaternoContacto: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                CelularContacto: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },


            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });

    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CALLudopata/LudopataDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });
    $(document).on('click','.btnContacto',function (e){
        e.preventDefault()
        let ContactoID= $(this).data('id')
        $("#textContacto").text("Editar");
        ObtenerContacto(ContactoID);

    })
    $(document).on('click','.btnGuardarContacto',function (e){
        e.preventDefault()
        let validarContacto = $("#form_registro_contacto").data('bootstrapValidator').validate();
        if(validarContacto.isValid()){
            var dataForm = $('#form_registro_contacto').serializeFormJSON();
            let url = basePath + 'CALContacto/UpdateContacto'
            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta) {
                        $("#full-modal_contacto").modal("hide");
                        //$("#btnBuscar").click();
                        toastr.success(response.mensaje, "Mensaje Servidor");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Error Servidor", "Mensaje Servidor");
                }
            });
        }
    })
    $(document).on('click','.btnGuardarLudopata',function (e){
        e.preventDefault();
        $("#form_registro_ludopata").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_ludopata").data('bootstrapValidator').validate()
        if(validar.isValid()){
            let LudopataID=$("#LudopataID").val()
            let url=basePath
            if(LudopataID==0){
                url +="CALLudopata/LudopataGuardarJson"
            }else{
                url +="CALLudopata/LudopataEditarJson"
            }
            let dataForm = new FormData(document.getElementById("form_registro_ludopata"))
            $.ajax({
                url: url,
                type: "POST",
                method: "POST",
                contentType: false,
                data: dataForm,
                cache: false,
                processData: false,
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if(response.respuesta){
                        // $("form input,select,textarea").val(""); 
                        LimpiarFormValidator();
                        toastr.success(response.mensaje,"Mensaje Servidor")
                        setTimeout(function(){
                            window.location.reload()
                        },2000)
                    }else{
                        toastr.error(response.mensaje,"Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                }
            });
        }

    })
    $(document).on('change','#cboDepartamento',function(e){
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        ObtenerListaProvincias(DepartamentoID);
    })
    $(document).on('change','#cboProvincia',function(e){
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        let ProvinciaID = $("#cboProvincia option:selected").val();
        ObtenerListaDistritos(DepartamentoID,ProvinciaID)
    })
});

// function ListarLudopata2() {
//     var url = basePath + "Ludopata/ListarLudopataJson";
//     var data = {}; var respuesta = "";
//     $.ajax({
//         url: url,
//         type: "POST",
//         contentType: "application/json",
//         data: JSON.stringify(data),
//         beforeSend: function () {
//             $.LoadingOverlay("show");
//         },
//         complete: function () {
//             $.LoadingOverlay("hide");
//         },
//         success: function (response) {
//             respuesta = response.data
//             objetodatatable = $("#tableLudopata").DataTable({
//                 "bDestroy": true,
//                 "bSort": true,
//                 "ordering": true,
//                 "scrollCollapse": true,
//                 "scrollX": true,
//                 "paging": true,
//                 "aaSorting": [],
//                 "autoWidth": false,
//                 "bProcessing": true,
//                 "bDeferRender": true,
//                 data: response.data,
//                 columns: [
//                     //{ data: "LudopataID", title: "ID" },

//                     { data: "NombreCompleto", title: "Nombre Completo" },
//                     { data: "DOINombre", title: "Tipo DOI" },
//                     { data: "DNI", title: "DOI" },
//                     { data: "CodRegistro", title: "Codigo Registro" },
//                     {
//                         data: "FechaInscripcion", title: "Fecha Inscripcion",
//                         "render": function (o) {
//                             return moment(o).format("DD/MM/YYYY");
//                         } },
//                     //{ data: "Nombre", title: "Nombre" },
//                     //{ data: "ApellidoPaterno", title: "Apellido Paterno" },
//                     //{ data: "ApellidoMaterno", title: "Apellido Materno" },
//                     //{ data: "TipoExclusion", title: "Tipo Exclusion" },
//                     {
//                         data: "Foto", title: "Foto"/*
//                         ,"render":
//                             function (value) {
//                                 var ruta;
//                                 if (value.length !== 0) {

//                                     ruta = basePath + "Content/img/profile/standard/" + value + '?';

//                                 } else {
//                                     ruta = basePath + "Content/img/profile/standard/default.jpg?";
//                                 }
//                                 var mylink = '<img src="' + ruta + new Date().getTime() + '" width="40" height="40"  alt="' + value + '"/>';
//                                 return mylink;
//                             }*/
//                     },
//                     //{ data: "ContactoID", title: "ContactoID" },
//                     //{ data: "Telefono", title: "Telefono" },
//                     {
//                         data: "Estado", title: "Estado",
//                         "render": function (o) {
//                             var estado = "INACTIVO";
//                             var css = "btn-danger";
//                             if (o == 1) {
//                                 estado = "ACTIVO"
//                                 css = "btn-success";
//                             }
//                             return '<span class="label ' + css + '">' + estado + '</span>';

//                         }
//                     },
//                     //{ data: "Imagen", title: "Imagen" },
//                     //{ data: "TipoDoiID", title: "TipoDoiID" },
//                     //{ data: "CodUbigeo", title: "CodUbigeo" },
//                     /*{
//                         data: "FechaRegistro", title: "Fecha Registro",
//                         "render": function (o) {
//                             return moment(o).format("DD/MM/YYYY hh:mm");
//                         }
//                     },*/
//                     {
//                         data: "ContactoID", title: "Contacto",
//                         "bSortable": false,
//                         "render": function (o,value,oData) {
//                             return `<button type="button" class="btn btn-xs btn-info btnContacto" data-id="${o}" data-ludopataid="${oData.LudopataID}"><i class="glyphicon glyphicon-user"></i></button>`;
//                         },
//                         className: "text-center"
//                     },
//                     {
//                         data: "LudopataID", title: "Acción",
//                         "bSortable": false,
//                         "render": function (o) {
//                             return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
//                                     <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
//                         },
//                         className:"text-center"
//                     }
//                 ],
//                 "drawCallback": function (settings) {
//                     $('.btnEditar').tooltip({
//                         title: "Editar"
//                     });
//                 },

//                 "initComplete": function (settings, json) {



//                 },
//             });
//             $('.btnEditar').tooltip({
//                 title: "Editar"
//             });

//         },
//         error: function (xmlHttpRequest, textStatus, errorThrow) {

//         }
//     });
// };


function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALLudopata/ListarLudopataIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ LudopataID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if(response.respuesta){
                let data = response.data;
                console.log(data)
                $("#LudopataID").val(data.LudopataID);
                $("#LContactoID").val(data.ContactoID);
                $("#Nombre").val(data.Nombre);
                $("#ApellidoPaterno").val(data.ApellidoPaterno);
                $("#ApellidoMaterno").val(data.ApellidoMaterno);
                $("#DNI").val(data.DNI);
                $("#Telefono").val(data.Telefono);
                $("#CodRegistro").val(data.CodRegistro);
                $("#FechaInscripcion").val(moment(data.FechaInscripcion).format('DD/MM/YYYY'));
                $("#File").val('');
                $("#File").text(data.Foto);
                $("#Foto").val(data.Foto);
                $("#Imagen").val(data.Imagen);
                ObtenerDataSelectConUbigeo(data);
                $("#full-modal_ludopata").modal("show");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};
function ObtenerContacto(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALContacto/GetContactoByID",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ContactoID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje, "Mensaje Servidor")
                let data = response.data;
                $("#ContactoID").val(data.ContactoID)
                $("#NombreContacto").val(data.NombreContacto)
                $("#ApellidoPaternoContacto").val(data.ApellidoPaternoContacto)
                $("#ApellidoMaternoContacto").val(data.ApellidoMaternoContacto)
                $("#TelefonoContacto").val(data.TelefonoContacto)
                $("#CelularContacto").val(data.CelularContacto)
                $("#full-modal_contacto").modal("show")

            }
            else{
                toastr.error(response.mensaje, "Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};
function ListarLudopata() {
    objetodatatableLudopatas = $("#tableLudopata").DataTable({
        language: {
            searchPlaceholder: "Presione Enter para buscar"
        },
        "bDestroy": true,
        "bSort": true,
        "ordering": true,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": true,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        //data: response.data,
        "serverSide": true,
        "searching": { regex: true },
        "processing": true,
        "ajax": {
            url: basePath + "CALLudopata/ListarLudopataServerJson",
            type: "POST",
            dataType: "json",
            processData: false,
            contentType: "application/json;charset=UTF-8",
            data: function (data) {
                return JSON.stringify(data);
            },
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
                canSearch = true;
            },
        },
        columns: [
            { data: "LudopataID", title: "ID" },

            { data: "NombreCompleto", title: "Nombre Completo" },
            { data: "DOINombre", title: "Tipo DOI" },
            { data: "DNI", title: "DOI",className: "text-right", },
            { data: "CodRegistro", title: "Codigo Registro" ,className: "text-right",},
            {
                data: "FechaInscripcion", title: "Fecha Inscripcion",
                "render": function (o) {
                    return moment(o).format("DD/MM/YYYY");
                },
                className: "text-right",
            },
            //{ data: "Nombre", title: "Nombre" },
            //{ data: "ApellidoPaterno", title: "Apellido Paterno" },
            //{ data: "ApellidoMaterno", title: "Apellido Materno" },
            //{ data: "TipoExclusion", title: "Tipo Exclusion" },
            /*{
                data: "Imagen", title: "Foto"
                ,"render":
                    function (value) {
                        let myLink=` <img src="data:image/png;base64, ${value}"  width="40" height="40" alt="Ludópata" />`
                        return myLink;
                },
                "orderable": false,
                className: "text-center",
            },*/
            {
                data: "Foto", title: "Foto"
                , "render":
                    function (value) {
                        let myLink = ` <img src="${value}"  width="40" height="40" alt="Ludópata" />`
                        return myLink;
                    },
                "orderable": false,
                className: "text-center",
            },
            //{ data: "ContactoID", title: "ContactoID" },
            //{ data: "Telefono", title: "Telefono" },
            {
                data: "Estado", title: "Estado",
                "render": function (o) {
                    var estado = "INACTIVO";
                    var css = "btn-danger";
                    if (o == 1) {
                        estado = "ACTIVO"
                        css = "btn-success";
                    }
                    return '<span class="label ' + css + '">' + estado + '</span>';

                },
                className: "text-center",
            },
            //{ data: "Imagen", title: "Imagen" },
            //{ data: "TipoDoiID", title: "TipoDoiID" },
            //{ data: "CodUbigeo", title: "CodUbigeo" },
            /*{
                data: "FechaRegistro", title: "Fecha Registro",
                "render": function (o) {
                    return moment(o).format("DD/MM/YYYY hh:mm");
                }
            },*/
            {
                data: "ContactoID", title: "Contacto",
                "bSortable": false,
                "render": function (o,value,oData) {
                    return `<button style="width:40px;heigth:40px;" type="button" class="btn btn-xs btn-info btnContacto" data-id="${o}" data-ludopataid="${oData.LudopataID}"><i class="glyphicon glyphicon-user" ></i></button>`;
                },
                className: "text-center",
                "orderable": false,
            },
            {
                data: "LudopataID", title: "Acción",
                "bSortable": false,
                "render": function (o,value,oData) {
                    return `<button style="width:40px;heigth:40px;" type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                            <button style="width:40px;heigth:40px;" type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}" data-contactoid="${oData.ContactoID}"><i class="glyphicon glyphicon-remove"></i></button> `;
                },
                className:"text-center",
                "orderable": false,
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
    $('#tableLudopata_filter input').css('width', '175px');
    $('#tableLudopata_filter input').unbind().bind('keyup', function (e) {
        let search = this.value;
        if (e.keyCode === 13 && canSearch) {
            canSearch = false;
            objetodatatableLudopatas.search(search).draw();
        }
    });
}
function ObtenerListaProvincias(DepartamentoID) {
    if(DepartamentoID){
        let dataForm={ DepartamentoID:DepartamentoID}
        $.ajax({
            type: "POST",
            url: basePath + "AsistenciaCliente/GetListadoProvincia",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboProvincia,#cboDistrito").html("");
                $("#cboProvincia,#cboDistrito").append('<option value="">---Seleccione---</option>');
                if(result.respuesta){
                    var datos = result.data;
                    $.each(datos, function (index, value) {
                        $("#cboProvincia").append('<option value="' + value.ProvinciaId + '">' + value.Nombre + '</option>');
                    });
                }
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
        return false;
    }

}
function ObtenerListaDistritos(DepartamentoID,ProvinciaID) {
    if(DepartamentoID&&ProvinciaID){
        let dataForm={ DepartamentoID:DepartamentoID,ProvinciaID:ProvinciaID}
        $.ajax({
            type: "POST",
            url: basePath + "AsistenciaCliente/GetListadoDistrito",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboDistrito").html("");
                $("#cboDistrito").append('<option value="">---Seleccione---</option>');
                if(result.respuesta){
                    var datos = result.data;
                    $.each(datos, function (index, value) {
                        $("#cboDistrito").append('<option value="' + value.CodUbigeo + '">' + value.Nombre + '</option>');
                    });
                }
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
        return false;
    }

}
function ObtenerDataSelects(){
    $.ajax({
        type: "POST",
        url: basePath + "CALLudopata/ObtenerDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if(result.respuesta){
                let dataUbigeo=result.data.dataUbigeo;
                let dataTipoDocumento=result.data.dataTipoDocumento;
                if(dataUbigeo.length>0){
                    $.each(dataUbigeo, function (index, value) {
                        $("#cboDepartamento").append('<option value="' + value.DepartamentoId + '"  >' + value.Nombre + '</option>');
                    });
                }
                if(dataTipoDocumento.length>0){
                    $.each(dataTipoDocumento, function (index, value) {
                        $("#cboTipoDoiID").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });
                }
                $("#divContacto").show()
                $("#full-modal_ludopata").modal("show");
            }
            else{
                toastr.error(result.mensaje,"Mensaje Servidor");
            }
            // $(".mySelect").select2({
            //     multiple: false,placeholder: "--Seleccione--"
            // });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function ObtenerDataSelectConUbigeo(ludopata){
    let ubigeo=ludopata.Ubigeo
    let dataForm={ ubigeo:ubigeo}
    $.ajax({
        type: "POST",
        url: basePath + "CALLudopata/ObtenerDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if(result.respuesta){
                let dataUbigeo=result.data.dataUbigeo;
                let dataTipoDocumento=result.data.dataTipoDocumento;
                let dataProvincias=result.data.dataProvincias;
                let dataDistritos=result.data.dataDistritos;
                if(dataUbigeo.length>0){
                    $.each(dataUbigeo, function (index, value) {
                        $("#cboDepartamento").append('<option value="' + value.DepartamentoId + '"  >' + value.Nombre + '</option>');
                    });
                    if(dataProvincias&&dataDistritos){
                        $.each(dataProvincias, function (index, value) {
                            $("#cboProvincia").append('<option value="' + value.ProvinciaId + '">' + value.Nombre + '</option>');
                        });
                        $.each(dataDistritos, function (index, value) {
                            $("#cboDistrito").append('<option value="' + value.CodUbigeo + '"  >' + value.Nombre + '</option>');
                        });
                    }
                }
                if(dataTipoDocumento.length>0){
                    $.each(dataTipoDocumento, function (index, value) {
                        $("#cboTipoDoiID").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });
                }
            }
            else{
                toastr.error(result.mensaje,"Mensaje Servidor");
            }
            console.log(ubigeo.DepartamentoId)
            if(ubigeo.DepartamentoId!=0){
                $("#cboDepartamento").val(ubigeo.DepartamentoId);
                $("#cboProvincia").val(ubigeo.ProvinciaId);
                $("#cboDistrito").val(ubigeo.CodUbigeo);
            }
            else{
                $("#cboDepartamento").val("");
            }
            $("#cboTipoDoiID").val(ludopata.TipoDoiID)
            $("#cboEstado").val(ludopata.Estado).trigger("change")
            $("#cboTipoExclusion").val(ludopata.TipoExclusion).trigger("change")
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}


function ObtenerListaTipoExcluison() {
    $.ajax({
        type: "POST",
        url: basePath + "CALTipoExclusion/ListarTipoExclusionJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboTipoExclusion").append('<option value="' + value.TipoExclusionID + '"  >' + value.Descripcion + '</option>');
            });
            $("#cboTipoExclusion").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_ludopata')

            });
            $("#cboTipoExclusion").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}


function LimpiarFormValidator() {
    $("#form_registro_ludopata").parent().find('div').removeClass("has-error");
    $("#form_registro_ludopata").parent().find('i').removeAttr("style").hide();
}
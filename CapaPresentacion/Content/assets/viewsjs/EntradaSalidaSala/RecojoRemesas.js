
//19-11 js
let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let IdRecojoRemesa = 0
let idIngresoSeleccionado = null;
let params = {}

$(document).ready(function () {
    //ObtenerListaSalas()
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    })

    let urlParams = new URLSearchParams(window.location.search);
    let codsala = urlParams.get('codsala');
    let ider = urlParams.get('id');
    let fechaini = urlParams.get('fechaInicio');
    let fechafin = urlParams.get('fechaFin');

    if (codsala) {
        ObtenerListaSalas(codsala)
        if (fechaini) {
            $("#fechaInicio").val(fechaini);
        }

        if (fechafin) {
            $("#fechaFin").val(fechafin);
        }
        params = { CodSala: codsala, fechaInicio: fechaini, fechaFin: fechafin, id: ider };
        buscarRecojoRemesa(params);

    } else {
        ObtenerListaSalas()

    }
    obtenerTiposDocumentos()
    // BUSCAR ----------------------------------------------------------
    $(document).on("click", "#btnBuscar", function () {
        if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
            toastr.error("Seleccione una sala.")
            return false
        }
        if ($("#fechaInicio").val() === "") {
            toastr.error("Ingrese una fecha de Inicio.")
            return false
        }
        if ($("#fechaFin").val() === "") {
            toastr.error("Ingrese una fecha Fin.")
            return false
        }
        buscarRecojoRemesa()
    });
    $('#PlacaRodaje').on('input', function () {
        $(this).val($(this).val().toUpperCase());
    }); $('#codigopersonal').on('input', function () {
        $(this).val($(this).val().toUpperCase());
    });
    // NUEVO ------------------------------------------------------------
    $(document).on('click', "#btnNuevo", function () {
        $('#textRecojoRemesa').text('Nuevo');
        $('#IdRecojoRemesa').val(0);
        IdRecojoRemesa = 0;

        // Limpiar los campos de entrada
        $('#cboSalaRecojoRemesa').val(null);
        $('#FechaSolucion').val(null);
        $('#CodigoPersonal').val(null);
        $('#OtroEstadoFotocheck').val(null);
        $('#PlacaRodaje').val(null);
        
        $('#FechaIngreso').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#FechaIngreso").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        });
      
        $('#cboEstadoFotocheck').val(null);
        $('#Observaciones').val("");
      
        $('#charCount').text(0 + '/' + 300);
      
        buscaobtenerListaEstadoFotocheck();
        obtenerListaEmpleadosPorEstado();
        renderSelectSalasModalRecojoRemesa(true);
        
        $('#full-modal_recojoremesas').modal('show');
        limpiarValidadorFormRecojoRemesa();

    });
    // Excel Plantilla ------------------------------------------------------------
    $(document).on('click', '#btnExcel_Plantilla', function (e) {
        e.preventDefault()

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "RecojoRemesas/PlantillaRecojoRemesasDescargarExcel",
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
    })

    //____EXCEL___________________________________________
    $(document).on('click', '#btnExcel', function (e) {
        e.preventDefault()
        if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
            toastr.error("Seleccione una sala.")
            return false
        }
        if ($("#fechaInicio").val() === "") {
            toastr.error("Ingrese una fecha de Inicio.")
            return false
        }
        if ($("#fechaFin").val() === "") {
            toastr.error("Ingrese una fecha Fin.")
            return false
        }
        let listasala = $("#cboSala").val();
        let fechaini = $("#fechaInicio").val();
        let fechafin = $("#fechaFin").val();
        let dataForm = {
            codsala: listasala,
            fechaini, fechafin
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "RecojoRemesas/ReporteRecojoRemesasDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
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
    })

    $('#cboTipoDocumento').on('change', function () {
        const tipo = $(this).val();
        const $docInput = $('#DocumentoRegistro');

        if (tipo === "1") {
            
            $docInput.data('regex', /^\d+$/); 
            $docInput.data('maxlength', 8); 
        } else if (tipo === "2") {
            
            $docInput.data('regex', /^[a-zA-Z0-9]+$/); 
            $docInput.data('maxlength', 12); 
        } else {
            
            $docInput.data('regex', /^[a-zA-Z0-9]+$/); 
            $docInput.data('maxlength', 20); 
        }

        $docInput.val(''); 
    });

    
    $('#DocumentoRegistro').on('input', function () {
        const regex = $(this).data('regex'); 
        const maxlength = $(this).data('maxlength'); 
        const value = $(this).val();

        
        if (!regex.test(value)) {
            $(this).val(value.slice(0, -1)); 
        }

        
        if (value.length > maxlength) {
            $(this).val(value.slice(0, maxlength)); 
        }
    });

    //---NUEVO CONTINUANDO-----------------------------------
    $("#full-modal_recojoremesas").on("shown.bs.modal", function (e) {
         
        //$("#FechaRegistro").datetimepicker({
        //    pickTime: false,
        //    format: 'DD/MM/YYYY HH:mm A',
        //    defaultDate: dateNow,
        //    pickTime: true
        //})
        $("#FechaSalida").datetimepicker({
            pickDate: true,
            format: 'DD/MM/YYYY hh:mm A',
            pickTime: true
        })
        limpiarValidadorFormRecojoRemesa()
    }) 
    // NUEVO VALIDACION ------------------------------------------------------
    $("#RecojoRemesaForm")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                IdPersonal: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                CodSala: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                }, 
                PlacaRodaje: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdEstadoFotocheck: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                OtroEstadoFotocheck: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                FechaIngreso: {
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
    $("#form_registro_personal")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                IdTipoDocumentoRegistro: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DocumentoRegistro: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Nombres: {
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
                ApellidoMaterno: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                CodigoPersonal: {
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

    $(document).on('click', '#btnGuardarLogOcurrencia', function (e) {

        e.preventDefault()

        $("#RecojoRemesaForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#RecojoRemesaForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {

            let url = basePath
            if (IdRecojoRemesa === 0) {
                url += "RecojoRemesas/GuardarRecojoRemesas"
            } else {
                url += "RecojoRemesas/EditarRecojoRemesas"

            }
            let dataForm = new FormData(document.getElementById("RecojoRemesaForm"))
            dataForm.delete('IdRecojoRemesa');
            dataForm.append('IdRecojoRemesa', IdRecojoRemesa);
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
                    if (response.respuesta) {
                        limpiarValidadorFormRecojoRemesa();
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        $("#full-modal_recojoremesas").modal("hide");
                        buscarRecojoRemesa();

                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Ocurrió un error al guardar el registro.", "Error");
                }
            });
        }
    })
    $(document).on('change', '#cboEstadoFotocheck', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#estadoFotocheckOtros").show()
            $('#RecojoRemesaForm').bootstrapValidator('enableFieldValidators', 'OtroEstadoFotocheck', true);
            $('#RecojoRemesaForm').bootstrapValidator('updateMessage', 'OtroEstadoFotocheck', 'notEmpty', '');

        }
        else {
            $("#estadoFotocheckOtros").hide()
            $('#OtroEstadoFotocheck').val('')
            $('#RecojoRemesaForm').bootstrapValidator('enableFieldValidators', 'OtroEstadoFotocheck', false);

        }
    })
    function buscaobtenerListaEstadoFotocheck(value) {
        $.ajax({
            type: "POST",
            url: basePath + "RecojoRemesas/ListarEstadoFotocheck",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboEstadoFotocheck").html('')
                if (result.data) {
                    $("#cboEstadoFotocheck").html(result.data.map(item => `<option value="${item.IdEstadoFotocheck}">${item.Nombre}</option>`).join(""))
                    $('#cboEstadoFotocheck').append('<option value="-1">OTROS</option>')
                    $('#cboEstadoFotocheck').select2({
                        placeholder: "--Seleccione--",
                        dropdownParent: $('#full-modal_recojoremesas')
                    })
                    //$("#cboEstadoFotocheck").select2({
                    //    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_recojoremesas')
                    //});
                    if (value) {
                        $("#cboEstadoFotocheck").val(value).trigger("change");
                    } else {
                        $("#cboEstadoFotocheck").val(null).trigger("change");

                    }
                    limpiarValidadorFormRecojoRemesa();
                }
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor")
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
        });
        return false;
    }
    function obtenerListaEmpleadosPorEstado(value) {
        $.ajax({
            type: "POST",
            url: basePath + "RecojoRemesas/ListarRecojoRemesaPersonal",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboPersonal").html('')
                if (result.data) {
                    const dataOrdenada = result.data.sort((a, b) => b.IdRecojoRemesaPersonal - a.IdRecojoRemesaPersonal);

                    $("#cboPersonal").html(dataOrdenada.map(item => `
                    <option value="${item.IdRecojoRemesaPersonal}" data-name="${item.Nombres} ${item.ApellidoPaterno} ${item.ApellidoMaterno}" data-dni="${item.IdTipoDocumentoRegistro == 2 ? "CARNET EXT." : item.TipoDocumentoRegistro} ${item.DocumentoRegistro}" data-codigo="${item.CodigoPersonal}">${item.Nombres} ${item.ApellidoPaterno} ${item.ApellidoMaterno} ${item.DocumentoRegistro}</option>
                `).join(""));
                
                    if (value) {
                        $('#cboPersonal').val(value);
                    } else {
                        $("#cboPersonal").val(null).trigger("change");
                    }

                    // Inicializar Select2 con un estilo personalizado
                    $("#cboPersonal").select2({
                        dropdownParent: $('#full-modal_recojoremesas'),
                        templateResult: formatOption2,
                        placeholder: "--Seleccione--",
                        templateSelection: function (data) {
                            if (data.id === '') {
                                return '--Seleccione--';
                            }

                            return formatSelection2(data);
                        }
                    });
                }
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor")
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
                limpiarValidadorFormRecojoRemesa();
            }
        });
        return false;
    }
    
    function formatOption2(option) {
        if (!option.id) { return option.text; }
        var $option = $(option.element);
        var name = $option.data('name').toString();
        var dni = $option.data('dni').toString().toUpperCase();
        var codigo = $option.data('codigo').toString().toUpperCase();
        
        return $(
            `<div style="display: flex; flex-direction: column; gap: 1.8rem; font-family: Arial, sans-serif;">
                <span style="font-size: 13px;">${name}  -  ${dni}</span>
                <span style="font-size: 11px;">CÓDIGO: ${codigo}</span>
            </div>`
        );
    }
    
    function formatSelection2(option) {
        var $option = $(option.element);
        var name = $option.data('name') + '  -  ' + $option.data('dni');
        return name.toString();
    }

    $('.btnGuardarPersonal').on('click', function (e) {
        $("#form_registro_personal").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_personal").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#persona_entidad_publica_id").val();
            var urlenvio = basePath + "RecojoRemesas/GuardarRecojoRemesasPersonal";

            var dataForm = $('#form_registro_personal').serializeFormJSON();
            dataForm.Estado = 1;
            $.ajax({
                url: urlenvio,
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
                        $('#persona_entidad_publica_id').val("0");
                        $('#nombre').val("");
                        $('#estado').val("0");
                        $("#full-modal_personal").modal("hide");
                        LimpiarFormValidator();
                        obtenerListaEmpleadosPorEstado(response._id) 
                        toastr.success("Personal Guardado", "Mensaje Servidor");
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

    });
    
    $('#Observaciones').on('input', function () {
        var maxLength = $(this).attr('maxlength');
        var currentLength = $(this).val().length;
        var remaining = maxLength - currentLength;
        $('#charCount').text(currentLength + '/' + maxLength);
    });
    $(document).on("click", ".btnEditar", function () {

        limpiarValidadorFormRecojoRemesa();

        IdRecojoRemesa = $(this).data("id");

        //carga los datos
        let rowData = objetodatatable
            .rows()
            .data()
            .toArray()
            .find(row => row.IdRecojoRemesa === IdRecojoRemesa);

        if (rowData) {
            $('#textRecojoRemesa').text('Editar');
            $('#full-modal_recojoremesas').modal('show');

            renderSelectSalasModalRecojoRemesa(rowData.CodSala)
            obtenerListaEmpleadosPorEstado(rowData.IdPersonal)
            buscaobtenerListaEstadoFotocheck(rowData.IdEstadoFotocheck);
            $("#FechaIngreso").datetimepicker({
                format: 'DD/MM/YYYY hh:mm A',
                defaultDate: moment(),
                pickTime: true
            }); 
            $('#FechaIngreso').val((moment(rowData.FechaIngreso).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaIngreso).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaIngreso).format('DD/MM/YYYY hh:mm A'));
            $('#FechaSalida').val((moment(rowData.FechaSalida).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaSalida).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaSalida).format('DD/MM/YYYY hh:mm A'));

            $('#IdPersonal').val(rowData.IdPersonal);
            $('#CodigoPersonal').val(rowData.CodigoPersonal);
            $('#IdEstadoFotocheck').val(rowData.IdEstadoFotocheck);
            $('#OtroEstadoFotocheck').val(rowData.OtroEstadoFotocheck);
            $('#PlacaRodaje').val(rowData.PlacaRodaje);
            $('#Observaciones').val(rowData.Observaciones);
            var currentLength = $('#Observaciones').val().length;
            var maxLength = $('#Observaciones').attr('maxlength');
            $('#charCount').text(currentLength + '/' + maxLength);
            

        } else {
            toastr.error("No se encontraron datos para este registro.", "Error");
        }
    });
    
  
    
});
 
function renderSelectSalasModalRecojoRemesa(value) {
    $("#cboSalaRecojoRemesa").html('class="form-control input-sm"')
    if (arraySalas) {
        $("#cboSalaRecojoRemesa").html(arraySalas.map(item => `'class="form-control input-sm"' <option value="${item.CodSala}">${item.Nombre}</option>`).join(""))

        if (value) {
            $('#cboSalaRecojoRemesa').val(value);
        } else {
            $("#cboSalaRecojoRemesa").val(null).trigger("change");
        }
        $("#cboSalaRecojoRemesa").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_recojoremesas')
        }); 
    }

}
function ObtenerListaSalas(value) {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboSala").html('')
            const codSalaArray = result.data.map(sala => String(sala.CodSala));

            if (result.data) {
                arraySalas = result.data
                arraySelect = [`<option value="${-1}" selected>TODOS</option>`]
                result.data.map(item => arraySelect.push(`<option value="${item.CodSala}">${item.Nombre}</option>`))
                $("#cboSala").html(arraySelect.join(""))
                $("#cboSala").select2({
                    multiple: true, placeholder: "--Seleccione--", allowClear: true
                });
                if (value) {
                    $('#cboSala').val(value).trigger("change");
                } else {
                    $("#cboSala").val(-1).trigger("change")
                }
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}

function buscarRecojoRemesa(params) {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla");

    if (params) {
        listasala = params.CodSala;
        fechaini = params.fechaInicio;
        fechafin = params.fechaFin;
    }

    $.ajax({
        type: "POST",
        url: basePath + "RecojoRemesas/ListarRecojoRemesasxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let datos = response.data || [];
            $(addtabla).empty();
            $(addtabla).append('<table id="tableResultado" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            let highlightId = params?.id; // ID del registro que debe resaltarse

            objetodatatable = $("#tableResultado").DataTable({
                destroy: true,
                sort: true,
                scrollCollapse: true,
                scrollX: true,
                paging: true,
                autoWidth: true,
                aaSorting: [[0, 'desc']],

                data: datos,
                columns: [
                    { data: "IdRecojoRemesa", title: "ID" },
                    { data: "Sala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fec. Registro",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "EstadoFotocheck",
                        title: "E.Fotocheck",
                        render: function (data, type, row) {
                            if (row.EstadoFotocheck && row.EstadoFotocheck.trim() !== "") {
                                return row.EstadoFotocheck;
                            } else {
                                if (row.IdEstadoFotocheck == -1) {
                                    return row.OtroEstadoFotocheck.length > 27 ? row.OtroEstadoFotocheck.substring(0, 27) + "..." : row.OtroEstadoFotocheck;
                                } else {
                                    return "";
                                }
                            }
                        }
                    },
                    { data: "NombreCompletoPersonal", title: "Personal" },
                    { data: "PlacaRodaje", title: "Placa/Rodaje" },
                    {
                        data: "FechaIngreso",
                        title: "Fec. Ingreso",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Fec. Salida",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "IdRecojoRemesa",
                        title: "Accion",
                        "render": function (o, value, oData) {
                            return `
                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}">
                                <i class="glyphicon glyphicon-pencil"></i>
                            </button> 
                           
                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-danger btnEliminar"  data-id="${o}">
                                <i class="glyphicon glyphicon-remove"></i>
                            </button>`
                        },
                        className: "text-center",
                        "orderable": false
                    }
                ],
                "drawCallback": function (settings) {
                    if (highlightId) {
                        let row = $(`#tableResultado tbody tr td:first-child:contains(${highlightId})`).parent();
                        row.addClass("highlight-row");
                    }
                    $('.btnEditar').tooltip({
                        title: "Editar Log Ocurrencia"
                    });
                    $('.btnEliminar').tooltip({
                        title: "Eliminar Log Ocurrencia"
                    });
                },
            });
        },
        error: function (request, status, error) {
            toastr.error("Error en la solicitud.", "Mensaje Servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar un registro de Recojo de Remesas?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteRecojoRemesa(id)
        }
    });
});

const deleteRecojoRemesa = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}RecojoRemesas/EliminarRecojoRemesas`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.respuesta) {
                toastr.error(response.mensaje, "Mensaje Servidor");
                return;
            }
            toastr.success(response.mensaje, "Mensaje Servidor");
            buscarRecojoRemesa()
        }
    });
}
 
//VALIDAR-----------------------------------------
function limpiarValidadorFormRecojoRemesa() {
    $("#RecojoRemesaForm").parent().find('div').removeClass("has-error");
    $("#RecojoRemesaForm").parent().find('i').removeAttr("style").hide();
}

function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "RecojoRemesas/ListarCargo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaCargos(result.data)
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}
function renderTablaCargos(data) {
    let element = $('#contenedorListadoCargos')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdCargo}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarCargo" data-id="${item.IdCargo}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoMotivos">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue un cargo</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoCargos").DataTable()
    } 
}

$(document).on('click', '#btnGuardarNuevoEmpleado', function (e) {
    e.preventDefault()
    let dataForm = {
        IdTipoDocumentoRegistro: $('#cboTipoDocumento').val(),
        DocumentoRegistro: $('#NuevoEmpleadoDocumentoRegistro').val(),
        Nombre: $('#NuevoEmpleadoNombre').val(),
        ApellidoMaterno: $('#NuevoEmpleadoApellidoMaterno').val(),
        ApellidoPaterno: $('#NuevoEmpleadoApellidoPaterno').val(),
        IdCargo: $('#NuevoEmpleadocbCargo').val(),
        Estado: 1,
    }
    $.ajax({
        url: `${basePath}RecojoRemesas/InsertarEmpleado`,
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
                $("#cboTipoDocumento").val(1)
                $("#NuevoEmpleadoDocumentoRegistro").val("")
                $("#NuevoEmpleadoNombre").val('')
                $("#NuevoEmpleadoApellidoMaterno").val('')
                $("#NuevoEmpleadoApellidoPaterno").val('')

                toastr.success(response.mensaje, "Mensaje Servidor");
                $('#full-modal_nuevoempleado').modal('hide');
                obtenerListaEmpleadosPorEstado();
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    });
})
   
$('#agregarPersonal').tooltip({
    title: "Agregar Personal"
});
$('#eliminarfechasalida').tooltip({
    title: "Limpiar Fecha Salida"
});
$('#labelEstadoFotocheck').tooltip({
    title: "Estado Fotocheck"
});
$(document).on('click', '#eliminarfechasalida', function (e) {
    e.preventDefault()
    $('#FechaSalida').val('')
})
$(document).on("click", "#agregarPersonal", function () {
    LimpiarFormValidator();
    var id = $(this).data("id");
    $('#cboTipoDocumento').val(1).trigger("change") 
    $('#DocumentoRegistro').val('')
    $('#Nombres').val('')
    $('#ApellidoPaterno').val('')
    $('#ApellidoMaterno').val('')
    $('#codigopersonal').val('')
    $("#textPersonal").text("Nuevo");
    $("#full-modal_personal").modal("show");
});
function LimpiarFormValidator() {
    $("#form_registro_personal").parent().find('div').removeClass("has-error");
    $("#form_registro_personal").parent().find('div').removeClass("has-success");
    $("#form_registro_personal").parent().find('i').removeAttr("style").hide();

}
function obtenerTiposDocumentos() {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetListadoTipoDocumento",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboTipoDocumento").html('')
            if (result.data) {
                $("#cboTipoDocumento").html(result.data.map(item => `<option value="${item.Id}">${item.Nombre}</option>`).join(""))
                $("#cboTipoDocumento").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_personal')
                });
                $("#cboTipoDocumento").val(null).trigger("change")
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}


$(document).ready(function () {
    $("#btnExcel_Importar").click(function (e) {
        e.preventDefault();
        $("#fileInput").click();
    });
    $("#fileInput").change(function () {
        let file = this.files[0];

        if (file) {
            let formData = new FormData();
            formData.append("file", file);
            formData.append("fileName", file.name);

            $.ajax({
                url: basePath + "RecojoRemesas/ImportarExcelRecojoRemesas",
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.respuesta) {
                        if (response.observaciones.length > 0) {
                            console.log("Observaciones:", response.observaciones);
                        }
                        if (response.excelModificado) {
                            let link = document.createElement("a");
                            link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + response.excelModificado;
                            link.download = response.nombreArchivo;
                            link.click();
                        }
                    }
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                },
                complete: function (resul) {
                    $("#fileInput").val("");
                }
            });
        }
    });
});

//19-11 js
let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let IdBienMaterial = 0   
let CboSeleccionado4 = 0
let idBienMaterialSeleccionado = null;
let empresaSeleccionada = null
let shouldTriggerChange = true
let params = {}



$(document).ready(function () {
    // initAutocomplete()
    //ObtenerListaSalas()
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    })

    $('#cbo_TipoBienMaterial').on('change', function () {
        if ($(this).val() === '1') {
            $('#btnNuevoEmpleado').prop('disabled', true).css({
                'background-color': '#ccc',
                'color': '#666',
                'cursor': 'not-allowed',
                'opacity': '0.7'
            });

            $('#agregarEmpresa').prop('disabled', true).css({
                'background-color': '#ccc',
                'color': '#666',
                'cursor': 'not-allowed',
                'opacity': '0.7'
            });

            initAutocomplete()
        } else {
            $('#btnNuevoEmpleado').prop('disabled', false).css({
                'background-color': '',
                'color': '',
                'cursor': '',
                'opacity': ''
            });
            $('#agregarEmpresa').prop('disabled', false).css({
                'background-color': '',
                'color': '',
                'cursor': '',
                'opacity': ''
            });
            initAutocompleteExterno()

        }
    })
    $(document).on('click','#descargarArchivo',function(e){
        e.preventDefault()
        let href=$(this).attr('href')
        window.open(href,'_blank');
    })

    let urlParams = new URLSearchParams(window.location.search);
    let codsala = urlParams.get('codsala');
    let ider = urlParams.get('id');
    let fechaini = urlParams.get('fechaInicio');
    let fechafin = urlParams.get('fechaFin');
    let idtipo = -1;

    if (codsala) {
        ObtenerListaSalas(codsala)
        if (fechaini) {
            $("#fechaInicio").val(fechaini);
        }

        if (fechafin) {
            $("#fechaFin").val(fechafin);
        }
        params = { CodSala: codsala, fechaInicio: fechaini, fechaFin: fechafin, id: ider, idtipobienmaterial: idtipo};
        buscarBienesMateriales(params);

    } else {
        ObtenerListaSalas()

    }

    // BUSCAR ----------------------------------------------------------
    $(document).on("click", "#btnBuscar", function () {
        if ($("#cboSala").length == 0 || $("#cboSala").val()==null ){
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
        buscarBienesMateriales()
    });
    $(document).on('change','#cboEmpresa',function(e){
        if (!shouldTriggerChange) {
            shouldTriggerChange = true;
            return;
        }
        e.preventDefault()
        empresaSeleccionada = $(this).val() 
        renderTablaEmpleadosSeleccionados(true)
        $('#busqueda').val('')
    }) 
    $(document).on('change', '#NuevoEmpleadoIdTipoDocumento', function (e) {
  
        empresaSeleccionada = $(this).val() 
    }) 

    $(document).on('click','#btnExcel',function(e){
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
        let idtipobienmaterial = $("#cboTipoBien").val();
        let fechaini = $("#fechaInicio").val();
        let fechafin = $("#fechaFin").val();
        let dataForm = {
            codsala :listasala,
            fechaini,fechafin,idtipobienmaterial
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "BienesMateriales/ReporteBienesMaterialesDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
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
    // Excel Plantilla ------------------------------------------------------------
    $(document).on('click','#btnExcel_Plantilla',function(e){
        e.preventDefault() 
        
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "BienesMateriales/PlantillaBienesMaterialesDescargarExcel",
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
    // NUEVO ------------------------------------------------------------
    $(document).on('click', "#btnNuevo", function (e) {
        $('#cboEmpresa').prop('disabled', false);

        IdBienMaterial = 0   
        $('#textBienMaterial').text('Nuevo')
        $('#IdBienMaterial').val(0)
        $('#cboSalaBienMaterial').val(null)
        $('#cboCategoria').val(null)
        $('#Descripcion').val('')
        $('#cboMotivo').val(null)
        $('#cboEmpresa').val(null)
          
        $('#GRRFT').val('')
        $('#busqueda').val('')

        $('#DescripcionCategoria').val('')
        $('#RutaImagen').val('')
        $('#descargarArchivo').attr('href', '#');
        $('#descargarArchivo').attr('data-ruta','') 
        $('#descargarArchivo').hide();
        $('#descargarArchivo').text('');
        $("#ArchivoExiste").hide()  
         
        $('#RutaImagen').val('');
        $('#FechaIngreso').val(moment().format('DD/MM/YYYY hh:mm A'));  
        $("#FechaIngreso").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',  
            defaultDate: moment(),         
            pickTime: true                 
        });
        $('#FechaSalida').val('')
        $('#Observaciones').val('')
        arrayEmpleadosSeleccionados = []
        renderTablaEmpleadosSeleccionados(); 
        $('#full-modal_bienmaterial').modal('show') 
        $("#cbo_TipoBienMaterial").val(null).trigger('change');
        obtenerListaEmpresas();

        renderSelectSalasModalBienMaterial(true)
        obtenerListaCategoriasPorEstado(true)
        obtenerListaMotivosPorEstado()
        rendercboEmpresa();

        $("#cbo_TipoBienMaterial").on('change', function () {
        const tipoBienMaterial = $('#cbo_TipoBienMaterial').val();
            if (tipoBienMaterial === '1') {
                $('#cboEmpresa').prop('disabled', false); 
                obtenerListaEmpresas();
            } else if (tipoBienMaterial === '2') {
                $('#cboEmpresa').prop('disabled', false);
                obtenerListaEmpresaExternaPorEstado();
            } else { 
                rendercboEmpresa();
            }
        })

        /*    obtenerListaCargosPorEstado();*/
        var maxLength = $('#Descripcion').attr('maxlength'); 
        $('#charCountDescripcion').text(0 + '/' + 300);
        var maxLengthObservaciones = $('#Observaciones').attr('maxlength'); 
        $('#charCountObservaciones').text(0 + '/' + 300);
       
    });

    $('#Descripcion').on('input', function () {
        var maxLengthDescripcion = $(this).attr('maxlength');
        var currentLengthDescripcion = $(this).val().length;
        $('#charCountDescripcion').text(currentLengthDescripcion + '/' + maxLengthDescripcion);
    });
    $('#Observaciones').on('input', function () {
        var maxLengthObservaciones = $(this).attr('maxlength');
        var currentLengthDescripcion = $(this).val().length;
        $('#charCountObservaciones').text(currentLengthDescripcion + '/' + maxLengthObservaciones);
});
    //---NUEVO CONTINUANDO-----------------------------------

    $("#full-modal_bienmaterial").on("shown.bs.modal", function (e) {
        $("#cbo_TipoBienMaterial").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
        })
        $("#FechaSalida").datetimepicker({
            pickDate: true,
            format: 'DD/MM/YYYY HH:mm A',
            pickTime: true
        })
        $('#RutaImagen').val(null);

        if ($('#cbo_TipoBienMaterial').val() == '1') {
            $('#btnNuevoEmpleado').hide();
            $('#agregarEmpresa').hide();
            initAutocomplete();
        } else if ($('#cbo_TipoBienMaterial').val() == '2') {
            $('#btnNuevoEmpleado').show();
            $('#agregarEmpresa').show();
            initAutocompleteExterno();
        }
    })

    $('#full-modal_bienmaterial').on('hidden.bs.modal', function () {
        $("#busqueda").autocomplete("destroy");
    })
    $(document).on('change', '#cboCategoria', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#categoriaOtros").show();
            $('#bienMaterialForm').bootstrapValidator('enableFieldValidators', 'DescripcionCategoria', true); 
            $('#bienMaterialForm').bootstrapValidator('updateMessage', 'DescripcionCategoria', 'notEmpty', '');

        }
        else {
            $("#categoriaOtros").hide();
            $('#DescripcionCategoria').val('')
            $('#bienMaterialForm').bootstrapValidator('enableFieldValidators', 'DescripcionCategoria', false);
        }
    })

    $('#formCategoria').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            CategoriaNombre: {
                validators: {
                    notEmpty: {
                        message: 'El nombre es obligatorio'
                    }
                }
            },
            CategoriaEstado: {
                validators: {
                    notEmpty: {
                        message: 'El estado es obligatorio'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    $('#formMotivo').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            MotivoNombre: {
                validators: {
                    notEmpty: {
                        message: 'El nombre es obligatorio'
                    }
                }
            },
            MotivoEstado: {
                validators: {
                    notEmpty: {
                        message: 'El estado es obligatorio'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    $('#formCargo').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: { 
            CargoNombre: {
                validators: {
                    notEmpty: {
                        message: 'El nombre es obligatorio'
                    }
                }
            },
            CargoEstado: {
                validators: {
                    notEmpty: {
                        message: 'El estado es obligatorio'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    $('#formEmpresa').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            EmpresaNombre: {
                validators: {
                    notEmpty: {
                        message: 'El nombre es obligatorio'
                    }
                }
            },
            EmpresaEstado: {
                validators: {
                    notEmpty: {
                        message: 'El estado es obligatorio'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    $('#formNuevoEmpleado').bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            NuevoEmpleadoIdTipoDocumento: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            NuevoEmpleadoDocumentoRegistro: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            CargoEmpleado: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            NuevoEmpleadoNombre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            }, NuevoEmpleadoApellidoPaterno: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            }, NuevoEmpleadoApellidoMaterno: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    $(document).on('click', '#eliminarfechasalida', function (e) {
        e.preventDefault()
        $('#FechaSalida').val('') 
    })
    $("#bienMaterialForm")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                TipoBienMaterial: {
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
                IdCategoria: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdMotivo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdEmpresa: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                GRFFT: {
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
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DescripcionCategoria: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                }

            }
        }).on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });
    $(document).on('click', '#btnGuardarBienMaterial', function (e) {
         
        e.preventDefault()  
        $("#bienMaterialForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#bienMaterialForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {
            
      
            let url = basePath
            if (IdBienMaterial === 0) {
                url += "BienesMateriales/GuardarBienesMateriales"
            } else {
                url += "BienesMateriales/EditarBienesMateriales" 
                
            } 
            let dataForm = new FormData(document.getElementById("bienMaterialForm"))

            dataForm.delete('IdBienMaterial'); 
            dataForm.append('IdBienMaterial', IdBienMaterial);

            dataForm.append('NombreEmpresa', $('#cboEmpresa option:selected').text())
            dataForm.append('NombreCategoria', $('#cboCategoria option:selected').text())
            dataForm.append('NombreMotivo', $('#cboMotivo option:selected').text())
            dataForm.append('NombreSala', $('#cboSalaBienMaterial option:selected').text()) 

            dataForm.append('Empleados', JSON.stringify(arrayEmpleadosSeleccionados))
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
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        $("#full-modal_bienmaterial").modal("hide");
                        buscarBienesMateriales();
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


    $("#imagenBienMaterial").change(function () {
        readURL(this);
    });

    $(document).on("click", ".btnEditarbien", function () {

        let url = basePath + 'BienesMateriales/ObtenerPorIdBienesMateriales'
         IdBienMaterial = $(this).data("id");  
         let dataForm = {
            id:IdBienMaterial
         }
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
                    let rowData = response.data 
                    $('#cboEmpresa').prop('disabled', false);

                    $('#textBienMaterial').text('Editar'); 
                    $('#full-modal_bienmaterial').modal('show');

                    $('#cbo_TipoBienMaterial').val(rowData.TipoBienMaterial);
                    if (rowData.TipoBienMaterial == '1') {
                        arrayEmpleadosSeleccionados = rowData.Empleados
                        renderTablaEmpleadosSeleccionados();
                        obtenerListaEmpresas(rowData.IdEmpresa)
                    } else if (rowData.TipoBienMaterial == '2') {
                        arrayEmpleadosSeleccionados = rowData.Empleados
                        renderTablaEmpleadoExternoSeleccionados();
                        obtenerListaEmpresaExternaPorEstado(rowData.IdEmpresa)
                    }

                    renderSelectSalasModalBienMaterial(rowData.CodSala)
                    obtenerListaCategoriasPorEstado(rowData.IdCategoria) 
                    //$('#cbo_TipoBienMaterial').val(rowData.TipoBienMaterial);
                    obtenerListaMotivosPorEstado(rowData.IdMotivo)
                    //obtenerListaEmpresas(rowData.IdEmpresa)
                    //renderTablaEmpleadosSeleccionados();  
                    //renderTablaEmpleadoExternoSeleccionados();  
                    $('#Descripcion').val(rowData.Descripcion); 
                    $('#DescripcionCategoria').val(rowData.DescripcionCategoria); 
                    var currentLength = $('#Descripcion').val().length;
                    var maxLength = $('#Descripcion').attr('maxlength');
                    $('#charCountDescripcion').text(currentLength + '/' + maxLength);
                    $('#GRRFT').val(rowData.GRRFT);
                    $('#FechaIngreso').val(moment(rowData.FechaIngreso).format('DD/MM/YYYY HH:mm A'));  
                    $('#FechaSalida').val((moment(rowData.FechaSalida).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaSalida).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaSalida).format('DD/MM/YYYY HH:mm A'));
                    $('#busqueda').val('')

                    $('#Observaciones').val(rowData.Observaciones);
                    var currentLengthObservaciones = $('#Observaciones').val().length;
                    var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
                    $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);
                    if (rowData.RutaImagen) {
                        var timestamp = new Date().getTime();
                        var rutaImagenUrl = rowData.RutaImagen+ '?timestamp=' + new Date().getTime(); 
                        $('#descargarArchivo').attr('href', rutaImagenUrl);

                        // $('#descargarArchivo').attr('target', '_blank');
                        var fileName = rowData.RutaImagen.split("/").pop(); 
                        $('#descargarArchivo').text('Ver ' + fileName);  
                        $('#descargarArchivo').attr('data-ruta',rutaImagenUrl) 

                        $('#descargarArchivo').show();
                        $("#ArchivoExiste").show() 
                    } else {
                        $('#descargarArchivo').hide();
                        $("#ArchivoExiste").hide() 
                    }
                   
                    // if(rowData.RutaImagen){
                    //     $('#mostrarDescarga').show()
                    //     $('#NombreDescarga').val(rowData.RutaImagen)
                    // }
                    //toastr.success(response.mensaje, "Mensaje Servidor");
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        });
    }); 
    // $('#descargarArchivo').on('click', function () {
        
    //     window.open($(this).attr('href'), '_blank');  
    // });
    $('#RutaImagen').on('change', function (e) {
        var file = e.target.files[0]; 

        if (file) {
            var url = URL.createObjectURL(file);
            var fileName = file.name;
            var maxLength = 30; 
            var fileNameDisplay = fileName;

            if (fileName.length > maxLength) {
                var partLength = Math.floor((maxLength - 3) / 2); 
                fileNameDisplay = fileName.substring(0, partLength) + '...' + fileName.substring(fileName.length - partLength);
            }
            $('#descargarArchivo').attr('data-ruta', url);
            $('#descargarArchivo').attr('href', url);
            $('#descargarArchivo').text('Ver ' + fileNameDisplay);  
            $('#descargarArchivo').show(); 
            $("#ArchivoExiste").show();

        }
    });
    $(document).on('click', '#btnAgregarEmpleadoBienMaterial', function (e) {
        e.preventDefault()
        $('#full-modal_empleadotabla').modal('show')
    })


    $("#full-modal_empleadotabla").on("shown.bs.modal", function () {
        obtenerListaEmpleadosPorEstado()
    })
    $(document).on('click', '.seleccionarEmpleado', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let elementoSeleccionado = arrayEmpleados.find(x => x.IdBuk == idSeleccionado) 
        if (elementoSeleccionado) {
            arrayEmpleadosSeleccionados.push({
                IdEmpleado: elementoSeleccionado.IdBuk,
                Nombre: elementoSeleccionado.Nombres,
                ApellidoPaterno: elementoSeleccionado.ApellidoPaterno,
                ApellidoMaterno: elementoSeleccionado.ApellidoMaterno,
                NombreDocumentoRegistro: elementoSeleccionado.TipoDocumento,
                DocumentoRegistro: elementoSeleccionado.NumeroDocumento,
                Cargo: elementoSeleccionado.Cargo,
                IdCargo:elementoSeleccionado.IdCargo,
                Empresa:elementoSeleccionado.Empresa,
                IdEmpresa:elementoSeleccionado.IdEmpresa
            })
        }
        renderTablaEmpleadosSeleccionados()
    })
  
    $(document).on('click', '.editarCargo', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#CargoIdCargo").val(idSeleccionado)
        $("#CargoNombre").val(nombre)
        $("#CargoEstado").val(estado)
        $("#textoCargo").text("Editar ")
    })
    $(document).on('click', '#btnCancelarCargo', function (e) {
        e.preventDefault()
        $("#CargoIdCargo").val(0)
        $("#CargoNombre").val('')
        $("#CargoEstado").val(1)
        $("#textoCargo").text("Agregar ")
    })

    $(document).on('click', '.editarEmpresa', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#EmpresaIdEmpresa").val(idSeleccionado)
        $("#EmpresaNombre").val(nombre)
        $("#EmpresaEstado").val(estado)
        $("#textoEmpresa").text("Editar ")
    })
    $(document).on('click', '#btnCancelarEmpresa', function (e) {
        e.preventDefault()
        $("#EmpresaIdEmpresa").val(0)
        $("#EmpresaNombre").val('')
        $("#EmpresaEstado").val(1)
        $("#textoEmpresa").text("Agregar ")
    })
    $(document).on('click','.quitarEmpleado',function(e){
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        arrayEmpleadosSeleccionados = arrayEmpleadosSeleccionados.filter(x => x.IdEmpleado != idSeleccionado)

        if ($('#cbo_TipoBienMaterial').val() === '2') {
            renderTablaEmpleadoExternoSeleccionados();
        } else {
            renderTablaEmpleadosSeleccionados();
        }

    })
    // MODAL CATEGORIAS ------------------------------------------------------------
    $(document).on('click', '#agregarCategoria', function (e) {
        e.preventDefault()
        limpiarValidadorFormCategoria()
        $("#full-modal_categorias").modal('show')
    })
    $("#full-modal_categorias").on("shown.bs.modal", function () {
        $("#CategoriaNombre").val('')
        limpiarValidadorFormCategoria()
        obtenerListaCategorias()
    })
    $(document).on('click', '.editarCategoria', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#CategoriaIdCategoria").val(idSeleccionado)
        $("#CategoriaNombre").val(nombre)
        $("#CategoriaEstado").val(estado)
        $("#textoCategoria").text("Editar ")
    })
    $(document).on('click', '#btnCancelarCategoria', function (e) {
        e.preventDefault()
        $("#CategoriaIdCategoria").val(0)
        $("#CategoriaNombre").val('')
        $("#CategoriaEstado").val(1)
        $("#textoCategoria").text("Agregar ")
    })
    $(document).on('click', '#btnGuardarEditarCategoria', function (e) {
        e.preventDefault()
        let dataForm = {
            IdCategoria: $('#CategoriaIdCategoria').val(),
            Nombre: $('#CategoriaNombre').val(),
            Estado: $('#CategoriaEstado').val(),
        }
        $("#formCategoria").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#formCategoria").data('bootstrapValidator').validate();
        if (validarRegistro.isValid()) {

            let url = `${basePath}BienesMateriales/InsertarCategoria`
            if ($('#CategoriaIdCategoria').val() > 0) {
                url = `${basePath}BienesMateriales/EditarCategoria`
            }

            let dataForm4 = new FormData(document.getElementById("formCategoria"))


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
                        $("#CategoriaIdCategoria").val(0)
                        $("#CategoriaNombre").val('')
                        $("#CategoriaEstado").val(1)
                        $("#textoCategoria").text("Agregar")
                        obtenerListaCategorias()
                        obtenerListaCategoriasPorEstado()
                        limpiarValidadorFormCategoria()
                        $("#full-modal_categorias").modal('hide')
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


    //MODAL FORMULARIO MOTIVO ----------------------------------
    $(document).on('click', '#agregarMotivo', function (e) {
        e.preventDefault()
        limpiarValidadorFormMotivo()
        $("#full-modal_motivos").modal('show')
    })
    $("#full-modal_motivos").on("shown.bs.modal", function () {
        //obtenerListaCategorias()
        $("#MotivoNombre").val('')
        limpiarValidadorFormMotivo()
        obtenerListaMotivos()
    })
    $(document).on('click', '.editarMotivo', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#MotivoIdMotivo").val(idSeleccionado)
        $("#MotivoNombre").val(nombre)
        $("#MotivoEstado").val(estado)
        $("#textoMotivo").text("Editar")
    })
    $(document).on('click', '#btnCancelarMotivo', function (e) {
        e.preventDefault()
        $("#MotivoIdMotivo").val(0)
        $("#MotivoNombre").val('')
        $("#MotivoEstado").val(1)
        $("#textoMotivo").text("Agregar")
    })
    $(document).on('click', '#btnGuardarEditarMotivo', function (e) {
        e.preventDefault()
        let dataForm = {
            IdMotivo: $('#MotivoIdMotivo').val(),
            Nombre: $('#MotivoNombre').val(),
            Estado: $('#MotivoEstado').val(),
        }

        $("#formMotivo").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#formMotivo").data('bootstrapValidator').validate();
        if (validarRegistro.isValid()) {

            let url = `${basePath}BienesMateriales/InsertarMotivo`
            if ($('#MotivoIdMotivo').val() > 0) {
                url = `${basePath}BienesMateriales/EditarMotivo`
            }

            let dataForm4 = new FormData(document.getElementById("formMotivo"))

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
                        $("#MotivoIdMotivo").val(0)
                        $("#MotivoNombre").val('')
                        $("#MotivoEstado").val(1)
                        $("#textoMotivo").text("Agregar")

                        limpiarValidadorFormMotivo()
                        obtenerListaMotivos()
                        obtenerListaMotivosPorEstado()
                        $("#full-modal_motivos").modal('hide')
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




});



// MODAL MOTIVO ------------------------------------------------------------------

// FUNCIONES----------------------------------------------------------------------------------
function obtenerListaCategorias() {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarCategoria",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaCategorias(result.data)
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

function obtenerListaMotivos() {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarMotivo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaMotivos(result.data)
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

function renderTablaCategorias(data) {
    let element = $('#contenedorListadoCategorias')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdCategoria}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarCategoria" data-id="${item.IdCategoria}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoCategorias">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una categoria</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoCategorias").DataTable()
    }


}


function renderTablaMotivos(data) {
    let element = $('#contenedorListadoMotivos')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdMotivo}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarMotivo" data-id="${item.IdMotivo}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
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
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue un motivo</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoMotivos").DataTable()
    }


}

function renderTablaEmpleadosSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()
    if (value) {
        arrayEmpleadosSeleccionados = [];
    } 
    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
            <td>${item.NombreDocumentoRegistro.toUpperCase()} - ${item.DocumentoRegistro}</td>
            <td>${item.Cargo}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosBienMaterial">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4" align="center">Agregue un empleado</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)

}
function obtenerListaEmpleadosPorEstado() {
    let element = $('#contenedorTableEmpleadosBienMaterial')
    element.empty()
    if(empresaSeleccionada){
        $.ajax({
            type: "POST",
            url: basePath + "BUKEmpleado/ObtenerEmpleadosActivos?idempresa="+empresaSeleccionada,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.data) {
                    arrayEmpleados = result.data
                    let tbody = result.data.map(item => `
                            <tr>
                                <td>${item.Nombres} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
                                <td>${item.TipoDocumento.toUpperCase()} - ${item.NumeroDocumento}</td>
                                <td>${item.Cargo}</td>
                                <td><a class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.IdBuk}">Seleccionar <span class="glyphicon glyphicon-check"></span></a></td>
                            </tr>
                        `)
                    let htmlContent = `
                        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosBienMaterial">
                            <thead>
                                <tr>
                                    <th>Nombres</th>
                                    <th>Nro Documento</th>
                                    <th>Cargo</th>
                                    <th>Acción</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4"><p class="text-center text-danger">No se encontraron resultados</p></td></tr>`}
                            </tbody>
                        </table>
                    `
                    element.html(htmlContent)
                    if (tbody.length > 0) {
                        $('#tableEmpleadosBienMaterial').DataTable()
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
    }

    return false
}

function renderSelectSalasModalBienMaterial(value) {
    $("#cboSalaBienMaterial").html('')
    if (arraySalas) {
        $("#cboSalaBienMaterial").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join("")) 
        $("#cboSalaBienMaterial").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
        });
        if (value) {
            $("#cboSalaBienMaterial").val(value).trigger("change");
        } else { 
            $("#cboSalaBienMaterial").val(null).trigger("change");
        }
        limpiarValidadorFormBienMaterial();
        
    }

}
function obtenerListaEmpresas(value) {
    $.ajax({
        type: "POST",
        url: basePath + "EquivalenciaEmpresa/ListarTodasLasEquivalenciasEmpresaActivasJSON",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            console.log(result)
            $("#cboEmpresa").html('')
            if (result.data) {
                $('#cboEmpresa').append('<option></option>')
                $("#cboEmpresa").append(result.data.map(item => `<option value="${item.IdEmpresaBuk}">${item.Nombre}</option>`).join(""))
             
                $("#cboEmpresa").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
                });
                if (value) {
                    shouldTriggerChange = false;
                    $("#cboEmpresa").val(value).trigger('change')
                }
            }
            limpiarValidadorFormBienMaterial();
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
function obtenerListaMotivosPorEstado(value) {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarMotivoPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboMotivo").html('')
            if (result.data) {
                $('#cboMotivo').append('<option></option>')
                $("#cboMotivo").append(result.data.map(item => `<option value="${item.IdMotivo}">${item.Nombre}</option>`).join("")) 
                $("#cboMotivo").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
                });
                if (value) {
                    $("#cboMotivo").val(value).trigger("change");
                }
            }
            limpiarValidadorFormBienMaterial();

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
function obtenerListaCategoriasPorEstado(value) {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarCategoriaPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboCategoria").html('')
            if (result.data) {
                $("#cboCategoria").html(result.data.map(item => `<option value="${item.IdCategoria}">${item.Nombre}</option>`).join(""))
                $('#cboCategoria').append('<option value="-1">OTROS</option>') 
                $("#cboCategoria").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
                });
                if (value) {
                    $("#cboCategoria").val(value).trigger("change");
                }
            }
            limpiarValidadorFormBienMaterial();

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
function buscarBienesMateriales(params) {
    let listasala = $("#cboSala").val();
    let idtipobienmaterial = $("#cboTipoBien").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla");

    if (params) {
        listasala = params.CodSala;
        fechaini = params.fechaInicio;
        fechafin = params.fechaFin;
        idtipobienmaterial = -1;
    }

    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarBienesMaterialesxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, idtipobienmaterial, fechaini, fechafin }),
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
                data: datos, 
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdBienMaterial", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fecha",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    },
                    {
                        data: "NombreCategoria", title: "Categoria",
                        render: function (data) {
                            if (data) {
                                return data.length > 27 ? data.substring(0, 27) + "..." : data;
                            }
                            return "";
                        }
                    },
                    {
                        data: "NombreMotivo",
                        title: "Motivo I/S",
                        render: function (data) {
                            if (data) {
                                return data.length > 27 ? data.substring(0, 27) + "..." : data;
                            }
                            return "";
                        }
                    },
                    { data: "NombreEmpresa", title: "Empresa / Institucion" },
                    {
                        data: "GRRFT",
                        title: "GRR/FT",
                        render: function (data) {
                            return data == "" ? '<span class="label btn-warning">No definido</span>' : data
                        }
                    },
                    {
                        data: "FechaIngreso",
                        title: "Ingreso",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Salida",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    }, 
                    {
                        data: "TipoBienMaterial",
                        title: "Tipo",
                        "render": function (data) {
                            var estado = "";
                            var css = "btn-secondary";
                            switch (data) {
                                case 1:
                                    css = "btn-success";
                                    estado = "Interno";

                                    break;
                                case 2:
                                    css = "btn-primary";
                                    estado = "Externo";
                                    break;
                                default:
                                    css = "btn-secondary";
                                    estado = "N/A";
                                    break;
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';
                        },
                        className: "text-center",
                    },
                    {
                        data: "IdBienMaterial",
                        title: "Accion",
                        "render": function (o, value, oData) {
                            return `
                                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditarbien" data-id="${o}">
                                                <i class="glyphicon glyphicon-pencil"></i>
                                            </button> 
                                            <!-- <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-primary btnFinalizarSalidabien" data-id="${o}">
                                                 <i class="glyphicon glyphicon-flag"></i>
                                            </button>-->
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

                    $('.btnEditarbien').tooltip({
                        title: "Editar Bien Material"
                    });

                    $('.btnFinalizarSalidabien').tooltip({
                        title: "Finalizar Bien Material"
                    });
                    $('.btnEliminar').tooltip({
                        title: "Eliminar Bien Material"
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
        content: '¿Eliminar Bien Material?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteBienMaterial(id)
        }
    });
});
const deleteBienMaterial = (id) => {
    const data = {
        id: id
    };
    $.ajax({
        url: `${basePath}BienesMateriales/EliminarBienesMateriales`,
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
            buscarBienesMateriales()
        }
    });
}
 
//VALIDAR-----------------------------------------
function limpiarValidadorFormBienMaterial() {
    $("#bienMaterialForm").parent().find('div').removeClass("has-error");
    $("#bienMaterialForm").parent().find('div').removeClass("has-success");
    $("#bienMaterialForm").parent().find('i').removeAttr("style").hide();
}
// NUEVO EMPLEADO------------------------------------------------------------



$(document).on('click', "#btnNuevoEmpleado", function () {
    let idEmpresa = $("#cboEmpresa").val()
    if (idEmpresa) {
        $('#textBienMaterial').text('Nuevo')
        limpiarValidadorFormNuevoEmpleado()
        limpiarValidadorFormBienMaterial()
        $('#NuevoEmpleadoDocumentoRegistro').val('');
        $('#NuevoEmpleadoNombre').val('');
        $('#NuevoEmpleadoApellidoPaterno').val('');
        $('#NuevoEmpleadoApellidoMaterno').val('');
        $('#full-modal_nuevoempleado').modal('show')
        } else {
            toastr.warning("Seleccione empresa", "Advertencia")
        }
})


//$(document).on("click", "#btnNuevoEmpleado", function () {
//    let idEmpresa = $("#cboEmpresa").val(); // Validar nuevamente por seguridad
//    if (idEmpresa) {
//        $('#textBienMaterial').text('Nuevo');
//        limpiarValidadorFormBienMaterial();
//        $('#full-modal_nuevoempleado').modal('show');
//    } else {
//        toastr.warning("Seleccione una empresa", "Advertencia");
//    }
//});

$("#full-modal_nuevoempleado").on("shown.bs.modal", function () { 
    limpiarValidadorFormNuevoEmpleado();
    obtenerListaCargosPorEstado();
    obtenerTiposDocumentos();
    let NuevoEmpleadoIdTipoDocumento = $(this).val(); 
})
function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarCargo",
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

function obtenerListaCargosPorEstado() {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarCargoPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#NuevoEmpleadocbCargo").html('')
            if (result.data) {
                $("#NuevoEmpleadocbCargo").html(result.data.map(item => `<option value="${item.IdCargo}">${item.Nombre}</option>`).join(""))
                $("#NuevoEmpleadocbCargo").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_nuevoempleado')
                });
                $("#NuevoEmpleadocbCargo").val(null).trigger("change")
            }
            limpiarValidadorFormNuevoEmpleado()

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

function obtenerListaEmpresaExterna() {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarEmpresa",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaEmpresaExterna(result.data)
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
function renderTablaEmpresaExterna(data) {
    let element = $('#contenedorListadoEmpresa')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdEmpresaExterna}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarEmpresa" data-id="${item.IdEmpresaExterna}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
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
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una empresa</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoEmpresa").DataTable()
    }


}
function obtenerListaEmpresaExternaPorEstado(value) {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarEmpresaPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboEmpresa").html('')
            if (result.data) {
                $('#cboEmpresa').append('<option></option>')
                $("#cboEmpresa").html(result.data.map(item => `<option value="${item.IdEmpresaExterna}">${item.Nombre}</option>`).join(""))
                $("#cboEmpresa").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
                });
                if (value) {
                    shouldTriggerChange = false;
                    $("#cboEmpresa").val(value).trigger('change')
                } else {
                    $("#cboEmpresa").val(null).trigger('change')

                }
            }
            limpiarValidadorFormBienMaterial();
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
function rendercboEmpresa() {
   
    $("#cboEmpresa").html('') 
        $('#cboEmpresa').append('<option></option>') 
        $("#cboEmpresa").select2({
            placeholder: "--Seleccione Tipo Bien Material--", dropdownParent: $('#full-modal_bienmaterial')
        }); 
    $("#cboEmpresa").val(null).trigger('change') 
    $('#cboEmpresa').prop('disabled', true);

    limpiarValidadorFormBienMaterial();
       
    return false;
}
//function obtenerListaEmpresas(value) {
//    $.ajax({
//        type: "POST",
//        url: basePath + "EquivalenciaEmpresa/ListarTodasLasEquivalenciasEmpresaJSON",
//        cache: false,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (result) {
//            $("#cboEmpresa").html('')
//            if (result.data) {
//                $('#cboEmpresa').append('<option></option>')
//                $("#cboEmpresa").append(result.data.map(item => `<option value="${item.IdEmpresaBuk}">${item.Nombre}</option>`).join(""))

//                $("#cboEmpresa").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_bienmaterial')
//                });
//                if (value) {
//                    shouldTriggerChange = false;
//                    $("#cboEmpresa").val(value).trigger('change')
//                }
//            }
//            limpiarValidadorFormBienMaterial();
//        },
//        error: function (request, status, error) {
//            toastr.error("Error", "Mensaje Servidor")
//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide")
//        }
//    });
//    return false;
//}


$(document).on('click', '#btnGuardarNuevoEmpleado', function (e) {
    e.preventDefault()
    let dataForm = {
        IdTipoDocumentoRegistro: $('#NuevoEmpleadoIdTipoDocumento').val(),
        DocumentoRegistro: $('#NuevoEmpleadoDocumentoRegistro').val(),
        Nombre: $('#NuevoEmpleadoNombre').val(),
        ApellidoMaterno: $('#NuevoEmpleadoApellidoMaterno').val(),
        ApellidoPaterno: $('#NuevoEmpleadoApellidoPaterno').val(),
        IdCargo: $('#NuevoEmpleadocbCargo').val(),
        IdEmpresaExterna: $('#cboEmpresa').val(),
        Estado: 1,
    } 
    $("#formNuevoEmpleado").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#formNuevoEmpleado").data('bootstrapValidator').validate();
    if (validarRegistro.isValid()) {
        let dataForm4 = new FormData(document.getElementById("formNuevoEmpleado"))

        $.ajax({
            url: `${basePath}BienesMateriales/InsertarEmpleado`,
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
                    $("#NuevoEmpleadoIdTipoDocumento").val("")
                    $("#NuevoEmpleadoDocumentoRegistro").val("")
                    $("#NuevoEmpleadoNombre").val('')
                    $("#NuevoEmpleadoApellidoMaterno").val('')
                    $("#NuevoEmpleadoApellidoPaterno").val('')

                    toastr.success(response.mensaje, "Mensaje Servidor");
                    $('#full-modal_nuevoempleado').modal('hide');
                    obtenerListaEmpleadosPorEstado();
                    limpiarValidadorFormNuevoEmpleado();
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

$(document).on('click', "#btnCancelarNuevoEmpleado", function () {
    $('#full-modal_nuevoempleado').modal('hide'); 
});
 
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
            $("#NuevoEmpleadoIdTipoDocumento").html('')
            if (result.data) {
                $("#NuevoEmpleadoIdTipoDocumento").html(result.data.map(item => `<option value="${item.Id}">${item.Nombre}</option>`).join(""))
                $("#NuevoEmpleadoIdTipoDocumento").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_nuevoempleado')
                });
                $("#NuevoEmpleadoIdTipoDocumento").val(null).trigger("change")
            }
            limpiarValidadorFormNuevoEmpleado()

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

$(document).on('click', '#agregarEmpresa', function (e) {
    e.preventDefault()
    limpiarValidadorFormEmpresa();
    $("#full-modal_empresa").modal('show')
    $("#full-modal_empresa").on("shown.bs.modal", function () {
        $("#EmpresaNombre").val('')
        obtenerListaEmpresaExterna()
    })
})
$(document).on('click', '#btnGuardarEditarEmpresa', function (e) {
    e.preventDefault();

    let dataForm = {

        IdEmpresa: $('#EmpresaIdEmpresa').val(),
        Nombre: $('#EmpresaNombre').val(),
        Estado: $('#EmpresaEstado').val(),
    }

    $("#formEmpresa").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#formEmpresa").data('bootstrapValidator').validate();
    if (validarRegistro.isValid()) {
        let IdRegistro = $("#EmpresaIdEmpresa").val()
        let url = basePath
        if (IdRegistro == 0) {
            url += "BienesMateriales/InsertarEmpresa"
        } else {
            url += "BienesMateriales/EditarEmpresa"
        }
        let dataForm4 = new FormData(document.getElementById("formEmpresa"))
        $.ajax({
            url: url,
            type: "POST",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataForm),
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
                    limpiarValidadorFormEmpresa();
                    obtenerListaEmpresaExterna();
                    obtenerListaEmpresaExternaPorEstado();
                    $("#full-modal_empresa").modal('hide')
                    toastr.success(response.mensaje, "Mensaje Servidor")

                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
            }
        });
    }

})
function limpiarValidadorFormEmpresa() {
    $("#formEmpresa").parent().find('div').removeClass("has-error");
    $("#formEmpresa").parent().find('i').removeAttr("style").hide();

}




$(document).on('click', '#agregarCargo', function (e) {
    e.preventDefault()
    limpiarValidadorFormCargo()
    $("#full-modal_cargos").modal('show')
    $("#full-modal_cargos").on("shown.bs.modal", function () {
        $('#CargoNombre').val('')
        obtenerListaCargos()
    })
}) 
$(document).on('click', '#btnGuardarEditarCargo', function (e) {
    e.preventDefault();
   
    let dataForm = {

        IdCargo: $('#CargoIdCargo').val(),
        Nombre: $('#CargoNombre').val(),
        Estado: $('#CargoEstado').val(),
    }
 
    $("#formCargo").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#formCargo").data('bootstrapValidator').validate();
    if (validarRegistro.isValid()) {
    let IdRegistro = $("#CargoIdCargo").val()
        let url = basePath
    if (IdRegistro == 0) {
        url += "BienesMateriales/InsertarCargo"
        } else {
        url += "BienesMateriales/EditarCargo"
        }
        let dataForm4 = new FormData(document.getElementById("formCargo"))
        $.ajax({
            url: url,
            type: "POST",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataForm),
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
                    limpiarValidadorFormCargo();
                    obtenerListaCargos();
                    obtenerListaCargosPorEstado();
                    $("#full-modal_cargos").modal('hide')
                    toastr.success(response.mensaje, "Mensaje Servidor")
                   
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
            }
        });
}

})
//$(document).on('click', "#btnCancelarCargo", function () {
//    $('#full-modal_cargos').modal('hide');
//});


function limpiarValidadorFormMotivo() {
    $("#formMotivo").parent().find('div').removeClass("has-error");
    $("#formMotivo").parent().find('div').removeClass("has-success");
    $("#formMotivo").parent().find('i').removeAttr("style").hide();
}
function limpiarValidadorFormCategoria() {
    $("#formCategoria").parent().find('div').removeClass("has-error");
    $("#formCategoria").parent().find('div').removeClass("has-success");
    $("#formCategoria").parent().find('i').removeAttr("style").hide();
}
function limpiarValidadorFormCargo() {
    $("#formCargo").parent().find('div').removeClass("has-error");
    $("#formCargo").parent().find('div').removeClass("has-success");
    $("#formCargo").parent().find('i').removeAttr("style").hide();
}

function limpiarValidadorFormNuevoEmpleado() {
    $("#formNuevoEmpleado").parent().find('div').removeClass("has-error");
    $("#formNuevoEmpleado").parent().find('div').removeClass("has-success");
    $("#formNuevoEmpleado").parent().find('i').removeAttr("style").hide();
}
 
$(document).on("click", ".btnFinalizarSalidabien", function () {
    idBienMaterialSeleccionado = $(this).data("id"); 
    const fechaHora = obtenerFechaHora();
    const mensaje = `¿Está seguro de finalizar el registro a las ${fechaHora}?`;
    $("#confirmationMessage").text(mensaje);
     
    $("#full-modal_confirmacion").modal("show");
});
$("#confirmFinalizar").on("click", function () {
    if (idBienMaterialSeleccionado) { 
        finalizarBienMaterial(idBienMaterialSeleccionado);
    } 
    $("#full-modal_confirmacion").modal("hide");
});
function finalizarBienMaterial(id) {
    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/FinalizarHoraRegistroBienMaterial", 
        data: JSON.stringify({ idbienmaterial: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Éxito");
            } else {
                toastr.error(response.mensaje, "Error");
            }
            $("#full-modal_confirmacion").modal("hide");
            buscarBienesMateriales()
        },
        error: function () {
            toastr.error("No se pudo completar la solicitud.", "Error del servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
function obtenerFechaHora() {
    const ahora = new Date();
    let horas = ahora.getHours();
    const minutos = String(ahora.getMinutes()).padStart(2, '0');
    const periodo = horas >= 12 ? 'PM' : 'AM';

    horas = horas % 12 || 12;

    return `${horas}:${minutos} ${periodo}`;
} 

function initAutocomplete(){
    $("#busqueda").autocomplete({
        source: function(request, response) {
            let idEmpresa = $("#cboEmpresa").val()
            if(idEmpresa){
                $.ajax({
                    url: basePath + "BUKEmpleado/ObtenerEmpleadosActivosPorPatron",
                    dataType: "json",
                    data: {
                        term: request.term,
                        idempresa:idEmpresa
                    },
                    success: function(json) {
                        var results = $.map(json.data, function(item) {
                            return {
                                label: item.NombreCompleto + " - " + item.NumeroDocumento,
                                value: item.NombreCompleto,
                                fullData: item
                            };
                        });
                        response(results);
                    },
                    error: function() {
                        response([]);
                    }
                });
            }
            else{
                toastr.warning("Seleccione empresa","Advertencia")
                response([]);
            }
        },
        minLength: 1,
        select: function (event, ui) {
            let existEmpleado = arrayEmpleadosSeleccionados.find(x => x.IdEmpleado == ui.item.fullData.IdBuk)

            if (!existEmpleado) {
                arrayEmpleadosSeleccionados.push({
                    IdEmpleado: ui.item.fullData.IdBuk,
                    Nombre: ui.item.fullData.Nombres,
                    ApellidoPaterno: ui.item.fullData.ApellidoPaterno,
                    ApellidoMaterno: ui.item.fullData.ApellidoMaterno,
                    NombreDocumentoRegistro: ui.item.fullData.TipoDocumento,
                    DocumentoRegistro: ui.item.fullData.NumeroDocumento,
                    Cargo: ui.item.fullData.Cargo,
                    IdCargo: ui.item.fullData.IdCargo,
                    Empresa: ui.item.fullData.Empresa,
                    IdEmpresa: ui.item.fullData.IdEmpresa
                })
            } else {
                toastr.warning("Empleado ya seleccionado", "Advertencia")
            }

            renderTablaEmpleadosSeleccionados()
        },
        open: function(event, ui) {
            $('.ui-autocomplete').appendTo('#AutoCompleteContainer')
            $('.ui-autocomplete').css('z-index', '9999');
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('top', '');
            $('.ui-autocomplete').css('left', '0');
            $('.ui-autocomplete').css('width', '100%');
        },
        close: function(event, ui) {
            // Limpiar el input después de seleccionar un elemento
        }
    }).autocomplete("instance")._renderItem = function(ul, item) {
        // // Personaliza cómo se muestra cada elemento en la lista
        return $("<li>")
            .append("<div>" + item.label + "<br><small>" + item.fullData.Cargo + " - " + item.fullData.Empresa + "</small></div>")
            .appendTo(ul);
    };
}

function renderTablaEmpleadosSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()
    if (value) {
        arrayEmpleadosSeleccionados = [];
    }
    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
            <td>${item.NombreDocumentoRegistro.toUpperCase()} - ${item.DocumentoRegistro}</td>
            <td>${item.Cargo}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosBienMaterial">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4" align="center">Agregue un empleado</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)

}




function initAutocompleteExterno() {
    $("#busqueda").autocomplete({
        source: function (request, response) {
            let idEmpresa = $("#cboEmpresa").val()

            if (idEmpresa) {
                $.ajax({
                    url: basePath + "BienesMateriales/ObtenerEmpleadoExternoActivosPorPatron",
                    dataType: "json",
                    data: {
                        term: request.term,
                        idempresa: idEmpresa
                    },
                    success: function (json) {
                        var results = $.map(json.data, function (item) {

                            let NombreCompleto = `${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}`;
                            return {
                                label: NombreCompleto + " - " + item.DocumentoRegistro,
                                value: item.Nombre,
                                fullData: item
                            };
                        });
                        response(results);
                    },
                    error: function () {
                        response([]);
                    }
                });
            }
            else {
                toastr.warning("Seleccione empresa", "Advertencia")
                response([]);
            }
        },
        minLength: 1,
        select: function (event, ui) {
            let existEmpleado = arrayEmpleadosSeleccionados.find(x => x.IdEmpleado == ui.item.fullData.IdEmpleado)

            if (!existEmpleado) {
                arrayEmpleadosSeleccionados.push({
                    IdEmpleado: ui.item.fullData.IdEmpleado,
                    Nombre: ui.item.fullData.Nombre,
                    ApellidoPaterno: ui.item.fullData.ApellidoPaterno,
                    ApellidoMaterno: ui.item.fullData.ApellidoMaterno,
                    NombreDocumentoRegistro: ui.item.fullData.NombreDocumento,
                    DocumentoRegistro: ui.item.fullData.DocumentoRegistro,
                    Cargo: ui.item.fullData.NombreCargo,
                    IdCargo: ui.item.fullData.IdCargo,
                    NombreEmpresaExterna: ui.item.fullData.NombreEmpresaExterna,
                    IdEmpresaExterna: ui.item.fullData.IdEmpresaExterna
                })
            } else {
                toastr.warning("Empleado ya seleccionado", "Advertencia")
            }

            renderTablaEmpleadoExternoSeleccionados()
        },
        open: function (event, ui) {
            $('.ui-autocomplete').appendTo('#AutoCompleteContainer')
            $('.ui-autocomplete').css('z-index', '9999');
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('top', '0');
            $('.ui-autocomplete').css('left', '0');
            $('.ui-autocomplete').css('width', '100%');
        },
        close: function (event, ui) {
            // Limpiar el input después de seleccionar un elemento
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        // // Personaliza cómo se muestra cada elemento en la lista
        return $("<li>")
            .append("<div>" + item.label + "<br><small>" + item.fullData.NombreCargo + " - " + item.fullData.NombreEmpresaExterna + "</small></div>")
            .appendTo(ul);
    };
}



function renderTablaEmpleadoExternoSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()
    if (value) {
        arrayEmpleadosSeleccionados = [];
    }
    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
            <td>${item.NombreDocumentoRegistro ? item.NombreDocumentoRegistro.toUpperCase(): 'N/A'} - ${item.DocumentoRegistro}</td>
            <td>${item.Cargo}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosBienMaterial">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4" align="center">Agregue un empleado</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)

}







$('#agregarMotivo').tooltip({
    title: "Agregar Motivo"
});
$('#agregarCategoria').tooltip({
    title: "Agregar Categoría"
});
$('#agregarCargo').tooltip({
    title: "Agregar Cargo"
});
$('#agregarEmpresa').tooltip({
    title: "Agregar Empresa"
});
$('#eliminarfechasalida').tooltip({
    title: "Limpiar Fecha Salida"
});
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
                url: basePath + "BienesMateriales/ImportarExcelBienesMateriales",
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

$(document).ready(function () {
    ObtenerListaSalas();
   
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        buscarListadoAsistencia();
    });
    $(document).on("click",'.btnEliminar',function(e) {
        let AsistenciaId=$(this).data("id")
        if(AsistenciaId){
            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: '¿Esta seguro de Eliminar el registro?',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: 'SI',
                cancelButton: 'NO',
                content: false,
                confirm: function () {
                    let dataForm={ AsistenciaId:AsistenciaId}
                    $.ajax({
                        type: "POST",
                        url: basePath + "AsistenciaCliente/EliminarAsistenciaClienteSala",
                        cache: false,
                        data:JSON.stringify(dataForm),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (xhr) {
                            $.LoadingOverlay("show");
                        },
                        success: function (result) {
                            if(result.respuesta){
                                toastr.success(result.mensaje, "Mensaje Servidor")
                                buscarListadoAsistencia();
                            }else{
                                toastr.error(result.mensaje,"Mensaje Servidor")
                            }
                        },
                        error: function (request, status, error) {
                            toastr.error("Error", "Mensaje Servidor");
                        },
                        complete: function (resul) {
                            $.LoadingOverlay("hide");
                        }
                    });
                },
                cancel: function () {
                }
            });
          
        }
        else{
            toastr.error("Id Incorrecto","Mensaje Servidor")
        }
    })
    $(document).on("click", "#btnExcel", function () {
        let fechaIni = $("#fechaInicio").val();
        let fechaFin = $("#fechaFin").val();
        let sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        let listasala = $("#cboSala").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AsistenciaCliente/GetListadoAsistenciaSalaFiltrosExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
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
});
function ObtenerListaSalas() {
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
            var datos = result.data;
            
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
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
function buscarListadoAsistencia() {
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "AsistenciaCliente/GetListadoAsistenciaSalaFiltros",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response.data)
            permiso=response.data.dataPermiso;
            response = response.data.dataListado;
            let columnas=ColumnasDatatable(permiso);
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
                columns: columnas,
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
            $('#ResumenSala tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
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
function ColumnasDatatable(permiso){
    let obj=[
        { data: "Id", title: "ID" },
        {
            data: null, title: "Cliente","render":function(value,type, oData, meta){
                if(oData.Cliente.NombreCompleto==''||oData.Cliente.NombreCompleto==null){
                    return `${oData.Cliente.Nombre} ${oData.Cliente.ApelPat} ${oData.Cliente.ApelMat}`
                }
                else{
                    return oData.Cliente.NombreCompleto
                }
            }
        },
        { data: "Sala.Nombre", title: "Sala" },
        { data: "CodMaquina", title: "Cod. Máquina" },
        { data: "MarcaMaquina", title: "Marca" },
        { data: "JuegoMaquina", title: "Juego" },
        { data: "TipoFrecuencia.Nombre", title: "Tipo Frecuencia" },
        { data: "TipoCliente.Nombre", title: "Tipo Cliente" },
        { data: "TipoJuego.Nombre", title: "Tipo Juego" },
        { data: null, title: "Apuesta Importante","render":function(value,type,oData,meta){
            return oData.ApuestaImportante.toFixed(2)
        } },
        {
            data: "FechaRegistro", title: "Fecha","render":function(value, type, oData, meta){
                return moment(oData.FechaRegistro).format('YYYY/MM/DD hh:mm:ss A')
                
            }
        },
    ]
    if(permiso){
        obj.push(
            {
                data: null, title: "Acción","render":function(value, type, oData, meta){
                    let botones = '<button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="' + oData.Id + '"><i class="glyphicon glyphicon-pencil"></i> Eliminar</button> ';
                    return botones;
                }
            },
        )
    }
    return obj
}
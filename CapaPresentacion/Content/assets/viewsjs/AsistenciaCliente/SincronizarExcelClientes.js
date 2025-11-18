$(document).ready(function () {
    ObtenerListaSalas();
    $("#cboOrden").select2({
        multiple: false,
    })
    $(document).on('click', '.btnSubirExcel', function (e) {
        e.preventDefault()
        let file = $('#ExcelClientes')[0].files[0]
        if (file == null) {
            toastr.error("Error", "Debe Seleccionar Un Archivo Adjunto")
            return false
        }
        else {
            let image_arr = file.name.split(".")
            let extension = image_arr.pop().toLowerCase()
            console.log(extension)
            let dataForm = new FormData()
            dataForm.append('file', file)

            if (extension != 'xls' && extension != 'xlsx') {
                toastr.warning("Warning", "Sólo Se Permite formato Excel (.xls||xlsx)")
            }
            else {
                let url = basePath + 'AsistenciaCliente/SincronizarExcelClientesJson'
                $.ajax({
                    url: url,
                    type: "POST",
                    method: "POST",
                    data: dataForm,
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function () {
                        $.LoadingOverlay("show")
                    },
                    complete: function () {
                        $.LoadingOverlay("hide")
                    },
                    success: function (response) {
                        if(response.respuesta){
                            toastr.success("Respuesta", response.mensaje)
                            let data=response.data
                            let link = document.createElement('a');
                            document.body.appendChild(link); //required in FF, optional for Chrome
                            link.href = "data:application/vnd.ms-excel;base64, " + response.base64;;
                            link.download = "ExcelResultado.xlsx";
                            link.click();
                            link.remove()
                            llenarDatatable(data);
                        }
                        else{
                            toastr.error("Error", response.mensaje)
                        }
                        $("#ExcelClientes").val('');
                    },
                });
            }
        }
    });

    $(document).on('click', '.btnSubirExcelv2', function (e) {
        e.preventDefault()
        let orden=$('#cboOrden').val()
        let file = $('#ExcelClientesv2')[0].files[0];
        let codsala = $('#cboSala').val();

        if (codsala == null || codsala=="") {
            toastr.error("Error", "Debe Seleccionar Sala");
            return false
        }

        if (file == null) {
            toastr.error("Error", "Debe Seleccionar Un Archivo Adjunto");
            return false
        }
        else {
            let image_arr = file.name.split(".")
            let extension = image_arr.pop().toLowerCase();
            console.log(extension)
            let dataForm = new FormData();
            dataForm.append('file', file);
            dataForm.append('codsala', codsala);
            dataForm.append('orden', orden)
            if (extension != 'xls' && extension != 'xlsx') {
                toastr.warning("Warning", "Sólo Se Permite formato Excel (.xls||xlsx)");
            }
            else {
                let url = basePath + 'AsistenciaCliente/SincronizarExcelClientesV2Json';
                $.ajax({
                    url: url,
                    type: "POST",
                    method: "POST",
                    data: dataForm,
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function () {
                        $.LoadingOverlay("show")
                    },
                    complete: function () {
                        $.LoadingOverlay("hide")
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            toastr.success("Respuesta", response.mensaje)
                            let data = response.data
                            let link = document.createElement('a');
                            document.body.appendChild(link); //required in FF, optional for Chrome
                            link.href = "data:application/vnd.ms-excel;base64, " + response.base64;;
                            link.download = "ExcelResultadoV2.xlsx";
                            link.click();
                            link.remove()
                            llenarDatatable(data);
                        }
                        else {
                            toastr.error("Error", response.mensaje)
                        }
                        $("#ExcelClientesv2").val('');
                    },
                });
            }
        }
    })
})
function llenarDatatable(obj){
    let addtabla = $("#contenedorRespuesta");
    $(addtabla).empty();
    $(addtabla).append('<table id="ResumenSincronizacion" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
    let columnas=ColumnasDatatable()
    objetodatatable = $("#ResumenSincronizacion").DataTable({
        "bDestroy": true,
        "bSort": false,
        "scrollCollapse": true,
        "scrollX": false,
        "sScrollX": "100%",
        "paging": true,
        "autoWidth": false,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        data: obj,
        columns: columnas,
        "initComplete": function (settings, json) {
        },
        "drawCallback": function (settings) {
        }
    });
}
function ColumnasDatatable(){
    let obj=[
        { data: "cliente.NroDoc", title: "Nro Doc" },
        {
            data: null, title: "Cliente","render":function(value,type, oData, meta){
                return oData.cliente.NombreCompleto
            }
        },
        { data: "accionRealizada", title: "Accion Realizada" },
    ]
    return obj
}

function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (result) {
            var datos = result.data
            $("#cboSala").append('<option value="">--Seleccione--</option>')
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>')
            })
            $("#cboSala").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            })
            // $("#cbo_sala_id").val(0).trigger("change");
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
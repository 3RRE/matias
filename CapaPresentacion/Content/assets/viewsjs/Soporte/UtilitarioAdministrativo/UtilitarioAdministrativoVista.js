$(document).ready(function () {   
   // llenarSelect(basePath + "Sala/ListadoSalaPorUsuarioJson", {}, "cboSala", "UrlCuadre", "Nombre", '');
    ObtenerListaSalas();
    $("#cboSala").select2();

    $('#btnSicronizar').on('click', function (e) {
        ListarConsulta(); 
    });

    fechaMin = moment(new Date()).format('YYYY-MM-DD');
    var dateMin = new Date(fechaMin);
    dateMin.setDate(dateMin.getDate() - 2);
    $(".dateOnly1").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        showToday: true,
        minDate: dateMin,
        maxDate: moment(new Date()),
        defaultDate: moment(new Date())
    });

    $(".dateOnly2").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        showToday: true,
        useCurrent: false,
        //minDate: dateMin,
        defaultDate: moment(new Date())
    });

    $(".dateOnly2").on("dp.change", function (e) {
        $('.dateOnly1').data("DateTimePicker").destroy();
        $('#txtFechaInicio').val(moment(e.date).format('DD/MM/YYYY'));
        var fechaMin = moment(e.date).format('YYYY/MM/DD');
        var dateMin = new Date(fechaMin);
        dateMin.setDate(dateMin.getDate() - 3);

        $(".dateOnly1").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY',
            showToday: true,
            minDate: dateMin,
            maxDate: moment(e.date).format('DD/MM/YYYY'),
            defaultDate: moment(e.date).format('DD/MM/YYYY')
        });

    });

    function ListarConsulta() {
        
        var fechaini = $("#txtFechaInicio").val();
        var fechafin = $("#txtFechaFin").val();
        var sala = $("#cboSala").val();
        var url = basePath + "UtilitarioAdministrativo/UtilitarioAdministrativoSincronizarJson";
        var data = { fechaIni: fechaini, fechaFin: fechafin, Sala: sala };
        dataAuditoria(1, "#formfiltro", 3, "UtilitarioAdministrativo/UtilitarioAdministrativoSincronizarJson", "BOTON SINCRONIZAR");
        $.ajax({
            url: url,
            type: "POST",
            timeout: -1,
            contentType: "application/json",
            data: JSON.stringify(data),
            deferRender: true,
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                var respuesta = response.respuesta;

                if (respuesta === true) {
                    toastr.success("Se Sincronizó Correctamente", "Mensaje Servidor");

                    setTimeout(function () {
                        window.location = basePath + "UtilitarioAdministrativo/UtilitarioAdministrativoVista";
                    }, 3000);

                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }



    function ObtenerListaSalas() {
        comboImagen = $("#cboImagen");
        comboImagen.find('option').remove();
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
                    $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
                });
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



});
VistaAuditoria("UtilitarioAdministrativo/UtilitarioAdministrativoVista", "VISTA", 0, "", 3);
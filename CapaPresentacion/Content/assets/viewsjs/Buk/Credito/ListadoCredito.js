$(document).ready(function () {
    listarEquivalenciaEmpresas();

    $(document).on('click', '.btnDetalle', function (e) {
        let id = $(this).data("id");
        let nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        let redirectUrl = `${basePath}Credito/CreditoDetalleVista/${id}`;
        let ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });
});

const listarEquivalenciaEmpresas = () => {
    let url = basePath + "Credito/ListarCreditosJSON";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (!response.success) {
                toastr.error(response.displayMessage)
                return;
            }
            createDataTable(response.data)
        }
    });
};

const createDataTable = (data) => {
    objetodatatable = $("#tableCreditos").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        bDeferRender: true,
        data: data,
        columnDefs: [
            {
                targets: '_all',
                className: 'text-center'
            }
        ],
        columns: [
            { data: "IdCreditoBuk", title: "ID Buk" },
            { data: "CodigoEmpresa", title: "Cod. Empresa" },
            { data: "NumeroDocumento", title: "Nro. Doc." },
            { data: "DescripcionTipo", title: "Tipo" },
            {
                data: "Periodo",
                title: "Periodo - Año",
                bSortable: false,
                render: function (value, type, row) {
                    return `${row.Periodo} - ${row.Anio}`;
                }
            },
            {
                data: "CuotaMensual", title: "Cuota Mensual", render: function (value) {
                    return `S/${value}`;
                }
            },
            { data: "CantidadCoutas", title: "Nro. Cuotas" },
            {
                data: "MontoTotal", title: "Total", render: function (value) {
                    return `S/${value}`;
                }
            },
            {
                data: "Estado", title: "Estado", render: function (value) {
                    return value ? '<span class="badge badge-success">Activo</span>' : '<span class="badge badge-danger">Anulado</span>';
                }
            },
            {
                data: "IdCredito",
                title: "Acción",
                bSortable: false,
                render: function (id) {
                    let btnDetail = `<button type="button" class="btn btn-xs btn-primary btnDetalle" data-nombre="Detalle del Crédito" data-id="${id}"><i class="glyphicon glyphicon-info-sign"></i></button> `;
                    return btnDetail;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnDetalle').tooltip({
                title: "Detalle del Crédito"
            });
        }
    });
    $('.btnDetalle').tooltip({
        title: "Detalle del Crédito"
    });
}
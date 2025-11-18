$(document).ready(function(){
    ListarSalas()
    $('#btnNuevaEmpresa').on('click', function (e) {
        window.location.replace(basePath + "Empresa/EmpresaInsertarVista");
    });
    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Empresa/EmpresaModificarVista/" + id;
        window.location.replace(url);
    });
})

function imgLoader(element) {
    var overlay = $(element).siblings('.overlayEmpresa');
    overlay.hide();
}
function ListarSalas() { 
    var url = basePath + "Empresa/ListadoEmpresa";
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
            //console.log(response.data)
            respuesta = response.data
            objetodatatable = $("#tableEmpresa").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {
                },
                data: response.data,
                columns: [
                    { data: "RazonSocial", title: "Razón Social" },
                    { data: "Ruc", title: "RUC" },
                    { data: "NombreRepresentanteLegal", title: "Representante Legal" }, 
                    { 
                        data: null, 
                        title: "Logo",
                        width: "50px",
                        "render": function (a, b, c) {
                            let span

                            span = `<div style="position:relative;height:50px; width:50px"><div class="overlayEmpresa" style="position:absolute;height:50px; width:50px;background-color:red;"></div><img style="object-fit: cover;height:50px;width:50px;" src=${basePath}/Uploads/LogosEmpresas/${c.RutaArchivoLogo}  onLoad="imgLoader(this)" /></div>`;

                            return span;
                        }
                    }, 
                    {
                        data: "CodEmpresa",
                        "bSortable": false,
                        "title":"Accion",
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button> ' ;
                        }
                    }
                ],
                "drawCallback": function (settings) {
                },

                "initComplete": function (settings, json) {
                },
            });
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};
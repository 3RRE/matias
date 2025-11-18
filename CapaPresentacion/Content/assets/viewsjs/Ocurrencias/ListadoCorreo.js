$(document).ready(function(){
    ListadoCorreo();
    $('#btnNuevoRegistro').on('click', function (e) {
        window.location.replace(basePath + "Ocurrencias/RegistroCorreo");
    });
    $(document).on('click', '.btnEditar', function (e) {
        let id = $(this).data("id");
        let url = basePath + "Ocurrencias/EditarCorreo/" + id;
        window.location.replace(url);
    });
    $(document).on('click', '.btnDetalleSalas', function (e) {
        let correo = $(this).data("correo")
        let dataJson = $(this).data("json")
        $("#direc_correo").html(correo)
        let span = "<table style='border: 1px solid black;'>";
        span += "<tr  style='border: 1px solid black;'> <th  style='border: 1px solid black;'>Id</th> <th  style='border: 1px solid black;'>Nombre</th> </tr>";
        $.each(dataJson, function (index, value) {
            span += `<tr style='border: 1px solid black;'> <th  style='border: 1px solid black;'>${value.SalaId} </th> <th  style='border: 1px solid black;'>${value.Sala.Nombre}</th></tr>`;
        })
        span += "</table>";
        $("#detail_correo").html(span)
        $("#full-modal").modal("show");
    });


    $(document).on("change", ".selectEstado", function () {
        let Id = $(this).data("id");
        let Estado = $(this).val();
        let dataForm = {
            Estado: Estado,
            Id: Id
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea Cambiar el Estado del registro?',
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
                    url: basePath + "Ocurrencias/EditarEstadoCorreoJson",
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
                            if (Estado == 1) {
                                $('.selectIdCorreo$' + Id).val(0);
                            }
                            else {
                                $('.selectIdCorreo$' + Id).val(1);
                            }
                            toastr.error(response.mensaje, "Mensaje");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        if (Estado == 1) {
                            $('.selectIdCorreo$' + Id).val(0);
                        }
                        else {
                            $('.selectIdCorreo$' + Id).val(1);
                        }
                    }
                });
            },
            cancel: function () {
                if (Estado == 1) {
                    $('.selectIdCorreo$' + Id).val(0);
                }
                else {
                    $('.selectIdCorreo$' + Id).val(1);
                }
            }
        });
    });



})
function ListadoCorreo(){
    let url = basePath + "Ocurrencias/GetListadoCorreoJson";
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
            if(response.respuesta){
                let columns=ColumnasDatatable(response.data)
                objetodatatable = $("#tableCorreo").DataTable({
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
                    data: response.data,
                    columns:columns,
                    "initComplete": function (settings, json) {
                    },
                    "drawCallback": function (settings) {
                    }
                });
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            console.log("errorrrrrrrr");
        }
    });
}
function ColumnasDatatable(data){
    let obj=[
        { 
            title: "ID" 
        },
        // {
        //     title: "Sala",
        // },
        {
            title: "Tipo",
        },
        { 
            title: "Nombre" 
        },
        {
            title: "Email",
        },
        {
            title: "Estado",
        },
        {
            title: "Acciones",
        },
    ]
    if(data){
        if(data.length>0){
            obj=[
                { 
                    data: "Id", 
                    title: "ID" 
                },
                // { 
                //     data: null, 
                //     title: "Sala", 
                //     "render":function(value,type, oData, meta){
                //         let salas=oData.ListaCorreoSala.map(({Sala})=> Sala.Nombre);
                //         return salas.join(" - ");
                //     } 
                // },
                {
                    data: null, 
                    title: "Tipo",
                    "render":function(value,type, oData, meta){
                        return oData.CodTipoCorreo==1?"REMITENTE":"DESTINATARIO";
                    }
                },
                { 
                    data: "Nombre", 
                    title: "Nombre" 
                },
                {
                    data: "Email", 
                    title: "Email",
                },    
                {   
                    data: null, title: "Estado","render":function(value,type,oData,metadata){
                        let select = `<select class="selectEstado selectIdCorreo${oData.Id}" data-id=${oData.Id} style="width:100%">`;
                        if (oData.Estado == 1) {
                            select += `<option value=1 selected>ACTIVO</option><option value=0 >INACTIVO</option>`;
                        }
                        else {
                            select += `<option value=1>ACTIVO</option><option value=0 selected>INACTIVO</option>`;
                        }
                        select += `</select>`;
                        return select;
                    }
                },
                {
                    data: null,
                    title:"ACCIONES",
                    "bSortable": false,
                    "render": function (value, type, oData, meta) {
                        let botones =  `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${oData.Id}">
                                            <i class="glyphicon glyphicon-pencil"></i> Editar
                                        </button>
                                        <button type="button" class="btn btn-xs btn-info btnDetalleSalas" 
                                            data-json='${JSON.stringify(oData.ListaCorreoSala).replace(/'/g, "\\'")}'
                                            data-correo=${oData.Email}
                                            >
                                            <i class="glyphicon glyphicon-pencil"></i> Ver Salas
                                        </button>`;
                        return botones;
                    }
                }
            ]
        }
    }
    return obj
}
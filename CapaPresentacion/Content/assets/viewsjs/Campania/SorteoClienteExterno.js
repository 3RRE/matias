let delay=10000//10 segundos
let timerId=''
let respuestaServicio=false
$(document).ready(function(){
    $("#SalaId").val(codSala)
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: new Date(),
        //maxDate: dateNow,
    });
    //listarMaquinas().then(result=>{renderSelectMaquinas(result)})
    listarTipoDocumento().then(result=>{renderizarTipoDocumento(result.data)})
    $("#NroDoc").keypress(function(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if(code==13){
            let nroDocumento=$("#NroDoc").val()
            let tipoDocumento=$("#cboTipoDoc").val()
            if(nroDocumento&&tipoDocumento){
                buscarCliente(nroDocumento,tipoDocumento)
            }
            else{
                toastr.warning('Ingrese Nro. de Documento','Mensaje')
            }
        }
    })
    $(document).on('click','#spanBuscar',function(e){
        e.preventDefault()
        let nroDocumento=$("#NroDoc").val()
        let tipoDocumento=$("#cboTipoDoc").val()
        if(nroDocumento&&tipoDocumento){
            buscarCliente(nroDocumento,tipoDocumento)
        }
        else{
            toastr.warning('Ingrese Nro. de Documento','Mensaje')
        }
    })
    $(document).on('click','.btnSeleccionarCliente',function(e){
        e.preventDefault()
        listarMaquinas().then(response=>{
            renderSelectMaquinas(response)
        })
    })
    $(document).on('click','.btnGuardarclienteNuevo', function (e) {
        e.preventDefault()
        $("#form_registro_cliente").data('bootstrapValidator').resetForm()
        var validar = $("#form_registro_cliente").data('bootstrapValidator').validate()
        if (validar.isValid()) {
            var urlenvio = ""
            var lugar = ""
            lugar = "CampaniaClienteExterno/GuardarClienteJson"
            urlenvio = basePath + "CampaniaClienteExterno/GuardarClienteJson"

            var dataForm = $('#form_registro_cliente').serializeFormJSON();
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
                        $("#modalClienteNuevo").modal("hide")
                        $("#spanBuscar").trigger("click")
                        toastr.success("Cliente Registrado", "Mensaje Servidor");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {

                }
            })

        }

    })
    $(document).on('click','#btnJugadas',function(e){
        e.preventDefault()
        let nroDocumento=$("#NroDoc").val()
        let tipoDocumento=$("#cboTipoDoc :selected").text()
        sesionesCliente(nroDocumento,tipoDocumento)
    })
    $(document).on('click','#btnGenerarSesion',function(e){
        e.preventDefault()
        generarSesion()
    })
})
function listarMaquinas(){
    return $.ajax({
        type: "POST",
        url: `${urlProgresivo}/servicio/ListadoMaquinasBDtec?estado=1`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}
function renderSelectMaquinas(data){
    $("#CodMaquina").html('')
    if(data){
        let html=data.map(item=>{
            return `<option value="${item.CodMaquina}">${item.CodMaquina} - ${item.Juego}</option>`
        })
        $("#CodMaquina").html(html.join(''))
        $("#CodMaquina").select2()
        $('.search-success').show()
        $("#modalClientes").modal('hide')
    }
}
function generarSesion(){
    let CodMaquina=$("#CodMaquina").val()
    let NroDoc=$("#NroDoc").val()
    let Mail=$("#Mail").val()
    let NombreCompleto=$("#NombreCompleto").val()
    let ClienteIdIas=$("#Id").val()
    let Nombre=$("#Nombre").val()
    let ApelPat=$("#ApelPat").val()
    let ApelMat=$("#ApelMat").val()
    let nombreTipoDocumento=$("#cboTipoDoc :selected").text()
    let tipoDocumentoId=$("#cboTipoDoc").val()
    let dataForm={
        ClienteIdIas:ClienteIdIas,
        NombreCompleto:NombreCompleto,
        NroDoc:NroDoc,
        UsuarioIas:0,
        Mail:Mail,
        Nombre:Nombre,
        ApelPat:ApelPat,
        ApelMat:ApelMat,
        NombreTipoDocumento:nombreTipoDocumento,
        TipoDocumentoId:tipoDocumentoId,
        TipoSesion:2
    }
    $.ajax({
        type: "POST",
        url: `${urlProgresivo}/servicio/IniciarSesionSorteoSalaExterno?CodMaquina=${CodMaquina}`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        data:JSON.stringify(dataForm),
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        },
        success:function(response){
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje")
            }
            else{
                toastr.error("Ha ocurrido un error","Mensaje")
            }
           sesionesCliente(NroDoc,tipoDocumentoId)
        }
    })
}
function sesionesCliente(NroDoc,tipoDocumentoId){
    obtenerSesionesCliente(NroDoc,tipoDocumentoId).then(result=>{renderizarSesionesCliente(result)})
    clearTimeout(timerId);
    timerId = setTimeout(function request() {
        obtenerSesionesCliente(NroDoc,tipoDocumentoId).then(result=>{renderizarSesionesCliente(result)})
    // if (true) {
    //   //aumentar el intervalo en la próxima ejecución
    //   delay *= 2;
    // }
    timerId = setTimeout(request, delay);
    }, delay);
}
function obtenerSesionesCliente(nroDocumento,tipoDocumento){
    return $.ajax({
        type: "GET",
        url: `${urlProgresivo}/servicio/ObtenerSesionesPorNroDocumento?nrodocumento=${nroDocumento}&tipoDocumento=${tipoDocumento}&pagina=1&cantRegistros=10`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            //$.LoadingOverlay("show")
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}
function renderizarSesionesCliente(data){
    let contenedor=$("#contenedorJugadas")
    contenedor.empty()
    if(data){
        let tbody='<tr><td colspan="11"><div class="alert alert-danger">Jugadas</div></td></tr>'
        if(data.length>0){
            tbody=data.map(item=>{
                let span=''
                var estado = "SI";
                var css = "btn-success";
                if (item.Terminado == 1) {
                    estado = "NO"
                    css = "btn-danger";
                }
                span= '<span class="label '+css+'">'+estado+'</span>';
                return `
                <tr>
                <td>${item.CodMaquina}</td>
                <td>${item.NroDocumento}</td>
                <td>${item.NombreCliente}</td>
                <td>${item.CantidadCupones}</td>
                <td>${item.CantidadJugadas}</td>
                <td>${moment(item.FechaInicio).format('DD/MM/YYYY HH:mm:ss A')}</td>
                <td>${span}</td>
                <tr>
                `
            })
                
            let tabla=`<h1 style="justify-content:center;margin-top:15px"> ULTIMAS JUGADAS</h1><hr/>
                <table class="table table-bordered table-light table-condensed" style="border:1px solid #fff;background-color: #fff;">
                    <thead>
                        <tr>
                            <th>Maquina</th>
                            <th>NroDocumento</th>
                            <th>Cliente</th>
                            <th>Cupones</th>
                            <th>Jugadas</th>
                            <th>Fecha</th>
                            <th>En Juego</th>
                        </tr>
                    </thead>
                    <tbody>${tbody.join('')}</tbody>
                </table>
                `
            contenedor.html(tabla)
        }
    }
}

function llenarInputsCliente(data){
    $(".search-success").hide()
    if(data){
        $("#Id").val(data.Id)
        $("#Nombre").val(data.Nombre)
        $("#ApelPat").val(data.ApelPat)
        $("#ApelMat").val(data.ApelMat)
        $("#NombreCompleto").val(data.NombreCompleto)
        $("#Mail").val(data.Mail)
        $(".search-success").show()
    }
}

function listarTipoDocumento(){
    return $.ajax({
        type: "POST",
        url: `${basePath}AsistenciaCliente/GetListadoTipoDocumento`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}
function renderizarTipoDocumento(data){
    $("#cboTipoDoc,#cboTipoDocumento").empty()
    $("#cboTipoDoc,#cboTipoDocumnento").append('<option value="">--Seleccione--</option>')
    $.each(data, function (index, value) {
        $("#cboTipoDoc,#cboTipoDocumnento").append('<option value="' + value.Id + '" >' + value.Nombre + '</option>');
    });
    $("#cboTipoDoc").select2({
        multiple: false, placeholder: "--Seleccione--", allowClear: true
    })
    $("#cboTipoDocumnento").select2({
        multiple: false, placeholder: "--Seleccione--", allowClear: true,dropdownParent:$("#modalClienteNuevo")
    })
}
/**Validadores */
$("#form_registro_cliente")
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
            ApelPat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelMat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            // Celular1: {
            //     validators: {
            //         notEmpty: {
            //             message: 'Ingrese Telefono, Obligatorio'
            //         }
            //     }
            // },
            NroDoc: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            FechaNacimiento: {
                validators: {
                    notEmpty: {
                        message: ' '
                    },
                    date: {
                        format: 'DD/MM/YYYY',
                        message: ' '
                    }

                }
            },
           
        }
    })                              
    .on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });1
/**End validadores */
function buscarCliente(nroDocumento,tipoDocumento){
    obtenerDataClienteServicio(nroDocumento,tipoDocumento).then(response=>{
        if(respuestaServicio){
            if(response.data.Id>0){
                llenarInputsCliente(response.data)
                listarMaquinas().then(res=>{
                    renderSelectMaquinas(res)
                    sesionesCliente(nroDocumento,tipoDocumento)
                })
            }
            else{
                obtenerDataClienteAzure(nroDocumento,tipoDocumento).then(result=>{
                    result=result.data
                    if(result.respuesta){
                        if(result.data.Id>0||result.reniec){
                            result.data.Id=0
                            llenarInputsCliente(result.data)
                            listarMaquinas().then(res=>{
                                renderSelectMaquinas(res)
                                sesionesCliente(nroDocumento,tipoDocumento)
                            })
                        }
                    }
                    else{
                        $("#modalClienteNuevo").modal('show')
                    }
                })
            }
        }
        else{
            toastr.error('No se puede conectar con el servicio, contacte con administrador','Mensaje')
        }
    })
    $('#modalClienteNuevo').on('shown.bs.modal', function (e) {
        $("#txtNroDoc").val($("#NroDoc").val())
        let tipodoc=$("#cboTipoDoc").val()
        console.log(tipodoc)
        $("#cboTipoDocumnento").val(parseInt(tipodoc)).trigger('change')
    })
}
function obtenerDataClienteServicio(nroDocumento,tipoDocumento){
    $.LoadingOverlay("show")
    return axios.get(`${urlProgresivo}servicio/ObtenerCliente?nroDocumento=${nroDocumento}&tipodocumento=${tipoDocumento}`)
    .then(function (response) {
        // handle success
        if(response){
            respuestaServicio=true
        }
        return response
    })
    .catch(function (error) {
        // handle error
        return false
    })
    .finally(function () {
        // always executed
        $.LoadingOverlay("hide")
    })
    // return $.ajax({
    //     type: "GET",
    //     url: `${urlProgresivo}servicio/ObtenerCliente?nroDocumento=${nroDocumento}&tipodocumento=${tipoDocumento}`,
    //     contentType: "application/json; charset=utf-8",
    //     dataType: "json",
    //     processdata: true,
    //     cache: false,
    //     beforeSend: function ()
    //     {
    //         $.LoadingOverlay("show")
    //     },
    //     complete: function ()
    //     {
    //         $.LoadingOverlay("hide")
    //     }
    // })
}
function obtenerDataClienteAzure(nroDocumento,tipoDocumento){
    let dataForm={
        nroDocumento:nroDocumento,
        tipodocumento:tipoDocumento,
        CodSala:codSala
    }
    $.LoadingOverlay("show")
    return axios.post(
        `${basePath}CampaniaClienteExterno/ObtenerCliente`, 
        dataForm
    )
    .then(function (response) {
        return response
    })
    .catch(function (error) {
        return false
    })
    .finally(function(){
        $.LoadingOverlay("hide")
    })
    // return $.ajax({
    //     type: "POST",
    //     url: `${basePath}CampaniaClienteExterno/ObtenerCliente`,
    //     contentType: "application/json; charset=utf-8",
    //     dataType: "json",
    //     processdata: true,
    //     cache: false,
    //     data:JSON.stringify(dataForm),
    //     beforeSend: function ()
    //     {
    //         $.LoadingOverlay("show")
    //     },
    //     complete: function ()
    //     {
    //         $.LoadingOverlay("hide")
    //     }
    // })
}
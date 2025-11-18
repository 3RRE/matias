
$(document).ready(function () {
    
    //const tituloPersonal = document.querySelector('.unselected-title');
    //tituloPersonal.style.fontWeight = "700";
    listarPersonalActivoSinComite();
    datainicial = "";
    function listarPersonalComiteActual() {

        if (document.querySelector('select.selected') != null) {
            var cbPersonal2 = document.querySelector('select.selected');
            /* https://api.jqueryui.com/1.11/tooltip/ */
            let atr = document.querySelector('.atr');
            atr.dataset.toggle = "tooltip";
            atr.dataset.title = "Agregar Todos";
            $(".atr").tooltip();

            let str = document.querySelector('.str');
            str.dataset.toggle = "tooltip";
            str.dataset.title = "Agregar uno";
            $(".str").tooltip();

            let stl = document.querySelector('.stl');
            atr.dataset.toggle = "tooltip";
            atr.dataset.title = "Quitar uno";
            $(".stl").tooltip();

            let atl = document.querySelector('.atl');
            atl.dataset.toggle = "tooltip";
            atl.dataset.title = "Quitar Todos";
            $(".atl").tooltip();

        $(".selected").children().remove();
        var url = basePath + "GestionCambios/ComiteCambiosListadoJson";
        var data = {};
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            /*beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },*/
            success: function (response) {
                data = response.data;
                //$("#cbmpersonal").children().remove();
                if (data.length > 0) {
                    var btnRegresarTodos = document.querySelector('button.atl');
                    btnRegresarTodos.disabled = false;
                }
                //$("#cbmpersonal").bootstrapDualListbox('refresh', true);
                var selects = '';
                for (var i = 0; i < data.length; i++) {
                    //selects += '<option value="' + data[i].EmpleadoID + '">' + data[i].NombreCompleto+'</option>';
                    var opt = document.createElement('option');
                    opt.value = data[i].EmpleadoID;
                    opt.innerHTML = data[i].NombreCompleto;
                    cbPersonal2.appendChild(opt);
                }
                var texto = [];
                var id = [];
                for (var i = 0; i < data.length; i++) {
                    texto.push(data[i].NombreCompleto);
                    id.push(data[i].EmpleadoID);
                }
                datainicial = {
                    Miembros: id.toString(),
                    Miembros_Text: texto.toString(),
                }; console.log(JSON.stringify(datainicial))
               
                dataAuditoriaJSON(3, "GestionCambios/GestionCambiosComiteCambiosPersonalListadoVista", "VISTA", "", "");

            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                console.log("errorrrrrrrr");
            }
        });
        }
    }

    
    $("#GuardarComite").on("click", function (e) {
        //e.preventDefault();
        toastr.success('Se actualizo los miembros del Comite');
        var opt = $(".selected option");
        var texto = [];
        var id = [];
        $(opt).each(function () {
            texto.push($(this).text());
            id.push($(this).val());
        });
        var datafinal = {
            Miembros: id.toString(),
            Miembros_Text: texto.toString(),
        }; 

        dataAuditoriaJSON(3, "GestionCambios/ComiteCambiosGuardarJson", "ACTUALIZAR COMITE", datainicial, datafinal);
        
    });
    $("#btnCancelar").on("click", function (e) {
        listarPersonalActivoSinComite();

    });
    
    function listarPersonalActivoSinComite() {
        var cbPersonal = document.querySelector('select.unselected');
        var url = basePath + "GestionCambios/ComiteCambiosEmpleadoListadoJson";
        var data = {};
        $(".unselected").children().remove();
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            /*beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },*/
            success: function (response) {
                data = response.data;
                //$("#cbmpersonal").children().remove();
                //$("#cbmpersonal").bootstrapDualListbox('refresh', true);
                var selects = '';
                for (var i = 0; i < data.length; i++) {
                    //selects += '<option value="' + data[i].EmpleadoID + '">' + data[i].NombreCompleto+'</option>';
                    var opt = document.createElement('option');
                    opt.value = data[i].EmpleadoID;
                    opt.innerHTML = data[i].NombreCompleto;
                    cbPersonal.appendChild(opt);
                }
                //cbPersonal.innerHTML = selects;
                $("#cbmpersonal").DualListBox();
                listarPersonalComiteActual();
                $('.unselected-title').text('Personal Disponible');
                $('.selected-title').text('Miembros del Comite');
            },

            error: function (xmlHttpRequest, textStatus, errorThrow) {
                console.log("errorrrrrrrr");
            }
        });
    };


});

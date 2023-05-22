$(document).ready(function () {

    $(".card-body").LoadingOverlay("show");

    fetch("/Negocio/Obtener")
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.estado) {
                const d = responseJson.objeto
                $("txtNumeroDocumento").val(d.numerodocumento)
                $("txtRazonSocial").val(d.nombre)
                $("txtCorreo").val(d.correo)
                $("txtDireccion").val(d.direccion)
                $("txTelefono").val(d.telefono)
                $("txtImpuesto").val(d.porcentajeImpuesto)
                $("txtSimboloMoneda").val(d.simboloMoneda)
                $("imgLogo").attr("src", d.UrlLogo)

            }
            else {

                swal("Lo sentimos", responseJson.mensaje, "error")
            }
        })

})

$("#btnGuardarCambios").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo:"${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    const modelo = {
        numerodocumento: $("txtNumeroDocumento").val(),
        nombre:$("txtRazonSocial").val(),
        Correo:$("txtCorreo").val(),    
        Direccion:$("txtDireccion").val(),   
        Telefono:$("txTelefono").val(),   
        Impuesto:$("txtImpuesto").val(),     
        SimboloMoneda:$("txtSimboloMoneda").val()                              
    }

    const imputLog = document.getElementById("imgLogo")

    const formData = new FormData()
    formData.append("logo", inputFoto.files[0])
    formData.append("modelo", JSON.stringify(modelo))

    $(".card-body").LoadingOverlay("show");

    fetch("/Negocio/GuardarCambios", {
        method: "POST",
        body: formData
        })
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.estado) {
                const d = responseJson.objeto
                $("imgLogo").attr("src", d.UrlLogo)

            }
            else {

                swal("Lo sentimos", responseJson.mensaje, "error")
            }
        })

})

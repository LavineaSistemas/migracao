function msgalert(){
    Swal.fire({
        title: "<h4>Aguarde, estamos processando.</h4>",
        // html: '<img src="/images/vertical_loading.gif" alt="Carregando" width="200" />',
        html: '<div class="row"><div class="processa"><div class="bola"></div><div class="bola"></div><div class="bola"></div>'+
            '<div class="sombra"></div><div class="sombra"></div><div class="sombra"></div></div>',
        footer: '<span class="text-center"></span>',
        showConfirmButton: false,
    });
}
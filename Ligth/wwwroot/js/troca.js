function troca() {
    Swal.fire({
        title: 'Alterar senha',
        html: `
                  <input type="password" id="nova" class="swal2-input" placeholder="Nova senha">
                  <input type="password" id="novarepete" class="swal2-input" placeholder="Repita a nova senha">
                `,
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        showLoaderOnConfirm: true,
        preConfirm: () => {
            const nova = Swal.getPopup().querySelector('#nova').value;
            const novarepete = Swal.getPopup().querySelector('#novarepete').value;
            
            if (!nova || !novarepete) {
                Swal.showValidationMessage('Por favor, preencha todos os campos');
            }
            
            if (novarepete !== nova){
                Swal.showValidationMessage('O segundo valor não confere com o primeiro');
            }
            
            if (nova.length < 6) {
                Swal.showValidationMessage('Informe no mínimo 6 caracteres');
            }
            return { nova: nova, novarepete: novarepete };
        },
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        if (result != null) {
            $.ajax({
                url: '/Usuarios/Trocar/',
                type: 'post',
                data: JSON.stringify({ nova: result.value.nova, novarepete: result.value.novarepete }),
                contentType: "application/json; charset=UTF-8",
                success: function (data) {
                    Msgtoast('Senha alterada com sucesso', 1);
                },
                error: function () {
                    Msgtoast('Oops! Algo não saiu bem', 2);
                }
            });
        }
    });
}
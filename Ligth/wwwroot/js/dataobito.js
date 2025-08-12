var table = null;
let livro = 0;

if (table == null) {
    CarregaTabela(livro);
}

$('#btnLivro').click(function () {
    livro = $('#livro').val();
    table.clear().draw();
    table.destroy();
    CarregaTabela(livro);
});

function CarregaTabela(livro) {
    table = $('#dataobito').DataTable({
        "pageLength": '100',
        "processing": true,
        "serverSide": true,
        "language" : {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },
        "ajax": {
            "url": "/Obito/GetObitos/",
            "type": "POST",
            "dataType": "json",
            "data" : function ( d ) {
                d.id = livro;
            },
            // "success" : function (data) {
            //     console.log(data);    
            // },
        },
        "rowId": "id",
        "columns": [
            { "data": "id",
                "className":"text-center"
            },
            { "data": "numliv",
                "className": "text-center"
            },
            { "data": "numpag",
                "className": "text-center"
            },
            { "data": "registro",
                "className": "text-center"
            },
            { "data": "nome"},
            { "data": "dataregistro"},
            { "data": "id",
                "orderable":false,
                "render": function (data, type, row) {
                    return botoesDeAcao(data, type, row);
                }
            }
        ]
    });
}

function botoesDeAcao(data, type, row) {
    // Inicia a construção do HTML para o grupo de botões
    let renderAcoes =
        '<td align="center">' +
        '<div class="btn-group">' +
        '<a class="btn btn-info dropdown-toggle" data-toggle="dropdown" href="#" aria-haspopup="true" aria-expanded="true">' +
        '<i class="fa fa-cogs"></i> <span class="caret"></span>' +
        '</a>' +
        '<div class="dropdown-menu">';

    // Fecha as tags do dropdown e do grupo de botões
    renderAcoes += '</div></div></td>';
    return renderAcoes;
}
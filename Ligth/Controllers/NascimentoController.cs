using System.Text;
using Ligth.Dao;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

public class NascimentoController : Controller
{
    private readonly ILogger<NascimentoController> _logger;

    public NascimentoController(ILogger<NascimentoController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult ListarNascimento()
    {
        ViewBag.Locais = new LocalDAO().GetAllLocais();
        ViewBag.Titulo = "Lista de Nascimentos";
        return View();
    }
    
    public IActionResult CarregarColunas(int idLocal)
    {
        var msgToast = new MsgToast();
        try
        {
            var local = new LocalDAO().GetLocal(idLocal);
            var dir = new DiretorioDAO().getDiretorioNascimento(local.Id);
            _logger.LogInformation($"Base de dados: {local.Schema} - Diretorio arquivo: {dir}");
            using (StreamReader leitor = new StreamReader(dir, Encoding.GetEncoding("ISO-8859-1")))
            {
                string linha;
                var idaux = 0;
                var colunas = new List<string>();
                var indice = new NascimentoDAO().getIndice(local.Schema);
                while ((linha = leitor.ReadLine()) != null)
                {
                    if (linha != "!C25")
                    {
                        var id = int.Parse(linha.Substring(1,8));
                        if (id > indice)
                        {
                            var coluna = linha.Substring(9,15).TrimEnd().ToLower();
                            var dado = linha.Substring(24, linha.Length-24).TrimEnd().Replace("'","");
                            var addRegistro = false;
                            if (id != idaux)
                            {
                                idaux = id;
                                addRegistro = new NascimentoDAO().addRegistro(local.Schema, id);
                            }
                            var addColuna = false;
                            if (!colunas.Contains(coluna))
                            {
                                addColuna = new NascimentoDAO().addColuna(local.Schema, coluna);
                                colunas.Add(coluna);
                            }
                        
                            var addDado = new NascimentoDAO().addDado(local.Schema, id, coluna, dado);
                            var texto = $"Registro: {id} - " + (addRegistro ? "adicionado" : "já está adicionado") +
                                        $" | Coluna: {coluna} - " + (addColuna ? "adicionado" : "já está adicionado") +
                                        $" | Valor: {dado} - " + (addDado ? "adicionado" : "já está adicionado");
                            _logger.LogInformation(texto);
                        }
                    }
                }
            }
            _logger.LogInformation($"Carregamento finalizado");
            msgToast.Tipo = 1;
            msgToast.Mensagem = "Carga de colunas e registros, realizado com sucesso.";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            msgToast.Tipo = 2;
            msgToast.Mensagem = e.Message;
        }
        ViewBag.MsgToast = msgToast;
        ViewBag.Locais = new LocalDAO().GetAllLocais();
        return View("ListarNascimento");
    }

    public IActionResult ImportarNascimento()
    {
        var msgToast = new MsgToast();
        try
        {
            var local = new LocalDAO().GetLocal(1);
            var dir = new DiretorioDAO().getDiretorioNascimento(local.Id);
            using (StreamReader leitor = new StreamReader(dir))
            {
                // string linha;
                // while ((linha = leitor.ReadLine()) != null)
                // {
                //     var id = int.Parse(linha.Substring(1,8));
                //     var coluna = linha.Substring(10,15).TrimEnd();
                //     var dado = linha.Substring(26, 500).TrimEnd();
                //     var isRegistro = new NascimentoDAO().achouRegistro(local.Schema, id);
                //     if (!isRegistro)
                //     {
                //         new NascimentoDAO().addRegistro(local.Schema, id);
                //     }
                //     else
                //     {
                //         var nascimento = new Nascimento();
                //     }
                //     var isColuna = new NascimentoDAO().achouColuna(local.Schema, coluna);
                // }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            msgToast.Tipo = 2;
            msgToast.Mensagem = e.Message;
        }
        ViewBag.Message = msgToast;
        return View("ListarNascimento");
    }
}
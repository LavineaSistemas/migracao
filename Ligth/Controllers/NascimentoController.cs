using System.Text;
using Ligth.Dao;
using Ligth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

[Authorize]
public class NascimentoController : Controller
{
    private readonly ILogger<NascimentoController> _logger;

    public NascimentoController(ILogger<NascimentoController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult ListarNascimento()
    {
        try
        {
            ViewBag.Titulo = "Lista de Nascimentos";
            int idlocal = int.Parse(User.Claims.First(c => c.Type == "local").Value);
            string schema = new LocalDAO().GetNomeLocal(idlocal);
            ViewBag.Perfil = User.Claims.First(c => c.Type == "perfil").Value;
            ViewBag.Livros = new NascimentoDAO().GetAllLivros(schema);
            ViewBag.IdLocal = idlocal;
            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TempData["Codigo"] = 2;
            TempData["Mensagem"] = e.Message;
            return RedirectToAction("Index", "Home");
        }
        
        ViewBag.Locais = new LocalDAO().GetAllLocais();
        
    }
    
    public IActionResult CarregarColunas(int idLocal)
    {
        var msgToast = new MsgToast();
        try
        {
            var local = new LocalDAO().GetLocal(idLocal);
            var dir = $"wwwroot/arquivos/{local.Schema}/NASC.TXT";
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

    [HttpPost]
    [AllowAnonymous]
    public IActionResult GetNascimentos(int? length, int? draw, int? start)
    {
        var id = Request.Form["id"].FirstOrDefault();
        var search = Request.Form["search[value]"].FirstOrDefault();
        int recordsTotal = 0;
        int recordsFiltered = 0;
        start = start.HasValue ? start / 100 : 0;
        int idlocal = int.Parse(User.Claims.First(c => c.Type == "local").Value);
        string schema = new LocalDAO().GetNomeLocal(idlocal);
        var data = new NascimentoDAO().ListaNascimentoFiltro(search, start.Value, length ?? 100, out recordsTotal, out recordsFiltered, id, schema);
        return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
    }
}
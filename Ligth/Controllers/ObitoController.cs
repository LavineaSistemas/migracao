using System.Text;
using Ligth.Dao;
using Ligth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

public class ObitoController : Controller
{
    private readonly ILogger<ObitoController> _logger;

    public ObitoController(ILogger<ObitoController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListarObitos()
    {
        try
        {
            ViewBag.Titulo = "Lista de Óbitos";
            int idlocal = int.Parse(User.Claims.First(c => c.Type == "local").Value);
            string schema = new LocalDAO().GetNomeLocal(idlocal);
            ViewBag.Perfil = User.Claims.First(c => c.Type == "perfil").Value;
            ViewBag.Livros = new CasamentoCivilDAO().GetAllLivros(schema);
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
    }
    
    public IActionResult CarregarColunasObito(int idLocal)
    {
        var msgToast = new MsgToast();
        var tabela = "obitos";
        try
        {
            var local = new LocalDAO().GetLocal(idLocal);
            var dir = $"wwwroot/arquivos/{local.Schema}/obito.txt";
            _logger.LogInformation($"Base de dados: {local.Schema} - Diretorio arquivo: {dir}");
            using (StreamReader leitor = new StreamReader(dir, Encoding.GetEncoding("ISO-8859-1")))
            {
                string linha;
                var idaux = 0;
                var colunas = new List<string>();
                var indice = new RegistroDAO().getIndice(local.Schema, "obito");
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
                                addRegistro = new RegistroDAO().addRegistro(local.Schema, tabela, id);
                            }
                            var addColuna = false;
                            if (coluna == "do")
                            {
                                coluna = "doo";
                            }
                            
                            if (!colunas.Contains(coluna))
                            {
                                addColuna = new RegistroDAO().addColuna(local.Schema, tabela, coluna);
                                colunas.Add(coluna);
                            }
                        
                            var addDado = new RegistroDAO().addDado(local.Schema, tabela, id, coluna, dado);
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
        return RedirectToAction("ListarObitos");
    }
    
    [HttpPost]
    [AllowAnonymous]
    public IActionResult GetObito(int? length, int? draw, int? start)
    {
        var id = Request.Form["id"].FirstOrDefault();
        var search = Request.Form["search[value]"].FirstOrDefault();
        int recordsTotal = 0;
        int recordsFiltered = 0;
        start = start.HasValue ? start / 100 : 0;
        int idlocal = int.Parse(User.Claims.First(c => c.Type == "local").Value);
        string schema = new LocalDAO().GetNomeLocal(idlocal);
        var data = new ObitoDAO().ListaObitoFiltro(search, start.Value, length ?? 100, out recordsTotal, out recordsFiltered, id, schema);
        return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
    }
    
    public IActionResult ListarCasamentoReligioso()
    {
        try
        {
            ViewBag.Titulo = "Lista de Casamentos Religioso";
            int idlocal = int.Parse(User.Claims.First(c => c.Type == "local").Value);
            string schema = new LocalDAO().GetNomeLocal(idlocal);
            ViewBag.Perfil = User.Claims.First(c => c.Type == "perfil").Value;
            ViewBag.Livros = new CasamentoReligiosoDAO().GetAllLivros(schema);
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
    }
    
    public IActionResult CarregarColunasReligioso(int idLocal)
    {
        var msgToast = new MsgToast();
        var tabela = "casamentosreligiosos";
        try
        {
            var local = new LocalDAO().GetLocal(idLocal);
            var dir = $"wwwroot/arquivos/{local.Schema}/casamentoreligioso.txt";
            _logger.LogInformation($"Base de dados: {local.Schema} - Diretorio arquivo: {dir}");
            using (StreamReader leitor = new StreamReader(dir, Encoding.GetEncoding("ISO-8859-1")))
            {
                string linha;
                var idaux = 0;
                var colunas = new List<string>();
                var indice = new RegistroDAO().getIndice(local.Schema, "casamentoreligioso");
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
                                addRegistro = new RegistroDAO().addRegistro(local.Schema, tabela, id);
                            }
                            var addColuna = false;
                            if (!colunas.Contains(coluna))
                            {
                                addColuna = new RegistroDAO().addColuna(local.Schema, tabela, coluna);
                                colunas.Add(coluna);
                            }
                        
                            var addDado = new RegistroDAO().addDado(local.Schema, tabela, id, coluna, dado);
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
        return RedirectToAction("ListarCasamentoReligioso");
    }
    
    [HttpPost]
    [AllowAnonymous]
    public IActionResult GetCasamentoReligioso(int? length, int? draw, int? start)
    {
        var id = Request.Form["id"].FirstOrDefault();
        var search = Request.Form["search[value]"].FirstOrDefault();
        int recordsTotal = 0;
        int recordsFiltered = 0;
        start = start.HasValue ? start / 100 : 0;
        int idlocal = int.Parse(User.Claims.First(c => c.Type == "local").Value);
        string schema = new LocalDAO().GetNomeLocal(idlocal);
        var data = new CasamentoReligiosoDAO().ListaCasamentoReligiosoFiltro(search, start.Value, length ?? 100, out recordsTotal, out recordsFiltered, id, schema);
        return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
    }
}
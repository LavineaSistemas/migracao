using Ligth.Dao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

[Authorize]
public class CasamentoController : Controller
{
    private readonly ILogger<CasamentoController> _logger;
    
    public CasamentoController(ILogger<CasamentoController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult ListarCasamentoCivil()
    {
        try
        {
            ViewBag.Titulo = "Lista de Casamentos Civis";
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
}
using CJCartorioCore5.Utils;
using Ligth.Dao;
using Ligth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

[Authorize]
public class UsuariosController : Controller
{
    public IActionResult ListarUsuarios()
    {
        var model = new UsuariosDAO().GetAll();
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult AddUsuario([FromBody] Usuarios model)
    {
        try
        {
            model.status = true;
            model.senha = Cripto.EncSenha("Cartorio@123");
            new UsuariosDAO().AddUsuario(model);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
    
    [HttpPost]
    public IActionResult Trocar([FromBody] Usuarios model)
    {
        try
        {
            var id = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var model2 = new UsuariosDAO().GetUsuarioId(id);
            model2.senha = Cripto.EncSenha(model.nova);
            new UsuariosDAO().UpdSenhaUsuario(model2);
            return Ok("Nova senha cadastrada com sucesso");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            TempData["Codigo"] = 2;
            TempData["Mensagem"] = e.Message;
            return BadRequest(e.Message);
        }
    }
}
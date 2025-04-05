using CJCartorioCore5.Utils;
using Ligth.Dao;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

public class UsuariosController : Controller
{
    public IActionResult ListarUsuarios()
    {
        var model = new UsuariosDAO().GetAll();
        return View(model);
    }

    [HttpPost]
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
}
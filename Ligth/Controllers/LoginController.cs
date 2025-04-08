using System.Security.Claims;
using System.Text.Json;
using CJCartorioCore5.Utils;
using Ligth.Dao;
using Ligth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ligth.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        this.ViewData["ReturnUrl"] = null;
        ViewData["Title"] = "Login";
        ViewBag.Locais = new LocalDAO().GetAllLocais();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(IFormCollection form)
    {
        var msgToast = new MsgToast();
        try
        {
            int local = int.Parse(form["Local"]);
            string logUser = form["login"];
            string senha = form["senha"];
            var usuario = new UsuariosDAO().GetUsuario(local, logUser);
            if (usuario == null)
            {
                return RedirectToAction("Index");
            }

            if (!usuario.status)
            {
                msgToast.Tipo = 2;
                msgToast.Mensagem = "Usuário bloqueado";
                return RedirectToAction("Index");
            }
            
            string senhaUsuario = Cripto.Decrypt(usuario.senha);
            if (senhaUsuario != senha)
            {
                TempData["Codigo"] = 2;
                TempData["Mensagem"] = "Senha incorreta";
                return RedirectToAction("Index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.nome),
                new Claim(ClaimTypes.NameIdentifier, usuario.cpf, ClaimValueTypes.String),
                new Claim(ClaimTypes.GivenName, usuario.nome, ClaimValueTypes.String)
            };
            
            var userIdentity = new ClaimsIdentity(claims, "Passport");
            userIdentity.AddClaim(new Claim("id", usuario.id.ToString()));
            userIdentity.AddClaim(new Claim("nomeusuario", usuario.nome));
            userIdentity.AddClaim(new Claim("perfil", usuario.perfil));
            userIdentity.AddClaim(new Claim("local", local.ToString()));
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
            
            msgToast.Tipo = 1;
            msgToast.Mensagem = "Usuário logado com sucesso";
            ViewBag.Toast = JsonSerializer.Serialize(msgToast);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            msgToast.Tipo = 2;
            msgToast.Mensagem = e.Message;
            _logger.LogError(e.Message);
        }
        ViewBag.Toast = JsonSerializer.Serialize(msgToast);
        return RedirectToAction("Index");
    }
    
    [Authorize]
    public async Task<IActionResult> EncerraSessao()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index");
    }
}
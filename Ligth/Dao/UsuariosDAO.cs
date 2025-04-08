using Dapper;
using Ligth.Models;

namespace Ligth.Dao;

public class UsuariosDAO
{
    public IEnumerable<Usuarios> GetAll()
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<Usuarios>("SELECT * FROM adm.usuarios;");
        }
    }

    public void AddUsuario(Usuarios model)
    {
        using (var _db = new DBDapper().getCon)
        {
            _db.Execute("insert into adm.usuarios (nome, cpf, perfil, status, senha, id_local) values (@nome, @cpf, @perfil, @status, @senha, @id_local)", model);
        }
    }

    public Usuarios GetUsuario(int local, string login)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.QueryFirst<Usuarios>("SELECT * FROM adm.usuarios WHERE id_local=@local and cpf=@login;", new { local, login });
        }
    }
    
    public Usuarios GetUsuarioId(int id)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.QueryFirst<Usuarios>("SELECT * FROM adm.usuarios WHERE id=@id;", new { id });
        }
    }

    public void UpdSenhaUsuario(Usuarios model)
    {
        using (var _db = new DBDapper().getCon)
        {
            _db.Execute("update adm.usuarios set senha=@senha where id=@id", model);
        }
    }
}
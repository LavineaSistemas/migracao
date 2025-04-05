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
            _db.Execute("insert into adm.usuarios (nome, cpf, perfil, status, senha) values (@nome, @cpf, @perfil, @status, @senha)", model);
        }
    }
}
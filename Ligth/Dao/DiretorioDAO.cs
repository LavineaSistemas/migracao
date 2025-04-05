using Dapper;
using Ligth.Models;

namespace Ligth.Dao;

public class DiretorioDAO
{
    public string getDiretorioNascimento(int id)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.QueryFirstOrDefault<string>("SELECT dir_nascimento FROM adm.diretorios where id_local=@id;", new { id });
        }
    }
}
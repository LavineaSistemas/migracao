using Dapper;
using Ligth.Dao;

namespace Ligth.Controllers;

public class RegistroDAO
{
    public bool addRegistro(string db, string tb, int id)
    {
        var realizou = false;
        using (var _db = new DBDapper().getCon)
        {
            var sql = $"select id from {db}.{tb} where id=@id;";
            if (!_db.Query<int>(sql, new { id }).Any())
            {
                sql = $"insert into {db}.{tb} (id) values (@id);";
                _db.Execute(sql, new { id });
                realizou = true;
            }
        }
        return realizou;
    }
    
    public bool addColuna(string db, string tb, string coluna)
    {
        var realizou = false;
        using (var _db = new DBDapper().getCon)
        {
            var sql =
                $"select column_name from information_schema.columns where table_schema='{db}' and table_name='{tb}' and column_name='{coluna}';";
            if (!_db.Query<string>(sql).Any())
            {
                if (coluna == "averbacao")
                {
                    _db.Execute($"alter table {db}.{tb}\n    add {coluna} text;");
                }
                else
                {
                    _db.Execute($"alter table {db}.{tb}\n    add {coluna} varchar(200);");
                }
               
                realizou = true;
            }
        }
        return realizou;
    }
    
    public bool addDado(string db, string tb, int id, string coluna, string dado)
    {
        var realizou = false;
        using (var _db = new DBDapper().getCon)
        {
            var sql = $"select {coluna} from {db}.{tb} where id={id};";
            var rs = _db.QueryFirstOrDefault<string>(sql);
            if (rs == null || coluna == "averbacao")
            {
                if (coluna == "averbacao")
                {
                    var dadoAux = string.Concat(rs, " ", dado);
                    dado = dadoAux;
                }
                sql = $"update {db}.{tb} set {coluna}='{dado}' where id={id};";
                _db.Execute(sql);
                realizou = true;
            }
        }
        return realizou;
    }
}
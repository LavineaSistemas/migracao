using Dapper;
using Ligth.Models;

namespace Ligth.Dao;

public class NascimentoDAO
{
    public bool addColuna(string db, string coluna)
    {
        var realizou = false;
        using (var _db = new DBDapper().getCon)
        {
            var sql =
                $"select column_name from information_schema.columns where table_schema='{db}' and table_name='nascimentos' and column_name='{coluna}';";
            if (!_db.Query<string>(sql).Any())
            {
                if (coluna == "averbacao")
                {
                    _db.Execute($"alter table {db}.nascimentos\n    add {coluna} text;");
                }
                else
                {
                    _db.Execute($"alter table {db}.nascimentos\n    add {coluna} varchar(200);");
                }
               
                realizou = true;
            }
        }
        return realizou;
    }
    
    public bool addRegistro(string db, int id)
    {
        var realizou = false;
        using (var _db = new DBDapper().getCon)
        {
            var sql = $"select id from {db}.nascimentos where id=@id;";
            if (!_db.Query<int>(sql, new { id }).Any())
            {
                sql = $"insert into {db}.nascimentos (id) values (@id);";
                _db.Execute(sql, new { id });
                realizou = true;
            }
        }
        return realizou;
    }
    
    public bool addDado(string db, int id, string coluna, string dado)
    {
        var realizou = false;
        using (var _db = new DBDapper().getCon)
        {
            var sql = $"select {coluna} from {db}.nascimentos where id={id};";
            var rs = _db.QueryFirstOrDefault<string>(sql);
            if (rs == null || coluna == "averbacao")
            {
                if (coluna == "averbacao")
                {
                    var dadoAux = string.Concat(rs, " ", dado);
                    dado = dadoAux;
                }
                sql = $"update {db}.nascimentos set {coluna}='{dado}' where id={id};";
                _db.Execute(sql);
                realizou = true;
            }
        }
        return realizou;
    }
    
    public Nascimento getRegistro(string db, int id)
    {
        using (var _db = new DBDapper().getCon)
        {
            var sql = $"select * from {db}.nascimentos where id=@id;";
            return _db.QueryFirstOrDefault<Nascimento>(sql, new { id });
        }
    }

    public int getIndice(string db)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.QueryFirst<int>(@"select nascimento from {db}.controle_registro where id=1;");
        }
    }
}


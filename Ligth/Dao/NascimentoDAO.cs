using Dapper;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            return _db.QueryFirst<int>($"select nascimento from {db}.controle_registro where id=1;");
        }
    }
    
    public IEnumerable<SelectListItem> GetAllLivros(string db)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<Nascimento>($"select numlivro from {db}.nascimentos group by numlivro order by numlivro;")
                .Select(x => new SelectListItem { Text = x.numlivro, Value = x.numlivro });
        }
    }

    public IEnumerable<Nascimento> ListaNascimentoFiltro(string filter, int initialPage, int pageSize,
        out int recordsTotal, out int recordsFiltered, string id, string db)
    {
        List<Nascimento> recs = new List<Nascimento>();
        List<Nascimento> recTB = new List<Nascimento>();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            filter = "%" + filter + "%";
        }
        else
        {
            recs = GetAllNascimentos(db, id).ToList();
        }
        
        recordsTotal = recs.Count();
        foreach (var i in recs.OrderBy(x => x.numlivro).ThenBy(x => x.pagina)
                     .Skip((initialPage * pageSize)).Take(pageSize))
        {
            recTB.Add(i);
        }
        recordsFiltered = recTB.Count();
        return recTB;
    }

    public IEnumerable<Nascimento> GetAllNascimentos(string db, string livro)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<Nascimento>($"select * from {db}.nascimentos where numlivro=@livro;", new { livro });
        }
    }
}


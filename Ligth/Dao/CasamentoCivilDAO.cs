using Dapper;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ligth.Dao;

public class CasamentoCivilDAO
{
    public IEnumerable<SelectListItem> GetAllLivros(string db)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<CasamentoCivil>($"select numero from {db}.casamentoscivis group by numero order by numero;")
                .Select(x => new SelectListItem { Text = x.numero, Value = x.numero });
        }
    }
    
    public int getIndice(string db)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.QueryFirst<int>($"select casamentocivil from {db}.controle_registro where id=1;");
        }
    }
    
    public IEnumerable<CasamentoCivil> ListaCasamentoCivilFiltro(string filter, int initialPage, int pageSize,
        out int recordsTotal, out int recordsFiltered, string id, string db)
    {
        List<CasamentoCivil> recs = new List<CasamentoCivil>();
        List<CasamentoCivil> recTB = new List<CasamentoCivil>();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            filter = "%" + filter + "%";
        }
        else
        {
            recs = GetAllCasamentoCivil(db, id).ToList();
        }
        
        recordsTotal = recs.Count();
        foreach (var i in recs.OrderBy(x => x.numero).ThenBy(x => x.pagina)
                     .Skip((initialPage * pageSize)).Take(pageSize))
        {
            recTB.Add(i);
        }
        recordsFiltered = recTB.Count();
        return recTB;
    }

    public IEnumerable<CasamentoCivil> GetAllCasamentoCivil(string db, string livro)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<CasamentoCivil>($"select * from {db}.casamentoscivis where numero=@livro;", new { livro });
        }
    }
}
using Dapper;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ligth.Dao;

public class ObitoDAO
{
    public IEnumerable<SelectListItem> GetAllLivros(string db)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<Obito>($"select numero from {db}.obitos group by numero order by numero;")
                .Select(x => new SelectListItem { Text = x.numero, Value = x.numero });
        }
    }
    
    public IEnumerable<Obito> ListaObitoFiltro(string filter, int initialPage, int pageSize,
        out int recordsTotal, out int recordsFiltered, string id, string db)
    {
        List<Obito> recs = new List<Obito>();
        List<Obito> recTB = new List<Obito>();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            filter = "%" + filter + "%";
        }
        else
        {
            recs = GetAllObito(db, id).ToList();
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

    public IEnumerable<Obito> GetAllObito(string db, string livro)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<Obito>($"select * from {db}.obitos where numero=@livro;", new { livro });
        }
    }
}
using Dapper;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ligth.Dao;

public class CasamentoReligiosoDAO
{
    public IEnumerable<SelectListItem> GetAllLivros(string db)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<CasamentoReligioso>($"select numero from {db}.casamentosreligiosos group by numero order by numero;")
                .Select(x => new SelectListItem { Text = x.numero, Value = x.numero });
        }
    }
    
    public IEnumerable<CasamentoReligioso> ListaCasamentoReligiosoFiltro(string filter, int initialPage, int pageSize,
        out int recordsTotal, out int recordsFiltered, string id, string db)
    {
        List<CasamentoReligioso> recs = new List<CasamentoReligioso>();
        List<CasamentoReligioso> recTB = new List<CasamentoReligioso>();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            filter = "%" + filter + "%";
        }
        else
        {
            recs = GetAllCasamentoReligioso(db, id).ToList();
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

    public IEnumerable<CasamentoReligioso> GetAllCasamentoReligioso(string db, string livro)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<CasamentoReligioso>($"select * from {db}.casamentosreligiosos where numero=@livro;", new { livro });
        }
    }
}
using Dapper;
using Ligth.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ligth.Dao;

public class LocalDAO
{
    public Local GetLocal(int id)
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.QueryFirstOrDefault<Local>("SELECT * FROM adm.locais WHERE id = @id;", new { id });
        }
    }
    
    public IEnumerable<SelectListItem> GetAllLocais()
    {
        using (var _db = new DBDapper().getCon)
        {
            return _db.Query<Local>($"select * from adm.locais;")
                .Select(x => new SelectListItem { Text = x.Schema, Value = x.Id.ToString() });
        }
    }
}
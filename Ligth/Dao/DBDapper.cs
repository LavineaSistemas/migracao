using Npgsql;

namespace Ligth.Dao;

public class DBDapper
{
    private string _con;
    public DBDapper()
    {
        _con = "Host=149.56.122.174;Port=5432;Username=lavinea;Password=Je5u5@12;Database=light";
    }

    public NpgsqlConnection getCon
    {
        get { return new NpgsqlConnection(_con); }
    }
}
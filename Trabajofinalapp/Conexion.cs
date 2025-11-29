using Microsoft.Data.Sqlite;

public class Conexion
{
    public static SqliteConnection CrearConexion()
    {
        var conn = new SqliteConnection("Data Source=nomina.db");
        conn.Open();
        return conn;
    }
}
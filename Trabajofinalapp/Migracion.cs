using Microsoft.Data.Sqlite;

public class Migracion
{
    public static void CrearTablas()
    {
        using var conn = Conexion.CrearConexion();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Empleados (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Codigo TEXT NOT NULL,
                Nombre TEXT NOT NULL,
                Departamento TEXT NOT NULL,
                SalarioBase REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Nominas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmpleadoId INTEGER NOT NULL,
                Mes TEXT NOT NULL,
                AFP REAL,
                ARS REAL,
                ISR REAL,
                SalarioBruto REAL,
                SalarioNeto REAL,
                FOREIGN KEY (EmpleadoId) REFERENCES Empleados(Id)
            );
        ";

        cmd.ExecuteNonQuery();
    }
}
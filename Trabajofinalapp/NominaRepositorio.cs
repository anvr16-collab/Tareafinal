using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class NominaRepositorio
{
    public void Insertar(int empleadoId, string mes, decimal salarioBruto)
    {
        decimal afp = Nomina.CalcularAFP(salarioBruto);
        decimal ars = Nomina.CalcularARS(salarioBruto);
        decimal isr = Nomina.CalcularISR(salarioBruto);
        decimal neto = Nomina.CalcularNeto(salarioBruto);

        using var conn = Conexion.CrearConexion();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Nominas 
            (EmpleadoId, Mes, AFP, ARS, ISR, SalarioBruto, SalarioNeto) 
            VALUES (@emp, @mes, @afp, @ars, @isr, @bruto, @neto)";
        cmd.Parameters.AddWithValue("@emp", empleadoId);
        cmd.Parameters.AddWithValue("@mes", mes);
        cmd.Parameters.AddWithValue("@afp", afp);
        cmd.Parameters.AddWithValue("@ars", ars);
        cmd.Parameters.AddWithValue("@isr", isr);
        cmd.Parameters.AddWithValue("@bruto", salarioBruto);
        cmd.Parameters.AddWithValue("@neto", neto);
        cmd.ExecuteNonQuery();
    }

    public List<(string Nombre, string Mes, decimal Neto)> ObtenerReporte()
    {
        var lista = new List<(string, string, decimal)>();
        using var conn = Conexion.CrearConexion();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT e.Nombre, n.Mes, n.SalarioNeto 
                            FROM Nominas n 
                            JOIN Empleados e ON e.Id = n.EmpleadoId";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add((reader.GetString(0), reader.GetString(1), reader.GetDecimal(2)));
        }
        return lista;
    }

   public void EliminarNominasPorEmpleado(int empleadoId)
    {
        using var conn = Conexion.CrearConexion();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Nominas WHERE EmpleadoId = @id";
        cmd.Parameters.AddWithValue("@id", empleadoId);
        cmd.ExecuteNonQuery();
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class EmpleadoRepositorio
{
    public void Insertar(Empleado emp)
    {
        using var conn = Conexion.CrearConexion();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Empleados (Codigo, Nombre, Departamento, SalarioBase) VALUES (@c, @n, @d, @s)";
        cmd.Parameters.AddWithValue("@c", emp.Codigo);
        cmd.Parameters.AddWithValue("@n", emp.Nombre);
        cmd.Parameters.AddWithValue("@d", emp.Departamento);
        cmd.Parameters.AddWithValue("@s", emp.SalarioBase);
        cmd.ExecuteNonQuery();
    }

    public List<Empleado> ObtenerTodos()
    {
        var lista = new List<Empleado>();
        using var conn = Conexion.CrearConexion();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Empleados";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Empleado
            {
                Id = reader.GetInt32(0),
                Codigo = reader.GetString(1),
                Nombre = reader.GetString(2),
                Departamento = reader.GetString(3),
                SalarioBase = reader.GetDecimal(4)
            });
        }
        return lista;
    }

        public void Actualizar(Empleado emp)
        {
            using var conn = Conexion.CrearConexion();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Empleados SET Nombre=@n, Departamento=@d, SalarioBase=@s WHERE Id=@id";
            cmd.Parameters.AddWithValue("@n", emp.Nombre);
            cmd.Parameters.AddWithValue("@d", emp.Departamento);
            cmd.Parameters.AddWithValue("@s", emp.SalarioBase);
            cmd.Parameters.AddWithValue("@id", emp.Id);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var conn = Conexion.CrearConexion();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Empleados WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }

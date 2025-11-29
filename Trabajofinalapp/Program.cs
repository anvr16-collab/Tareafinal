using System;
using System.Linq;

class Program
{
    static EmpleadoRepositorio repo = new EmpleadoRepositorio();
    static NominaRepositorio nominaRepo = new NominaRepositorio();

    static void Main()
    {
        Migracion.CrearTablas();

        while (true)
        {
            Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
            Console.WriteLine("1. Agregar empleado");
            Console.WriteLine("2. Consultar empleados");
            Console.WriteLine("3. Editar empleado");
            Console.WriteLine("4. Eliminar empleado");
            Console.WriteLine("5. Mostrar reporte mensual (en consola)");
            Console.WriteLine("6. Registrar nómina mensual (guardar en DB)");
            Console.WriteLine("7. Consultar nóminas guardadas (desde DB)");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1": AgregarEmpleado(); break;
                case "2": ConsultarEmpleados(); break;
                case "3": EditarEmpleado(); break;
                case "4": EliminarEmpleado(); break;
                case "5": MostrarReporte(); break;
                case "6": RegistrarNomina(); break;
                case "7": ConsultarNominas(); break;
                case "0": return;
                default: Console.WriteLine("Opción inválida."); break;
            }
        }
    }

    static void AgregarEmpleado()
    {
        Console.Write("Código: ");
        string codigo = Console.ReadLine();
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Departamento: ");
        string depto = Console.ReadLine();
        Console.Write("Salario Base: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal salario) || salario < 0)
        {
            Console.WriteLine("Salario inválido.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            Console.WriteLine("El nombre no puede estar vacío.");
            return;
        }

        var existentes = repo.ObtenerTodos();
        if (existentes.Any(e => e.Codigo == codigo))
        {
            Console.WriteLine("Ya existe un empleado con ese código.");
            return;
        }

        var emp = new Empleado
        {
            Codigo = codigo,
            Nombre = nombre,
            Departamento = depto,
            SalarioBase = salario
        };

        repo.Insertar(emp);
        Console.WriteLine("Empleado agregado.");
    }

    static void ConsultarEmpleados()
    {
        var lista = repo.ObtenerTodos();
        Console.WriteLine("\n--- Lista de Empleados ---");
        foreach (var e in lista)
        {
            Console.WriteLine($"ID: {e.Id}, Código: {e.Codigo}, Nombre: {e.Nombre}, Departamento: {e.Departamento}, Salario: {e.SalarioBase:C}");
        }
    }

    static void EditarEmpleado()
    {
        ConsultarEmpleados();
        Console.Write("Ingrese el ID del empleado a editar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        var lista = repo.ObtenerTodos();
        var emp = lista.FirstOrDefault(e => e.Id == id);
        if (emp == null)
        {
            Console.WriteLine("Empleado no encontrado.");
            return;
        }

        Console.Write("Nuevo nombre (enter para mantener): ");
        string nuevoNombre = Console.ReadLine();
        Console.Write("Nuevo departamento (enter para mantener): ");
        string nuevoDepto = Console.ReadLine();
        Console.Write("Nuevo salario base (enter para mantener): ");
        string nuevoSalarioStr = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nuevoNombre)) emp.Nombre = nuevoNombre;
        if (!string.IsNullOrWhiteSpace(nuevoDepto)) emp.Departamento = nuevoDepto;
        if (decimal.TryParse(nuevoSalarioStr, out decimal nuevoSalario) && nuevoSalario >= 0)
            emp.SalarioBase = nuevoSalario;

        repo.Actualizar(emp);
        Console.WriteLine("Empleado actualizado.");
    }

   static void EliminarEmpleado()
    {
        ConsultarEmpleados();
        Console.Write("Ingrese el ID del empleado a eliminar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        nominaRepo.EliminarNominasPorEmpleado(id);

        repo.Eliminar(id);
        Console.WriteLine("Empleado eliminado correctamente.");
    }

    static void MostrarReporte()
    {
        var lista = repo.ObtenerTodos();
        Console.WriteLine("\n--- Reporte Mensual (Consola) ---");
        foreach (var e in lista)
        {
            decimal afp = Nomina.CalcularAFP(e.SalarioBase);
            decimal ars = Nomina.CalcularARS(e.SalarioBase);
            decimal isr = Nomina.CalcularISR(e.SalarioBase);
            decimal neto = Nomina.CalcularNeto(e.SalarioBase);
            decimal total = e.SalarioBase;

            Console.WriteLine($"Empleado: {e.Nombre}");
            Console.WriteLine($"Bruto: {e.SalarioBase:C}, AFP: {afp:C}, ARS: {ars:C}, ISR: {isr:C}, Neto: {neto:C}, Total pagado: {total:C}");
            Console.WriteLine("--------------------------------------------------");
        }
    }

    static void RegistrarNomina()
    {
        ConsultarEmpleados();
        Console.Write("Ingrese el ID del empleado: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        Console.Write("Mes (ejemplo: Noviembre 2025): ");
        string mes = Console.ReadLine();

        var lista = repo.ObtenerTodos();
        var emp = lista.FirstOrDefault(e => e.Id == id);
        if (emp == null)
        {
            Console.WriteLine("Empleado no encontrado.");
            return;
        }

        nominaRepo.Insertar(emp.Id, mes, emp.SalarioBase);
        Console.WriteLine("Nómina registrada en la base de datos.");
    }

    static void ConsultarNominas()
    {
        var lista = nominaRepo.ObtenerReporte();
        Console.WriteLine("\n--- Nóminas Registradas (DB) ---");
        foreach (var item in lista)
        {
            Console.WriteLine($"Empleado: {item.Nombre}, Mes: {item.Mes}, Neto: {item.Neto:C}");
        }
    }
}
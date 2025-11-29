Sistema de Gestión de Nómina en C#

Descripción

Este proyecto es una aplicación de consola en C# que permite gestionar empleados y calcular sus nóminas mensuales.
Los datos se almacenan en una base de datos SQLite (.db), garantizando persistencia y consultas posteriores.

Funcionalidades principales

- Gestión de empleados (CRUD):
- Agregar empleados
- Consultar empleados
- Editar empleados
- Eliminar empleados (con manejo de nóminas asociadas)
- 
- Gestión de nóminas:
- Calcular AFP, ARS, ISR y salario neto
- Mostrar reporte mensual en consola
- Registrar nómina mensual en la base de datos
- Consultar nóminas guardadas

Arquitectura del sistema
El sistema está compuesto por las siguientes clases:

Program-------------Menú principal y flujo de la aplicación
Empleado------------Entidad que representa a un empleado
Nomina--------------Métodos de cálculo de AFP, ARS, ISR y salario neto
Conexion------------Crea la conexión a la base de datos SQLite
Migracion-----------Crea las tablas necesarias (Empleados, Nominas)
EmpleadoRepositorio-CRUD de empleados en la base de datos
NominaRepositorio---Inserción y consulta de nóminas, eliminación de nóminas por empleado

Requisitos
•	.NET 6.0 o superior
•	Paquete NuGet: Microsoft.Data.Sqlite
•	SQLite instalado o disponible en el proyecto

Nota
•	Al eliminar un empleado, primero se eliminan sus nóminas asociadas para evitar errores de clave foránea.
•	Los IDs de empleados se gestionan automáticamente por SQLite

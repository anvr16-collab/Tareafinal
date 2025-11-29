public class Nomina
{
    public static decimal CalcularAFP(decimal salario) => salario * 0.0287M;
    public static decimal CalcularARS(decimal salario) => salario * 0.0304M;
    public static decimal CalcularISR(decimal salario) => 0; // Puedes modificar esta lógica

    public static decimal CalcularNeto(decimal salario)
    {
        var afp = CalcularAFP(salario);
        var ars = CalcularARS(salario);
        var isr = CalcularISR(salario);
        return salario - afp - ars - isr;
    }
}
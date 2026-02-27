using Microsoft.EntityFrameworkCore;

namespace SmartHome.Persistencia.Test;
internal sealed class ContextoSqlTests
{
    public static ContextoSql CrearContextoMemoria()
    {
        var constructor = new DbContextOptionsBuilder<ContextoSql>();
        constructor.UseInMemoryDatabase("TestBD");
        return new ContextoSql(constructor.Options);
    }
}

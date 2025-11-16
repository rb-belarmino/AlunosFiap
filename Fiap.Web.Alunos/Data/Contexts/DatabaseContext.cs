using Microsoft.EntityFrameworkCore;

namespace Fiap.Web.Alunos.Data.Contexts;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    protected DatabaseContext()
    {
    }
}
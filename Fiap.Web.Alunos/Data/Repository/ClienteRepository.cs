using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Data.Repository;
using Fiap.Web.Alunos.Models;
using Microsoft.EntityFrameworkCore;
public class ClienteRepository : IClienteRepository
{
    private readonly DatabaseContext _context;
    public ClienteRepository(DatabaseContext context)
    {
        _context = context;
    }
    public IEnumerable<ClienteModel> GetAll() => _context.Clientes.Include(c => c.Representante).ToList();
    public ClienteModel GetById(int id) => _context.Clientes.Find(id);
    public void Add(ClienteModel cliente)
    {
        _context.Clientes.Add(cliente);
        _context.SaveChanges();
    }
    public void Update(ClienteModel cliente)
    {
        _context.Update(cliente);
        _context.SaveChanges();
    }
    public void Delete(ClienteModel cliente)
    {
        _context.Clientes.Remove(cliente);
        _context.SaveChanges();
    }
}
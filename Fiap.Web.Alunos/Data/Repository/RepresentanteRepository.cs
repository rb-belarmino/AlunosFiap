using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Models;

namespace Fiap.Web.Alunos.Data.Repository
{
    public class RepresentanteRepository : IRepresentanteRepository
    {
        private readonly DatabaseContext _context;

        public RepresentanteRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<RepresentanteModel> GetAll()
        {
            return _context.Representantes.ToList();
        }
    }
}
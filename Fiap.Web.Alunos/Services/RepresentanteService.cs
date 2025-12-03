using Fiap.Web.Alunos.Data.Repository;
using Fiap.Web.Alunos.Models;

namespace Fiap.Web.Alunos.Services
{
    public class RepresentanteService : IRepresentanteService
    {
        private readonly IRepresentanteRepository _repository;

        public RepresentanteService(IRepresentanteRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<RepresentanteModel> ListarRepresentantes()
        {
            return _repository.GetAll();
        }
    }
}
using Fiap.Web.Alunos.Models;

namespace Fiap.Web.Alunos.Data.Repository
{
    public interface IRepresentanteRepository
    {
        IEnumerable<RepresentanteModel> GetAll();
    }
}
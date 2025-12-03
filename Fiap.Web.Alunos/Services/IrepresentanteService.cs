using Fiap.Web.Alunos.Models;

namespace Fiap.Web.Alunos.Services
{
    public interface IRepresentanteService
    {
        IEnumerable<RepresentanteModel> ListarRepresentantes();
    }
}
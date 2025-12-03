using Fiap.Web.Alunos.Models;
namespace Fiap.Web.Alunos.Data.Repository
{
    public interface IClienteRepository
    {
        IEnumerable<ClienteModel> GetAll();
        ClienteModel GetById(int id);
        void Add(ClienteModel cliente);
        void Update(ClienteModel cliente);
        void Delete(ClienteModel cliente);
    }
}
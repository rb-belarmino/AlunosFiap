using Fiap.Web.Alunos.Models;
namespace Fiap.Web.Alunos.Services
{
    public interface IClienteService
    {
        IEnumerable<ClienteModel> ListarClientes();
        ClienteModel ObterClientePorId(int id);
        void CriarCliente(ClienteModel cliente);
        void AtualizarCliente(ClienteModel cliente);
        void DeletarCliente(int id);
    }
}
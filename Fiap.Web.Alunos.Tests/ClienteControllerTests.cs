using Fiap.Web.Alunos.Controllers;
using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace Fiap.Web.Alunos.Tests
{
    public class ClienteControllerTests
    {
        // Mock do contexto do banco de dados
        private readonly Mock<DatabaseContext> _mockContext;
        // Controlador que será testado
        private readonly ClienteController _controller;
        // Mock do DbSet para ClienteModel
        private readonly DbSet<ClienteModel> _mockSet;
        public ClienteControllerTests()
        {
            // Inicializa o mock do contexto
            _mockContext = new Mock<DatabaseContext>();
            // Cria e configura o mock DbSet
            _mockSet = MockDbSet();
            // Configura o contexto mock para retornar o DbSet mock quando a propriedade Clientes for acessada
            _mockContext.Setup(m => m.Clientes).Returns(_mockSet);
            // Inicializa o controller com o contexto mock
            _controller = new ClienteController(_mockContext.Object);
        }
        // Método para criar e configurar um DbSet mock para ClienteModel
        private DbSet<ClienteModel> MockDbSet()
        {
            // Lista de clientes para simular dados no banco de dados
            var data = new List<ClienteModel>
            {
                new ClienteModel { ClienteId = 1, Nome = "Cliente 1" },
                new ClienteModel { ClienteId = 2, Nome = "Cliente 2" }
            }.AsQueryable();
            // Cria o mock do DbSet
            var mockSet = new Mock<DbSet<ClienteModel>>();
            // Configura o comportamento do mock DbSet para simular uma consulta ao banco de dados
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            // Retorna o DbSet mock
            return mockSet.Object;
        }
        [Fact]
        public void Index_ReturnsViewResult_WithListOfClients()
        {
            // Act
            // Invoca o método Index do controlador para testar seu comportamento
            var result = _controller.Index();
            // Assert
            // Verifica se o resultado obtido é do tipo ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);
            // Verifica se o modelo retornado pelo ViewResult pode ser atribuído a uma coleção de ClienteModel
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            // Confirma se o número de clientes no modelo é igual a 2, conforme esperado
            Assert.Equal(2, model.Count()); 
        }
        [Fact]
        public void Index_ReturnsEmptyList_WhenNoClientsExist()
        {
            // Arrange
            // Limpa o mock DbSet para simular uma condição onde não existem clientes no banco de dados
            _mockSet.RemoveRange(_mockSet.ToList());
            // Configura o contexto mock para retornar o DbSet vazio quando a propriedade Clientes for acessada
            _mockContext.Setup(m => m.Clientes).Returns(_mockSet);
            // Act
            // Chama o método Index do controlador para testar o comportamento com uma lista vazia
            var result = _controller.Index();
            // Assert
            // Verifica se o resultado obtido é do tipo ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);
            // Confirma que o modelo associado à ViewResult é uma coleção vazia de ClienteModel
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            // Checa se o modelo está vazio, validando o cenário de nenhum cliente presente
            Assert.Empty(model);
        }
        [Fact]
        public void Index_ThrowsException_WhenDatabaseFails()
        {
            // Arrange
            // Configura o contexto mock para lançar uma exceção quando a propriedade Clientes for acessada, simulando uma falha de banco de dados
            _mockContext.Setup(m => m.Clientes).Throws(new System.Exception("Database error"));
            // Act &amp; Assert
            // Verifica se a chamada ao método Index lança uma exceção, conforme esperado durante uma falha de banco de dados
            Assert.Throws<System.Exception>(() => _controller.Index());
        }
    }
}
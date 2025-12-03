using AutoMapper;
using Fiap.Web.Alunos.Controllers;
using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Models;
using Fiap.Web.Alunos.Services; // Adicione o using para o serviço
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Fiap.Web.Alunos.Tests
{
    public class ClienteControllerTests
    {
        private readonly Mock<DatabaseContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IClienteService> _mockService; // Adiciona o mock para o IClienteService
        private readonly ClienteController _controller;
        private readonly DbSet<ClienteModel> _mockSet;

        public ClienteControllerTests()
        {
            _mockContext = new Mock<DatabaseContext>();
            _mockMapper = new Mock<IMapper>();
            _mockService = new Mock<IClienteService>(); // Inicializa o mock do IClienteService
            _mockSet = MockDbSet();

            _mockContext.Setup(m => m.Clientes).Returns(_mockSet);

            // Passa o objeto mock do IClienteService para o construtor
            _controller = new ClienteController(_mockContext.Object, _mockMapper.Object, _mockService.Object);
        }

        private DbSet<ClienteModel> MockDbSet()
        {
            var data = new List<ClienteModel>
            {
                new ClienteModel { ClienteId = 1, Nome = "Cliente 1" },
                new ClienteModel { ClienteId = 2, Nome = "Cliente 2" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<ClienteModel>>();
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ClienteModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet.Object;
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfClients()
        {
            // Configura o mock do serviço para retornar a lista de clientes
            _mockService.Setup(s => s.ListarClientes()).Returns(_mockContext.Object.Clientes.ToList());
            
            var result = _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Index_ReturnsEmptyList_WhenNoClientsExist()
        {
            var emptyData = new List<ClienteModel>().AsQueryable();
            var emptyMockSet = new Mock<DbSet<ClienteModel>>();
            emptyMockSet.As<IQueryable<ClienteModel>>().Setup(m => m.Provider).Returns(emptyData.Provider);
            emptyMockSet.As<IQueryable<ClienteModel>>().Setup(m => m.Expression).Returns(emptyData.Expression);
            emptyMockSet.As<IQueryable<ClienteModel>>().Setup(m => m.ElementType).Returns(emptyData.ElementType);
            emptyMockSet.As<IQueryable<ClienteModel>>().Setup(m => m.GetEnumerator()).Returns(emptyData.GetEnumerator());

            _mockContext.Setup(m => m.Clientes).Returns(emptyMockSet.Object);
            
            // Configura o mock do serviço para retornar uma lista vazia
            _mockService.Setup(s => s.ListarClientes()).Returns(new List<ClienteModel>());
            
            var controller = new ClienteController(_mockContext.Object, _mockMapper.Object, _mockService.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Index_ThrowsException_WhenDatabaseFails()
        {
            // Configura o mock do serviço para lançar uma exceção
            _mockService.Setup(s => s.ListarClientes()).Throws(new System.Exception("Database error"));

            Assert.Throws<System.Exception>(() => _controller.Index());
        }
    }
}

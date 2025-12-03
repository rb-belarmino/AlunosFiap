using AutoMapper;
using Fiap.Web.Alunos.Controllers;
using Fiap.Web.Alunos.Models;
using Fiap.Web.Alunos.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Fiap.Web.Alunos.Tests
{
    public class ClienteControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IClienteService> _mockClienteService;
        private readonly Mock<IRepresentanteService> _mockRepresentanteService; // Adiciona o mock para IRepresentanteService
        private readonly ClienteController _controller;

        public ClienteControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockClienteService = new Mock<IClienteService>();
            _mockRepresentanteService = new Mock<IRepresentanteService>(); // Inicializa o mock

            // Instancia o controller com os mocks na ordem correta
            _controller = new ClienteController(
                _mockMapper.Object,
                _mockClienteService.Object,
                _mockRepresentanteService.Object
            );
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfClients()
        {
            // Arrange
            var clientes = new List<ClienteModel>
            {
                new ClienteModel { ClienteId = 1, Nome = "Cliente 1" },
                new ClienteModel { ClienteId = 2, Nome = "Cliente 2" }
            };
            _mockClienteService.Setup(s => s.ListarClientes()).Returns(clientes);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Index_ReturnsEmptyList_WhenNoClientsExist()
        {
            // Arrange
            _mockClienteService.Setup(s => s.ListarClientes()).Returns(new List<ClienteModel>());

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Index_ThrowsException_WhenServiceFails()
        {
            // Arrange
            _mockClienteService.Setup(s => s.ListarClientes()).Throws(new System.Exception("Service error"));

            // Act & Assert
            Assert.Throws<System.Exception>(() => _controller.Index());
        }
    }
}

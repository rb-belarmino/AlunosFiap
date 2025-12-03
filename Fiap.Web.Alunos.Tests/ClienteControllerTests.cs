using AutoMapper;
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
        private readonly Mock<DatabaseContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ClienteController _controller;
        private readonly DbSet<ClienteModel> _mockSet;

        public ClienteControllerTests()
        {
            _mockContext = new Mock<DatabaseContext>();
            _mockMapper = new Mock<IMapper>();
            _mockSet = MockDbSet();
            
            _mockContext.Setup(m => m.Clientes).Returns(_mockSet);
            
            _controller = new ClienteController(_mockContext.Object, _mockMapper.Object);
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
            var controller = new ClienteController(_mockContext.Object, _mockMapper.Object);

            var result = controller.Index();
            
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ClienteModel>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Index_ThrowsException_WhenDatabaseFails()
        {
            _mockContext.Setup(m => m.Clientes).Throws(new System.Exception("Database error"));
            
            Assert.Throws<System.Exception>(() => _controller.Index());
        }
    }
}

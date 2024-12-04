using FIAP_TC.Contact.Api.Controllers;
using FIAP_TC.Contact.Api.Models;
using FIAP_TC.Contact.Api.Repository.Interfaces;
using FIAP_TC.Contact.Core.DTO;
using FIAP_TC.Contact.Core.Entities;
using FIAP_TC.Contact.Infraestructure.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FIAP_TC.Contact.Tests.Controllers;

public class ContatoControllerTests
{
    private Mock<IContatoRepository> _mockRepo;
    private Mock<IRabbitMQService> _mockRabbitMQService;
    private ContatoController _controller;

    public ContatoControllerTests()
    {
        _mockRepo = new Mock<IContatoRepository>();
        _mockRabbitMQService = new Mock<IRabbitMQService>();
        _controller = new ContatoController(_mockRepo.Object, _mockRabbitMQService.Object);
    }

    [Fact]
    public void GetContato_ReturnsOkResult_WithListOfContatos()
    {
        // Arrange
        var contatos = new List<ContatoDTO>
            {
                new() { Id = 1, Nome = "Teste", Email = "teste@teste.com", Telefone = "123456789", DDD = 11, Regiao = "Sudeste" }
            };
        _mockRepo.Setup(repo => repo.ListarContatos()).Returns(contatos);

        // Act
        var result = _controller.GetContato();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ContatoDTO>>(okResult.Value);
        Assert.Equal(contatos.Count, returnValue.Count);
    }

    [Fact]
    public void GetContato_ReturnsOkResult_WhenNoContatos()
    {
        // Arrange
        var contatos = new List<ContatoDTO>();
        _mockRepo.Setup(repo => repo.ListarContatos()).Returns(contatos);

        // Act
        var result = _controller.GetContato();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ContatoDTO>>(okResult.Value);
        Assert.Empty(returnValue);
    }

    [Fact]
    public void GetContatoDdd_ReturnsOkResult_WithListOfContatos()
    {
        // Arrange
        int ddd = 11;
        var contatos = new List<ContatoDTO>
            {
                new() { Id = 1, Nome = "Teste", Email = "teste@teste.com", Telefone = "123456789", DDD = ddd, Regiao = "Sudeste" }
            };
        _mockRepo.Setup(repo => repo.ListarContatosDdd(ddd)).Returns(contatos);

        // Act
        var result = _controller.GetContatoDdd(ddd);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ContatoDTO>>(okResult.Value);
        Assert.Equal(contatos.Count, returnValue.Count);
    }

    [Fact]
    public void GetContatoDdd_ReturnsOkResult_WhenNoContatos()
    {
        // Arrange
        int ddd = 11;
        var contatos = new List<ContatoDTO>();
        _mockRepo.Setup(repo => repo.ListarContatosDdd(ddd)).Returns(contatos);

        // Act
        var result = _controller.GetContatoDdd(ddd);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ContatoDTO>>(okResult.Value);
        Assert.Empty(returnValue);
    }

    [Fact]
    public void CreateContato_ReturnsOkResult_WithCreatedContato()
    {
        // Arrange
        var inputContato = new ContatoModel { Nome = "Teste", Email = "teste@teste.com", Telefone = "123456789", DDD = 11 };
        _mockRepo.Setup(repo => repo.CriarContato(It.IsAny<Contato>())).Returns(new Contato());

        // Act
        var result = _controller.CreateContato(inputContato);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void CreateContato_ReturnsBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var invalidContato = new ContatoModel { Nome = "", Email = "invalidEmail", Telefone = "123", DDD = 999 };
        _controller.ModelState.AddModelError("Nome", "Required");
        _controller.ModelState.AddModelError("Email", "Invalid format");
        _controller.ModelState.AddModelError("Telefone", "Invalid format");
        _controller.ModelState.AddModelError("DDD", "Invalid format");

        // Act
        var result = _controller.CreateContato(invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Nome"));
        Assert.True(returnValue.ContainsKey("Email"));
        Assert.True(returnValue.ContainsKey("Telefone"));
        Assert.True(returnValue.ContainsKey("DDD"));
    }

    [Fact]
    public void DeleteContato_ReturnsOkResult_WhenContatoExists()
    {
        // Arrange
        var existingContato = new ContatoDTO { Id = 1, Nome = "Teste 1", Email = "teste1@teste.com", Telefone = "123456789", DDD = 11, DataCriacao = DateTime.Now, DataModificacao = DateTime.Now, Regiao = "Sudeste" };
        _mockRepo.Setup(repo => repo.ListarContatos()).Returns([existingContato]);
        _mockRepo.Setup(repo => repo.ExcluirContato(It.IsAny<int>()));

        // Act
        var result = _controller.DeleteContato(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void DeleteContato_ReturnsNotFound_WhenContatoDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockRepo.Setup(repo => repo.ListarContatos()).Returns([]);

        // Act
        var result = _controller.DeleteContato(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Contato não encontrado.", notFoundResult.Value);
    }

    [Fact]
    public void UpdateContato_ReturnsOkResult_WhenContatoExists()
    {
        // Arrange
        int id = 1;
        var inputContato = new UpdateContatoModel { Nome = "Teste Atualizado", Email = "teste1@teste.com", Telefone = "123456789", DDD = 11 };
        var existingContato = new ContatoDTO { Id = id, Nome = "Teste", Email = "teste@teste.com", Telefone = "123456789", DDD = 11, Regiao = "Sudeste" };
        _mockRepo.Setup(repo => repo.ListarContatos()).Returns([existingContato]);
        _mockRepo.Setup(repo => repo.AtualizarContato(It.IsAny<Contato>()));

        // Act
        var result = _controller.UpdateContato(id, inputContato);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void UpdateContato_ReturnsNotFound_WhenContatoDoesNotExist()
    {
        // Arrange
        int id = 1;
        var inputContato = new UpdateContatoModel { Nome = "Teste Atualizado", Email = "teste1@teste.com", Telefone = "123456789", DDD = 11 };
        _mockRepo.Setup(repo => repo.ListarContatos()).Returns([]);

        // Act
        var result = _controller.UpdateContato(id, inputContato);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Contato não encontrado.", notFoundResult.Value);
    }

    [Fact]
    public void UpdateContato_ReturnsBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        int id = 1;
        var invalidContato = new UpdateContatoModel { Nome = "", Email = "invalidEmail", Telefone = "123", DDD = 999 };
        _controller.ModelState.AddModelError("Nome", "Required");
        _controller.ModelState.AddModelError("Email", "Invalid format");
        _controller.ModelState.AddModelError("Telefone", "Invalid format");
        _controller.ModelState.AddModelError("DDD", "Invalid format");

        // Act
        var result = _controller.UpdateContato(id, invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Nome"));
        Assert.True(returnValue.ContainsKey("Email"));
        Assert.True(returnValue.ContainsKey("Telefone"));
        Assert.True(returnValue.ContainsKey("DDD"));
    }

    [Fact]
    public void CreateContato_ReturnsBadRequest_WhenNomeIsInvalid()
    {
        // Arrange
        var invalidContato = new ContatoModel { Nome = "", Email = "teste@teste.com", Telefone = "123456789", DDD = 11 };
        _controller.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = _controller.CreateContato(invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Nome"));
    }

    [Fact]
    public void CreateContato_ReturnsBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var invalidContato = new ContatoModel { Nome = "Teste", Email = "invalidEmail", Telefone = "123456789", DDD = 11 };
        _controller.ModelState.AddModelError("Email", "Invalid format");

        // Act
        var result = _controller.CreateContato(invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Email"));
    }

    [Fact]
    public void CreateContato_ReturnsBadRequest_WhenTelefoneIsInvalid()
    {
        // Arrange
        var invalidContato = new ContatoModel { Nome = "Teste", Email = "teste@teste.com", Telefone = "123", DDD = 11 };
        _controller.ModelState.AddModelError("Telefone", "Invalid format");

        // Act
        var result = _controller.CreateContato(invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Telefone"));
    }

    [Fact]
    public void CreateContato_ReturnsBadRequest_WhenDDDIsInvalid()
    {
        // Arrange
        var invalidContato = new ContatoModel { Nome = "Teste", Email = "teste@teste.com", Telefone = "123456789", DDD = 999 };
        _controller.ModelState.AddModelError("DDD", "Invalid format");

        // Act
        var result = _controller.CreateContato(invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("DDD"));
    }

    [Fact]
    public void UpdateContato_ReturnsBadRequest_WhenNomeIsInvalid()
    {
        // Arrange
        int id = 1;
        var invalidContato = new UpdateContatoModel { Nome = "", Email = "teste@teste.com", Telefone = "123456789", DDD = 11 };
        _controller.ModelState.AddModelError("Nome", "Required");

        // Act
        var result = _controller.UpdateContato(id, invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Nome"));
    }

    [Fact]
    public void UpdateContato_ReturnsBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        int id = 1;
        var invalidContato = new UpdateContatoModel { Nome = "Teste", Email = "invalidEmail", Telefone = "123456789", DDD = 11 };
        _controller.ModelState.AddModelError("Email", "Invalid format");

        // Act
        var result = _controller.UpdateContato(id, invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Email"));
    }

    [Fact]
    public void UpdateContato_ReturnsBadRequest_WhenTelefoneIsInvalid()
    {
        // Arrange
        int id = 1;
        var invalidContato = new UpdateContatoModel { Nome = "Teste", Email = "teste@teste.com", Telefone = "123", DDD = 11 };
        _controller.ModelState.AddModelError("Telefone", "Invalid format");

        // Act
        var result = _controller.UpdateContato(id, invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Telefone"));
    }

    [Fact]
    public void UpdateContato_ReturnsBadRequest_WhenDDDIsInvalid()
    {
        // Arrange
        int id = 1;
        var invalidContato = new UpdateContatoModel { Nome = "Teste", Email = "teste@teste.com", Telefone = "123456789", DDD = 999 };
        _controller.ModelState.AddModelError("DDD", "Invalid format");

        // Act
        var result = _controller.UpdateContato(id, invalidContato);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("DDD"));
    }
}

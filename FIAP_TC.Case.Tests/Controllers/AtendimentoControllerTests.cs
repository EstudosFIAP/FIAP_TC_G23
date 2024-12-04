using FIAP_TC.Case.Api.Controllers;
using FIAP_TC.Case.Api.Models;
using FIAP_TC.Case.Api.Repository.Interfaces;
using FIAP_TC.Case.Core.DTO;
using FIAP_TC.Case.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FIAP_TC.Case.Tests.Controllers;

public class AtendimentoControllerTests
{
    private readonly AtendimentoController _controller;
    private readonly Mock<IAtendimentoRepository> _mockRepo;

    public AtendimentoControllerTests()
    {
        _mockRepo = new Mock<IAtendimentoRepository>();
        _controller = new AtendimentoController(_mockRepo.Object);
    }

    [Fact]
    public void GetAtendimentos_ReturnsOkResult_WithListOfAtendimentos()
    {
        // Arrange
        var atendimentos = new List<AtendimentoDTO>
        {
            new AtendimentoDTO { IdAtendimento = 1, Assunto = "Teste", Descricao = "Descrição de Teste", DataSolicitacao = DateTime.Now, IdContato = 1 }
        };
        _mockRepo.Setup(repo => repo.BuscarAtendimentos(null, null, null)).Returns(atendimentos);

        // Act
        var result = _controller.BuscarAtendimentos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<AtendimentoDTO>>(okResult.Value);
        Assert.Equal(atendimentos.Count, returnValue.Count);
    }

    [Fact]
    public void GetAtendimentos_ReturnsNotFoundResult_WhenNoAtendimentos()
    {
        // Arrange
        var atendimentos = new List<AtendimentoDTO>();
        _mockRepo.Setup(repo => repo.BuscarAtendimentos(null, null, null)).Returns(atendimentos);

        // Act
        var result = _controller.BuscarAtendimentos();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Nenhum atendimento encontrado com os parâmetros fornecidos.", notFoundResult.Value?.GetType().GetProperty("message")?.GetValue(notFoundResult.Value)?.ToString());
    }

    [Fact]
    public void GetAtendimentoById_ReturnsOkResult_WithAtendimento()
    {
        // Arrange
        var atendimento = new AtendimentoDTO { IdAtendimento = 1, Assunto = "Teste", Descricao = "Descrição de Teste", DataSolicitacao = DateTime.Now, IdContato = 1 };
        _mockRepo.Setup(repo => repo.ObterAtendimentoPorId(1)).Returns(atendimento);

        // Act
        var result = _controller.GetAtendimentoById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<AtendimentoDTO>(okResult.Value);
        Assert.Equal(atendimento.IdAtendimento, returnValue.IdAtendimento);
    }

    [Fact]
    public void GetAtendimentoById_ReturnsNotFoundResult_WhenAtendimentoDoesNotExist()
    {
        // Arrange
        // Certifique-se de que o método no repositório está retornando um tipo anulável (AtendimentoDTO?).
        _mockRepo.Setup(repo => repo.ObterAtendimentoPorId(1)).Returns((AtendimentoDTO?)null);

        // Act
        var result = _controller.GetAtendimentoById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // Verifica se retornou 404 - NotFound

        // Verifique a mensagem retornada
        var expectedMessage = "Atendimento não encontrado.";
        var actualMessage = notFoundResult.Value?.GetType()?.GetProperty("message")?.GetValue(notFoundResult.Value)?.ToString();

        Assert.Equal(expectedMessage, actualMessage); // Verifica se a mensagem retornada é correta
    }


    [Fact]
    public void CreateAtendimento_ReturnsOkResult_WithCreatedAtendimento()
    {
        // Arrange
        var inputAtendimento = new AtendimentoModel
        {
            Assunto = "Teste Assunto",
            Descricao = "Teste Descricao",
            IdContato = 1
        };

        var atendimentoCriado = new Atendimento
        {
            Assunto = "Teste Assunto",
            Descricao = "Teste Descricao",
            DataSolicitacao = DateTime.Now,
            DataModificacao = DateTime.Now,
            IdContato = 1
        };

        _mockRepo.Setup(repo => repo.CriarAtendimento(It.IsAny<Atendimento>()))
            .Returns(atendimentoCriado);

        // Act
        var result = _controller.CreateAtendimento(inputAtendimento);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void CreateAtendimento_ReturnsBadRequest_WhenDataIsInvalid()
    {
        // Arrange
        var invalidAtendimento = new AtendimentoModel { Assunto = "", Descricao = "", IdContato = 0 };
        _controller.ModelState.AddModelError("Assunto", "Required");
        _controller.ModelState.AddModelError("Descricao", "Required");

        // Act
        var result = _controller.CreateAtendimento(invalidAtendimento);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var returnValue = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(returnValue.ContainsKey("Assunto"));
        Assert.True(returnValue.ContainsKey("Descricao"));
    }

    [Fact]
    public void UpdateAtendimento_ReturnsOkResult_WhenAtendimentoExists()
    {
        // Arrange
        var inputAtendimento = new UpdateAtendimentoModel { Assunto = "Teste Atualizado", Descricao = "Teste Atualizado", IdContato = 1 };
        var existingAtendimento = new AtendimentoDTO { IdAtendimento = 1, Assunto = "Teste", Descricao = "Teste", IdContato = 1 };
        _mockRepo.Setup(repo => repo.ObterAtendimentoPorId(1)).Returns(existingAtendimento);
        _mockRepo.Setup(repo => repo.AtualizarAtendimento(It.IsAny<Atendimento>()));

        // Act
        var result = _controller.UpdateAtendimento(1, inputAtendimento);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void UpdateAtendimento_ReturnsNotFoundResult_WhenAtendimentoDoesNotExist()
    {
        // Arrange
        var inputAtendimento = new UpdateAtendimentoModel { Assunto = "Teste Atualizado", Descricao = "Teste Atualizado", IdContato = 1 };

        // Simula o repositório retornando null quando o atendimento não for encontrado
        _mockRepo.Setup(repo => repo.ObterAtendimentoPorId(1)).Returns((AtendimentoDTO?)null);

        // Act
        var result = _controller.UpdateAtendimento(1, inputAtendimento);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualMessage = notFoundResult.Value?.GetType().GetProperty("message")?.GetValue(notFoundResult.Value);
        Assert.Equal("Atendimento não encontrado.", actualMessage);
    }


    [Fact]
    public void DeleteAtendimento_ReturnsOkResult_WhenAtendimentoExists()
    {
        // Arrange
        var existingAtendimento = new AtendimentoDTO { IdAtendimento = 1, Assunto = "Teste 1", Descricao = "Teste 1", IdContato = 1 };
        _mockRepo.Setup(repo => repo.ObterAtendimentoPorId(1)).Returns(existingAtendimento);
        _mockRepo.Setup(repo => repo.ExcluirAtendimento(1));

        // Act
        var result = _controller.DeleteAtendimento(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void DeleteAtendimento_ReturnsNotFoundResult_WhenAtendimentoDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.ObterAtendimentoPorId(1)).Returns((AtendimentoDTO?)null);

        // Act
        var result = _controller.DeleteAtendimento(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        // Verifica se o Value não é nulo antes de acessar a propriedade "message"
        var actualMessage = notFoundResult.Value?.GetType().GetProperty("message")?.GetValue(notFoundResult.Value)?.ToString();

        Assert.Equal("Atendimento não encontrado.", actualMessage);
    }


}

using FIAP_TC.Contact.Api.Models;
using FIAP_TC.Contact.Api.Repository.Interfaces;
using FIAP_TC.Contact.Core.Entities;
using FIAP_TC.Contact.Infraestructure.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_TC.Contact.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContatoController(
    IContatoRepository contatoRepository,
    IRabbitMQService rabbitMQService) : ControllerBase
{
    private readonly IContatoRepository _contatoRepository = contatoRepository;
    private readonly IRabbitMQService _rabbitMQService = rabbitMQService;

    /// <summary>
    /// Listar todos os contatos cadastrados
    /// </summary>
    [HttpGet]
    public IActionResult GetContato()
    {
        try
        {
            var result = _contatoRepository.ListarContatos();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Listar os contatos cadastrados por DDD
    /// </summary>
    /// <response code="404">Registro não encontrado.</response>
    [HttpGet("{ddd:int}")]
    public IActionResult GetContatoDdd(int ddd)
    {
        try
        {
            var result = _contatoRepository.ListarContatosDdd(ddd);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Criar contatos
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Sucesso na transação.</response>
    /// <response code='400'>Requisição inválida.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpPost]
    public IActionResult CreateContato([FromBody] ContatoModel inputContato)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var contato = new Contato()
            {
                Nome = inputContato.Nome,
                DDD = inputContato.DDD,
                Telefone = inputContato.Telefone,
                Email = inputContato.Email,
            };

            var contatoCriado = _contatoRepository.CriarContato(contato);

            _rabbitMQService.CreateContact(contatoCriado);

            return Ok(new { message = "Contato criado com sucesso!" });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Atualizar contato cadastrado
    /// </summary>
    /// <response code="200">Sucesso na transação.</response>
    /// <response code='400'>Requisição inválida.</response>
    /// <response code='404'>Registro não encontrado.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpPut("{id:int}")]
    public IActionResult UpdateContato(int id, [FromBody] UpdateContatoModel inputContato)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existingContato = _contatoRepository.ListarContatos().FirstOrDefault(c => c.Id == id);
            if (existingContato == null)
            {
                return NotFound("Contato não encontrado.");
            }

            var contato = new Contato()
            {
                Id = existingContato.Id,
                Nome = inputContato.Nome,
                DDD = inputContato.DDD,
                Telefone = inputContato.Telefone,
                Email = inputContato.Email
            };
            _contatoRepository.AtualizarContato(contato);
            return Ok(new { message = "Contato atualizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Excluir contato cadastrado
    /// </summary>
    /// <response code="200">Sucesso na transação.</response>
    /// <response code="404">Registro não encontrado.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpDelete("{id:int}")]
    public IActionResult DeleteContato(int id)
    {
        try
        {
            var existingContato = _contatoRepository.ListarContatos().FirstOrDefault(c => c.Id == id);
            if (existingContato == null)
            {
                return NotFound("Contato não encontrado.");
            }

            _contatoRepository.ExcluirContato(id);
            return Ok(new { message = "Contato excluído com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
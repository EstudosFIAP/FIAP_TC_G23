using FIAP_TC.Case.Api.Models;
using FIAP_TC.Case.Api.Repository.Interfaces;
using FIAP_TC.Case.Core.DTO;
using FIAP_TC.Case.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FIAP_TC.Case.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtendimentoController(IAtendimentoRepository atendimentoRepository) : ControllerBase
{
    private readonly IAtendimentoRepository _atendimentoRepository = atendimentoRepository;

    /// <summary>
    /// Listar todos os atendimentos cadastrados com filtros opcionais
    /// </summary>
    /// <param name="idAtendimento">Filtro por ID do atendimento</param>
    /// <param name="idContato">Filtro por ID do contato</param>
    /// <param name="nomeContato">Filtro por nome do contato</param>
    /// <returns>Lista de atendimentos que correspondem aos filtros</returns>
    [HttpGet("buscar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult BuscarAtendimentos(int? idAtendimento = null, int? idContato = null, string? nomeContato = null)
    {
        try
        {
            var result = _atendimentoRepository.BuscarAtendimentos(idAtendimento, idContato, nomeContato);

            if (!result.Any())
            {
                return NotFound(new { message = "Nenhum atendimento encontrado com os parâmetros fornecidos." });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Obter um atendimento por ID
    /// </summary>
    /// <response code="404">Atendimento não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAtendimentoById(int id)
    {
        try
        {
            var result = _atendimentoRepository.ObterAtendimentoPorId(id);
            if (result == null)
            {
                return NotFound(new { message = "Atendimento não encontrado." });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Criar um atendimento
    /// </summary>
    /// <response code="200">Sucesso na transação.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateAtendimento([FromBody] AtendimentoModel inputAtendimento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var atendimento = new Atendimento()
            {
                Assunto = inputAtendimento.Assunto,
                Descricao = inputAtendimento.Descricao,
                DataSolicitacao = DateTime.Now,
                DataModificacao = DateTime.Now,
                IdContato = inputAtendimento.IdContato
            };
            _atendimentoRepository.CriarAtendimento(atendimento);
            return Ok(new { message = "Atendimento criado com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Atualizar um atendimento cadastrado
    /// </summary>
    /// <response code="200">Sucesso na transação.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="404">Atendimento não encontrado.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateAtendimento(int id, [FromBody] UpdateAtendimentoModel inputAtendimento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existingAtendimento = _atendimentoRepository.ObterAtendimentoPorId(id);
            if (existingAtendimento == null)
            {
                return NotFound(new { message = "Atendimento não encontrado." });
            }

            var atendimento = new Atendimento()
            {
                IdAtendimento = existingAtendimento.IdAtendimento,
                Assunto = !string.IsNullOrEmpty(inputAtendimento.Assunto) ? inputAtendimento.Assunto : existingAtendimento.Assunto,
                Descricao = !string.IsNullOrEmpty(inputAtendimento.Descricao) ? inputAtendimento.Descricao : existingAtendimento.Descricao,
                DataModificacao = DateTime.Now,
                IdContato = inputAtendimento.IdContato != 0 ? inputAtendimento.IdContato : existingAtendimento.IdContato
            };
            _atendimentoRepository.AtualizarAtendimento(atendimento);
            return Ok(new { message = "Atendimento atualizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
        }
    }


    /// <summary>
    /// Excluir um atendimento cadastrado
    /// </summary>
    /// <response code="200">Sucesso na transação.</response>
    /// <response code="404">Atendimento não encontrado.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteAtendimento(int id)
    {
        try
        {
            var existingAtendimento = _atendimentoRepository.ObterAtendimentoPorId(id);
            if (existingAtendimento == null)
            {
                return NotFound(new { message = "Atendimento não encontrado." });
            }

            _atendimentoRepository.ExcluirAtendimento(id);
            return Ok(new { message = "Atendimento excluído com sucesso!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
        }
    }
}

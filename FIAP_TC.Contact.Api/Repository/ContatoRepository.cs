using FIAP_TC.Contact.Api.Repository.Interfaces;
using FIAP_TC.Contact.Core.DTO;
using FIAP_TC.Contact.Core.Entities;
using FIAP_TC.Contact.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP_TC.Contact.Api.Repository;

public class ContatoRepository : IContatoRepository
{
    private readonly DbTcFase1Context _context;

    public ContatoRepository(DbTcFase1Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Contato CriarContato(Contato dadosContato)
    {
        if (dadosContato == null)
            throw new ArgumentException("Você precisa informar os dados do contato para cadastro");

        dadosContato.DataCriacao = DateTime.Now;
        dadosContato.DataModificacao = DateTime.Now;
        _context.Contatos.Add(dadosContato);
        _context.SaveChanges();

        return dadosContato;
    }

    public void AtualizarContato(Contato dadosContato)
    {
        if (dadosContato == null)
            throw new ArgumentException("Você precisa informar os dados do contato para atualização");

        var existingContato = _context.Contatos.FirstOrDefault(c => c.Id == dadosContato.Id);
        if (existingContato == null)
            throw new ArgumentException("Contato não encontrado");

        existingContato.DataModificacao = DateTime.Now;
        existingContato.Nome = dadosContato.Nome != null ? dadosContato.Nome : existingContato.Nome;
        existingContato.Email = dadosContato.Email != null ? dadosContato.Email : existingContato.Email;
        existingContato.Telefone = dadosContato.Telefone != null ? dadosContato.Telefone : existingContato.Telefone;
        existingContato.DDD = dadosContato.DDD > 0 ? dadosContato.DDD : existingContato.DDD;

        _context.Entry(existingContato).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void ExcluirContato(int Id)
    {
        var contato = _context.Contatos.FirstOrDefault(c => c.Id == Id);
        if (contato == null)
            throw new ArgumentException("Contato não encontrado");

        _context.Contatos.Remove(contato);
        _context.SaveChanges();
    }

    public IEnumerable<ContatoDTO> ListarContatos()
    {
        var query = from contato in _context.Contatos
                    join municipio in _context.Municipios on contato.DDD equals municipio.Ddd
                    join estado in _context.Estados on municipio.CodigoUf equals estado.CodigoUf
                    select new ContatoDTO
                    {
                        Id = contato.Id,
                        Nome = contato.Nome!,
                        Email = contato.Email!,
                        Telefone = contato.Telefone!,
                        DDD = contato.DDD ?? 0,
                        DataCriacao = contato.DataCriacao,
                        DataModificacao = contato.DataModificacao,
                        NomeEstado = estado.Nome,
                        Regiao = estado.Regiao!
                    };

        return query.Distinct().ToList();
    }

    public IEnumerable<ContatoDTO> ListarContatosDdd(int ddd)
    {
        var query = from contato in _context.Contatos
                    where contato.DDD == ddd
                    join municipio in _context.Municipios on contato.DDD equals municipio.Ddd
                    join estado in _context.Estados on municipio.CodigoUf equals estado.CodigoUf
                    select new ContatoDTO
                    {
                        Id = contato.Id,
                        Nome = contato.Nome!,
                        Email = contato.Email!,
                        Telefone = contato.Telefone!,
                        DDD = contato.DDD ?? 0,
                        DataCriacao = contato.DataCriacao,
                        DataModificacao = contato.DataModificacao,
                        NomeEstado = estado.Nome,
                        Regiao = estado.Regiao!
                    };

        return query.Distinct().ToList();
    }
}

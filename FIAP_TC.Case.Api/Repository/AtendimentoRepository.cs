using FIAP_TC.Case.Api.Repository.Interfaces;
using FIAP_TC.Case.Core.DTO;
using FIAP_TC.Case.Core.Entities;
using FIAP_TC.Case.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP_TC.Case.Api.Repository
{
    public class AtendimentoRepository : IAtendimentoRepository
    {
        private readonly DbTcContext _context;

        public AtendimentoRepository(DbTcContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<AtendimentoDTO> BuscarAtendimentos(int? idAtendimento = null, int? idContato = null, string? nomeContato = null)
        {
            var query = from atendimento in _context.Atendimentos
                        join contato in _context.Contatos on atendimento.IdContato equals contato.IdContato
                        select new AtendimentoDTO
                        {
                            IdAtendimento = atendimento.IdAtendimento,
                            Assunto = atendimento.Assunto,
                            Descricao = atendimento.Descricao,
                            DataSolicitacao = atendimento.DataSolicitacao,
                            DataModificacao = atendimento.DataModificacao,
                            IdContato = atendimento.IdContato,
                            NomeContato = contato.Nome,
                            EmailContato = contato.Email
                        };

            if (idAtendimento.HasValue)
            {
                query = query.Where(a => a.IdAtendimento == idAtendimento.Value);
            }

            if (idContato.HasValue)
            {
                query = query.Where(a => a.IdContato == idContato.Value);
            }

            if (!string.IsNullOrEmpty(nomeContato))
            {
                query = query.Where(a => a.NomeContato.Contains(nomeContato));
            }

            return query.ToList();
        }

        public Atendimento CriarAtendimento(Atendimento dadosAtendimento)
        {
            if (dadosAtendimento == null)
                throw new ArgumentException("Você precisa informar os dados do atendimento para cadastro");

            dadosAtendimento.DataModificacao = DateTime.Now;
            _context.Atendimentos.Add(dadosAtendimento);
            _context.SaveChanges();

            return dadosAtendimento;
        }

        public void AtualizarAtendimento(Atendimento dadosAtendimento)
        {
            if (dadosAtendimento == null)
                throw new ArgumentException("Você precisa informar os dados do atendimento para atualização");

            var existingAtendimento = _context.Atendimentos.FirstOrDefault(a => a.IdAtendimento == dadosAtendimento.IdAtendimento);
            if (existingAtendimento == null)
                throw new ArgumentException("Atendimento não encontrado");

            existingAtendimento.Assunto = dadosAtendimento.Assunto;
            existingAtendimento.Descricao = dadosAtendimento.Descricao;
            existingAtendimento.DataModificacao = DateTime.Now;
            existingAtendimento.IdContato = dadosAtendimento.IdContato;

            _context.Entry(existingAtendimento).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void ExcluirAtendimento(int id)
        {
            var atendimento = _context.Atendimentos.FirstOrDefault(a => a.IdAtendimento == id);
            if (atendimento == null)
                throw new ArgumentException("Atendimento não encontrado");

            _context.Atendimentos.Remove(atendimento);
            _context.SaveChanges();
        }

        public AtendimentoDTO? ObterAtendimentoPorId(int id)
        {
            var query = from atendimento in _context.Atendimentos
                        join contato in _context.Contatos on atendimento.IdContato equals contato.IdContato
                        where atendimento.IdAtendimento == id
                        select new AtendimentoDTO
                        {
                            IdAtendimento = atendimento.IdAtendimento,
                            Assunto = atendimento.Assunto,
                            Descricao = atendimento.Descricao,
                            DataSolicitacao = atendimento.DataSolicitacao,
                            DataModificacao = atendimento.DataModificacao,
                            IdContato = atendimento.IdContato,
                            NomeContato = contato.Nome,
                            EmailContato = contato.Email
                        };

            // Retorna null se não encontrar o atendimento
            return query.FirstOrDefault();
        }

    }
}

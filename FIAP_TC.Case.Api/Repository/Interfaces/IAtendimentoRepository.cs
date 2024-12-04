using FIAP_TC.Case.Core.DTO;
using FIAP_TC.Case.Core.Entities;

namespace FIAP_TC.Case.Api.Repository.Interfaces
{
    public interface IAtendimentoRepository
    {
        IEnumerable<AtendimentoDTO> BuscarAtendimentos(int? idAtendimento = null, int? idContato = null, string? nomeContato = null);

        Atendimento CriarAtendimento(Atendimento dadosAtendimento);

        void AtualizarAtendimento(Atendimento dadosAtendimento);

        void ExcluirAtendimento(int id);

        // Método atualizado para permitir retorno nulo
        AtendimentoDTO? ObterAtendimentoPorId(int id);
    }
}

using FIAP_TC.Contact.Core.DTO;
using FIAP_TC.Contact.Core.Entities;

namespace FIAP_TC.Contact.Api.Repository.Interfaces;

public interface IContatoRepository
{
    IEnumerable<ContatoDTO> ListarContatos();
    IEnumerable<ContatoDTO> ListarContatosDdd(int ddd);
    Contato CriarContato(Contato dadosContato);
    void AtualizarContato(Contato dadosContato);
    void ExcluirContato(int Id);
}
using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;

namespace Hospisim.Business.Contracts
{
    public interface IInternacaoService
    {
        Task<IEnumerable<Internacao>> GetAllAsync();
        Task<Internacao?> GetByIdAsync(Guid id);
        Task<Internacao> CreateAsync(Internacao internacao);
        Task<Internacao> UpdateAsync(Internacao internacao);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Internacao>> GetByPacienteAsync(Guid pacienteId);
        Task<IEnumerable<Internacao>> GetByStatusAsync(StatusInternacao status);
        Task<IEnumerable<Internacao>> GetAtivasAsync();
        Task<IEnumerable<Internacao>> GetBySetorAsync(string setor);
    }
} 
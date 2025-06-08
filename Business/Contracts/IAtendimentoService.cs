using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;

namespace Hospisim.Business.Contracts
{
    public interface IAtendimentoService
    {
        Task<IEnumerable<Atendimento>> GetAllAsync();
        Task<Atendimento?> GetByIdAsync(Guid id);
        Task<Atendimento> CreateAsync(Atendimento atendimento);
        Task<Atendimento> UpdateAsync(Atendimento atendimento);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Atendimento>> GetByPacienteAsync(Guid pacienteId);
        Task<IEnumerable<Atendimento>> GetByProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<Atendimento>> GetByStatusAsync(StatusAtendimento status);
        Task<IEnumerable<Atendimento>> GetByDataAsync(DateTime data);
    }
} 
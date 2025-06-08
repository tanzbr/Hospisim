using Hospisim.Domain.Entities;

namespace Hospisim.Business.Contracts
{
    public interface IProntuarioService
    {
        Task<IEnumerable<Prontuario>> GetAllAsync();
        Task<Prontuario?> GetByIdAsync(Guid id);
        Task<Prontuario> CreateAsync(Prontuario prontuario);
        Task<Prontuario> UpdateAsync(Prontuario prontuario);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Prontuario>> GetByPacienteAsync(Guid pacienteId);
        Task<Prontuario?> GetByNumeroAsync(string numero);
    }
} 
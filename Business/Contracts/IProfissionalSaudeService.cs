using Hospisim.Domain.Entities;

namespace Hospisim.Business.Contracts
{
    public interface IProfissionalSaudeService
    {
        Task<IEnumerable<ProfissionalSaude>> GetAllAsync();
        Task<ProfissionalSaude?> GetByIdAsync(Guid id);
        Task<ProfissionalSaude> CreateAsync(ProfissionalSaude profissional);
        Task<ProfissionalSaude> UpdateAsync(ProfissionalSaude profissional);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<ProfissionalSaude>> GetByEspecialidadeAsync(Guid especialidadeId);
        Task<IEnumerable<ProfissionalSaude>> GetAtivosAsync();
    }
} 
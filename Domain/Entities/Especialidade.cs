using System.ComponentModel.DataAnnotations;

namespace Hospisim.Domain.Entities
{
    public class Especialidade
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(80)]
        public string Nome { get; set; } = default!;

        public ICollection<ProfissionalSaude> Profissionais { get; set; } = new List<ProfissionalSaude>();
    }
}

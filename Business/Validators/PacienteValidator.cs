using FluentValidation;
using Hospisim.Business.Common;
using Hospisim.Domain.Entities;

namespace Hospisim.Business.Validators
{
    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(p => p.NomeCompleto).NotEmpty().MaximumLength(120);
            RuleFor(p => p.CPF)
                .NotEmpty()
                .Must(CpfHelper.IsValid).WithMessage("CPF inválido");
            RuleFor(p => p.Email)
                .EmailAddress()
                .When(p => !string.IsNullOrWhiteSpace(p.Email));
            RuleFor(p => p.DataNascimento)
                .LessThan(DateTime.Today).WithMessage("Data de nascimento no futuro");
        }
    }
}

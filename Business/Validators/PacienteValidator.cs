using FluentValidation;
using Hospisim.Business.Common;
using Hospisim.Domain.Entities;

namespace Hospisim.Business.Validators
{
    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(p => p.NomeCompleto)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(120);

            RuleFor(p => p.CPF)
                .NotEmpty().Must(CpfHelper.IsValid)
                .WithMessage("CPF inválido");

            RuleFor(p => p.DataNascimento)
                .LessThan(DateTime.Today).WithMessage("Data de nascimento futura");

            When(p => !string.IsNullOrWhiteSpace(p.Email), () =>
            {
                RuleFor(p => p.Email).EmailAddress();
            });

            RuleFor(p => p.Telefone)
                .MaximumLength(20);

            RuleFor(p => p.NumeroCartaoSUS)
                .MaximumLength(15);
        }
    }
}

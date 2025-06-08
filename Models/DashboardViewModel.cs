using Hospisim.Domain.Entities;

namespace Hospisim.Models
{
    public class DashboardViewModel
    {
        public int TotalPacientes { get; set; }
        public int AtendimentosHoje { get; set; }
        public int InternacoesAtivas { get; set; }
        public int ProfissionaisAtivos { get; set; }
        public int AltasHoje { get; set; }
        public int TotalAtendimentos { get; set; }
        public int TotalInternacoes { get; set; }
        public int TotalAltas { get; set; }
        
        public List<AtendimentoRecente> AtendimentosRecentes { get; set; } = new();
        public List<InternacaoAtiva> InternacoesAtivasDetalhes { get; set; } = new();
        public List<AltaRecente> AltasRecentes { get; set; } = new();
    }

    public class AtendimentoRecente
    {
        public string PacienteNome { get; set; } = string.Empty;
        public string ProfissionalNome { get; set; } = string.Empty;
        public string TipoAtendimento { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public Guid AtendimentoId { get; set; }
    }

    public class InternacaoAtiva
    {
        public string PacienteNome { get; set; } = string.Empty;
        public string Setor { get; set; } = string.Empty;
        public string Quarto { get; set; } = string.Empty;
        public string Leito { get; set; } = string.Empty;
        public DateTime DataEntrada { get; set; }
        public string StatusDescricao { get; set; } = string.Empty;
        public string StatusCor { get; set; } = string.Empty;
        public Guid InternacaoId { get; set; }
        public int DiasInternado { get; set; }
    }

    public class AltaRecente
    {
        public string PacienteNome { get; set; } = string.Empty;
        public string CondicaoPaciente { get; set; } = string.Empty;
        public DateTime DataAlta { get; set; }
        public string Setor { get; set; } = string.Empty;
        public Guid AltaId { get; set; }
        public int DiasInternado { get; set; }
    }
} 
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hospisim.Models;
using Hospisim.Data;
using Hospisim.Domain.Enums;

namespace Hospisim.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var hoje = DateTime.Today;
        
        var viewModel = new DashboardViewModel
        {
            // Estatísticas gerais
            TotalPacientes = await _context.Pacientes.CountAsync(),
            AtendimentosHoje = await _context.Atendimentos
                .Where(a => a.DataHora.Date == hoje)
                .CountAsync(),
            InternacoesAtivas = await _context.Internacoes
                .Where(i => i.StatusInternacao == StatusInternacao.Ativa)
                .CountAsync(),
            ProfissionaisAtivos = await _context.Profissionais
                .Where(p => p.Ativo)
                .CountAsync(),
            AltasHoje = await _context.AltasHospitalares
                .Where(a => a.DataAlta.Date == hoje)
                .CountAsync(),
            TotalAtendimentos = await _context.Atendimentos.CountAsync(),
            TotalInternacoes = await _context.Internacoes.CountAsync(),
            TotalAltas = await _context.AltasHospitalares.CountAsync(),
            
            // Atendimentos recentes (últimos 5)
            AtendimentosRecentes = await _context.Atendimentos
                .Include(a => a.Paciente)
                .Include(a => a.Profissional)
                .ThenInclude(p => p!.Especialidade)
                .OrderByDescending(a => a.DataHora)
                .Take(5)
                .Select(a => new AtendimentoRecente
                {
                    PacienteNome = a.Paciente!.NomeCompleto,
                    ProfissionalNome = a.Profissional!.NomeCompleto,
                    TipoAtendimento = a.Tipo.ToString(),
                    Especialidade = a.Profissional.Especialidade!.Nome,
                    DataHora = a.DataHora,
                    AtendimentoId = a.Id
                })
                .ToListAsync(),
                
            // Internações ativas (últimas 5)
            InternacoesAtivasDetalhes = await _context.Internacoes
                .Include(i => i.Paciente)
                .Where(i => i.StatusInternacao == StatusInternacao.Ativa)
                .OrderByDescending(i => i.DataEntrada)
                .Take(5)
                .Select(i => new InternacaoAtiva
                {
                    PacienteNome = i.Paciente!.NomeCompleto,
                    Setor = i.Setor,
                    Quarto = i.Quarto,
                    Leito = i.Leito,
                    DataEntrada = i.DataEntrada,
                    StatusDescricao = "Ativa",
                    StatusCor = "success",
                    InternacaoId = i.Id,
                    DiasInternado = (int)(DateTime.Now - i.DataEntrada).TotalDays
                })
                .ToListAsync(),
                
            // Altas recentes (últimas 5)
            AltasRecentes = await _context.AltasHospitalares
                .Include(a => a.Internacao)
                .ThenInclude(i => i!.Paciente)
                .OrderByDescending(a => a.DataAlta)
                .Take(5)
                .Select(a => new AltaRecente
                {
                    PacienteNome = a.Internacao!.Paciente!.NomeCompleto,
                    CondicaoPaciente = a.CondicaoPaciente,
                    DataAlta = a.DataAlta,
                    Setor = a.Internacao.Setor,
                    AltaId = a.Id,
                    DiasInternado = (int)(a.DataAlta - a.Internacao.DataEntrada).TotalDays
                })
                .ToListAsync()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

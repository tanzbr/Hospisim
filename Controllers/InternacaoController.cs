using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospisim.Controllers
{
    public class InternacaoController : Controller
    {
        private readonly IInternacaoService _internacaoService;
        private readonly IPacienteService _pacienteService;
        private readonly IAtendimentoService _atendimentoService;

        public InternacaoController(
            IInternacaoService internacaoService,
            IPacienteService pacienteService,
            IAtendimentoService atendimentoService)
        {
            _internacaoService = internacaoService;
            _pacienteService = pacienteService;
            _atendimentoService = atendimentoService;
        }

        public async Task<IActionResult> Index()
        {
            var internacoes = await _internacaoService.GetAllAsync();
            return View(internacoes);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var internacao = await _internacaoService.GetByIdAsync(id);
            if (internacao == null)
                return NotFound();

            return View(internacao);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Internacao internacao)
        {
            // Validações customizadas
            if (internacao.PacienteId == Guid.Empty)
            {
                ModelState.AddModelError("PacienteId", "O paciente é obrigatório.");
            }
            if (internacao.AtendimentoId == Guid.Empty)
            {
                ModelState.AddModelError("AtendimentoId", "O atendimento é obrigatório.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _internacaoService.CreateAsync(internacao);
                    TempData["Success"] = "Internação criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro ao criar internação: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = $"Dados inválidos: {string.Join(", ", errors)}";
            }

            await PopulateDropDowns(internacao.PacienteId, internacao.AtendimentoId);
            return View(internacao);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var internacao = await _internacaoService.GetByIdAsync(id);
            if (internacao == null)
                return NotFound();

            await PopulateDropDowns(internacao.PacienteId, internacao.AtendimentoId);
            return View(internacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Internacao internacao)
        {
            if (id != internacao.Id)
                return NotFound();

            // Validações customizadas
            if (internacao.PacienteId == Guid.Empty)
            {
                ModelState.AddModelError("PacienteId", "O paciente é obrigatório.");
            }
            if (internacao.AtendimentoId == Guid.Empty)
            {
                ModelState.AddModelError("AtendimentoId", "O atendimento é obrigatório.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar a entidade existente do banco
                    var existingInternacao = await _internacaoService.GetByIdAsync(id);
                    if (existingInternacao == null)
                        return NotFound();

                    // Atualizar as propriedades
                    existingInternacao.PacienteId = internacao.PacienteId;
                    existingInternacao.AtendimentoId = internacao.AtendimentoId;
                    existingInternacao.DataEntrada = internacao.DataEntrada;
                    existingInternacao.PrevisaoAlta = internacao.PrevisaoAlta;
                    existingInternacao.MotivoInternacao = internacao.MotivoInternacao;
                    existingInternacao.Leito = internacao.Leito;
                    existingInternacao.Quarto = internacao.Quarto;
                    existingInternacao.Setor = internacao.Setor;
                    existingInternacao.PlanoSaudeUtilizado = internacao.PlanoSaudeUtilizado;
                    existingInternacao.ObservacoesClinicas = internacao.ObservacoesClinicas;
                    existingInternacao.StatusInternacao = internacao.StatusInternacao;

                    await _internacaoService.UpdateAsync(existingInternacao);
                    TempData["Success"] = "Internação atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro ao atualizar internação: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = $"Dados inválidos: {string.Join(", ", errors)}";
            }

            await PopulateDropDowns(internacao.PacienteId, internacao.AtendimentoId);
            return View(internacao);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var internacao = await _internacaoService.GetByIdAsync(id);
            if (internacao == null)
                return NotFound();

            return View(internacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _internacaoService.DeleteAsync(id);
            if (success)
            {
                TempData["Success"] = "Internação excluída com sucesso!";
            }
            else
            {
                TempData["Error"] = "Erro ao excluir internação.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDowns(object selectedPaciente = null, object selectedAtendimento = null)
        {
            var pacientes = await _pacienteService.GetAllAsync();
            var atendimentos = await _atendimentoService.GetAllAsync();

            ViewBag.PacienteId = new SelectList(pacientes, "Id", "NomeCompleto", selectedPaciente);
            ViewBag.AtendimentoId = new SelectList(atendimentos, "Id", "Id", selectedAtendimento);
        }
    }
} 
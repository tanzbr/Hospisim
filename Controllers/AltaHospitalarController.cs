using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospisim.Controllers
{
    public class AltaHospitalarController : Controller
    {
        private readonly IAltaHospitalarService _altaService;
        private readonly IInternacaoService _internacaoService;

        public AltaHospitalarController(
            IAltaHospitalarService altaService,
            IInternacaoService internacaoService)
        {
            _altaService = altaService;
            _internacaoService = internacaoService;
        }

        public async Task<IActionResult> Index()
        {
            var altas = await _altaService.GetAllAsync();
            return View(altas);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var alta = await _altaService.GetByIdAsync(id);
            if (alta == null)
                return NotFound();

            return View(alta);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateInternacoesDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AltaHospitalar alta)
        {
            // Validação customizada
            if (alta.InternacaoId == Guid.Empty)
            {
                ModelState.AddModelError("InternacaoId", "A internação é obrigatória.");
            }
            else
            {
                // Verificar se já existe alta para esta internação
                var altaExistente = await _altaService.GetByInternacaoAsync(alta.InternacaoId);
                if (altaExistente != null)
                {
                    ModelState.AddModelError("InternacaoId", "Esta internação já possui uma alta hospitalar.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _altaService.CreateAsync(alta);
                    
                    // Atualizar status da internação para AltaConcedida
                    var internacao = await _internacaoService.GetByIdAsync(alta.InternacaoId);
                    if (internacao != null)
                    {
                        internacao.StatusInternacao = StatusInternacao.AltaConcedida;
                        await _internacaoService.UpdateAsync(internacao);
                    }

                    TempData["Success"] = "Alta hospitalar criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro ao criar alta hospitalar: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = $"Dados inválidos: {string.Join(", ", errors)}";
            }

            await PopulateInternacoesDropDown(alta.InternacaoId);
            return View(alta);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var alta = await _altaService.GetByIdAsync(id);
            if (alta == null)
                return NotFound();

            await PopulateInternacoesDropDown(alta.InternacaoId);
            return View(alta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AltaHospitalar alta)
        {
            if (id != alta.Id)
                return NotFound();

            // Validação customizada
            if (alta.InternacaoId == Guid.Empty)
            {
                ModelState.AddModelError("InternacaoId", "A internação é obrigatória.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar a entidade existente do banco
                    var existingAlta = await _altaService.GetByIdAsync(id);
                    if (existingAlta == null)
                        return NotFound();

                    // Atualizar as propriedades
                    existingAlta.DataAlta = alta.DataAlta;
                    existingAlta.CondicaoPaciente = alta.CondicaoPaciente;
                    existingAlta.InstrucoesPosAlta = alta.InstrucoesPosAlta;
                    existingAlta.InternacaoId = alta.InternacaoId;

                    await _altaService.UpdateAsync(existingAlta);
                    TempData["Success"] = "Alta hospitalar atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro ao atualizar alta hospitalar: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = $"Dados inválidos: {string.Join(", ", errors)}";
            }

            await PopulateInternacoesDropDown(alta.InternacaoId);
            return View(alta);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var alta = await _altaService.GetByIdAsync(id);
            if (alta == null)
                return NotFound();

            return View(alta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var alta = await _altaService.GetByIdAsync(id);
                if (alta != null)
                {
                    // Reverter status da internação para Ativa
                    var internacao = await _internacaoService.GetByIdAsync(alta.InternacaoId);
                    if (internacao != null)
                    {
                        internacao.StatusInternacao = StatusInternacao.Ativa;
                        await _internacaoService.UpdateAsync(internacao);
                    }
                }

                var success = await _altaService.DeleteAsync(id);
                if (success)
                {
                    TempData["Success"] = "Alta hospitalar excluída com sucesso!";
                }
                else
                {
                    TempData["Error"] = "Erro ao excluir alta hospitalar.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao excluir alta hospitalar: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateInternacoesDropDown(object selectedInternacao = null)
        {
            // Buscar apenas internações ativas (sem alta)
            var internacoes = await _internacaoService.GetByStatusAsync(StatusInternacao.Ativa);
            
            ViewBag.InternacaoId = new SelectList(
                internacoes.Select(i => new { 
                    Id = i.Id, 
                    Display = $"{i.Paciente?.NomeCompleto} - {i.Setor} {i.Quarto}/{i.Leito} ({i.DataEntrada:dd/MM/yyyy})" 
                }), 
                "Id", 
                "Display", 
                selectedInternacao);
        }
    }
} 
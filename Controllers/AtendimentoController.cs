using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospisim.Controllers
{
    public class AtendimentoController : Controller
    {
        private readonly IAtendimentoService _atendimentoService;
        private readonly IPacienteService _pacienteService;
        private readonly IProfissionalSaudeService _profissionalService;
        private readonly IProntuarioService _prontuarioService;

        public AtendimentoController(
            IAtendimentoService atendimentoService,
            IPacienteService pacienteService,
            IProfissionalSaudeService profissionalService,
            IProntuarioService prontuarioService)
        {
            _atendimentoService = atendimentoService;
            _pacienteService = pacienteService;
            _profissionalService = profissionalService;
            _prontuarioService = prontuarioService;
        }

        public async Task<IActionResult> Index()
        {
            var atendimentos = await _atendimentoService.GetAllAsync();
            return View(atendimentos);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var atendimento = await _atendimentoService.GetByIdAsync(id);
            if (atendimento == null)
                return NotFound();

            return View(atendimento);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Atendimento atendimento)
        {
            if (ModelState.IsValid)
            {
                await _atendimentoService.CreateAsync(atendimento);
                TempData["Success"] = "Atendimento criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDowns(atendimento.PacienteId, atendimento.ProfissionalId, atendimento.ProntuarioId);
            return View(atendimento);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var atendimento = await _atendimentoService.GetByIdAsync(id);
            if (atendimento == null)
                return NotFound();

            await PopulateDropDowns(atendimento.PacienteId, atendimento.ProfissionalId, atendimento.ProntuarioId);
            return View(atendimento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Atendimento atendimento)
        {
            if (id != atendimento.Id)
                return NotFound();

            // Validações customizadas
            if (atendimento.PacienteId == Guid.Empty)
            {
                ModelState.AddModelError("PacienteId", "O paciente é obrigatório.");
            }
            if (atendimento.ProfissionalId == Guid.Empty)
            {
                ModelState.AddModelError("ProfissionalId", "O profissional é obrigatório.");
            }
            if (atendimento.ProntuarioId == Guid.Empty)
            {
                ModelState.AddModelError("ProntuarioId", "O prontuário é obrigatório.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar a entidade existente do banco
                    var existingAtendimento = await _atendimentoService.GetByIdAsync(id);
                    if (existingAtendimento == null)
                        return NotFound();

                    // Atualizar as propriedades
                    existingAtendimento.DataHora = atendimento.DataHora;
                    existingAtendimento.Tipo = atendimento.Tipo;
                    existingAtendimento.Status = atendimento.Status;
                    existingAtendimento.Local = atendimento.Local;
                    existingAtendimento.PacienteId = atendimento.PacienteId;
                    existingAtendimento.ProfissionalId = atendimento.ProfissionalId;
                    existingAtendimento.ProntuarioId = atendimento.ProntuarioId;

                    await _atendimentoService.UpdateAsync(existingAtendimento);
                    TempData["Success"] = "Atendimento atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro ao atualizar atendimento: {ex.Message}";
                }
            }
            else
            {
                // Debug: Adicionar erros do ModelState ao TempData
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = $"Dados inválidos: {string.Join(", ", errors)}";
            }
            
            await PopulateDropDowns(atendimento.PacienteId, atendimento.ProfissionalId, atendimento.ProntuarioId);
            return View(atendimento);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var atendimento = await _atendimentoService.GetByIdAsync(id);
            if (atendimento == null)
                return NotFound();

            return View(atendimento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _atendimentoService.DeleteAsync(id);
            if (success)
            {
                TempData["Success"] = "Atendimento excluído com sucesso!";
            }
            else
            {
                TempData["Error"] = "Erro ao excluir atendimento.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetProntuariosByPaciente(Guid pacienteId)
        {
            var prontuarios = await _prontuarioService.GetByPacienteAsync(pacienteId);
            var result = prontuarios.Select(p => new { 
                value = p.Id, 
                text = $"Prontuário {p.Numero} - {p.DataAbertura:dd/MM/yyyy}" 
            });
            return Json(result);
        }

        private async Task PopulateDropDowns(object selectedPaciente = null, object selectedProfissional = null, object selectedProntuario = null)
        {
            var pacientes = await _pacienteService.GetAllAsync();
            var profissionais = await _profissionalService.GetAtivosAsync();
            var prontuarios = await _prontuarioService.GetAllAsync();

            ViewBag.PacienteId = new SelectList(pacientes, "Id", "NomeCompleto", selectedPaciente);
            ViewBag.ProfissionalId = new SelectList(profissionais, "Id", "NomeCompleto", selectedProfissional);
            ViewBag.ProntuarioId = new SelectList(prontuarios, "Id", "Numero", selectedProntuario);
        }
    }
} 
using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospisim.Controllers
{
    public class ProfissionalSaudeController : Controller
    {
        private readonly IProfissionalSaudeService _profissionalService;
        private readonly IEspecialidadeService _especialidadeService;

        public ProfissionalSaudeController(
            IProfissionalSaudeService profissionalService,
            IEspecialidadeService especialidadeService)
        {
            _profissionalService = profissionalService;
            _especialidadeService = especialidadeService;
        }

        public async Task<IActionResult> Index()
        {
            var profissionais = await _profissionalService.GetAllAsync();
            return View(profissionais);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var profissional = await _profissionalService.GetByIdAsync(id);
            if (profissional == null)
                return NotFound();

            return View(profissional);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateEspecialidadesDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfissionalSaude profissional)
        {
            if (ModelState.IsValid)
            {
                await _profissionalService.CreateAsync(profissional);
                TempData["Success"] = "Profissional criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateEspecialidadesDropDown(profissional.EspecialidadeId);
            return View(profissional);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var profissional = await _profissionalService.GetByIdAsync(id);
            if (profissional == null)
                return NotFound();

            await PopulateEspecialidadesDropDown(profissional.EspecialidadeId);
            return View(profissional);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfissionalSaude profissional)
        {
            if (id != profissional.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _profissionalService.UpdateAsync(profissional);
                TempData["Success"] = "Profissional atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateEspecialidadesDropDown(profissional.EspecialidadeId);
            return View(profissional);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var profissional = await _profissionalService.GetByIdAsync(id);
            if (profissional == null)
                return NotFound();

            return View(profissional);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _profissionalService.DeleteAsync(id);
            if (success)
            {
                TempData["Success"] = "Profissional excluído com sucesso!";
            }
            else
            {
                TempData["Error"] = "Erro ao excluir profissional.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateEspecialidadesDropDown(object selectedEspecialidade = null)
        {
            var especialidades = await _especialidadeService.GetAllAsync();
            ViewBag.EspecialidadeId = new SelectList(especialidades, "Id", "Nome", selectedEspecialidade);
        }
    }
} 
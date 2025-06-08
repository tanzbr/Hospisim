using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hospisim.Controllers
{
    public class EspecialidadeController : Controller
    {
        private readonly IEspecialidadeService _especialidadeService;

        public EspecialidadeController(IEspecialidadeService especialidadeService)
        {
            _especialidadeService = especialidadeService;
        }

        public async Task<IActionResult> Index()
        {
            var especialidades = await _especialidadeService.GetAllAsync();
            return View(especialidades);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var especialidade = await _especialidadeService.GetByIdAsync(id);
            if (especialidade == null)
                return NotFound();

            return View(especialidade);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Especialidade especialidade)
        {
            if (ModelState.IsValid)
            {
                await _especialidadeService.CreateAsync(especialidade);
                TempData["Success"] = "Especialidade criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(especialidade);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var especialidade = await _especialidadeService.GetByIdAsync(id);
            if (especialidade == null)
                return NotFound();

            return View(especialidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Especialidade especialidade)
        {
            if (id != especialidade.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _especialidadeService.UpdateAsync(especialidade);
                TempData["Success"] = "Especialidade atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(especialidade);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var especialidade = await _especialidadeService.GetByIdAsync(id);
            if (especialidade == null)
                return NotFound();

            return View(especialidade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _especialidadeService.DeleteAsync(id);
            if (success)
            {
                TempData["Success"] = "Especialidade excluída com sucesso!";
            }
            else
            {
                TempData["Error"] = "Erro ao excluir especialidade.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 
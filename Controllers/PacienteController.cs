using Hospisim.Business.Contracts;
using Hospisim.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospisim.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteService _service;
        public PacienteController(IPacienteService service) => _service = service;

        // GET: /Paciente
        public async Task<IActionResult> Index()
            => View((await _service.GetAllAsync())
                    .Select(PacienteVM.FromEntity));

        // GET: /Paciente/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();
            return View(PacienteVM.FromEntity(ent));
        }

        // GET: /Paciente/Create
        public IActionResult Create() => View();

        // POST: /Paciente/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var entidade = new Hospisim.Domain.Entities.Paciente();
            vm.UpdateEntity(entidade);
            await _service.AddAsync(entidade);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Paciente/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();
            return View(PacienteVM.FromEntity(ent));
        }

        // POST: /Paciente/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PacienteVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();

            vm.UpdateEntity(ent);
            await _service.UpdateAsync(ent);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Paciente/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();
            return View(PacienteVM.FromEntity(ent));
        }

        // POST: /Paciente/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

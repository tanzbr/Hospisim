using FluentValidation;
using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Hospisim.Domain.Enums;
using Hospisim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospisim.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteService _service;
        public PacienteController(IPacienteService service) => _service = service;

        /* ---------- helpers ---------- */
        private static IEnumerable<SelectListItem> EnumSelect<T>() where T : Enum =>
            Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new SelectListItem(e.ToString(), e.ToString()));

        private void PreencherDrops()
        {
            ViewBag.Sexos = EnumSelect<Sexo>();
            ViewBag.Sangues = EnumSelect<TipoSanguineo>();
            ViewBag.EstadosCivis = EnumSelect<EstadoCivil>();
        }

        /* ---------- Actions ---------- */

        public async Task<IActionResult> Index()
            => View((await _service.GetAllAsync()).Select(PacienteVM.FromEntity));

        public async Task<IActionResult> Details(Guid id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();
            return View(PacienteVM.FromEntity(ent));
        }

        public IActionResult Create()
        {
            PreencherDrops();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteVM vm)
        {
            if (!ModelState.IsValid)
            {
                PreencherDrops();
                return View(vm);
            }

            var entidade = new Paciente();
            vm.UpdateEntity(entidade);

            try
            {
                await _service.AddAsync(entidade);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                foreach (var err in ex.Errors)
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            PreencherDrops();
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();

            PreencherDrops();
            return View(PacienteVM.FromEntity(ent));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PacienteVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                PreencherDrops();
                return View(vm);
            }

            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();

            vm.UpdateEntity(ent);

            try
            {
                await _service.UpdateAsync(ent);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                foreach (var err in ex.Errors)
                    ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
            }

            PreencherDrops();
            return View(vm);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var ent = await _service.GetByIdAsync(id);
            if (ent is null) return NotFound();
            return View(PacienteVM.FromEntity(ent));
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

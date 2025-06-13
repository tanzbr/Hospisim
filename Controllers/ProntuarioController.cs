using Hospisim.Business.Contracts;
using Hospisim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospisim.Controllers
{
    public class ProntuarioController : Controller
    {
        private readonly IProntuarioService _prontuarioService;
        private readonly IPacienteService _pacienteService;

        public ProntuarioController(IProntuarioService prontuarioService, IPacienteService pacienteService)
        {
            _prontuarioService = prontuarioService;
            _pacienteService = pacienteService;
        }

        public async Task<IActionResult> Index()
        {
            var prontuarios = await _prontuarioService.GetAllAsync();
            return View(prontuarios);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var prontuario = await _prontuarioService.GetByIdAsync(id);
            if (prontuario == null) return NotFound();
            return View(prontuario);
        }

        public async Task<IActionResult> Create()
        {
            await PopularPacientesDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prontuario prontuario)
        {
            if (ModelState.IsValid)
            {
                await _prontuarioService.CreateAsync(prontuario);
                TempData["Success"] = "Prontuário criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            await PopularPacientesDropDown(prontuario.PacienteId);
            return View(prontuario);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var prontuario = await _prontuarioService.GetByIdAsync(id);
            if (prontuario == null) return NotFound();
            await PopularPacientesDropDown(prontuario.PacienteId);
            return View(prontuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Prontuario prontuario)
        {
            if (id != prontuario.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var existente = await _prontuarioService.GetByIdAsync(id);
                if (existente == null) return NotFound();
                existente.Numero = prontuario.Numero;
                existente.DataAbertura = prontuario.DataAbertura;
                existente.ObservacoesGerais = prontuario.ObservacoesGerais;
                existente.PacienteId = prontuario.PacienteId;
                await _prontuarioService.UpdateAsync(existente);
                TempData["Success"] = "Prontuário atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            await PopularPacientesDropDown(prontuario.PacienteId);
            return View(prontuario);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var prontuario = await _prontuarioService.GetByIdAsync(id);
            if (prontuario == null) return NotFound();
            return View(prontuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _prontuarioService.DeleteAsync(id);
            TempData["Success"] = "Prontuário excluído com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopularPacientesDropDown(Guid? selecionado = null)
        {
            var pacientes = await _pacienteService.GetAllAsync();
            ViewBag.PacienteId = new SelectList(pacientes, "Id", "NomeCompleto", selecionado);
        }
    }
} 
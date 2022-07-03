using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicalHandbook.Models;
using MedicalHandbook.ViewModels;

namespace MedicalHandbook.Controllers
{
    public class MedicineController : Controller
    {
        private readonly MedicineContext _context;

        public MedicineController(MedicineContext context)
        {
            _context = context;
        }

        // GET: Medicine
        public IActionResult Index(string name, string group, string activeSubstance)
        {
            IQueryable<Medicine> medicines = _context.Medicines;

            if(name!=null)
            {
                medicines = medicines.Where(m => m.Name == name);
            }

            if(group!=null)
            {
                medicines = medicines.Where(m => m.Group == group);
            }

            if(activeSubstance!=null)
            {
                medicines = medicines.Where(m => m.ActiveSubstance == activeSubstance);
            }

            var names = _context.Medicines.Select(m => m.Name).Distinct().ToList();
            var groups = _context.Medicines.Select(m => m.Group).Distinct().ToList();
            var activeSubstances = _context.Medicines.Select(m => m.ActiveSubstance).Distinct();

            var viewModel = new MedicineListViewModel
            {
                Medicines = medicines.ToList(),
                ActiveSubstances = new SelectList(activeSubstances),
                Groups = new SelectList(groups),
                Names = new SelectList(names)
            };

            return View(viewModel);
        }

        // GET: Medicine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: Medicine/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ActiveSubstance,Group,Description")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: Medicine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // POST: Medicine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ActiveSubstance,Group,Description")] Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: Medicine/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: Medicine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}

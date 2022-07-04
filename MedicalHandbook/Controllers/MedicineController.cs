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
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = _context.Medicines.FirstOrDefault(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: Medicine/Create
        public IActionResult Create(bool isFailed = false)
        {
            ViewBag.IsFailed = isFailed;
            return View();
        }

        // POST: Medicine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,ActiveSubstance,Group,Description")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                bool isExisted = _context.Medicines
                    .Any(m => m.Name == medicine.Name && m.ActiveSubstance == medicine.ActiveSubstance);

                if (!isExisted)
                {
                    _context.Add(medicine);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Create), new { isFailed = isExisted });
                }
            }
            return View(medicine);
        }

        // GET: Medicine/Edit/5
        public IActionResult Edit(int id, bool isFailed = false)
        {
            ViewBag.IsFailed = isFailed;

            var medicine = _context.Medicines.Find(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // POST: Medicine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,ActiveSubstance,Group,Description")] Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return NotFound();
            }
            bool isExisted = false;
            var existed = _context.Medicines
                .Where(m => m.Name == medicine.Name && m.ActiveSubstance == medicine.ActiveSubstance && m.Id != medicine.Id)
                .FirstOrDefault();
            
            if (existed != null)
                isExisted = true;

            if (ModelState.IsValid)
            {

                if (!isExisted)
                {
                    try
                    {
                        _context.Update(medicine);
                        _context.SaveChanges();
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
                else
                {
                    return RedirectToAction(nameof(Edit), new { id = medicine.Id, isFailed = isExisted });
                }
            }
            return View(medicine);
        }

        // GET: Medicine/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = _context.Medicines
                .FirstOrDefault(m => m.Id == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: Medicine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var medicine = _context.Medicines.Find(id);
            _context.Medicines.Remove(medicine);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}

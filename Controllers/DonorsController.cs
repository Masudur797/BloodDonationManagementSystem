using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloodDonationManagementSystem.Models;

namespace BloodDonationManagementSystem.Controllers
{
    public class DonorsController : Controller
    {
        private readonly BloodBankDbContext _context;

        public DonorsController(BloodBankDbContext context)
        {
            _context = context;
        }

        // GET: Donors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donors.ToListAsync());
        }

        // GET: Donors/Details/5
        public async Task<IActionResult> Details(int? donorid)
        {
            if (donorid == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors
                .FirstOrDefaultAsync(m => m.DonorId == donorid);

            if (donor == null)
            {
                return NotFound();
            }

            return View(donor);
        }

        // GET: Donors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Donors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DonorId,FullName,BloodGroup,ContactNo,City,LastDonationDate")] Donor donor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(donor);
        }

        // GET: Donors/Edit/5
        public async Task<IActionResult> Edit(int? donorid)
        {
            if (donorid == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors.FindAsync(donorid);

            if (donor == null)
            {
                return NotFound();
            }

            return View(donor);
        }

        // POST: Donors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? donorid, [Bind("DonorId,FullName,BloodGroup,ContactNo,City,LastDonationDate")] Donor donor)
        {
            if (donorid != donor.DonorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonorExists(donor.DonorId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(donor);
        }

        // GET: Donors/Delete/5
        public async Task<IActionResult> Delete(int? donorid)
        {
            if (donorid == null)
            {
                return NotFound();
            }

            var donor = await _context.Donors
                .FirstOrDefaultAsync(m => m.DonorId == donorid);

            if (donor == null)
            {
                return NotFound();
            }

            return View(donor);
        }

        // POST: Donors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? donorid)
        {
            var donor = await _context.Donors.FindAsync(donorid);

            if (donor != null)
            {
                _context.Donors.Remove(donor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DonorExists(int? donorid)
        {
            return _context.Donors.Any(e => e.DonorId == donorid);
        }
    }
}
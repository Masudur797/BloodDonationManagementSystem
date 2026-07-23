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

        // ===========================
        // Donor List
        // ===========================
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donors.ToListAsync());
        }

        // ===========================
        // Details
        // ===========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var donor = await _context.Donors
                .FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

        // ===========================
        // Create (GET)
        // ===========================
        public IActionResult Create()
        {
            return View();
        }

        // ===========================
        // Create (POST)
        // ===========================
        [HttpPost]
        public async Task<IActionResult> Create(Donor donor)
        {
            if (string.IsNullOrWhiteSpace(donor.FullName))
                ModelState.AddModelError("", "Full Name is required.");

            if (string.IsNullOrWhiteSpace(donor.BloodGroup))
                ModelState.AddModelError("", "Blood Group is required.");

            if (!ModelState.IsValid)
                return View(donor);

            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // Edit (GET)
        // ===========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var donor = await _context.Donors.FindAsync(id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

        // ===========================
        // Edit (POST)
        // ===========================
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Donor donor)
        {
            if (id != donor.DonorId)
                return NotFound();

            if (string.IsNullOrWhiteSpace(donor.FullName))
                ModelState.AddModelError("", "Full Name is required.");

            if (string.IsNullOrWhiteSpace(donor.BloodGroup))
                ModelState.AddModelError("", "Blood Group is required.");

            if (!ModelState.IsValid)
                return View(donor);

            try
            {
                _context.Update(donor);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return View(donor);
            }

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // Delete (GET)
        // ===========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var donor = await _context.Donors
                .FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

        // ===========================
        // Delete (POST)
        // ===========================
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donor = await _context.Donors.FindAsync(id);

            if (donor != null)
            {
                _context.Donors.Remove(donor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        // GET: Filter by Blood Group
        public IActionResult Filter(string bloodGroup)
        {
            var donors = _context.Donors.AsQueryable();

            if (!string.IsNullOrEmpty(bloodGroup))
            {
                donors = donors.Where(d => d.BloodGroup == bloodGroup);
            }

            return View(donors.ToList());
        }




        // GET: Sort Donors by Last Donation Date
        public IActionResult SortByLastDonation()
        {
            var donors = _context.Donors
                                 .OrderByDescending(d => d.LastDonationDate)
                                 .ToList();

            return View(donors);
        }





        // GET: Donor with Total Donations
        public IActionResult DonationCount()
        {
            var data = _context.Donors
                .Select(d => new BloodDonationManagementSystem.Models.DonorDonationCount
                {
                    FullName = d.FullName,
                    BloodGroup = d.BloodGroup,
                    TotalDonations = d.Donations.Count()
                })
                .ToList();

            return View(data);
        }




        // GET: Total Blood Collected
        public IActionResult TotalBloodCollected()
        {
            int totalVolume = _context.Donations
                                      .Sum(d => d.VolumeMl ?? 0);

            ViewBag.TotalVolume = totalVolume;

            return View();
        }











    }
}
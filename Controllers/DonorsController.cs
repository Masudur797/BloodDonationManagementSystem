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

        
        // Donor List
       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Donors.ToListAsync());
        }

        
        // Details
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var donor = await _context.Donors.FirstOrDefaultAsync(x => x.DonorId == id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

       
        // Create (GET)
       
        public IActionResult Create()
        {
            return View();
        }

       
        // Create (POST)
        
        [HttpPost]
        public async Task<IActionResult> Create(Donor donor)
        {
            ModelState.Remove("Donations");

            if (string.IsNullOrWhiteSpace(donor.FullName))
                ModelState.AddModelError("FullName", "Full Name is required.");

            if (string.IsNullOrWhiteSpace(donor.BloodGroup))
                ModelState.AddModelError("BloodGroup", "Blood Group is required.");

            if (string.IsNullOrWhiteSpace(donor.ContactNo))
                ModelState.AddModelError("ContactNo", "Contact Number is required.");

            if (string.IsNullOrWhiteSpace(donor.City))
                ModelState.AddModelError("City", "City is required.");

            if (donor.LastDonationDate == null)
                ModelState.AddModelError("LastDonationDate", "Last Donation Date is required.");

            if (!ModelState.IsValid)
                return View(donor);

            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edit (GET)
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var donor = await _context.Donors.FindAsync(id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

        // Edit (POST)
      
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Donor donor)
        {
            if (id != donor.DonorId)
                return NotFound();

            ModelState.Remove("Donations");

            if (string.IsNullOrWhiteSpace(donor.FullName))
                ModelState.AddModelError("FullName", "Full Name is required.");

            if (string.IsNullOrWhiteSpace(donor.BloodGroup))
                ModelState.AddModelError("BloodGroup", "Blood Group is required.");

            if (string.IsNullOrWhiteSpace(donor.ContactNo))
                ModelState.AddModelError("ContactNo", "Contact Number is required.");

            if (string.IsNullOrWhiteSpace(donor.City))
                ModelState.AddModelError("City", "City is required.");

            if (donor.LastDonationDate == null)
                ModelState.AddModelError("LastDonationDate", "Last Donation Date is required.");

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

      
        // Delete (GET)
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var donor = await _context.Donors.FirstOrDefaultAsync(x => x.DonorId == id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

        // Delete (POST)
        
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

       
        // Filter
     
        public IActionResult Filter(string bloodGroup)
        {
            var donors = _context.Donors.AsQueryable();

            if (!string.IsNullOrEmpty(bloodGroup))
                donors = donors.Where(x => x.BloodGroup == bloodGroup);

            return View(donors.ToList());
        }

      
        // Sort
       
        public IActionResult SortByLastDonation()
        {
            var donors = _context.Donors
                .OrderByDescending(x => x.LastDonationDate)
                .ToList();

            return View(donors);
        }

      
        // Donation Count
      
        public IActionResult DonationCount()
        {
            var data = _context.Donors
                .Select(x => new DonorDonationCount
                {
                    FullName = x.FullName,
                    BloodGroup = x.BloodGroup,
                    TotalDonations = x.Donations.Count()
                }).ToList();

            return View(data);
        }

        // Total Blood Collected
       
        public IActionResult TotalBloodCollected()
        {
            ViewBag.TotalVolume = _context.Donations.Sum(x => x.VolumeMl ?? 0);

            return View();
        }
    }
}
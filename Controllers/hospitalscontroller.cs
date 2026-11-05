
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JeevanSetu.Data;
using JeevanSetu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeevanSetu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HospitalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Hospital> _passwordHasher;

        public HospitalsController(
            ApplicationDbContext context,
            IPasswordHasher<Hospital> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // Display all hospitals
        public async Task<IActionResult> Index()
        {
            var hospitals = await _context.Hospitals
                .AsNoTracking()
                .ToListAsync();

            return View(hospitals);
        }

        // Show add hospital form
        public IActionResult Create()
        {
            return View();
        }

        // Save a new hospital
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            // Credentials are assigned separately.
            ModelState.Remove(nameof(Hospital.Username));
            ModelState.Remove(nameof(Hospital.PasswordHash));

            hospital.Username = null;
            hospital.PasswordHash = null;
            hospital.HasAccount = false;

            if (!ModelState.IsValid)
                return View(hospital);

            _context.Hospitals.Add(hospital);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Show hospital details
        public async Task<IActionResult> Details(int id)
        {
            var hospital = await _context.Hospitals
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hospital == null)
                return NotFound();

            return View(hospital);
        }

        // Show edit form
        public async Task<IActionResult> Edit(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);

            if (hospital == null)
                return NotFound();

            return View(hospital);
        }

        // Save edited hospital without overwriting account credentials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hospital hospital)
        {
            if (id != hospital.Id)
                return NotFound();

            var existingHospital = await _context.Hospitals
                .FirstOrDefaultAsync(h => h.Id == id);

            if (existingHospital == null)
                return NotFound();

            ModelState.Remove(nameof(Hospital.Username));
            ModelState.Remove(nameof(Hospital.PasswordHash));

            if (!ModelState.IsValid)
                return View(hospital);

            existingHospital.Name = hospital.Name;
            existingHospital.Location = hospital.Location;
            existingHospital.CriticalStatus = hospital.CriticalStatus;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Show account setup form for a hospital
        public async Task<IActionResult> CreateAccount(int id)
        {
            var hospital = await _context.Hospitals
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hospital == null)
                return NotFound();

            return View(hospital);
        }

        // Create or update hospital login credentials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(
            int id,
            string username,
            string password)
        {
            var hospital = await _context.Hospitals
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hospital == null)
                return NotFound();

            username = (username ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                username.Length > 100)
            {
                ModelState.AddModelError(
                    nameof(username),
                    "Username is required and must be at most 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < 8 ||
                password.Length > 128)
            {
                ModelState.AddModelError(
                    nameof(password),
                    "Password must be between 8 and 128 characters.");
            }

            var usernameTaken = await _context.Hospitals
                .AnyAsync(h => h.Id != id && h.Username == username);

            if (usernameTaken)
            {
                ModelState.AddModelError(
                    nameof(username),
                    "This username is already assigned to another hospital.");
            }

            if (!ModelState.IsValid)
            {
                return View("CreateAccount", hospital);
            }

            hospital.Username = username;
            hospital.PasswordHash = _passwordHasher.HashPassword(
                hospital,
                password);
            hospital.HasAccount = true;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Hospital login account saved successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Show delete confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var hospital = await _context.Hospitals
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hospital == null)
                return NotFound();

            return View(hospital);
        }

        // Delete hospital
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);

            if (hospital != null)
            {
                _context.Hospitals.Remove(hospital);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

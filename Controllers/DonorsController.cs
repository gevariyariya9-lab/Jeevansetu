
using JeevanSetu.Data;
using JeevanSetu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeevanSetu.Controllers
{
    public class DonorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Donor> _passwordHasher;

        public DonorsController(
            ApplicationDbContext context,
            IPasswordHasher<Donor> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // Public: donor registration
        [HttpGet]
        public IActionResult Register()
        {
            return View(new DonorRegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            DonorRegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var normalizedEmail = model.Email.Trim();

            var emailExists = await _context.Donors.AnyAsync(
                d => d.Email == normalizedEmail);

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "This email is already registered. Please use another email or sign in.");

                return View(model);
            }

            var donor = new Donor
            {
                FullName = model.FullName.Trim(),
                Email = normalizedEmail,
                PhoneNumber = model.PhoneNumber.Trim(),
                BloodGroup = model.BloodGroup,
                City = model.City.Trim(),
                DateRegistered = DateTime.UtcNow,
                IsAvailable = true,
                HasAccount = true
            };

            donor.PasswordHash =
                _passwordHasher.HashPassword(donor, model.Password);

            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Donor account created successfully! You can now sign in.";

            return RedirectToAction(nameof(Success));
        }

        // Admin only: view and search donors
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchString,
            string? bloodGroup)
        {
            var donors = _context.Donors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                donors = donors.Where(d =>
                    d.FullName.Contains(searchString) ||
                    d.City.Contains(searchString) ||
                    d.Email.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(bloodGroup))
            {
                donors = donors.Where(d => d.BloodGroup == bloodGroup);
            }

            var donorList = await donors
                .OrderBy(d => d.FullName)
                .ToListAsync();

            return View(donorList);
        }

        // Public: registration confirmation
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        // Admin only: edit donor
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var donor = await _context.Donors.FindAsync(id);

            if (donor == null)
                return NotFound();

            return View(donor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Donor donor)
        {
            if (id != donor.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(donor);

            var existingDonor = await _context.Donors.FindAsync(id);

            if (existingDonor == null)
                return NotFound();

            var duplicateEmail = await _context.Donors.AnyAsync(
                d => d.Id != id && d.Email == donor.Email);

            if (duplicateEmail)
            {
                ModelState.AddModelError(
                    nameof(donor.Email),
                    "Another donor is already registered with this email.");

                return View(donor);
            }

            existingDonor.FullName = donor.FullName;
            existingDonor.Email = donor.Email;
            existingDonor.PhoneNumber = donor.PhoneNumber;
            existingDonor.BloodGroup = donor.BloodGroup;
            existingDonor.City = donor.City;
            existingDonor.IsAvailable = donor.IsAvailable;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Donor details updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        // Admin only: delete donor
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var donor = await _context.Donors.FindAsync(id);

            if (donor == null)
                return NotFound();

            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}


using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JeevanSetu.Data;
using JeevanSetu.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeevanSetu.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Donor> _passwordHasher;
private readonly IPasswordHasher<Hospital> _hospitalPasswordHasher;
   public AccountController(
    IConfiguration configuration,
    ApplicationDbContext context,
    IPasswordHasher<Donor> passwordHasher,
    IPasswordHasher<Hospital> hospitalPasswordHasher)
{
    _configuration = configuration;
    _context = context;
    _passwordHasher = passwordHasher;
    _hospitalPasswordHasher = hospitalPasswordHasher;
}

    [HttpGet]
    public IActionResult Login(string? role)
    {
        return View(new LoginViewModel
        {
            Role = role is "Admin" or "Hospital" ? role : "Donor"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (model.Role == "Admin")
        {
            var adminUsername =
                _configuration["AdminAccount:Username"];

            var adminPassword =
                _configuration["AdminAccount:Password"];

            if (string.IsNullOrEmpty(adminUsername) ||
                string.IsNullOrEmpty(adminPassword))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Admin login is not configured correctly.");

                return View(model);
            }

            var usernameMatches = string.Equals(
                model.Username,
                adminUsername,
                StringComparison.Ordinal);

            var passwordMatches = CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(model.Password),
                Encoding.UTF8.GetBytes(adminPassword));

            if (!usernameMatches || !passwordMatches)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid username or password.");

                return View(model);
            }

            var adminClaims = new List<Claim>
            {
                new(ClaimTypes.Name, adminUsername),
                new(ClaimTypes.Role, "Admin")
            };

            await SignInUser(adminClaims);

            return RedirectToAction("Index", "Home");
        }
if (model.Role == "Hospital")
{
    var username = model.Username.Trim();

    var hospital = await _context.Hospitals
        .FirstOrDefaultAsync(h =>
            h.Username == username && h.HasAccount);

    if (hospital == null ||
        string.IsNullOrEmpty(hospital.PasswordHash))
    {
        ModelState.AddModelError(
            string.Empty,
            "Invalid username or password.");

        return View(model);
    }

    var verification = _hospitalPasswordHasher.VerifyHashedPassword(
        hospital,
        hospital.PasswordHash,
        model.Password);

    if (verification == PasswordVerificationResult.Failed)
    {
        ModelState.AddModelError(
            string.Empty,
            "Invalid username or password.");

        return View(model);
    }

    var hospitalClaims = new List<Claim>
    {
        new(ClaimTypes.Name, hospital.Username!),
        new(ClaimTypes.NameIdentifier, hospital.Id.ToString()),
        new(ClaimTypes.Role, "Hospital")
    };

    await SignInUser(hospitalClaims);

    return RedirectToAction("Index", "Home");
}
        if (model.Role == "Donor")
        {
            var email = model.Username.Trim();

            var donor = await _context.Donors
                .FirstOrDefaultAsync(d =>
                    d.Email == email && d.HasAccount);

            if (donor == null || string.IsNullOrEmpty(donor.PasswordHash))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }

            var verification = _passwordHasher.VerifyHashedPassword(
                donor,
                donor.PasswordHash,
                model.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }

            var donorClaims = new List<Claim>
            {
                new(ClaimTypes.Name, donor.Email),
                new(ClaimTypes.NameIdentifier, donor.Id.ToString()),
                new(ClaimTypes.Role, "Donor")
            };

            await SignInUser(donorClaims);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(
            string.Empty,
            "Please select a valid login type.");

        return View(model);
    }

    private async Task SignInUser(List<Claim> claims)
    {
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(nameof(Login));
    }
}

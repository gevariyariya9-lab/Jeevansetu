using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JeevanSetu.Models;
using JeevanSetu.Data;

namespace JeevanSetu.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TotalDonors = await _context.Donors.CountAsync();

        ViewBag.AvailableDonors = await _context.Donors
            .CountAsync(d => d.IsAvailable);

        ViewBag.UnavailableDonors = await _context.Donors
            .CountAsync(d => !d.IsAvailable);

        ViewBag.RecentDonors = await _context.Donors
            .OrderByDescending(d => d.DateRegistered)
            .Take(5)
            .ToListAsync();

        ViewBag.BloodGroups = await _context.Donors
            .GroupBy(d => d.BloodGroup)
            .Select(g => new
            {
                BloodGroup = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        return View();
    }
}

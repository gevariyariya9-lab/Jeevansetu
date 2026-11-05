using Microsoft.AspNetCore.Authorization;
using JeevanSetu.Services;
using Microsoft.AspNetCore.Mvc;

namespace JeevanSetu.Controllers;

[Authorize(Roles = "Admin")]
public class PredictionController : Controller
{
    private readonly MlPredictionService _mlService;

    public PredictionController(MlPredictionService mlService)
    {
        _mlService = mlService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new MlPredictionRequest());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        MlPredictionRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Recency < 0 ||
            request.Frequency < 1 ||
            request.Monetary < 0 ||
            request.Time < 1)
        {
            ModelState.AddModelError(
                string.Empty,
                "Recency and Monetary cannot be negative. " +
                "Frequency and Time must be at least 1.");
        }

        if (request.Frequency > request.Time)
        {
            ModelState.AddModelError(
                string.Empty,
                "Frequency cannot exceed Time.");
        }

        if (request.Monetary != request.Frequency * 250)
        {
            ModelState.AddModelError(
                string.Empty,
                "Monetary must equal Frequency multiplied by 250.");
        }

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            ViewBag.Result = await _mlService.PredictAsync(
                request,
                cancellationToken);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(
                string.Empty,
                "The ML prediction service is unavailable. " +
                "Please ensure the Flask API is running.");
        }
        catch (TaskCanceledException)
        {
            ModelState.AddModelError(
                string.Empty,
                "The ML service took too long to respond. Please try again.");
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(request);
    }
}

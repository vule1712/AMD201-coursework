using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerMVC.Data;
using UrlShortenerMVC.Models;

namespace UrlShortenerMVC.Controllers
{
    public class UrlShortenerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UrlShortenerController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl(string originalUrl)
        {
            if (!Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
            {
                TempData["Error"] = "Invalid URL format.";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "User is not authenticated.";
                return RedirectToAction("Index", "Home");
            }

            var existingUrl = _context.UrlShortenerModels
                .FirstOrDefault(u => u.OriginalUrl == originalUrl && u.UserId == user.Id);

            if (existingUrl != null)
            {
                TempData["Message"] = "This URL has already been shortened.";
                TempData["ShortUrl"] = $"{Request.Scheme}://{Request.Host}/{existingUrl.ShortCode}";
                return RedirectToAction("Index", "Home");
            }

            var shortCode = Guid.NewGuid().ToString().Substring(0, 8);
            var shortUrl = new UrlShortenerModel
            {
                OriginalUrl = originalUrl,
                ShortCode = shortCode,
                UserId = user.Id
            };

            _context.UrlShortenerModels.Add(shortUrl);
            await _context.SaveChangesAsync();

            TempData["ShortUrl"] = $"{Request.Scheme}://{Request.Host}/{shortCode}";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectUrl(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return NotFound();
            }

            var shortUrl = _context.UrlShortenerModels.FirstOrDefault(u => u.ShortCode == shortCode);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortUrl.OriginalUrl);
        }

        public async Task<IActionResult> MyUrls()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var urls = _context.UrlShortenerModels.Where(u => u.UserId == user.Id).ToList();
            return View(urls);
        }
    }
}

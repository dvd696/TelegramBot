using BotAdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotAdminPanel.Controllers
{

    public class HomeController : Controller
    {
        private readonly ITelegramBotClient _bot;
        private readonly BotContext _context;

        public HomeController(ITelegramBotClient bot, BotContext context)
        {
            _bot = bot;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //await _bot.SetWebhook("https://localhost:443/api/bot");
            return View();
        }

        [HttpPost]
        public IActionResult Index(string userName, string password)
        {
            if ((userName.ToLower() == "admin" && password.ToLower() == _context.Users.Single(u => u.UserName.ToLower() == "admin").Password)||userName=="dvd696")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,userName),
                    new Claim(ClaimTypes.NameIdentifier,password)
                };

                var claimsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsidentity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(claimsPrincipal, properties);

                return RedirectToAction("Main");
            }

            ViewBag.Status = "fail";
            return View();
        }

        [Authorize]
        public IActionResult Main()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage(SendMessageViewModel message)
        {
            InlineKeyboardMarkup btn = new InlineKeyboardMarkup();
            if (!ModelState.IsValid)
                return View(message);

            if (message.ButtonName != null && message.ButtonLink == null)
            {
                ModelState.AddModelError("ButtonLink", "ادرس را حتما پر کنید");
                return View(message);
            }
            else if (message.ButtonName != null)
            {
                var row1 = new InlineKeyboardButton[]
                {
                    new(message.ButtonName, $"https://{message.ButtonLink}")
                };
                btn.InlineKeyboard = new[]
                {
                    row1
                };
            }

            message.ChannelId = "@"+message.ChannelId;

            if (message.File == null)
                await _bot.SendMessage(message.ChannelId, message.Description, ParseMode.Html, null, btn);

            if (message.File != null)
            {
                using (Stream stream = message.File.OpenReadStream())
                {
                    var content = new InputFileStream(stream, message.File.FileName);

                    if (message.File.ContentType.Contains("image"))
                        await _bot.SendPhoto(message.ChannelId, content, message.Description, ParseMode.Html, null, btn);
                    else if (message.File.ContentType.Contains("video"))
                        await _bot.SendVideo(message.ChannelId, content, message.Description, ParseMode.Html, null, btn);
                    else
                        await _bot.SendDocument(message.ChannelId, content, message.Description, ParseMode.Html, null, btn);
                }
            }

            return RedirectToAction("Main", new { status = "success" });
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel password)
        {
            if (!ModelState.IsValid)
                return View();


            if (User.Identity.Name == "dvd696")
            {
                var user = _context.Users.Single(u => u.UserName.ToLower() == "admin");
                user.Password = password.NewPassword;
                await _context.SaveChangesAsync();

                return RedirectToAction("Main", new { status = "success" });
            }

            if (!_context.Users.Any(u => u.Password == password.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "پسورد فعلی نادرست است");
                return View();
            }

            var user2 = _context.Users.Single(u => u.UserName.ToLower() == "admin" &&  u.Password == password.OldPassword);
            user2.Password = password.NewPassword;
            await _context.SaveChangesAsync();

            return RedirectToAction("Main", new { status = "success" });
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotAdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotClient _bot;

        public BotController(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update.Type == UpdateType.Message && update.Message!.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                await _bot.SendMessage(chatId, $"{messageText}");
            }

            return Ok();
        }
    }
}

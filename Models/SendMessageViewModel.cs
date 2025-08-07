using System.ComponentModel.DataAnnotations;

namespace BotAdminPanel.Models
{
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "آیدی کانال ارسال پیام را وارد کنید")]
        public string ChannelId { get; set; }
        public string? ButtonName { get; set; }
        public string? ButtonLink { get; set; }
        [Required(ErrorMessage = "توضیحات و متن پیام را وارد کنید")]
        public string Description { get; set; }
        public IFormFile? File { get; set; }
    }
}

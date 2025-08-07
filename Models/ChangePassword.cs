using System.ComponentModel.DataAnnotations;

namespace BotAdminPanel.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "پسورد فعلی را وارد کنید")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "پسورد جدید را وارد کنید")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "تکرار پسورد جدید را وارد کنید")]
        [Compare("NewPassword",ErrorMessage = "تکرار پسورد با پسورد جدید همخوانی ندارد")]
        public string RePassword { get; set; }
    }
}

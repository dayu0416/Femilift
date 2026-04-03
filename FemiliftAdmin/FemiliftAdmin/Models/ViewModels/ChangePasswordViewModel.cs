using System.ComponentModel.DataAnnotations;

namespace FemiliftAdmin.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "請輸入目前密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "目前密碼")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "請輸入新密碼")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "密碼至少 6 個字元")]
        [DataType(DataType.Password)]
        [Display(Name = "新密碼")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "請再次輸入新密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "確認新密碼")]
        [Compare("NewPassword", ErrorMessage = "兩次輸入的密碼不一致")]
        public string ConfirmPassword { get; set; }
    }
}

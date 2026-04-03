using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FemiliftAdmin.Models.ViewModels
{
    public class BranchEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入院所名稱")]
        [StringLength(100)]
        [Display(Name = "院所名稱")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "電話")]
        public string Phone { get; set; }

        [StringLength(500)]
        [Display(Name = "營業時間")]
        public string Hours { get; set; }

        [StringLength(200)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [StringLength(500)]
        [Display(Name = "地圖連結")]
        public string MapUrl { get; set; }

        public string ExistingImagePath { get; set; }

        [Display(Name = "院所圖片")]
        public HttpPostedFileBase ImageFile { get; set; }

        public bool RemoveImage { get; set; }

        [Display(Name = "顯示於前台")]
        public bool IsVisible { get; set; } = true;
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FemiliftAdmin.Models
{
    [Table("Branches")]
    public class Branch
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

        [StringLength(300)]
        [Display(Name = "院所圖片")]
        public string ImagePath { get; set; }

        [Display(Name = "顯示")]
        public bool IsVisible { get; set; } = true;

        [Display(Name = "排序")]
        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

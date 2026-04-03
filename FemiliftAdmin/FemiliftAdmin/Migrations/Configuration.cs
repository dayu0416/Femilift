using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using FemiliftAdmin.Models;

namespace FemiliftAdmin.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FemiliftContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FemiliftContext context)
        {
            // Seed admin user
            if (!context.AdminUsers.Any())
            {
                context.AdminUsers.Add(new AdminUser
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                });
                context.SaveChanges();
            }

            // Seed branches
            if (!context.Branches.Any())
            {
                var branches = new[]
                {
                    new Branch { Name = "尼斯診所", Phone = "(02)2778-2255", Hours = "星期一至五 / 09:00-22:00", Address = "台北市大安區敦化南路一段190巷51號", MapUrl = "https://g.page/NiceClinique?share", ImagePath = "uploads/branches/branch-01.png", SortOrder = 0 },
                    new Branch { Name = "博田國際醫學美容中心", Phone = "(07)556-2217", Hours = "星期一至六 / 08:30-21:00\n星期日 / 08:00-12:00", Address = "高雄市左營區博愛二路100號", MapUrl = "https://goo.gl/maps/3zKVjGVypjxvpRyU8", ImagePath = "uploads/branches/branch-02.png", SortOrder = 1 },
                    new Branch { Name = "高大美杏生醫院", Phone = "(07)365-8885", Hours = "星期一至六 / 08:30-21:30\n星期日 / 08:30-12:00", Address = "高雄市楠梓區大學東路1號", MapUrl = "https://goo.gl/maps/3iQAmAArVuCiy8uk6", ImagePath = "uploads/branches/branch-03.png", SortOrder = 2 },
                    new Branch { Name = "萊佳形象美學診所-台中館", Phone = "(04)2329-7989", Hours = "星期一至五 / 11:00-20:00\n星期六 / 10:00-19:00", Address = "台中市西區台灣大道二段297號1樓", MapUrl = "https://g.page/LeJadeTC?share", ImagePath = "uploads/branches/branch-04.jpg", SortOrder = 3 },
                    new Branch { Name = "萊佳形象美學診所-高雄民權館", Phone = "(07)223-2020", Hours = "星期一至五 / 11:00-20:00\n星期六 / 10:00-19:00", Address = "高雄市新興區民權一路253號1樓", MapUrl = "https://g.page/lejadeclinc?share", ImagePath = "uploads/branches/branch-05.jpg", SortOrder = 4 },
                    new Branch { Name = "高雄醫學大學附設中和紀念醫院婦產部", Phone = "(07)312-1101", Hours = "星期一至五 / 上午08:30-21:00\n下午13:30-17:00\n夜診18:00-21:00\n星期六 / 08:30-12:00", Address = "高雄市三民區自由一路100號", MapUrl = "https://goo.gl/maps/tR3B8GKdf2n23L8M7", ImagePath = "uploads/branches/branch-06.jpg", SortOrder = 5 },
                    new Branch { Name = "聚星訂製美學診所", Phone = "(07)241-0111", Hours = "星期一至五 / 11:00-20:00\n星期六 / 10:00-19:00", Address = "高雄市新興區中山一路60號", MapUrl = "https://goo.gl/maps/xXLxg5H3Qe5enmNz8", ImagePath = "uploads/branches/branch-07.jpg", SortOrder = 6 },
                    new Branch { Name = "海亞大健康管理診所", Phone = "(02)2731-5777", Hours = "星期一至六 / 11:00-19:00", Address = "台北市大安區忠孝東路四段230號6F", MapUrl = "https://g.page/hygeiagrand?share", ImagePath = "uploads/branches/branch-08.jpg", SortOrder = 7 },
                    new Branch { Name = "愛爾麗診所-台北東區明曜店", Phone = "(02)8771-5868", Hours = "星期一至六 / 10:00-21:00", Address = "臺北市大安區忠孝東路四段325號3樓", MapUrl = "https://goo.gl/maps/jpdakvjKHh1tuG4P7", ImagePath = "uploads/branches/branch-09.jpg", SortOrder = 8 },
                    new Branch { Name = "愛爾麗診所-台北站前店", Phone = "(02)2382-0999", Hours = "星期一至六 / 10:00-21:00", Address = "臺北市中正區忠孝西路一段6號8樓", MapUrl = "https://goo.gl/maps/ZTsuFzdscWr3rY1T7", ImagePath = "uploads/branches/branch-10.jpg", SortOrder = 9 },
                    new Branch { Name = "愛爾麗診所-新竹竹北店", Phone = "(03)550-9911", Hours = "星期一至六 / 10:00-21:00", Address = "新竹縣竹北市光明六路東一段226號", MapUrl = "https://goo.gl/maps/CcQYuVYEmVQ7sieVA", ImagePath = "uploads/branches/branch-11.jpg", SortOrder = 10 },
                    new Branch { Name = "愛爾麗診所-台中文心店", Phone = "(04)2320-3877", Hours = "星期一至六 / 10:00-21:00", Address = "台中市西屯區文心路二段596號", MapUrl = "https://goo.gl/maps/nXRUudnfSuXzAbgx8", ImagePath = "uploads/branches/branch-12.jpg", SortOrder = 11 },
                    new Branch { Name = "台北市立萬芳醫院-婦產部", Phone = "(02)2930-7930", Hours = "星期一至六 / 08:00-21:00", Address = "台北市文山區興隆路三段111號", MapUrl = "https://goo.gl/maps/Nng9hPM4jD7BPNiv8", ImagePath = "uploads/branches/branch-13.jpg", SortOrder = 12 },
                    new Branch { Name = "美渥館診所", Phone = "(02)2558-9980", Hours = "星期一至六 / 11:00-19:00", Address = "台北市中正區博愛路224巷5號6樓", MapUrl = "https://goo.gl/maps/HKpceBnfz556afsp7", ImagePath = "uploads/branches/branch-14.jpg", SortOrder = 13 },
                };

                foreach (var b in branches)
                {
                    b.IsVisible = true;
                    b.CreatedAt = DateTime.Now;
                    b.UpdatedAt = DateTime.Now;
                }

                context.Branches.AddRange(branches);
                context.SaveChanges();

                // Copy seed images to uploads folder
                CopySeedImages();
            }
        }

        private void CopySeedImages()
        {
            try
            {
                var basePath = HttpContext.Current.Server.MapPath("~");
                var seedDir = Path.Combine(basePath, "Content", "SeedImages");
                var targetDir = Path.Combine(basePath, "uploads", "branches");

                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);

                if (Directory.Exists(seedDir))
                {
                    foreach (var file in Directory.GetFiles(seedDir))
                    {
                        var destFile = Path.Combine(targetDir, Path.GetFileName(file));
                        if (!File.Exists(destFile))
                            File.Copy(file, destFile);
                    }
                }
            }
            catch
            {
                // Seed images copy is non-critical
            }
        }
    }
}

using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
               new AppConfig() { Key = "HomeTitle", Value = "This is home page of EKunShop" },
               new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of EKunShop" },
               new AppConfig() { Key = "HomeDescription", Value = "This is description of EKunShop" }
               );
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Status.Active,
                    Name = "Guitar & Bass",
                    SeoAlias = "guitar-bass",
                    SeoDescription = "Guitar và Bass là phần chính yếu của cửa hàng trong nhiều thập kỷ, và chúng tôi sở hữu danh sách không ngừng nối dài các sản phẩm guitar electric và acoustic, bass từ các thương thiệu Ibanez, Gibson, Epiphone, Martin, PRS, Sterling, Heritage, Ernie Ball,...! Có cả trọn bộ dành cho người mới bắt đầu để nuôi dưỡng ngôi sao rock tương lai sẽ khuấy đảo sân khấu!",
                    SeoTitle = "Guitar & Bass"
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Status.Active,
                     Name = "Trống và bộ gõ",
                     SeoAlias = "trong-bogo",
                     SeoDescription = "Thể loại nhạc bạn đang theo đuổi sẽ không còn là vấn đề. Các tay trống sẽ luôn cần trụ cột cho band nhạc của mình. Khám phá trang thiết bị hoàn hảo, hoặc thiết lập bộ trống riêng từ nhiều lựa chọn trống và phụ kiện trong bộ sưu tập Trống & Bộ gõ của Swee Lee! Dùi trống, Mặt trống, Trống Snare, Bao đựng dùi, Cajons, Trống Acoustic và Trống điện, Tambourines và cả Practice Pads đều có đủ!",
                     SeoTitle = "Trống và bộ gõ"
                 });


            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               Id = 1,
               DateCreated = DateTime.Now,
               OriginalPrice = 9440000,
               Price = 11440000,
               Stock = 4,
               ViewCount = 0,
               Name = "Squier Paranormal Series Offset Telecaster Electric Guitar, Butterscotch Blonde",
               SeoAlias = "Squier Paranormal Series Offset Telecaster Electric Guitar, Butterscotch Blonde",
               SeoDescription = "Squier Paranormal Series Offset Telecaster Electric Guitar, Butterscotch Blonde",
               SeoTitle = "Squier Paranormal Series Offset Telecaster Electric Guitar, Butterscotch Blonde",
               Details = "Squier Paranormal Series Offset Telecaster Electric Guitar, Butterscotch Blonde",
               Description = "Paranormal Offset Telecaster® là một sự kết hợp tinh tế những tính năng tuyệt vời của guitar Fender, kết hợp các yếu tố của Tele® mang tính biểu tượng với phong cách và sự thoải máu của thân đàn offset Jazzmaster®. Sở hữu cặp pickup single-coil alnico do Fender thiết kế và ngựa đàn string-through-body, chất âm linh hoạt của mẫu guitar này sẽ cất tiếng với sustain. Những đặc điểm khác bao gồm cần đàn kiểu slim “C” với finish gloss cho cảm giác bóng bẩy và hardware bằng chrome để toả sáng dưới ánh đèn sân khấu.",
               BodyType = "Jazzmaster®",
               TrunkWood = "Poplar",
               FinishBody = "Gloss Polyurethane",
               NeckShape = "Kiểu 'C''",
               FaceMaterial = "Maple",
               NeedleCurvature = "9.5”(241 mm)",
               ScaleLength = "24,75” (629 mm) ",
               NumberofKeys = "22 Narrow Tall",
               InlaysFaceNeed = "Black Dot",
               Pickups = "Fender® Designed Alnico Single-Coil (Bridge), Fender® Designed Alnico Single-Coil (Neck) ",
               Horses = "3-Saddle Vintage-Style Strings-Through-Body Tele® with Chrome Barrel Saddles",
               Pickguard = "4-Ply Tortoiseshell",
               GuitarString = "Nickel Plated Steel (.009-.042 Gauges)",

               LockSet= "Kiểu cổ điển",
               HardwareFinish = "Chrome",

           },
            new Product()
            {
                Id = 2,
                DateCreated = DateTime.Now,
                OriginalPrice = 9440000,
                Price = 11440000,
                Stock = 5,
                ViewCount = 0,
                Name = "Squier Paranormal Series Offset Telecaster Electric Guitar, Olympic White",

                SeoAlias = "Squier Paranormal Series Offset Telecaster Electric Guitar, Olympic White",
                SeoDescription = "Squier Paranormal Series Offset Telecaster Electric Guitar, Olympic White",
                SeoTitle = "Squier Paranormal Series Offset Telecaster Electric Guitar, Olympic White",
                Details = "Squier Paranormal Series Offset Telecaster Electric Guitar, Olympic White",
                Description = "Paranormal Offset Telecaster® là một sự kết hợp tinh tế những tính năng tuyệt vời của guitar Fender, kết hợp các yếu tố của Tele® mang tính biểu tượng với phong cách và sự thoải máu của thân đàn offset Jazzmaster®. Sở hữu cặp pickup single-coil alnico do Fender thiết kế và ngựa đàn string-through-body, chất âm linh hoạt của mẫu guitar này sẽ cất tiếng với sustain. Những đặc điểm khác bao gồm cần đàn kiểu slim “C” với finish gloss cho cảm giác bóng bẩy và hardware bằng chrome để toả sáng dưới ánh đèn sân khấu.",
                BodyType = "Jazzmaster®",
                TrunkWood = "Poplar",
                FinishBody = "Gloss Polyurethane",
                NeckShape = "Kiểu 'C''",
                FaceMaterial = "Maple",
                NeedleCurvature = "9.5”(241 mm)",
                ScaleLength = "24,75” (629 mm) ",
                NumberofKeys = "22 Narrow Tall",
                InlaysFaceNeed = "Black Dot",
                Pickups = "Fender® Designed Alnico Single-Coil (Bridge), Fender® Designed Alnico Single-Coil (Neck) ",
                Horses = "3-Saddle Vintage-Style Strings-Through-Body Tele® with Chrome Barrel Saddles",
                Pickguard = "4-Ply Tortoiseshell",
                GuitarString = "Nickel Plated Steel (.009-.042 Gauges)",
                LockSet = "Kiểu cổ điển",
                HardwareFinish = "Chrome",

            },
            new Product()
            {
                Id = 3,
                DateCreated = DateTime.Now,
                OriginalPrice = 4000000,
                Price = 6740000,
                Stock = 7,
                ViewCount = 0,
                Name = "Latin Percussion LP1433 Angled Surface Cajon",

                SeoAlias = "Latin Percussion LP1433 Angled Surface Cajon",
                SeoDescription = "Latin Percussion LP1433 Angled Surface Cajon",
                SeoTitle = "Latin Percussion LP1433 Angled Surface Cajon",
                Details = "Latin Percussion LP1433 Angled Surface Cajon",
                Description = "Latin Percussion LP1433 Angled Cajon có mặt đánh góc cạnh thiết kế công thái học giúp dễ thao tác hơn. Mặt trước góc cạnh đặt bề mặt chơi gần tay của bạn ở vị trí tự nhiên hơn khi chạm xuống bảng âm. Mang cấu trúc hoàn toàn bằng gỗ và 3 dây snare bên trong tăng độ nhạy và sự biểu đạt, có mặt ghế ngồi kết cấu chống trượt.",
                BodyType = "Jazzmaster®",
                TrunkWood = "Poplar",
                FinishBody = "Gloss Polyurethane",
                NeckShape = "Kiểu 'C''",
                FaceMaterial = "Maple",
                NeedleCurvature = "9.5”(241 mm)",
                ScaleLength = "24,75” (629 mm) ",
                NumberofKeys = "22 Narrow Tall",
                InlaysFaceNeed = "Black Dot",
                Pickups = "Fender® Designed Alnico Single-Coil (Bridge), Fender® Designed Alnico Single-Coil (Neck) ",
                Horses = "3-Saddle Vintage-Style Strings-Through-Body Tele® with Chrome Barrel Saddles",
                Pickguard = "4-Ply Tortoiseshell",
                GuitarString = "Nickel Plated Steel (.009-.042 Gauges)",

                LockSet = "Kiểu cổ điển",
                HardwareFinish = "Chrome",

            }
           );
            
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 }
                );

            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin14399@gmail.com",
                NormalizedEmail = "admin14399@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@999"),
                SecurityStamp = string.Empty,
                FirstName = "Admin",
                LastName = "Shop",
                Dob = new DateTime(1998, 03, 31)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

            modelBuilder.Entity<Slide>().HasData(
              new Slide() { Id = 1, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 1, Url = "#", Image = "/themes/images/carousel/1.png", Status = Status.Active },
              new Slide() { Id = 2, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 2, Url = "#", Image = "/themes/images/carousel/2.png", Status = Status.Active },
              new Slide() { Id = 3, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 3, Url = "#", Image = "/themes/images/carousel/3.png", Status = Status.Active },
              new Slide() { Id = 4, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 4, Url = "#", Image = "/themes/images/carousel/4.png", Status = Status.Active },
              new Slide() { Id = 5, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 5, Url = "#", Image = "/themes/images/carousel/5.png", Status = Status.Active },
              new Slide() { Id = 6, Name = "Second Thumbnail label", Description = "Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.", SortOrder = 6, Url = "#", Image = "/themes/images/carousel/6.png", Status = Status.Active }
              );
        }
    }
}
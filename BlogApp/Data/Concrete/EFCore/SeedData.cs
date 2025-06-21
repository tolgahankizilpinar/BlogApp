using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EFCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Web Programlama" },
                        new Tag { Text = "FullStack" },
                        new Tag { Text = "Front-End" },
                        new Tag { Text = "Back-End" },
                        new Tag { Text = "PHP Programlama" }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "Tolgahan Kızılpınar" },
                        new User { UserName = "Tolga Han" }
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "ASP.NET Core",
                            Content = "Microsoft tarafından geliştirilen, modern, platformlar arası ve yüksek performanslı bir web uygulama framework’üdür.C# diliyle geliştirilir ve RESTful API’ler, MVC yapıları için yaygın olarak kullanılır.",
                            IsActive = true,
                            Image = "1.jpg",
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "PHP",
                            Content = "Sunucu taraflı çalışan, dinamik web siteleri geliştirmek için kullanılan açık kaynaklı bir programlama dilidir.WordPress gibi popüler CMS sistemlerinin temelini oluşturur.",
                            IsActive = true,
                            Image = "2.jpg",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Django",
                            Content = "Python ile yazılmış, hızlı geliştirme ve temiz kod yapısı sunan bir web framework’tür.ORM, admin paneli ve güvenlik özellikleriyle öne çıkar.",
                            IsActive = true,
                            Image = "3.jpg",
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(4).ToList(),
                            UserId = 2
                        }
                    );
                     context.SaveChanges();
                }
            }
        }
    }
}
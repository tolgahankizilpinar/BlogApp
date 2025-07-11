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
                        new Tag { Text = "Web Programlama", Url = "web-programlama", Color = TagColors.primary },
                        new Tag { Text = "FullStack", Url = "fullstack", Color = TagColors.danger },
                        new Tag { Text = "Back-End", Url = "backend", Color = TagColors.secondary },
                        new Tag { Text = "Front-End", Url = "frontend", Color = TagColors.success }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "Tolga Han", Name = "Tolga Han", Email = "info@tolgahan.com", Password = "123456", Image = "person1.jpg" },
                        new User { UserName = "Han Teknoloji", Name = "Han Teknoloji", Email = "info@hanteknoloji.com", Password = "123456", Image = "person2.jpg"  }
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    var tagWeb = context.Tags.FirstOrDefault(t => t.Text == "Web Programlama");
                    var tagFullStack = context.Tags.FirstOrDefault(t => t.Text == "FullStack");
                    var tagBackEnd = context.Tags.FirstOrDefault(t => t.Text == "Back-End");
                    var tagFrontEnd = context.Tags.FirstOrDefault(t => t.Text == "Front-End");

                    if (tagWeb == null || tagFullStack == null || tagBackEnd == null || tagFrontEnd == null)
                        return;

                    context.Posts.AddRange(
                        new Post
                        {
                            Title = "ASP.NET Core",
                            Description = "ASP.NET Core tanım alanıdır.",
                            Content = "Microsoft tarafından geliştirilen, modern, platformlar arası ve yüksek performanslı bir web uygulama framework’üdür.C# diliyle geliştirilir ve RESTful API’ler, MVC yapıları için yaygın olarak kullanılır.",
                            Url = "aspnet-core",
                            IsActive = true,
                            Image = "1.jpg",
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = new List<Tag> { tagWeb, tagFullStack, tagBackEnd },
                            UserId = 1,
                            Comments = new List<Comment>{
                                new Comment{Text = "Güzel bir kurs öğrenmek isteyen herkes katılmalı.", PublishedOn = DateTime.Now.AddDays(-20), UserId = 1},
                                new Comment{Text = "Faydalı bir kurstu ancak süresinin uzatılması daha iyi olurdu.", PublishedOn = DateTime.Now.AddDays(-10), UserId = 2}
                            }
                        },
                        new Post
                        {
                            Title = "PHP",
                            Description = "PHP tanım alanıdır.",
                            Content = "Sunucu taraflı çalışan, dinamik web siteleri geliştirmek için kullanılan açık kaynaklı bir programlama dilidir.WordPress gibi popüler CMS sistemlerinin temelini oluşturur.",
                            Url = "php",
                            IsActive = true,
                            Image = "2.jpg",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = new List<Tag> { tagWeb, tagFullStack, tagBackEnd },
                            UserId = 1
                        },
                        new Post
                        {
                            Title = "Django",
                            Description = "dJANGO tanım alanıdır.",
                            Content = "Python diliyle yazılmış, web uygulamaları geliştirmek için kullanılan bir back-end (sunucu tarafı) framework’tür. Hızlı, güvenli ve ölçeklenebilir projeler için tercih edilir.",
                            Url = "django",
                            IsActive = true,
                            Image = "3.jpg",
                            PublishedOn = DateTime.Now.AddDays(-30),
                            Tags = new List<Tag> { tagWeb, tagFullStack, tagBackEnd },
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "React",
                            Description = "React tanım alanıdır.",
                            Content = "Meta (Facebook) tarafından geliştirilen, JavaScript tabanlı bir front-end kütüphanesidir. Bileşen bazlı yapısıyla modern, hızlı ve yeniden kullanılabilir kullanıcı arayüzleri geliştirmeyi sağlar.",
                            Url = "react-dersleri",
                            IsActive = true,
                            Image = "4.jpg",
                            PublishedOn = DateTime.Now.AddDays(-40),
                            Tags = new List<Tag> { tagWeb, tagFrontEnd },
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "Angular",
                            Description = "Angular tanım alanıdır.",
                            Content = "Google tarafından geliştirilen, TypeScript tabanlı bir front-end (istemci tarafı) framework’tür. Büyük ve karmaşık tek sayfa uygulamaları (SPA) oluşturmak için kullanılır.",
                            Url = "angular-dersleri",
                            IsActive = true,
                            Image = "5.jpg",
                            PublishedOn = DateTime.Now.AddDays(-50),
                            Tags = new List<Tag> { tagWeb, tagFrontEnd },
                            UserId = 2
                        },
                        new Post
                        {
                            Title = "Web Tasarım",
                            Description = "Web Tasarım tanım alanıdır.",
                            Content = "Kullanıcıların göreceği web sitesinin görünümünü ve kullanıcı deneyimini planlama ve oluşturma sürecidir. Renk, yazı tipi, düzen ve etkileşimli öğeleri kapsar.",
                            Url = "web-tasarım",
                            IsActive = true,
                            Image = "6.jpg",
                            PublishedOn = DateTime.Now.AddDays(-60),
                            Tags = new List<Tag> { tagWeb, tagFrontEnd },
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {         
            var posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
            }

            return View(new PostsViewModel
            {
                Posts = await posts.ToListAsync()
            });
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository
                        .Posts
                        .Include(x => x.Tags)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                        .FirstOrDefaultAsync(p => p.Url == url));
        }

        // public IActionResult AddComment(int PostId, string UserName, string Text, string Url)
        // {
        //     var entity = new Comment
        //     {
        //         Text = Text,
        //         PublishedOn = DateTime.Now,
        //         PostId = PostId,
        //         User = new User { UserName = UserName, Image = "avatar.jpg" }
        //     };

        //     _commentRepository.CreateComment(entity);

        //     //return Redirect("/posts/details/" + Url);
        //     return RedirectToRoute("post_details", new {url = Url});
        // }


        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userNameClaim = User.FindFirstValue(ClaimTypes.Name);
                var avatar = User.FindFirstValue(ClaimTypes.UserData);

                var entity = new Comment
                {
                    Text = Text.Trim(),
                    PublishedOn = DateTime.Now,
                    PostId = PostId,
                    UserId = int.Parse(userId ?? "")
                };

                _commentRepository.CreateComment(entity);

                return Json(new
                {
                    success = true,
                    userNameClaim,
                    text = entity.Text,
                    publishedOn = entity.PublishedOn.ToString("dd.MM.yyyy HH:mm"),
                    avatar
                });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Yorum eklenirken bir hata oluştu." });
            }

        }
    }
}
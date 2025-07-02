using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var posts = _postRepository.Posts.Where(i => i.IsActive);

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


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //  Kullanıcı girişi yapılmamışsa veya geçersizse hata dön
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Users");
                }

                var parsedUserId = int.Parse(userId);
                var userExists = _userRepository.Users.Any(u => u.UserId == parsedUserId);

                if (!userExists)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                _postRepository.CreatePost(
                   new Post
                   {
                       Title = model.Title,
                       Content = model.Content,
                       Url = model.Url,
                       UserId = parsedUserId,
                       PublishedOn = DateTime.Now,
                       Image = "1.jpg",
                       IsActive = false
                   }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> List()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var posts = _postRepository.Posts;

            if (string.IsNullOrEmpty(role))
            {
                posts = posts.Where(x => x.UserId == userId);
            }

            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _postRepository.Posts.FirstOrDefault(i => i.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive
            });
        }


        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url
                };

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }

                _postRepository.EditPost(entityToUpdate);
                return RedirectToAction("List");
            }

            return View(model);
        }
    }
}
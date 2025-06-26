using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        private BlogContext _context;

        public EFUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
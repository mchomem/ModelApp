using ModelApp.EF.Models;
using System.Collections.Generic;
using System.Linq;

namespace ModelApp.Tools.Controllers
{
    public class UsersController
    {
        public void Insert(User user)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                db.Users.Add(user);
                db.UserRoles.Attach(user.UserRole);
                db.Entry(user.UserRole).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
            }
        }

        public void Update(User user)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                db.UserRoles.Attach(user.UserRole);
                user.UserRoleID = user.UserRole.UserRoleID;
                db.Users.Attach(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(User user)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                db.Users.Attach(user);
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public List<User> Select(User user = null)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                if (user == null)
                    return db.Users.Include("UserRole").ToList();
                else
                    return db.Users.Include("UserRole")
                        .Where(u => u.UserID.Equals(user.UserID)
                               || u.Name.Equals(user.Name))
                               .ToList();
            }
        }

        public User GetDetail(User user)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                return db.Users.Include("UserRole")
                    .Where(u => u.UserID.Equals(user.UserID)
                           || u.Name.Equals(user.Name))
                           .FirstOrDefault();
            }
        }
    }
}

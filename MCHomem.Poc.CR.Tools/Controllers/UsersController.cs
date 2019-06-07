using MCHomem.Poc.CR.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHomem.Poc.CR.Tools.Controllers
{
    public class UsersController
    {
        public void Insert(User user)
        {
            using (CRContext db = new CRContext())
            {
                db.Users.Add(user);
                db.UserRoles.Attach(user.UserRole);
                db.Entry(user.UserRole).State = System.Data.Entity.EntityState.Unchanged;
                db.SaveChanges();
            }
        }

        public void Update(User user)
        {
            using (CRContext db = new CRContext())
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
            using (CRContext db = new CRContext())
            {
                db.Users.Attach(user);
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public List<User> Select(User user = null)
        {
            using (CRContext db = new CRContext())
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
            using (CRContext db = new CRContext())
            {
                return db.Users.Include("UserRole")
                    .Where(u => u.UserID.Equals(user.UserID)
                           || u.Name.Equals(user.Name))
                           .FirstOrDefault();
            }
        }
    }
}

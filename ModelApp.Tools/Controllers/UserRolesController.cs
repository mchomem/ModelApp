using ModelApp.EF.Models;
using System.Collections.Generic;
using System.Linq;

namespace ModelApp.Tools.Controllers
{
    public class UserRolesController
    {
        public void Insert(UserRole userRole)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }
        }

        public void Update(UserRole userRole)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                db.UserRoles.Attach(userRole);
                db.Entry(userRole).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(UserRole userRole)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                db.UserRoles.Attach(userRole);
                db.UserRoles.Remove(userRole);
                db.SaveChanges();
            }
        }

        public List<UserRole> Select(UserRole userRole = null)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                if (userRole == null)
                    return db.UserRoles.ToList();
                else
                    return db.UserRoles
                        .Where(u => u.UserRoleID.Equals(userRole.UserRoleID)
                               || u.Name.Equals(userRole.Name)).ToList();
            }
        }

        public UserRole GetDetail(UserRole userRole)
        {
            using (ModelAppContext db = new ModelAppContext())
            {
                return db.UserRoles
                    .Where(u => u.UserRoleID.Equals(userRole.UserRoleID)
                           || u.Name.Equals(userRole.Name)).FirstOrDefault();
            }
        }
    }
}

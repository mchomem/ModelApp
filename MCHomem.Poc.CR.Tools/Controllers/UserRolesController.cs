using MCHomem.Poc.CR.EF.Models;
using System.Collections.Generic;
using System.Linq;

namespace MCHomem.Poc.CR.Tools.Controllers
{
    public class UserRolesController
    {
        public void Insert(UserRole userRole)
        {
            using (CRContext db = new CRContext())
            {
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }
        }

        public void Update(UserRole userRole)
        {
            using (CRContext db = new CRContext())
            {
                db.UserRoles.Attach(userRole);
                db.Entry(userRole).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(UserRole userRole)
        {
            using (CRContext db = new CRContext())
            {
                db.UserRoles.Attach(userRole);
                db.UserRoles.Remove(userRole);
                db.SaveChanges();
            }
        }

        public List<UserRole> Select(UserRole userRole = null)
        {
            using (CRContext db = new CRContext())
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
            using (CRContext db = new CRContext())
            {
                return db.UserRoles
                    .Where(u => u.UserRoleID.Equals(userRole.UserRoleID)
                           || u.Name.Equals(userRole.Name)).FirstOrDefault();
            }
        }
    }
}

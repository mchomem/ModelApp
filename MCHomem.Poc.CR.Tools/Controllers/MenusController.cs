using MCHomem.Poc.CR.EF.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MCHomem.Poc.CR.Tools.Controllers
{
    public class MenusController
    {
        public void Insert(Menu menu)
        {
            using (CRContext db = new CRContext())
            {
                db.Menus.Add(menu);
                Menu parentMenu = (menu.ParentMenu != null ? menu.ParentMenu : null);
                if (parentMenu != null)
                {
                    db.Menus.Attach(parentMenu);
                    db.Entry(menu.ParentMenu).State = EntityState.Unchanged;
                }
                db.SaveChanges();
            }
        }

        public void Update(Menu menu)
        {
            using (CRContext db = new CRContext())
            {
                Menu parentMenu = (menu.ParentMenu != null ? menu.ParentMenu : null);
                if (parentMenu != null)
                {
                    db.Menus.Attach(menu.ParentMenu);
                    menu.ParentMenuID = menu.ParentMenu.ParentMenuID;
                }

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(Menu menu)
        {
            using (CRContext db = new CRContext())
            {
                db.Menus.Attach(menu);
                db.Menus.Remove(menu);
                db.SaveChanges();
            }
        }


        public List<Menu> Select(Menu menu = null)
        {
            using (CRContext db = new CRContext())
            {
                if (menu == null)
                    return db.Menus
                        .Include("ParentMenu")
                            .ToList();
                else
                    return db.Menus
                        .Include("ParentMenu")
                            .Where(m => m.MenuID.Equals(menu.MenuID)
                                   || m.Label.Equals(menu.Label))
                                .ToList();
            }
        }

        public EF.Models.Menu GetDetail(EF.Models.Menu menu)
        {
            using (CRContext db = new CRContext())
            {
                return db.Menus
                    .Include("ParentMenu")
                        .Where(m => m.MenuID.Equals(menu.MenuID)
                               || m.Label.Equals(menu.Label))
                            .FirstOrDefault();
            }
        }
    }
}

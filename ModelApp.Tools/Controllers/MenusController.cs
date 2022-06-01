using ModelApp.EF.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ModelApp.Tools.Controllers
{
    public class MenusController
    {
        public void Insert(Menu menu)
        {
            using (ModelAppContext db = new ModelAppContext())
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
            using (ModelAppContext db = new ModelAppContext())
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
            using (ModelAppContext db = new ModelAppContext())
            {
                db.Menus.Attach(menu);
                db.Menus.Remove(menu);
                db.SaveChanges();
            }
        }

        public List<Menu> Select(Menu menu = null)
        {
            using (ModelAppContext db = new ModelAppContext())
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

        public Menu GetDetail(Menu menu)
        {
            using (ModelAppContext db = new ModelAppContext())
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

using ModelApp.EF.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ModelApp.Api.Controllers
{
    public class MenusController : ApiController
    {
        private ModelAppContext db = new ModelAppContext();

        // GET: api/Menus
        public IQueryable<Menu> GetMenus(String filter = null)
        {
            IQueryable<Menu> menus = null;

            switch (filter)
            {
                case "ordenated":
                    menus = db.Menus
                        .Include(m => m.ParentMenu)
                            .OrderBy(m => m.Order)
                                .AsQueryable();
                    break;

                case "all":
                    menus = db.Menus
                        .Include(m => m.ParentMenu)
                            .AsQueryable();
                    break;

                case "actives":
                    menus = db.Menus
                        .Include(m => m.ParentMenu)
                            .Where(m => m.Active == true)
                                .AsQueryable();
                    break;

                case "deactives":
                    menus = db.Menus
                        .Include(m => m.ParentMenu)
                            .Where(m => m.Active == false)
                                .AsQueryable();
                    break;

                default:
                    menus = db.Menus
                        .Include(m => m.ParentMenu)
                            .AsQueryable();
                    break;
            }

            return menus;
        }

        // GET: api/Menus/5
        [ResponseType(typeof(Menu))]
        public IHttpActionResult GetMenu(int id)
        {
            //Menu menu = db.Menus.Find(id);
            Menu menu = db.Menus
                .Include(m => m.ParentMenu)
                    .Where(m => m.MenuID == id)
                        .FirstOrDefault();

            if (menu == null)
            {
                return NotFound();
            }

            return Ok(menu);
        }

        // PUT: api/Menus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMenu(int id, Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menu.MenuID)
            {
                return BadRequest();
            }

            Menu parentMenu = (menu.ParentMenu != null ? menu.ParentMenu : null);
            if (parentMenu != null)
            {
                db.Menus.Attach(menu.ParentMenu);
                menu.ParentMenuID = menu.ParentMenu.ParentMenuID;
            }
            db.Entry(menu).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Menus
        [ResponseType(typeof(Menu))]
        public IHttpActionResult PostMenu(Menu menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Menus.Add(menu);
            Menu parentMenu = (menu.ParentMenu != null ? menu.ParentMenu : null);
            if (parentMenu != null)
            {
                db.Menus.Attach(parentMenu);
                db.Entry(menu.ParentMenu).State = EntityState.Unchanged;
            }
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = menu.MenuID }, menu);
        }

        // DELETE: api/Menus/5
        [ResponseType(typeof(Menu))]
        public IHttpActionResult DeleteMenu(int id)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return NotFound();
            }

            db.Menus.Remove(menu);
            db.SaveChanges();

            return Ok(menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MenuExists(int id)
        {
            return db.Menus.Count(e => e.MenuID == id) > 0;
        }
    }
}
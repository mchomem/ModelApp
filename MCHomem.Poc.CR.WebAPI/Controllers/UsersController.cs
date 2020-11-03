using MCHomem.Poc.CR.EF.Models;
using MCHomem.Poc.CR.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace MCHomem.Poc.CR.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private CRContext db = new CRContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users
                .Include("UserRole")
                    .OrderBy(u => u.Name)
                        .AsQueryable();
        }

        // GET: api/Users/GetUserLogin?name=VALUE&password=VALUE
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUserLogin(String name, String password)
        {
            password = new Cypher().Encrypt(password);

            User user = db.Users
                .Include("UserRole")
                    .Where(u => u.Name == name
                           && u.Password == password
                           && u.Active == true)
                        .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [ResponseType(typeof(User))]
        public IHttpActionResult GetUserSecretPhrase(String name, String secretPhrase)
        {
            if(secretPhrase == null)
            {
                return NotFound();
            }

            secretPhrase = new Cypher().Encrypt(secretPhrase);

            User user = db.Users
                .Include("UserRole")
                    .Where(u => u.Name == name
                            && u.SecretPhrase == secretPhrase
                            && u.Active == true)
                        .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users
                .Include("UserRole")
                    .Where(u => u.UserID == id)
                        .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(user.SecretPhrase))
            {
                user.SecretPhrase = new Cypher().Decrypt(user.SecretPhrase);
            }

            user.Password = new Cypher().Decrypt(user.Password);

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            if (!String.IsNullOrEmpty(user.SecretPhrase))
            {
                user.SecretPhrase = new Cypher().Encrypt(user.SecretPhrase);
            }

            user.Password = new Cypher().Encrypt(user.Password);

            db.UserRoles.Attach(user.UserRole);
            user.UserRoleID = user.UserRole.UserRoleID;
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [Route("api/users/reset-password/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutResetPassword(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            if (!String.IsNullOrEmpty(user.SecretPhrase))
            {
                user.SecretPhrase = new Cypher().Encrypt(user.SecretPhrase);
            }

            String newPassword = new PasswordGenerator().WithAlphaValue(user.Name);
            user.Password = new Cypher().Encrypt(newPassword);
            
            String pathTemplate = HttpContext.Current.Server.MapPath("/Templates/Email/email-reset-password.html");
            String body = String.Empty;

            using (StreamReader sr = new StreamReader(pathTemplate))
            {
                body = sr.ReadToEnd();
            }

            body = body
                    .Replace("{{user}}", user.Name)
                    .Replace("{{new-password}}", newPassword)
                    .Replace("{{url}}", ConfigurationManager.AppSettings["URL_APP_CLIENT"]);

            String smtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];
            Int32 smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
            String smtpUser = ConfigurationManager.AppSettings["SMTP_USER"];
            String smtpMailFrom = ConfigurationManager.AppSettings["SMTP_MAIL_FROM"];
            String smtpPass = ConfigurationManager.AppSettings["SMTP_PASS"];

            List<String> emails = new List<String>();
            emails.Add(user.Email);
            
            new MailHelper()
                .Send(
                    smtpHost
                    , smtpPort
                    , smtpUser
                    , smtpPass
                    , smtpMailFrom
                    , emails
                    , "Redefinição de senha"
                    , body
                    , true
                    );

            db.UserRoles.Attach(user.UserRole);
            user.UserRoleID = user.UserRole.UserRoleID;
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!String.IsNullOrEmpty(user.SecretPhrase))
            {
                user.SecretPhrase = new Cypher().Encrypt(user.SecretPhrase);
            }

            user.Password = new Cypher().Encrypt(user.Password);

            db.Users.Add(user);
            db.UserRoles.Attach(user.UserRole);
            db.Entry(user.UserRole).State = EntityState.Unchanged;
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }
    }
}
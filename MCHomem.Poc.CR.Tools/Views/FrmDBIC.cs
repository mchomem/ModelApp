using MCHomem.Poc.CR.EF.Models;
using MCHomem.Poc.CR.Tools.Controllers;
using MCHomem.Poc.CR.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = MCHomem.Poc.CR.EF.Models.Menu;

namespace MCHomem.Poc.CR.Tools.Views
{
    public partial class FrmDBIC : Form
    {
        #region Constructor

        public FrmDBIC()
        {
            InitializeComponent();
        }

        #endregion

        #region Envents

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnStart.Enabled = false;
            this.InsertUserRole();
            this.InsertUser();
            this.InsertMenu();
            this.btnStart.Enabled = true;

            MessageBox.Show(this, "Done", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Methods

        private void InsertUserRole()
        {
            try
            {
                UserRolesController userRolesController = new UserRolesController();

                UserRole userRole = new UserRole();
                userRole.Name = "Administrador";
                userRole.Description = "Usuário administrador com acesso irrestrito ao sistema.";
                userRole.CreatedBy = "System";
                userRole.CreatedIn = DateTime.Now;

                List<UserRole> userRoles = userRolesController.Select(new UserRole() { Name = "Administrador" });

                if(userRoles.Count > 0)
                {
                    this.dgvResult.Rows.Add("UserRole Select", "Already exists");
                    return;
                }

                this.dgvResult.Rows.Add("UserRole Select", "No exists");
                userRolesController.Insert(userRole);
                this.dgvResult.Rows.Add("UserRole Insert", "OK");
            }
            catch (Exception e)
            {
                this.dgvResult.Rows.Add("UserRole Insert", String.Format("Error\nMessage {0} \n StackTrace {1}", e.Message, e.StackTrace));
            }
        }

        private void InsertUser()
        {
            try
            {
                UsersController userController = new UsersController();

                User user = new User();
                user.Name = "admin";
                user.Password = new Cypher().Encrypt("admin");
                user.SecretPhrase = new Cypher().Encrypt("Master of system");
                user.Email = "misael.homem@gmail.com";
                user.Active = true;
                user.Avatar = null;
                user.UserRole = new UserRolesController().GetDetail(new UserRole() { Name = "Administrador" });
                user.CreatedBy = "System";
                user.CreatedIn = DateTime.Now;

                List<User> users = userController.Select(new User() { Name = "admin"});

                if(users.Count > 0)
                {
                    this.dgvResult.Rows.Add("User Select", "Already exists");
                    return;
                }

                this.dgvResult.Rows.Add("User Select", "No exists");
                userController.Insert(user);
                this.dgvResult.Rows.Add("User Insert", "OK");
            }
            catch (Exception e)
            {
                this.dgvResult.Rows.Add("User Insert", String.Format("Error\nMessage {0} \n StackTrace {1}", e.Message, e.StackTrace));
            }
        }

        private void InsertMenu()
        {
            this.InsertMenu("Home", "Home.html", "fas fa-home", 1);

            this.InsertMenu("Administração", "#", "fas fa-cogs", 2);
            this.InsertMenu("Cadastros", "#", "fas fa-book", 3);

            EF.Models.Menu parentMenuAdmin = new MenusController().Select(new EF.Models.Menu() { Label = "Administração" })[0];
            this.InsertMenu("Papel de usuário", "userrole-listing.html", "fas fa-user-tie", 1, parentMenuAdmin);
            this.InsertMenu("Usuário", "user-listing.html", "fas fa-user", 2, parentMenuAdmin);
            this.InsertMenu("Menu", "menu-listing.html", "fas fa-ellipsis-v", 3, parentMenuAdmin);

            EF.Models.Menu parentMenuCadastre = new MenusController().Select(new EF.Models.Menu() { Label = "Cadastros" })[0];
            this.InsertMenu("Cliente", "customer-listing.html", "fas fa-users", 2, parentMenuCadastre);
        }

        private void InsertMenu(String label, String page, String cssfa, int order, EF.Models.Menu parentMenu = null)
        {
            try
            {
                MenusController menuController = new MenusController();

                if (menuController.Select(new EF.Models.Menu() { Label = label }).Count > 0)
                {
                    this.dgvResult.Rows.Add("Menu Select", "Already exists");
                }
                else
                {
                    this.dgvResult.Rows.Add("Menu Select", "No exists");
                    EF.Models.Menu menu = new Menu();
                    menu.Label = label;
                    menu.Page = page;
                    menu.ParentMenu = parentMenu;
                    menu.CssFontAwesomeIcon = cssfa;
                    menu.Active = true;
                    menu.Visible = true;
                    menu.Order = order;
                    menu.CreatedBy = "System";
                    menu.CreatedIn = DateTime.Now;
                    menuController.Insert(menu);
                    this.dgvResult.Rows.Add("Menu Insert", "OK");
                }
            }
            catch (Exception e)
            {
                this.dgvResult.Rows.Add("Menu Insert", String.Format("Error\nMessage {0} \n StackTrace {1}", e.Message, e.StackTrace));
            }
        }

        #endregion
    }
}

using ModelApp.AppTools.Shareds;
using ModelApp.Domain.Entities;
using ModelApp.Service.Helpers.Interfaces;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.AppTools.Views
{
    public partial class DBICView : MdiChieldFormBase
    {
        #region Friends

        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;
        private readonly ICypherHelper _cypherHelper;

        #endregion

        #region Constructors

        public DBICView
            (
                IUserRoleService userRoleService
                , IUserService userService
                , IMenuService menuService
                , ICypherHelper cypherHelper
            )
        {
            _userRoleService = userRoleService;
            _userService = userService;
            _menuService = menuService;
            _cypherHelper = cypherHelper;
            InitializeComponent();
        }

        #endregion

        #region Events

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // this.backgroundWorker.RunWorkerAsync();
        }

        // TODO: add a backgroundWorker control and test togheter async/await methods.

        //private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    this.buttonStart.Enabled = false;
        //    this.InsertUserRole();
        //    this.InsertUser();
        //    this.InsertMenu();
        //    this.buttonStart.Enabled = true;
        //    MessageBox.Show(this, "Done", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        #endregion

        #region Methods

        private async Task InsertUserRole()
        {
            try
            {
                UserRole userRole = new UserRole();
                userRole.Name = "Administrador";
                userRole.Description = "Usuário administrador com acesso irrestrito ao sistema.";

                IEnumerable<UserRole> userRoles = await _userRoleService.GetAllAsync();

                if (userRoles.Count() > 0)
                {
                    this.dataGridViewResult.Rows.Add("UserRole Select", "Already exists");
                    return;
                }

                this.dataGridViewResult.Rows.Add("UserRole Select", "No exists");
                await _userRoleService.AddAsync(userRole);
                this.dataGridViewResult.Rows.Add("UserRole Insert", "OK");
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("UserRole Insert", $"Error\nMessage {e.Message} \n StackTrace {e.StackTrace}");
            }
        }

        private async Task InsertUser()
        {
            try
            {
                User user = new User();
                user.Name = "admin";
                user.Password = _cypherHelper.Encrypt("admin");
                user.SecretPhrase = _cypherHelper.Encrypt("Master of system");
                user.Email = "misael.homem@gmail.com";
                user.Active = true;
                user.Avatar = null;
                user.UserRole = await _userRoleService.GetAsync(new UserRole() { Name = "Administrador" });

                IEnumerable<User> users = await _userService.GetAllAsync();

                if (users.Count() > 0)
                {
                    this.dataGridViewResult.Rows.Add("User Select", "Already exists");
                    return;
                }

                this.dataGridViewResult.Rows.Add("User Select", "No exists");
                await _userService.AddAsync(user);
                this.dataGridViewResult.Rows.Add("User Insert", "OK");
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("User Insert", string.Format("Error\nMessage {0} \n StackTrace {1}", e.Message, e.StackTrace));
            }
        }

        private async void InsertMenu()
        {
            this.InsertMenu("Home", "Home.html", "fas fa-home", 1);

            this.InsertMenu("Administração", "#", "fas fa-cogs", 2);
            this.InsertMenu("Cadastros", "#", "fas fa-book", 3);

            Menu parentMenuAdmin = await _menuService.GetAsync(new Menu() { Label = "Administração" });
            this.InsertMenu("Papel de usuário", "userrole-listing.html", "fas fa-user-tie", 1, parentMenuAdmin);
            this.InsertMenu("Usuário", "user-listing.html", "fas fa-user", 2, parentMenuAdmin);
            this.InsertMenu("Menu", "menu-listing.html", "fas fa-ellipsis-v", 3, parentMenuAdmin);

            Menu parentMenuCadastre = await _menuService.GetAsync(new Menu() { Label = "Cadastros" });
            this.InsertMenu("Cliente", "customer-listing.html", "fas fa-users", 2, parentMenuCadastre);
        }

        private async void InsertMenu(string label, string page, string cssfa, int order, Menu parentMenu = null)
        {
            try
            {
                IEnumerable<Menu> menus = await _menuService.GetAllAsync(new Menu() { Label = label });

                if (menus.Count() > 0)
                {
                    this.dataGridViewResult.Rows.Add("Menu Select", "Already exists");
                }
                else
                {
                    this.dataGridViewResult.Rows.Add("Menu Select", "No exists");
                    Menu menu = new Menu();
                    menu.Label = label;
                    menu.Page = page;
                    menu.ParentMenu = parentMenu;
                    menu.CssFontAwesomeIcon = cssfa;
                    menu.Active = true;
                    menu.Visible = true;
                    menu.Order = order;
                    await _menuService.AddAsync(menu);
                    this.dataGridViewResult.Rows.Add("Menu Insert", "OK");
                }
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("Menu Insert", $"Error\nMessage {e.Message} \n StackTrace {e.StackTrace}");
            }
        }

        #endregion
    }
}

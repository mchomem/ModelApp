using Microsoft.EntityFrameworkCore;
using ModelApp.AppTools.Shareds;
using ModelApp.Domain.Entities;
using ModelApp.Infra.Contexts;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.AppTools.Views
{
    public partial class DBICView : MdiChieldFormBase
    {
        #region Friends

        private readonly ModelAppContext _context;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;

        #endregion

        #region Constructors

        public DBICView
            (
                ModelAppContext context
                , IUserRoleService userRoleService
                , IUserService userService
                , IMenuService menuService
            )
        {
            _context = context;
            _userRoleService = userRoleService;
            _userService = userService;
            _menuService = menuService;
            InitializeComponent();
        }

        #endregion

        #region Events

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            this.buttonStart.Enabled = false;

            await this.CreateDataBase();

            await this.InsertUserRole();
            await this.InsertUser();
            await this.InsertMenu();

            this.buttonStart.Enabled = true;

            MessageBox.Show(this, "Done", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Methods

        private async Task CreateDataBase()
        {
            try
            {
                if (!await _context.Database.CanConnectAsync())
                {
                    await Task.Run(() => _context.Database.Migrate());
                    this.dataGridViewResult.Rows.Add("Database Created", "OK");
                }
                else
                {
                    this.dataGridViewResult.Rows.Add("Database Created", "Already exists");
                }                
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("Database Create", $"Error\nMessage {e.Message} \n StackTrace {e.StackTrace}");
            }
        }

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
                    this.dataGridViewResult.Rows.Add("UserRole Query", "Already exists");
                    return;
                }

                this.dataGridViewResult.Rows.Add("UserRole Query", "No exists");
                await _userRoleService.AddAsync(userRole);
                this.dataGridViewResult.Rows.Add("UserRole Add", "OK");
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("UserRole Add", $"Error\nMessage {e.Message} \n StackTrace {e.StackTrace}");
            }
        }

        private async Task InsertUser()
        {
            try
            {
                User user = new User();
                user.Login = "admin";
                user.Name = "admin";
                user.Password = "admin";
                user.SecretPhrase = "Master of system";
                user.Email = "misael.homem@gmail.com";
                user.Active = true;
                user.Avatar = null;
                user.UserRole = await _userRoleService.GetAsync(new UserRole() { Name = "Administrador" });

                IEnumerable<User> users = await _userService.GetAllAsync();

                if (users.Count() > 0)
                {
                    this.dataGridViewResult.Rows.Add("User Query", "Already exists");
                    return;
                }

                this.dataGridViewResult.Rows.Add("User Query", "No exists");
                await _userService.AddAsync(user);
                this.dataGridViewResult.Rows.Add("User Add", "OK");
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("User Add", string.Format("Error\nMessage {0} \n StackTrace {1}", e.Message, e.StackTrace));
            }
        }

        private async Task InsertMenu()
        {
            await this.InsertMenu("Home", "Home.html", "fas fa-home", 1);

            await this.InsertMenu("Administração", "#", "fas fa-cogs", 2);
            await this.InsertMenu("Cadastros", "#", "fas fa-book", 3);

            Menu parentMenuAdmin = await _menuService.GetAsync(new Menu() { Label = "Administração" });
            await this.InsertMenu("Papel de usuário", "userrole-listing.html", "fas fa-user-tie", 1, parentMenuAdmin);
            await this.InsertMenu("Usuário", "user-listing.html", "fas fa-user", 2, parentMenuAdmin);
            await this.InsertMenu("Menu", "menu-listing.html", "fas fa-ellipsis-v", 3, parentMenuAdmin);

            Menu parentMenuCadastre = await _menuService.GetAsync(new Menu() { Label = "Cadastros" });
            await this.InsertMenu("Cliente", "customer-listing.html", "fas fa-users", 2, parentMenuCadastre);
        }

        private async Task InsertMenu(string label, string page, string cssfa, int order, Menu? parentMenu = null)
        {
            try
            {
                IEnumerable<Menu> menus = await _menuService.GetAllAsync(new Menu() { Label = label });

                if (menus.Count() > 0)
                {
                    this.dataGridViewResult.Rows.Add("Menu Query", "Already exists");
                }
                else
                {
                    this.dataGridViewResult.Rows.Add("Menu Query", "No exists");
                    Menu menu = new Menu();
                    menu.Label = label;
                    menu.Page = page;
                    menu.ParentMenu = parentMenu;
                    menu.CssFontAwesomeIcon = cssfa;
                    menu.Active = true;
                    menu.Visible = true;
                    menu.Order = order;
                    await _menuService.AddAsync(menu);
                    this.dataGridViewResult.Rows.Add("Menu Add", "OK");
                }
            }
            catch (Exception e)
            {
                this.dataGridViewResult.Rows.Add("Menu Add", $"Error\nMessage {e.Message} \n StackTrace {e.StackTrace}");
            }
        }

        #endregion
    }
}

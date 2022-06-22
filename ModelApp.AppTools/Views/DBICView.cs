using ModelApp.AppTools.Shareds;
using ModelApp.Domain.Entities;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.AppTools.Views
{
    public partial class DBICView : MdiChieldFormBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;

        public DBICView(IUserRoleService userRoleService, IUserService userService, IMenuService menuService)
        {
            _userRoleService = userRoleService;
            _userService = userService;
            _menuService = menuService;
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {

        }

        private async Task InsertUserRole()
        {
            UserRole userRole = new UserRole();
            userRole.Name = "Administrador";
            userRole.Description = "Usuário administrador com acesso irrestrito ao sistema.";

            IEnumerable<UserRole> userRoles = await _userRoleService.GetAllAsync();

            if(userRoles.Count() > 0)
            {
                this.dataGridViewResult.Rows.Add("UserRole Select", "Already exists");
                return;
            }

            this.dataGridViewResult.Rows.Add("UserRole Select", "No exists");
            await _userRoleService.AddAsync(userRole);
            this.dataGridViewResult.Rows.Add("UserRole Insert", "OK");
        }
    }
}

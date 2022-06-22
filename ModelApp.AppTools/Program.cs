using Microsoft.Extensions.DependencyInjection;
using ModelApp.AppTools.Views;
using ModelApp.Infra.Contexts;
using ModelApp.Infra.Repositories;
using ModelApp.Infra.Repositories.Interfaces;
using ModelApp.Service.Helpers;
using ModelApp.Service.Helpers.Interfaces;
using ModelApp.Service.Services;
using ModelApp.Service.Services.Interfaces;

namespace ModelApp.AppTools
{
    internal static class Program
    {
        // see:
        //
        // https://stackoverflow.com/questions/47516409/using-microsoft-extension-dependency-injection-on-winforms-in-c-sharp
        // https://docs.microsoft.com/en-us/answers/questions/277466/dependency-injection-in-windows-forms-and-ef-core.html
        // https://www.thecodebuzz.com/dependency-injection-windows-form-desktop-app-net-core/

        // https://stackoverflow.com/questions/70475830/how-to-use-dependency-injection-in-winforms


        [STAThread]
        static void Main()
        {
            ServiceCollection? serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ApplicationConfiguration.Initialize();

            using (ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider())
            {
                MainView mainView = serviceProvider.GetRequiredService<MainView>();
                Application.Run(mainView);
            }
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MainView, MainView>();
            services.AddScoped<CypherView, CypherView>();
            services.AddScoped<DBICView, DBICView>();

            services.AddScoped<ModelAppContext, ModelAppContext>();
            
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IMenuService, MenuService>();
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<ICypherHelper, CypherHelper>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddScoped<IPasswordGeneratorHelper, PasswordGeneratorHelper>();
        }
    }
}

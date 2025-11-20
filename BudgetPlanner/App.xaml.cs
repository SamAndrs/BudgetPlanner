using BudgetPlanner.DataAccessLayer;
using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using Wpf.Ui.Appearance;

namespace BudgetPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public static IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Build configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();


            // 2. Register services and DbContext
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddSingleton<BudgetPostService>();
            services.AddSingleton<CategoryService>();
            services.AddSingleton<PrognosisService>();
            services.AddSingleton<ViewModelBase>();

            ServiceProvider = services.BuildServiceProvider();

            // 3. Start MainWindow
            //var mainWindow = new MainWindow()
            //{
            //    DataContext = ServiceProvider.GetService<ViewModelBase>()
            //};

            //mainWindow.Show();

            ApplicationThemeManager.Apply(ApplicationTheme.Dark); // or Light
        }
    }

}

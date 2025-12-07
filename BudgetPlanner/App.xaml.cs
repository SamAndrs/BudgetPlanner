using BudgetPlanner.DataAccessLayer;
using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.IO;
using System.Windows;

namespace BudgetPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public static IConfiguration? Configuration { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Sätt trådkultur globalt
            var culture = new CultureInfo("sv-SE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Säkerställ att WPF-format använder rätt kultur
            var lang = System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag);
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(lang)
            );

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
            services.AddSingleton<UserSettingsService>();

            services.AddSingleton<MainViewModel>();
            services.AddTransient<DashboardViewVM>();
            services.AddTransient<BudgetPostsViewVM>();
            services.AddTransient<PrognosisViewVM>();
            services.AddTransient<SettingsViewVM>();

            ServiceProvider = services.BuildServiceProvider();


            // 3. Seed database
            using (var scope = ServiceProvider.CreateScope())
            {
               var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
               await context.Database.MigrateAsync();
               await DataSeeder.SeedAsync(context);

            }

            // 4. Start MainWindow
            //var mainWindow = new MainWindow();
            //mainWindow.Show();
        }
    }

}

using System.Windows;
using EmployeeManager.Services;
using EmployeeManager.Views;
using Prism.Ioc;
using Prism.Unity;

namespace EmployeeManager
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
        }
    }
}

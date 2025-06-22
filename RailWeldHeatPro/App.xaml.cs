using Microsoft.Extensions.DependencyInjection;
using RailWeldHeatPro.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace RailWeldHeatPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*
         new:隐藏基类Application中的同名静态属性public static Application Current { get; }
         (App):将Current强制转换成App类，方便其他类通过Current直接访问App中的自定义成员

        // 直接访问App类中定义的Services属性
        var service = App.Current.Services.GetService<MyService>();
        // 无需每次都进行类型转换
        // 对比：((App)Application.Current).Services...

        =>
        // 传统属性写法
        public new static App Current
        {
            get { return (App)Application.Current; }
        }
        // 表达式体属性（等价写法）
        public new static App Current => (App)Application.Current;
         */
        public new static App Current => (App)Application.Current;

        //创建服务提供者
        public IServiceProvider ServiceProviders {  get; private set; }

        public App()
        {
            ServiceProviders = ConfigureServiceProviders();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //显示默认登录窗体
            //ServiceProviders.GetService<LoginView>().Show();
            ServiceProviders.GetService<MainView>().Show();
        }

        //返回服务提供者
        private IServiceProvider? ConfigureServiceProviders()
        {
            //服务容器
            var serviceCollection = new ServiceCollection();

            //注册Views层与ViewModels层中的对象
            RegisterViewsAndViewModels(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        //注册Views层与ViewModels层中的对象
        private void RegisterViewsAndViewModels(ServiceCollection serviceCollection)
        {
            var assembly = typeof(App).Assembly;
            var viewTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("View") && !t.IsAbstract)
                .ToList();

            foreach (var viewType in viewTypes)
            {
                // 注册 View
                serviceCollection.AddSingleton(viewType);

                // 查找并注册对应的 ViewModel
                var viewModelName = $"{viewType.Namespace.Replace(".Views", ".ViewModels")}.{viewType.Name}Model";
                var viewModelType = assembly.GetTypes()
                    .FirstOrDefault(t => t.FullName == viewModelName);

                if (viewModelType != null)
                {
                    serviceCollection.AddSingleton(viewModelType);
                }
            }
        }
    }

}

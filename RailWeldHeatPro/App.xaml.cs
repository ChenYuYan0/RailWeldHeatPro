using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RailWeldHeatPro.Helper;
using RailWeldHeatPro.Models;
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
            if (!HslCommunication.Authorization.SetAuthorizationCode("e0397905-7455-4533-8c7a-3ec89b68b2a7"))
            {
                System.Windows.Forms.MessageBox.Show("active failed");
            }
            //显示默认登录窗体
            //ServiceProviders.GetService<LoginView>().Show();
            ServiceProviders.GetService<MainView>().Show();
        }

        //返回服务提供者
        private IServiceProvider? ConfigureServiceProviders()
        {
            //服务容器
            var serviceCollection = new ServiceCollection();

            //注入配置类
            ConfigureJson(serviceCollection);

            //注册Views层与ViewModels层中的对象
            RegisterViewsAndViewModels(serviceCollection);

            //注册GlobalConfig
            serviceCollection.AddSingleton<GlobalConfig>();

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// 注入json配置文件
        /// </summary>
        private void ConfigureJson(ServiceCollection serviceCollection)
        {
            //构建配置类
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory + "\\Config")
                .AddJsonFile("appsettings.json").Build();
            //将配置类注册到容器中
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            //连接数据库
            var parseResult = Enum.TryParse<SqlSugar.DbType>(configuration["SqlParam:DbType"], out var dbType);
            var connectionString = configuration["SqlParam:ConnectionString"];
            if (parseResult)
            {
                SqlSugarHelper.AddSqlSugarSetup(dbType, connectionString);
            }

            //日志配置
            /*
            log.ClearProviders();清空默认的 Console/Debug 等 Provider
            log.SetMinimumLevel(LogLevel.Trace);设置最小日志级别
            log.AddNLog(); 把 NLog 接入日志系统
             */
            serviceCollection.AddLogging(log => {
                log.ClearProviders();
                log.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                log.AddNLog();
            });
            //从json配置文件中读取nlog的相关参数
            var nLogConfig = configuration.GetSection("NLog");
            LogManager.Configuration = new NLogLoggingConfiguration(nLogConfig);

            //参数映射
            /*
             只有先执行了 AddOptions()，后面的 Configure<T>() 才能正常把服务放进容器。
             AddOptions()
            .Configure<RootParam>(e => configuration.Bind(e))把整个 IConfiguration（appsettings.json 的根对象）一次性绑定到 RootParam 实例 e 上
             */
            serviceCollection.AddOptions()
                .Configure<RootParam>(e => configuration.Bind(e))
                .Configure<SqlParam>(e => configuration.GetSection("SqlParam").Bind(e))
                .Configure<SystemParam>(e => configuration.GetSection("SystemParam").Bind(e))
                .Configure<PlcParam>(e => configuration.GetSection("PlcParam").Bind(e)); ;
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

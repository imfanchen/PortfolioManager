using System;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.UI;
using PortfolioManager.ClientApi;
using PortfolioManager.Common;
using PortfolioManager.Main.Properties;
using PortfolioManager.Main.ViewModels;
using PortfolioManager.Main.Views;
using PortfolioManager.Modules.ViewModels;
using PortfolioManager.Modules.Views;
using AppModules = PortfolioManager.Common.Modules;

namespace PortfolioManager.Main
{
    public class Bootstrapper
    {
        const string StateVersion = "1.0";
        SimpleInjector.Container container = new SimpleInjector.Container();

        public virtual void Run()
        {
            ConfigureContainer();
            RegisterModules();
            if (!RestoreState())
                InjectModules();
            ConfigureNavigation();
            ShowMainWindow();
        }

        protected IModuleManager Manager { get { return ModuleManager.DefaultManager; } }

        protected virtual void ConfigureContainer()
        {            
            container.Register<IHttpClientFactory, HttpClientFactory>(SimpleInjector.Lifestyle.Singleton);
            container.Register<SecuritiesClientApi>(SimpleInjector.Lifestyle.Singleton);
        }
        protected virtual void RegisterModules()
        {
            Manager.Register(Regions.MainWindow, new Module(AppModules.Main, MainViewModel.Create, typeof(MainView)));
            Manager.Register(Regions.Navigation, new Module(AppModules.Symbol, () => new NavigationItem(AppModules.Symbol)));
            Manager.Register(Regions.Documents, new Module(AppModules.Symbol, () => SymbolViewModel.Create(AppModules.Symbol, container.GetInstance<SecuritiesClientApi>()), typeof(SymbolView)));
            Manager.Register(Regions.Navigation, new Module(AppModules.Company, () => new NavigationItem(AppModules.Company)));
            Manager.Register(Regions.Documents, new Module(AppModules.Company, () => CompanyViewModel.Create(AppModules.Company, container.GetInstance<SecuritiesClientApi>()), typeof(CompanyView)));
        }
        protected virtual bool RestoreState()
        {
#if !DEBUG
            if (Settings.Default.StateVersion != StateVersion) return false;
            return Manager.Restore(Settings.Default.LogicalState, Settings.Default.VisualState);
#else
            return false;
#endif
        }
        protected virtual void InjectModules()
        {
            Manager.Inject(Regions.MainWindow, AppModules.Main);
            Manager.Inject(Regions.Navigation, AppModules.Symbol);
            Manager.Inject(Regions.Navigation, AppModules.Company);
        }
        protected virtual void ConfigureNavigation()
        {
            Manager.GetEvents(Regions.Navigation).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Documents).Navigation += OnDocumentsNavigation;
        }
        protected virtual void ShowMainWindow()
        {
            App.Current.MainWindow = new MainWindow();
            App.Current.MainWindow.Show();
            App.Current.MainWindow.Closing += OnClosing;
        }
        void OnNavigation(object sender, NavigationEventArgs e)
        {
            if (e.NewViewModelKey == null) return;
            Manager.InjectOrNavigate(Regions.Documents, e.NewViewModelKey);
        }
        void OnDocumentsNavigation(object sender, NavigationEventArgs e)
        {
            Manager.Navigate(Regions.Navigation, e.NewViewModelKey);
        }
        void OnClosing(object sender, CancelEventArgs e)
        {
            string logicalState;
            string visualState;
            Manager.Save(out logicalState, out visualState);
            Settings.Default.StateVersion = StateVersion;
            Settings.Default.LogicalState = logicalState;
            Settings.Default.VisualState = visualState;
            Settings.Default.Save();
        }
    }
}
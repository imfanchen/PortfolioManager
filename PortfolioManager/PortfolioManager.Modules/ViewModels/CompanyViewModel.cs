using System;
using SimpleInjector;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using PortfolioManager.Common;
using PortfolioManager.ClientApi;

namespace PortfolioManager.Modules.ViewModels
{
    public class CompanyViewModel : ViewModelBase, IDocumentModule, ISupportState<CompanyViewModel.Info>
    {

        #region Constructor

        protected CompanyViewModel()
        {
            if (string.IsNullOrEmpty(Symbol))
            {
                Content = "To load data, please first select a row from the Symbol module.";
            }
            Messenger.Default.Register<string>(this, OnSymbolReceived);
        }
        public static CompanyViewModel Create(string caption, SecuritiesClientApi clientApi)
        {
            return ViewModelSource.Create(() => new CompanyViewModel { Caption = caption, ClientApi = clientApi });
        }
        #endregion
            
        #region Commands
        public DelegateCommand RefreshCommand { get { return new DelegateCommand(() => OnSymbolReceived(Symbol)); } }

        #endregion

        #region Properties
        public bool IsActive { get; set; }
        public bool IsLoading { get; set; }
        public string Caption { get; set; }
        public string Content { get { return GetProperty(() => Content); } set { SetProperty(() => Content, value); } }
        public string Symbol { get { return GetProperty(() => Symbol); } set { SetProperty(() => Symbol, value); } }
        public SecuritiesClientApi ClientApi { get { return GetProperty(() => ClientApi); } set { SetProperty(() => ClientApi, value); } }
        public Company CompanyData { get { return GetProperty(() => CompanyData); } set { SetProperty(() => CompanyData, value); } }
        public Quote QuoteData { get { return GetProperty(() => QuoteData); } set { SetProperty(() => QuoteData, value); } }
        public ObservableCollection<Chart> ChartData { get; set; } = new ObservableCollection<Chart>();
        #endregion

        #region Methods

        private async void OnSymbolReceived(string symbol)
        {
            try
            {
                IsLoading = true;
                Symbol = symbol;
                await LoadCompanyAsync(symbol);
                await LoadQuoteAsync(symbol);
                await LoadChartAsync(symbol);
                Content = $"Symbol {Symbol} | Last Loaded: {DateTime.Now.ToString()}";
            }
            catch (Exception ex)
            {
                throw;
                //Log.Error(ex);
            }
            finally
            {
                IsLoading = false;
            }

        }
        public async Task LoadCompanyAsync(string symbol)
        {
            try
            {
                CompanyData = await ClientApi.GetCompany(symbol);
            }
            catch (Exception)
            {
                GetService<IMessageBoxService>()?.ShowMessage("An error occurred getting company data");
            }

        }

        public async Task LoadQuoteAsync(string symbol)
        {
            try
            {
                QuoteData = await ClientApi.GetQuote(symbol);                
            }
            catch (Exception)
            {
                GetService<IMessageBoxService>()?.ShowMessage("An error occurred getting quote data");
            }
        }
        public async Task LoadChartAsync(string symbol)
        {
            try
            {
                var prices = await ClientApi.GetChart(symbol);
                foreach(var price in prices)
                {
                    ChartData.Add(price);
                }
            }
            catch (Exception)
            {
                GetService<IMessageBoxService>()?.ShowMessage("An error occurred getting chart data");
            }
        }
        #endregion

        #region Serialization
        [Serializable]
        public class Info
        {
            public string Content { get; set; }
            public string Caption { get; set; }
        }
        Info ISupportState<Info>.SaveState()
        {
            return new Info()
            {
                Caption = this.Caption,
                Content = this.Content,
            };
        }
        void ISupportState<Info>.RestoreState(Info state)
        {
            this.Caption = state.Caption;
            this.Content = state.Content;
        }
        #endregion
    }
}

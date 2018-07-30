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
    public class SymbolViewModel : ViewModelBase, IDocumentModule, ISupportState<SymbolViewModel.Info>
    {

        #region Constructor

        protected SymbolViewModel() { }
        public static SymbolViewModel Create(string caption, SecuritiesClientApi clientApi)
        {
            return ViewModelSource.Create(() => new SymbolViewModel { Caption = caption, ClientApi = clientApi });
        }
        #endregion
            
        #region Commands
        public DelegateCommand RefreshCommand { get { return new DelegateCommand(Refresh); } }

        #endregion

        #region Properties
        public bool IsActive { get; set; }
        public bool IsLoading { get; set; }
        public string Caption { get; set; }
        public string Content { get { return GetProperty(() => Content); } set { SetProperty(() => Content, value); } }
        public SecuritiesClientApi ClientApi { get { return GetProperty(() => ClientApi); } set { SetProperty(() => ClientApi, value, Refresh); } }
        public Security Selection { get { return GetProperty(() => Selection); } set { SetProperty(() => Selection, value, () => SendMessage(Selection.Symbol)); } }
        public ObservableCollection<Security> Collection { get; set; } = new ObservableCollection<Security>();
        #endregion

        #region Methods

        private async void Refresh()
        {
            await LoadStockAsync();
        }

        private async Task LoadStockAsync()
        {
            try
            {
                IsLoading = true;
                var symbols = await ClientApi.GetSymbols();                
                if (symbols != null)
                {
                    Collection.Clear();
                    foreach (var symbol in symbols)
                    {
                        string typeName;
                        if (Security.TypeDictionary.TryGetValue(symbol.Type.ToUpper(), out typeName))
                            symbol.Type = typeName;                        
                        Collection.Add(symbol);
                    }
                }
                Content = $"Last Loaded: {DateTime.Now.ToString()}";
            }
            catch (Exception ex)
            {
                GetService<IMessageBoxService>()?.ShowMessage("An error occurred getting symbols data");
                //Log.Error(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SendMessage(string symbol)
        {
            Messenger.Default.Send(symbol);
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

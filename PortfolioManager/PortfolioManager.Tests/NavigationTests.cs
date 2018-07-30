using PortfolioManager.Common;
using NUnit.Framework;
using System.Threading;
using AppModules = PortfolioManager.Common.Modules;

namespace PortfolioManager.Tests
{
    [TestFixture]
    public class NavigationTests : BaseTestFixture
    {
        [Test]
        public void InitialModules()
        {
            Assert.IsNotNull(Manager.GetRegion(Regions.MainWindow).GetViewModel(AppModules.Main));
            Assert.IsNotNull(Manager.GetRegion(Regions.Navigation).GetViewModel(AppModules.Company));
            Assert.IsNotNull(Manager.GetRegion(Regions.Navigation).GetViewModel(AppModules.Symbol));
            Assert.IsNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Company));
            Assert.IsNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Symbol));
        }
        [Test]
        public void Navigation()
        {
            Assert.IsNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Company));
            Assert.IsNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Symbol));

            Manager.Navigate(Regions.Navigation, AppModules.Company);
            Assert.IsNotNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Company));
            Assert.IsNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Symbol));

            Manager.Navigate(Regions.Navigation, AppModules.Symbol);
            Assert.IsNotNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Company));
            Assert.IsNotNull(Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Symbol));

            Manager.Navigate(Regions.Documents, AppModules.Company);
            Assert.AreEqual(AppModules.Company, Manager.GetRegion(Regions.Navigation).SelectedKey);
        }
    }
    [TestFixture, Category("Functional"), Apartment(ApartmentState.STA)]
    public class FunctionalNavigationTests : BaseTestFixture
    {
        protected override bool IsFunctionalTesting { get { return true; } }
        [Test]
        public void Navigation()
        {
            Manager.Navigate(Regions.Navigation, AppModules.Company);
            DoEvents();
            var document1 = (IDocumentModule)Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Company);
            Assert.IsTrue(document1.IsActive);

            Manager.Navigate(Regions.Navigation, AppModules.Symbol);
            DoEvents();
            var document2 = (IDocumentModule)Manager.GetRegion(Regions.Documents).GetViewModel(AppModules.Symbol);
            Assert.IsFalse(document1.IsActive);
            Assert.IsTrue(document2.IsActive);

            document1.IsActive = true;
            DoEvents();
            Assert.IsTrue(document1.IsActive);
            Assert.IsFalse(document2.IsActive);
            Assert.AreEqual(AppModules.Company, Manager.GetRegion(Regions.Navigation).SelectedKey);
        }
    }
}
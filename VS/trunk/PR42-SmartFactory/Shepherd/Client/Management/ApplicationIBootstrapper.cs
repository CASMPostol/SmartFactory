using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure.Behaviors;
using Microsoft.Practices.Prism.MefExtensions;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  internal partial class ApplicationIBootstrapper : MefBootstrapper
  {
    
    protected override void ConfigureAggregateCatalog()
    {
      this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationIBootstrapper).Assembly));
      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(StockTraderRICommands).Assembly));
      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MarketModule).Assembly));
      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(PositionModule).Assembly));
      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(WatchModule).Assembly));
      //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(NewsModule).Assembly));
    }

    protected override void ConfigureContainer()
    {
      base.ConfigureContainer();
    }

    protected override void InitializeShell()
    {
      base.InitializeShell();
      Application.Current.MainWindow = (Shell)this.Shell;
      Application.Current.MainWindow.Show();
    }

    protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
    {
      var factory = base.ConfigureDefaultRegionBehaviors();

      factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

      return factory;
    }

    protected override DependencyObject CreateShell()
    {
      return this.Container.GetExportedValue<Shell>();
    }

  }
}

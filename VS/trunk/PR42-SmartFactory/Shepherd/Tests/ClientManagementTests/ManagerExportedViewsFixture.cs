using CAS.SmartFactory.Shepherd.Client.Management.Controls;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure.Behaviors;
using Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Client.Management.Tests
{
  [TestClass]
  public class ManagerExportedViewsFixture
  {
    [TestMethod]
    public void AutoPopulateButtonPanel()
    {
      Assert.Inconclusive("The test must be adopted from the RI - container cannot be created because there are errors for imports satisfied by IRegionManager export.");
      var catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(Shell).Assembly));
      CompositionContainer container = new CompositionContainer(catalog);
      IEnumerable<object> _v1 = container.GetExportedValues<object>();
      Assert.IsNotNull(_v1);
      Assert.AreEqual<int>(2, _v1.Count<object>());
      
      AutoPopulateExportedViewsBehavior behavior = container.GetExportedValue<AutoPopulateExportedViewsBehavior>();
      Region region = new Region() { Name = RegionNames.ButtonsRegion };
      region.Behaviors.Add("", behavior);
      Assert.AreEqual(1, region.Views.Cast<object>().Count());
      Assert.IsTrue(region.Views.Cast<object>().Any(e => e.GetType() == typeof(ButtonsPanel)));
    }
    [TestMethod]
    public void ManuallyPopulateButtonPanel()
    {
      var catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(Shell).Assembly));
      CompositionContainer container = new CompositionContainer(catalog);
      Region region = new Region() { Name = RegionNames.ActionRegion };
 
    }
    [TestMethod]
    public void CreateButtonPanel()
    {
      ButtonsPanel _nbp = new ButtonsPanel();
      Assert.IsNotNull(_nbp); 
    }
  }
}

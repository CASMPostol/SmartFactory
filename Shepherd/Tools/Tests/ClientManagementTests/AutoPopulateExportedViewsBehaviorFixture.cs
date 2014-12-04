//<summary>
//  Title   : AutoPopulateExportedViewsBehaviorFixture
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure.Behaviors;
using Microsoft.Practices.Prism.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  [TestClass]
  public class AutoPopulateExportedViewsBehaviorFixture
  {
    [TestMethod]
    public void WhenAttached_ThenAddsViewsRegisteredToTheRegion()
    {
      AggregateCatalog catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(AutoPopulateExportedViewsBehavior).Assembly));
      catalog.Catalogs.Add(new TypeCatalog(typeof(View1), typeof(View2)));
      CompositionContainer container = new CompositionContainer(catalog);

      AutoPopulateExportedViewsBehavior behavior = container.GetExportedValue<AutoPopulateExportedViewsBehavior>();

      Region region = new Region() { Name = "region1" };

      region.Behaviors.Add("", behavior);

      Assert.AreEqual(1, region.Views.Cast<object>().Count());
      Assert.IsTrue(region.Views.Cast<object>().Any(e => e.GetType() == typeof(View1)));
    }

    [TestMethod]
    public void WhenRecomposed_ThenAddsNewViewsRegisteredToTheRegion()
    {
      var catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(AutoPopulateExportedViewsBehavior).Assembly));
      catalog.Catalogs.Add(new TypeCatalog(typeof(View1), typeof(View2)));

      var container = new CompositionContainer(catalog);

      var behavior = container.GetExportedValue<AutoPopulateExportedViewsBehavior>();

      var region = new Region() { Name = "region1" };

      region.Behaviors.Add("", behavior);

      catalog.Catalogs.Add(new TypeCatalog(typeof(View3), typeof(View4)));

      Assert.AreEqual(2, region.Views.Cast<object>().Count());
      Assert.IsTrue(region.Views.Cast<object>().Any(e => e.GetType() == typeof(View1)));
      Assert.IsTrue(region.Views.Cast<object>().Any(e => e.GetType() == typeof(View3)));
    }
  }

  [ViewExport(RegionName = "region1")]
  public class View1 { }

  [ViewExport(RegionName = "region2")]
  public class View2 { }

  [ViewExport(RegionName = "region1")]
  public class View3 { }

  [ViewExport(RegionName = "region2")]
  public class View4 { }
}


using System;
using System.Security.Principal;
using System.Windows.Forms;
using CAS.SmartFactory.Deployment.Properties;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.Deployment
{
  class structure
  {
    public SPFarm SPFarm { get; set; }
    public SPWebApplication SPWebApplication { get; private set; }
  }
  static class Program
  {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new SetUpData());
      if (MessageBox.Show("Start retracking the solutions", "Uninstall", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        Application.Run(new Uninstall());
      //LookupEnvironment();
    }
    private static void LookupEnvironment()
    {
      SPFarm farm = SPFarm.Local;
      Console.WriteLine("Solutions in the SPFarm");
      foreach (SPSolution item in farm.Solutions)
      {
        Console.WriteLine("SPFarm.Solutions" + item.Name);
      }
      SPSolution _Sol = farm.Solutions["ShepherdDashboards.wsp"];
      Console.WriteLine("ShepherdDashboards.wsp status = " + _Sol.Status.ToString());
      //foreach (var item in farm.FeatureDefinitions)
      //{
      //  Console.WriteLine("AvailableFeatures: " + item.DisplayName );
      //}
      Uri _uri = new Uri("http://casmp");
      SPWebApplication _wa = SPWebApplication.Lookup(_uri);
      WindowsIdentity _id = WindowsIdentity.GetCurrent();
      Console.WriteLine("You are: " + _id.Name);
      _wa.Sites.Add(@"sites/AutoSite", _id.Name, "");
      Console.WriteLine("");
      Console.WriteLine("Sites in the web application at http://casmp");
      foreach (SPSite _spsite in _wa.Sites)
      {
        Console.WriteLine(String.Format("Site url: {0} Features count: {1}", _spsite.Url, _spsite.Features.Count));
        Console.WriteLine("Features in the SPSite:");
        foreach (SPFeature _ftr in _spsite.Features)
        {
          Console.WriteLine(String.Format
            ("SPFeature Definition.DisplayName={0}, Definition.Name={1}, Definition.Scope={2}, FeatureDefinitionScope={3}, Version={4}",
             _ftr.Definition.DisplayName, _ftr.Definition.Name, _ftr.Definition.Scope, _ftr.FeatureDefinitionScope, _ftr.Version));
        }
        Console.WriteLine("Features in the SPSite:");
        foreach (SPFeatureDefinition item in _spsite.FeatureDefinitions)
        {
          Console.WriteLine(String.Format("SPFeatureDefinition SPFeatureDefinition", item.Name));
        }
        Console.WriteLine("Solution in the SPSite:");
        foreach (SPUserSolution _sl in _spsite.Solutions)
        {
          Console.WriteLine(String.Format("SPSolution Name={0}, Signature={1}, Status={2}", _sl.Name, _sl.Signature, _sl.Status));
        }
        SPWeb _wp = _spsite.RootWeb;
        WriteSPWebInfo(_wp);
        Console.WriteLine("Webs in the SPWeb");
        foreach (SPWeb _spweb in _wp.Webs)
          WriteSPWebInfo(_spweb);
        SPList _ssg = _wp.GetCatalog(SPListTemplateType.SolutionCatalog);
        Console.WriteLine("This web solutions in the Solution Catalog info: ");
        foreach (SPListItem _solIdx in _ssg.Items)
        {
          Console.WriteLine(String.Format("Solution Name{0}, Level= {1}, Title={2}, Url={3}, ID={4}", _solIdx.Name, _solIdx.Level, _solIdx.Title, _solIdx.Url, _solIdx.ID));
        }
      }
    }
    private static void WriteSPWebInfo(SPWeb _wp)
    {
      Console.WriteLine(String.Format("SPWeb Name={0}, Language={1}, ParentWeb.Name={2}, PortalName={3}, PortalUrl={4}",
        _wp.Name,
        _wp.Language,
        _wp.ParentWeb == null ? "null" : _wp.ParentWeb.Name,
        _wp.PortalName,
        _wp.PortalUrl));
    }
  }
}

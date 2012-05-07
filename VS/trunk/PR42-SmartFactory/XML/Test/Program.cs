using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.Security.Principal;

namespace CAS.SmartFactory.SPMetalHelper
{

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

      using (SPSite site = new SPSite(Properties.Settings.Default.URL))
      {
        using (SPWeb _wb = site.RootWeb)
        {
          foreach (SPContentType item in _wb.AvailableContentTypes)
          {

          }
        }
      }
    }
  }
}

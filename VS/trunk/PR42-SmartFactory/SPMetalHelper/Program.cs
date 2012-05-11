using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.Security.Principal;
using CAS.SmartFactory.SPMetalHelper.SPMetalParameters;
using System.IO;

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
      PRWeb _web = null;
      using (SPSite site = new SPSite(Properties.Settings.Default.URL))
      {
        using (SPWeb _wb = site.RootWeb)
        {
          List<PRContentType> _cts = new List<PRContentType>();
          foreach (SPContentType _contentType in _wb.AvailableContentTypes)
          {
            if (!_contentType.Group.Contains("CAS"))
              continue;
            List<PRColumn> _columns = new List<PRColumn>();
            foreach (SPField _fx in _contentType.Fields)
            {
              if (_fx.Hidden)
                continue;
              PRColumn _newColumn = new PRColumn()
              {
                Name = _fx.InternalName,
                Member = _fx.InternalName,
                TypeSpecified = false
              };
              _columns.Add(_newColumn);
            }
            PRContentType _newCT = new PRContentType
            {
              Name = _contentType.Name,
              Class = _contentType.Name,
              Column = _columns.ToArray()
            };
            _cts.Add(_newCT);
          }
          _web = new PRWeb()
          {
            AccessModifier = PRAccessModifier.Internal,
            Class = "EntitiesDataContext",
            ContentType = _cts.ToArray()
          };
        }
      }
      using (Stream _str = new FileInfo(Properties.Settings.Default.FileName).Create() )
        PRWeb.ImportDocument(_str, _web);
    }
  }
}

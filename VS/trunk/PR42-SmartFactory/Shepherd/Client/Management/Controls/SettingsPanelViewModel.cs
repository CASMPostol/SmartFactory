using CAS.Common.ComponentModel;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  [Export]
  public class SettingsPanelViewModel: PropertyChangedBase
  {
    public SettingsPanelViewModel()
    {
      RestoreSettings();
    }
    public string URL
    {
      get
      {
        return b_URL;
      }
      set
      {
        RaiseHandler<string>(value, ref b_URL, "URL", this);
      }
    }
    public string DatabaseName
    {
      get
      {
        return b_DatabaseName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_DatabaseName, "DatabaseName", this);
      }
    }
    public string SQLServer
    {
      get
      {
        return b_SQLServer;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SQLServer, "SQLServer", this);
      }
    }
    [Import]
    public ShellViewModel MasterShellViewModel
    {
      set
      {
        value.EnterState<SetupDataDialogMachine>();
      }
    }
    //vars
    private string b_URL = string.Empty;
    private string b_DatabaseName = string.Empty;
    private string b_SQLServer = string.Empty;
    //methods
    private void RestoreSettings()
    {
      //User
      URL = Properties.Settings.Default.SiteURL;
      DatabaseName = Properties.Settings.Default.SQLDatabaseName;
      SQLServer = Properties.Settings.Default.SQLServer;
    }

  }
}

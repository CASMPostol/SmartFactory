using CAS.SmartFactory.Customs.Account;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  /// <summary>
  /// CWClearanceHelpers implementation of the <see cref="ICWAccountFactory"/> interface
  /// </summary>
  public class CWClearanceHelpers
  {
    private CWClearanceHelpers()
    { }
    internal static ICWAccountFactory GetICWAccountFactory()
    {
      IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
      return serviceLocator.GetInstance<ICWAccountFactory>();
    }
  }
}

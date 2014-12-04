//<summary>
//  Title   : EnterpriseLibraryLoggerAdapter
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
      
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Prism.Logging;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  public class EnterpriseLibraryLoggerAdapter : ILoggerFacade
  {
    public EnterpriseLibraryLoggerAdapter()
    {
      Logger.SetLogWriter(new LogWriter(new LoggingConfiguration()));
    }

    #region ILoggerFacade Members

    public void Log(string message, Microsoft.Practices.Prism.Logging.Category category, Priority priority)
    {
      Logger.Write(message, category.ToString(), (int)priority);
    }

    #endregion

  }
}

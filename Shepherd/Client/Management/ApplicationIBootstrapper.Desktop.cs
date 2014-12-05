//<summary>
//  Title   : ApplicationIBootstrapper - adopted from StockTraderRI
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.MefExtensions;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  /// <summary>
  /// ApplicationIBootstrapper part that defines the logger 
  /// </summary>
  internal partial class ApplicationIBootstrapper 
    {
      private readonly EnterpriseLibraryLoggerAdapter _logger = new EnterpriseLibraryLoggerAdapter();

      protected override ILoggerFacade CreateLogger()
      {
        return _logger;
      }

    }
}

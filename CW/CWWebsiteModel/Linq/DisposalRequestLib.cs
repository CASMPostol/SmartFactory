//<summary>
//  Title   : partial class DisposalRequestLib
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// partial class DisposalRequestLib
  /// </summary>
  public partial class DisposalRequestLib
  {

    /// <summary>
    /// Statements the name of the template document name file.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public string StatementDocumentNameFileName(Entities entities)
    {
      return Settings.StatementDocumentNameFileName(entities, this.Id.Value);
    }

  }
}

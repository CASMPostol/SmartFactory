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

using System.Collections.Generic;
using System.Linq;

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
    /// <summary>
    /// Reverse lookup to <see cref="CustomsWarehouseDisposal"/> entities.
    /// </summary>
    /// <param name="edc">The entities.</param>
    /// <param name="emptyListIfNew">if the <see cref="DisposalRequestLib"/> is just created the method returns empty list if it is set to <c>true</c>, null otherwise.</param>
    /// <returns></returns>
    public IEnumerable<CustomsWarehouseDisposal> CustomsWarehouseDisposal(Entities edc, bool emptyListIfNew)
    {
      if (!this.Id.HasValue)
        return emptyListIfNew ? new CustomsWarehouseDisposal[] { } : null;
      if (m_CustomsWarehouseDisposal == null)
        m_CustomsWarehouseDisposal = from _cwdx in edc.CustomsWarehouseDisposal let _id = _cwdx.CWL_CWDisposal2DisposalRequestLibraryID.Id.Value where _id == this.Id.Value select _cwdx;
      return m_CustomsWarehouseDisposal;
    }
    private IEnumerable<CustomsWarehouseDisposal> m_CustomsWarehouseDisposal = null;
  }
}

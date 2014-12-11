//<summary>
//  Title   : InvoiceLib
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

using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// InvoiceLib Entity
  /// </summary>
  public partial class InvoiceLib
  {
    /// <summary>
    /// Reverse lookup returning associated <see cref="InvoiceContent"/> collection.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public IEnumerable<InvoiceContent> InvoiceContent(Entities entities)
    {
      if (!this.Id.HasValue)
        return null;
      if (m_InvoiceContent == null)
        m_InvoiceContent = from _idx in entities.InvoiceContent let _id = _idx.InvoiceIndex.Id.Value where _id == this.Id.Value select _idx;
      return m_InvoiceContent; 
    }

    private IEnumerable<InvoiceContent> m_InvoiceContent = null;

  }
}

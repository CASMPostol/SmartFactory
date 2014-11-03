//<summary>
//  Title   : InvoiceLib
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
      return entities.InvoiceContent.Where<InvoiceContent>(x => x.InvoiceIndex == this);
    }
  }
}

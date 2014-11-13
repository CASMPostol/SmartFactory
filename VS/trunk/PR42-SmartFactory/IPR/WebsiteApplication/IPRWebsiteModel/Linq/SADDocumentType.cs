//<summary>
//  Title   : SADDocumentType
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
  /// SADDocumentType entity class
  /// </summary>
  public partial class SADDocumentType
  {

    /// <summary>
    /// Reverse lookup to <see cref="SADGood"/> entities.
    /// </summary>
    /// <param name="_entities">The _entities.</param>
    /// <returns></returns>
    public IEnumerable<SADGood> SADGood(Entities _entities)
    {
      if (m_SADGood == null)
        m_SADGood = from _sdx in _entities.SADGood let _id = _sdx.SADDocumentIndex.Id.Value where this.Id.Value == _id select _sdx;    
      return m_SADGood;
    }

    private IEnumerable<SADGood> m_SADGood = null;
  }
}

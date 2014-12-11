//<summary>
//  Title   : SADGood
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

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// SADGood entity
  /// </summary>
  public partial class SADGood
  {

    #region public API
    /// <summary>
    /// Reverse lookup for <see cref="SADRequiredDocuments"/> entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public IEnumerable<SADRequiredDocuments> SADRequiredDocuments(Entities entities, bool emptyListIfNew)
    {

      if (!this.Id.HasValue)
        return emptyListIfNew ? new SADRequiredDocuments[] { } : null;
      if (m_SADRequiredDocuments == null)
        m_SADRequiredDocuments = from _srdx in entities.SADRequiredDocuments
                                 let _id = _srdx.SADRequiredDoc2SADGoodID.Id.Value
                                 where this.Id.Value == _id
                                 select _srdx;
      return m_SADRequiredDocuments;
    }
    /// <summary>
    /// Reverse lookup for <see cref="SADDuties"/>
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    internal IEnumerable<SADDuties> SADDuties(Entities entities, bool emptyListIfNew)
    {
      if (!this.Id.HasValue)
        return emptyListIfNew ? new SADDuties[] { } : null;
      if (m_SADDuties == null)
        m_SADDuties = from _sdtsx in entities.SADDuties let _id = _sdtsx.SADDuties2SADGoodID.Id.Value where this.Id.Value == _id select _sdtsx;
      return m_SADDuties;
    }
    internal IEnumerable<SADPackage> SADPackage(Entities entities, bool emptyListIfNew)
    {
      if (!this.Id.HasValue)
        return emptyListIfNew ? new SADPackage[] { } : null;
      if (m_SADPackage == null)
        m_SADPackage = from _sdpckx in entities.SADPackage let _id = _sdpckx.SADPackage2SADGoodID.Id.Value where this.Id.Value == _id select _sdpckx;
      return m_SADPackage;
    }
    internal IEnumerable<SADQuantity> SADQuantity(Entities entities, bool emptyListIfNew)
    {
      if (!this.Id.HasValue)
        return emptyListIfNew ? new SADQuantity[] { } : null;
      if (m_SADQuantity == null)
        m_SADQuantity = from _SADQuantityx in entities.SADQuantity let _id = _SADQuantityx.SADQuantity2SADGoodID.Id.Value where this.Id.Value == _id select _SADQuantityx;
      return m_SADQuantity;
    }
    #endregion

    #region private
    private IEnumerable<SADRequiredDocuments> m_SADRequiredDocuments = null;
    private IEnumerable<SADDuties> m_SADDuties = null;
    private IEnumerable<SADPackage> m_SADPackage = null;
    private IEnumerable<SADQuantity> m_SADQuantity = null;
    #endregion

  }
}

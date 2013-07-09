//<summary>
//  Title   : Common Accoun tData
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.Customs.Account
{
  /// <summary>
  /// CommonAccountData classs
  /// </summary>
  public abstract class CommonAccountData
  {

    #region cretor
    protected CommonAccountData()
    {
      ConsentLookup = new Nullable<int>();
    }
    #endregion

    #region private
    /// <summary>
    /// The m_ warnings
    /// </summary>
    protected List<Warnning> m_Warnings = new List<Warnning>();

    /// <summary>
    /// Gets the process.
    /// </summary>
    /// <value>
    /// The process.
    /// </value>
    protected abstract CustomsProcess Process { get; }
    #endregion

    #region public
    /// <summary>
    /// Customs process enum
    /// </summary>
    public enum CustomsProcess
    {
      /// <summary>
      /// The ipr
      /// </summary>
      ipr,
      /// <summary>
      /// The cw
      /// </summary>
      cw
    };
    /// <summary>
    /// Message Type
    /// </summary>
    public enum MessageType
    {
      /// <summary>
      /// The PZC
      /// </summary>
      PZC,
      /// <summary>
      /// The SAD
      /// </summary>
      SAD
    }
    /// <summary>
    /// Validates the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="warnnings">The warnnings.</param>
    /// <returns></returns>
    public bool Validate( List<Warnning> warnnings )
    {
      bool _ret = m_Warnings.Count == 0;
      warnnings.AddRange( m_Warnings );
      return _ret;
    }
    public int? ConsentLookup { get; protected set; }
    public DateTime CustomsDebtDate { get; protected set; }
    public object DocumentNo { get; protected set; }
    public string GradeName { get; protected set; }
    public double GrossMass { get; protected set; }
    public string Invoice { get; protected set; }
    public double NetMass { get; protected set; }
    public string TobaccoName { get; protected set; }
    public double UnitPrice { get; protected set; }
    public double Value { get; protected set; }
    public string BatchId { get; protected set; }
    public int PCNTariffCodeLookup { get; protected set; }
    public string SKU { get; protected set; }
    public DateTime ValidToDate { get; protected set; }
    #endregion

  }
}

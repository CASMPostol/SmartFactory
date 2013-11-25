//<summary>
//  Title   : Common Accoun tData
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

using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.Customs.Account
{
  /// <summary>
  /// CommonAccountData classs
  /// </summary>
  public abstract class CommonAccountData : CommonClearanceData
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="CommonAccountData"/> class.
    /// </summary>
    /// <param name="clearenceLookup">The clearence lookup.</param>
    public CommonAccountData(int clearenceLookup)
      : base(clearenceLookup)
    { }

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
    /// <param name="warnnings">The warnnings.</param>
    /// <returns></returns>
    public bool Validate(List<Warnning> warnnings)
    {
      bool _ret = m_Warnings.Count == 0;
      warnnings.AddRange(m_Warnings);
      return _ret;
    }
    /// <summary>
    /// Gets or sets the consent lookup.
    /// </summary>
    /// <value>
    /// The consent lookup.
    /// </value>
    public int ConsentLookup { get; protected set; }
    /// <summary>
    /// Gets or sets the PCN tariff code lookup.
    /// </summary>
    /// <value>
    /// The PCN tariff code lookup.
    /// </value>
    public int PCNTariffCodeLookup { get; protected set; }
    /// <summary>
    /// Gets or sets the customs debt date.
    /// </summary>
    /// <value>
    /// The customs debt date.
    /// </value>
    public DateTime CustomsDebtDate { get; protected set; }
    /// <summary>
    /// Gets or sets the document no.
    /// </summary>
    /// <value>
    /// The document no.
    /// </value>
    public string DocumentNo { get; protected set; }
    /// <summary>
    /// Gets or sets the name of the grade.
    /// </summary>
    /// <value>
    /// The name of the grade.
    /// </value>
    public string GradeName { get; protected set; }
    /// <summary>
    /// Gets or sets the gross mass.
    /// </summary>
    /// <value>
    /// The gross mass.
    /// </value>
    public double GrossMass { get; protected set; }
    /// <summary>
    /// Gets or sets the invoice.
    /// </summary>
    /// <value>
    /// The invoice.
    /// </value>
    public string Invoice { get; protected set; }
    /// <summary>
    /// Gets or sets the net mass.
    /// </summary>
    /// <value>
    /// The net mass.
    /// </value>
    public double NetMass { get; protected set; }
    /// <summary>
    /// Gets or sets the name of the tobacco.
    /// </summary>
    /// <value>
    /// The name of the tobacco.
    /// </value>
    public string TobaccoName { get; protected set; }
    /// <summary>
    /// Gets or sets the batch unique identifier.
    /// </summary>
    /// <value>
    /// The batch unique identifier.
    /// </value>
    public string BatchId { get; protected set; }
    /// <summary>
    /// Gets or sets the sku.
    /// </summary>
    /// <value>
    /// The sku.
    /// </value>
    public string SKU { get; protected set; }
    #endregion

  }
}

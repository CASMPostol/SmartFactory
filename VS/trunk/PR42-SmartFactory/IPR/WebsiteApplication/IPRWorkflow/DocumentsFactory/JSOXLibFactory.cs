//<summary>
//  Title   : JSOXLibFActory class
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
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{

  /// <summary>
  /// JSOXLibFActory class
  /// </summary>
  internal class JSOXLibFactory: JSOXLibFactoryBase
  {

    #region public

    #region override
    /// <summary>
    /// Gets the outbound quantity.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    /// <returns></returns>
    public override decimal GetOutboundQuantity(Entities entities, JSOXLib parent, out DateTime start, out DateTime end)
    {
      decimal _ret = 0;
      SummaryContentList = new List<JSOXCustomsSummaryContent>();
      start = LinqIPRExtensions.DateTimeMaxValue;
      end = LinqIPRExtensions.DateTimeMinValue;
      foreach (Disposal _dspx in Disposal.GetEntries4JSOX(entities, parent))
      {
        start = LinqIPRExtensions.Min(start, _dspx.SADDate.GetValueOrDefault(LinqIPRExtensions.DateTimeMaxValue));
        end = LinqIPRExtensions.Max(end, _dspx.SADDate.GetValueOrDefault(LinqIPRExtensions.DateTimeMinValue));
        JSOXCustomsSummaryContent _new = new JSOXCustomsSummaryContent()
        {
          CompensationGood = _dspx.Disposal2PCNID == null ? "TBD" : _dspx.Disposal2PCNID.CompensationGood,
          EntryDocumentNo = _dspx.Disposal2IPRIndex.DocumentNo,
          ExportOrFreeCirculationSAD = _dspx.SADDocumentNo,
          InvoiceNo = _dspx.InvoiceNo,
          SADDate = _dspx.SADDate.GetValueOrDefault(),
          Quantity = _dspx.SettledQuantityDec,
          Balance = _dspx.RemainingQuantity.Round2DecimalOrDefault(),
          Procedure = _dspx.CustomsProcedure,
          SadConsignmentNo = _dspx.SadConsignmentNo
        };
        _ret += _dspx.SettledQuantityDec;
        _dspx.JSOXReportID = parent.Id.Value;
        SummaryContentList.Add(_new);
      }
      return _ret;
    }
    #endregion
    #endregion

    #region internal
    internal static JSOXLibFactory ConstructJSOXLibFActory( Entities edc, int jsoxLibindex )
    {
      JSOXLibFactory _ret = new JSOXLibFactory();
      _ret.GetJSOXLib( edc, jsoxLibindex );
      return _ret;
    }
    internal List<JSOXCustomsSummaryContent> SummaryContentList { get; private set; }
    #endregion

    #region private
    private JSOXLibFactory() { }
    #endregion
  }
}

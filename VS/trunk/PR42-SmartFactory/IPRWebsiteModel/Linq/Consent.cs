using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Consent
  /// </summary>
  public partial class Consent
  {
    /// <summary>
    /// Finds the specified edc.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="consentNo">The consent no.</param>
    /// <returns></returns>
    public static Consent Find( Entities edc, string consentNo )
    {
      return ( from _cidx in edc.Consent where _cidx.Title.Trim().Equals( consentNo.Trim() ) orderby _cidx.Wersja descending select _cidx ).FirstOrDefault();
    }
    public enum CustomsProcess {ipr, cw}; 
    internal static Consent DefaultConsent( Entities edc, CustomsProcess process, string number  )
    {
      int _defPeriod = 90;
      DateTime _defDate = DateTime.Today.Date;
      Consent _ret = new Consent()
      {
        ConsentDate = _defDate,
        ConsentPeriod =_defPeriod,
        IsIPR = process == CustomsProcess.ipr,
        Title = String.Format("Preliminary for: {0};", number),
        ValidFromDate = _defDate,
        ValidToDate = _defDate + TimeSpan.FromDays( _defPeriod )
      };
      edc.Consent.InsertOnSubmit( _ret );
      return _ret;
    }
  }
}

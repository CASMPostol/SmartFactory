using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;

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
      return ( from _cidx in edc.Consent where _cidx.Title.Trim().Equals( consentNo.Trim() ) orderby _cidx.Version descending select _cidx ).FirstOrDefault();
    }
    /// <summary>
    /// Enumerates supported customs processes
    /// </summary>
    internal enum CustomsProcess { ipr, cw };
    internal static Consent DefaultConsent( Entities edc, CustomsProcess process, string number )
    {
      int _defPeriod = 360;
      DateTime _defDate = DateTime.Today.Date;
      Consent _ret = new Consent()
      {        
        ConsentDate = _defDate,
        ConsentPeriod = _defPeriod / 30,
        IsIPR = process == CustomsProcess.ipr,
        Title = String.Format( "{0}", number.NotAvailable() ),
        ValidFromDate = _defDate,
        ValidToDate = _defDate + TimeSpan.FromDays( _defPeriod )
      };
      edc.Consent.InsertOnSubmit( _ret );
      edc.SubmitChanges();
      return _ret;
    }
  }
}

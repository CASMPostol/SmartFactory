//<summary>
//  Title   : Settings entity class
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
using System.Linq;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Settings entity class
  /// </summary>
  public partial class Settings
  {
    /// <summary>
    /// Gets the parameter.
    /// </summary>
    /// <param name="entities">The edc.</param>
    /// <param name="index">The parameter index.</param>
    /// <returns></returns>
    public static string GetParameter( Entities entities, SettingsEntry index )
    {
      Settings _ret = ( from _sx in entities.Settings where _sx.KeyValue.Contains( index.ToString() ) select _sx ).FirstOrDefault();
      if ( _ret == null )
      {
        _ret = new Settings()
        {
          KeyValue = index.ToString(),
          Title = m_DefaultSettings[ index ]
        };
        entities.Settings.InsertOnSubmit( _ret );
        entities.SubmitChanges();
      }
      return _ret.Title;
    }
    public static string BinCardDocumentName( Entities entities, int itemId )
    {
      return String.Format( GetParameter( entities, SettingsEntry.BinCardFileName ), itemId );
    }
    public static string CustomsProcedureCodeA004 = "A004";
    public static string CustomsProcedureCodeN865 = "N865";
    public static string CustomsProcedureCodeN954 = "N954";

    private static Dictionary<SettingsEntry, string> m_DefaultSettings = new Dictionary<SettingsEntry, string>()
    {
       { SettingsEntry.GoodsDescription_CWQuantity_Pattern,  @"(?<=\bBATCH:)\D*\d*\D*(\d*[.,,]\d*)\W*\w*\D*\d*\W*CT" },
       { SettingsEntry.GoodsDescription_CWPackageUnits_Pattern, @"(?<=\bBATCH:)\D*\d*\D*\d*[.,,]\d*\W*\w*\D*(\d*)\W*CT" },
       { SettingsEntry.GoodsDescription_Units_Pattern,  @"(?<=\bBATCH:)\D*\d*\D*\d*[.,,]\d*\W*(\w*)\D*\d*\W*CT" },
       { SettingsEntry.GoodsDescription_CertificateOfAuthenticity_Pattern,  @"\b([\w\d\s\.,-]*)/.*" },
       { SettingsEntry.GoodsDescription_CertificateOfOrgin_Pattern, @"\b([\w\d\s\.,-]*)/.*" },
       { SettingsEntry.DefaultValidToDatePeriod, "730" },
       { SettingsEntry.LooselyFormatedDate, @"(?<=/)\D*(\d{1,2}).(\d{1,2}).(\d{4})" },
       { SettingsEntry.BinCardFileName, "Bin Card {0:D7}" },
       { SettingsEntry.OrganizationEmail, "gstmaan@jti.com" },
    };
  }
  public enum SettingsEntry
  {
    GoodsDescription_CWQuantity_Pattern,
    GoodsDescription_CWPackageUnits_Pattern,
    GoodsDescription_Units_Pattern,
    GoodsDescription_CertificateOfAuthenticity_Pattern,
    GoodsDescription_CertificateOfOrgin_Pattern,
    DefaultValidToDatePeriod,
    LooselyFormatedDate,
    BinCardFileName,
    OrganizationEmail
  }
}

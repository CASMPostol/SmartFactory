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

    #region public
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
    /// <summary>
    /// Bins the name of the card document.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="itemId">The item unique identifier.</param>
    public static string BinCardDocumentName( Entities entities, int itemId )
    {
      return String.Format( GetParameter( entities, SettingsEntry.BinCardFileName ), itemId );
    }
    /// <summary>
    /// Finisheds the name of the goods export form file.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="number">The number.</param>
    public static string SADTemplateDocumentNameFileName( Entities edc, int number )
    {
      return String.Format( GetParameter( edc, SettingsEntry.SADTemplateDocumentNamePattern ), number );
    }
    public static string CustomsProcedureCodeA004 = "A004";
    public static string CustomsProcedureCodeN865 = "N865";
    public static string CustomsProcedureCodeN954 = "N954";
    public static string CustomsProcedureCode9DK8 = "9DK8";
    #endregion

    private const string c_documentNumberFormat = "{0:D7}";
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
       { SettingsEntry.SADTemplateDocumentNamePattern, "SAD_" + c_documentNumberFormat },
       { SettingsEntry.ClearanceTitleFormat, "Nr: {3:D5} {0}/{1} SAD: {2} " }
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
    OrganizationEmail,
    SADTemplateDocumentNamePattern,
    ClearanceTitleFormat
  }
}

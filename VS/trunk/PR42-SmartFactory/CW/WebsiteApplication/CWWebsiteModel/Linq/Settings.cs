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
    public static string GetParameter(Entities entities, SettingsEntry index)
    {
      Settings _ret = (from _sx in entities.Settings where _sx.KeyValue == (index.ToString()) select _sx).FirstOrDefault();
      if (_ret == null)
      {
        _ret = new Settings()
        {
          KeyValue = index.ToString(),
          Title = m_DefaultSettings[index]
        };
        entities.Settings.InsertOnSubmit(_ret);
        entities.SubmitChanges();
      }
      return _ret.Title;
    }
    /// <summary>
    /// Bins the name of the card document.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="itemId">The item unique identifier.</param>
    public static string BinCardDocumentName(Entities entities, int itemId)
    {
      return String.Format(GetParameter(entities, SettingsEntry.BinCardFileName), itemId);
    }
    /// <summary>
    /// Clearances the name of the clerance request file.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="itemId">The item unique identifier.</param>
    /// <returns></returns>
    public static string ClearanceRequestFileName(Entities entities, int itemId)
    {
      return String.Format(GetParameter(entities, SettingsEntry.ClearanceRequestFileName), itemId);
    }
    //CustomsWarehouseReportFileName
    public static string CustomsWarehouseReportFileName(Entities entities, int itemId)
    {
      return String.Format(GetParameter(entities, SettingsEntry.CustomsWarehouseReportFileName), itemId);
    }
    /// <summary>
    /// Finisheds the name of the goods export form file.
    /// </summary>
    /// <param name="entities">The edc.</param>
    /// <param name="number">The number.</param>
    internal static string SADTemplateDocumentNameFileName(Entities entities, int number)
    {
      return String.Format(GetParameter(entities, SettingsEntry.SADTemplateDocumentNamePattern), number);
    }
    internal static string StatementDocumentNameFileName(Entities entities, int number)
    {
      return String.Format(GetParameter(entities, SettingsEntry.StatementDocumentNameFileNamePattern), number);
    }
    public static string CustomsProcedureCodeA004 = "A004";
    public static string CustomsProcedureCodeN865 = "N865";
    public static string CustomsProcedureCodeN954 = "N954";
    public static string CustomsProcedureCode9DK8 = "9DK8";
    public static string CustomsProcedureCodeN935 = "N935";
    internal enum DutyKindEnum
    {
      VAT,
      Duty,
      ExciseDuty
    }
    internal static DutyKindEnum DutyKind(string dutyKind)
    {
      Dictionary<string, DutyKindEnum> _dctnry = new Dictionary<string, DutyKindEnum>() 
        { 
          { "A10", DutyKindEnum.Duty }, { "A00", DutyKindEnum.Duty }, { "A20", DutyKindEnum.Duty },
          { "1A1", DutyKindEnum.ExciseDuty },
          { "B00", DutyKindEnum.VAT }, { "B10", DutyKindEnum.VAT }, { "B20", DutyKindEnum.VAT }
        };
      return _dctnry[dutyKind];
    }
    internal static string FormatGoodsName(Entities entities, string tobaccoName, string grade, string sku, string batch, ClearingType clearingType, string documentNo)
    {
      string _ctText = String.Empty;
      switch (clearingType)
      {
        case ClearingType.PartialWindingUp:
          _ctText = GetParameter(entities, SettingsEntry.ClearingTypePartialWindingUpText);
          break;
        case ClearingType.TotalWindingUp:
          _ctText = GetParameter(entities, SettingsEntry.ClearingTypeTotalWindingUpText);
          break;
      }
      return String.Format(GetParameter(entities, SettingsEntry.GoodsDescription_Format), tobaccoName, grade, sku, batch, _ctText, documentNo);
    }
    public static string SADCollectionStylesheetName = "SADCollectionStylesheet";
    #endregion

    #region private
    private const string c_documentNumberFormat = "{0:D7}";
    private static Dictionary<SettingsEntry, string> m_DefaultSettings = new Dictionary<SettingsEntry, string>()
    {
       { SettingsEntry.GoodsDescription_CWQuantity_Pattern,  @"(?<=\bBATCH:)\D*\d*\D*(\d*[.,,]\d*)\W*\w*\D*\d*\W*CT" },
       { SettingsEntry.GoodsDescription_CWPackageUnits_Pattern, @"(?<=\bBATCH:)\D*\d*\D*\d*[.,,]\d*\W*\w*\D*(\d*)\W*CT" },
       { SettingsEntry.GoodsDescription_Units_Pattern,  @"(?<=\bBATCH:)\D*\d*\D*\d*[.,,]\d*\W*(\w*)\D*\d*\W*CT" },
       { SettingsEntry.GoodsDescription_CertificateOfAuthenticity_Pattern,  @"\b([\w\d\s\.,-]*)/.*" },
       { SettingsEntry.GoodsDescription_Format, @"{0} GRADE:{1} SKU:{2} Batch:{3} {4} {5}" },
       { SettingsEntry.GoodsDescription_CertificateOfOrgin_Pattern, @"\b([\w\d\s\.,-]*)/.*" },
       { SettingsEntry.DefaultValidToDatePeriod, "730" },
       { SettingsEntry.LooselyFormatedDate, @"(?<=/)\D*(\d{1,2}).(\d{1,2}).(\d{4})" },
       { SettingsEntry.BinCardFileName, "Bin Card "+ c_documentNumberFormat },
       { SettingsEntry.ClearanceRequestFileName, "Wniosek o zamkniecie CW"+ c_documentNumberFormat },
       { SettingsEntry.CustomsWarehouseReportFileName, "Raport CW"+ c_documentNumberFormat },
       { SettingsEntry.OrganizationEmail, "gstmaan@jti.com" },
       { SettingsEntry.SADTemplateDocumentNamePattern, "CW Wyprowadzenie " + c_documentNumberFormat },
       { SettingsEntry.ClearanceTitleFormatCW, "Nr: {3:D5} {0}/{1} SAD: {2} " },
       { SettingsEntry.RecipientOrganization, 
          @"{""EORI"":""PL828000181900000"",""Id"":1,""Kod"":""99-220"",""Kraj"":""PL"",""Miejscowosc"":""WARTKOWICE"",""Nazwa"":""JTI POLSKA SP. Z O.O."",""Pesel"":null,""Regon"":""00130199100000"",""TIN"":""PL8280001819"",""UlicaNr"":""GOSTKOW STARY 42""}"},
       { SettingsEntry.DefaultCustomsOffice, 
          @"{""Lokalizacja"":{""Miejsce"":""PL360000SC0002"",""Opis"":null,""UC"":null},""SkladCelny"":{""Kraj"":""PL"",""Miejsce"":""PL360000SC0002"",""Typ"":""C""},""UCGraniczny"":""PL362010"",""UCKontrolny"":null,""UCPrzeznaczenia"":null,""UCTranzytowy"":null,""UCZgloszenia"":""PL362010""}" },
       { SettingsEntry.ClearingTypePartialWindingUpText, "CZESCIOWA LIKWIDACJA"},
       { SettingsEntry.ClearingTypeTotalWindingUpText, "CALKOWITA LIKWIDACJA"},
       { SettingsEntry.StatementDocumentNameFileNamePattern, "Zestawienie Naleznosci " + c_documentNumberFormat }
    };
    #endregion
  }
  public enum SettingsEntry
  {
    GoodsDescription_CWQuantity_Pattern,
    GoodsDescription_CWPackageUnits_Pattern,
    GoodsDescription_Units_Pattern,
    GoodsDescription_CertificateOfAuthenticity_Pattern,
    GoodsDescription_CertificateOfOrgin_Pattern,
    GoodsDescription_Format,
    DefaultValidToDatePeriod,
    LooselyFormatedDate,
    BinCardFileName,
    ClearanceRequestFileName,
    CustomsWarehouseReportFileName,
    OrganizationEmail,
    SADTemplateDocumentNamePattern,
    ClearanceTitleFormatCW,
    RecipientOrganization,
    DefaultCustomsOffice,
    ClearingTypePartialWindingUpText,
    ClearingTypeTotalWindingUpText,
    StatementDocumentNameFileNamePattern
  }
}

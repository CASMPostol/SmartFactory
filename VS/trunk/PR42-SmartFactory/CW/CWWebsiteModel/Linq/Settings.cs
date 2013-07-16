using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Settings entity class
  /// </summary>
  public partial class Settings
  {
    public static string CustomsProcedureCodeA004 { get; set; }

    public static string CustomsProcedureCodeN865 { get; set; }

    public static string CustomsProcedureCodeN954 { get; set; }

  }

  internal enum SettingsEntry
  {
    GoodsDescription_CWPackageKg_Pattern,
    GoodsDescription_CWQuantity_CWPackageKg_Pattern,
    GoodsDescription_CWQuantity_Pattern,
    GoodsDescription_Units_Pattern,
    GoodsDescription_CertificateOfAuthenticity_Pattern,
    DefaultValidToDatePeriod
  }
}

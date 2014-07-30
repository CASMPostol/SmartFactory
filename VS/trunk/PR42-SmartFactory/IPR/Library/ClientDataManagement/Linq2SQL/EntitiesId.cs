﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{
  /// <summary>
  /// IItem
  /// </summary>
  public interface IItem
  {
    int ID { get; set; }
  }
  public partial class JSOXLibrary : IItem { }
  public partial class BalanceBatch : IItem { }
  public partial class SADDocumentLibrary : IItem { }
  public partial class SADDocument : IItem { }
  public partial class SADGood : IItem { }
  public partial class SADConsignment : IItem { }
  public partial class Clearence : IItem { }
  public partial class Consent : IItem { }
  public partial class PCNCode : IItem { }
  public partial class IPRLibrary : IItem { }
  public partial class IPR : IItem { }
  public partial class BalanceIPR : IItem { }
  public partial class BatchLibrary : IItem { }
  public partial class SPFormat : IItem { }
  public partial class SKULibrary : IItem { }
  public partial class SKU : IItem { }
  public partial class Batch : IItem { }
  public partial class CustomsUnion : IItem { }
  public partial class CutfillerCoefficient : IItem { }
  public partial class InvoiceLibrary : IItem { }
  public partial class InvoiceContent : IItem { }
  public partial class Material : IItem { }
  public partial class JSOXCustomsSummary : IItem { }
  public partial class Disposal : IItem { }
  public partial class Dust : IItem { }
  public partial class SADDuties : IItem { }
  public partial class SADPackage : IItem { }
  public partial class SADQuantity : IItem { }
  public partial class SADRequiredDocuments : IItem { }
  public partial class Settings : IItem { }
  public partial class SHMenthol : IItem { }
  public partial class StockLibrary : IItem { }
  public partial class StockEntry : IItem { }
  public partial class Usage : IItem { }
  public partial class Warehouse : IItem { }
  public partial class Waste : IItem { }
  public partial class History : IItem { }
  public partial class ArchivingLogs : IItem { }
  public partial class ActivitiesLogs : IItem { }
}

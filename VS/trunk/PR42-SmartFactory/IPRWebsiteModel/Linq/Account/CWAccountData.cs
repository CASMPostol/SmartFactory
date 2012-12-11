using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data
  /// </summary>
  public class CWAccountData: AccountData
  {

    public CWAccountData( Entities edc, SADGood good, MessageType messageType )
      : base( edc, good, messageType )
    {
      //TODO how to assign values. More info required.
    }
    internal CWLib CWLibraryID { get; private set; }
    internal string CWCertificate { get; private set; }
    internal double? CWMassPerPackage { get; private set; }
    internal double? CWPackageKg { get; private set; }
    internal double? CWPackageUnits { get; private set; }
    internal string CWPzNo { get; private set; }
    internal double? CWQuantity { get; private set; }
    internal DateTime? EntryDate { get; private set; }
    internal string Units { get; private set; }
    protected internal override Consent.CustomsProcess Process
    {
      get { return Consent.CustomsProcess.cw; }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  public class CWAccountData: AccountData
  {

    public CWAccountData( Entities edc, SADGood good, MessageType messageType )
      : base( edc, good, messageType )
    {

    }
    public CWLibraryCWLib cwLibraryID { get; set; }
    public string CWCertificate { get; set; }
    public double? CWMassPerPackage { get; set; }
    public double? CWPackageKg { get; set; }
    public double? CWPackageUnits { get; set; }
    public string CWPzNo { get; set; }
    public double? CWQuantity { get; set; }
    public DateTime? EntryDate { get; set; }
    public string Units { get; set; }
  }
}

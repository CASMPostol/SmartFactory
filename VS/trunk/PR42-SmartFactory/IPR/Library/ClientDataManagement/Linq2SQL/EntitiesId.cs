using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{
  public interface IItem
  {
    int ID { get; set; }
  }
  public partial class JSOXLibrary : IItem { }
  //BalanceBatch();
  //SADDocumentLibrary();
  //SADDocument();
  //SADGood();
  //SADConsignment();
  //Clearence();
  //Consent();
  //PCNCode();
  //IPRLibrary();
  //IPR();
  //BalanceIPR();
  //BatchLibrary();
  //SPFormat();
  //SKULibrary();
  //SKU();
  //Batch();
  //CustomsUnion();
  //CutfillerCoefficient();
  //InvoiceLibrary();
  //InvoiceContent();
  //Material();
  //JSOXCustomsSummary();
  //Disposal();
  //Dust();
  //SADDuties();
  //SADPackage();
  //SADQuantity();
  //SADRequiredDocuments();
  //Settings();
  //SHMenthol();
  //StockLibrary();
  //StockEntry();
  //Usage();
  //Warehouse();
  //Waste();
  //History();
  //ArchivingLogs();
  //ActivitiesLogs();
}

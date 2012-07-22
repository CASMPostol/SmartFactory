using CAS.SmartFactory.Linq.IPR;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts
{

  internal enum ConnectionSelector
  {
    BatchInterconnection,
    InvoiceInterconnection,
    InvoiceContentInterconnection
  }
  internal abstract class InterconnectionData<DerivedType>: CAS.SharePoint.Web.InterconnectionData<DerivedType>
      where DerivedType: InterconnectionData<DerivedType>
  {
    internal string ID { get { return GetFieldValue( Element.IDColunmName ); } }
    internal string Title { get { return GetFieldValue( Element.TitleColunmName ); } }
  }
  internal class BatchInterconnectionData: InterconnectionData<BatchInterconnectionData>
  {
    internal BatchInterconnectionData()
      : base()
    { }
  }
  internal class InvoiceInterconnectionData: InterconnectionData<InvoiceInterconnectionData>
  {
    internal InvoiceInterconnectionData()
      : base()
    { }
  }
  internal class InvoiceContentInterconnectionnData: InterconnectionData<InvoiceContentInterconnectionnData>
  {
    internal InvoiceContentInterconnectionnData()
      : base()
    { }
  }
}

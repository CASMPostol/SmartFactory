using CAS.SmartFactory.Linq.IPR;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts
{

  internal enum ConnectionSelector
  {
    BatchInterconnection,
    InvoiceInterconnection,
    InvoiceContentInterconnection,
    ClearenceInterconnection
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
      : base() { }
    internal bool ReadOnly
    {
      get
      {
        string _value = GetFieldValue( "InvoiceLibraryReadOnly" );
        return _value.IsNullOrEmpty() ? false : !_value.Contains( "0" ); // it is returned as number not bool 
      }
    }
  }
  internal class InvoiceContentInterconnectionnData: InterconnectionData<InvoiceContentInterconnectionnData>
  {
    internal InvoiceContentInterconnectionnData()
      : base()
    { }
  }
  internal class ClearenceInterconnectionnData: InterconnectionData<ClearenceInterconnectionnData>
  {
    internal ClearenceInterconnectionnData()
      : base()
    { }
  }
}

using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Linq.IPR;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts
{

  internal enum ConnectionSelector
  {
    BatchInterconnection,
    InvoiceInterconnection,
    //ShippingInterconnection,
    //TimeSlotInterconnection,
    //PartnerInterconnection,
    //CityInterconnection,
    //DriverInterconnection,
    //RouteInterconnection,
    //SecurityEscortCatalogInterconnection
  }
  internal abstract class InterconnectionData<DerivedType>: CAS.SharePoint.Web.InterconnectionData<DerivedType>
      where DerivedType: InterconnectionData<DerivedType>

  {
    internal string ID { get { return GetFieldValue( Element.IDColunmName ); } }
    internal string Title { get { return GetFieldValue( Element.TitleColunmName ); } }
  }
  //internal class TrailerInterconnectionData : InterconnectionData<TrailerInterconnectionData>
  //{
  //  internal TrailerInterconnectionData()
  //    : base()
  //  { }
  //}
}

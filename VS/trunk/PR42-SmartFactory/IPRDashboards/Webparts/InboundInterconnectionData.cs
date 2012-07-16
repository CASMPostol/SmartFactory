using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Linq.IPR;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard
{

  internal abstract class InterconnectionData<DerivedType> : CAS.SharePoint.Web.InterconnectionData<DerivedType>
      where DerivedType: InterconnectionData<DerivedType>

  {
    internal enum ConnectionSelector
    {
      //TrailerInterconnection,
      //TruckInterconnection,
      //ShippingInterconnection,
      //TimeSlotInterconnection,
      //PartnerInterconnection,
      //CityInterconnection,
      //DriverInterconnection,
      //RouteInterconnection,
      //SecurityEscortCatalogInterconnection
    }
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

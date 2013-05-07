using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard
{
  internal class InboundInterconnectionData : EventArgs
  {
    internal enum ConnectionSelector
    {
      TrailerInterconnection,
      TruckInterconnection,
      ShippingInterconnection,
      TimeSlotInterconnection,
      PartnerInterconnection,
      CityInterconnection,
      DriverInterconnection,
      RouteInterconnection,
      SecurityEscortCatalogInterconnection
    }
    internal string ID { get { return GetFieldValue(Element.IDColunmName); } }
    internal string Title { get { return GetFieldValue(Element.TitleColunmName); } }
    protected virtual DataRowView Row { get; set; }
    protected string GetFieldValue(string _name)
    {
      if (Row == null)
        return String.Empty;
      string _val = Row[_name] as String;
      if (_val == null)
        return String.Empty;
      return _val;
    }
  }
  internal abstract class InterconnectionData<DerivedType> : InboundInterconnectionData
    where DerivedType : InterconnectionData<DerivedType>
  {
    internal InterconnectionData() { }
    private EventHandler<DerivedType> m_Update;
    internal void SetRowData(IWebPartRow _connector, EventHandler<DerivedType> _update)
    {
      m_Update = _update;
      _connector.GetRowData(SetData);
    }
    private void SetData(object _data)
    {
      Row = _data as DataRowView;
      m_Update(this, (DerivedType)this);
    }
  }
  internal class TrailerInterconnectionData : InterconnectionData<TrailerInterconnectionData>
  {
    internal TrailerInterconnectionData()
      : base()
    { }
  }
  internal class TruckInterconnectionData : InterconnectionData<TruckInterconnectionData>
  {
    internal TruckInterconnectionData()
      : base()
    { }
  }
  internal class DriverInterconnectionData : InterconnectionData<DriverInterconnectionData>
  {
    public DriverInterconnectionData()
      : base()
    { }
  }
  internal class ShippingInterconnectionData : InterconnectionData<ShippingInterconnectionData>
  {
    public ShippingInterconnectionData()
      : base()
    { }
    internal DateTime EstimateDeliveryTime
    {
      get
      {
        string _edt = GetFieldValue("EstimateDeliveryTime");
        return DateTime.Parse(_edt);
      }
    }
  }
  internal class TimeSlotInterconnectionData : InterconnectionData<TimeSlotInterconnectionData>
  {
    public TimeSlotInterconnectionData()
      : base()
    { }
    internal bool IsDouble { get { return Boolean.Parse(GetFieldValue(TimeSlotTimeSlot.NameOfIsDouble)); } }
  }
  internal class PartnerInterconnectionData : InterconnectionData<PartnerInterconnectionData>
  { }
  internal class CityInterconnectionData : InterconnectionData<CityInterconnectionData>
  { }
  internal class RouteInterconnectionnData : InterconnectionData<RouteInterconnectionnData>
  { }
  internal class SecurityEscortCatalogInterconnectionData : InterconnectionData<SecurityEscortCatalogInterconnectionData>
  { }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  internal class InterconnectionDataBase : EventArgs
  {
    internal enum ConnectionSelector
    {
      TrailerInterconnection, TruckInterconnection, ShippingInterconnection, TimeSlotInterconnection, WarehouseInterconnection
    }
  }
  internal abstract class InterconnectionData<DerivedType> : InterconnectionDataBase
    where DerivedType : InterconnectionData<DerivedType>
  {
    internal InterconnectionData() { }
    internal void GetRowData(IWebPartRow _connector, EventHandler<DerivedType> _update)
    {
      m_Update = _update;
      _connector.GetRowData(GetData);
    }
    internal string ID { get { return GetFieldValue("ID"); } }
    internal string Title { get { return GetFieldValue("Title"); } }
    protected DataRowView Row { get; private set; }
    protected string GetFieldValue(string _name)
    {
      if (Row == null)
        return String.Empty;
      string _val = Row[_name] as String;
      if (_val == null)
        return String.Empty;
      return _val;
    }
    private EventHandler<DerivedType> m_Update;
    private void GetData(object _data)
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
    internal string TruckCarRegistrationNumber { get { return GetFieldValue("TruckTitle"); } }
    internal string TrailerRegistrationNumber { get { return GetFieldValue("TrailerTitle"); } }
    internal string Warehouse { get { return GetFieldValue("ShippingPointTitle"); } }
    internal string StartTime
    {
      get
      {
        return DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
      }
    }
  }
  internal class TimeSlotInterconnectionData : InterconnectionData<TimeSlotInterconnectionData>
  {
    public TimeSlotInterconnectionData()
      : base()
    { }
    internal string GetDate { get { return GetFieldValue("EventDate"); } }
  }
  //WarehouseInterconnectionData
  internal class WarehouseInterconnectionData : InterconnectionData<WarehouseInterconnectionData>
  {
    public WarehouseInterconnectionData()
      : base()
    { }
  }
}

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
    internal string ID
    {
      get
      {
        if (Row == null)
          return String.Empty;
        string _val = Row["ID"] as String;
        if (_val == null)
          return String.Empty;
        return _val;
      }
    }
    internal string Title
    {
      get
      {
        if (Row == null)
          return String.Empty;
        try
        {
          return (string)Row["Title"];
        }
        catch (Exception)
        {
          return String.Empty;
        }
      }
    }
    protected DataRowView Row { get; private set; }
    private EventHandler<DerivedType> m_Update;
    private void GetData(object _data)
    {
      Row = _data as DataRowView;
      m_Update(this, (DerivedType)this );
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
  }
  internal class TimeSlotInterconnectionData : InterconnectionData<TimeSlotInterconnectionData>
  {
    public TimeSlotInterconnectionData()
      : base()
    { }
    internal string GetDate()
    {
      return String.Empty;
    }
  }
  //WarehouseInterconnectionData
  internal class WarehouseInterconnectionData : InterconnectionData<WarehouseInterconnectionData>
  {
    public WarehouseInterconnectionData()
      : base()
    { }
  }
}

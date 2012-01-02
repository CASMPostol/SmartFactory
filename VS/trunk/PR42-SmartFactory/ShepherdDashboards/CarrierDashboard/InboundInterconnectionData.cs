﻿using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard
{
  internal class InboundInterconnectionData : EventArgs
  {
    internal enum ConnectionSelector
    {
      TrailerInterconnection, TruckInterconnection, ShippingInterconnection, TimeSlotInterconnection, PartnerInterconnection, DriverInterconnection
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
    internal void GetRowData(IWebPartRow _connector, EventHandler<DerivedType> _update)
    {
      m_Update = _update;
      _connector.GetRowData(GetData);
    }
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
  }
  internal class TimeSlotInterconnectionData : InterconnectionData<TimeSlotInterconnectionData>
  {
    public TimeSlotInterconnectionData()
      : base()
    { }
  }
  internal class PartnerInterconnectionData : InterconnectionData<PartnerInterconnectionData>
  {

  }
  internal static class InterconnectionExtensions
  {
    public static int? GetIndex(this InboundInterconnectionData _id)
    {
      if (_id == null) return null;
      return _id.ID.String2Int();
    }
  }
}

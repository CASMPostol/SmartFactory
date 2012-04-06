using System;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using System.Web.UI.WebControls;

namespace CAS.SmartFactory.Shepherd.SendNotification
{
  public static class Extensions
  {
    public static string Cast2String(this object value)
    {
      return value == null ? String.Empty : (string)value;
    }
    public static string Title(this Element _val)
    {
      return _val == null ? "N/A" : _val.Tytuł;
    }
    public static string UnknownIfEmpty(this String _val)
    {
      return String.IsNullOrEmpty(_val) ? CommonDefinition.UnknownEmail : _val;
    }
    public static string ToMonthString(this DateTime _dateTime)
    {
      return new DateTime(_dateTime.Year, _dateTime.Month, 1).ToShortDateString();
    }
    public static int String2IntOrDefault(this string _val, int _default)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return _default;
    }
    public static void AddTextAndValue(this DropDownList _list, int i)
    {
      _list.Items.Add(new ListItem(i.ToString(), i.ToString()));
    }
    public static string NotAvailable(this string _value)
    {
      return String.IsNullOrEmpty(_value) ? "N/A" : _value;
    }
    public static int Hour2Int(this StartHour _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int Hour2Int(this EndHour _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int Minute2Int(this StartMinute _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int Minute2Int(this EndMinute _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int ParseDashed(this string _val)
    {
      return Int16.Parse(_val.Replace("_", ""));
    }
  }
}

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint.Utilities;

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
      return _val == null ? "NotApplicable".GetLocalizedString() : _val.Tytuł;
    }
    public static string ToMonthString(this DateTime _dateTime)
    {
      return new DateTime(_dateTime.Year, _dateTime.Month, 1).ToString("yyyy-MM");
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
      return String.IsNullOrEmpty(_value) ? "NotApplicable".GetLocalizedString() : _value;
    }
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    public static int Hour2Int(this TimeSlotsTemplateStartHour _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int Hour2Int(this TimeSlotsTemplateEndHour _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int Minute2Int(this TimeSlotsTemplateStartMinute _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int Minute2Int(this TimeSlotsTemplateEndMinute _valu)
    {
      return _valu.ToString().ParseDashed();
    }
    public static int ParseDashed(this string _val)
    {
      return Int16.Parse(_val.Replace("_", ""));
    }
    public static double? String2Double(this string _val)
    {
      double _res = 0;
      if (double.TryParse(_val, out _res))
        return _res;
      else
        return null;
    }
    /// <summary>
    /// Gets the localized string.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    public static string GetLocalizedString(this string val)
    {
      string _frmt = "$Resources:{0}";
      return SPUtility.GetLocalizedString(String.Format(_frmt, val), RootResourceFileName, (uint)CultureInfo.CurrentUICulture.LCID);
    }
    public static string GetLocalizationExpresion(this string val)
    {
      string _frmt = "$Resources:{0},{1}";
      return String.Format(_frmt, RootResourceFileName, val);
    }
    internal const string RootResourceFileName = "CASSmartFactoryShepherdCode";
  }
}

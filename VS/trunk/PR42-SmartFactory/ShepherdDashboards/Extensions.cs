using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Drawing;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public static class Extensions
  {
    public static string ToString(this int? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    public static string ToString(this DateTime? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    public static string ToString(this DateTime? _val, string _format)
    {
      return _val.HasValue ? string.Format(_format, _val.Value.ToString()) : String.Empty;
    }
    public static string ControlTextProperty(this string _val)
    {
      return String.IsNullOrEmpty(_val) ? " -- Select from list -- " : _val;
    }
    public static void TextBoxTextProperty(this TextBox _control, string _val, bool _required)
    {
      _control.Text = _val;
      if (String.IsNullOrEmpty(_val))
        _control.BackColor = _required ? _warrningBackColor : Color.DimGray;
      else
        _control.BackColor = Color.Empty;
    }
    public static void LabelTextProperty(this Label _control, string _val, bool _required)
    {
      if (String.IsNullOrEmpty(_val))
      {
        _control.Text = " -- Select from list -- ";
        _control.BackColor = _required ? _warrningBackColor : Color.DimGray;
      }
      else
      {
        _control.Text = _val;
        _control.BackColor = Color.Empty;
      }
    }
    public static int? HiddenField2Int(this HiddenField _control)
    {
      int _ret;
      if (int.TryParse(_control.Value, out _ret))
      {
        return _ret;
      }
      return null;
    }
    public static int? String2Int(this string _val)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return null;
    }
    private static Color _warrningBackColor = Color.Tomato;
  }
}

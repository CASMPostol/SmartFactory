using System;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public static class Extensions
  {
    #region public

    public static string IntToString(this int? _val)
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
    /// <summary>
    /// Sets the text property of <see cref="TextBox "/> control.
    /// </summary>
    /// <param name="_control">The control.</param>
    /// <param name="_val">The value.</param>
    /// <param name="_required">if set to <c>true</c> the property is requires.</param>
    public static void TextBoxTextProperty(this TextBox _control, string _val, bool _required)
    {
      _control.Text = _val;
      if (String.IsNullOrEmpty(_val))
        _control.BackColor = _required ? _warrningBackColor : Color.Azure;
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
    public static double? TextBox2Double(this TextBox _value, List<string> _errors)
    {
      string _trimed = _value.Text.Trim();
      if (_trimed.IsNullOrEmpty())
        return null;
      double _dv;
      if (Double.TryParse(_trimed, out _dv))
        return _dv;
      _errors.Add(String.Format("Wrong value of {0}.", _value.Text));
      return null;
    }
    public static int? String2Int(this string _val)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return new Nullable<int>();
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
    ///  Indicates whether the specified System.String object is null or an System.String.Empty string.
    /// </summary>
    /// <param name="_val"> A System.String reference.</param>
    /// <returns>
    ///   true if the value parameter is null or an empty string (""); otherwise, false.</c>.
    /// </returns>
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    public static void Select(this DropDownList _ddl, Element _row)
    {
      if (_row == null)
        return;
      _ddl.Select(_row.Identyfikator.Value);
    }
    public static void Select(this DropDownList _ddl, int _row)
    {
      _ddl.SelectedIndex = -1;
      ListItem _li = _ddl.Items.FindByValue(_row.ToString());
      if (_li == null)
        return;
      _li.Selected = true;
    }
    public static void AddPartner(this DropDownList _ddl, bool _all, Partner _Partner, EntitiesDataContext EDC)
    {
      if (_all)
      {
        _ddl.DataSource = from Partner _prtnrs in EDC.Partner
                          select new { Name = _prtnrs.Tytuł, ID = _prtnrs.Identyfikator.Value.ToString() };
        _ddl.DataValueField = "ID";
        _ddl.DataTextField = "Name";
        _ddl.DataBind();
        if (_Partner == null)
          _ddl.SelectedIndex = -1;
        else
          _ddl.Select(_Partner);
      }
      else
        _ddl.Items.Add(new ListItem(_Partner.Tytuł, _Partner.Identyfikator.Value.ToString()));
    }
    #endregion

    #region private

    private static Color _warrningBackColor = Color.MintCream;
    #endregion
  }
}

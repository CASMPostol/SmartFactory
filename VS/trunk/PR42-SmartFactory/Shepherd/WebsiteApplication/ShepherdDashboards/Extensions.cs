//<summary>
//  Title   : static class Extensions
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  /// <summary>
  /// Extensions
  /// </summary>
  public static class Extensions
  {
    #region public

    /// <summary>
    /// Convert int to string.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string IntToString(this int? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents <see cref="DateTime"/>.
    /// </summary>
    /// <param name="_val">The value to convert</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString(this DateTime? _val)
    {
      return _val.HasValue ? _val.Value.ToString(CultureInfo.CurrentCulture) : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <param name="_format">The _format.</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString(this DateTime? _val, string _format)
    {
      return _val.HasValue ? string.Format(_format, _val.Value.ToString(CultureInfo.CurrentCulture)) : String.Empty;
    }
    ///// <summary>
    ///// Labels the text property.
    ///// </summary>
    ///// <param name="_control">The _control.</param>
    ///// <param name="_val">The _val.</param>
    ///// <param name="_required">if set to <c>true</c> [_required].</param>
    //public static void LabelTextProperty(this Label _control, string _val, bool _required)
    //{
    //  if (String.IsNullOrEmpty(_val))
    //  {
    //    _control.Text = " -- Select from list -- ";
    //    _control.BackColor = _required ? _warrningBackColor : Color.DimGray;
    //  }
    //  else
    //  {
    //    _control.Text = _val.GetLocalizedString();
    //    _control.BackColor = Color.Empty;
    //  }
    //}
    /// <summary>
    /// Hiddens the field2 int.
    /// </summary>
    /// <param name="_control">The _control.</param>
    /// <returns></returns>
    public static int? HiddenField2Int(this HiddenField _control)
    {
      int _ret;
      if (int.TryParse(_control.Value, out _ret))
      {
        return _ret;
      }
      return null;
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
    /// <summary>
    /// Texts the box2 double.
    /// </summary>
    /// <param name="_value">The _value.</param>
    /// <param name="_errors">The _errors.</param>
    /// <returns></returns>
    public static double? TextBox2Double(this TextBox _value, List<string> _errors)
    {
      string _trimed = _value.Text.Trim();
      if (_trimed.IsNullOrEmpty())
        return null;
      double _dv;
      if (Double.TryParse(_trimed, NumberStyles.Any, CultureInfo.CurrentCulture, out _dv))
        return _dv;
      _errors.Add(String.Format("WrongValue".GetShepherdLocalizedString(), _value.Text));
      return null;
    }

    #region string
    /// <summary>
    /// Gets the localized string.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    public static string GetShepherdLocalizedString(this string val)
    {
      string _frmt = "$Resources:{0}";
      return SPUtility.GetLocalizedString(String.Format(_frmt, val), GlobalDefinitions.ShepherdResourceFileName, (uint)CultureInfo.CurrentCulture.LCID);
    }
    /// <summary>
    /// Gets the shepherd localization expresion.
    /// </summary>
    /// <param name="val">The value to be localized.</param>
    /// <returns></returns>
    public static string GetShepherdLocalizationExpresion(this string val)
    {
      string _frmt = "$Resources:{0},{1}";
      return String.Format(_frmt, GlobalDefinitions.ShepherdResourceFileName, val);
    }
    /// <summary>
    /// Controls the text property.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string ControlTextProperty(this string _val)
    {
      return String.IsNullOrEmpty(_val) ? "SelectFromList".GetShepherdLocalizedString() : _val;
    }
    /// <summary>
    /// String2s the int.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static int? String2Int(this string _val)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return new Nullable<int>();
    }
    /// <summary>
    /// String2s the double.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
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
    ///   true if the value parameter is null or an empty string (""); otherwise, false.
    /// </returns>
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    #endregion

    #region DropDownList
    /// <summary>
    /// Selects the specified _DDL.
    /// </summary>
    /// <param name="_ddl">The _DDL.</param>
    /// <param name="_row">The _row.</param>
    public static void Select(this DropDownList _ddl, Element _row)
    {
      if (_row == null)
        return;
      _ddl.Select(_row.Id.Value);
    }
    /// <summary>
    /// Selects the specified _DDL.
    /// </summary>
    /// <param name="_ddl">The _DDL.</param>
    /// <param name="_row">The _row.</param>
    public static void Select(this DropDownList _ddl, int _row)
    {
      _ddl.SelectedIndex = -1;
      ListItem _li = _ddl.Items.FindByValue(_row.ToString());
      if (_li == null)
        return;
      _li.Selected = true;
    }
    /// <summary>
    /// Adds the partner.
    /// </summary>
    /// <param name="_ddl">The _DDL.</param>
    /// <param name="_all">if set to <c>true</c> [_all].</param>
    /// <param name="_Partner">The _ partner.</param>
    /// <param name="EDC">The EDC.</param>
    public static void AddPartner(this DropDownList _ddl, bool _all, Partner _Partner, EntitiesDataContext EDC)
    {
      if (_all)
      {
        _ddl.DataSource = from Partner _prtnrs in EDC.Partner
                          select new { Name = _prtnrs.Title, ID = _prtnrs.Id.Value.ToString() };
        _ddl.DataValueField = "ID";
        _ddl.DataTextField = "Name";
        _ddl.DataBind();
        if (_Partner == null)
          _ddl.SelectedIndex = -1;
        else
          _ddl.Select(_Partner);
      }
      else
        _ddl.Items.Add(new ListItem(_Partner.Title, _Partner.Id.Value.ToString()));
    }
    #endregion

    #region DateTimeControl
    /// <summary>
    /// Sets the time picker.
    /// </summary>
    /// <param name="control">The control to be setup.</param>
    /// <param name="date">The date.</param>
    internal static void SetTimePicker(this DateTimeControl control, DateTime? date)
    {
      if (date.HasValue)
        control.SelectedDate = date.Value;
      else
      {
        control.IsValid = false;
        control.SelectedDate = SPContext.Current.Web.CurrentUser.RegionalSettings.TimeZone.UTCToLocalTime(System.DateTime.UtcNow);
      }
    }
    #endregion

    #endregion

    #region private
    private static Color _warrningBackColor = Color.MintCream;
    #endregion
  }
}

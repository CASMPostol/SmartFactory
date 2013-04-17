using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.Entities;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.Utilities;

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
      return _val.HasValue ? _val.Value.ToString(CultureInfo.CurrentUICulture) : String.Empty;
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
      return _val.HasValue ? string.Format(_format, _val.Value.ToString(CultureInfo.CurrentUICulture)) : String.Empty;
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
      if (Double.TryParse(_trimed, NumberStyles.Any, CultureInfo.CurrentUICulture, out _dv))
        return _dv;
      _errors.Add(String.Format("WrongValue".GetLocalizedString(), _value.Text));
      return null;
    }

    #region string
    /// <summary>
    /// Gets the localized string.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    public static string GetLocalizedString(this string val)
    {
      string _frmt = "$Resources:{0}";
      return SPUtility.GetLocalizedString(String.Format(_frmt, val), GlobalDefinitions.RootResourceFileName, (uint)CultureInfo.CurrentUICulture.LCID);
    }
    public static string GetLocalizationExpresion(this string val)
    {
      string _frmt = "$Resources:{0},{1}";
      return String.Format(_frmt, GlobalDefinitions.RootResourceFileName, val);
    }
    /// <summary>
    /// Controls the text property.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string ControlTextProperty(this string _val)
    {
      return String.IsNullOrEmpty(_val) ? "SelectFromList".GetLocalizedString() : _val;
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
    ///   true if the value parameter is null or an empty string (""); otherwise, false.</c>.
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
      _ddl.Select(_row.Identyfikator.Value);
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

    /// <summary>
    /// Resolves the change conflicts.
    /// </summary>
    /// <param name="_EDC">The _ EDC.</param>
    /// <param name="_rsult">The _rsult.</param>
    public static void ResolveChangeConflicts(this EntitiesDataContext _EDC, ActionResult _rsult)
    {
      string _cp = "Starting";
      try
      {
        foreach (ObjectChangeConflict _itx in _EDC.ChangeConflicts)
        {
          _cp = "ObjectChangeConflict";
          string _tmp = String.Format("Object: {0}", _itx.Object == null ? "null" : _itx.Object.ToString());
          if (_itx.MemberConflicts != null)
          {
            string _ft = ", Conflicts: Member.Name={0}; CurrentValue={1}; DatabaseValue={2}; OriginalValue={3}";
            String _chnges = String.Empty;
            foreach (MemberChangeConflict _mid in _itx.MemberConflicts)
            {
              _chnges += String.Format(_ft,
                _mid.Member == null ? "null" : _mid.Member.Name,
                _mid.CurrentValue == null ? "null" : _mid.CurrentValue.ToString(),
                _mid.DatabaseValue == null ? "null" : _mid.DatabaseValue.ToString(),
                _mid.OriginalValue == null ? "null" : _mid.OriginalValue.ToString());
            }
            _tmp += _chnges;
          }
          else
            _tmp += "; No member details";
          _rsult.AddMessage(_tmp);
          _cp = "AddMessage";
          _itx.Resolve(RefreshMode.KeepCurrentValues);
        } //foreach (ObjectChangeConflict
      }
      catch (Exception ex)
      {
        string _frmt = "The current operation has been interrupted in ResolveChangeConflicts at {0} by error {1}.";
        throw new ApplicationException(String.Format(_frmt, _cp, ex.Message));
      }
    }
    #endregion

    #region private
    private static Color _warrningBackColor = Color.MintCream;
    #endregion
  }
}

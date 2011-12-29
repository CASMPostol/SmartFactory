using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public class InterconnectionDataTable<T> : DataTable, IWebPartRow
    where T : class
  {
    #region IWebPartRow
    public PropertyDescriptorCollection Schema { get; private set; }
    public void GetRowData(RowCallback callback)
    {
      callback(this.Row0);
    }
    #endregion

    #region public
    public InterconnectionDataTable() : this("Schema of " + typeof(T).Name) { }
    public InterconnectionDataTable(string _TableName) : this(null, _TableName) { }
    public InterconnectionDataTable(IEnumerable<T> items, string _TableName)
    {
      List<PropertyInfo> props = CreateSchema(_TableName);
      if (items != null)
        foreach (var item in items)
        {
          DataRow _nr = this.NewRow();
          foreach (PropertyInfo _prop in props)
          {
            try
            {
              object _cv = _prop.GetValue(item, null);
              string _strVal;
              if (_cv == null)
                _strVal = String.Empty;
              else
              {
                Element _el = _cv as Element;
                if (_el != null)
                  _strVal = _el.Identyfikator.Value.ToString();
                else
                  _strVal = _cv.ToString();
              }
              _nr[_prop.Name] = _strVal;
            }
            catch (Exception)
            {
              _nr[_prop.Name] = String.Empty;
            }
          }
          this.Rows.Add(_nr);
        }
      if (this.Rows.Count == 0)
      {
        DataRow _nr = this.NewRow();
        foreach (DataColumn _col in this.Columns)
          _nr[_col.ColumnName] = String.Empty;
        this.Rows.Add(_nr);
      }
      this.Schema = TypeDescriptor.GetProperties(Row0);
    }
    #endregion

    #region private
    private List<PropertyInfo> CreateSchema(string _TableName)
    {
      PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      List<PropertyInfo> _ret = new List<PropertyInfo>();
      foreach (PropertyInfo prop in props)
      {
        if (IsRemoved(prop.GetCustomAttributes(true)))
          continue;
        this.Columns.Add(prop.Name, typeof(String));
        _ret.Add(prop);
      }
      this.TableName = _TableName;
      return _ret;
    }
    private static bool IsRemoved(object[] _attributes)
    {
      foreach (var item in _attributes)
      {
        if (item.GetType() == typeof(Microsoft.SharePoint.Linq.RemovedColumnAttribute))
          return true;
      }
      return false;
    }
    /// <summary>
    /// Determine of specified type is nullable
    /// </summary>
    private DataRowView Row0 { get { return this.Rows.Count == 0 ? null : DefaultView[0]; } }
    #endregion
  }
}

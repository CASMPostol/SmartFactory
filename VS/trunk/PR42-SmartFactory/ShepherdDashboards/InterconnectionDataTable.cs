using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint.Linq;

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
    public InterconnectionDataTable(IEnumerable<T> _list, string _tableName)
    {
      Dictionary<string, PropertyInfo> _properties = CreateSchema(_tableName);
      if (_list != null)
        foreach (var item in _list)
        {
          DataRow _row = this.NewRow();
          foreach (var _cp in _properties)
          {
            try
            {
              object _cv = _cp.Value.GetValue(item, null);
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
              _row[_cp.Key] = _strVal;
            }
            catch (Exception)
            {
              _row[_cp.Key] = String.Empty;
            }
          }
          this.Rows.Add(_row);
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
    private Dictionary<string, PropertyInfo> CreateSchema(string _TableName)
    {
      PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      Dictionary<string, PropertyInfo> _ret = new Dictionary<string, PropertyInfo>();
      foreach (PropertyInfo prop in props)
      {
        object[] _attributes = prop.GetCustomAttributes(false);
        bool _isRemoved = false;
        string _ColName = prop.Name;
        foreach (var item in _attributes)
        {
          if (item is RemovedColumnAttribute)
          {
            _isRemoved = true;
            break;
          }
          if (item is ColumnAttribute)
            _ColName = ((ColumnAttribute)item).Name;
        }
        if (_isRemoved)
          continue;
        this.Columns.Add(_ColName, typeof(String));
        _ret.Add(_ColName, prop);
      }
      this.TableName = _TableName;
      return _ret;
    }
    /// <summary>
    /// Determine of specified type is nullable
    /// </summary>
    private DataRowView Row0 { get { return this.Rows.Count == 0 ? null : DefaultView[0]; } }
    #endregion
  }
}

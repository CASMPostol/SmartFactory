//<summary>
//  Title   : InterconnectionDataTable DataTable
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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  /// <summary>
  /// InterconnectionDataTable DataTable
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class InterconnectionDataTable<T> : DataTable, IWebPartRow
    where T : class
  {
    #region IWebPartRow
    /// <summary>
    /// Gets the schema information for a data row that is used to share data between two <see cref="T:System.Web.UI.WebControls.WebParts.WebPart" /> controls.
    /// </summary>
    /// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> describing the data.</returns>
    public PropertyDescriptorCollection Schema { get; private set; }
    /// <summary>
    /// Gets the row of data.
    /// </summary>
    /// <param name="callback">The callback .</param>
    public void GetRowData(RowCallback callback)
    {
      if (m_DataReadyToSend)
        callback(this.Row0);
      else
        m_callback = callback;
    }
    #endregion

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="InterconnectionDataTable{T}"/> class.
    /// </summary>
    public InterconnectionDataTable() : this("Schema of " + typeof(T).Name) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InterconnectionDataTable{T}"/> class.
    /// </summary>
    /// <param name="_TableName">Name of the _ table.</param>
    public InterconnectionDataTable(string _TableName) : this(null, _TableName) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="InterconnectionDataTable{T}"/> class.
    /// </summary>
    /// <param name="_list">The _list.</param>
    /// <param name="_tableName">Name of the _table.</param>
    public InterconnectionDataTable(IEnumerable<T> _list, string _tableName)
    {
      CreateSchema(_tableName);
      DataRow _nr = this.NewRow();
      foreach (DataColumn _col in this.Columns)
        _nr[_col.ColumnName] = String.Empty;
      this.Rows.Add(_nr);
      this.Schema = TypeDescriptor.GetProperties(Row0);
      if (_list == null)
        return;
      SetData(this, new InterconnectionEventArgs(_list.ToList()[0]));
    }
    /// <summary>
    /// InterconnectionEventArgs
    /// </summary>
    public class InterconnectionEventArgs : System.EventArgs
    {
      /// <summary>
      /// Gets the data provided by the ecent.
      /// </summary>
      /// <value>
      /// The data to be returned.
      /// </value>
      public T Data { get; private set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="InterconnectionEventArgs"/> class.
      /// </summary>
      /// <param name="_data">The _data.</param>
      public InterconnectionEventArgs(T _data)
      {
        Data = _data;
      }
    }
    /// <summary>
    /// delegate SetDataEventArg used to represent operations that must be done after receiving new interconnection data. 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="_value">The <see cref="InterconnectionEventArgs"/> instance containing the event data.</param>
    public delegate void SetDataEventArg(object sender, InterconnectionEventArgs _value);
    /// <summary>
    /// Sets the data.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="_value">The _value to be set and sent to the .</param>
    public void SetData(object sender, InterconnectionEventArgs _value)
    {
      if (_value == null)
        throw new NullReferenceException();
      DataRow _row = this.Rows[0];
      foreach (var _cp in m_PropertiesInfo)
      {
        try
        {
          object _cv = _cp.Value.GetValue(_value.Data, null);
          string _strVal;
          if (_cv == null)
            _strVal = String.Empty;
          else
          {
            Element _el = _cv as Element;
            if (_el != null)
              _strVal = _el.Id.Value.ToString();
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
      m_DataReadyToSend = true;
      if (m_callback != null)
        m_callback(this.Row0);
    }
    #endregion

    #region private
    private RowCallback m_callback = null;
    private bool m_DataReadyToSend = false;
    private Dictionary<string, PropertyInfo> m_PropertiesInfo = new Dictionary<string, PropertyInfo>();
    private void CreateSchema(string _TableName)
    {
      PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
        m_PropertiesInfo.Add(_ColName, prop);
      }
      this.TableName = _TableName;
    }
    /// <summary>
    /// Determine of specified type is nullable
    /// </summary>
    private DataRowView Row0 { get { return DefaultView[0]; } }
    #endregion
  }
}

//<summary>
//  Title   : partial class Item
//  System  : Microsoft VisulaStudio 2013 / C#
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

using CAS.SharePoint.Client.Link2SQL;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// SharePoint Item Entity class
  /// </summary>
  public partial class Item
  {
    #region SharePoint fields access
    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    /// <value>
    /// The created.
    /// </value>
    [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Created", Storage = "_Created", ReadOnly = true, FieldType = "DateTime")]
    public System.Nullable<DateTime> Created
    {
      get
      {
        return this._Created;
      }
      set
      {
        if ((value != this._Created))
        {
          this.OnPropertyChanging("Created", this._id);
          this._Created = value;
          this.OnPropertyChanged("Created");
        }
      }
    }
    /// <summary>
    /// Gets or sets the author.
    /// </summary>
    /// <value>
    /// The author.
    /// </value>
    [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Author", Storage = "_Author", ReadOnly = true, FieldType = "SPFieldUserValue")]
    public string Author
    {
      get
      {
        return this._Author;
      }
      set
      {
        if ((value != this._Author))
        {
          this.OnPropertyChanging("Author", this._id);
          this._Author = value;
          this.OnPropertyChanged("Author");
        }
      }
    }
    /// <summary>
    /// Gets or sets the date of last modification.
    /// </summary>
    /// <value>
    /// The modified.
    /// </value>
    [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Modified", Storage = "_Modified", ReadOnly = true, FieldType = "DateTime")]
    public System.Nullable<DateTime> Modified
    {
      get
      {
        return this._Modified;
      }
      set
      {
        if ((value != this._Modified))
        {
          this.OnPropertyChanging("Modified", this._id);
          this._Modified = value;
          this.OnPropertyChanged("Modified");
        }
      }
    }
    /// <summary>
    /// Gets or sets the editor.
    /// </summary>
    /// <value>
    /// The editor.
    /// </value>
    [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Editor", Storage = "_Editor", ReadOnly = true, FieldType = "SPFieldUserValue")]
    public string Editor
    {
      get
      {
        return this._Editor;
      }
      set
      {
        if ((value != this._Editor))
        {
          this.OnPropertyChanging("Editor", this._id);
          this._Editor = value;
          this.OnPropertyChanged("Editor");
        }
      }
    }
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>
    /// The version.
    /// </value>
    [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "_UIVersionString", Storage = "_version0", ReadOnly = true, FieldType = "Text")]
    public string UIVersionString
    {
      get
      {
        return this._version0;
      }
      set
      {
        if ((value != this._Editor))
        {
          this.OnPropertyChanging("UIVersionString", this._id);
          this._version0 = value;
          this.OnPropertyChanged("UIVersionString");
        }
      }
    }
    #endregion

    #region internal
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    /// <returns></returns>
    internal static Dictionary<string, string> GetMappings()
    {
      return new Dictionary<string, string>() 
      {
        {"ID", "Id"},
        {"OnlySQL", ""} 
      };
    }
    internal static void Delete<TEntity>(EntityList<TEntity> list, IEnumerable<TEntity> entities, IEnumerable<IArchival> toBeMarkedAsArchival, Func<int, IItem> getSQLEntity, Action<int, string> addLog)
      where TEntity : Item, new()
    {
      foreach (TEntity _item in entities)
      {
        IItem _sqlItem = getSQLEntity(_item.Id.Value);
        _sqlItem.OnlySQL = true;
        addLog(_item.Id.Value, list.Name);
        list.DeleteOnSubmit(_item);
      }
      foreach (IArchival _ax in toBeMarkedAsArchival)
        _ax.Archival = true;
    }
    #endregion

    #region private
    private string _Editor;
    private System.Nullable<DateTime> _Created;
    private string _Author;
    private System.Nullable<DateTime> _Modified;
    private string _version0; 
    #endregion

  }
}

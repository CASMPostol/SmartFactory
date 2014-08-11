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

using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// SharePoint Item Entity class
  /// </summary>
  public partial class Item
  {
    /// <summary>
    /// Gets the mappings the key is SQL property name, the value is SP property name.
    /// </summary>
    /// <returns></returns>
    internal static Dictionary<string, string> GetMappings()
    {
      return new Dictionary<string, string>() 
      {
        {"ID", "Id"},
        {"Owshiddenversion", "Version"}
      };
    }
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
    private string _Editor;
    private System.Nullable<DateTime> _Created;
    private string _Author;
    private System.Nullable<DateTime> _Modified;
  }
}

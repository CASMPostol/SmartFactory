//<summary>
//  Title   : partial class Item
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  /// <summary>
  /// SharePoint Item Entity class
  /// </summary>
  public partial class Item
  {
    internal virtual Dictionary<string, string> GetMappings()
    {
      return new Dictionary<string, string>() 
      {
        {"Author", "CreatedBy"},
        {"Editor", "ModifiedBy"}
      };
    }
    private System.Nullable<DateTime> _Created;
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
    private string _Author;
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
    private System.Nullable<DateTime> _Modified;
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
    private string _Editor;
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
  }
}

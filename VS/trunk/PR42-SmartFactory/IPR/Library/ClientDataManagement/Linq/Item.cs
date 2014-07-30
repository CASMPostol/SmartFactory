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
      return new Dictionary<string, string>() { };
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

  }
}

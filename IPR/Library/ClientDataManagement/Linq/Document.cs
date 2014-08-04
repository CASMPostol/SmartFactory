//<summary>
//  Title   : partial class Document
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
  public partial class Document
  {
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("Name", "");
      _ret.Add("DocumentModifiedBy", "");
      _ret.Add("DocumentCreatedBy", "");
      return _ret;
    }
  }
}

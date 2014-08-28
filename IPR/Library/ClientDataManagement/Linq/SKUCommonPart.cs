//<summary>
//  Title   : public partial class SKUCommonPart
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

using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  public partial class SKUCommonPart
  {
    /// <summary>
    /// Gets the mappings, the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("SKU1", "SKU");
      return _ret;
    }
  }
}

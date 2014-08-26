//<summary>
//  Title   : public partial class SKUCigarette
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

using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  public partial class SKUCigarette
  {
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = SKUCommonPart.GetMappings();
      _ret.Add("BlendPurpose", "");
      _ret.Add("Units", "");
      _ret.Add("SKU1", "SKU");
      return _ret;
    }

  }
}

//<summary>
//  Title   : public partial class SKUCutfiller
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
  public partial class SKUCutfiller
  {
    /// <summary>
    /// Gets the mappings, the key is SQL property name, the value is SP property name.
    /// </summary>
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = SKUCommonPart.GetMappings();
      _ret.Add("Family", "");    
      _ret.Add("CigaretteLenght", "");
      _ret.Add("FilterLenght", "");
      _ret.Add("PrimeMarket", "");
      _ret.Add("MentholMaterial", "");
      _ret.Add("Menthol", "");
      _ret.Add("Brand", "");
      return _ret;
    }
  }
}

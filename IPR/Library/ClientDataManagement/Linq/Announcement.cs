//<summary>
//  Title   : partial class Announcement
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

  /// <summary>
  /// Announcement
  /// </summary>
  public partial class Announcement
  {
    internal new static Dictionary<string, string> GetMappings()
    {
      Dictionary<string, string> _ret = Item.GetMappings();
      _ret.Add("SPPropert", "SQLProperty");
      return _ret;
    }
  }
}

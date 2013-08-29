//<summary>
//  Title   : Vendor custom partial entity class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Vendor custom partial entity class
  /// </summary>
  public partial class Vendor
  {

    internal static Vendor FirstOrDefault( Entities entities )
    {
      return ( from _vx in entities.Vendor orderby _vx.Id ascending select _vx ).FirstOrDefault<Vendor>();
    }

  }
}

//<summary>
//  Title   : class IPRIngredient
//  System  : Microsoft Visual C# .NET 2012
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
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class IPRIngredient
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRIngredient"/> class.
    /// </summary>
    [Obsolete( "Use only for XML serializer" )]
    public IPRIngredient() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRIngredient"/> class.
    /// </summary>
    /// <param name="tobaccoBatch">The tobacco batch.</param>
    /// <param name="tobaccoSKU">The tobacco SKU.</param>
    /// <param name="quantity">The quantity.</param>
    public IPRIngredient( string tobaccoBatch, string tobaccoSKU, double quantity ) :
      base( tobaccoBatch, tobaccoSKU, quantity )
    { }
  }
}

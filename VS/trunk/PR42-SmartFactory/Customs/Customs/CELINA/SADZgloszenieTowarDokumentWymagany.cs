﻿//<summary>
//  Title   : partial class SADZgloszenieTowarDokumentWymagany
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD 
{
  /// <summary>
  /// partial class SADZgloszenieTowarDokumentWymagany
  /// </summary>
  public partial class SADZgloszenieTowarDokumentWymagany: RequiredDocumentsDescription
  {
    /// <summary>
    /// Gets the code.
    /// </summary>
    /// <returns></returns>
    public override string GetCode()
    {
      return this.Kod;
    }
    /// <summary>
    /// Gets the number.
    /// </summary>
    /// <returns></returns>
    public override string GetNumber()
    {
      return this.Nr;
    }
  }
}

//<summary>
//  Title   : partial class IE529ZwolnienieTowarDokumentWymagany
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

namespace CAS.SmartFactory.Customs.Messages.ECS
{
  /// <summary>
  /// partial class IE529ZwolnienieTowarDokumentWymagany
  /// </summary>
  public partial class IE529ZwolnienieTowarDokumentWymagany: RequiredDocumentsDescription
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

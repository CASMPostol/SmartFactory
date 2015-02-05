//<summary>
//  Title   : partial class SADZgloszenieTowarDokumentWymagany
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

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  /// <summary>
  /// partial class SADZgloszenieTowarDokumentWymagany
  /// </summary>
  public partial class SADZgloszenieTowarDokumentWymagany: RequiredDocumentsDescription
  {
    /// <summary>
    /// Creates the instanmce of <see cref="SADZgloszenieTowarDokumentWymagany"/>.
    /// </summary>
    /// <param name="position">The position on the list.</param>
    /// <param name="code">The code of the document.</param>
    /// <param name="number">The number (name) of the document.</param>
    /// <param name="notes">The notes - additional definition.</param>
    /// <returns></returns>
    public static SADZgloszenieTowarDokumentWymagany Create( decimal position, string code, string number, string notes )
    {
      return new SADZgloszenieTowarDokumentWymagany()
      {
        PozId = position,
        Kod = code,
        Nr = number,
        Uwagi = notes
      };
    }
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

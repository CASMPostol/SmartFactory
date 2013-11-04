//<summary>
//  Title   : public enum CustomsProcedureCodes 
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


namespace CAS.SmartFactory.Customs.Messages
{
  /// <summary>
  /// public enum CustomsProcedureCodes 
  /// </summary>
  public enum CustomsProcedureCodes 
  {
    /// <summary>
    /// No procedure specified
    /// </summary>
    NoProcedure = 0,
    /// <summary>
    /// Reexport
    /// </summary>
    ReExport = 31,
    /// <summary>
    /// Return to the free circulation
    /// </summary>
    FreeCirculation = 40,
    /// <summary>
    /// The inward processing
    /// </summary>
    InwardProcessing = 51,
    /// <summary>
    /// The customs warehousing procedure
    /// </summary>
    CustomsWarehousingProcedure = 71
  }
}

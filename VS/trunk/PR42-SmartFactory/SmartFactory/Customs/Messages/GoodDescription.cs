//<summary>
//  Title   : class GoodDescription
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
  /// abstract class GoodDescription
  /// </summary>
  public abstract class GoodDescription
  {
    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <returns></returns>
    public abstract string GetDescription();
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetNetMass();
    /// <summary>
    /// Gets the units.
    /// </summary>
    /// <returns></returns>
    public abstract string GetUnits();
    /// <summary>
    /// Gets the PCN tariff code.
    /// </summary>
    /// <returns></returns>
    public abstract string GetPCNTariffCode();
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetGrossMass();
    /// <summary>
    /// Gets the procedure.
    /// </summary>
    /// <returns></returns>
    public abstract string GetProcedure();
    /// <summary>
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public abstract string GetPackage();
    /// <summary>
    /// Gets the total amount invoiced.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetTotalAmountInvoiced();
    /// <summary>
    /// Gets the cartons in kg.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetCartonsInKg();
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetItemNo();
    /// <summary>
    /// Gets the SAD duties.
    /// </summary>
    /// <returns></returns>
    public abstract DutiesDescription[] GetSADDuties();
    /// <summary>
    /// Gets the SAD package.
    /// </summary>
    /// <returns></returns>
    public abstract PackageDescription[] GetSADPackage();
    /// <summary>
    /// Gets the SAD quantity.
    /// </summary>
    /// <returns></returns>
    public abstract QuantityDescription[] GetSADQuantity();
    /// <summary>
    /// Gets the SAD required documents.
    /// </summary>
    /// <returns></returns>
    public abstract RequiredDocumentsDescription[] GetSADRequiredDocuments();
  }
}

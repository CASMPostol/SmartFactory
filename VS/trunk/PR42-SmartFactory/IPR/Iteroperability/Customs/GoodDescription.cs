using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs
{
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

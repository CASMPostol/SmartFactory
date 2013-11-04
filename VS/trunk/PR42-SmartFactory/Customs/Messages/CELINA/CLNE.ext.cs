//<summary>
//  Title   : public partial class CLNE
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

namespace CAS.SmartFactory.Customs.Messages.CELINA.CLNE 
{
  /// <summary>
  /// partial class CLNE
  /// </summary>
  public partial class CLNE : CustomsDocument
  {
    #region CustomsDocument
    /// <summary>
    /// Gets the reference number.
    /// </summary>
    /// <returns></returns>
    public override string GetReferenceNumber()
    {
      if (Przyjecie == null)
        return null;
      return this.Przyjecie.NrWlasny;
    }
    /// <summary>
    /// The name of the root message.
    /// </summary>
    /// <returns></returns>
    public override DocumentType MessageRootName()
    {
      return DocumentType.CLNE;
    }
    /// <summary>
    /// Gets the SAD good.
    /// </summary>
    /// <returns></returns>
    public override GoodDescription[] GetSADGood()
    {
      return null;
    }
    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <returns></returns>
    public override string GetCurrency()
    {
      return String.Empty;
    }
    /// <summary>
    /// Gets the customs debt date.
    /// </summary>
    /// <returns></returns>
    public override DateTime? GetCustomsDebtDate()
    {
      if (Przyjecie == null)
        return null;
      return this.Przyjecie.CzasPrzyjecia;
    }
    /// <summary>
    /// Gets the document number.
    /// </summary>
    /// <returns></returns>
    public override string GetDocumentNumber()
    {
      if (Przyjecie == null)
        return null;
      return this.Przyjecie.NrCelina;
    }
    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <returns></returns>
    public override double? GetExchangeRate()
    {
      return null;
    }
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetGrossMass()
    {
      return null;
    }
    #endregion
  }
}

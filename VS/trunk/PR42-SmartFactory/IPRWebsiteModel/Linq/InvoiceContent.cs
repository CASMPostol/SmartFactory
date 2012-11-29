using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class InvoiceContent
  {
    /// <summary>
    /// Checks if export is possible.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="quantity">The quantity to export.</param>
    /// <returns>
    /// Not empty string if there is a warning.
    /// </returns>
    public string ExportIsPossible(Entities edc, double? quantity )
    {
      if ( !quantity.HasValue )
        return "Valid quantity value must be provided";
      Batch _batch = Batch.FindLookup( edc, InvoiceContent2BatchIndex.Batch0 );
      double _availableQuantity = _batch.FGQuantityAvailable.Value;
      if ( _availableQuantity < quantity.Value )
        return String.Format( m_quantityIsUnavailable, _availableQuantity );
      return String.Empty;
    }
    private const string m_quantityIsUnavailable = "The requested quantity is unavailable. There is only {0} on the stock.";

  }
}

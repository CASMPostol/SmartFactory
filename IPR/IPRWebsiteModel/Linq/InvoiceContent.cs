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
    public string ExportIsPossible( Entities edc, double? quantity )
    {
      if ( !quantity.HasValue )
        return "Valid quantity value must be provided";
      Batch _batch = Batch.FindLookup( edc, InvoiceContent2BatchIndex.Batch0 );
      double _availableQuantity = _batch.FGQuantityAvailable.Value;
      if ( _availableQuantity < quantity.Value )
        return String.Format( m_quantityIsUnavailable, _availableQuantity );
      return String.Empty;
    }
    internal void UpdateExportedDisposals( Entities edc )
    {
      IQueryable<IGrouping<int, Disposal>> _dspslsGroups = from _dsx in this.Disposal
                                                           let _midx = _dsx.Disposal2MaterialIndex.Identyfikator.Value
                                                           group _dsx by _midx;
      foreach ( IGrouping<int, Disposal> _gx in _dspslsGroups )
      {
        Disposal _dsp = _gx.FirstOrDefault<Disposal>();
        if ( _dsp == null )
          continue;
        Material _mtrl = _dsp.Disposal2MaterialIndex;
        decimal _2Add = _mtrl.CalculatedQuantity( this ) - _gx.Sum<Disposal>( v => v.SettledQuantityDec );
        IEnumerable<Disposal> _sorted = from _dx in _gx
                                        orderby _dx.SettledQuantityDec ascending
                                        select _dx;
        foreach ( Disposal _dx in _sorted )
        {
          _dx.Adjust( ref _2Add );
          if (_2Add <= 0)
            break;
        }
        if ( _2Add <= 0 )
          continue;
        _mtrl.AddNewDisposals( edc, DisposalEnum.TobaccoInCigaretess, ref _2Add, this );
      }
    }

    private const string m_quantityIsUnavailable = "The requested quantity is unavailable. There is only {0} on the stock.";

  }
}

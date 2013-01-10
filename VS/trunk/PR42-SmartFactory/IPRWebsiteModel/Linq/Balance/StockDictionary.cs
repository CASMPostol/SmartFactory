using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance
{
  internal class StockDictionary: Dictionary<string, StockDictionary.BalanceStock>
  {
    internal enum StockValueKey
    {
      Balance,
      TobaccoInCigarettesProduction,
      TobaccoInCigarettesWarehouse,
      TobaccoInCutfillerWarehouse,
      TobaccoInWarehouse
    }
    internal class BalanceStock: Dictionary<StockValueKey, decimal>
    {
      internal BalanceStock()
      {
        foreach ( StockValueKey _vkx in Enum.GetValues( typeof( StockValueKey ) ) )
          base[ _vkx ] = 0;
      }
      internal new double this[ StockValueKey index ]
      {
        get { return Convert.ToDouble( base[ index ] ); }
      }
      internal void CalculateBalance( decimal TobaccoInFGCSNotStarted, decimal TobaccoAvailable )
      {
        base[ StockValueKey.TobaccoInCigarettesWarehouse ] = TobaccoInFGCSNotStarted;
        base[ StockValueKey.Balance ] =
          TobaccoAvailable -
          base[ StockValueKey.TobaccoInCigarettesProduction ] -
          base[ StockValueKey.TobaccoInCigarettesWarehouse ] -
          base[ StockValueKey.TobaccoInCutfillerWarehouse ] -
          base[ StockValueKey.TobaccoInWarehouse ];
      }
      internal Dictionary<StockValueKey, decimal> Base { get { return this; } }
    }
    internal BalanceStock GetOrDefault( string batch )
    {
      if ( !this.ContainsKey( batch ) )
        this.Add( batch, new BalanceStock() );
      return this[ batch ];
    }
    internal void Sum( decimal quantity, string batch, StockValueKey _key )
    {
      BalanceStock _bs = GetOrDefault( batch );
      _bs.Base[ _key ] += quantity;
    }
    internal void Sum( double quantity, string batch, StockValueKey _key )
    {
      Sum( quantity, batch, _key );
    }
  }
}

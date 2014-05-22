//<summary>
//  Title   : class StockDictionary
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;

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
    internal void Sum( decimal quantity, string batch, StockValueKey key )
    {
      BalanceStock _bs = GetOrDefault( batch );
      _bs.Base[ key ] += quantity;
    }
    internal void Sum( double quantity, string batch, StockValueKey key )
    {
      Sum( Convert.ToDecimal( quantity ), batch, key );
    }
  }
}

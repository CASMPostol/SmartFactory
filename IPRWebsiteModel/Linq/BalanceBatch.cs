using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  partial class BalanceBatch
  {
    internal static void Create( Entities edc, IGrouping<string, IPR> _grpx, JSOXLib parent, IPR.BalanceStock balanceStock )
    {
      IPR _firsTIPR = _grpx.FirstOrDefault<IPR>();
      BalanceBatch _newBB = new BalanceBatch()
      {
        Balance2JSOXLibraryIndex = parent,
        Batch = _grpx.Key,
        Title = "creating",
        SKU = _firsTIPR == null ? "NA" : _firsTIPR.SKU,
      };
      edc.BalanceBatch.InsertOnSubmit( _newBB );
      _newBB.Update( edc, _grpx, balanceStock );
    }
    internal void Update( Entities edc, IGrouping<string, IPR> grouping, IPR.BalanceStock balanceStock)
    {
      Dictionary<string, IPR> _iprDictionary = grouping.ToDictionary( x => x.DocumentNo );
      List<string> _processed = new List<string>();
      IPR.BalanceTotals _totals = new IPR.BalanceTotals();
      foreach ( BalanceIPR _blncIPRx in this.BalanceIPR )
      {
        if ( _iprDictionary.ContainsKey( _blncIPRx.DocumentNo ) )
        {
          IPR.Balance _newBipr = _blncIPRx.Update();
          _totals.Add( _newBipr );
        }
        else
          edc.BalanceIPR.DeleteOnSubmit( _blncIPRx );
        _processed.Add( _blncIPRx.DocumentNo );
      }
      foreach ( string _dcn in _processed )
        _iprDictionary.Remove( _dcn );
      foreach ( IPR _iprx in _iprDictionary.Values )
      {
        IPR.Balance _newBipr = Linq.BalanceIPR.Create( edc, _iprx, this, this.Balance2JSOXLibraryIndex );
        _totals.Add( _newBipr );
      }
      DustCSNotStarted = _totals[ IPR.ValueKey.DustCSNotStarted ];
      DustCSStarted = _totals[ IPR.ValueKey.DustCSStarted ];
      IPRBook = _totals[ IPR.ValueKey.IPRBook ];
      OveruseCSNotStarted = _totals[ IPR.ValueKey.OveruseCSNotStarted ];
      OveruseCSStarted = _totals[ IPR.ValueKey.OveruseCSStarted ];
      PureTobaccoCSNotStarted = _totals[ IPR.ValueKey.PureTobaccoCSNotStarted ];
      PureTobaccoCSStarted = _totals[ IPR.ValueKey.PureTobaccoCSStarted ];
      SHMentholCSNotStarted = _totals[ IPR.ValueKey.SHMentholCSNotStarted ];
      SHMentholCSStarted = _totals[ IPR.ValueKey.SHMentholCSStarted ];
      SHWasteOveruseCSNotStarted = _totals[ IPR.ValueKey.SHWasteOveruseCSNotStarted ];
      TobaccoAvailable = _totals[ IPR.ValueKey.TobaccoAvailable ];
      TobaccoCSFinished = _totals[ IPR.ValueKey.TobaccoCSFinished ];
      TobaccoEnteredIntoIPR = _totals[ IPR.ValueKey.TobaccoEnteredIntoIPR ];
      TobaccoInFGCSNotStarted = _totals[ IPR.ValueKey.TobaccoInFGCSNotStarted ];
      TobaccoInFGCSStarted = _totals[ IPR.ValueKey.TobaccoInFGCSStarted ];
      TobaccoToBeUsedInTheProduction = _totals[ IPR.ValueKey.TobaccoToBeUsedInTheProduction ];
      TobaccoUsedInTheProduction = _totals[ IPR.ValueKey.TobaccoUsedInTheProduction ];
      WasteCSNotStarted = _totals[ IPR.ValueKey.WasteCSNotStarted ];
      WasteCSStarted = _totals[ IPR.ValueKey.WasteCSStarted ];
      //
      balanceStock.CalculateBalance( _totals.Base[ IPR.ValueKey.TobaccoInFGCSNotStarted ], _totals.Base[ IPR.ValueKey.TobaccoAvailable ] );
      this.Balance = balanceStock[ IPR.StockValueKey.Balance ];
      this.TobaccoInCigarettesProduction = balanceStock[ IPR.StockValueKey.TobaccoInCigarettesProduction ];
      this.TobaccoInCigarettesWarehouse = balanceStock[ IPR.StockValueKey.TobaccoInCigarettesWarehouse ];
      this.TobaccoInCutfillerWarehouse = balanceStock[ IPR.StockValueKey.TobaccoInCutfillerWarehouse ];
      this.TobaccoInWarehouse = balanceStock[ IPR.StockValueKey.TobaccoInWarehouse ];
    }
  }
}

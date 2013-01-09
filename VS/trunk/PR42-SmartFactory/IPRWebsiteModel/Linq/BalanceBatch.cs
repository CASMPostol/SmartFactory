using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  partial class BalanceBatch
  {
    internal void Update( IGrouping<string, IPR> grouping )
    {
      Dictionary<string, IPR> _iprList = grouping.ToDictionary( x => x.DocumentNo );
      List<string> _processed = new List<string>();
      foreach ( BalanceIPR _blncx in this.BalanceIPR )
      {
        if ( _iprList.ContainsKey( _blncx.DocumentNo ) )
        {
          IPR.Balance _blnc = _blncx.Update();
          _processed.Add( _blncx.DocumentNo );
        }
        else
          ;
      }
    }
    internal static void Create( Entities edc, IGrouping<string, IPR> _grpx, JSOXLib parent )
    {
      List<IPR.Balance> _blnces = new List<IPR.Balance>();
      BalanceBatch _newBB = new BalanceBatch()
      {
        Balance2JSOXLibraryIndex = parent,
        Batch = _grpx.Key,
        Title = "creating",
      };
      IPR.BalanceTotals _totals = new IPR.BalanceTotals();
      foreach ( IPR _iprx in _grpx )
      {
        IPR.Balance _newBipr = Linq.BalanceIPR.CreateBalanceIPR( edc, _iprx, _newBB, parent );
        _totals.Sum( _newBipr );
      }
      _newBB.Update( _totals );
    }
    private void Update( IPR.BalanceTotals _balnce )
    {
      DustCSNotStarted = _balnce[ IPR.ValueKey.DustCSNotStarted ];
      DustCSStarted = _balnce[ IPR.ValueKey.DustCSStarted ];
      IPRBook = _balnce[ IPR.ValueKey.IPRBook ];
      OveruseCSNotStarted = _balnce[ IPR.ValueKey.OveruseCSNotStarted ];
      OveruseCSStarted = _balnce[ IPR.ValueKey.OveruseCSStarted ];
      PureTobaccoCSNotStarted = _balnce[ IPR.ValueKey.PureTobaccoCSNotStarted ];
      PureTobaccoCSStarted = _balnce[ IPR.ValueKey.PureTobaccoCSStarted ];
      SHMentholCSNotStarted = _balnce[ IPR.ValueKey.SHMentholCSNotStarted ];
      SHMentholCSStarted = _balnce[ IPR.ValueKey.SHMentholCSStarted ];
      SHWasteOveruseCSNotStarted = _balnce[ IPR.ValueKey.SHWasteOveruseCSNotStarted ];
      TobaccoAvailable = _balnce[ IPR.ValueKey.TobaccoAvailable ];
      TobaccoCSFinished = _balnce[ IPR.ValueKey.TobaccoCSFinished ];
      TobaccoEnteredIntoIPR = _balnce[ IPR.ValueKey.TobaccoEnteredIntoIPR ];
      TobaccoInFGCSNotStarted = _balnce[ IPR.ValueKey.TobaccoInFGCSNotStarted ];
      TobaccoInFGCSStarted = _balnce[ IPR.ValueKey.TobaccoInFGCSStarted ];
      TobaccoToBeUsedInTheProduction = _balnce[ IPR.ValueKey.TobaccoToBeUsedInTheProduction ];
      TobaccoUsedInTheProduction = _balnce[ IPR.ValueKey.TobaccoUsedInTheProduction ];
      WasteCSNotStarted = _balnce[ IPR.ValueKey.WasteCSNotStarted ];
      WasteCSStarted = _balnce[ IPR.ValueKey.WasteCSStarted ];
    }
  }
}

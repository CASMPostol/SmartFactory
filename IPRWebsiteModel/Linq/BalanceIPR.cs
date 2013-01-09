﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class BalanceIPR
  {
    internal IPR.Balance Update()
    {
      if ( this.IPRIndex == null )
        throw new ArgumentNullException( "IPRIndex", "IPRIndex for Balance IPR cannot be null" );
      IPR.Balance _ret = new IPR.Balance( this.IPRIndex );
      Update( _ret );
      return _ret;
    }
    public static IPR.Balance CreateBalanceIPR( Entities edc, IPR _iprAccount, BalanceBatch parent, JSOXLib masterReport )
    {
      IPR.Balance _balnce = new IPR.Balance( _iprAccount );
      BalanceIPR _newItem = new BalanceIPR()
      {
        Balance = -1,
        BalanceBatchIndex = parent,
        BalanceIPR2JSOXIndex = masterReport,
        Batch = _iprAccount.Batch,
        CustomsProcedure = _iprAccount.ClearenceIndex.ClearenceProcedure.ToString(),
        DocumentNo = _iprAccount.DocumentNo,
        InvoiceNo = _iprAccount.InvoiceNo,
        IPRIndex = _iprAccount,
        OGLIntroduction = _iprAccount.DocumentNo,
        SKU = _iprAccount.SKU,
        Title = "Creating",
      };
      edc.BalanceIPR.InsertOnSubmit( _newItem );
      _newItem.Update( _balnce );
      return _balnce;
    }
    private void Update( IPR.Balance _balnce )
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

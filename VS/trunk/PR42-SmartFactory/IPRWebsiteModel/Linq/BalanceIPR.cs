using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class BalanceIPR
  {
    internal static BalanceIPR CreateBalanceIPR( IPR _iprAccount, BalanceBatch parent, JSOXLib masterReport )
    {
      IPR.Balance _balnce = new IPR.Balance( _iprAccount );
      BalanceIPR _ret = new BalanceIPR()
      {
        Balance = -1,
        BalanceBatchIndex = parent,
        BalanceIPR2JSOXIndex = masterReport,
        Batch = _iprAccount.Batch,
        CustomsProcedure = _iprAccount.ClearenceIndex.ClearenceProcedure.ToString(),
        DocumentNo = _iprAccount.DocumentNo,
        DustCSNotStarted = _balnce[ IPR.Balance.ValueKey.DustCSNotStarted ],
        DustCSStarted = _balnce[ IPR.Balance.ValueKey.DustCSStarted ],
        InvoiceNo = _iprAccount.InvoiceNo,
        IPRBook = _balnce[ IPR.Balance.ValueKey.IPRBook ],
        IPRIndex = _iprAccount,
        OGLIntroduction = _iprAccount.DocumentNo,
        OveruseCSNotStarted = _balnce[ IPR.Balance.ValueKey.OveruseCSNotStarted ],
        OveruseCSStarted = _balnce[ IPR.Balance.ValueKey.OveruseCSStarted ],
        PureTobaccoCSNotStarted = _iprAccount.TobaccoNotAllocated + _balnce[ IPR.Balance.ValueKey.PureTobaccoCSNotStarted ],
        PureTobaccoCSStarted = _balnce[ IPR.Balance.ValueKey.PureTobaccoCSStarted ],
        SHMentholCSNotStarted = _balnce[ IPR.Balance.ValueKey.SHMentholCSNotStarted ],
        SHMentholCSStarted = _balnce[ IPR.Balance.ValueKey.SHMentholCSStarted ],
        SHWasteOveruseCSNotStarted = _balnce[ IPR.Balance.ValueKey.SHWasteOveruseCSNotStarted ],
        SKU = _iprAccount.SKU,
        Title = "Creating",
        TobaccoAvailable = _balnce[ IPR.Balance.ValueKey.TobaccoAvailable ],
        TobaccoCSFinished = _balnce[ IPR.Balance.ValueKey.TobaccoCSFinished ],
        TobaccoEnteredIntoIPR = _balnce[ IPR.Balance.ValueKey.TobaccoEnteredIntoIPR ],
        TobaccoInFGCSNotStarted = _balnce[ IPR.Balance.ValueKey.TobaccoInFGCSNotStarted ],
        TobaccoInFGCSStarted = _balnce[ IPR.Balance.ValueKey.TobaccoInFGCSStarted ],
        TobaccoToBeUsedInTheProduction = _balnce[ IPR.Balance.ValueKey.TobaccoToBeUsedInTheProduction],
        TobaccoUsedInTheProduction = _balnce[ IPR.Balance.ValueKey.TobaccoUsedInTheProduction ],
        WasteCSNotStarted = _balnce[ IPR.Balance.ValueKey.WasteCSNotStarted ],
        WasteCSStarted = _balnce[ IPR.Balance.ValueKey.WasteCSStarted ] 

      };
      return _ret;
    }
  }
}

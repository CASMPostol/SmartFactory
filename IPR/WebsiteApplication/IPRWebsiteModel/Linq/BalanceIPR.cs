//<summary>
//  Title   : BalanceIPR
//  System  : Microsoft VisulaStudio 2013 / C#
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

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// BalanceIPR entity
  /// </summary>
  public partial class BalanceIPR
  {
    internal static IPR.Balance Create(Entities edc, IPR _iprAccount, BalanceBatch parent, JSOXLib masterReport)
    {
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
      edc.BalanceIPR.InsertOnSubmit(_newItem);
      return _newItem.Update(edc);
    }
    internal IPR.Balance Update(Entities edc)
    {
      if (this.IPRIndex == null)
        throw new ArgumentNullException("IPRIndex", "IPRIndex for Balance IPR cannot be null");
      IPR.Balance _balnce = new IPR.Balance(edc, this.IPRIndex);
      DustCSNotStarted = _balnce[IPR.ValueKey.DustCSNotStarted];
      DustCSStarted = _balnce[IPR.ValueKey.DustCSStarted];
      IPRBook = _balnce[IPR.ValueKey.IPRBook];
      OveruseCSNotStarted = _balnce[IPR.ValueKey.OveruseCSNotStarted];
      OveruseCSStarted = _balnce[IPR.ValueKey.OveruseCSStarted];
      PureTobaccoCSNotStarted = _balnce[IPR.ValueKey.PureTobaccoCSNotStarted];
      PureTobaccoCSStarted = _balnce[IPR.ValueKey.PureTobaccoCSStarted];
      SHMentholCSNotStarted = _balnce[IPR.ValueKey.SHMentholCSNotStarted];
      SHMentholCSStarted = _balnce[IPR.ValueKey.SHMentholCSStarted];
      SHWasteOveruseCSNotStarted = _balnce[IPR.ValueKey.SHWasteOveruseCSNotStarted];
      TobaccoAvailable = _balnce[IPR.ValueKey.TobaccoAvailable];
      TobaccoCSFinished = _balnce[IPR.ValueKey.TobaccoCSFinished];
      TobaccoEnteredIntoIPR = _balnce[IPR.ValueKey.TobaccoEnteredIntoIPR];
      TobaccoInFGCSNotStarted = _balnce[IPR.ValueKey.TobaccoInFGCSNotStarted];
      TobaccoInFGCSStarted = _balnce[IPR.ValueKey.TobaccoInFGCSStarted];
      TobaccoToBeUsedInTheProduction = _balnce[IPR.ValueKey.TobaccoToBeUsedInTheProduction];
      TobaccoUsedInTheProduction = _balnce[IPR.ValueKey.TobaccoUsedInTheProduction];
      WasteCSNotStarted = _balnce[IPR.ValueKey.WasteCSNotStarted];
      WasteCSStarted = _balnce[IPR.ValueKey.WasteCSStarted];
      TobaccoStarted = _balnce[IPR.ValueKey.TobaccoStarted];
      Balance = this.IPRIndex.TobaccoNotAllocated - _balnce[IPR.ValueKey.TobaccoToBeUsedInTheProduction];
      Title = String.Format("{0}/{1}", this.Batch, this.DocumentNo);
      return _balnce;
    }
  }
}

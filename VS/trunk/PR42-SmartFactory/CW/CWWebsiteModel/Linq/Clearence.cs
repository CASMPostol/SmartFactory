﻿//<summary>
//  Title   : partial class Clearence
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;
using CAS.SmartFactory.CW.WebsiteModel;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// partial class Clearence
  /// </summary>
  public partial class Clearence
  {
    /// <summary>
    /// Creatas the clearence.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <returns></returns>
    public static Clearence CreataClearence(Entities entities, string procedure, ClearenceProcedure procedureCode)
    {
      Clearence _newClearence = CreateClearance(procedure, procedureCode);
      entities.Clearence.InsertOnSubmit(_newClearence);
      _newClearence.UpdateTitle(entities);
      entities.SubmitChanges();
      _newClearence.UpdateTitle(entities);
      return _newClearence;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    /// <param name="entities">The autogenerated <see cref="Microsoft.SharePoint.Linq.DataContext"/> object.</param>
    public void UpdateTitle(Entities entities)
    {
      string _quantity = String.Empty;
      //IQueryable<CustomsWarehouseDisposal> _Dspsls = from _Dspx in entities.CustomsWarehouseDisposal where _Dspx == this.Id select {ssss = _d.s }
      //if ( this.Disposal.Any() )
      //  _quantity = this.Disposal.Sum<Disposal>( x => x.SettledQuantity.Value ).ToString( "F2" );
      //else
      //  _quantity = " --- ";
      string _ClearanceTitleFormat = Settings.GetParameter(entities, SettingsEntry.ClearanceTitleFormatCW);
      Title = String.Format(_ClearanceTitleFormat,
                             this.ProcedureCode, //0
                             ClearenceProcedure.GetValueOrDefault(Linq.ClearenceProcedure.Invalid).Convert2String(), //1
                             ReferenceNumber.NotAvailable(), //2
                             Id.GetValueOrDefault(-999)); //3
    }
    public string SADTemplateDocumentNameFileName(Entities entities)
    {
      return Settings.SADTemplateDocumentNameFileName(entities, this.Id.Value);
    }
    internal void FinishClearThroughCustoms(Entities entities, SADGood sadGood)
    {
      SADDocumentType sadDocument = Clearence2SadGoodID.SADDocumentIndex;
      DocumentNo = sadDocument.DocumentNumber;
      ReferenceNumber = sadDocument.ReferenceNumber;
      Status = true;
      foreach (CustomsWarehouseDisposal _cwdx in this.CustomsWarehouseDisposal)
        _cwdx.FinishClearThroughCustoms(sadGood);
      UpdateTitle(entities);
    }

    #region private
    private static Clearence CreateClearance(string code, ClearenceProcedure procedure)
    {
      Clearence _newClearence = new Clearence()
      {
        DocumentNo = String.Empty.NotAvailable(),
        ProcedureCode = code,
        ReferenceNumber = String.Empty.NotAvailable(),
        Status = false,
        ClearenceProcedure = procedure
      };
      return _newClearence;
    }
    #endregion

  }
}
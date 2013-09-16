//<summary>
//  Title   : Name of Application
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

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Entity partial class of CustomsWarehouseDisposal.
  /// </summary>
  public partial class CustomsWarehouseDisposal
  {
    /// <summary>
    /// CustomsWarehouseDisposal data obtained from xml message.
    /// </summary>
    public struct XmlData
    {
      /// <summary>
      /// Additional quantity declared to dispose
      /// </summary>
      public double AdditionalQuantity ;
      /// <summary>
      /// The declared quantity
      /// </summary>
      public double DeclaredQuantity;
      /// <summary>
      /// The sku description
      /// </summary>
      public string SKUDescription; 
    }
    /// <summary>
    /// Creates the specified <see cref="CustomsWarehouseDisposal"/>.
    /// </summary>
    /// <param name="parent">The parent <see cref="DisposalRequestLib"/>.</param>
    /// <param name="xmlData">The XML data.</param>
    /// <param name="account">The associated account.</param>
    /// <returns>A new <see cref="CustomsWarehouseDisposal "/>. </returns>
    public static CustomsWarehouseDisposal Create(DisposalRequestLib parent, XmlData xmlData, Linq.CustomsWarehouse account)
    {
      return new CustomsWarehouseDisposal()
        {
          Batch = account.Batch,
          CWL_CWDisposal2DisposalRequestLibraryID = parent,
          CW_AddedKg = xmlData.AdditionalQuantity,
          CW_DeclaredToClear = xmlData.AdditionalQuantity, // ??
          CW_QuantityToClear = xmlData.DeclaredQuantity, //TODO duplicated
          CW_DeclaredNetMass = xmlData.DeclaredQuantity,

          Currency = account.Currency,
          CustomsStatus = Linq.CustomsStatus.NotStarted,
          CWL_CWDisposal2PCNTID = account.CWL_CW2PCNID,
          CWC_EntryDate = DateTime.Today, //TODO duplcated creation date
          Grade = account.Grade,
          SKU = account.SKU,
          TobaccoName = account.TobaccoName,
          SKUDescription = xmlData.SKUDescription,
          Title = "xreating", //TODO
          CW_PackageToClear = 0,// count
          CW_SettledGrossMass = 0, //count
          CW_SettledNetMass = 0,//count
          CW_PackageAvailable = 0,//TODO duplicated
          CW_QuantityAvailable = 0,//TODO duplicated
        };
    }

  }
}

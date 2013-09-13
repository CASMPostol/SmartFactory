using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  public partial class CustomsWarehouseDisposal
  {
    private static double addedKg;
    private static double declaredNetMass;
    private static string skuDescription; //xml
    public static CustomsWarehouseDisposal Create(DisposalRequestLib parent, Linq.CustomsWarehouse account)
    {
      return new CustomsWarehouseDisposal()
        {
          Batch = account.Batch,
          CWL_CWDisposal2DisposalRequestLibraryID = parent,
          CW_AddedKg = addedKg,
          CW_DeclaredToClear = addedKg, // ??
          CW_QuantityToClear = declaredNetMass, //TODO duplicated
          CW_DeclaredNetMass = declaredNetMass,

          Currency = account.Currency,
          CustomsStatus = Linq.CustomsStatus.NotStarted,
          CWL_CWDisposal2PCNTID = account.CWL_CW2PCNID,
          CWC_EntryDate = DateTime.Today, //TODO duplcated creation date
          Grade = account.Grade,
          SKU = account.SKU,
          TobaccoName = account.TobaccoName,
          SKUDescription = skuDescription,
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

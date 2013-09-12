using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  public partial class CustomsWarehouseDisposal
  {
    private static string batch;
    public static CustomsWarehouseDisposal Create(DisposalRequestLib parent)
    {
      return new CustomsWarehouseDisposal()
        {
          Batch = batch,
           CWL_CWDisposal2DisposalRequestLibraryID = parent,
            
        };
    }

  }
}

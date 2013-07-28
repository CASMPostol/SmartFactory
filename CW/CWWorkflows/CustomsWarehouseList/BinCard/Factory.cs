using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.BinCard;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.BinCard
{
  internal static class Factory
  {
    internal static BinCardContentType CreateContent( BinCard item )
    {
      BinCardContentType _ret = new BinCardContentType()
      {
        //TODO create
      };
      return _ret;
    }
  }
}

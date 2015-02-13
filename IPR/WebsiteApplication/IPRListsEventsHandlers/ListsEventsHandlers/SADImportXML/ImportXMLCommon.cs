//<summary>
//  Title   : Import XML message Common Fuctionality
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection;
using CAS.SmartFactory.xml.Customs;
using System;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  internal static class ImportXMLCommon
  {

    internal static CAS.SmartFactory.Customs.Account.CommonAccountData.MessageType Convert2MessageType( CustomsDocument.DocumentType type )
    {
      AccountData.MessageType _ret = default( AccountData.MessageType );
      switch ( type )
      {
        case CustomsDocument.DocumentType.SAD:
          _ret = AccountData.MessageType.SAD;
          break;
        case CustomsDocument.DocumentType.PZC:
          _ret = AccountData.MessageType.SAD;
          break;
        case CustomsDocument.DocumentType.IE529:
        case CustomsDocument.DocumentType.CLNE:
        default:
          throw new ArgumentException( "Out of range value for CustomsDocument.DocumentType argument in Convert2MessageType ", "type" );
      }
      return _ret;
    }

  }
}

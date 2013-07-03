using System;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Account;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  internal static class ImportXMLCommon
  {
    internal static AccountData.MessageType Convert2MessageType( CustomsDocument.DocumentType type )
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

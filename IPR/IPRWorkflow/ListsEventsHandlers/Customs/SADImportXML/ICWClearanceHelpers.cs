using System;
namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  interface ICWClearanceHelpers
  {
    void CreateCWAccount( CAS.SmartFactory.IPR.WebsiteModel.Linq.Entities entities, CAS.SmartFactory.IPR.WebsiteModel.Linq.Clearence clearence, CAS.SmartFactory.xml.Customs.CustomsDocument.DocumentType _messageType, out string _comments, System.Collections.Generic.List<CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException> warnings );
  }
}

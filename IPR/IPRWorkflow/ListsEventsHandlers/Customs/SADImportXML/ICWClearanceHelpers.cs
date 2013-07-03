using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  interface ICWClearanceHelpers
  {
    void CreateCWAccount( Entities entities, Clearence clearence, CustomsDocument.DocumentType _messageType, out string _comments, List<InputDataValidationException> warnings );
  }
}

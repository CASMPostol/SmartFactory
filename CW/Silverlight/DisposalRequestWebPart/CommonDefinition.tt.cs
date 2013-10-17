//<summary>
//  Title   : partial class CommonDefinition
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

using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  /// <summary>
  /// CommonDefinition 
  /// </summary>
  internal partial class CommonDefinition
  {
    internal static CamlQuery GetCAMLSelectedID( int value, string fieldName, string camlType )
    {
      return new CamlQuery() { ViewXml = string.Format( CAMLQueryString, value, fieldName, camlType ) };
    }
    internal static CamlQuery GetCAMLSelectedID( string value, string fieldName, string camlType )
    {
      return new CamlQuery() { ViewXml = string.Format( CAMLQueryString, value, fieldName, camlType ) };
    }
    internal static string CAMLTypeNumber = "Number";
    internal static string CAMLTypeText = "Text";
    internal static string FieldID = "ID";
    internal static string FieldCWDisposal2DisposalRequestLibraryID = "CWL_CWDisposal2DisposalRequestLibraryID";
    internal static string CustomsWarehouseTitle = "Customs Warehouse";
    internal static string FieldBatch = "Batch";
    private static string CAMLQueryString = @"
      <View>
        <Query>
          <Where>
            <Eq>
              <FieldRef Name='{1}'></FieldRef>
              <Value Type='{2}'>{0}</Value>
            </Eq>
          </Where>
        </Query>
      </View>";
  }
}

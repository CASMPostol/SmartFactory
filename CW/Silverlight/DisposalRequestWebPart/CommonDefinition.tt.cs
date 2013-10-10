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
    internal static CamlQuery GetCAMLSelectedID( int id, string fieldName )
    {
      return new CamlQuery() { ViewXml = string.Format( CAMLSelectedID, id, fieldName ) };
    }
    internal static string FieldID = "ID";
    internal static string FieldCWDisposal2DisposalRequestLibraryID = "CWL_CWDisposal2DisposalRequestLibraryID";
    private static string CAMLSelectedID = @"
      <View>
        <Query>
          <Where>
            <Eq>
              <FieldRef Name='{1}'></FieldRef>
              <Value Type='Number'>{0}</Value>
            </Eq>
          </Where>
        </Query>
      </View>";
  }
}

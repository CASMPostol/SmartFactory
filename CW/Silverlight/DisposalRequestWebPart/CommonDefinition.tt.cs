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
    internal static CamlQuery GetCAMLSelectedID( int id )
    {
      return new CamlQuery () { ViewXml = string.Format( CAMLSelectedID, id ) };
    }
    private static string CAMLSelectedID = @"
      <View>
        <Query>
          <Eq>
            <FieldRef Name='ID'></FieldRef>
            <Value Type='Counter'>{0}</Value>
          </Eq>
        </Query>
      </View>";
  }
}
